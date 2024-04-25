using System.ComponentModel.DataAnnotations;

namespace OpenDnD.DB
{
    public class Session
    {
        [Key]
        public Guid SessionId { get; set;}
        public string SessionName { get; set; }
        public virtual ICollection<SessionPlayer>? Players { get; set; }
    }
}
