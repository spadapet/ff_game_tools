using ff.wpf_tools;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;

namespace ff.resource_editor.model
{
    /// <summary>
    /// Persists between app sessions
    /// </summary>
    [DataContract]
    internal class app_state : property_notifier
    {
        private string project_file_;

        public app_state()
        {
            this.project_file_ = string.Empty;
        }

        [DataMember]
        public string project_file
        {
            get => this.project_file_;
            set => this.set_property(ref this.project_file_, value ?? string.Empty);
        }

        public static string state_file => Path.Combine(app_state.app_data_path, $"{nameof(app_state)}.json");
        private static string exe_name => Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location);
        private static string app_data_path => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData, Environment.SpecialFolderOption.Create), app_state.exe_name);
    }
}
