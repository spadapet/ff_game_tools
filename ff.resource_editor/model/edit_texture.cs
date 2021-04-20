using Efficient.Json;
using ff.wpf_tools;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ff.resource_editor.model
{
    internal class edit_texture : property_notifier
    {
        private resource resource_;
        private BitmapImage image_;
        private double zoom_;

        public edit_texture()
            : this(null)
        { }

        public edit_texture(resource resource)
        {
            this.resource_ = resource;
            this.zoom_ = 1.0;
            this.load_async().fire_and_forget("texture load");
        }

        private async Task load_async()
        {
            if (this.resource_?.value is JsonValue value && value["file"] is JsonValue file_value && file_value.IsString)
            {
                string file = file_value.String;
                if (file.StartsWith("file:"))
                {
                    file = Path.GetFullPath(file.Substring(5), this.resource_.source.directory);
                    if (File.Exists(file))
                    {
                        BitmapImage image = new(new Uri(file));
                        this.image_ = image;

                        this.on_property_changed(nameof(this.image_source));
                        this.on_property_changed(nameof(this.image_width));
                        this.on_property_changed(nameof(this.image_height));
                    }
                }
            }

            await Task.CompletedTask;
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
                if (this.set_property(ref this.zoom_, value))
                {
                    this.on_property_changed(nameof(this.image_width));
                    this.on_property_changed(nameof(this.image_height));
                }
            }
        }

        public ICommand zoom_in_command => new delegate_command(() =>
        {
            this.zoom = Math.Floor(this.zoom + 1.0);
        });

        public ICommand zoom_out_command => new delegate_command(() =>
        {
            this.zoom = Math.Floor(this.zoom - 1.0);
        });
    }
}
