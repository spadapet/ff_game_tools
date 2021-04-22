using ff.resource_editor.model;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace ff.resource_editor
{
    internal static class utility
    {
        public static void fire_and_forget(this Task task, string task_name)
        {
            task?.ContinueWith((Task t) =>
            {
                Debug.Fail($"Task failed: {task_name}\r\nException: {t.Exception?.Message}");
            }, TaskContinuationOptions.OnlyOnFaulted);
        }

        public static string to_file(resource resource, string value)
        {
            string base_path = resource?.source?.directory;
            if (!string.IsNullOrEmpty(base_path) && value.StartsWith("file:"))
            {
                string sub_path = value.Substring(5);
                if (!string.IsNullOrEmpty(sub_path))
                {
                    return Path.GetFullPath(sub_path, base_path);
                }
            }

            return string.Empty;
        }

        public static string to_relative_file(resource resource, string value)
        {
            if (!string.IsNullOrEmpty(value) && value.StartsWith("file:"))
            {
                return value.Substring(5);
            }

            return string.Empty;
        }
    }
}
