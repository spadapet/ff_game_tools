using ff.resource_editor.ui;
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
        private bool dirty_;
        private Lazy<FrameworkElement> root_element_;

        protected edit_tab(main_vm main_vm, resource resource, Func<FrameworkElement> root_element_factory)
        {
            this.main_vm = main_vm;
            this.resource_ = resource;
            this.root_element_ = new(root_element_factory);
        }

        public static edit_tab create(main_vm main_vm, resource resource)
        {
            Func<FrameworkElement> root_element_factory = () => null;

            switch (resource.type)
            {
                case resource_type.texture:
                    root_element_factory = () => new texture_editor(new edit_texture(resource));
                    break;
            }

            return new edit_tab(main_vm, resource, root_element_factory);
        }

        public resource resource => this.resource_;
        public string tab_name => this.resource.name;
        public string tab_tooltip => this.resource.source.file;

        public bool active
        {
            get => this.active_;
            set => this.set_property(ref this.active_, value);
        }

        public bool dirty
        {
            get => this.dirty_;
            set => this.set_property(ref this.dirty_, value);
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
