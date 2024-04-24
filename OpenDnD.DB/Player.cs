using System.ComponentModel.DataAnnotations;

namespace OpenDnD.DB
{
    public class Player
    {
        [Key]
        public Guid PlayerId { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
    }
}
