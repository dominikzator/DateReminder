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
    /// Interaction logic for ConfirmationWindow.xaml
    /// </summary>
    public partial class ConfirmationWindow : Window
    {
        private Action? onConfirm, onDecline;

        public ConfirmationWindow(string title, string message, Action? onConfirm = null, Action? onDecline = null)
        {
            InitializeComponent();
            MessageLabel.Content = message;
            Title = title;
            this.onConfirm = onConfirm;
            this.onDecline = onDecline;
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("YesButton_Click");
            onConfirm?.Invoke();
            Close();
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("NoButton_Click");
            onDecline?.Invoke();
            Close();
        }
    }
}
