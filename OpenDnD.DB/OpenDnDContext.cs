using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OpenDnD.DB
{
    public class OpenDnDContextFactory : IDesignTimeDbContextFactory<OpenDnDContext>
    {
        public OpenDnDContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<OpenDnDContext>();
            optionsBuilder.UseSqlite("Data Source=database.db");

            return new OpenDnDContext(optionsBuilder.Options);
        }
    }
    public class OpenDnDContext : DbContext
    {
        public DbSet<PlayerCharacters> PlayerCharacters { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<SessionPlayer> SessionPlayers { get; set; }
        public DbSet<SessionMap> SessionMaps { get; set; }
        public DbSet<SessionMapEntity> SessionMapEntities { get; set; }
        public DbSet<Entity> Entities { get; set; }
        public DbSet<SessionChatMessage> SessionChatMessages { get; set; }

        public OpenDnDContext(DbContextOptions<OpenDnDContext> options) : base(options) { }
    }
}
