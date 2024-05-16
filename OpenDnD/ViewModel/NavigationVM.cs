using Microsoft.Extensions.DependencyInjection;
using OpenDnD.Model;
using OpenDnD.Utilities;
using OpenDnD.Utilities.DI;
using System.Reflection;

namespace OpenDnD.ViewModel
{
    public class NavigationVM : ViewModelBase, ICloseWindow
    {
        // Properties

        public Type? PreviousViewType { get; set; }

        private ViewModelBase _currentView;
        public ViewModelBase CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }

        // Commands

        public ICommand SessionsCommand { get; }
        public ICommand CharactersCommand { get; }
        public ICommand CharacterListCommand { get; }
        public ICommand EntitiesCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand LogoutCommand { get; }

        // Services

        [FromDI]
        public IServiceProvider ServiceProvider { get; private set; }
        [FromDI]
        public UserAuthToken UserAuthToken { get; private set; }

        // Command's methods

        private void Sessions()
        {
            var sessionsVM = new SessionsVM(ServiceProvider)
            {
                CreateSessionCommand = ICommand.From(() => SessionCreation(null)),
                EditSessionCommand = ICommand.From((SessionModel sm) => SessionCreation(sm))
            };

            PreviousViewType = null;

            CurrentView = sessionsVM;
        }

        private void SessionCreation(SessionModel? session)
        {
            PreviousViewType = typeof(SessionsVM);

            if (session == null)
            {
                session = new SessionModel()
                {
                    SessionName = "New Session",
                    PlayersIds = new List<Guid>()
                };
            }

            var vm = new SessionCreationVM(ServiceProvider, session)
            {
                InviteCommand = ICommand.From((SessionModel sm) => SessionInviter(sm))
            };

            CurrentView = vm;
        }

        private void SessionInviter(SessionModel sessionModel)
        {
            PreviousViewType = typeof(SessionCreationVM);

            var vm = new SessionInviterVM(ServiceProvider)
            {
                CurrentSession = sessionModel
            };

            CurrentView = vm;
        }

        private void Characters()
        { 
            PreviousViewType = null;

            CurrentView = new CharactersVM(ServiceProvider);
        }
        private void CharacterList()
        {
            PreviousViewType = typeof (CharactersVM);

            CurrentView = new CharacterListVM(ServiceProvider);
        }
        private void Entities()
        {
            PreviousViewType = null;

            CurrentView = new EntitiesVM(ServiceProvider);
        }

        private void Logout()
        {
            var loginWindow = ServiceProvider.GetRequiredService<LoginRegisterWindow>();
            loginWindow.Show();
            CloseWindow();
        }

        private void Back()
        {
            
        }

        // Close stuff

        public Action Close { get; set; }

        private void CloseWindow()
        {
            Close?.Invoke();
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
            PreviousViewType = null;
        }
    }
}
