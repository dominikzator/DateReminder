using EncryptionDecryptionUsingSymmetricKey;
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

        private SecureString securePwd = new SecureString();
        private ConsoleKeyInfo key;

        private const string KeySensitiveCollation = "SQL_Latin1_General_CP1_CS_AS";

        private static string PasswordKey;

        public LogInWindow()
        {
            InitializeComponent();
            IncorrectLoginLabel.Visibility = Visibility.Hidden;
            printIncorrectLabelTokenSource = new CancellationTokenSource();

            IConfigurationRoot Configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            PasswordKey = Configuration.GetConnectionString("PasswordKey");

            try
            {
                using (StreamReader sr = File.OpenText("TempUser.txt"))
                {
                    string s = "";
                    int lineIndex = 0;
                    string login = "";
                    string decryptedPassword = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        if (lineIndex == 0)
                        {
                            login = s;
                        }
                        else if (lineIndex == 1)
                        {
                            try
                            {
                                using(var context = new ReminderDBContext())
                                {
                                    decryptedPassword = StringCipher.DecryptString(PasswordKey, s);
                                }
                            }
                            catch
                            {
                                return;
                            }
                        }
                        lineIndex++;
                    }
                    User loggedUser;
                    if (TryGetUser(login, decryptedPassword, out loggedUser))
                    {
                        var mainWindow = MainWindow.GetMainWindow(loggedUser);
                        mainWindow.Show();
                        Close();
                    }
                }
            }
            catch
            {
                Console.WriteLine("Couldn't find a TempFile, opening LoginWindow...");
            }
        }

        private async void TryCacheData(string login, string password)
        {
            if ((bool)RememberMeCheckBox.IsChecked)
            {
                using (StreamWriter sw = File.CreateText("TempUser.txt"))
                {
                    for (int i = 0; i < password.Length; i++)
                    {
                        securePwd.AppendChar(password[i]);
                    }

                    var encryptedString = StringCipher.EncryptString(PasswordKey, password);

                    sw.WriteLine(login);
                    sw.WriteLine(encryptedString);
                }
            }
        }

        private void SignInButton_Click(object sender, RoutedEventArgs? e = null)
        {
            User? loggedUser;
            if (TryGetUser(LoginTextBox.Text, PasswordTextBox.Text, out loggedUser))
            {
                TryCacheData(loggedUser.UserName, loggedUser.Password);
                var mainWindow = MainWindow.GetMainWindow(loggedUser);
                mainWindow.Show();
                Close();
            }
            else
            {
                PrintIncorrectLabel();
            }
        }

        private bool TryGetUser(string login, string password, out User? loggedUser)
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

                User? foundedUser = context.Users.FirstOrDefault(p => EF.Functions.Collate(p.UserName, KeySensitiveCollation) == login
                && EF.Functions.Collate(p.Password, KeySensitiveCollation) == password);
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
