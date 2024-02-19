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
using Org.BouncyCastle.Crypto;

namespace DateReminder
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance { get; set; }
        public static User ActiveUser { get; private set; }

        private ToastsManager toastsManager;
        private List<Reminder> reminders;

        private int maxRemindersInContainer;
        private int maxPagesIndex;
        private int pageIndex = 0;

        public MainWindow(User activeUser)
        {
            Instance = this;
            InitializeComponent();
            reminders = new List<Reminder>();
            toastsManager = ToastsManager.Instance;
            ActiveUser = activeUser;
            ReadDatabase();
        }

        public static MainWindow GetMainWindow(User loggedUser)
        {
            return Instance != null ? Instance : new MainWindow(loggedUser);
        }

        public async void ReadDatabase()
        {
            await Task.Delay(200);
            maxRemindersInContainer = (int)((MainBorder.ActualHeight - (SearchTextBox.ActualHeight + NewReminderButton.ActualHeight + 30))/ (76.5f)) - 1;

            using (var context = ReminderDBContext.GetContext())
            {
                maxPagesIndex = (int)Math.Ceiling((double)context.Reminders.Where(p => p.User.Id == ActiveUser.Id).Count() / maxRemindersInContainer) - 1;
                if(pageIndex > maxPagesIndex)
                {
                    pageIndex = maxPagesIndex;
                }
                reminders = await context.Reminders.Where(p => p.User.Id == ActiveUser.Id).Skip(pageIndex * maxRemindersInContainer).Take(maxRemindersInContainer).ToListAsync();
            }

            if (reminders != null)
            {
                RemindersListView.ItemsSource = reminders;
            }
            UpdatePageText();
        }
        private async void NewReminder_Click(object sender, RoutedEventArgs e)
        {
            new UpdateReminderWindow(ActiveUser).ShowDialog();
        }
        private async void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox searchTextBox = sender as TextBox;
            IEnumerable<Reminder> filteredReminders;
            using (var context = ReminderDBContext.GetContext())
            {
                filteredReminders = await context.Reminders.Where(c => c.User.Id == ActiveUser.Id && c.Title.ToLower().Contains(searchTextBox.Text.ToLower())).ToListAsync();
            }
            RemindersListView.ItemsSource = filteredReminders;
        }

        private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (--pageIndex < 0)
            {
                pageIndex = 0;
            }
            else
            {
                ReadDatabase();
            }
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (++pageIndex > maxPagesIndex)
            {
                pageIndex = maxPagesIndex;
            }
            else
            {
                ReadDatabase();
            }
        }
        private void UpdatePageText()
        {
            PageLabel.Content = $"Page {pageIndex + 1}";
        }
    }
}
