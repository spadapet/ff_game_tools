using ff.resource_editor.model;

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
    }
}
