using Microsoft.Extensions.DependencyInjection;
using OpenDnD.Model;
using OpenDnD.Utilities;
using OpenDnD.Utilities.DI;

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
        public ICommand CharactersCommand { get; }
        public ICommand CharacterListCommand { get; }
        public ICommand EntitiesCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand LogoutCommand { get; }
        [FromDI]
        public IServiceProvider ServiceProvider { get; private set; }
        [FromDI]
        public UserAuthToken UserAuthToken { get; private set; }
        public Action Close { get; set; }

        private void Sessions()
        {
            var sessionsVM = new SessionsVM(ServiceProvider)
            {
                CreateSessionCommand = ICommand.From(() => SessionCreation(null)),
                EditSessionCommand = ICommand.From((SessionModel sm) => SessionCreation(sm))
            };

            PreviousView = null;

            CurrentView = sessionsVM;
        }

        private void SessionCreation(SessionModel? obj)
        {
            PreviousView = (ViewModelBase?)CurrentView.Clone();

            var vm = new SessionCreationVM(ServiceProvider)
            {
                CurrentSession = obj ?? new SessionModel { },
                InviteCommand = ICommand.From((SessionModel sm) => SessionInviter(sm))
            };

            CurrentView = vm;
        }

        private void SessionInviter(SessionModel sessionModel)
        {
            PreviousView = (ViewModelBase?)CurrentView.Clone();

            var vm = new SessionInviterVM(ServiceProvider)
            {
                CurrentSession = sessionModel
            };

            CurrentView = vm;
        }

        private void Characters()
        { 
            PreviousView = null;

            CurrentView = new CharactersVM(ServiceProvider);
        }
        private void CharacterList()
        {
            CurrentView = new CharacterListVM(ServiceProvider);
        }
        private void Entities()
        {
            PreviousView = null;

            CurrentView = new EntitiesVM(ServiceProvider);
        }

        private void Back()
        {
            CurrentView = (ViewModelBase)PreviousView.Clone();

            PreviousView = null;
        }

        private void CloseWindow()
        {
            Close?.Invoke();
        }
        private void Logout()
        {
            var loginWindow = ServiceProvider.GetRequiredService<LoginRegisterWindow>();
            loginWindow.Show();
            CloseWindow();
        }

        public bool CanClose()
        {
            return true;
        }

        public NavigationVM(IServiceProvider serviceProvider)
        {
            serviceProvider.UseDI(this);

            SessionsCommand = ICommand.From(Sessions);
            CharactersCommand = ICommand.From(Characters);
            CharacterListCommand = ICommand.From(CharacterList);
            EntitiesCommand = ICommand.From(Entities);
            LogoutCommand = ICommand.From(Logout);
            BackCommand = ICommand.From(Back);

            
            SessionsCommand.Execute();
            PreviousView = null;
        }
    }
}
