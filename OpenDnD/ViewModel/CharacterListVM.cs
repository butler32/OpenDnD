using OpenDnD.Utilities;

namespace OpenDnD.ViewModel
{
    class CharacterListVM : ViewModelBase
    {
        public CharacterListVM(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public IServiceProvider ServiceProvider { get; }
    }
}
