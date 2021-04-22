using Efficient.Json;
using ff.WpfTools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ff.resource_editor.model
{
    [DataContract]
    internal class source_file : PropertyNotifier, IEquatable<source_file>, IComparable<source_file>
    {
        private project project_;
        private string file_;
        private bool dirty_;
        private List<JsonValue> extra_root_values;
        private ObservableCollection<resource> resources_;

        public source_file()
        {
            this.file_ = string.Empty;
            this.extra_root_values = new();
            this.resources_ = new();
        }

        [OnDeserialized]
        public async void deserialized(StreamingContext streaming_context)
        {
            await this.load_file_async();
        }

        public static async Task<source_file> load_async(string file)
        {
            source_file source = new();
            source.file = file;
            await source.load_file_async();
            return source;
        }

        public async Task load_file_async()
        {
            this.resources_.Clear();
            this.extra_root_values.Clear();

            if (File.Exists(this.file))
            {
                JsonValue root = await Task.Run(() => JsonValue.StringToValue(new StreamReader(this.file, detectEncodingFromByteOrderMarks: true)));
                if (root.IsObject)
                {
                    foreach (KeyValuePair<string, JsonValue> i in root.Object)
                    {
                        resource resource = null;

                        if (i.Value.IsObject)
                        {
                            resource = await resource.load_async(this, i.Key, i.Value);
                        }

                        if (resource != null)
                        {
                            this.resources.Add(resource);
                        }
                        else
                        {
                            this.extra_root_values.Add(i.Value);
                        }
                    }
                }
            }
        }

        public async Task save_async()
        {
            this.dirty = false;
            await Task.CompletedTask;
        }

        [DataMember]
        public string file
        {
            get => this.file_;
            set
            {
                if (value != null)
                {
                    try
                    {
                        value = Path.GetFullPath(value);
                    }
                    catch (Exception ex)
                    {
                        Debug.Fail(ex.Message);
                        value = string.Empty;
                    }
                }
                else
                {
                    value = string.Empty;
                }

                if (this.SetProperty(ref this.file_, value))
                {
                    this.OnPropertyChanged(nameof(this.name));
                    this.OnPropertyChanged(nameof(this.directory));
                    this.OnPropertyChanged(nameof(this.project_relative_file));
                }
            }
        }

        public string name => Path.GetFileName(this.file);
        public string directory => Path.GetDirectoryName(this.file);
        public string project_relative_file => (this.project_ != null) ? Path.GetRelativePath(this.project.directoy, this.file) : this.file;

        public project project
        {
            get => this.project_;
            set
            {
                if (this.SetProperty(ref this.project_, value))
                {
                    this.OnPropertyChanged(nameof(this.project_relative_file));
                }
            }
        }


        public bool dirty
        {
            get => this.dirty_;
            set => this.SetProperty(ref this.dirty_, value);
        }

        public IList<resource> resources => this.resources_;

        public override int GetHashCode()
        {
            return this.file_?.GetHashCode() ?? 0;
        }

        public override bool Equals(object obj)
        {
            return obj is source_file other && ((IEquatable<source_file>)this).Equals(other);
        }

        bool IEquatable<source_file>.Equals(source_file other)
        {
            return string.Equals(this.file, other.file, StringComparison.CurrentCultureIgnoreCase);
        }

        int IComparable<source_file>.CompareTo(source_file other)
        {
            return string.Compare(this.file, other.file, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
