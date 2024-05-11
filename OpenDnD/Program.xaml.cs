using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenDnD.DB;
using OpenDnD.DB.Services;
using OpenDnD.Interfaces;
using OpenDnD.Utilities;
using OpenDnD.Utilities.DI;
using OpenDnD.ViewModel;
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

                    // Add AuthTokens

                    services.AddSingleton<UserAuthToken>();
                    services.AddSingleton<ApplicationAuthToken>(x =>
                    {
                        var Secret = x.GetRequiredService<Secret>();
                        var token = CryptoService.GetAuthToken(Guid.Empty, Secret.SecretKey);
                        return new ApplicationAuthToken(new AuthToken(Guid.Empty, token));
                    });

                    // --- WARNING --- USE EXPERIMENTAL DI
                    services.UseDI();
                    //

                    // Add ViewModels
                    
                    /*
                    services.AddTransient<NavigationVM>();
                    services.AddTransient<SessionsVM>();
                    services.AddTransient<SessionCreationVM>();
                    services.AddTransient<CharacterListVM>();
                    services.AddTransient<CharactersVM>();
                    services.AddTransient<EntitiesVM>();
                    services.AddTransient<LoginVM>();
                    services.AddTransient<SessionsVM>();
                    services.AddTransient<SessionInviterVM>();
                    */

                    // Add Windows

                    services.AddTransient<MainWindow>(s => new MainWindow
                    {
                        DataContext = new NavigationVM(s)
                    });                 
                    services.AddTransient<LoginRegisterWindow>(s => new LoginRegisterWindow
                    {
                        DataContext = new LoginVM(s)
                    });

                    // Add Services

                    services.AddTransient<IAuthService, PlayerService>();
                    services.AddTransient<IPlayerService, PlayerService>();
                    services.AddTransient<ISessionService, SessionService>();

                    // Add Other Services

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

                })
                .Build();

            using (var scope = Host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<OpenDnDContext>();
                context.Database.Migrate();
            }

            var loginRegWindow = Host.Services.GetRequiredService<LoginRegisterWindow>();
            loginRegWindow.Show();

            this.Close();

        }
    }
}
