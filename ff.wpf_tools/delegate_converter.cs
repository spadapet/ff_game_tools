namespace ff.wpf_tools
{
    public sealed class delegate_converter : System.Windows.Data.IValueConverter
    {
        public delegate object convert_func(object value, System.Type target_type, object parameter, System.Globalization.CultureInfo culture);

        private readonly delegate_converter.convert_func convert;
        private readonly delegate_converter.convert_func convert_back;

        public delegate_converter(delegate_converter.convert_func convert, delegate_converter.convert_func convert_back = null)
        {
            this.convert = convert;
            this.convert_back = convert_back;
        }

        object System.Windows.Data.IValueConverter.Convert(object value, System.Type target_type, object parameter, System.Globalization.CultureInfo culture)
        {
            return this.convert?.Invoke(value, target_type, parameter, culture) ?? throw new System.InvalidOperationException();
        }

        object System.Windows.Data.IValueConverter.ConvertBack(object value, System.Type target_type, object parameter, System.Globalization.CultureInfo culture)
        {
            return this.convert_back?.Invoke(value, target_type, parameter, culture) ?? throw new System.InvalidOperationException();
        }
    }
}
