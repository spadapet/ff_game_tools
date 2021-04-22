namespace ff.WpfTools
{
    public sealed class DelegateDisposable : System.IDisposable
    {
        private readonly System.Action disposeAction;

        public DelegateDisposable(System.Action disposeAction)
        {
            this.disposeAction = disposeAction;
        }

        void System.IDisposable.Dispose()
        {
            this.disposeAction?.Invoke();
        }
    }
}
