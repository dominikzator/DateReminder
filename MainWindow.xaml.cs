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
using Microsoft.EntityFrameworkCore;
using DateReminder.Configurations;

namespace DateReminder
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ToastsManager toastsManager;
        private List<Reminder> reminders;

        private User _activeUser;
        public MainWindow(User activeUser)
        {
            InitializeComponent();
            reminders = new List<Reminder>();
            toastsManager = ToastsManager.Instance;
            _activeUser = activeUser;
            ReadDatabase();
        }

        private async void ReadDatabase()
        {
            using (var context = ReminderDBContext.GetContext())
            {
                await Console.Out.WriteLineAsync("context.Reminders.Count(): " + context.Reminders.Count());
                reminders = await context.Reminders.Where(p => p.User.Id == _activeUser.Id).ToListAsync();
            }

            if (reminders != null)
            {
                RemindersListView.ItemsSource = reminders;
            }
        }

        private async void Fire_Notification(object sender, RoutedEventArgs e)
        {
            FireManyRandomToastsDifferentInterval();
            //FireTooMuchToastsAtOneTime();
        }
        private async void NewReminder_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("NewReminder_Click");
        }
        private async void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox searchTextBox = sender as TextBox;
            IEnumerable<Reminder> filteredReminders;
            using (var context = ReminderDBContext.GetContext())
            {
                filteredReminders = await context.Reminders.Where(c => c.User.Id == _activeUser.Id && c.Title.ToLower().Contains(searchTextBox.Text.ToLower())).ToListAsync();
            }
            RemindersListView.ItemsSource = filteredReminders;
        }
        private void ContactsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Console.WriteLine("ContactsListView_SelectionChanged");
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
        }

    }
}
