namespace ff.WpfTools
{
    public sealed class DelegateValueConverter : ValueConverter
    {
        public delegate object ConvertFunc(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture);

        private readonly DelegateValueConverter.ConvertFunc convert;
        private readonly DelegateValueConverter.ConvertFunc convertBack;

        public DelegateValueConverter(DelegateValueConverter.ConvertFunc convert, DelegateValueConverter.ConvertFunc convertBack = null)
        {
            this.convert = convert;
            this.convertBack = convertBack;
        }

        public override object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return this.convert?.Invoke(value, targetType, parameter, culture) ?? throw new System.InvalidOperationException();
        }

        public override object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return this.convertBack?.Invoke(value, targetType, parameter, culture) ?? throw new System.InvalidOperationException();
        }
    }
}
