using OpenDnD.Utilities;

namespace OpenDnD.ViewModel
{
    class CharactersVM : ViewModelBase
    {
        public CharactersVM(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public IServiceProvider ServiceProvider { get; }
    }
}
