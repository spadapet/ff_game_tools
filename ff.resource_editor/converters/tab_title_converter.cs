using ff.resource_editor.model;
using ff.WpfTools;
using System;
using System.Globalization;

namespace ff.resource_editor.converters
{
    internal class tab_title_converter : MultiValueConverter
    {
        public override object Convert(object[] values, Type target_type, object parameter, CultureInfo culture)
        {
            edit_tab tab = (edit_tab)values[0];
            string name = (string)values[1];
            bool dirty = (bool)values[2];
            string dirty_flag = dirty ? "*" : string.Empty;

            return $"{name}{dirty_flag}";
        }
    }
}
