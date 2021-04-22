using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ff.WpfTools
{
    public static class WpfUtility
    {
        private static Lazy<bool> designMode = new Lazy<bool>(() => DesignerProperties.GetIsInDesignMode(new DependencyObject()));

        public static bool DesignMode => WpfUtility.designMode.Value;

        public static T FindVisualAncestor<T>(DependencyObject item, bool include_self = false) where T : class
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

        public static T FindItemContainer<T>(ItemsControl control, DependencyObject child) where T : DependencyObject
        {
            T parent = null;

            if (control.IsAncestorOf(child))
            {
                parent = WpfUtility.FindVisualAncestor<T>(child, include_self: true);
                while (parent != null && control.ItemContainerGenerator.ItemFromContainer(parent) == DependencyProperty.UnsetValue)
                {
                    parent = WpfUtility.FindVisualAncestor<T>(parent, include_self: false);
                }
            }

            return parent;
        }
    }
}
