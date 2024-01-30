using System.Windows;

namespace DateReminder
{
    /// <summary>
    /// Interaction logic for ToastWindow.xaml
    /// </summary>
    public partial class ToastWindow : Window
    {
        private static float lifeTimeInSeconds = 10f;

        private float timeElapsed;
        public ToastWindow()
        {
            InitializeToastWindow();
        }
        public ToastWindow(string title, string message)
        {
            InitializeToastWindow();
            ToastTitle.Content = title;
            ToastDescription.Content = message;
        }
        protected override void OnActivated(EventArgs e)
        {
            HandleLifeTime();
            InitializeTimer();
        }
        private void InitializeToastWindow()
        {
            InitializeComponent();
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
            //Console.WriteLine("Tick");
        }
        private async Task HandleLifeTime()
        {
            Console.WriteLine("Started Lifetime");
            await Task.Delay((int)(lifeTimeInSeconds * 1000f));
            Console.WriteLine("Ended Lifetime");
            Close();
        }
        private void OnToastWindowLoaded(object sender, EventArgs e)
        {
            Position();
        }

        private void Position()
        {
            double ScreenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double ScreenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;

            double widthMarginFactor = System.Windows.SystemParameters.PrimaryScreenWidth / 100;
            double heightMarginFactor = System.Windows.SystemParameters.PrimaryScreenWidth / 100;

            Left = ScreenWidth - this.Width;
            Top = 0;
        }
        protected override void OnClosed(EventArgs e)
        {
            ToastsManager.Instance.RemoveToast(this);
        }
    }
}
