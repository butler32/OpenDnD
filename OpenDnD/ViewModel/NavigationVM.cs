using Microsoft.Extensions.DependencyInjection;
using OpenDnD.Interfaces;
using OpenDnD.Utilities;
using System.Windows.Input;

namespace OpenDnD.ViewModel
{
    public class NavigationVM : ViewModelBase, ICloseWindow
    {
        private object _currentView;
        public object CurrentView 
        {
            get { return _currentView; } 
            set { _currentView = value; OnPropertyChanged(); }
        }

        public ICommand SessionsCommand { get; set; }
        public ICommand SessionCreationCommand { get; set; }
        public ICommand CharactersCommand { get; set; }
        public ICommand CharacterListCommand { get; set; }
        public ICommand EntitiesCommand { get; set; }
        public ICommand LogoutCommand { get; set; }
        public IServiceProvider ServiceProvider { get; }
        public UserAuthToken UserAuthToken { get; }
        public Action Close { get; set; }

        private void Sessions(object? obj) => CurrentView = ServiceProvider.GetRequiredService<SessionsVM>();
        private void SessionCreation(object obj) => CurrentView = new SessionCreationVM();
        private void Characters(object obj) => CurrentView = new CharactersVM();
        private void CharacterList(object obj) => CurrentView = new CharacterListVM();
        private void Entities(object obj) => CurrentView = new EntitiesVM();
        private void CloseWindow()
        {
            Close?.Invoke();
        }
        private void Logout(object ojb)
        {
            var loginWindow = ServiceProvider.GetRequiredService<LoginRegisterWindow>();
            loginWindow.Show();
            CloseWindow();
        }

        public bool CanClose()
        {
            return true;
        }

        public NavigationVM(IServiceProvider serviceProvider, UserAuthToken userAuthToken)
        {
            SessionsCommand = new RelayCommand(Sessions);
            SessionCreationCommand = new RelayCommand(SessionCreation);
            CharactersCommand = new RelayCommand(Characters);
            CharacterListCommand = new RelayCommand(CharacterList);
            EntitiesCommand = new RelayCommand(Entities);
            LogoutCommand = new RelayCommand(Logout);

            ServiceProvider = serviceProvider;
            UserAuthToken = userAuthToken;
            SessionsCommand.Execute(null);
        }
    }
}
