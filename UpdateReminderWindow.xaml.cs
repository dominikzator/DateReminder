using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for AddReminderWindow.xaml
    /// </summary>
    public partial class UpdateReminderWindow : Window
    {
        private CancellationTokenSource wrongDataCancellationTokenSource;

        private User _activeUser;

        private const string FieldsEmpty = "One of the fields are empty";
        private const string TooLongTitle = "Title can't be longer than 30 characters";
        private const string IncorrectDateFormat= "Target Date format is incorrect. The correct format is YYYY-MM-DD";
        private const string IncorrectDaysToNotifyFormat= "Incorrect format of Days to Notify, Please insert a digit";
        private const string IncorrectDaysToElapseFormat= "Incorrect format of Days to Elapse, Please insert a digit";

        private const string TargetDatePattern = "\\d{4}\\-(0[1-9]|1[012]|[1-9])\\-(0[1-9]|[12][0-9]|3[01]|[1-9])$";

        private const string AddReminderText = "Add Reminder";
        private const string UpdateReminderText = "Update Reminder";

        private Reminder _reminder;

        public UpdateReminderWindow(User user)
        {
            InitializeComponent();
            _activeUser = user;
            UpdateReminderButton.Content = AddReminderText;
            IncorrectDataLabel.Visibility = Visibility.Hidden;
            wrongDataCancellationTokenSource = new CancellationTokenSource();
        }
        public UpdateReminderWindow(User user, Reminder reminder)
        {
            InitializeComponent();
            _activeUser = user;
            _reminder = reminder;
            wrongDataCancellationTokenSource = new CancellationTokenSource();
            UpdateReminderButton.Content = UpdateReminderText;
            IncorrectDataLabel.Visibility = Visibility.Hidden;
            TitleTextBox.Text = reminder.Title;
            TargetDateTextBox.Text = $"{reminder.TargetDate.Year}-{reminder.TargetDate.Month}-{reminder.TargetDate.Day}";
            DaysToElapseTextBox.Text = (reminder.SecondsToElapse / 3600 / 24).ToString();
            DaysToNotifyTextBox.Text = (reminder.SecondsToNotify / 3600 / 24).ToString();
            IsCyclicCheckBox.IsChecked = reminder.IsCyclic;
        }
        private bool AreFieldsEmpty() => TitleTextBox.Text.Length == 0 || TargetDateTextBox.Text.Length == 0 || DaysToElapseTextBox.Text.Length == 0 || DaysToNotifyTextBox.Text.Length == 0;
        private bool IsTitleLegit() => TitleTextBox.Text.ToString().Length <= 30;
        private bool IsDateFormatLegit() => Regex.IsMatch(TargetDateTextBox.Text, TargetDatePattern);
        private bool IsDaysToNotifyLegit() => int.TryParse(DaysToNotifyTextBox.Text, out _);
        private bool IsDaysToElapseLegit() => int.TryParse(DaysToElapseTextBox.Text, out _);
        private async void UpdateReminderButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("UpdateReminderButton_Click");
            if(AreFieldsEmpty())
            {
                PrintErrorMessage(FieldsEmpty);
                return;
            }
            if (!IsTitleLegit())
            {
                PrintErrorMessage(TooLongTitle);
                return;
            }
            if(!IsDateFormatLegit())
            {
                PrintErrorMessage(IncorrectDateFormat);
                return;
            }
            if (!IsDaysToNotifyLegit())
            {
                PrintErrorMessage(IncorrectDaysToNotifyFormat);
                return;
            }
            if (!IsDaysToElapseLegit())
            {
                PrintErrorMessage(IncorrectDaysToElapseFormat);
                return;
            }
            using (var context = ReminderDBContext.GetContext())
            {
                var splittedDate = TargetDateTextBox.Text.Split('-');


                if(_reminder == null)
                {
                    Reminder reminder = new Reminder
                    {
                        Title = TitleTextBox.Text,
                        TargetDate = new DateTime(int.Parse(splittedDate[0]), int.Parse(splittedDate[1]), int.Parse(splittedDate[2])),
                        Priority = 5,
                        SecondsToNotify = int.Parse(DaysToNotifyTextBox.Text) * 3600 * 24,
                        SecondsToElapse = int.Parse(DaysToElapseTextBox.Text) * 3600 * 24,
                        UserId = _activeUser.Id,
                        IsCyclic = (bool)IsCyclicCheckBox.IsChecked
                    };
                    await context.AddAsync(reminder);
                    await context.SaveChangesAsync();

                    InfoWindow.ShowReminderAddedWindow(() =>
                    {
                        this.Close();
                        MainWindow.Instance.ReadDatabase();
                    });
                }
                else
                {
                    _reminder.Title = TitleTextBox.Text;
                    _reminder.TargetDate = new DateTime(int.Parse(splittedDate[0]), int.Parse(splittedDate[1]), int.Parse(splittedDate[2]));
                    _reminder.SecondsToNotify = int.Parse(DaysToNotifyTextBox.Text) * 3600 * 24;
                    _reminder.SecondsToElapse = int.Parse(DaysToElapseTextBox.Text) * 3600 * 24;
                    _reminder.IsCyclic = (bool)IsCyclicCheckBox.IsChecked;
                    context.Update(_reminder);
                    await context.SaveChangesAsync();

                    InfoWindow.ShowReminderUpdatedWindow(() =>
                    {
                        this.Close();
                        MainWindow.Instance.ReadDatabase();
                    });
                }
            }
        }
        private async Task PrintErrorMessage(string message)
        {
            wrongDataCancellationTokenSource.Cancel();
            wrongDataCancellationTokenSource = new CancellationTokenSource();
            IncorrectDataLabel.Content = message;
            IncorrectDataLabel.Visibility = Visibility.Visible;
            await Task.Delay(5000, wrongDataCancellationTokenSource.Token);
            IncorrectDataLabel.Visibility = Visibility.Hidden;
        }
        private void HideErrorMessage()
        {
            wrongDataCancellationTokenSource.Cancel();
            wrongDataCancellationTokenSource = new CancellationTokenSource();
            IncorrectDataLabel.Visibility = Visibility.Hidden;
        }
        private void TitleTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Console.WriteLine("TitleTextBox_TextChanged");
            var contentLength = TitleTextBox.Text.ToString().Length;
            if (contentLength > 30 )
            {
                PrintErrorMessage(TooLongTitle);
                //TitleTextBox.Text = TitleTextBox.Text.Substring(0, 30);
                //TitleTextBox.CaretIndex = 30;
            }
            else
            {
                HideErrorMessage();
            }
        }

        private void TargetDateTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Console.WriteLine("TargetDateTextBox_TextChanged");
            var splittedText = TargetDateTextBox.Text.Split('-');
            if(splittedText.Length > 3)
            {
                PrintErrorMessage(IncorrectDateFormat);
            }
            if(splittedText.Length > 2)
            {

            }
        }

        private void DaysToNotifyTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Console.WriteLine("DaysToNotifyTextBox_TextChanged");

        }

        private void DaysToElapseTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Console.WriteLine("DaysToElapseTextBox_TextChanged");

        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Return)
            {
                UpdateReminderButton_Click(sender, e);
            }
        }
    }
}
