using ff.resource_editor.model;
using System.Windows.Controls;

namespace ff.resource_editor.ui
{
    /// <summary>
    /// Interaction logic for texture_editor.xaml
    /// </summary>
    internal partial class json_editor : UserControl
    {
        public json_editor_vm view_model { get; }

        public json_editor(json_editor_vm view_model)
        {
            this.view_model = view_model;
            this.InitializeComponent();
        }
    }
}
