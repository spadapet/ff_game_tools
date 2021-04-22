using ff.resource_editor.model;
using System.Windows.Controls;
using System.Windows.Input;

namespace ff.resource_editor.ui
{
    /// <summary>
    /// Interaction logic for texture_editor.xaml
    /// </summary>
    internal partial class texture_editor : UserControl
    {
        public texture_editor_vm view_model { get; }
        private int wheel_scroll;

        public texture_editor(texture_editor_vm view_model)
        {
            this.view_model = view_model;
            this.InitializeComponent();
        }

        private void on_mouse_wheel(object sender, MouseWheelEventArgs args)
        {
            this.wheel_scroll += args.Delta;

            while (this.wheel_scroll >= Mouse.MouseWheelDeltaForOneLine)
            {
                this.wheel_scroll -= Mouse.MouseWheelDeltaForOneLine;
                this.view_model.zoom_in_command.Execute(null);
            }

            while (this.wheel_scroll <= -Mouse.MouseWheelDeltaForOneLine)
            {
                this.wheel_scroll += Mouse.MouseWheelDeltaForOneLine;
                this.view_model.zoom_out_command.Execute(null);
            }
        }
    }
}
