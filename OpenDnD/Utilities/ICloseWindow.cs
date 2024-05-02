using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDnD.Utilities
{
    public interface ICloseWindow
    {
        Action Close { get; set; }
        bool CanClose();
    }
}
