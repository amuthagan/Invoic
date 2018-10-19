using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Text.RegularExpressions;

namespace ProcessDesigner.UserControls
{
    #region Documentation Tags
    /// <summary>
    ///     WPF Maskable TextBox class. Just specify the TextBoxIntsOnly.Mask attached property to a TextBox. 
    ///     It protect your TextBox from unwanted non numeric symbols and make it easy to modify your numbers.
    /// </summary>
    /// <remarks>
    /// <para>
    ///     Class Information:
    ///     <list type="bullet">
    ///         <item name="authors">Authors: Ruben Hakopian</item>
    ///         <item name="date">February 2009</item>
    ///         <item name="originalURL">http://www.rubenhak.com/?p=8</item>
    ///     </list>
    /// </para>
    /// </remarks>
    #endregion
    public class TextBoxIntsOnly : TextBox
    {
        #region MinimumValue Property

        public static double GetMinimumValue(DependencyObject obj)
        {
            return (double)obj.GetValue(MinimumValueProperty);
        }

        public static void SetMinimumValue(DependencyObject obj, double value)
        {
            obj.SetValue(MinimumValueProperty, value);
        }

        public static readonly DependencyProperty MinimumValueProperty =
            DependencyProperty.RegisterAttached(
                "MinimumValue",
                typeof(double),
                typeof(TextBoxIntsOnly),
                new FrameworkPropertyMetadata(double.NaN, MinimumValueChangedCallback));

        private static void MinimumValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TextBox _this = (d as TextBox);
            ValidateTextBox(_this);
        }
        #endregion

        #region MaximumValue Property

        public static double GetMaximumValue(DependencyObject obj)
        {
            return (double)obj.GetValue(MaximumValueProperty);
        }


        #region MaxLength Property

        public static int GetMaxLength(DependencyObject obj)
        {
            return (int)obj.GetValue(MaxLengthProperty);
        }

        public static void SetMaxLength(DependencyObject obj, int value)
        {
            obj.SetValue(MaxLengthProperty, value);
        }

        public static readonly DependencyProperty MaxLengthProperty =
            DependencyProperty.RegisterAttached(
                "MaxLength",
                typeof(int),
                typeof(TextBoxIntsOnly),
                new FrameworkPropertyMetadata(MaxLengthChangedCallback));

