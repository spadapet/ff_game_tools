namespace ff.wpf_tools
{
    public static class wpf_utility
    {
        private static bool? design_mode_;

        public static bool design_mode
        {
            get
            {
                if (!wpf_utility.design_mode_.HasValue)
                {
                    wpf_utility.design_mode_ = System.ComponentModel.DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject());
                }

                return wpf_utility.design_mode_.Value;
            }
        }
    }
}
