using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDnD.Interfaces
{
    public class SessionPlayer
    {
        public SessionPlayer(Guid playerId, string userName, RoleEnum role)
        {
            PlayerId = playerId;
            UserName = userName;
            Role = role;
        }

        public Guid PlayerId { get; set; }
        public string UserName { get; set; }
        public RoleEnum Role { get; set; }
    }
}
