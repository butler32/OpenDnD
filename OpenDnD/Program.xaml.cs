using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenDnD.DB;
using OpenDnD.DB.Services;
using OpenDnD.Interfaces;
using OpenDnD.Windows;
using System.IO;
using System.Windows;

namespace OpenDnD
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Program : Window
    {
        public IHost Host { get; set; }
        public Program()
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
                    services.AddTransient<LoginWindowOld>();
                    services.AddTransient<LoginWindow>();
                    services.AddTransient<RegisterWindow>();
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

            var loginWindow = Host.Services.GetRequiredService<LoginWindow>();
            loginWindow.Top = (SystemParameters.PrimaryScreenHeight - loginWindow.Height) / 2;
            loginWindow.Left = (SystemParameters.PrimaryScreenWidth - loginWindow.Width) / 2;
            loginWindow.Show();

            this.Close();

        }
    }
}
