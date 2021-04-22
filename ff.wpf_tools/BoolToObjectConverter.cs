using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ff.WpfTools
{
    public class BoolToObjectConverter : DependencyObject, IValueConverter
    {
        public static readonly DependencyProperty TrueValueProperty = DependencyProperty.Register("TrueValue", typeof(object), typeof(BoolToObjectConverter), new PropertyMetadata(null));
        public static readonly DependencyProperty FalseValueProperty = DependencyProperty.Register("FalseValue", typeof(object), typeof(BoolToObjectConverter), new PropertyMetadata(null));

        public object TrueValue
        {
            get { return this.GetValue(BoolToObjectConverter.TrueValueProperty); }
            set { this.SetValue(BoolToObjectConverter.TrueValueProperty, value); }
        }

        public object FalseValue
        {
            get { return this.GetValue(BoolToObjectConverter.FalseValueProperty); }
            set { this.SetValue(BoolToObjectConverter.FalseValueProperty, value); }
        }

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
            {
                return b ? this.TrueValue : this.FalseValue;
            }

            throw new InvalidOperationException();
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (object.Equals(value, this.TrueValue))
            {
                return true;
            }

            if (object.Equals(value, this.FalseValue))
            {
                return false;
            }

            throw new InvalidOperationException();
        }
    }
}
