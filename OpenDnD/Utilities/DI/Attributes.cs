using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDnD.Utilities.DI
{
    public enum DIInsertStrategy
    {
        Required,
        NotRequired,
    }
    public enum DILookStrategy
    {
        Required,
        NotRequired
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class FromDIAttribute : Attribute
    {
        public FromDIAttribute(DIInsertStrategy strategy = DIInsertStrategy.Required)
        {
            Strategy = strategy;
        }

        public DIInsertStrategy Strategy { get; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class DILookAttribute : Attribute
    {
        public DILookAttribute(DILookStrategy dIStrategy = DILookStrategy.Required)
        {
            Strategy = dIStrategy;
        }

        public DILookStrategy Strategy { get; }
    }
}
