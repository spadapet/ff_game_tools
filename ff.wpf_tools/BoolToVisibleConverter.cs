using System.Windows;

namespace ff.WpfTools
{
    public sealed class BoolToVisibleConverter : BoolToObjectConverter
    {
        public BoolToVisibleConverter()
        {
            this.TrueValue = Visibility.Visible;
            this.FalseValue = Visibility.Collapsed;
        }
    }
}
