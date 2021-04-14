using ff.wpf_tools;
using System;
using System.IO;
using System.Runtime.Serialization;

namespace ff.resource_editor.model
{
    [DataContract]
    internal class source_file : property_notifier
    {
        private string path_;
        private bool dirty_;

        public source_file()
        {
            this.path_ = string.Empty;
        }

        [DataMember]
        public string path
        {
            get => this.path_;
            set
            {
                if (this.set_property(ref this.path_, value ?? string.Empty))
                {
                    this.on_property_changed(nameof(this.name));
                }
            }
        }

        public string name => Path.GetFileName(this.path);

        public bool dirty
        {
            get => this.dirty;
            set => this.set_property(ref this.dirty_, value);
        }

        public void save()
        {
            this.dirty = false;
        }
    }
}
