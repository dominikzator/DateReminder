using EncryptionDecryptionUsingSymmetricKey;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for CoreWindow.xaml
    /// </summary>
    public partial class CoreWindow : Window
    {
        private int attempts;
        private int maxAttempts = 10;

        private SecureString securePwd = new SecureString();
        private ConsoleKeyInfo key;

        private static string PasswordKey;
        private const string KeySensitiveCollation = "SQL_Latin1_General_CP1_CS_AS";

        public static CoreWindow Instance { get; private set; }

        public User ActiveUser { get; private set; }

        private TaskbarIcon taskBarIcon;

        private ToastsManager toastsManager;

        private User loggedUser;

        public CoreWindow()
        {
            Console.WriteLine("CoreWindow");
            Instance = this;
            InitializeComponent();
            toastsManager = ToastsManager.Instance;

            taskBarIcon = myNotifyIcon;
            taskBarIcon.Icon = Resource1.BellIcon;
            taskBarIcon.ToolTipText = "Double Click to open DateReminder Main Panel";
            taskBarIcon.TrayMouseDoubleClick += OnTrayIconDoubleClick;
            taskBarIcon.TrayRightMouseDown += OnTrayIconRightClickPressed;
            //QuitMenuItem.Icon = Resource1.cancel_icon;
            QuitMenuItem.Click += (object sender, RoutedEventArgs e) => {
                Console.WriteLine("QUIT APP");
                Application.Current.Shutdown();
            };

            IConfigurationRoot Configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            PasswordKey = Configuration.GetConnectionString("PasswordKey");
            TryReadFromTempFile();
            Console.WriteLine("Making a Tray");
        }

        private void OnTrayIconRightClickPressed(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("OnTrayIconRightClickPressed");
            //SingletonWindow<IconOptions>.Instance.WindowInstance.Show();
        }

        private void OnTrayIconDoubleClick(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("OnTrayIconDoubleClick");
            if(ActiveUser != null)
            {
                SingletonWindow<MainWindow>.Instance.WindowInstance.Show();
            }
            else
            {
                SingletonWindow<LogInWindow>.Instance.WindowInstance.Show();
            }
        }

        private void TryReadFromTempFile()
        {
            Console.WriteLine("TryReadFromTempFile attempt: " + attempts);
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
                            decryptedPassword = StringCipher.DecryptString(PasswordKey, s);
                        }
                        lineIndex++;
                    }
                    TryGetUser(login, decryptedPassword, out loggedUser);
                    if (loggedUser != null)
                    {
                        ToastsManager.Instance.SynchronizeRemindersWithDelay(delayInSeconds: 3f);
                    }
                    else
                    {
                        Console.WriteLine($"Readed from temp file, but couldn't get a User from Database, attempts: {attempts}");
                        if (++attempts < maxAttempts)
                        {
                            TryReadFromTempFile();
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Catch!");
                if (ex is IOException)
                {
                    Console.WriteLine("Couldn't find a TempFile, opening LoginWindow...");
                    SingletonWindow<LogInWindow>.Instance.WindowInstance.Show();
                }
                else if (ex is SqlException)
                {
                    Console.WriteLine($"Couldn't login to database... Attempts: {attempts}");
                    if (++attempts < maxAttempts)
                    {
                        TryReadFromTempFile();
                        return;
                    }
                    else
                    {
                        Console.WriteLine($"More than {maxAttempts} Attempts");
                        SingletonWindow<LogInWindow>.Instance.WindowInstance.Show();
                    }
                }
            }
            Console.WriteLine($"After {maxAttempts} Attempts");
        }
        public async void TryCacheData(string login, string password)
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
        public bool TryGetUser(string login, string password, out User? loggedUser)
        {
            loggedUser = null;

            using (var context = ReminderDBContext.GetContext())
            {
                if (context.Users.Count() == 0)
                {
                    return false;
                }

                User? foundedUser = context.Users.FirstOrDefault(p => EF.Functions.Collate(p.UserName, KeySensitiveCollation) == login
                && EF.Functions.Collate(p.Password, KeySensitiveCollation) == password);
                if (foundedUser != null)
                {
                    loggedUser = foundedUser;
                    ActiveUser = loggedUser;
                    return true;
                }

                return false;
            }
        }
    }
}
