using System.Media;
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

        private SoundPlayer player;

        public ToastWindow()
        {
            InitializeToastWindow();
            InitializeToastSound();
        }

        private void InitializeToastSound()
        {
            player = new SoundPlayer(Properties.Resources.popSound);
            player.Load();
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
            ToastsManager.Instance.ToastsOpened++;
            if(player == null)
            {
                InitializeToastSound();
            }
            player.Play();

            //Console.WriteLine("ToastsManager.Instance.ToastsOpened: " + ToastsManager.Instance.ToastsOpened);
            //Console.WriteLine("ToastsManager.Instance.ToastsClosed: " + ToastsManager.Instance.ToastsClosed);
        }
        private void InitializeToastWindow()
        {
            InitializeComponent();
        }
        private void Tick(object sender, EventArgs e)
        {
            //Console.WriteLine("Tick");
        }
        private async Task HandleLifeTime()
        {
            await Task.Delay((int)(lifeTimeInSeconds * 1000f));
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
            ToastsManager.Instance.ToastsClosed++;
            //Console.WriteLine("ToastsManager.Instance.ToastsOpened: " + ToastsManager.Instance.ToastsOpened);
            //Console.WriteLine("ToastsManager.Instance.ToastsClosed: " + ToastsManager.Instance.ToastsClosed);
        }
    }
}
