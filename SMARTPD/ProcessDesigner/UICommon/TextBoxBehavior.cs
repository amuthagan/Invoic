using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Interactivity;
using ProcessDesigner.Common;

namespace ProcessDesigner.UICommon
{
    public class TextBoxBehavior : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            if (this.AssociatedObject == null)
                throw new InvalidOperationException("AssociatedObject must not be null");

            AssociatedObject.GotFocus += AssociatedObject_GotFocus;
           
        }

        void AssociatedObject_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if (AssociatedObject.Tag.IsNotNullOrEmpty()) StatusMessage.setStatus(AssociatedObject.Tag.ToString());
        }

        protected override void OnDetaching()
        {
            AssociatedObject.GotFocus -= AssociatedObject_GotFocus;
        }
    }
}
