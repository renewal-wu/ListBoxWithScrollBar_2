using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;


namespace ListBoxWithScrollBar.Controls
{
    [TemplatePartAttribute(Name = "ItemNavigateSlider", Type = typeof(Slider))]
    public class ListBoxWithScrollBar : ListBox
    {
        Slider itemSlider;

        public ListBoxWithScrollBar()
        {
            DefaultStyleKey = typeof(ListBoxWithScrollBar);
            this.Loaded += ListBoxWithScrollBar_Loaded;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            itemSlider = base.GetTemplateChild("ItemNavigateSlider") as Slider;
            if (itemSlider != null)
            {
                if (this.Items == null)
                {
                    itemSlider.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    itemSlider.Maximum = this.Items.Count - 1;
                    itemSlider.SmallChange = 1.0;
                    itemSlider.LargeChange = 10.0;
                    itemSlider.Value = itemSlider.Maximum;
                    itemSlider.MouseEnter += itemSlider_MouseEnter;
                    itemSlider.MouseLeave += itemSlider_MouseLeave;
                }
            }

            List<ScrollViewer> controlScrollViewerList = GetVisualChildCollection<ScrollViewer>(this);

            if (controlScrollViewerList != null && controlScrollViewerList.Count > 0)
            {
                List<ScrollBar> controlScrollBarList = GetVisualChildCollection<ScrollBar>(controlScrollViewerList.First());

                if (controlScrollBarList == null)
                {
                    return;
                }

                controlScrollBarList.ForEach(scrollBar =>
                {
                    scrollBar.ValueChanged += scrollBar_ValueChanged;
                });
            }

        }

        private void ListBoxWithScrollBar_Loaded(object sender, RoutedEventArgs e)
        {
            List<ScrollViewer> controlScrollViewerList = GetVisualChildCollection<ScrollViewer>(sender);

            if (controlScrollViewerList != null && controlScrollViewerList.Count > 0)
            {
                List<ScrollBar> controlScrollBarList = GetVisualChildCollection<ScrollBar>(controlScrollViewerList.First());

                if (controlScrollBarList != null && controlScrollBarList.Count > 0)
                {
                    controlScrollBarList.ForEach(scrollBar =>
                    {
                        scrollBar.ValueChanged += scrollBar_ValueChanged;
                    });
                }
            }
        }

        private void scrollBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ScrollBar targetScrollBar = sender as ScrollBar;
            if (targetScrollBar != null && targetScrollBar.Maximum > 0 && itemSlider != null)
            {
                double ratio = e.NewValue / targetScrollBar.Maximum;
                itemSlider.Value = itemSlider.Maximum * (1 - ratio);
            }
        }

        private void itemSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider targetSlider = sender as Slider;
            if (targetSlider != null)
            {
                Int32 scrollItemIndex = (Int32)(targetSlider.Maximum - targetSlider.Value);
                if (this.Items.Count >= scrollItemIndex)
                {
                    Object targetItem = this.Items.ElementAt(scrollItemIndex);
                    this.ScrollIntoView(targetItem);
                }
            }
        }

        private void itemSlider_MouseEnter(object sender, RoutedEventArgs e)
        {
            itemSlider.ValueChanged += itemSlider_ValueChanged;
        }

        private void itemSlider_MouseLeave(object sender, RoutedEventArgs e)
        {
            itemSlider.ValueChanged -= itemSlider_ValueChanged;
        }

        private static List<T> GetVisualChildCollection<T>(object parent) where T : UIElement
        {
            List<T> visualCollection = new List<T>();
            GetVisualChildCollection(parent as DependencyObject, visualCollection);
            return visualCollection;
        }

        private static void GetVisualChildCollection<T>(DependencyObject parent, List<T> visualCollection) where T : UIElement
        {
            int count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child is T)
                    visualCollection.Add(child as T);
                else if (child != null)
                    GetVisualChildCollection(child, visualCollection);
            }
        }
    }
}
