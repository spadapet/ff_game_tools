namespace ff.wpf_tools
{
    public sealed class delegate_multi_value_converter : System.Windows.Data.IMultiValueConverter
    {
        public delegate object convert_func(object[] values, System.Type target_type, object parameter, System.Globalization.CultureInfo culture);
        public delegate object[] convert_back_func(object value, System.Type[] target_types, object parameter, System.Globalization.CultureInfo culture);

        private readonly delegate_multi_value_converter.convert_func convert;
        private readonly delegate_multi_value_converter.convert_back_func convert_back;

        public delegate_multi_value_converter(delegate_multi_value_converter.convert_func convert, delegate_multi_value_converter.convert_back_func convert_back = null)
        {
            this.convert = convert;
            this.convert_back = convert_back;
        }

        object System.Windows.Data.IMultiValueConverter.Convert(object[] values, System.Type target_type, object parameter, System.Globalization.CultureInfo culture)
        {
            return this.convert?.Invoke(values, target_type, parameter, culture) ?? throw new System.InvalidOperationException();
        }

        object[] System.Windows.Data.IMultiValueConverter.ConvertBack(object value, System.Type[] target_types, object parameter, System.Globalization.CultureInfo culture)
        {
            return this.convert_back?.Invoke(value, target_types, parameter, culture) ?? throw new System.InvalidOperationException();
        }
    }
}
