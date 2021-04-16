using ff.wpf_tools;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ff.resource_editor.model
{
    internal class main_vm : property_notifier
    {
        private project project_;
        private source_file edit_source_;
        private resource edit_resource_;

        public main_vm()
        {
            this.project_ = new project();
        }

        public project project
        {
            get => this.project_;
            set => this.set_property(ref this.project_, value ?? new());
        }

        public source_file edit_source
        {
            get => this.edit_source_;
            set
            {
                if (this.set_property(ref this.edit_source_, value))
                {
                    this.remove_source_command_?.update_can_execute();
                }
            }
        }

        public resource edit_resource
        {
            get => this.edit_resource_;
            set
            {
                if (this.set_property(ref this.edit_resource_, value))
                {
                    this.delete_resource_command_?.update_can_execute();
                }
            }
        }

        public ICommand new_command => new delegate_command(async () =>
        {
            if (await this.check_dirty())
            {
                this.project = new();
            }
        });

        public ICommand open_command => new delegate_command(async () =>
        {
            if (await this.check_dirty())
            {
                OpenFileDialog dialog = new OpenFileDialog
                {
                    Title = "Open project",
                    Filter = $"Resource Project|*.res_project.json",
                    DefaultExt = ".res_project.json",
                };

                if (dialog.ShowDialog(Application.Current.MainWindow) == true)
                {
                    try
                    {
                        this.project = await project.load_async(dialog.FileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Open project failed", MessageBoxButton.OK);
                    }
                }
            }
        });

        public ICommand save_command => new delegate_command(async (object parameter) =>
        {
            if (this.project.has_file)
            {
                try
                {
                    await this.project.save_async();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Save project failed", MessageBoxButton.OK);
                }
            }
            else
            {
                this.save_as_command.Execute(parameter);
            }
        });

        public ICommand save_as_command => new delegate_command(async () =>
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                Title = "Save project as",
                Filter = $"Resource Project|*.res_project.json",
                DefaultExt = ".res_project.json",
            };

            if (this.project.has_file)
            {
                dialog.InitialDirectory = Path.GetDirectoryName(this.project.file);
                dialog.FileName = Path.GetFileName(this.project.file);
            }

            if (dialog.ShowDialog(Application.Current.MainWindow) == true)
            {
                try
                {
                    await this.project.save_async(dialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Save project failed", MessageBoxButton.OK);
                }
            }
        });

        public ICommand add_source_command => new delegate_command(async () =>
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Title = "Add source file",
                Filter = $"Game Resources|*.res.json",
                DefaultExt = ".res.json",
                Multiselect = true,
            };

            if (dialog.ShowDialog(Application.Current.MainWindow) == true)
            {
                foreach (string file in dialog.FileNames)
                {
                    source_file source = await source_file.load_async(file);
                    if (!this.project.sources.Contains(source))
                    {
                        this.project.sources.Add(source);
                    }
                }
            }
        });

        private delegate_command remove_source_command_;
        public ICommand remove_source_command => this.remove_source_command_ ??= new delegate_command((object parameter) =>
        {
            if (parameter is IEnumerable sources)
            {
                foreach (source_file source in sources.OfType<source_file>().ToArray())
                {
                    bool removed = this.project.sources.Remove(source);
                    Debug.Assert(removed);
                }
            }
        },
        (object parameter) =>
        {
            return parameter is source_file;
        });

        private delegate_command delete_resource_command_;
        public ICommand delete_resource_command => this.delete_resource_command_ ??= new delegate_command((object parameter) =>
        {
        },
        (object parameter) =>
        {
            return parameter is resource;
        });

        public async Task<bool> check_dirty()
        {
            if (this.project.dirty)
            {
                switch (MessageBox.Show("Save changes?", "Resource Editor", MessageBoxButton.YesNoCancel))
                {
                    case MessageBoxResult.Cancel:
                        return false;

                    case MessageBoxResult.Yes:
                        try
                        {
                            await this.project.save_async();
                        }
                        catch (Exception ex)
                        {
                            return false;
                        }
                        break;
                }
            }

            return true;
        }
    }
}
