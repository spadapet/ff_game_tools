using ff.wpf_tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ff.resource_editor.model
{
    [DataContract]
    internal class project : property_notifier
    {
        private readonly ObservableCollection<source_file> sources_;
        private string file_;
        private bool dirty_;

        public project()
        {
            this.sources_ = new();
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

        public static async Task<project> load_async(string file)
        {
            string json_project = await File.ReadAllTextAsync(file, Encoding.UTF8);
            project project = await Task.Run(() =>
            {
                project project_ = Efficient.Json.JsonValue.StringToObject<project>(json_project);
                project_.file = file;
                project_.dirty = false;
                return project_;
            });

            return project;
        }

        public async Task save_async(string file = null)
        {
            List<Exception> exceptions = new();

            foreach (source_file source in this.sources.ToArray())
            {
                try
                {
                    await source.save_async();
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            string project_json = await Task.Run(() => Efficient.Json.JsonValue.ObjectToString(this, true));

            file = file ?? this.file;
            await File.WriteAllTextAsync(file, project_json, Encoding.UTF8);
            this.file = file;

            if (exceptions.Count > 0)
            {
                throw new AggregateException($"Save failed: {file}", exceptions);
            }

            this.dirty = false;
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
