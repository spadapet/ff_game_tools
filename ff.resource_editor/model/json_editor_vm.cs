using Efficient.Json;
using ff.WpfTools;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ff.resource_editor.model
{
    internal class json_editor_vm : PropertyNotifier
    {
        private resource resource_;
        private JsonValue value_;
        private string text_;
        private string error_message_;

        private json_editor_vm(resource resource)
        {
            this.resource_ = resource;
            this.value_ = resource.value;
            this.text_ = string.Empty;
            this.error_message_ = string.Empty;
        }

        public static async Task<json_editor_vm> load_async(resource resource)
        {
            json_editor_vm value = new(resource);
            await value.load_async();
            return value;

        }

        private async Task load_async()
        {
            this.text_ = await Task.Run(() => this.value_?.ToString(formatted: true) ?? string.Empty);
        }

        public string text
        {
            get => this.text_;
            set
            {
                string text = value;

                if (this.SetProperty(ref this.text_, value))
                {
                    Task.Run<JsonValue>(() => JsonValue.StringToValue(text)).ContinueWith(
                        (Task<JsonValue> task) =>
                        {
                            if (task.IsFaulted)
                            {
                                this.error_message = task.Exception.Message;
                            }
                            else if (task.IsCompleted)
                            {
                                if (text == this.text)
                                {
                                    this.value_ = task.Result;
                                    this.error_message = string.Empty;
                                }
                            }
                        },
                        CancellationToken.None,
                        TaskContinuationOptions.NotOnCanceled,
                        TaskScheduler.FromCurrentSynchronizationContext());
                }
            }
        }

        public bool error => !string.IsNullOrEmpty(this.error_message);

        public string error_message
        {
            get => this.error_message_;
            set
            {
                if (this.SetProperty(ref this.error_message_, value))
                {
                    this.OnPropertyChanged(nameof(this.error));
                }
            }
        }
    }
}
