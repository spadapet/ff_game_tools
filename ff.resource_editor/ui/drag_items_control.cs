using ff.WpfTools;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace ff.resource_editor.ui
{
    /// <summary>
    /// ItemsControl that allows drag/drop of items
    /// </summary>
    internal sealed class drag_items_control : ItemsControl
    {
        public interface drag_host
        {
            void on_drop(ItemsControl source, object dropped_model, int dropped_index, bool copy);
            bool can_drop_copy(object dropped_model);
        }

        private bool dragging;
        private Point? mouse_capture_point;
        private ContentPresenter capture_item;
        private DragItemAdorner drop_adorner;
        private const string single_item_data_format = "drag_single_item";

        public drag_items_control()
        {
            this.AllowDrop = true;
            this.Drop += this.on_drop;
            this.DragOver += this.on_drag_over;
            this.DragLeave += this.on_drag_leave;
        }

        private drag_host host => WpfUtility.FindVisualAncestor<drag_host>(this);

        private void on_drop(object sender, DragEventArgs args)
        {
            this.remove_drop_adorner();

            if (args.Data.GetData(drag_items_control.single_item_data_format) is object dropModel &&
                this.get_drop_target(args, out int index, out bool first_half))
            {
                bool copy =
                    (args.Effects & DragDropEffects.Copy) == DragDropEffects.Copy &&
                    (args.KeyStates & DragDropKeyStates.ControlKey) == DragDropKeyStates.ControlKey;

                if (!first_half)
                {
                    index++;
                }

                if (index >= 0 && index <= this.Items.Count)
                {
                    this.host?.on_drop(this, dropModel, index, copy);
                }
            }
        }

        private void on_drag_over(object sender, DragEventArgs args)
        {
            if (this.get_drop_target(args, out int index, out bool left))
            {
                this.ensure_drop_adorner(this.ItemContainerGenerator.ContainerFromIndex(index) as ContentPresenter, left);
            }
            else
            {
                args.Effects = DragDropEffects.None;
                args.Handled = true;
            }
        }

        private void on_drag_leave(object sender, DragEventArgs args)
        {
            this.remove_drop_adorner();
        }

        private bool get_drop_target(DragEventArgs args, out int index, out bool left)
        {
            ItemContainerGenerator itemGen = this.ItemContainerGenerator;
            ItemCollection items = this.Items;
            Point point = args.GetPosition(this);
            IInputElement hit = this.InputHitTest(point);
            ContentPresenter item = WpfUtility.FindItemContainer<ContentPresenter>(this, hit as DependencyObject);

            if (item == null)
            {
                // Use the last item
                item = this.HasItems
                    ? itemGen.ContainerFromIndex(items.Count - 1) as ContentPresenter
                    : null;
            }

            if (item != null)
            {
                index = itemGen.IndexFromContainer(item);
                Point itemPoint = args.GetPosition(item);
                left = itemPoint.X < item.ActualWidth / 2;
                return true;
            }

            index = 0;
            left = false;
            return false;
        }

        private void ensure_drop_adorner(ContentPresenter item, bool left)
        {
            if (this.drop_adorner == null || this.drop_adorner.AdornedElement != item || left != this.drop_adorner.Left)
            {
                this.remove_drop_adorner();

                if (item != null)
                {
                    this.drop_adorner = new DragItemAdorner(item, left);

                    if (AdornerLayer.GetAdornerLayer(item) is AdornerLayer layer)
                    {
                        layer.Add(this.drop_adorner);
                    }
                }
            }
        }

        private void remove_drop_adorner()
        {
            if (this.drop_adorner != null)
            {
                if (AdornerLayer.GetAdornerLayer(this.drop_adorner.AdornedElement) is AdornerLayer layer)
                {
                    layer.Remove(this.drop_adorner);
                }

                this.drop_adorner = null;
            }
        }

        public void notify_mouse_move(object sender, MouseEventArgs args)
        {
            if (this.mouse_capture_point.HasValue && this.capture_item != null)
            {
                object captureModel = this.ItemContainerGenerator.ItemFromContainer(this.capture_item);
                Point point = args.GetPosition(this);
                if (Math.Abs(this.mouse_capture_point.Value.X - point.X) >= SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(this.mouse_capture_point.Value.Y - point.Y) >= SystemParameters.MinimumVerticalDragDistance)
                {
                    try
                    {
                        if (this.CaptureMouse())
                        {
                            this.dragging = true;
                            bool can_copy = this.host?.can_drop_copy(captureModel) == true;
                            DragDropEffects effects = can_copy
                                ? DragDropEffects.Move | DragDropEffects.Copy
                                : DragDropEffects.Move;

                            DataObject data = new DataObject(drag_items_control.single_item_data_format, captureModel);
                            DragDrop.DoDragDrop(this, data, effects);
                        }
                    }
                    finally
                    {
                        if (this.dragging)
                        {
                            this.ReleaseMouseCapture();
                            this.dragging = false;
                            this.mouse_capture_point = null;
                            this.capture_item = null;
                        }
                    }
                }
            }
        }

        public void notify_mouse_capture(UIElement sender, MouseEventArgs args)
        {
            if (args.MouseDevice.Captured != null)
            {
                if (WpfUtility.FindItemContainer<ContentPresenter>(this, sender) is ContentPresenter item && !this.dragging)
                {
                    this.mouse_capture_point = args.GetPosition(this);
                    this.capture_item = item;
                }
            }
            else
            {
                this.mouse_capture_point = null;
                this.capture_item = null;
            }
        }

        private sealed class DragItemAdorner : Adorner
        {
            public bool Left { get; private set; }

            public DragItemAdorner(UIElement adornedElement, bool left)
                : base(adornedElement)
            {
                this.Left = left;
                this.IsHitTestVisible = false;
            }

            protected override void OnRender(DrawingContext drawing)
            {
                Size size = this.AdornedElement.RenderSize;
                double x = this.Left ? 0.0 : size.Width;

                drawing.DrawRectangle(SystemColors.HighlightBrush, null, new Rect(x - 1.5, 0, 3.0, size.Height));
            }
        }
    }
}
