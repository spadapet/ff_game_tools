using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;

namespace ff.resource_editor.model
{
    [DataContract]
    internal class project : ff.wpf_tools.property_notifier
    {
        private readonly ObservableCollection<source_file> sources_;
        private string file_;

        public project()
        {
            this.sources_ = new();
            this.sources_.CollectionChanged += this.on_sources_changed;
            this.file_ = string.Empty;
        }

        [DataMember]
        public IList<source_file> sources => this.sources_;

        public bool dirty => this.sources.Any(s => s.dirty);

        public bool has_file => !string.IsNullOrEmpty(this.file_);
        public string file => this.file_;

        public void save()
        {
            foreach (source_file source in this.sources)
            {
                source.save();
            }

            Debug.Assert(!this.dirty);
        }

        private void on_sources_changed(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (args.OldItems != null)
            {
                foreach (INotifyPropertyChanged source in args.OldItems)
                {
                    source.PropertyChanged -= this.on_source_property_changed;
                }
            }

            if (args.NewItems != null)
            {
                foreach (INotifyPropertyChanged source in args.NewItems)
                {
                    source.PropertyChanged += this.on_source_property_changed;
                }
            }
        }

        private void on_source_property_changed(object sender, PropertyChangedEventArgs args)
        {
            bool any = string.IsNullOrEmpty(args.PropertyName);

            if (any || args.PropertyName == nameof(source_file.dirty))
            {
                this.on_property_changed(nameof(this.dirty));
            }
        }
    }
}
