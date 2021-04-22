using System.Windows;

namespace ff.WpfTools
{
    public sealed class BoolToCollapsedConverter : BoolToObjectConverter
    {
        public BoolToCollapsedConverter()
        {
            this.TrueValue = Visibility.Collapsed;
            this.FalseValue = Visibility.Visible;
        }
    }
}
