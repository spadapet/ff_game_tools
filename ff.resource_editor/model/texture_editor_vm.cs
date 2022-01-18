using Efficient.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfTools;

namespace ff.resource_editor.model
{
    internal class texture_editor_vm : PropertyNotifier
    {
        private model model_;
        private resource resource_;
        private BitmapImage image_;
        private double zoom_;

        private class model
        {
            public string file = default;
            public bool? compress = default;
        }

        private texture_editor_vm(resource resource)
        {
            this.resource_ = resource;
            this.zoom_ = 1.0;
        }

        public static async Task<texture_editor_vm> load_async(resource resource)
        {
            texture_editor_vm value = new(resource);
            await value.load_async();
            return value;

        }

        private async Task load_async()
        {
            if (this.resource_?.value is JsonValue value)
            {
                this.model_ = await Task.Run(() => value.ToObject<model>());
                this.OnPropertyChanged(nameof(this.compress));
                this.OnPropertyChanged(nameof(this.file));


                string file = utility.to_file(this.resource_, this.model_.file);

                if (File.Exists(file))
                {
                    BitmapImage image = new(new Uri(file));
                    this.image_ = image;

                    this.OnPropertyChanged(nameof(this.image_source));
                    this.OnPropertyChanged(nameof(this.image_width));
                    this.OnPropertyChanged(nameof(this.image_height));
                }
            }
        }

        public ImageSource image_source => this.image_;
        public double image_width => (this.image_?.Width ?? 0.0) * this.zoom_;
        public double image_height => (this.image_?.Height ?? 0.0) * this.zoom_;

        public double zoom
        {
            get => this.zoom_;
            set
            {
                value = Math.Floor(Math.Clamp(value, 1.0, 16.0));
                if (this.SetProperty(ref this.zoom_, value))
                {
                    this.OnPropertyChanged(nameof(this.image_width));
                    this.OnPropertyChanged(nameof(this.image_height));
                }
            }
        }

        public bool? compress
        {
            get => this.model_.compress;
            set
            {
                if (this.SetProperty(ref this.model_.compress, value))
                {
                    this.resource_.editor.dirty = true;
                }
            }
        }

        public string file => utility.to_file(this.resource_, this.model_.file);
        public string relative_file => utility.to_relative_file(this.resource_, this.model_.file);

        public ICommand zoom_in_command => new DelegateCommand(() =>
        {
            this.zoom = Math.Floor(this.zoom + 1.0);
        });

        public ICommand zoom_out_command => new DelegateCommand(() =>
        {
            this.zoom = Math.Floor(this.zoom - 1.0);
        });
    }
}
