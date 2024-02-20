using System.Configuration;
using System.Data;
using System.Windows;

namespace DateReminder
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application a = new Application();
            a.StartupUri = new Uri("CoreWindow.xaml", System.UriKind.Relative);
            a.Run();
        }
    }

}
