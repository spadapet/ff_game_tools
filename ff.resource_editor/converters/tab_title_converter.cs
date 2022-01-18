using ff.resource_editor.model;
using System;
using System.Globalization;
using WpfTools;

namespace ff.resource_editor.converters
{
    internal class tab_title_converter : MultiValueConverter
    {
        public override object Convert(object[] values, Type target_type, object parameter, CultureInfo culture)
        {
            edit_tab tab = values[0] as edit_tab;
            string name = values[1] as string ?? string.Empty;
            bool dirty = values[2] as bool? ?? false;
            string dirty_flag = dirty ? "*" : string.Empty;

            return $"{name}{dirty_flag}";
        }
    }
}
