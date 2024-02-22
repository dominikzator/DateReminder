using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

        public static bool trayInitialized;

        public ToastsManager()
        {
            MaxToastsOnScreen = (int)(System.Windows.SystemParameters.PrimaryScreenHeight / (ToastWindowHeight + ToastMargin));
            InitializeTimer();
        }

        private void InitializeTimer()
        {
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += SynchronizeReminders;
            dispatcherTimer.Interval = TimeSpan.FromHours(1);
            dispatcherTimer.Start();
        }
        public async void SynchronizeReminders(object sender, EventArgs e)
        {
            SynchronizeRemindersWithDelay(sender, e);
        }
        public async void SynchronizeRemindersWithDelay(object sender = null, EventArgs e = null, float delayInSeconds = 0f)
        {
            await Task.Delay((int)(delayInSeconds * 1000));
            Console.WriteLine("Tick");
            Console.WriteLine("DateTime.Now: " + DateTime.Now);
            using (var context = new ReminderDBContext())
            {
                if(context.Reminders.Where(p => p.UserId == CoreWindow.Instance.ActiveUser.Id).Count() == 0)
                {
                    Console.WriteLine("No reminders for current user");
                    return;
                }
                //Try rescedule cyclic reminders to the year
                foreach (var reminder in context.Reminders.Where(p => p.UserId == CoreWindow.Instance.ActiveUser.Id && p.Type != 0))
                {
                    while (reminder.TargetDate < DateTime.Now && (reminder.TargetDate.Year != DateTime.Now.Year || reminder.TargetDate.Month != DateTime.Now.Month || reminder.TargetDate.Day != DateTime.Now.Day))
                    {
                        TryRescheduleReminder(reminder);
                    }
                }
                await context.SaveChangesAsync();
                if (MainWindow.IsActive)
                {
                    SingletonWindow<MainWindow>.Instance.WindowInstance.ReadDatabase();
                }
                //Fire corresponding Reminders Notifications
                foreach (var reminder in context.Reminders.Where(p => p.UserId == CoreWindow.Instance.ActiveUser.Id && !p.Reminded))
                {
                    if (DateTime.Now >= reminder.TargetDate.AddSeconds(-reminder.SecondsToNotify) && DateTime.Now <= reminder.TargetDate.AddDays(1))
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
        private void TryRescheduleReminder(Reminder reminder)
        {
            Console.WriteLine($"RESCHEDULE Reminder, title: {reminder.Title}, targetdate: {reminder.TargetDate.ToShortDateString()}, IsCyclic: {reminder.Type}, Reminded: {reminder.Reminded}");
            switch (reminder.Type)
            {
                case Reminder.ReminderType.WEEKLY:
                    {
                        reminder.TargetDate = reminder.TargetDate.AddDays(7);
                        reminder.Reminded = false;
                        break;
                    }
                case Reminder.ReminderType.MONTHLY:
                    {
                        reminder.TargetDate = reminder.TargetDate.AddMonths(1);
                        reminder.Reminded = false;
                        break;
                    }
                case Reminder.ReminderType.ANNUAL:
                    {
                        reminder.TargetDate = new DateTime(reminder.TargetDate.Year + 1, reminder.TargetDate.Month, reminder.TargetDate.Day);
                        reminder.Reminded = false;
                        break;
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
