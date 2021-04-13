namespace ff.wpf_tools
{
    public sealed class delegate_disposable : System.IDisposable
    {
        private readonly System.Action dispose_action;

        public delegate_disposable(System.Action dispose_action)
        {
            this.dispose_action = dispose_action;
        }

        void System.IDisposable.Dispose()
        {
            this.dispose_action?.Invoke();
        }
    }
}
