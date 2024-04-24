using System.ComponentModel.DataAnnotations;

namespace OpenDnD.DB
{
    public class SessionPlayer
    {
        [Key]
        public Guid SessionId { get; set;}
        public virtual Session Session { get; set; }
        [Key]
        public Guid PlayerId { get; set; }
        public virtual Player Player { get; set; }

        public string PlayerRole { get; set; }
    }
}
