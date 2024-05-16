using System.Windows;
using System.Windows.Media;

namespace OpenDnD.Styles
{
    public static class StyleColors
    {
        //public static LinearGradientBrush BackgroundColorBrush = new LinearGradientBrush
        //{
        //    StartPoint = new Point(0, 0),
        //    EndPoint = new Point(1, 1),
        //    GradientStops = new GradientStopCollection
        //    {
        //        new GradientStop(Color.FromRgb(13, 27, 42), 0),
        //        new GradientStop(Color.FromRgb(10, 36, 47), 1),
        //    }
        //};
        public static Brush BackgroundColorBrush = new SolidColorBrush(Color.FromRgb(13, 27, 42));
        public static Brush TextColorBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        public static Brush IconsColorBrush = new SolidColorBrush(Color.FromRgb(1, 0, 17));
        public static Brush PrimaryColorBrush = new SolidColorBrush(Color.FromRgb(27, 38, 59));
        public static Brush PrimaryHoverColorBrush = new SolidColorBrush(Color.FromRgb(52, 73, 94));
        public static Brush SeconadryColorBrush = new SolidColorBrush(Color.FromRgb(127, 4, 160));
        public static Brush SeconadryHoverColorBrush = new SolidColorBrush(Color.FromRgb(148, 15, 184));
        public static Brush AccentColorBrush = new SolidColorBrush(Color.FromRgb(26, 188, 156));
        public static Brush AccentHoverColorBrush = new SolidColorBrush(Color.FromRgb(22, 160, 133));

        public static Color BackgroundColor = ((SolidColorBrush)BackgroundColorBrush).Color;
        public static Color TextColor = ((SolidColorBrush)TextColorBrush).Color;
        public static Color IconsColor = ((SolidColorBrush)IconsColorBrush).Color;
        public static Color PrimaryColor = ((SolidColorBrush)PrimaryColorBrush).Color;
        public static Color PrimaryHoverColor = ((SolidColorBrush)PrimaryHoverColorBrush).Color;
        public static Color SeconadryColor = ((SolidColorBrush)SeconadryColorBrush).Color;
        public static Color SeconadryHoverColor = ((SolidColorBrush)SeconadryHoverColorBrush).Color;
        public static Color AccentColor = ((SolidColorBrush)AccentColorBrush).Color;
        public static Color AccentHoverColor = ((SolidColorBrush)AccentHoverColorBrush).Color;
    }
}
