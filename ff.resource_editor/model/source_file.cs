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
        private ObservableCollection<resource> resources_;

        public source_file()
        {
            this.file_ = string.Empty;

            if (wpf_utility.design_mode)
            {
                this.resources_ = new()
                {
                    new(this, "name_1", "texture"),
                    new(this, "name_2", "sprites"),
                    new(this, "name_3", "animation"),
                };
            }
            else
            {
                this.resources_ = new();
            }
        }

        public static async Task<source_file> load_async(string file)
        {
            source_file source = new()
            {
                file = file,
            };

            return await Task.FromResult(source);
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
