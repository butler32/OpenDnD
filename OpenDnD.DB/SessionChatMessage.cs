using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDnD.DB
{
    public class SessionChatMessage
    {
        public Guid SessionChatMessageId { get; set; }
        public Guid SessionId { get; set; }
        public virtual Session Session { get; set; }
        public Guid PlayerId { get; set; }
        public virtual Player Player { get; set; }
        public string Message { get; set; }
        public DateTime DateTime { get; set; }
    }
}
