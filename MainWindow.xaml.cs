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
        public static MainWindow Instance { get; set; }
        public static User ActiveUser { get; private set; }

        private ToastsManager toastsManager;
        private List<Reminder> reminders;

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
            using (var context = ReminderDBContext.GetContext())
            {
                reminders = await context.Reminders.Where(p => p.User.Id == ActiveUser.Id).ToListAsync();
            }

            if (reminders != null)
            {
                RemindersListView.ItemsSource = reminders;
            }
        }
        private async void NewReminder_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("NewReminder_Click");

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
        private void ContactsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Console.WriteLine("ContactsListView_SelectionChanged");
        }
    }
}
