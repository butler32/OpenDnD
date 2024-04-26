using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDnD.Interfaces
{
    public class Player
    {
        public Player(Guid playerId, string userName)
        {
            PlayerId = playerId;
            UserName = userName;
        }

        public Guid PlayerId { get; set; }
        public string UserName { get; set; }
    }

    public class PlayerRequest
    {
        public string UserName { get; set; }
        public string PasswordSalt { get; set; }
        public string PasswordHash { get; set; }
    }

    public interface IPlayerService : ICRUDService<Player, PlayerRequest>
    {
        public List<Player> GetPlayerListFromSession(AuthToken authToken, List<Guid> playerIds);
    }
}
