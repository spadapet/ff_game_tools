using ff.resource_editor.model;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ff.resource_editor.ui
{
    internal partial class main_window : System.Windows.Window
    {
        public main_vm view_model { get; }
        private bool allow_close;
        private bool saving_on_close;

        public main_window()
        {
            this.view_model = new();
            this.InitializeComponent();
        }

        /// <summary>
        /// Restore the state of the app from the last run
        /// </summary>
        private async void on_loaded(object sender, RoutedEventArgs args)
        {
            try
            {
                if (File.Exists(app_state.state_file))
                {
                    string state_json = await File.ReadAllTextAsync(app_state.state_file, Encoding.UTF8);

                    if (!string.IsNullOrEmpty(state_json))
                    {
                        app_state state = await Task.Run(() => Efficient.Json.JsonValue.StringToObject<app_state>(state_json));
                        await this.restore_from_state_async(state);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Save the state of the app for the next run
        /// </summary>
        private async void on_closing(object sender, CancelEventArgs args)
        {
            if (!this.allow_close)
            {
                args.Cancel = true;

                if (!this.saving_on_close)
                {
                    try
                    {
                        this.saving_on_close = true;

                        if (await this.view_model.check_dirty())
                        {
                            app_state state = this.current_state;
                            await this.save_state_async(state);
                            this.allow_close = true;
                        }
                    }
                    finally
                    {
                        this.saving_on_close = false;
                    }

                    if (this.allow_close)
                    {
                        this.Close();
                    }
                }
            }
        }

        private app_state current_state
        {
            get
            {
                app_state state = new()
                {
                    project_file = this.view_model.project.file
                };

                return state;
            }
        }

        private async Task restore_from_state_async(app_state state)
        {
            try
            {
                if (!string.IsNullOrEmpty(state.project_file) && File.Exists(state.project_file))
                {
                    project project = await project.load_async(state.project_file);
                    this.view_model.project = project;
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message);
            }
        }

        private async Task save_state_async(app_state state)
        {
            string state_json = await Task.Run(() => Efficient.Json.JsonValue.ObjectToString(state, true));
            try
            {
                await Task.Run(() => Directory.CreateDirectory(Path.GetDirectoryName(app_state.state_file)));
                await File.WriteAllTextAsync(app_state.state_file, state_json, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message);
            }
        }

        private void on_add_resource_click(object sender, RoutedEventArgs args)
        {
            ((Button)sender).ContextMenu.IsOpen = true;
        }

        private void resource_list_item_mouse_double_click(object sender, MouseButtonEventArgs args)
        {
            resource resource = (resource)((FrameworkElement)sender).DataContext;
            this.view_model.open_edit_tab(resource);
        }
    }
}
