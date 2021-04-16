using System.Diagnostics;
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
    }
}
