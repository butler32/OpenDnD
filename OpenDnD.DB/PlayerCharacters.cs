using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDnD.DB
{
    public class PlayerCharacters
    {
        [Key]
        public Guid PlayerCharacterId { get; set; }
        public Guid PlayerId { get; set; }
        public string PlayerCharacterName { get; set; }

    }
}
