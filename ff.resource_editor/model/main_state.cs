using ff.wpf_tools;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;

namespace ff.resource_editor.model
{
    internal class main_state : property_notifier
    {
        private string project_file_;

        public main_state()
        {
            this.project_file_ = string.Empty;
        }

        [DataMember]
        public string project_file
        {
            get => this.project_file_;
            set => this.set_property(ref this.project_file_, value ?? string.Empty);
        }

        public static string states_file => Path.Combine(main_state.app_data_path, "main_state.json");
        private static string exe_name => Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location);

        private static string app_data_path
        {
            get
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(path, main_state.exe_name);
            }
        }
    }
}
