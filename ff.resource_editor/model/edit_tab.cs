using ff.resource_editor.ui;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfTools;

namespace ff.resource_editor.model
{
    internal class edit_tab : PropertyNotifier
    {
        private main_vm main_vm;
        private resource resource_;
        private bool active_;
        private bool dirty_;
        private FrameworkElement root_element_;
        private Lazy<Task<FrameworkElement>> root_element_task;

        protected edit_tab(main_vm main_vm, resource resource, Func<Task<FrameworkElement>> root_element_factory)
        {
            this.main_vm = main_vm;
            this.resource_ = resource;
            this.root_element_task = new(root_element_factory);
        }

        public static edit_tab create(main_vm main_vm, resource resource)
        {
            Func<Task<FrameworkElement>> root_element_factory = () => null;

            switch (resource.type)
            {
                default:
                    root_element_factory = async () => new json_editor(await json_editor_vm.load_async(resource));
                    break;

                case resource_type.texture:
                    root_element_factory = async () => new texture_editor(await texture_editor_vm.load_async(resource));
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
            set => this.SetProperty(ref this.active_, value);
        }

        public bool dirty
        {
            get => this.dirty_;
            set
            {
                if (this.SetProperty(ref this.dirty_, value))
                {
                    if (value)
                    {
                        this.resource.source.dirty = true;
                    }
                }
            }
        }

        public virtual FrameworkElement root_element
        {
            get
            {
                if (this.root_element_ == null && !this.root_element_task.IsValueCreated)
                {
                    this.root_element_task.Value?.ContinueWith((Task<FrameworkElement> task) =>
                    {
                        this.root_element_ = task.Result;
                        this.OnPropertyChanged(nameof(this.root_element));
                    },
                    CancellationToken.None,
                    TaskContinuationOptions.OnlyOnRanToCompletion,
                    TaskScheduler.FromCurrentSynchronizationContext());
                }

                return this.root_element_;
            }
        }

        public ICommand activate_command => new DelegateCommand(() =>
        {
            this.main_vm.active_edit_tab = this;
        });

        public ICommand close_command => new DelegateCommand(() =>
        {
            this.main_vm.close_edit_tab(this);
        });
    }
}
