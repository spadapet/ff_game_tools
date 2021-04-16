using Efficient.Json;
using ff.wpf_tools;
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
    internal class source_file : property_notifier, IEquatable<source_file>, IComparable<source_file>
    {
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

        public static async Task<source_file> load_async(string file)
        {
            return await load_into_async(new(), file);
        }

        public static async Task<source_file> load_into_async(source_file source, string file)
        {
            source.resources_.Clear();
            source.extra_root_values.Clear();

            if (File.Exists(file))
            {
                JsonValue root = await Task.Run(() => JsonValue.StringToValue(new StreamReader(file, detectEncodingFromByteOrderMarks: true)));
                if (root.IsObject)
                {
                    foreach (KeyValuePair<string, JsonValue> i in root.Object)
                    {
                        resource resource = null;

                        if (i.Value.IsObject)
                        {
                            resource = await resource.load_async(source, i.Key, i.Value);
                        }

                        if (resource != null)
                        {
                            source.resources.Add(resource);
                        }
                        else
                        {
                            source.extra_root_values.Add(i.Value);
                        }
                    }
                }
            }

            source.dirty = false;
            return source;
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

                if (this.set_property(ref this.file_, value))
                {
                    this.on_property_changed(nameof(this.name));

                    source_file.load_into_async(this, value).fire_and_forget("source_file.file");
                }
            }
        }

        public string name => Path.GetFileName(this.file);

        public bool dirty
        {
            get => this.dirty_;
            set => this.set_property(ref this.dirty_, value);
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
