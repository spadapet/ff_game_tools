using ff.wpf_tools;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace ff.resource_editor.model
{
    [DataContract]
    internal class source_file : property_notifier, IEquatable<source_file>, IComparable<source_file>
    {
        private string path_;
        private bool dirty_;

        public source_file()
            : this(string.Empty)
        { }

        public source_file(string path)
        {
            this.path_ = path ?? string.Empty;
        }

        [DataMember]
        public string path
        {
            get => this.path_;
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

                if (this.set_property(ref this.path_, value))
                {
                    this.on_property_changed(nameof(this.name));
                }
            }
        }

        public string name => Path.GetFileName(this.path);

        public bool dirty
        {
            get => this.dirty_;
            set => this.set_property(ref this.dirty_, value);
        }

        public bool save()
        {
            this.dirty = false;
            return true;
        }

        public override int GetHashCode()
        {
            return this.path_?.GetHashCode() ?? 0;
        }

        public override bool Equals(object obj)
        {
            return obj is source_file other && ((IEquatable<source_file>)this).Equals(other);
        }

        bool IEquatable<source_file>.Equals(source_file other)
        {
            return string.Equals(this.path, other.path, StringComparison.CurrentCultureIgnoreCase);
        }

        int IComparable<source_file>.CompareTo(source_file other)
        {
            return string.Compare(this.path, other.path, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
