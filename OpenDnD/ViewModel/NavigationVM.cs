using Microsoft.Extensions.DependencyInjection;
using OpenDnD.Model;
using OpenDnD.Utilities;
using System.Windows.Input;

namespace OpenDnD.ViewModel
{
    public class NavigationVM : ViewModelBase, ICloseWindow
    {
        private ViewModelBase? _previousView;
        public ViewModelBase? PreviousView
        {
            get { return _previousView; }
            set { _previousView = value; OnPropertyChanged(); }
        }

        private ViewModelBase _currentView;
        public ViewModelBase CurrentView 
        {
            get { return _currentView; } 
            set { _currentView = value; OnPropertyChanged(); }
        }

        public ICommand SessionsCommand { get; }
        public ICommand SessionCreationCommand { get; }
        public ICommand CharactersCommand { get; }
        public ICommand CharacterListCommand { get; }
        public ICommand EntitiesCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand SessionInviterCommand { get; }
        public IServiceProvider ServiceProvider { get; }
        public UserAuthToken UserAuthToken { get; }
        public Action Close { get; set; }

        private void Sessions(object? obj)
        {
            var sessionsVM = ServiceProvider.GetRequiredService<SessionsVM>();

            sessionsVM.SessionCreationRequested += SessionCreationEventHandler;

            PreviousView = null;

            CurrentView = sessionsVM;
        }

        private void SessionCreation(object obj)
        {
            PreviousView = (ViewModelBase?)CurrentView.Clone();

            var vm = ServiceProvider.GetRequiredService<SessionCreationVM>();

            if (obj is SessionModel)
            {
                vm.CurrentSession = (SessionModel)obj;
            }

            vm.InviteEvent += InviteEventHandler;

            CurrentView = vm;
        }
        
        private void SessionInviter(object obj)
        {
            PreviousView = (ViewModelBase?)CurrentView.Clone();

            var vm = ServiceProvider.GetRequiredService<SessionInviterVM>();

            vm.SetCurrentSession((Guid)obj);

            CurrentView = vm;
        }

        private void InviteEventHandler(Guid guid)
        {
            SessionInviterCommand.Execute(guid);
        }

        private void Characters(object obj)
        {
            PreviousView = null;

            CurrentView = ServiceProvider.GetRequiredService<CharactersVM>();
        }
        private void CharacterList(object obj) => CurrentView = ServiceProvider.GetRequiredService<CharacterListVM>();
        private void Entities(object obj)
        {
            PreviousView = null;

            CurrentView = ServiceProvider.GetRequiredService<EntitiesVM>();
        }

        private void Back(object obj)
        {
            CurrentView = (ViewModelBase)PreviousView.Clone();

            PreviousView = null;
        }

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

        private void SessionCreationEventHandler(object? obj)
        {
            SessionCreationCommand.Execute(obj);
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
            BackCommand = new RelayCommand(Back);
            SessionInviterCommand = new RelayCommand(SessionInviter);

            ServiceProvider = serviceProvider;
            UserAuthToken = userAuthToken;
            SessionsCommand.Execute(null);
            PreviousView = null;
        }
    }
}
