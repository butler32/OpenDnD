using OpenDnD.Utilities;
using OpenDnD.Utilities.DI;

namespace OpenDnD.ViewModel
{
    class EntitiesVM : ViewModelBase
    {
        public EntitiesVM(IServiceProvider serviceProvider)
        {
            serviceProvider.UseDI(this);
        }

        [FromDI]
        public IServiceProvider ServiceProvider { get; private set; }
    }
}
