using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Windows.Data;
using ProcessDesigner.Common;

namespace ProcessDesigner.UserControls
{
    public class DecimalTextBox : TextBox
    {
        private int backspaceAction = 0;
        public DecimalTextBox()
        {
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Cut, null, CanExecuteCut)); //surpress cut
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, CopyExecuted)); //handle paste
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, PasteExecuted)); //handle paste


        }

        protected override void OnRender(System.Windows.Media.DrawingContext drawingContext)
        {
            if (Convert.ToString(Text).Trim().Length == 0 && IsFormatRequired)
            {
                Text = MaxScale == 0 ? TryParseInt("0") : TryParseDouble("0");
                UpdateBinding();
            }

            base.OnRender(drawingContext);
        }

        private bool _isFormatRequired = true;
        public bool IsFormatRequired
        {
            get { return _isFormatRequired; }
            set { _isFormatRequired = value; if (!value) Text = null; }
        }

        void UpdateBinding()
        {
            BindingExpression binding = this.GetBindingExpression(TextBox.TextProperty);
            if (binding != null)
            {
                binding.UpdateSource();
                object value = GetValue<object>(binding, binding.DataItem);

                if (Convert.ToString(value).Trim().Length == 0 && IsFormatRequired)
                    Text = MaxScale == 0 ? TryParseInt("0") : TryParseDouble("0");
                else
                    Text = Convert.ToString(value);

                isCellValueChanged = true;

                if (binding.DataItem != null && binding.ParentBinding != null && binding.ParentBinding.Path != null)
                {
                    System.Reflection.PropertyInfo propertyInfo = binding.DataItem.GetType().GetProperty(binding.ParentBinding.Path.Path);
                    if (propertyInfo != null)
                    {
                        propertyInfo.SetValue(binding.DataItem, Text, null);

                        //bool isNullable = propertyInfo.PropertyType.IsGenericType &&
                        //    propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);

                        //switch (propertyInfo.PropertyType.Name)
                        //{
                        //    case "String":
                        //        propertyInfo.SetValue(binding.DataItem, isNullable ? null : DBNull.Value, null);
                        //        break;
                        //    case "Nullable`1":
                        //        propertyInfo.SetValue(binding.DataItem, isNullable ? null : DBNull.Value, null);
                        //        break;
                        //}

                    }
                }

                binding.UpdateSource();
            }
        }

        protected override void OnInitialized(EventArgs e)
        {
            UpdateBinding();
            base.OnInitialized(e);
        }

        public override void EndInit()
        {
            UpdateBinding();
            base.EndInit();
        }

        public static readonly DependencyProperty AllowNegativeProperty = DependencyProperty.Register("AllowNegative", typeof(bool), typeof(DecimalTextBox), new FrameworkPropertyMetadata() { DefaultValue = true });
        public bool AllowNegative
        {
            get { return (bool)GetValue(AllowNegativeProperty); }
            set { SetValue(AllowNegativeProperty, value); }
        }

        #region DependencyProperty - MaxDecimalPlaces
        /// <summary>  
        /// Dependency property to set the maximum number of decimal places allowed  
        /// </summary>  
        public int MaxDecimalPlaces
        {
            get { return (int)GetValue(MaxDecimalPlacesProperty); }
            set { SetValue(MaxDecimalPlacesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxDecimalPlaces.  This enables animation, styling, binding, etc...  
        public static readonly DependencyProperty MaxDecimalPlacesProperty =
            DependencyProperty.Register("MaxDecimalPlaces", typeof(int), typeof(DecimalTextBox), new UIPropertyMetadata(2));
        #endregion

        object previouseCellValue = null;
        bool isCellValueChanged = false;

        public int MaxScale
        {
            get { return MaxDecimalPlaces; }
        }

        public int MaxPrecision
        {
            get { return (MaxLength - MaxDecimalPlaces) - (MaxScale > 0 ? 1 : 0); }
        }

        protected override void OnQueryContinueDrag(QueryContinueDragEventArgs e)
        {
            backspaceAction = 0;
            e.Action = DragAction.Cancel;
            e.Handled = true;
            base.OnQueryContinueDrag(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (Convert.ToString(Text).Trim().Length == 0 && !IsFormatRequired)
            {
                base.OnMouseUp(e);
                return;
            }

            if (Convert.ToString(Text).Trim().Length == 0 && IsFormatRequired) Text = "0";
            if (Convert.ToDouble(Text) == 0)
            {
                SelectionStart = (MaxScale == 0 && Convert.ToString(Text) == "0") ? 1 : 0;
            }
            base.OnMouseUp(e);
        }

        private int _dotPosition = -1;
        private int dotPosition
        {
            get
            {
                return _dotPosition;
            }
            set
            {
                _dotPosition = value;
                hasDecimalChar = _dotPosition > -1;

            }
        }

        private bool isValidKey(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            bool bReturnValue = false;
            backspaceAction = 0;
            TextBox tb = (sender as TextBox);
            if (tb == null) return bReturnValue;
            if (tb.SelectionLength == tb.Text.Length) tb.Text = string.Empty;

            int selectionLength = tb.SelectionLength;
            int selectionStart = tb.SelectionStart;
            string selectedText = tb.SelectedText;

            tb.SelectionLength = 0;
            char[] chars = e.Text.ToCharArray();
            if (chars.Length == 0) return bReturnValue;

            char currentChar = chars[0];
            if (char.IsControl(currentChar))
            {
                bReturnValue = true;
                return bReturnValue;
            }

            if (currentChar == '-')
            {
                bReturnValue = true;
                return bReturnValue;
            }

            bool isCharAllowed = !char.IsControl(currentChar) && (char.IsDigit(currentChar) && MaxScale == 0) ||
                                                     ((!char.IsDigit(currentChar) || currentChar != '.') && MaxScale > 0);

            if (!isCharAllowed)
            {
                bReturnValue = true;
                return bReturnValue;
            }

            int currentLength = Convert.ToString(tb.Text).Length;
            string currentText = tb.Text.Remove(selectionStart, Math.Min(selectionLength, tb.Text.Length - selectionStart)).Insert(selectionStart, currentChar.ToString());

            if (currentChar == '.')
            {
                dotPosition = currentText.IndexOf('.');
                tb.SelectionStart = !hasDecimalChar ? 0 : dotPosition + 1;
                tb.SelectionLength = selectionLength == 0 ? 1 : selectionLength;
            }

            int no_of_chars = (MaxScale > 0 ? MaxPrecision + MaxScale + 1 : MaxPrecision - MaxScale);
            if (currentText.Length > no_of_chars)
            {
                bReturnValue = true;
                return bReturnValue;
            }

            dotPosition = currentText.IndexOf('.');
            if (currentChar == '.' && hasDecimalChar)
            {
                try
                {
                    tb.Text = MaxScale == 0 ? TryParseInt(tb.Text) : TryParseDouble(tb.Text);

                    dotPosition = currentText.IndexOf('.');
                    tb.SelectionStart = !hasDecimalChar ? 0 : dotPosition + 1;
                    tb.SelectionLength = selectionLength == 0 ? 1 : selectionLength;
                    bReturnValue = true;
                    return bReturnValue;
                }
                catch (Exception)
                {

                }
            }

            bool isValidNumeric = IsNumeric(currentText);
            if (!isValidNumeric)
            {
                bReturnValue = true;
                return bReturnValue;
            }
            else if (isValidNumeric && MaxScale > 0)
            {
                string currentPrecisionText = "";
                string currentScaleText = "";
                if (hasDecimalChar)
                {
                    currentPrecisionText = currentText.Substring(0, dotPosition);
                    currentScaleText = currentText.Substring(dotPosition + 1);
                }

                if (currentPrecisionText.Length > MaxPrecision)
                {
                    bReturnValue = true;
                    return bReturnValue;
                }

                if (currentScaleText.Length > MaxScale)
                {
                    tb.SelectionLength = selectionLength == 0 ? 1 : selectionLength;
                    bReturnValue = true;
                    return bReturnValue;
                }
            }
            return bReturnValue;
        }

        private string TryParseDouble(string currentText)
        {
            string bReturnValue = IsNumeric(currentText) ? currentText : "";
            try
            {
                string decimalFormat = "0." + "".PadLeft(MaxScale, '0');
                if (Convert.ToString(currentText).Length == 0) currentText = "0";

                int no_of_chars = (MaxScale == 0 ? MaxPrecision - MaxScale : MaxPrecision + MaxScale + 1);

                dotPosition = currentText.IndexOf('.');

                string currentPrecisionText = "";
                string currentScaleText = "";
                if (hasDecimalChar && currentText.Length > no_of_chars)
                {
                    currentPrecisionText = currentText.Substring(0, dotPosition).Replace(" ", string.Empty);
                    currentScaleText = currentText.Substring(dotPosition + 1).Replace(" ", string.Empty);

                    if (currentPrecisionText.Length > MaxPrecision) currentPrecisionText = currentText.Substring(0, MaxPrecision);
                    if (currentScaleText.Length > MaxScale) currentScaleText = currentText.Substring(dotPosition + 1, MaxScale);
                    currentText = currentPrecisionText + "." + currentScaleText;

                }
                else if (hasDecimalChar)
                {
                    currentPrecisionText = currentText.Substring(0, dotPosition).Replace(" ", string.Empty);
                    currentScaleText = currentText.Substring(dotPosition + 1).Replace(" ", string.Empty);

                    if (currentPrecisionText.Length > MaxPrecision) currentPrecisionText = currentText.Substring(0, MaxPrecision);
                    if (currentScaleText.Length > MaxScale) currentScaleText = currentText.Substring(dotPosition + 1, MaxScale);
                    currentText = currentPrecisionText + "." + currentScaleText;

                }
                if (currentText.Length > 0 && currentText.Length > no_of_chars)
                    currentText = currentText.Substring(0, Math.Min(currentText.Length, no_of_chars));

                double doubleOutput;
                if (double.TryParse(currentText, out doubleOutput))
                {
                    bReturnValue = doubleOutput.ToString(decimalFormat);
                }
            }
            catch (Exception)
            {
                bReturnValue = "";
            }
            return bReturnValue;
        }

        private string TryParseInt(string currentText)
        {
            backspaceAction = 0;
            string bReturnValue = IsNumeric(currentText) ? currentText : "";
            try
            {
                if (Convert.ToString(currentText).Length == 0) currentText = "0";

                int no_of_chars = (MaxScale == 0 ? MaxPrecision - MaxScale : MaxPrecision + MaxScale + 1);
                if (currentText.Length > 0 && currentText.Length > no_of_chars)
                    currentText = currentText.Substring(0, Math.Min(currentText.Length, no_of_chars));

                Int64 doubleOutput;
                if (Int64.TryParse(currentText, out doubleOutput))
                {
                    bReturnValue = doubleOutput.ToString();
                }

            }
            catch (Exception)
            {
                bReturnValue = "";
            }
            return bReturnValue;
        }

        public bool IsNumeric(string val, System.Globalization.NumberStyles numberStyle = System.Globalization.NumberStyles.Number)
        {
            if (Convert.ToString(val).Length == 0) return true;
            if (Convert.ToString(val).Split('.').Length - 1 > 1) return false;
            Double result;
            return Double.TryParse(val, numberStyle,
                System.Globalization.CultureInfo.CurrentCulture, out result);
        }

        bool hasDecimalChar = false;
        private KeyConverter _keyConv = new KeyConverter();
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (IsReadOnly) return;
            backspaceAction = 0;
            string keyPressed = _keyConv.ConvertToString(e.Key);
            if (!(keyPressed == "Space" || keyPressed == "Delete" || keyPressed == "Backspace" || keyPressed == "Down" || keyPressed == "Up"))
            {
                base.OnPreviewKeyDown(e);
                return;
            }

            if (keyPressed != null)
            {
                isCellValueChanged = true;
                if (keyPressed.Length == 1 && char.IsControl(keyPressed[0]))
                {
                    e.Handled = false;
                }

                if (keyPressed == "Space")
                {
                    if (SelectionLength == Convert.ToString(Text).Length)
                    {
                        Clear();
                        e.Handled = true;
                    }
                }

                int selectionStart = SelectionStart;
                dotPosition = Text.IndexOf('.');

                if (keyPressed == "Delete")
                {

                    if (Text.Length > 0 &&
                        (SelectedText.Contains(".") || (SelectedText.Length == 0 && SelectionStart == dotPosition)) &&
                        Text.Length != SelectionLength)
                    {
                        Text = Text.Substring(0, Math.Min(selectionStart, Text.Length));
                        SelectionStart = Math.Min(selectionStart, Text.Length);
                        e.Handled = true;
                    }
                }
                else if (keyPressed == "Backspace")
                {
                    if (Text.Length > 0 && (SelectedText.Contains(".") || (SelectedText.Length == 0 && SelectionStart - 1 == dotPosition)) && Text.Length != SelectionLength)
                    {
                        Text = Text.Substring(0, Math.Min(selectionStart, Text.Length));
                        if (SelectedText.Length == 0 && selectionStart - 1 == dotPosition && selectionStart - 1 >= 0)
                        {
                            SelectionStart = Math.Min(selectionStart - 1, Text.Length);
                        }
                        else
                        {
                            SelectionStart = Math.Min(selectionStart, Text.Length);
                        }
                        e.Handled = true;
                    }
                    else
                    {
                        backspaceAction = 1;
                    }
                }
                else if ((keyPressed == "Down") && dotPosition > 0)
                {
                    SelectionStart = hasDecimalChar ? dotPosition + 1 : 0;
                    SelectionLength = hasDecimalChar ? MaxScale : 0;
                    e.Handled = hasDecimalChar;
                    return;
                }
                else if ((keyPressed == "Up") && dotPosition > 0)
                {

                    dotPosition = Text.IndexOf('.');

                    string currentPrecisionText = "";
                    string currentScaleText = "";
                    if (hasDecimalChar)
                    {
                        currentPrecisionText = Text.Substring(0, dotPosition);
                        currentScaleText = Text.Substring(dotPosition + 1);
                    }

                    SelectionStart = 0;
                    SelectionLength = hasDecimalChar ? Math.Min(dotPosition, currentPrecisionText.Length) : 0;
                    e.Handled = hasDecimalChar;
                    return;
                }
            }
            base.OnPreviewKeyDown(e);
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            try
            {
                if (IsReadOnly) return;
                backspaceAction = 0;
                char[] chars = e.Text.ToCharArray();
                if (chars.Length == 0) e.Handled = false;

                char currentChar = chars[0];
                bool isCharAllowed = !char.IsControl(currentChar) && (!char.IsDigit(currentChar) && MaxScale == 0) ||
                                                                     ((!char.IsDigit(currentChar) || currentChar != '.') && MaxScale > 0);
                int selectionStart = SelectionStart;
                int selectionLength = SelectionLength;
                string selectedText = SelectedText;
                int currentLength = Convert.ToString(Text).Length;

                Text = MaxScale == 0 ? TryParseInt(Text) : TryParseDouble(Text);
                string currentText = Text;

                dotPosition = currentText.IndexOf('.');
                bool isFirstCharZero = Text.Length > 0 && Text.Substring(0, 1) == "0" || (SelectionStart == 0 && currentChar == '0');

                if (isFirstCharZero && selectionLength != Convert.ToString(Text).Length)
                {
                    dotPosition = Text.IndexOf('.');
                    if (hasDecimalChar)
                    {
                        if (selectionStart > dotPosition)
                        {
                            if (selectionStart > dotPosition) selectionLength = 1;
                            string remainingText = Text.Remove(selectionStart, Math.Min(selectionLength, Text.Length - selectionStart));
                            currentText = remainingText.Insert(Math.Min(selectionStart, remainingText.Length), currentChar.ToString());
                            e.Handled = !IsNumeric(currentText);
                            if (!e.Handled)
                            {
                                Text = MaxScale == 0 ? TryParseInt(currentText) : TryParseDouble(currentText);

                                dotPosition = Text.IndexOf('.');
                                SelectionStart = hasDecimalChar ? (selectionStart > dotPosition ? selectionStart + 1 : dotPosition + 1) : 0;
                                SelectionLength = 1;
                                e.Handled = true;
                            }
                            else if (e.Handled && currentChar == '.')
                            {
                                dotPosition = Text.IndexOf('.');
                                SelectionStart = hasDecimalChar ? dotPosition + 1 : 0;
                                SelectionLength = hasDecimalChar ? MaxScale : 0;
                            }
                            return;
                        }
                        else
                        {
                            currentText = Text.Remove(selectionStart, Math.Min(selectionLength, Text.Length - selectionStart)).Insert(selectionStart + 1, currentChar.ToString());
                            SelectionStart = 1;
                            SelectionLength = 1;
                        }

                    }
                }
                else if (selectionLength != Convert.ToString(Text).Length && currentChar != '.' && isCharAllowed && SelectionStart != currentLength)
                {
                    if (selectionStart > dotPosition) selectionLength = 1;
                    string remainingText = Text.Remove(selectionStart, Math.Min(selectionLength, Text.Length - selectionStart));
                    currentText = remainingText.Insert(Math.Min(selectionStart, remainingText.Length), currentChar.ToString());
                    remainingText = currentText;
                    dotPosition = remainingText.IndexOf('.');
                    string currentPrecisionText = "";
                    string currentScaleText = "";

                    if (hasDecimalChar)
                    {
                        currentPrecisionText = remainingText.Substring(0, dotPosition);
                        currentScaleText = remainingText.Substring(dotPosition + 1);
                        if (currentPrecisionText.Length - 1 == MaxPrecision && selectionStart < dotPosition)
                        {
                            e.Handled = true;
                            return;
                        }
                        else if (selectionStart > dotPosition)
                        {
                            e.Handled = true;
                            Text = currentText;
                            SelectionStart = hasDecimalChar ? (selectionStart > dotPosition ? selectionStart + 1 : dotPosition + 1) : 0;
                            SelectionLength = selectionLength;
                            return;
                        }
                    }

                    currentText = Text.Remove(selectionStart, Math.Min(selectionLength, Text.Length - selectionStart)).Insert(selectionStart, currentChar.ToString());
                    dotPosition = currentText.IndexOf('.');
                    if (selectionStart >= dotPosition && MaxScale > 0)
                    {
                        SelectionLength = hasDecimalChar ? 1 : 0;
                    }
                }
                else if (currentChar == '.' && MaxScale > 0)
                {
                    if (Convert.ToString(Text).Length == 0)
                        Text = MaxScale == 0 ? TryParseInt(Text) : TryParseDouble(Text);

                    dotPosition = Text.IndexOf('.');
                    if (selectionStart < dotPosition && selectionLength == currentLength && isCharAllowed)
                    {
                        Text = string.Empty;
                        Text = MaxScale == 0 ? TryParseInt(Text) : TryParseDouble(Text);
                        SelectionStart = hasDecimalChar ? (selectionStart > dotPosition ? selectionStart + 1 : dotPosition + 1) : 0;
                        SelectionLength = 1;
                        e.Handled = true;
                        return;
                    }
                    else
                    {
                        SelectionStart = hasDecimalChar ? dotPosition + 1 : 0;
                        SelectionLength = hasDecimalChar ? 1 : 0;
                    }
                    e.Handled = hasDecimalChar;
                    return;
                }

                dotPosition = currentText.IndexOf('.');
                int no_of_chars = (!hasDecimalChar || MaxScale == 0 ? MaxPrecision - MaxScale : MaxPrecision + MaxScale + 1);
                if (currentText.Length > no_of_chars)
                {
                    e.Handled = isValidKey(this, e);
                    return;
                }

                if (selectionLength > 1)
                {
                    e.Handled = !IsNumeric(currentText);
                }
                else
                {
                    e.Handled = isValidKey(this, e);
                }
            }
            catch
            {
                e.Handled = true;
            }
            base.OnPreviewTextInput(e);
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            try
            {
                if (Text == null || Convert.ToString(Text).Length == 0) return;
                int selectionStart = SelectionStart;
                int selectionLength = SelectionLength;
                string selectedText = SelectedText;
                string newText = MaxScale == 0 ? TryParseInt(Text) : TryParseDouble(Text);
                if (Text != newText && Convert.ToString(Text).Length > 0)
                {
                    isCellValueChanged = true;
                    Text = newText;
                    dotPosition = Text.IndexOf('.');

                    string currentPrecisionText = "";
                    string currentScaleText = "";
                    if (hasDecimalChar)
                    {
                        currentPrecisionText = newText.Substring(0, dotPosition);
                        currentScaleText = newText.Substring(dotPosition + 1);
                    }

                    if (hasDecimalChar && currentPrecisionText.Length != 1 && selectionStart != Convert.ToString(Text).Length && selectionStart <= dotPosition)
                        SelectionStart = selectionStart - 1 < 0 ? 0 : selectionStart - 1;
                    else if (hasDecimalChar && currentPrecisionText.Length == 1 && selectionStart != Convert.ToString(Text).Length && selectionStart > dotPosition)
                        SelectionStart = selectionStart - 1 < 0 ? 0 : selectionStart - 1 + backspaceAction;
                    else if (hasDecimalChar && selectionStart != Convert.ToString(Text).Length && selectionStart <= dotPosition)
                        SelectionStart = Math.Min(selectionStart, dotPosition);
                    else if (hasDecimalChar && selectionStart != Convert.ToString(Text).Length && selectionStart > dotPosition)
                        SelectionStart = Math.Min(selectionStart, Text.Length);
                    else
                        SelectionStart = selectionStart;

                    SelectionLength = selectionLength;
                    SelectedText = selectedText;
                }
                else
                {
                    if (Text != newText && Convert.ToString(Text).Length == 0)
                    {
                        isCellValueChanged = true;
                        Text = newText;
                    }

                    SelectionStart = MaxScale == 0 && Convert.ToString(Text) == "0" ? 1 : selectionStart;
                }
            }
            catch (Exception)
            {
            }



            base.OnTextChanged(e);
        }
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            backspaceAction = 0;
            SelectionStart = 0;
            SelectionLength = Convert.ToString(Text).Length;
            previouseCellValue = Text;
            if (Tag.IsNotNullOrEmpty()) StatusMessage.setStatus(Tag.ToString());
            base.OnGotFocus(e);
        }

        private bool _formatTextAfterLostFocus = true;
        public bool FormatTextAfterLostFocus
        {
            get { return _formatTextAfterLostFocus; }
            set { _formatTextAfterLostFocus = value; }
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            backspaceAction = 0;
            if (!isCellValueChanged)
            {
                Text = Convert.ToString(previouseCellValue);
                if (previouseCellValue != null)
                    UpdateBinding();
            }

            if (MaxScale > 0 && FormatTextAfterLostFocus)
            {
                if (Convert.ToString(Text).Trim().Length == 0)
                {
                    base.OnLostFocus(e);
                    return;
                }
                string decimalFormat = "0." + "".PadLeft(MaxScale, '0');
                Text = Convert.ToDecimal(Convert.ToString(Text)).ToString(decimalFormat);
                UpdateBinding();
            }

            base.OnLostFocus(e);
        }

        private void CanExecuteCut(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            e.Handled = true;
        }

        private void CopyExecuted(object sender, RoutedEventArgs e)
        {

        }

        private void PasteExecuted(object sender, RoutedEventArgs e)
        {

            if (IsReadOnly)
                return;

            object data = Clipboard.GetData(DataFormats.Text);
            if (data != null)
            {
                if (IsNumeric(Convert.ToString(data).Trim()))
                    Text = MaxScale == 0 ? TryParseInt(Convert.ToString(data).Trim()) : TryParseDouble(Convert.ToString(data).Trim());
            }
        }

        public T GetValue<T>(BindingExpression expression, object dataItem)
        {
            if (expression == null || dataItem == null)
            {
                return default(T);
            }

            string bindingPath = expression.ParentBinding.Path.Path;
            string[] properties = bindingPath.Split('.');

            object currentObject = dataItem;
            Type currentType = null;

            for (int i = 0; i < properties.Length; i++)
            {
                currentType = currentObject.GetType();
                System.Reflection.PropertyInfo property = currentType.GetProperty(properties[i]);
                if (property == null)
                {
                    currentObject = null;
                    break;
                }
                currentObject = property.GetValue(currentObject, null);
                if (currentObject == null)
                {
                    break;
                }
            }

            return (T)currentObject;
        }

    }

    #region Previous DecimalTextBox Code commented by Anandan.C
    //public class DecimalTextBox : TextBox
    //{
    //    private static string decimalSeparator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

    //    /// <summary>  
    //    /// Get/Set the decimal value.  This is not a dependency property, so you cannot bind things to it.  
    //    /// To bind a decimal data value to this control, bind to the Text property, and do a conversion.  
    //    /// </summary>  
    //    public decimal DecimalValue
    //    {
    //        get
    //        {
    //            decimal value = 0.0M;
    //            if (!string.IsNullOrEmpty(this.Text))
    //            {
    //                value = Convert.ToDecimal(this.Text);
    //            }
    //            return value;
    //        }
    //        set
    //        {
    //            if (value < 0 && this.NegativeBlank)
    //            {
    //                base.Text = string.Empty;
    //            }
    //            else
    //            {
    //                string decFormat = string.Format("{{0:{0}}}{{1}}", "0.".PadRight(this.MaxDecimalPlaces + 2, '#'));
    //                base.Text = string.Format(decFormat, value);
    //            }
    //        }
    //    }
    //    #region Text Entry Validation

    //    /// <summary>  
    //    /// Prevent space characters.    
    //    /// Thanx to aelij on http://social.msdn.microsoft.com/Forums/en-US/wpf/thread/5460722b-619b-4937-b939-38610e9e01ea  
    //    /// </summary>  
    //    /// <param name="e"></param>  
    //    /// 
    //    //private override void textBox_PreviewKeyDown(object sender, KeyEventArgs e)
    //    //{
    //    //    MessageBox.Show("sdfdsf");
    //    //}

    //    //tb.GotFocus += OnInputTextBoxGotFocus;





    //    private void OnPreviewKeyLostFocus(object sender, EventArgs e)
    //    {
    //        try
    //        {
    //            TextBox tb = (TextBox)sender;
    //            if (tb.Text == string.Empty)
    //            {
    //                StringBuilder str = new StringBuilder();
    //                str.Append("0.00");
    //                tb.Text = str.ToString();
    //                //tb.Insert(0, str);
    //                //SelectionLength = 0;
    //            }


    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex.LogException();
    //        }

    //        // MessageBox.Show("asasdsaasdsa");
    //    }

    //    private void OnPreviewKeyDownvalue(object sender, KeyEventArgs e)
    //    {
    //        TextBox tb = (TextBox)sender;
    //        int select = 0;
    //        int selectionLength = 0;
    //        string indexstring = string.Empty;
    //        if (e.Key == Key.Space)
    //        {
    //            e.Handled = true;
    //            return;
    //        }

    //        string seletionText = tb.SelectedText;

    //        if (e.Key == Key.Back || e.Key == Key.Delete)
    //        {
    //            select = tb.SelectionStart;
    //            selectionLength = tb.SelectionLength;


    //            if (tb.SelectionLength == tb.Text.ToString().Length)
    //            {
    //                if (e.Key == Key.Back || e.Key == Key.Delete)
    //                {
    //                    StringBuilder str = new StringBuilder();
    //                    str.Append("0.00");
    //                    base.Text = "";
    //                    base.Text = str.ToString();
    //                    //base.Text.Insert(0, str.ToString());
    //                    // base.SelectionStart = 1;
    //                    base.SelectionLength = 0;
    //                    e.Handled = true;
    //                    return;
    //                }

    //                e.Handled = false;
    //                return;
    //            }

    //            if (seletionText.Contains('.'))
    //            {
    //                tb.SelectionStart = tb.SelectionStart;
    //                e.Handled = true;
    //                return;
    //            }

    //            if (select == 0)
    //            {
    //                e.Handled = false;
    //                return;
    //            }

    //            if (select > tb.Text.ToString().Length - 1)
    //            {
    //                e.Handled = false;
    //                return;
    //            }

    //            if (e.Key == Key.Back)
    //            {
    //                indexstring = tb.Text.ToString()[select].ToString();

    //                if ((tb.SelectionStart + tb.SelectionLength) == tb.Text.ToString().Length && tb.SelectionLength == 2)
    //                {
    //                    e.Handled = false;
    //                    return;
    //                }

    //                if (indexstring == "." && selectionLength == 1)
    //                {
    //                    e.Handled = true;
    //                }
    //                select = select - 1;
    //            }
    //            else if (e.Key == Key.Delete)
    //            {
    //                select = select;
    //            }

    //            indexstring = tb.Text.ToString()[select].ToString();
    //            if (indexstring == ".")
    //            {
    //                e.Handled = true;
    //            }

    //        }
    //        else if (e.Key == Key.LeftCtrl)
    //        {
    //            //StringBuilder str = new StringBuilder();
    //            //str.Append("0.00");
    //            //base.Text = "";
    //            //base.Text = str.ToString();
    //            ////base.Text.Insert(0, str.ToString());
    //            //// base.SelectionStart = 1;
    //            //base.SelectionLength = 0;
    //            e.Handled = false;
    //            return;
    //        }
    //        else if (seletionText.Contains('.'))
    //        {
    //            if (seletionText.ToString().Length == tb.Text.Length)
    //            {
    //                e.Handled = false;
    //                return;
    //            }
    //            e.Handled = true;
    //            return;
    //        }


    //    }



    //    /// <summary>  
    //    /// Determines if text input is valid, and if not, rejects it.  
    //    /// Note that the space character doesn't result in a call to this method,  
    //    /// so it is handled separately in OnPreviewKeyDown().  
    //    /// </summary>  
    //    /// <param name="e"></param>  
    //    /// 
    //    public string TextboxString = string.Empty;
    //    protected override void OnPreviewTextInput(TextCompositionEventArgs e)
    //    {

    //        // Force a binding update if the user presses enter  
    //        if (e.Text == "\r")
    //        {
    //            e.Handled = true;
    //            BindingExpression be = this.GetBindingExpression(TextProperty);
    //            if (be != null)
    //            {
    //                be.UpdateSource();
    //            }
    //        }
    //        else
    //        {
    //            e.Handled = !isMyTextValid(computeProposed(e.Text));
    //            //  ConvertText();
    //        }

    //        //if (e.Text == '.')
    //        //{
    //        //      string IndexString = string.Empty;
    //        //        IndexString = text.ToString().IndexOf('.').ToString();
    //        //     // SelectionLength = 0;
    //        //      SelectionStart  = Convert.ToInt32(IndexString);
    //        //     SelectionStart = SelectionStart + 1;
    //        //    //    SelectionLength = SelectionLength + 2;
    //        //    //base.SelectionStart = SelectionStart;
    //        //    //base.SelectionLength = SelectionLength;
    //        //    //  return text ;
    //        //}
    //    }

    //    private static void OnInputTextBoxGotFocus(object sender, RoutedEventArgs e)
    //    {
    //        var tb = e.OriginalSource as TextBox;
    //        try
    //        {
    //            // decimal decimalValue = 0.00m;
    //            if (tb == null || tb.Text == string.Empty)
    //            {
    //                //tb.Text = decimalValue;
    //                tb.Text = null;
    //            }
    //            else if (tb.ToString().Length > 0 && (tb.Text.ToString() == "0.00"))
    //            {
    //                // tb.Text = decimalValue;
    //                tb.Text = null;
    //            }
    //            else if (tb.ToString().Length > 0 && (tb.Text.ToString() != "0.00"))
    //            {
    //                tb.SelectionStart = 0;
    //            }
    //        }
    //        catch
    //        {
    //            // tb.Text = "0.00";
    //        }

    //    }

    //    public override void OnApplyTemplate()
    //    {
    //        base.OnApplyTemplate();
    //        this.PreviewKeyDown += OnPreviewKeyDownvalue;
    //        this.LostFocus += OnPreviewKeyLostFocus;
    //        this.GotFocus += OnInputTextBoxGotFocus;
    //        if (base.Text == null || base.Text == string.Empty)
    //        {
    //            base.Text = "0.00";
    //        }
    //        //     
    //        //this.PreviewKeyUp += KeyPressed;
    //    }
    //    private bool isMyTextValid(String text)
    //    {
    //        bool isValid = false;
    //        if (!AllowNegative && text.StartsWith("-") || text.EndsWith("-") || text.Contains("-"))
    //        {
    //            return false;
    //        }
    //        //else if (AllowNegative == false && text.StartsWith("-") && text.EndsWith("-"))
    //        //{
    //        //    return false;
    //        //}
    //        if (!String.IsNullOrEmpty(text == null ? null : text.Trim()))
    //        {
    //            if (text.Contains(decimalSeparator) && this.MaxDecimalPlaces <= 0)
    //            {
    //                isValid = false;
    //            }
    //            else
    //            {
    //                Decimal result = Decimal.MinValue;
    //                if (Decimal.TryParse(text, out result))
    //                {
    //                    if (result != Decimal.MinValue)
    //                    {
    //                        isValid = checkDecimalPlaces(result);
    //                    }
    //                }
    //            }
    //        }

    //        return isValid;
    //    }

    //    public static readonly DependencyProperty AllowNegativeProperty = DependencyProperty.Register("AllowNegative", typeof(bool), typeof(DecimalTextBox), new FrameworkPropertyMetadata() { DefaultValue = true });
    //    public bool AllowNegative
    //    {
    //        get { return (bool)GetValue(AllowNegativeProperty); }
    //        set { SetValue(AllowNegativeProperty, value); }
    //    }

    //    /// <summary>  
    //    /// Check to see if the MaxDecimalPlaces rule is violated  
    //    /// </summary>  
    //    /// <param name="value"></param>  
    //    /// <returns></returns>  
    //    private bool checkDecimalPlaces(decimal value)
    //    {
    //        string textValue = value.ToString("0.############");
    //        string[] parts = textValue.Split(new string[] { decimalSeparator }, StringSplitOptions.None);
    //        bool valid = (parts.Length <= 1) || (parts[1].Length <= this.MaxDecimalPlaces);
    //        // bool valid = (parts.Length <= 1) || (parts[1].Length <= this.MaxDecimalPlaces) || (Convert.ToInt32(parts[1].ToString()) <= 999);

    //        return valid;
    //    }

    //    /// <summary>  
    //    /// Compute the proposed text - what would be in the textbox if the input was allowed.  
    //    /// </summary>  
    //    /// <param name="newText"></param>  
    //    /// <returns></returns>  
    //    private string computeProposed(string newText)
    //    {
    //        string text = base.Text;
    //        // _this.PreviewKeyUp += textBox_PreviewKeyDown;
    //        // TextBox tb = (TextBox)sender;

    //        if (newText == ".")
    //        {
    //            string indexstring = string.Empty;
    //            indexstring = text.ToString().IndexOf('.').ToString();
    //            if (indexstring == "-1")
    //            {
    //                return text.Insert(this.SelectionStart, "0.00");
    //            }

    //            //  SelectionLength = 0;
    //            base.SelectionStart = Convert.ToInt32(indexstring);
    //            base.SelectionStart = SelectionStart + 1;
    //            //    SelectionLength = SelectionLength + 2;
    //            //base.SelectionStart = SelectionStart;
    //            base.SelectionLength = SelectionLength + 2;
    //            //  return text ;
    //        }
    //        else if (newText != ".")
    //        {
    //            //  if(AllowNegative =false && newText ="-")
    //            // {

    //            // }

    //            //if(base.SelectionStart ==12)
    //            //{
    //            //    base.SelectionLength = SelectionLength + 1;
    //            //} else 
    //            if (base.SelectionStart == base.Text.Length - 2 || base.SelectionStart == base.Text.Length - 1)
    //            {
    //                base.SelectionLength = SelectionLength + 1;
    //            }

    //        }

    //        if (base.SelectionLength > 0)
    //        {
    //            text = text.Remove(this.SelectionStart, this.SelectionLength);
    //            return text.Insert(this.SelectionStart, newText);

    //        }
    //        // else
    //        // {
    //        return text.Insert(this.SelectionStart, newText);
    //        //  }
    //    }
    //    #endregion

    //    #region Special handling for pasting events

    //    static DecimalTextBox()
    //    {
    //        EventManager.RegisterClassHandler(
    //            typeof(DecimalTextBox),
    //            DataObject.PastingEvent,
    //            (DataObjectPastingEventHandler)((sender, e) =>
    //            {
    //                if (!IsDataValid(e.DataObject, sender as DecimalTextBox))
    //                {
    //                    DataObject data = new DataObject();
    //                    data.SetText(String.Empty);
    //                    e.DataObject = data;
    //                    e.Handled = false;
    //                }
    //            }));

    //        TextProperty.OverrideMetadata(
    //            typeof(DecimalTextBox),
    //            new FrameworkPropertyMetadata(null,
    //                (CoerceValueCallback)((DependencyObject element, Object baseValue) =>
    //                {
    //                    return IsTextValid(baseValue.ToString()) ? baseValue : string.Empty;
    //                })));
    //    }


    //    private static bool IsDataValid(IDataObject data, DecimalTextBox dtb)
    //    {
    //        bool isValid = false;
    //        if (data != null)
    //        {
    //            String text = data.GetData(DataFormats.Text) as String;
    //            if (dtb != null)
    //            {
    //                isValid = dtb.isMyTextValid(dtb.computeProposed(text));
    //            }
    //            else
    //            {
    //                isValid = IsTextValid(text);
    //            }
    //        }

    //        return isValid;
    //    }

    //    private static bool IsTextValid(String text)
    //    {
    //        bool isValid = false;
    //        if (!String.IsNullOrEmpty(text == null ? null : text.Trim()))
    //        {
    //            Decimal result = Decimal.MinValue;
    //            if (Decimal.TryParse(text, out result))
    //            {
    //                if (result != Decimal.MinValue)
    //                {
    //                    isValid = true;
    //                }
    //            }
    //        }

    //        return isValid;
    //    }
    //    #endregion

    //    #region DependencyProperty - MaxDecimalPlaces
    //    /// <summary>  
    //    /// Dependency property to set the maximum number of decimal places allowed  
    //    /// </summary>  
    //    public int MaxDecimalPlaces
    //    {
    //        get { return (int)GetValue(MaxDecimalPlacesProperty); }
    //        set { SetValue(MaxDecimalPlacesProperty, value); }
    //    }

    //    // Using a DependencyProperty as the backing store for MaxDecimalPlaces.  This enables animation, styling, binding, etc...  
    //    public static readonly DependencyProperty MaxDecimalPlacesProperty =
    //        DependencyProperty.Register("MaxDecimalPlaces", typeof(int), typeof(DecimalTextBox), new UIPropertyMetadata(2));
    //    #endregion

    //    #region Dependency Property - NegativeBlank
    //    /// <summary>  
    //    /// Dependency property to show a blank in the textbox if the value is negative.  
    //    /// </summary>  
    //    public bool NegativeBlank
    //    {
    //        get { return (bool)GetValue(NegativeBlankProperty); }
    //        set { SetValue(NegativeBlankProperty, value); }
    //    }

    //    // Using a DependencyProperty as the backing store for NegativeBlank.  This enables animation, styling, binding, etc...  
    //    public static readonly DependencyProperty NegativeBlankProperty =
    //        DependencyProperty.Register("NegativeBlank", typeof(bool), typeof(DecimalTextBox), new UIPropertyMetadata(true));
    //    #endregion

    //    #region Drag/Drop
    //    /// <summary>  
    //    /// Currently not allowing Drag/Drop.  May in the future add a dependency property called  
    //    /// 'AllowDrop', which when true, will cause this method to test and possibly accept a drop.  
    //    /// </summary>  
    //    /// <param name="e"></param>  
    //    protected override void OnDrop(DragEventArgs e)
    //    {
    //        // no drag and drop allowed  
    //        e.Handled = true;
    //        base.OnDrop(e);
    //    }

    //    /// <summary>  
    //    /// See comments for OnDrop  
    //    /// </summary>  
    //    /// <param name="e"></param>  
    //    protected override void OnDragOver(DragEventArgs e)
    //    {
    //        // no drag and drop allowed  
    //        e.Handled = true;
    //        e.Effects = DragDropEffects.None;
    //        base.OnDragOver(e);
    //    }
    //    #endregion
    //}
    #endregion
}
