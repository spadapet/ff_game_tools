using ff.wpf_tools;
using System;

namespace ff.resource_editor.model
{
    internal class resource : property_notifier, IEquatable<resource>, IComparable<resource>
    {
        private source_file source_;
        private string name_;
        private string type_;

        public resource(source_file source, string name, string type)
        {
            this.source_ = source;
            this.name_ = name ?? string.Empty;
            this.type_ = type ?? string.Empty;
        }

        public source_file source => this.source_;
        public string name => this.name_;
        public string type => this.type_;

        public override int GetHashCode()
        {
            return this.name_.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is resource other && ((IEquatable<resource>)this).Equals(other);
        }

        bool IEquatable<resource>.Equals(resource other)
        {
            return object.Equals(this.source_, other.source_) && string.Equals(this.name, other.name);
        }

        int IComparable<resource>.CompareTo(resource other)
        {
            IComparable<source_file> other_comparable = other.source;
            int i = other_comparable.CompareTo(this.source_);

            if (i != 0)
            {
                i = string.Compare(this.name_, other.name_);
            }

            return i;
        }
    }
}
