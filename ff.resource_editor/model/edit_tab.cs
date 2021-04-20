using ff.wpf_tools;
using System;
using System.Windows;
using System.Windows.Input;

namespace ff.resource_editor.model
{
    internal class edit_tab : property_notifier
    {
        private main_vm main_vm;
        private resource resource_;
        private bool active_;
        private Lazy<FrameworkElement> root_element_;

        protected edit_tab(main_vm main_vm, resource resource, Func<FrameworkElement> root_element_factory)
        {
            this.main_vm = main_vm;
            this.resource_ = resource;
            this.root_element_ = new(root_element_factory);
        }

        public static edit_tab create(main_vm main_vm, resource resource)
        {
            switch (resource.type)
            {
                default:
                    return new edit_tab(main_vm, resource, () => null);
            }
        }

        public resource resource => this.resource_;
        public string tab_name => this.resource.name;
        public string tab_tooltip => this.resource.source.file;

        public bool active
        {
            get => this.active_;
            set => this.set_property(ref this.active_, value);
        }

        public virtual FrameworkElement root_element => this.root_element_.Value;

        public ICommand activate_command => new delegate_command(() =>
        {
            this.main_vm.active_edit_tab = this;
        });

        public ICommand close_command => new delegate_command(() =>
        {
            this.main_vm.close_edit_tab(this);
        });
    }
}
