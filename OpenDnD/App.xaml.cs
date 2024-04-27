using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenDnD.DB;
using OpenDnD.DB.Services;
using OpenDnD.Interfaces;
using OpenDnD.Windows;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Navigation;

namespace OpenDnD
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IHost Host { get; set; }
        public void InitializeComponent()
        {
            var loginWindow = Host.Services.GetRequiredService<LoginWindow>();
            loginWindow.Show();
        }

        

        [STAThread]
        public static void Main()
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddSingleton<App>();
                    services.AddSingleton<ApplicationAuthToken>(x => 
                    {
                        var Secret = x.GetRequiredService<Secret>();
                        var token = CryptoService.GetAuthToken(Guid.Empty, Secret.SecretKey);
                        return new ApplicationAuthToken
                        {
                            AuthToken = new AuthToken
                            {
                                PlayerId = Guid.Empty,
                                TokenValue = token
                            }
                        };
                    });
                    services.AddTransient<LoginWindow>();
                    services.AddTransient<SessionSelectionWindow>();
                    services.AddTransient<SessionCreationWindow>();
                    services.AddTransient<SessionInviterWindow>();
                    services.AddSingleton(x => new Secret { SecretKey = "UpEpMJRbaFfDoj9pRPAw780uGPeDCnpBi3zQzb3CTLK1Z0lmTkZZS29ViVhvQrElFK26iSz03fC7wqLx8pbvbMIaYqlUOXJZ" });

                    services.AddSingleton(x =>
                    {
                        var config = Newtonsoft.Json.JsonConvert.DeserializeObject<Config>(File.ReadAllText("appsettings.json")) 
                            ?? throw new Exception("cannot read appsettings.json file");
                        return config;
                    });

                    services.AddDbContext<OpenDnDContext>((provider, x) =>
                    {
                        var config = provider.GetRequiredService<Config>();
                        x.UseSqlite(config.ConnectionString);
                    }, ServiceLifetime.Transient);

                    services.AddTransient<IAuthService, PlayerService>();
                    services.AddTransient<IPlayerService, PlayerService>();
                    services.AddTransient<ISessionService, SessionService>();
                    
                })
                .Build();

            using (var scope = Host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<OpenDnDContext>();
                context.Database.Migrate();
            }

            var app = Host.Services.GetRequiredService<App>();

            app.InitializeComponent();
            app?.Run();
        }
    }
}