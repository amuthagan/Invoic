using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;

namespace ProcessDesigner.UICommon
{
    public static class FocusExtension
    {
        public static readonly DependencyProperty IsFocusedProperty =
            DependencyProperty.RegisterAttached("IsFocused", typeof(bool?), typeof(FocusExtension), new FrameworkPropertyMetadata(IsFocusedChanged) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });

        public static bool? GetIsFocused(DependencyObject element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            return (bool?)element.GetValue(IsFocusedProperty);
        }

        public static void SetIsFocused(DependencyObject element, bool? value)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            element.SetValue(IsFocusedProperty, value);
        }

        private static void IsFocusedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var fe = (FrameworkElement)d;

            if (e.OldValue == null)
            {
                fe.GotFocus += FrameworkElement_GotFocus;
                fe.LostFocus += FrameworkElement_LostFocus;
            }

            if (!fe.IsVisible)
            {
                fe.IsVisibleChanged += new DependencyPropertyChangedEventHandler(fe_IsVisibleChanged);
            }

            if ((bool)e.NewValue)
            {
                ProcessDesigner.UserControls.ComboBoxCus combo = fe as ProcessDesigner.UserControls.ComboBoxCus;
                ProcessDesigner.UserControls.ComboBoxWithSearch combosearch = fe as ProcessDesigner.UserControls.ComboBoxWithSearch;
                ProcessDesigner.UserControls.DatePickerCus datepic = fe as ProcessDesigner.UserControls.DatePickerCus;
                if (combo != null)
                {
                    ((System.Windows.Controls.TextBox)combo.FindName("txtCombobox")).Focus();
                }
                else if (combosearch != null)
                {
                    ((System.Windows.Controls.TextBox)combosearch.FindName("txtCombobox")).Focus();
                }
                else if (datepic != null)
                {
                    ((System.Windows.Controls.TextBox)datepic.FindName("txtDate")).Focus();
                }
                else
                {
                    fe.Focus();
                }
            }
        }

        private static void fe_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var fe = (FrameworkElement)sender;
            if (fe.IsVisible && (bool)((FrameworkElement)sender).GetValue(IsFocusedProperty))
            {
                fe.IsVisibleChanged -= fe_IsVisibleChanged;
                fe.Focus();
            }
        }

        private static void FrameworkElement_GotFocus(object sender, RoutedEventArgs e)
        {
            ((FrameworkElement)sender).SetValue(IsFocusedProperty, true);
        }

        private static void FrameworkElement_LostFocus(object sender, RoutedEventArgs e)
        {
            ((FrameworkElement)sender).SetValue(IsFocusedProperty, false);
        }
    }

    //public static class FocusExtension
    //{
    //    public static bool GetIsFocused(DependencyObject obj)
    //    {
    //        return (bool)obj.GetValue(IsFocusedProperty);
    //    }


    //    public static void SetIsFocused(DependencyObject obj, bool value)
    //    {
    //        obj.SetValue(IsFocusedProperty, value);
    //    }


    //    public static readonly DependencyProperty IsFocusedProperty =
    //        DependencyProperty.RegisterAttached(
    //         "IsFocused", typeof(bool), typeof(FocusExtension),
    //         new UIPropertyMetadata(false, OnIsFocusedPropertyChanged));


    //    private static void OnIsFocusedPropertyChanged(DependencyObject d,
    //        DependencyPropertyChangedEventArgs e)
    //    {
    //        var uie = (UIElement)d;
    //        //if ((bool)e.NewValue)
    //        //{
    //        //    uie.Focus(); // Don't care about false values.
    //        //}
    //        if ((bool)e.NewValue && uie.Dispatcher != null)
    //        {
    //            uie.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() => uie.Focus())); // invoke behaves nicer, if e.g. you have some additional handler attached to 'GotFocus' of UIE. 
    //            uie.SetValue(IsFocusedProperty, false); // reset bound value if possible, to allow setting again ...
    //        }
    //    }
    //}
}
