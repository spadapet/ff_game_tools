using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace ff.resource_editor.model
{
    [DataContract]
    internal class project : ff.wpf_tools.property_notifier
    {
        private readonly ObservableCollection<source_file> sources_;

        public project()
        {
            this.sources_ = new();
        }

        [DataMember]
        public IList<source_file> sources => this.sources_;
    }
}
