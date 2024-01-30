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
using Microsoft.Extensions.DependencyInjection;
using Autofac;

namespace DateReminder
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SoundPlayer player;

        private ToastsManager toastsManager;
        public MainWindow()
        {
            InitializeComponent();
            toastsManager = ToastsManager.Instance;

            InitializeToastSound();

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
        private void InitializeToastSound()
        {
            player = new SoundPlayer(Properties.Resources.popSound);
            player.Load();
        }
        private async void Fire_Notification(object sender, RoutedEventArgs e)
        {
            FireManyRandomToastsDifferentInterval();
            //FireTooMuchToastsAtOneTime();
        }
        private async Task FireTooMuchToastsAtOneTime()
        {
            for (int i = 0; i < 40; i++)
            {
                OpenToastWithDelay(3f, $"Title {i}", $"Description {i}");
            }
        }
        private async Task FireManyRandomToastsDifferentInterval()
        {
            int numberOfToasts = 15;

            for (int i = 0; i < numberOfToasts; i++)
            {
                var random = new Random();
                int randomInterval = random.Next(0, 5);
                OpenToastWithDelay(randomInterval, $"Title {i}", $"Description {i}");
            }
        }

        private async Task OpenToastWithDelay(double delayInSeconds, string toastTitle, string toastDescription)
        {
            await Task.Delay((int)(delayInSeconds * 1000));
            ToastWindow toast = new ToastWindow(toastTitle, toastDescription);
            ToastsManager.Instance.AddToast(toast);
            player.Play();
        }

    }
}
