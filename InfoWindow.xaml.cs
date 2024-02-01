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

        private Window parentToClose;

        private InfoWindow(string message, Window parentToClose = null)
        {
            InitializeComponent();
            MessageLabel.Content = message;
            this.parentToClose = parentToClose;
        }

        public static void ShowAccountCreatedWindow(Window parentToClose) => new InfoWindow(AccountCreatedDialog, parentToClose).ShowDialog();

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
            parentToClose?.Close();
        }
    }
}
