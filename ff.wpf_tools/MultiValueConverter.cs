namespace ff.WpfTools
{
    public abstract class MultiValueConverter : System.Windows.Data.IMultiValueConverter
    {
        public virtual object Convert(object[] values, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.InvalidOperationException();
        }

        public virtual object[] ConvertBack(object value, System.Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.InvalidOperationException();
        }

        object System.Windows.Data.IMultiValueConverter.Convert(object[] values, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return this.Convert(values, targetType, parameter, culture);
        }

        object[] System.Windows.Data.IMultiValueConverter.ConvertBack(object value, System.Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return this.ConvertBack(value, targetTypes, parameter, culture);
        }
    }
}
