using System.ComponentModel.DataAnnotations;

namespace OpenDnD.DB
{
    public class SessionMapEntity
    {
        [Key]
        public Guid SessionMapEntityId { get; set;}

        public Guid SessionMapId { get; set; }
        public virtual SessionMap SessionMap { get; set; }

        public Guid EntityId { get; set; }
        public virtual Entity Entity { get; set; }

        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public bool Visibility { get; set; }
    }
}
