using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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

        public static T find_visual_ancestor<T>(DependencyObject item, bool include_self = false) where T : class
        {
            if (item is Visual)
            {
                for (DependencyObject parent = item != null ? (include_self ? item : VisualTreeHelper.GetParent(item)) : null;
                    parent != null;
                    parent = VisualTreeHelper.GetParent(parent))
                {
                    if (parent is T typedParent)
                    {
                        return typedParent;
                    }
                }
            }

            return null;
        }

        public static T find_item_container<T>(ItemsControl control, DependencyObject child) where T : DependencyObject
        {
            T parent = null;

            if (control.IsAncestorOf(child))
            {
                parent = wpf_utility.find_visual_ancestor<T>(child, include_self: true);
                while (parent != null && control.ItemContainerGenerator.ItemFromContainer(parent) == DependencyProperty.UnsetValue)
                {
                    parent = wpf_utility.find_visual_ancestor<T>(parent, include_self: false);
                }
            }

            return parent;
        }
    }
}
