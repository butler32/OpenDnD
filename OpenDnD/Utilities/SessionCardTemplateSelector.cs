using System.Windows;
using System.Windows.Controls;

namespace OpenDnD.Utilities
{
    class SessionCardTemplateSelector : DataTemplateSelector
    {
        public DataTemplate EmptyTemplate { get; set; }
        public DataTemplate SessionTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null)
                return EmptyTemplate;
            else
                return SessionTemplate;
        }
    }
}
