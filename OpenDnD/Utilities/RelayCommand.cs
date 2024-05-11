namespace OpenDnD.Utilities
{
    public interface ICommand : System.Windows.Input.ICommand {
        void System.Windows.Input.ICommand.Execute(object? parameter) => Execute();
        bool System.Windows.Input.ICommand.CanExecute(object? parameter) => true;

        void Execute();
        public static RelayCommand<T> From<T>(Action<T> action) => new(action);
        public static RelayCommand From(Action action) => new(action);

    }

    public interface ICommand<T> : System.Windows.Input.ICommand
    {
        void System.Windows.Input.ICommand.Execute(object? parameter) => Execute((T)parameter);
        bool System.Windows.Input.ICommand.CanExecute(object? parameter) => true;
        void Execute(T parameter);
    }

    public class RelayCommand : ICommand
    {
        public RelayCommand(Action action)
        {
            Action = action;
        }

        public Action Action { get; }

        public event EventHandler? CanExecuteChanged;

        public void Execute()
        {
            Action.Invoke();
        }
    }
    

    public class RelayCommand<T> : ICommand<T>
    {
        private readonly Action<T> _execute;

        public RelayCommand(Action<T> execute)
        {
            _execute = execute;
        }

        public event EventHandler? CanExecuteChanged;

        public void Execute(T parameter) => _execute(parameter);
    }
}
