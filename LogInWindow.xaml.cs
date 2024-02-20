using EncryptionDecryptionUsingSymmetricKey;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security;
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
    /// Interaction logic for LogInWindow.xaml
    /// </summary>
    public partial class LogInWindow : Window
    {
        private CancellationTokenSource printIncorrectLabelTokenSource;

        public LogInWindow()
        {
            InitializeComponent();

            IncorrectLoginLabel.Visibility = Visibility.Hidden;
            printIncorrectLabelTokenSource = new CancellationTokenSource();
        }

        private void SignInButton_Click(object sender, RoutedEventArgs? e = null)
        {
            User? loggedUser;
            if (CoreWindow.Instance.TryGetUser(LoginTextBox.Text, PasswordTextBox.Text, out loggedUser))
            {
                if ((bool)RememberMeCheckBox.IsChecked)
                {
                    CoreWindow.Instance.TryCacheData(loggedUser.UserName, loggedUser.Password);
                }
                SingletonWindow<MainWindow>.Instance.WindowInstance.Show();
                Close();
            }
            else
            {
                PrintIncorrectLabel();
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs? e = null)
        {
            var registerWindow = new RegisterWindow();
            registerWindow.ShowDialog();
        }

        private async Task PrintIncorrectLabel()
        {
            printIncorrectLabelTokenSource.Cancel();
            printIncorrectLabelTokenSource = new CancellationTokenSource();
            IncorrectLoginLabel.Visibility = Visibility.Visible;
            await Task.Delay(5000, printIncorrectLabelTokenSource.Token);
            IncorrectLoginLabel.Visibility = Visibility.Hidden;
        }

        private void PasswordTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                SignInButton_Click(sender);
            }
        }
    }
}
