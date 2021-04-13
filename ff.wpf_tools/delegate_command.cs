namespace ff.wpf_tools
{
    public sealed class delegate_command : property_notifier, System.Windows.Input.ICommand
    {
        private bool? can_execute_;
        private object can_execute_param_;
        private event System.EventHandler can_execute_changed;
        private readonly System.Action<object> execute_action;
        private readonly System.Func<object, bool> can_execute_func;

        public delegate_command(System.Action execute_action, System.Func<bool> can_execute_func = null)
        {
            this.execute_action = (object arg) => execute_action?.Invoke();
            this.can_execute_func = (object arg) => can_execute_func?.Invoke() ?? true;
        }

        public delegate_command(System.Action<object> execute_action = null, System.Func<object, bool> can_execute_func = null)
        {
            this.execute_action = execute_action;
            this.can_execute_func = can_execute_func;
        }

        public void update_can_execute()
        {
            this.can_execute_ = null;
            this.can_execute_param_ = null;
            this.can_execute_changed?.Invoke(this, System.EventArgs.Empty);
            this.on_property_changed(nameof(this.can_execute));
        }

        public bool can_execute => this.can_execute_param(null);

        public bool can_execute_param(object parameter)
        {
            if (this.can_execute_ == null || !object.Equals(this.can_execute_param_, parameter))
            {
                this.can_execute_param_ = parameter;
                this.can_execute_ = this.can_execute_func?.Invoke(parameter) ?? true;
            }

            return this.can_execute_ == true;
        }

        public void execute()
        {
            this.execute_param(null);
        }

        public void execute_param(object parameter)
        {
            this.execute_action?.Invoke(parameter);
        }

        event System.EventHandler System.Windows.Input.ICommand.CanExecuteChanged
        {
            add => this.can_execute_changed += value;
            remove => this.can_execute_changed -= value;
        }

        bool System.Windows.Input.ICommand.CanExecute(object parameter)
        {
            return this.can_execute_param(parameter);
        }

        void System.Windows.Input.ICommand.Execute(object parameter)
        {
            this.execute_param(parameter);
        }
    }
}
