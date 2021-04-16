using System.Windows;
using System.Windows.Media;

namespace ff.resource_editor.ui
{
    internal static class properties
    {
        public static readonly DependencyProperty IconProperty = DependencyProperty.RegisterAttached(
            "Icon", typeof(Brush), typeof(properties), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));
        public static void SetIcon(UIElement element, Brush value) => element.SetValue(properties.IconProperty, value);
        public static Brush GetIcon(UIElement element) => (Brush)element.GetValue(properties.IconProperty);

        public static readonly DependencyProperty IconHotProperty = DependencyProperty.RegisterAttached(
            "IconHot", typeof(Brush), typeof(properties), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));
        public static void SetIconHot(UIElement element, Brush value) => element.SetValue(properties.IconHotProperty, value);
        public static Brush GetIconHot(UIElement element) => (Brush)element.GetValue(properties.IconHotProperty);

        public static readonly DependencyProperty IconGrayProperty = DependencyProperty.RegisterAttached(
            "IconGray", typeof(Brush), typeof(properties), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));
        public static void SetIconGray(UIElement element, Brush value) => element.SetValue(properties.IconGrayProperty, value);
        public static Brush GetIconGray(UIElement element) => (Brush)element.GetValue(properties.IconGrayProperty);
    }
}
