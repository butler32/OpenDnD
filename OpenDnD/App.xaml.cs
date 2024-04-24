using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Configuration;
using System.Data;
using System.Windows;

namespace OpenDnD
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public void InitializeComponent()
        {
            this.StartupUri = new System.Uri("MainWindow.xaml", System.UriKind.Relative);
        }

        [STAThread]
        public static void Main()
        {
            // создаем хост приложения
            var host = Host.CreateDefaultBuilder()
                // внедряем сервисы
                .ConfigureServices(services =>
                {
                    services.AddSingleton<App>();
                    services.AddSingleton<MainWindow>();
                })
                .Build();
            // получаем сервис - объект класса App
            var app = host.Services.GetService<App>();
            app.InitializeComponent();
            // запускаем приложения
            app?.Run();
        }
    }
}