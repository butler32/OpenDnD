using Microsoft.Extensions.DependencyInjection;
using OpenDnD.Utilities;
using System.Windows;
using System.Windows.Input;

namespace OpenDnD.ViewModel
{
    public class LoginNavigationVM : ViewModelBase
    {
        private bool _isShouldClose;
        public bool IsShouldClose
        {
            get { return _isShouldClose; }
            set { _isShouldClose = value; OnPropertyChanged(); }
        }

        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }

        public Action Close { get; set; }

        void CloseWindow(object obj)
        {
            Close?.Invoke();
        }

        IServiceProvider ServiceProvider;
        public ICommand CloseCommand {  get; set; }
        public ICommand LoginCommand { get; set; }
        public ICommand RegisterCommand { get; set; }

        private void Register(object obj) => CurrentView = ServiceProvider.GetRequiredService<RegistrationVM>();
        private void Login(object? obj) => CurrentView = ServiceProvider.GetRequiredService<LoginVM>();

        public LoginNavigationVM(IServiceProvider serviceProvider)
        {
            LoginCommand = new RelayCommand(Login);
            RegisterCommand = new RelayCommand(Register);
            CloseCommand = new RelayCommand(CloseWindow);
            ServiceProvider = serviceProvider;

            Login(null);
        }
    }
}
