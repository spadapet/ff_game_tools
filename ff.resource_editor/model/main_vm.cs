using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfTools;

namespace ff.resource_editor.model
{
    internal class main_vm : PropertyNotifier
    {
        private project project_;
        private source_file edit_source_;
        private resource edit_resource_;
        private edit_tab active_edit_tab_;
        private ObservableCollection<edit_tab> edit_tabs_;

        public main_vm()
        {
            this.project_ = new();
            this.edit_tabs_ = new();
        }

        public project project
        {
            get => this.project_;
            set => this.SetProperty(ref this.project_, value ?? new());
        }

        public source_file edit_source
        {
            get => this.edit_source_;
            set
            {
                if (this.SetProperty(ref this.edit_source_, value))
                {
                    this.OnPropertyChanged(nameof(this.has_edit_source));
                    this.remove_source_command_?.UpdateCanExecute();
                }
            }
        }

        public bool has_edit_source => this.edit_source_ != null;

        public resource edit_resource
        {
            get => this.edit_resource_;
            set
            {
                if (this.SetProperty(ref this.edit_resource_, value))
                {
                    this.delete_resource_command_?.UpdateCanExecute();
                }
            }
        }

        public IList<edit_tab> edit_tabs => this.edit_tabs_;

        public edit_tab active_edit_tab
        {
            get => this.active_edit_tab_;
            set
            {
                if (this.active_edit_tab_ != value)
                {
                    if (this.active_edit_tab_ != null)
                    {
                        this.active_edit_tab_.active = false;
                    }

                    this.SetProperty(ref this.active_edit_tab_, value);

                    if (this.active_edit_tab_ != null)
                    {
                        this.active_edit_tab_.active = true;
                    }
                }
            }
        }

        public ICommand new_command => new DelegateCommand(async () =>
        {
            if (await this.check_dirty())
            {
                this.project = new();
            }
        });

        public ICommand open_command => new DelegateCommand(async () =>
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

        public ICommand save_command => new DelegateCommand(async (object parameter) =>
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

        public ICommand save_as_command => new DelegateCommand(async () =>
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

        public ICommand add_source_command => new DelegateCommand(async () =>
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

        internal void on_drop(edit_tab tab, int dropped_index)
        {
            int index = this.edit_tabs_.IndexOf(tab);
            if (index >= 0 && index != dropped_index)
            {
                int final_index = (dropped_index > index) ? dropped_index - 1 : dropped_index;
                this.edit_tabs_.Move(index, final_index);
            }
        }

        public void open_edit_tab(resource resource)
        {
            resource.editor ??= edit_tab.create(this, resource);

            if (!this.edit_tabs_.Contains(resource.editor))
            {
                this.edit_tabs_.Add(resource.editor);
            }

            this.active_edit_tab = resource.editor;
        }

        public bool close_edit_tab(edit_tab editor)
        {
            if (editor != null)
            {
                int i = this.edit_tabs_.IndexOf(editor);

                if (this.active_edit_tab_ == editor)
                {
                    this.active_edit_tab = null;
                }

                editor.resource.editor = null;

                if (this.edit_tabs_.Remove(editor) && this.edit_tabs.Count > 0 && this.active_edit_tab == null)
                {
                    this.active_edit_tab = this.edit_tabs_[i >= 0 && i < this.edit_tabs_.Count ? i : this.edit_tabs_.Count - 1];
                }
            }

            return true;
        }

        private DelegateCommand remove_source_command_;
        public ICommand remove_source_command => this.remove_source_command_ ??= new DelegateCommand((object parameter) =>
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

        private DelegateCommand delete_resource_command_;
        public ICommand delete_resource_command => this.delete_resource_command_ ??= new DelegateCommand((object parameter) =>
        {
        },
        (object parameter) =>
        {
            return parameter is resource;
        });

        public ICommand close_active_tab_command => new DelegateCommand(() =>
        {
            this.close_edit_tab(this.active_edit_tab_);
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
                            Debug.Fail(ex.Message);
                            return false;
                        }
                        break;
                }
            }

            return true;
        }
    }
}
