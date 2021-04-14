using ff.resource_editor.model;
using System.ComponentModel;

namespace ff.resource_editor.ui
{
    internal partial class main_window : System.Windows.Window
    {
        public main_vm view_model { get; }

        public main_window()
        {
            this.view_model = new();
            this.InitializeComponent();
        }

        private void on_closing(object sender, CancelEventArgs args)
        {
            args.Cancel = args.Cancel || !this.view_model.check_dirty();

            if (!args.Cancel)
            {
            }
        }
    }
}
