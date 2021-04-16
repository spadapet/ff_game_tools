﻿using Efficient.Json;
using ff.wpf_tools;
using System;
using System.Threading.Tasks;

namespace ff.resource_editor.model
{
    internal class resource : property_notifier, IEquatable<resource>, IComparable<resource>
    {
        private source_file source_;
        private resource_type type_;
        private string name_;

        protected resource(source_file source, string name, resource_type type)
        {
            this.source_ = source;
            this.type_ = type;
            this.name_ = name ?? string.Empty;
        }

        public source_file source => this.source_;
        public resource_type type => this.type_;
        public string type_name => resource_type_utility.name_of(this.type_);
        public string name => this.name_;

        public async static Task<resource> load_async(source_file source, string name, JsonValue value)
        {
            return await Task.Run(() =>
            {
                resource resource = null;

                if (value.IsObject)
                {
                    JsonValue type_value = value["res:type"];
                    if (type_value != null && type_value.IsString && Enum.TryParse<resource_type>(type_value.String, out resource_type type) && type != resource_type.none)
                    {
                        switch (type)
                        {
                            default:
                                resource = new resource(source, name, type);
                                break;
                        }
                    }
                }

                return resource;
            });
        }

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
