using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DateReminder
{
    public class ToastsManager
    {
        private static ToastsManager _instance;

        public static ToastsManager Instance
        {
            get
            {
                if(_instance == null )
                {
                    _instance = new ToastsManager();
                }
                return _instance;
            }
            private set => _instance = value;
        }

        private const int ToastWindowHeight = 60;
        private const int ToastMargin = 5;

        public int MaxToastsOnScreen;
        public int ToastsOpened, ToastsClosed;

        public List<ToastWindow> Toasts = new List<ToastWindow>();
        public List<ToastWindow> PendingToasts = new List<ToastWindow>();

        public Dictionary<int, bool> ToastsIdsAlreadyShown = new Dictionary<int, bool>();

        public ToastsManager()
        {
            MaxToastsOnScreen = (int)(System.Windows.SystemParameters.PrimaryScreenHeight / (ToastWindowHeight + ToastMargin));
            InitializeTimer();
        }

        private void InitializeTimer()
        {
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 5000);
            dispatcherTimer.Start();
        }
        private void Tick(object sender, EventArgs e)
        {
            Console.WriteLine("Tick");
            Console.WriteLine("DateTime.Now: " + DateTime.Now);
            using (var context = new ReminderDBContext())
            {
                foreach (var reminder in context.Reminders.Where(p => p.UserId == MainWindow.ActiveUser.Id))
                {
                    if(DateTime.Now >= reminder.TargetDate.AddSeconds(-reminder.SecondsToNotify) && DateTime.Now <= reminder.TargetDate.AddDays(1))
                    {
                        int daysToEvent = reminder.TargetDate.Subtract(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)).Days;
                        string dayString = daysToEvent == 1 ? "day" : "days";
                        string titleMessage = (daysToEvent == 0) ? $"Today is: {reminder.Title}" : $"Upcoming event in {daysToEvent} {dayString}: {reminder.Title}";
                        Console.WriteLine("MATCHING reminder.Title: " + reminder.Title);
                        OpenToast(reminder, titleMessage, $"Target date: {reminder.TargetDate.Year}-{reminder.TargetDate.Month}-{reminder.TargetDate.Day}");
                    }
                }
            }
        }
        private async Task OpenToast(Reminder reminder, string toastTitle, string toastDescription)
        {
            ToastWindow toast = new ToastWindow(reminder, toastTitle, toastDescription);
            ToastsManager.Instance.AddToast(toast);
        }

        public void AddToast(ToastWindow toastWindow)
        {
            if(ToastsIdsAlreadyShown.ContainsKey(toastWindow.Reminder.Id) && ToastsIdsAlreadyShown[toastWindow.Reminder.Id])
            {
                return;
            }
            if(Toasts.Count >= MaxToastsOnScreen)
            {
                PendingToasts.Add(toastWindow);
                return;
            }
            Toasts.Add(toastWindow);
            toastWindow.Show();
            PositionToast(toastWindow);
            ToastsIdsAlreadyShown.Add(toastWindow.Reminder.Id, true);
        }
        public void RemoveToast(ToastWindow toastWindow)
        {
            Toasts.Remove(toastWindow);
            TryAddPending();
            RefreshToasts();
        }
        private void TryAddPending()
        {
            while (PendingToasts.Count > 0 && Toasts.Count < MaxToastsOnScreen)
            {
                var firstPendingToast = PendingToasts[0];
                AddToast(firstPendingToast);
                PendingToasts.RemoveAt(0);
            }
        }
        private void PositionToast(ToastWindow toastWindow)
        {
            toastWindow.Top = Toasts.IndexOf(toastWindow) * ToastWindowHeight + Toasts.IndexOf(toastWindow) * ToastMargin;
        }

        private void RefreshToasts()
        {
            foreach (ToastWindow toastWindow in Toasts)
            {
                PositionToast(toastWindow);
            }
        }
    }
}
