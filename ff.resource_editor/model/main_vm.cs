using ff.wpf_tools;
using System.Windows;
using System.Windows.Input;

namespace ff.resource_editor.model
{
    internal class main_vm : property_notifier
    {
        private project project_;

        public main_vm()
        {
            this.project_ = new project();
        }
    
        public project project
        {
            get => this.project_;
            set => this.set_property(ref this.project_, value ?? new());
        }

        public ICommand exit_command => new delegate_command(() =>
        {
            Application.Current.MainWindow?.Close();
        });

        public ICommand new_command => new delegate_command(() =>
        {
            if (this.check_dirty())
            {
                this.project = new();
            }
        });

        public ICommand open_command => new delegate_command(() =>
        {
            if (this.check_dirty())
            {
                this.project = new();
            }
        });

        public ICommand save_command => new delegate_command(() =>
        {
        });

        public ICommand add_source_command => new delegate_command(() =>
        {
        });

        public ICommand remove_source_command => new delegate_command(() =>
        {
        });

        private bool check_dirty()
        {
            // TODO: Check if the project is dirty
            return true;
        }
    }
}
