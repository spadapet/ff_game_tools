using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace ff.resource_editor.model
{
    [DataContract]
    internal class project : ff.wpf_tools.property_notifier
    {
        private readonly ObservableCollection<source_file> sources_;
        private string file_;
        private bool dirty_;

        public project()
        {
            if (ff.wpf_tools.wpf_utility.design_mode)
            {
                this.sources_ = new()
                {
                    new("c:\\foo\\bar.res.json"),
                    new("c:\\hello world\\hi.json")
                };
            }
            else
            {
                this.sources_ = new();
            }

            this.sources_.CollectionChanged += this.on_sources_changed;
            this.file_ = string.Empty;
        }

        [DataMember]
        public IList<source_file> sources => this.sources_;

        public bool dirty
        {
            get => this.dirty_ || this.sources.Any(s => s.dirty);
            private set => this.set_property(ref this.dirty_, value);
        }

        public bool has_file => !string.IsNullOrEmpty(this.file_);
        public string file
        {
            get => this.file_;
            private set => this.set_property(ref this.file_, value ?? string.Empty);
        }

        public static project load(string file)
        {
            string json_project = File.ReadAllText(file);
            project project = Efficient.Json.JsonValue.StringToObject<project>(json_project);
            project.file = file;
            project.dirty = false;
            return project;
        }

        public bool save(string file = null)
        {
            bool status = true;

            foreach (source_file source in this.sources)
            {
                if (!source.save())
                {
                    status = false;
                }
            }

            string project_json = Efficient.Json.JsonValue.ObjectToString(this, true);

            file = file ?? this.file;
            File.WriteAllText(file, project_json);
            this.file = file;
            this.dirty = false;

            return status;
        }

        private void on_sources_changed(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (args.OldItems != null)
            {
                foreach (INotifyPropertyChanged source in args.OldItems)
                {
                    source.PropertyChanged -= this.on_source_property_changed;
                    this.dirty = true;
                }
            }

            if (args.NewItems != null)
            {
                foreach (INotifyPropertyChanged source in args.NewItems)
                {
                    source.PropertyChanged += this.on_source_property_changed;
                    this.dirty = true;
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
