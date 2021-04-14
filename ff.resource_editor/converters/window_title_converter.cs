using ff.resource_editor.model;
using ff.wpf_tools;
using System;
using System.Globalization;

namespace ff.resource_editor.converters
{
    /// <summary>
    /// Generates the text that's shown in the window's title bar
    /// </summary>
    internal class window_title_converter : multi_value_converter
    {
        public override object convert(object[] values, Type target_type, object parameter, CultureInfo culture)
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
