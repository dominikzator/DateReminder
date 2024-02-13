using Microsoft.EntityFrameworkCore;
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
    /// Interaction logic for LogInWindow.xaml
    /// </summary>
    public partial class LogInWindow : Window
    {
        private CancellationTokenSource printIncorrectLabelTokenSource;

        private const string KeySensitiveCollation = "SQL_Latin1_General_CP1_CS_AS";
        public LogInWindow()
        {
            InitializeComponent();
            IncorrectLoginLabel.Visibility = Visibility.Hidden;
            printIncorrectLabelTokenSource = new CancellationTokenSource();
        }

        private void SignInButton_Click(object sender, RoutedEventArgs? e = null)
        {
            Console.WriteLine("Sign In Click!");
            User? loggedUser;
            if(IsLoginLegit(out loggedUser))
            {
                var mainWindow = MainWindow.GetMainWindow(loggedUser);
                mainWindow.Show();
                Close();
            }
            else
            {
                PrintIncorrectLabel();
            }
        }

        private bool IsLoginLegit(out User? loggedUser)
        {
            loggedUser = null;
            if(!ReminderDBContext.IsDisposed)
            {
                return false;
            }
            using (var context = ReminderDBContext.GetContext())
            {
                if(context.Users.Count() == 0)
                {
                    return false;
                }

                Console.WriteLine($"LoginTextBox.Text: {LoginTextBox.Text}");

                foreach(var user in context.Users)
                {
                    Console.WriteLine($"user.UserName: {user.UserName}");
                }
                User? foundedUser = context.Users.FirstOrDefault(p => EF.Functions.Collate(p.UserName, KeySensitiveCollation) == LoginTextBox.Text
                && EF.Functions.Collate(p.Password, KeySensitiveCollation) == PasswordTextBox.Text);
                if(foundedUser != null)
                {
                    loggedUser = foundedUser;
                    return true;
                }

                return false;
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs? e = null)
        {
            Console.WriteLine("Register Click!");
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
            Console.WriteLine($"PasswordTextBox_KeyDown {e.Key}");
            if (e.Key == Key.Return)
            {
                Console.WriteLine("On Return!");
                SignInButton_Click(sender);
            }
        }
    }
}
