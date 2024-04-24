using Microsoft.EntityFrameworkCore;

namespace OpenDnD.DB
{
    public class OpenDnDContext : DbContext
    {
        public DbSet<Image> Images { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<SessionPlayer> SessionPlayers { get; set; }
        public DbSet<SessionMap> SessionMaps { get; set; }
        public DbSet<SessionMapEntity> SessionMapEntities { get; set; }
        public DbSet<Entity> Entities { get; set; }
    }
}
