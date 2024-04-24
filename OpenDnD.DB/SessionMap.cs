using System.ComponentModel.DataAnnotations;

namespace OpenDnD.DB
{
    public class SessionMap
    {
        [Key]
        public Guid SessionMapId { get; set; }
        public Guid SessionId { get; set; }
        public virtual Session Session { get; set; }

        public string SessionMapName { get; set; }
        public Guid ImageId { get; set; }
        public Image Image { get; set; }
    }
}
