using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Azure;
using System.IO;
using System.Media;

namespace DateReminder
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SoundPlayer player;
        public MainWindow()
        {
            InitializeComponent();
            player = new SoundPlayer(Properties.Resources.popSound);
            player.Load();

            using (var context = new ReminderDBContext())
            {
                //var testingSettings = new UserSettings
                //{
                //    TimeToElapse = default,
                //    TimeToNotify = default
                //};
                //var testingUser = new User
                //{
                //    UserName = "Heniu",
                //    Password = "Heniahaslo987",
                //    UserSettingsId = 1
                //};
                //var testingReminder = new Reminder
                //{
                //    Priority = 4,
                //    TargetDate = DateTime.Now,
                //    UserId = 4,
                //};
                //context.Add(testingReminder);
                //context.SaveChanges();
            }
        }

        private async void Fire_Notification(object sender, RoutedEventArgs e)
        {
            await OpenToastWithDelay(5);
        }

        private async UniTask OpenToastWithDelay(double delayInSeconds)
        {
            await Task.Delay((int)(delayInSeconds * 1000));
            ToastWindow toast = new ToastWindow("You have a new Reminder", "Urodziny Sylwii.");
            toast.Show();
            player.Play();
        }

    }
}
