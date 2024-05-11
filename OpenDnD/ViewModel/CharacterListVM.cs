using OpenDnD.Utilities;
using OpenDnD.Utilities.DI;

namespace OpenDnD.ViewModel
{
    class CharacterListVM : ViewModelBase
    {
        public CharacterListVM(IServiceProvider serviceProvider)
        {
            serviceProvider.UseDI(this);
        }

        [FromDI]
        public IServiceProvider ServiceProvider { get; private set; }
    }
}
