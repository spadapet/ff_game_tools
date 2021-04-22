namespace ff.WpfTools
{
    public abstract class ValueConverter : System.Windows.Data.IValueConverter
    {
        public virtual object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.InvalidOperationException();
        }

        public virtual object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.InvalidOperationException();
        }

        object System.Windows.Data.IValueConverter.Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return this.Convert(value, targetType, parameter, culture);
        }

        object System.Windows.Data.IValueConverter.ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return this.ConvertBack(value, targetType, parameter, culture);
        }
    }
}
