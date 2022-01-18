using ff.resource_editor.model;
using System;
using System.Globalization;
using WpfTools;

namespace ff.resource_editor.converters
{
    /// <summary>
    /// Generates the text that's shown in the window's title bar
    /// </summary>
    internal class window_title_converter : MultiValueConverter
    {
        public override object Convert(object[] values, Type target_type, object parameter, CultureInfo culture)
        {
            string title = (string)parameter;
            project project = (project)values[0];

            if (project != null && project.has_file)
            {
                string file = (string)values[1];
                bool dirty = (bool)values[2];
                string dirty_flag = dirty ? "*" : string.Empty;

                return $"{title} - {file}{dirty_flag}";
            }

            return title;
        }
    }
}
