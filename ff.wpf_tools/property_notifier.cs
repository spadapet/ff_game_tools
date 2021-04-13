namespace ff.wpf_tools
{
    [System.Runtime.Serialization.DataContract]
    public abstract class property_notifier : System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
    {
        private event System.ComponentModel.PropertyChangingEventHandler property_changing;
        private event System.ComponentModel.PropertyChangedEventHandler property_changed;

        protected void on_properties_changing()
        {
            this.on_property_changing(null);
        }

        protected void on_properties_changed()
        {
            this.on_property_changed(null);
        }

        protected void on_property_changing(string name)
        {
            this.property_changing?.Invoke(this, new System.ComponentModel.PropertyChangingEventArgs(name));
        }

        protected void on_property_changed(string name)
        {
            this.property_changed?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(name));
        }

        protected bool set_property<T>(ref T property, T value, [System.Runtime.CompilerServices.CallerMemberName] string name = null)
        {
            if (System.Collections.Generic.EqualityComparer<T>.Default.Equals(property, value))
            {
                return false;
            }

            if (name != null)
            {
                this.on_property_changing(name);
            }

            property = value;

            if (name != null)
            {
                this.on_property_changed(name);
            }

            return true;
        }

        event System.ComponentModel.PropertyChangingEventHandler System.ComponentModel.INotifyPropertyChanging.PropertyChanging
        {
            add => this.property_changing += value;
            remove => this.property_changing -= value;
        }

        event System.ComponentModel.PropertyChangedEventHandler System.ComponentModel.INotifyPropertyChanged.PropertyChanged
        {
            add => this.property_changed += value;
            remove => this.property_changed -= value;
        }
    }
}
