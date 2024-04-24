using System.ComponentModel.DataAnnotations;

namespace OpenDnD.DB
{
    public class Image
    {
        [Key]
        public Guid ImageId { get; set; }
        public byte[] ImageContent { get; set; }
        public string ImageType { get; set; }
    }
}
