using System.Windows;
using System.Windows.Input;

namespace ff.resource_editor.model
{
    internal class main_vm : ff.wpf_tools.property_notifier
    {
        private project project_;

        public main_vm()
        {
        }

        public project project
        {
            get => this.project_;
            set => this.set_property(ref this.project_, value ?? new());
        }

        public ICommand exit_command => new ff.wpf_tools.delegate_command(() =>
        {
            Application.Current.MainWindow?.Close();
        });
    }
}
