using OpenDnD.Utilities;

namespace OpenDnD.ViewModel
{
    class EntitiesVM : ViewModelBase
    {
        public EntitiesVM(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public IServiceProvider ServiceProvider { get; }
    }
}