        private static void MaxLengthChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TextBox _this = (d as TextBox);
            ValidateTextBox(_this);
        }
        #endregion


        public static void SetMaximumValue(DependencyObject obj, double value)
        {
            obj.SetValue(MaximumValueProperty, value);
        }


        public static readonly DependencyProperty IsFocusedProperty =
           DependencyProperty.Register(
               "IsFocused",
               typeof(bool),
               typeof(TextBoxIntsOnly),
               new PropertyMetadata(false));

        public bool IsFocused
        {
            get { return (bool)GetValue(IsFocusedProperty); }
            set { SetValue(IsFocusedProperty, value); }
        }

        public static readonly DependencyProperty MaximumValueProperty =
            DependencyProperty.RegisterAttached(
                "MaximumValue",
                typeof(double),
                typeof(TextBoxIntsOnly),
                new FrameworkPropertyMetadata(double.NaN, MaximumValueChangedCallback));

        private static void MaximumValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TextBox _this = (d as TextBox);
            ValidateTextBox(_this);
        }
        #endregion

        #region Mask Property

        public static MaskType GetMask(DependencyObject obj)
        {
            return (MaskType)obj.GetValue(MaskProperty);
        }

        public static void SetMask(DependencyObject obj, MaskType value)
        {
            obj.SetValue(MaskProperty, value);
        }




        static TextBoxIntsOnly()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBoxIntsOnly), new FrameworkPropertyMetadata(typeof(TextBoxIntsOnly)));

        }


        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            if (e.Key == Key.Back)
            {
                if (Text.Length == 5)
                {
                    Text = Text.Replace(",", "");
                    SelectionLength = 1;
                    SelectionStart += Text.Length;

                }
            }
        }



        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            //  var ue = d as FrameworkElement;
            //    ue.PreviewKeyDown += ue_PreviewKeyDown;

            //this.PreviewKeyDown += OnPreviewKeyDownvalue;


        }

        public static readonly DependencyProperty MaskProperty =
            DependencyProperty.RegisterAttached(
                "Mask",
                typeof(MaskType),
                typeof(TextBoxIntsOnly),
                new FrameworkPropertyMetadata(MaskChangedCallback));

        private static void MaskChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is TextBox)
            {
                (e.OldValue as TextBox).PreviewTextInput -= TextBox_PreviewTextInput;
                DataObject.RemovePastingHandler((e.OldValue as TextBox), (DataObjectPastingEventHandler)TextBoxPastingEventHandler);
            }

            TextBox _this = (d as TextBox);
            if (_this == null)
                return;

            if ((MaskType)e.NewValue != MaskType.Any)
            {
                _this.PreviewTextInput += TextBox_PreviewTextInput;
                DataObject.AddPastingHandler(_this, (DataObjectPastingEventHandler)TextBoxPastingEventHandler);
            }
            if ((MaskType)e.NewValue != MaskType.Decimal)
            {
                // var ue = d as FrameworkElement;
                // ue.PreviewKeyDown += ue_PreviewKeyDown;
            }

            var ue = d as FrameworkElement;
            ue.PreviewKeyDown += ue_PreviewKeyDown;

            ValidateTextBox(_this);
        }

        public static void ue_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            int select = 0;

            if (e.Key == Key.Back || e.Key == Key.Delete)
            {
                select = tb.SelectionStart;
                if (select == 0)
                {
                    e.Handled = false;
                    return;
                }

                if (select > tb.Text.ToString().Length - 1)
                {
                    e.Handled = false;
                    return;
                }

                if (e.Key == Key.Back)
                {
                    select = select - 1;
                }
                else if (e.Key == Key.Delete)
                {
                    select = select;
                }
                string indexstring = string.Empty;
                indexstring = tb.Text.ToString()[select].ToString();
                if (indexstring == ".")
                {
                    e.Handled = true;
                }

            }

            switch (GetMask(tb))
            {
                case MaskType.Integer: if (e.Key == Key.Space) e.Handled = true; break;
                case MaskType.Decimal: if (e.Key == Key.Space) e.Handled = true; break;
                case MaskType.Numeric: break;
                case MaskType.UnSignedDec: if (e.Key == Key.Space) e.Handled = true; break;
                case MaskType.UnSignedInt: if (e.Key == Key.Space) e.Handled = true; break;
                case MaskType.Date: if (e.Key == Key.Space) e.Handled = true; break;
                case MaskType.Delcimalvalues: if (e.Key == Key.Space) e.Handled = true; break;
            }

            //var ue = e.OriginalSource as FrameworkElement;

            //if (e.Key == Key.Enter)
            //{
            //    e.Handled = true;
            //    ue.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            //}
        }

        #endregion

        #region Private Static Methods

        private static void ValidateTextBox(TextBox _this)
        {
            if (GetMask(_this) != MaskType.Any)
            {
                _this.Text = ValidateValue(GetMask(_this), _this.Text, GetMinimumValue(_this), GetMaximumValue(_this));
            }
        }

        private static void TextBoxPastingEventHandler(object sender, DataObjectPastingEventArgs e)
        {
            TextBox _this = (sender as TextBox);
            string clipboard = e.DataObject.GetData(typeof(string)) as string;
            clipboard = ValidateValue(GetMask(_this), clipboard, GetMinimumValue(_this), GetMaximumValue(_this));
            if (!string.IsNullOrEmpty(clipboard))
            {
                _this.Text = clipboard;
            }
            e.CancelCommand();
            e.Handled = true;
        }

        protected override void OnPreviewKeyUp(KeyEventArgs e)
        {
            base.OnPreviewKeyUp(e);
        }


        private static void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            TextBox _this = (sender as TextBox);

            //   MessageBox.Show(_this.Parent.ToString());

            int textLength = _this.MaxLength;
            if (_this.Text.ToString().Length > textLength)
            {
                e.Handled = false;
                return;
            }

            bool isValid = IsSymbolValid(GetMask(_this), e.Text);
            e.Handled = !isValid;


            if (GetMask(_this).ToString() != "Numeric" || GetMask(_this).ToString() != "Date")
            {
                if (e.Text == "0")
                {
                    if (_this.Text.ToString().Length == 0 || _this.SelectionStart == 0)
                    {
                        //   e.Handled = true;
                        // return;
                    }
                }
            }

            if (GetMask(_this).ToString() == "Decimal")
            {
                if (isValid)
                {
                    int caret = _this.CaretIndex;
                    string text = _this.Text;
                    bool textInserted = false;
                    int selectionLength = 0;

                    if (_this.SelectionLength > 0)
                    {
                        text = text.Substring(0, _this.SelectionStart) +
                                text.Substring(_this.SelectionStart + _this.SelectionLength);
                        caret = _this.SelectionStart;
                    }

                    if (e.Text == NumberFormatInfo.CurrentInfo.NumberDecimalSeparator)
                    {
                        while (true)
                        {
                            int ind = text.IndexOf(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
                            if (ind == -1)
                                break;

                            text = text.Substring(0, ind) + text.Substring(ind + 1);
                            if (caret > ind)
                                caret--;
                        }

                        if (caret == 0)
                        {
                            text = "0" + text;
                            caret++;
                        }
                        else
                        {
                            if (caret == 1 && string.Empty + text[0] == NumberFormatInfo.CurrentInfo.NegativeSign)
                            {
                                text = NumberFormatInfo.CurrentInfo.NegativeSign + "0" + text.Substring(1);
                                caret++;
                            }
                        }

                        if (caret == text.Length)
                        {
                            selectionLength = 1;
                            textInserted = true;
                            text = text + NumberFormatInfo.CurrentInfo.NumberDecimalSeparator + "0";
                            caret++;
                        }
                    }
                    else if (e.Text == NumberFormatInfo.CurrentInfo.NegativeSign)
                    {
                        textInserted = true;
                        if (_this.Text.Contains(NumberFormatInfo.CurrentInfo.NegativeSign))
                        {
                            text = text.Replace(NumberFormatInfo.CurrentInfo.NegativeSign, string.Empty);
                            if (caret != 0)
                                caret--;
                        }
                        else
                        {
                            text = NumberFormatInfo.CurrentInfo.NegativeSign + _this.Text;
                            caret++;
                        }
                    }

                    if (!textInserted)
                    {
                        text = text.Substring(0, caret) + e.Text +
                            ((caret < _this.Text.Length) ? text.Substring(caret) : string.Empty);

                        caret++;
                    }

                    try
                    {
                        double val = Convert.ToDouble(text);
                        double newVal = ValidateLimits(GetMinimumValue(_this), GetMaximumValue(_this), val);
                        if (val != newVal)
                        {
                            text = newVal.ToString();
                        }
                        else if (val == 0)
                        {
                            if (!text.Contains(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator))
                                text = "0";
                        }
                    }
                    catch
                    {
                        text = "0";
                    }

                    while (text.Length > 1 && text[0] == '0' && string.Empty + text[1] != NumberFormatInfo.CurrentInfo.NumberDecimalSeparator)
                    {
                        text = text.Substring(1);
                        if (caret > 0)
                            caret--;
                    }

                    while (text.Length > 2 && string.Empty + text[0] == NumberFormatInfo.CurrentInfo.NegativeSign && text[1] == '0' && string.Empty + text[2] != NumberFormatInfo.CurrentInfo.NumberDecimalSeparator)
                    {
                        text = NumberFormatInfo.CurrentInfo.NegativeSign + text.Substring(2);
                        if (caret > 1)
                            caret--;
                    }

                    if (caret > text.Length)
                        caret = text.Length;

                    _this.Text = text;
                    _this.CaretIndex = caret;
                    _this.SelectionStart = caret;
                    _this.SelectionLength = selectionLength;
                    e.Handled = true;
                }
            }


        }

        //private static string ValidateValue(MaskType mask, string value, double min, double max)
        //{
        //    if (string.IsNullOrEmpty(value))
        //        return string.Empty;

        //    value = value.Trim();
        //    switch (mask)
        //    {
        //        case MaskType.Integer:
        //            try
        //            {
        //                Convert.ToInt64(value);
        //                return value;
        //            }
        //            catch
        //            {
        //            }
        //            return string.Empty;

        //        case MaskType.Decimal:
        //            try
        //            {
        //                Convert.ToDouble(value);

        //                return value;
        //            }
        //            catch
        //            {
        //            }
        //            return string.Empty;
        //    }

        //    return value;
        //}
        private static string ValidateValue(MaskType mask, string value, double min, double max)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            value = value.Trim();
            switch (mask)
            {
                case MaskType.Integer:
                    try
                    {
                        Convert.ToInt64(value);
                        return value;
                    }
                    catch
                    {
                    }
                    return string.Empty;

                case MaskType.Decimal:
                    try
                    {
                        Convert.ToDouble(value);

                        return value;
                    }
                    catch
                    {
                    }
                    return string.Empty;
                case MaskType.Numeric:
                    try
                    {
                        Convert.ToString(value);

                        return value;
                    }
                    catch
                    {
                    }
                    return string.Empty;

                case MaskType.UnSignedDec:
                    try
                    {
                        Convert.ToDouble(value);

                        return value;
                    }
                    catch
                    {
                    }
                    return string.Empty;

                case MaskType.UnSignedInt:
                    try
                    {
                        Convert.ToUInt64(value);
                        return value;
                    }
                    catch
                    {
                    }
                    return string.Empty;
                case MaskType.Date:
                    try
                    {
                        Convert.ToUInt64(value);
                        return value;
                    }
                    catch
                    {
                    }
                    return string.Empty;
                case MaskType.Delcimalvalues:
                    try
                    {
                        Convert.ToDouble(value);

                        return value;
                    }
                    catch
                    {
                    }
                    return string.Empty;
            }

            return value;
        }

        private static double ValidateLimits(double min, double max, double value)
        {
            if (!min.Equals(double.NaN))
            {
                if (value < min)
                    return min;
            }

            if (!max.Equals(double.NaN))
            {
                string[] decimalval = value.ToString().Split('.');
                value = Convert.ToDouble(decimalval[0].ToString());

                if (value > max)
                    return max;
            }



            return value;
        }

        //private static bool IsSymbolValid(MaskType mask, string str)
        //{
        //    switch (mask)
        //    {
        //        case MaskType.Any:
        //            return true;

        //        case MaskType.Integer:
        //            if (str == NumberFormatInfo.CurrentInfo.NegativeSign)
        //                return true;
        //            break;

        //        case MaskType.Decimal:
        //            if (str == NumberFormatInfo.CurrentInfo.NumberDecimalSeparator ||
        //                str == NumberFormatInfo.CurrentInfo.NegativeSign)
        //                return true;
        //            break;
        //    }

        //    if (mask.Equals(MaskType.Integer) || mask.Equals(MaskType.Decimal))
        //    {
        //        foreach (char ch in str)
        //        {
        //            if (!Char.IsDigit(ch))
        //                return false;
        //        }

        //        return true;
        //    }

        //    return false;
        //}
        private static bool IsSymbolValid(MaskType mask, string str)
        {
            switch (mask)
            {
                case MaskType.Any:
                    return true;

                case MaskType.Integer:
                    if (str == NumberFormatInfo.CurrentInfo.NegativeSign)
                        // if (str == NumberFormatInfo.CurrentInfo.NegativeSign)
                        return true;
                    break;

                case MaskType.Decimal:
                    if (str == NumberFormatInfo.CurrentInfo.NumberDecimalSeparator ||
                        str == NumberFormatInfo.CurrentInfo.NegativeSign)
                        return true;
                    break;
                case MaskType.Numeric:
                    // if (str == NumberFormatInfo.CurrentInfo.NumberDecimalSeparator ||
                    if (str == NumberFormatInfo.CurrentInfo.ToString())
                        return true;
                    break;
                case MaskType.UnSignedInt:
                    if (str == NumberFormatInfo.CurrentInfo.NegativeSign || str == NumberFormatInfo.CurrentInfo.PositiveSign)
                        return false;
                    break;
                case MaskType.UnSignedDec:
                    if (str == NumberFormatInfo.CurrentInfo.NumberDecimalSeparator)
                        // ||
                        //str == NumberFormatInfo.CurrentInfo.PositiveSign)
                        return true;
                    break;
                case MaskType.Date:
                    if (str == NumberFormatInfo.CurrentInfo.ToString() || str == "/")
                        // ||
                        //str == NumberFormatInfo.CurrentInfo.PositiveSign)
                        return true;
                    break;
                case MaskType.Delcimalvalues:
                    // if (str == NumberFormatInfo.CurrentInfo.ToString())
                    //    return true;
                    //break;
                    if (str == NumberFormatInfo.CurrentInfo.NumberDecimalSeparator ||
                        str == NumberFormatInfo.CurrentInfo.NegativeSign)
                        return true;
                    break;
            }

            if (mask.Equals(MaskType.Integer) || mask.Equals(MaskType.Decimal) || mask.Equals(MaskType.UnSignedDec) || mask.Equals(MaskType.UnSignedInt) || mask.Equals(MaskType.Delcimalvalues))
            {
                foreach (char ch in str)
                {
                    if (!Char.IsDigit(ch))
                        return false;
                }

                return true;
            }
            else if (mask.Equals(MaskType.Numeric))
            {
                foreach (char ch in str)
                {
                    if (Char.IsDigit(ch))
                        return false;
                }

                return true;
            }
            else if (mask.Equals(MaskType.Delcimalvalues))
            {
                foreach (char ch in str)
                {
                    if (Char.IsDigit(ch))
                        if (ch.ToString() == ".")
                        {
                            return true;
                        }
                        else
                        {
                            return true;
                        }
                    return false;
                }

                return true;
            }

            else if (mask.Equals(MaskType.Date))
            {
                foreach (char ch in str)
                {
                    if (Char.IsDigit(ch))
                        if (ch.ToString() == "/")
                        {
                            return true;
                        }
                        else
                        {
                            return true;
                        }
                    return false;
                }

                return true;
            }
            return false;
        }
        #endregion
    }

    public enum MaskType
    {
        Any,
        Integer,
        Decimal,
        Numeric,
        UnSignedInt,
        UnSignedDec,
        Date,
        Delcimalvalues
    }
    public enum TextAlign
    {
        Right,
        Left,
        Center
    }
}