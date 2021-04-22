using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ff.WpfTools
{
    public class NullToObjectConverter : DependencyObject, IValueConverter
    {
        public static readonly DependencyProperty NonNullValueProperty = DependencyProperty.Register("NonNullValue", typeof(object), typeof(NullToObjectConverter), new PropertyMetadata(null));
        public static readonly DependencyProperty NullValueProperty = DependencyProperty.Register("NullValue", typeof(object), typeof(NullToObjectConverter), new PropertyMetadata(null));
        public static readonly DependencyProperty EmptyStringValueProperty = DependencyProperty.Register("EmptyStringValue", typeof(object), typeof(NullToObjectConverter), new PropertyMetadata(null));

        public object NonNullValue
        {
            get { return this.GetValue(NullToObjectConverter.NonNullValueProperty); }
            set { this.SetValue(NullToObjectConverter.NonNullValueProperty, value); }
        }

        public object NullValue
        {
            get { return this.GetValue(NullToObjectConverter.NullValueProperty); }
            set { this.SetValue(NullToObjectConverter.NullValueProperty, value); }
        }

        public object EmptyStringValue
        {
            get { return this.GetValue(NullToObjectConverter.EmptyStringValueProperty); }
            set { this.SetValue(NullToObjectConverter.EmptyStringValueProperty, value); }
        }

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if (this.ReadLocalValue(NullToObjectConverter.EmptyStringValueProperty) != DependencyProperty.UnsetValue && value is string valueString && string.IsNullOrEmpty(valueString))
                {
                    return this.EmptyStringValue;
                }

                return this.NonNullValue;
            }

            return this.NullValue;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException();
        }
    }
}
