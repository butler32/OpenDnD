using OpenDnD.Interfaces;
using System.Windows.Media.Imaging;

namespace OpenDnD.Model
{
    class SessionPlayerModel
    {
        public Guid PlayerId { get; set; }
        public string UserName { get; set; }
        public Guid SessionId { get; set; }
        public BitmapImage Image { get; set; }
        public string Role { get; set; }
    }
}
