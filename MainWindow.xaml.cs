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
using System.ComponentModel;
using Hardcodet.Wpf.TaskbarNotification;

namespace DateReminder
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow? Instance { get; set; }

        private List<Reminder> reminders;

        private int maxRemindersInContainer;
        private int maxPagesIndex;
        private int pageIndex = 0;

        public static bool IsActive { get; set; }

        private int maxAttempts = 20;
        private int attempts = 0;

        private bool NeedsUpdate {  get; set; }

        private System.Windows.Threading.DispatcherTimer updateDispatcher = new System.Windows.Threading.DispatcherTimer();


        public MainWindow()
        {
            Instance = this;
            InitializeComponent();
            reminders = new List<Reminder>();
            IsActive = true;
            updateDispatcher.Tick += CheckUpdate;
            updateDispatcher.Interval = TimeSpan.FromMilliseconds(80);
        }
        public async void ReadDatabase()
        {
            Console.WriteLine("ReadDatabase");
            NeedsUpdate = false;
            updateDispatcher.Start();
            try
            {
                Console.WriteLine($"Try ReadDatabase, attempts: {attempts}");
                await Task.Delay(200);
                maxRemindersInContainer = (int)((MainBorder.ActualHeight - (SearchTextBox.ActualHeight + NewReminderButton.ActualHeight + 30)) / (96.45f)) - 1;

                using (var context = ReminderDBContext.GetContext())
                {
                    if (context.Reminders.Where(p => p.User.Id == CoreWindow.Instance.ActiveUser.Id).Count() == 0)
                    {
                        return;
                    }
                    maxPagesIndex = (int)Math.Ceiling((double)context.Reminders.Where(p => p.User.Id == CoreWindow.Instance.ActiveUser.Id).Count() / maxRemindersInContainer) - 1;
                    if (pageIndex > maxPagesIndex)
                    {
                        pageIndex = maxPagesIndex;
                    }

                    reminders = await context.Reminders.Where(p => p.User.Id == CoreWindow.Instance.ActiveUser.Id).Skip(pageIndex * maxRemindersInContainer).Take(maxRemindersInContainer).ToListAsync();
                }

                if (reminders.Count != 0)
                {
                    Console.WriteLine("Assign reminders");
                    NeedsUpdate = true;
                }
                attempts = 0;
            }
            catch(Exception ex)
            {
                attempts++;
                Console.WriteLine(ex.ToString());
                if (attempts < maxAttempts)
                {
                    ReadDatabase();
                }
                else
                {
                    Console.WriteLine($"Couldn't connect to the database after {maxAttempts} tries");
                }
            }
        }
        public async void CheckUpdate(object sender, EventArgs e)
        {
            if (SingletonWindow<MainWindow>.Instance.WindowInstance.NeedsUpdate)
            {
                SingletonWindow<MainWindow>.Instance.WindowInstance.UpdateWindow();
                updateDispatcher.Stop();
            }
        }
        public void UpdateWindow()
        {
            Console.WriteLine("UpdateWindow");
            HideConnectionText();
            RemindersListView.ItemsSource = reminders;
            UpdatePageText();
            NeedsUpdate = false;
        }
        private void HideConnectionText()
        {
            ConnectingLabel.Visibility = Visibility.Hidden;
        }
        private async void NewReminder_Click(object sender, RoutedEventArgs e)
        {
            new UpdateReminderWindow().ShowDialog();
        }
        private async void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox searchTextBox = sender as TextBox;
            IEnumerable<Reminder> filteredReminders;
            using (var context = ReminderDBContext.GetContext())
            {
                filteredReminders = await context.Reminders.Where(c => c.User.Id == CoreWindow.Instance.ActiveUser.Id && c.Title.ToLower().Contains(searchTextBox.Text.ToLower())).ToListAsync();
            }
            RemindersListView.ItemsSource = filteredReminders;
        }

        protected override void OnClosed(EventArgs e)
        {
            IsActive = false;
            ConnectingLabel.Visibility = Visibility.Visible;
            base.OnClosed(e);
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
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
