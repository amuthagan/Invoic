using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using ProcessDesigner.Common;

namespace ProcessDesigner.UserControls
{
    public class NumericTextBox : TextBox
    {
        public NumericTextBox()
        {
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Cut, null, CanExecuteCut)); //surpress cut
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, CopyExecuted)); //handle paste
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, PasteExecuted)); //handle paste
        }

        //string decimalSeparator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

        //private char DecimalChar = '.';

        object previouseCellValue = null;
        bool isCellValueChanged = false;
        private static DependencyProperty _maxScale = DependencyProperty.Register("MaxScale", typeof(int), typeof(NumericTextBox),
             new FrameworkPropertyMetadata(0) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
        public int MaxScale
        {
            get { return (int)GetValue(_maxScale); }
            set
            {
                if (value <= 0) value = 2;
                SetValue(_maxScale, value);
            }
        }

        private static DependencyProperty _maxPrecision = DependencyProperty.Register("MaxPrecision", typeof(int), typeof(NumericTextBox),
                      new FrameworkPropertyMetadata(0) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
        public int MaxPrecision
        {
            get { return (int)GetValue(_maxPrecision); }
            set
            {
                SetValue(_maxPrecision, value);
            }
        }

        protected override void OnQueryContinueDrag(QueryContinueDragEventArgs e)
        {
            e.Action = DragAction.Cancel;
            e.Handled = true;
            base.OnQueryContinueDrag(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (Convert.ToString(Text).Trim().Length == 0) Text = "0";
            if (Convert.ToDouble(Text) == 0)
            {
                SelectionStart = 0;
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
            string keyPressed = _keyConv.ConvertToString(e.Key);

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
                    //keyPressed = "Delete";
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

                char[] chars = e.Text.ToCharArray();
                if (chars.Length == 0) e.Handled = false;

                char currentChar = chars[0];
                bool isCharAllowed = !char.IsControl(currentChar) && (!char.IsDigit(currentChar) && MaxScale == 0) ||
                                                                     ((!char.IsDigit(currentChar) || currentChar != '.') && MaxScale > 0);
                int selectionStart = SelectionStart;
                int selectionLength = SelectionLength;
                string selectedText = SelectedText;
                int currentLength = Convert.ToString(Text).Length;

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
                        SelectionStart = selectionStart - 1 < 0 ? 0 : selectionStart - 1;
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
                    if (Text != newText && Convert.ToString(Text).Length == 0) Text = newText;
                    SelectionStart = selectionStart;
                }
            }
            catch (Exception)
            {
            }
            base.OnTextChanged(e);
        }
        protected override void OnGotFocus(RoutedEventArgs e)
        {
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
            if (!isCellValueChanged) Text = Convert.ToString(previouseCellValue);

            if (MaxScale > 0 && FormatTextAfterLostFocus)
            {
                if (Convert.ToString(Text).Trim().Length == 0) Text = "0";
                string decimalFormat = "0." + "".PadLeft(MaxScale, '0');
                Text = Convert.ToDecimal(Convert.ToString(Text)).ToString(decimalFormat);
            }

            BindingExpression binding = this.GetBindingExpression(TextBox.TextProperty);
            if (binding != null)
                binding.UpdateSource();

            base.OnLostFocus(e);
        }

        private void CanExecuteCut(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            e.Handled = true;

            //int selectionStart = SelectionStart;
            //dotPosition = Text.IndexOf('.');
            //e.CanExecute = false;
            //if (Text.Length > 0 &&
            //            (SelectedText.Contains(".") || (SelectedText.Length == 0 && SelectionStart == dotPosition)) &&
            //            Text.Length != SelectionLength)
            //{
            //    Text = Text.Substring(0, Math.Min(selectionStart, Text.Length));
            //    SelectionStart = Math.Min(selectionStart, Text.Length);
            //    e.Handled = true;
            //}
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

    }
}
