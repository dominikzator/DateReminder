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

namespace DateReminder
{
    /// <summary>
    /// Interaction logic for InfoWindow.xaml
    /// </summary>
    public partial class InfoWindow : Window
    {
        private const string AccountCreatedDialog = "Account has been successfully created";
        private const string ReminderAddedDialog = "A new Reminder has been successfully added";
        private const string ReminderUpdatedDialog = "A Reminder has been successfully updated";

        private Action? onClose;

        private InfoWindow(string message, Action? onClose = null)
        {
            InitializeComponent();
            MessageLabel.Content = message;
            this.onClose = onClose;
        }

        public static void ShowAccountCreatedWindow(Action? onClose = null) => new InfoWindow(AccountCreatedDialog, onClose).ShowDialog();
        public static void ShowReminderAddedWindow(Action? onClose = null) => new InfoWindow(ReminderAddedDialog, onClose).ShowDialog();
        public static void ShowReminderUpdatedWindow(Action? onClose = null) => new InfoWindow(ReminderUpdatedDialog, onClose).ShowDialog();

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
            onClose?.Invoke();
        }
    }
}
