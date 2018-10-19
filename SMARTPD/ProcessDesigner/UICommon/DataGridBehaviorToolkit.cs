using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;
using Microsoft.Windows.Controls;
using System.Windows.Interactivity;

namespace ProcessDesigner.UICommon
{
    internal class DataGridBehaviorToolkit : Behavior<DataGrid>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            if (this.AssociatedObject == null)
                throw new InvalidOperationException("AssociatedObject must not be null");

            AssociatedObject.BeginningEdit += AssociatedObject_BeginningEdit;
            AssociatedObject.CellEditEnding += AssociatedObject_CellEditEnding;
        }

        private void AssociatedObject_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            //var isReadOnlyRow = ReadOnlyService.GetIsReadOnly(e.Row);
            //if (isReadOnlyRow)
            //    e.Cancel = true;
        }

        private void AssociatedObject_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Column.GetType() == typeof(DataGridTemplateColumn))
            {
                var popup = GetVisualChild<System.Windows.Controls.Primitives.Popup>(e.EditingElement);
                if (popup != null && popup.IsOpen)
                {
                    e.Cancel = true;
                }
            }
        }

        protected override void OnDetaching()
        {
            AssociatedObject.BeginningEdit -= AssociatedObject_BeginningEdit;
            AssociatedObject.CellEditEnding -= AssociatedObject_CellEditEnding;
        }

        private static T GetVisualChild<T>(DependencyObject visual) where T : DependencyObject
        {
            if (visual == null)
                return null;

            var count = System.Windows.Media.VisualTreeHelper.GetChildrenCount(visual);
            for (int i = 0; i < count; i++)
            {
                var child = System.Windows.Media.VisualTreeHelper.GetChild(visual, i);

                var childOfTypeT = child as T ?? GetVisualChild<T>(child);
                if (childOfTypeT != null)
                    return childOfTypeT;
            }

            return null;
        }
    }
}