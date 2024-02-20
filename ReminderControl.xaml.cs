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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DateReminder
{
    /// <summary>
    /// Interaction logic for ReminderControl.xaml
    /// </summary>
    public partial class ReminderControl : UserControl
    {
        public Reminder Reminder
        {
            get { return (Reminder)GetValue(ReminderProperty); }
            set { SetValue(ReminderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Contact.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ReminderProperty =
            DependencyProperty.Register("Reminder", typeof(Reminder), typeof(ReminderControl), new PropertyMetadata(
                new Reminder() 
                { 
                    Priority = 1, 
                    Title = "This is Reminder Title",
                    TargetDate = DateTime.MinValue, 
                    UserId = 1,
                    SecondsToNotify = UserSettings.GetDefaultSecondsToNotify(),
                }, SetText));

        private static void SetText(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ReminderControl control = d as ReminderControl;

            if (control != null)
            {
                control.TitleText.Text = (e.NewValue as Reminder).Title;
                control.TargetDateText.Text = (e.NewValue as Reminder).TargetDate.ToShortDateString();
                string additionText = (e.NewValue as Reminder).IsCyclic ? "Yes" : "No";
                control.IsCyclicText.Text = "Is cyclic: " + additionText;
                string remindedText = (e.NewValue as Reminder).Reminded ? "Yes" : "No";
                control.RemindedText.Text = "Marked as Reminded: " + remindedText;
            }
        }
        public ReminderControl()
        {
            InitializeComponent();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine($"DeleteButton_Click {Reminder.Title}");
            ConfirmationWindow deleteWindow =
            new ConfirmationWindow($"Deleting {Reminder.Title}...", $"Delete Reminder: {Reminder.Title}?", async () =>
            {
                using (var context = ReminderDBContext.GetContext())
                {
                    context.Remove(Reminder);
                    await context.SaveChangesAsync();
                    MainWindow.Instance.ReadDatabase();
                }
            });
            deleteWindow.ShowDialog();
        }

        private void ModifyButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine($"ModifyButton_Click {Reminder.Title}");
            new UpdateReminderWindow(Reminder).ShowDialog();
        }

        private void RemindedButton_Click(object sender, RoutedEventArgs e)
        {
            ConfirmationWindow deleteWindow =
            new ConfirmationWindow($"Mark {Reminder.Title} as Reminded", $"Do You want to mark {Reminder.Title} as Reminded?", async () =>
            {
                using (var context = ReminderDBContext.GetContext())
                {
                    Reminder.Reminded = true;
                    await context.SaveChangesAsync();
                    MainWindow.Instance.ReadDatabase();
                }
            });
            deleteWindow.ShowDialog();
        }
    }
}
