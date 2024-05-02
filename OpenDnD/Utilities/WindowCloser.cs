using System.Windows;

namespace OpenDnD.Utilities
{
    public class WindowCloser
    {
        public static bool GetEnableWIndowClosing(DependencyObject obj)
        {
            return (bool)obj.GetValue(EnableWindowClosginProperty);
        }

        public static void SetEnableWindowClosing(DependencyObject obj, bool value)
        {
            obj.SetValue(EnableWindowClosginProperty, value);
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EnableWindowClosginProperty =
            DependencyProperty.RegisterAttached("EnableWindowClosgin", typeof(bool), typeof(WindowCloser), new PropertyMetadata(false, OnEnableWindowClosingChanged));

        private static void OnEnableWindowClosingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Window window)
            {
                window.Loaded += (s, e) =>
                {
                    if (window.DataContext is ICloseWindow vm)
                    {
                        vm.Close += () =>
                        {
                            window.Close();
                        };

                        window.Closing += (s, e) =>
                        {
                            e.Cancel = !vm.CanClose();
                        };
                    }
                };
            }
        }
    }
}
