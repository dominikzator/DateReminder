using System;
using System.Collections.Generic;
using System.Linq;
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
        }

        public void AddToast(ToastWindow toastWindow)
        {
            if(Toasts.Count >= MaxToastsOnScreen)
            {
                PendingToasts.Add(toastWindow);
                return;
            }
            Toasts.Add(toastWindow);
            toastWindow.Show();
            PositionToast(toastWindow);
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
