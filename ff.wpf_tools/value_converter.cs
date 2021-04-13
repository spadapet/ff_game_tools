namespace ff.wpf_tools
{
    public abstract class value_converter : System.Windows.Data.IValueConverter
    {
        public virtual object convert(object value, System.Type target_type, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.InvalidOperationException();
        }

        public virtual object convert_back(object value, System.Type target_type, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.InvalidOperationException();
        }

        object System.Windows.Data.IValueConverter.Convert(object value, System.Type target_type, object parameter, System.Globalization.CultureInfo culture)
        {
            return this.convert(value, target_type, parameter, culture);
        }

        object System.Windows.Data.IValueConverter.ConvertBack(object value, System.Type target_type, object parameter, System.Globalization.CultureInfo culture)
        {
            return this.convert_back(value, target_type, parameter, culture);
        }
    }
}
