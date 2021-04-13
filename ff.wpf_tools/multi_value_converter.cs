namespace ff.wpf_tools
{
    public abstract class multi_value_converter : System.Windows.Data.IMultiValueConverter
    {
        public virtual object convert(object[] values, System.Type target_type, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.InvalidOperationException();
        }

        public virtual object[] convert_back(object value, System.Type[] target_types, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.InvalidOperationException();
        }

        object System.Windows.Data.IMultiValueConverter.Convert(object[] values, System.Type target_type, object parameter, System.Globalization.CultureInfo culture)
        {
            return this.convert(values, target_type, parameter, culture);
        }

        object[] System.Windows.Data.IMultiValueConverter.ConvertBack(object value, System.Type[] target_types, object parameter, System.Globalization.CultureInfo culture)
        {
            return this.convert_back(value, target_types, parameter, culture);
        }
    }
}
