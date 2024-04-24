using System.ComponentModel.DataAnnotations;

namespace OpenDnD.DB
{

    public class Entity
    {
        [Key]
        public Guid EnitityId { get; set; }
        public string EnitityName { get; set; }

        public Guid ImageId { get; set; }
        public virtual Image Image { get; set; }
    }
}
