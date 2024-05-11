using OpenDnD.Utilities;
using OpenDnD.Utilities.DI;

namespace OpenDnD.ViewModel
{
    class CharactersVM : ViewModelBase
    {
        public CharactersVM(IServiceProvider serviceProvider)
        {
            serviceProvider.UseDI(this);
        }

        [FromDI]
        public IServiceProvider ServiceProvider { get; private set; }
    }
}
