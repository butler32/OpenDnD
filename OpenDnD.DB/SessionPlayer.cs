using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace OpenDnD.DB
{
    [PrimaryKey(nameof(SessionId), nameof(PlayerId))]
    public class SessionPlayer
    {
        public Guid SessionId { get; set;}
        public virtual Session Session { get; set; }
        public Guid PlayerId { get; set; }
        public virtual Player Player { get; set; }

        public int PlayerRole { get; set; }
    }
}
