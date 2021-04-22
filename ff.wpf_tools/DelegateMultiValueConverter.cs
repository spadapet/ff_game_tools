namespace ff.WpfTools
{
    public sealed class DelegateMultiValueConverter : MultiValueConverter
    {
        public delegate object ConvertFunc(object[] values, System.Type targetType, object parameter, System.Globalization.CultureInfo culture);
        public delegate object[] ConvertBackFunc(object value, System.Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture);

        private readonly DelegateMultiValueConverter.ConvertFunc convert;
        private readonly DelegateMultiValueConverter.ConvertBackFunc convertBack;

        public DelegateMultiValueConverter(DelegateMultiValueConverter.ConvertFunc convert, DelegateMultiValueConverter.ConvertBackFunc convertBack = null)
        {
            this.convert = convert;
            this.convertBack = convertBack;
        }

        public override object Convert(object[] values, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return this.convert?.Invoke(values, targetType, parameter, culture) ?? throw new System.InvalidOperationException();
        }

        public override object[] ConvertBack(object value, System.Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return this.convertBack?.Invoke(value, targetTypes, parameter, culture) ?? throw new System.InvalidOperationException();
        }
    }
}
