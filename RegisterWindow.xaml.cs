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
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private const string AccountExistsMessage = "Account with the same Login already exists";
        private const string PasswordsNotTheSameMessage = "Passwords You have typed are not the same";
        private const string OneOfTheFieldEmptyMessage = "One of the fields is empty";

        private CancellationTokenSource wrongRegisterCancellationTokenSource;

        public RegisterWindow()
        {
            InitializeComponent();
            IncorrectRegisterLabel.Visibility = Visibility.Hidden;
            wrongRegisterCancellationTokenSource = new CancellationTokenSource();
        }
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if(!ReminderDBContext.IsDisposed)
            {
                Console.WriteLine("DB Was in use, suspending action");
                return;
            }
            Console.WriteLine("RegisterButton_Click");
            if(IsRegisterLegit())
            {
                ResetLabels();
                InfoWindow.ShowAccountCreatedWindow(this);
            }
            //PrintAccountExistsMessage();
            //PrintPasswordsNotTheSameMessage();
        }

        private void ResetLabels()
        {
            LoginTextBox.Text = "";
            PasswordTextBox.Text = "";
            RepeatPasswordTextBox.Text = "";
        }

        private bool IsRegisterLegit()
        {
            if(LoginTextBox.Text == "" || PasswordTextBox.Text == "" || RepeatPasswordTextBox.Text == "")
            {
                Console.WriteLine("Login or Password Empty");
                PrintErrorMessage(OneOfTheFieldEmptyMessage);
                return false;
            }
            using (var context = new ReminderDBContext())
            {
                bool usersEmpty = context.Users.Count() == 0;
                if(!usersEmpty && context.Users.First(p => p.UserName == LoginTextBox.Text) != null)
                {
                    Console.WriteLine("An account with this Login already exists in the database");
                    PrintErrorMessage(AccountExistsMessage);
                    return false;
                }
                if(PasswordTextBox.Text != RepeatPasswordTextBox.Text)
                {
                    Console.WriteLine("Password doesn't match");
                    PrintErrorMessage(PasswordsNotTheSameMessage);
                    return false;
                }

            }
            HideErrorMessage();
            return true;
        }
        private void HideErrorMessage()
        {
            wrongRegisterCancellationTokenSource.Cancel();
            IncorrectRegisterLabel.Visibility = Visibility.Hidden;
        }
        private async Task PrintErrorMessage(string message)
        {
            wrongRegisterCancellationTokenSource.Cancel();
            wrongRegisterCancellationTokenSource = new CancellationTokenSource();
            IncorrectRegisterLabel.Content = message;
            IncorrectRegisterLabel.Visibility = Visibility.Visible;
            await Task.Delay(5000, wrongRegisterCancellationTokenSource.Token);
            IncorrectRegisterLabel.Visibility = Visibility.Hidden;
        }
    }
}
