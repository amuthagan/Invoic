using Microsoft.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;

namespace BHCustCtrl
{
    [DefaultProperty("Columns")]
    [ContentProperty("Columns")]
    public class DataGridNumericColumn : DataGridTextColumn
    {

        public DataGridNumericColumn()
        {
            Style columnStyle = new Style(typeof(TextBlock));
            columnStyle.Setters.Add(new Setter(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center));
            ElementStyle = columnStyle;
        }

        object previouseCellValue = null;
        bool isCellValueChanged = false;
        private int backspaceAction = 0;
        //System.Windows.Controls.TextBox textBox;

        //       public static DependencyProperty _MaxPrecision = DependencyProperty.Register("MaxPrecision", typeof(int), typeof(DataGridNumericColumn),
        //new FrameworkPropertyMetadata(0) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });

        //       public int MaxPrecision
        //       {
        //           get { return (int)GetValue(_MaxPrecision); }
        //           set
        //           {
        //               if (value <= 0) value = 13;
        //               SetValue(_MaxPrecision, value);
        //           }
        //       }

        //       public static DependencyProperty _maxDecimalPlaces = DependencyProperty.Register("MaxScale", typeof(int), typeof(DataGridNumericColumn),
        //                     new FrameworkPropertyMetadata(0) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
        //       public int MaxDecimalPlaces
        //       {
        //           get { return (int)GetValue(_maxDecimalPlaces); }
        //           set
        //           {
        //               if (value <= 0) value = 2;
        //               SetValue(_maxDecimalPlaces, value);
        //           }
        //       }

        public static DependencyProperty _maxScale = DependencyProperty.Register("MaxScale", typeof(int), typeof(DataGridNumericColumn),
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

        public static DependencyProperty _maxPrecision = DependencyProperty.Register("MaxPrecision", typeof(int), typeof(DataGridNumericColumn),
                      new FrameworkPropertyMetadata(0) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
        public int MaxPrecision
        {
            get { return (int)GetValue(_maxPrecision); }
            set
            {
                SetValue(_maxPrecision, value);
            }
        }

        private ObservableCollection<DataGridTextColumn> columns;
        //The property is default and Content property for CustComboBox
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ObservableCollection<DataGridTextColumn> Columns
        {
            get
            {
                if (this.columns == null)
                {
                    this.columns = new ObservableCollection<DataGridTextColumn>();
                }
                return this.columns;
            }
        }


        protected override object PrepareCellForEdit(System.Windows.FrameworkElement editingElement, System.Windows.RoutedEventArgs editingEventArgs)
        {
            backspaceAction = 0;
            isCellValueChanged = false;
            TextBox tb = editingElement as System.Windows.Controls.TextBox;
            if (tb != null)
            {
                //if (MaxDecimalPlaces == 0)
                //    textBox.PreviewTextInput += OnPreviewTextInput_IntOnly;
                //else
                tb.VerticalContentAlignment = VerticalAlignment.Center;
                tb.PreviewTextInput += OnPreviewTextInput;

                tb.PreviewKeyDown += TextBox_PreviewKeyDown;
                //textBox.PreviewKeyUp += TextBox_PreviewKeyUp;
                tb.TextChanged += TextBox_TextChanged;
                //tb.PreviewGiveFeedback += TextBox_GiveFeedback;
                //tb.PreviewGiveFeedback += TextBox_GiveFeedback;
                //tb.PreviewMouseLeftButtonDown += TextBox_PreviewMouseLeftButtonDown;
                //tb.PreviewMouseMove += TextBox_PreviewMouseMove;
                tb.PreviewQueryContinueDrag += TextBox_QueryContinueDragEventHandler;
            }
            previouseCellValue = tb.Text;

            return base.PrepareCellForEdit(editingElement, editingEventArgs);
        }

        //public override void OnPastingCellClipboardContent(object item, object cellContent)
        //{
        //    base.OnPastingCellClipboardContent(item, cellContent);
        //}
        //public void QueryContinueDragEventHandler(Object sender, QueryContinueDragEventArgs e)
        //{

        //}

        //protected void TextBox_GiveFeedback(object sender, System.Windows.GiveFeedbackEventArgs e)
        //{
        //    TextBox tb = sender as System.Windows.Controls.TextBox;
        //    if (tb != null)
        //    {
        //        tb.SelectionLength = 0;
        //        tb.SelectedText = "";

        //    }
        //    e.UseDefaultCursors = true;
        //    e.Handled = true;
        //}

        //protected void GiveFeedbackEventHandler(Object sender, GiveFeedbackEventArgs e)
        //{
        //    //TextBox tb = sender as System.Windows.Controls.TextBox;
        //    //if (tb != null)
        //    //{
        //    //    tb.SelectionLength = 0;
        //    //    tb.SelectedText = "";

        //    //}
        //    //e.UseDefaultCursors = false;
        //    //e.Handled = true;
        //}

        //private Point _startingPoint;
        //private bool isDisableDragAndDrop = false;
        //void TextBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    isDisableDragAndDrop = false;
        //    TextBox tb = sender as System.Windows.Controls.TextBox;
        //    if (e.LeftButton == MouseButtonState.Pressed && tb != null && tb.SelectionLength > 0)
        //    {
        //        isDisableDragAndDrop = true;
        //        _startingPoint = e.GetPosition(null);
        //    }
        //}

        //void TextBox_PreviewMouseMove(object sender, MouseEventArgs e)
        //{
        //    if (!isDisableDragAndDrop) return;
        //    TextBox tb = sender as System.Windows.Controls.TextBox;
        //    if (e.LeftButton == MouseButtonState.Pressed && tb != null && tb.SelectionLength > 0 && tb.SelectionLength != tb.Text.Length)
        //    {

        //        Point position = e.GetPosition(null);

        //        if (Math.Abs(position.X - _startingPoint.X) > SystemParameters.MinimumHorizontalDragDistance ||
        //                Math.Abs(position.Y - _startingPoint.Y) > SystemParameters.MinimumVerticalDragDistance)
        //        {
        //            //e.Handled = true;
        //            tb.SelectionLength = 0;
        //            tb.SelectedText = "";
        //        }
        //    }
        //}

        void TextBox_QueryContinueDragEventHandler(object sender, QueryContinueDragEventArgs e)
        {
            e.Action = DragAction.Cancel;
            e.Handled = true;
            backspaceAction = 0;
        }

        private KeyConverter _keyConv = new KeyConverter();

        public void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            backspaceAction = 0;
            string keyPressed = _keyConv.ConvertToString(e.Key);
            TextBox tb = sender as TextBox;

            if (keyPressed != null)
            {
                isCellValueChanged = true;
                if (keyPressed.Length == 1 && char.IsControl(keyPressed[0]))
                {
                    e.Handled = false;
                }

                if (keyPressed == "Space")
                {
                    if (tb.SelectionLength == Convert.ToString(tb.Text).Length)
                    {
                        tb.Clear();
                        e.Handled = true;
                    }
                    keyPressed = "Delete";
                    //tb.Clear();
                    //e.Handled = true;
                }

                int selectionStart = tb.SelectionStart;
                dotPosition = tb.Text.IndexOf('.');

                if (keyPressed == "Delete")
                {

                    if (tb.Text.Length > 0 &&
                        (tb.SelectedText.Contains(".") || (tb.SelectedText.Length == 0 && tb.SelectionStart == dotPosition)) &&
                        tb.Text.Length != tb.SelectionLength)
                    {
                        tb.Text = tb.Text.Substring(0, Math.Min(selectionStart, tb.Text.Length));
                        tb.SelectionStart = Math.Min(selectionStart, tb.Text.Length);
                        e.Handled = true;
                    }
                }
                else if (keyPressed == "Backspace")
                {
                    if (tb.Text.Length > 0 && (tb.SelectedText.Contains(".") || (tb.SelectedText.Length == 0 && tb.SelectionStart - 1 == dotPosition)) && tb.Text.Length != tb.SelectionLength)
                    {
                        tb.Text = tb.Text.Substring(0, Math.Min(selectionStart, tb.Text.Length));

                        if (tb.SelectedText.Length == 0 && selectionStart - 1 == dotPosition && selectionStart - 1 >= 0)
                        {
                            tb.SelectionStart = Math.Min(selectionStart - 1, tb.Text.Length);
                        }
                        else
                        {
                            tb.SelectionStart = Math.Min(selectionStart, tb.Text.Length);
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
                    tb.SelectionStart = hasDecimalChar ? dotPosition + 1 : 0;
                    tb.SelectionLength = hasDecimalChar ? MaxScale : 0;
                    e.Handled = hasDecimalChar;
                    return;
                }
                else if ((keyPressed == "Up") && dotPosition > 0)
                {

                    dotPosition = tb.Text.IndexOf('.');

                    string currentPrecisionText = "";
                    string currentScaleText = "";
                    if (hasDecimalChar)
                    {
                        currentPrecisionText = tb.Text.Substring(0, dotPosition);
                        currentScaleText = tb.Text.Substring(dotPosition + 1);
                    }

                    tb.SelectionStart = 0;
                    tb.SelectionLength = hasDecimalChar ? Math.Min(dotPosition, currentPrecisionText.Length) : 0;
                    e.Handled = hasDecimalChar;
                    return;
                }
            }
        }

        //public void TextBox_PreviewKeyUp(object sender, KeyEventArgs e)
        //{
        //    TextBox tb = sender as TextBox;

        //    string keyPressed = _keyConv.ConvertToString(e.Key);
        //    if (keyPressed == "Delete" || keyPressed == "Backspace")
        //    {

        //    }
        //    //TextBox tb = sender as TextBox;

        //    //string keyPressed = _keyConv.ConvertToString(e.Key);
        //    //if (keyPressed == "Delete" || keyPressed == "Backspace")
        //    //{
        //    //    int selectionStart = textBox.SelectionStart;
        //    //    int no_of_chars = (MaxDecimalPlaces > 0 ? (MaxPrecision - MaxDecimalPlaces) : MaxPrecision);

        //    //    if (textBox.Text.Length > no_of_chars && textBox.Text.Length > 0 &&
        //    //        (textBox.SelectedText.Contains(".")) &&
        //    //        textBox.Text.Length != textBox.SelectionLength)
        //    //    {
        //    //        textBox.Text = textBox.Text.Substring(0, no_of_chars);
        //    //        textBox.SelectionStart = Math.Min(selectionStart, textBox.Text.Length);
        //    //    }
        //    //    else if (keyPressed == "Backspace" && textBox.Text.Length > no_of_chars && textBox.Text.Length > 0)
        //    //    {
        //    //        dotPosition = textBox.Text.IndexOf('.');
        //    //        if (dotPosition == -1)
        //    //        {
        //    //            no_of_chars--;
        //    //            textBox.Text = textBox.Text.Substring(0, Math.Min(no_of_chars, textBox.Text.Length));
        //    //        }
        //    //        //if (dotPosition > -1) no_of_chars++;
        //    //        //textBox.Text = textBox.Text.Substring(0, Math.Min(no_of_chars, textBox.Text.Length));
        //    //        textBox.SelectionStart = Math.Min(selectionStart, textBox.Text.Length);
        //    //    }

        //    //    isCellValueChanged = true;
        //    //}

        //    ////try
        //    ////{
        //    ////    string decimalFormat = "0." + "".PadLeft(MaxDecimalPlaces, '0');
        //    ////    if (Convert.ToString(textBox.Text).Trim().Length == 0) textBox.Text = "0";

        //    ////    double doubleOutput;
        //    ////    if (double.TryParse(textBox.Text, out doubleOutput) && doubleOutput == 0)
        //    ////    {
        //    ////        textBox.Text = doubleOutput.ToString(decimalFormat);
        //    ////    }
        //    ////}
        //    ////catch (Exception)
        //    ////{

        //    ////}
        //}

        public void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                TextBox tb = sender as TextBox;

                if (tb == null) return;

                int selectionStart = tb.SelectionStart;
                int selectionLength = tb.SelectionLength;
                string selectedText = tb.SelectedText;
                string newText = MaxScale == 0 ? TryParseInt(tb.Text) : TryParseDouble(tb.Text);
                if (tb.Text != newText && Convert.ToString(tb.Text).Length > 0)
                {
                    isCellValueChanged = true;
                    //SetValue(TextBox.TextProperty, newText);
                    tb.Text = newText;

                    dotPosition = tb.Text.IndexOf('.');

                    string currentPrecisionText = "";
                    string currentScaleText = "";
                    if (hasDecimalChar)
                    {
                        currentPrecisionText = newText.Substring(0, dotPosition);
                        currentScaleText = newText.Substring(dotPosition + 1);
                    }

                    if (hasDecimalChar && currentPrecisionText.Length != 1 && selectionStart != Convert.ToString(tb.Text).Length && selectionStart <= dotPosition)
                        tb.SelectionStart = selectionStart - 1 < 0 ? 0 : selectionStart - 1;
                    else if (hasDecimalChar && currentPrecisionText.Length == 1 && selectionStart != Convert.ToString(tb.Text).Length && selectionStart > dotPosition)
                        tb.SelectionStart = selectionStart - 1 < 0 ? 0 : selectionStart - 1 + backspaceAction;
                    else if (hasDecimalChar && selectionStart != Convert.ToString(tb.Text).Length && selectionStart <= dotPosition)
                        tb.SelectionStart = Math.Min(selectionStart, dotPosition);
                    else if (hasDecimalChar && selectionStart != Convert.ToString(tb.Text).Length && selectionStart > dotPosition)
                        tb.SelectionStart = Math.Min(selectionStart, tb.Text.Length);
                    else
                        tb.SelectionStart = selectionStart;

                    tb.SelectionLength = selectionLength;
                    tb.SelectedText = selectedText;
                }
                else
                {
                    if (tb.Text != newText && Convert.ToString(tb.Text).Length == 0) tb.Text = newText;
                    tb.SelectionStart = selectionStart;
                }
            }
            catch (Exception)
            {
            }
        }

        //public bool OverrideMaxPrecisionExeceeds = true;
        //public bool OverrideFirstDegitIsZero = true;
        //void OnPreviewTextInput_IntOnly(object sender, System.Windows.Input.TextCompositionEventArgs e)
        //{
        //    try
        //    {
        //        TextBox tb = (sender as TextBox);
        //        if (tb == null) e.Handled = true;

        //        char[] Chars = e.Text.ToCharArray();
        //        if (Chars.Length == 0) e.Handled = false;

        //        char currentChar = Chars[0];
        //        bool isCharAllowed = !char.IsControl(currentChar) & !char.IsDigit(currentChar);

        //        int selectionStart = tb.SelectionStart;
        //        int selectionLength = tb.SelectionLength;
        //        int currentLength = Convert.ToString(tb.Text).Length;
        //        string currentText = tb.Text.Insert(selectionStart, currentChar.ToString());

        //        bool isFirstCharZero = tb.Text.Length > 0 && tb.Text.Substring(0, 1) == "0" && currentText.Length > 0 && currentText.Substring(0, 1) == "0";

        //        if ((isFirstCharZero && OverrideFirstDegitIsZero) && tb.SelectionStart <= 1)
        //        {
        //            tb.SelectionStart = 0; tb.SelectionLength = 1;
        //            e.Handled = isCharAllowed;
        //            if (!e.Handled) isCellValueChanged = true;
        //            return;
        //        }

        //        bool canOverrides = false;
        //        int no_of_chars = MaxPrecision - MaxScale;
        //        if (currentText.Length > no_of_chars)
        //        {
        //            e.Handled = true;
        //            if (OverrideMaxPrecisionExeceeds && selectionStart < no_of_chars && selectionLength <= 1)
        //            {
        //                canOverrides = tb.Text.Length > 0 && tb.Text.Substring(0, 1) != "0" && currentText.Length > 0 && currentText.Substring(0, 1) == "0";
        //                e.Handled = isCharAllowed;
        //                if (canOverrides)
        //                {
        //                    e.Handled = true;
        //                    tb.SelectionLength = OverrideMaxPrecisionExeceeds && !canOverrides ? selectionLength == 0 ? 1 : selectionLength : 0;
        //                }
        //                else if (OverrideMaxPrecisionExeceeds)
        //                    tb.SelectionLength = OverrideMaxPrecisionExeceeds && !canOverrides ? selectionLength == 0 ? 1 : selectionLength : 0;
        //            }
        //            else if (!OverrideMaxPrecisionExeceeds && currentText.Length > no_of_chars && selectionLength <= 1)
        //            {
        //                canOverrides = tb.Text.Length > 0 && tb.Text.Substring(0, 1) != "0" && currentText.Length > 0 && currentText.Substring(0, 1) == "0";
        //                e.Handled = true;
        //                tb.SelectionLength = !canOverrides ? selectionLength == 0 ? 1 : selectionLength : 0;

        //            }
        //            else if (!OverrideMaxPrecisionExeceeds && selectionStart < no_of_chars && selectionLength > 0)
        //            {
        //                canOverrides = tb.Text.Length > 0 && tb.Text.Substring(0, 1) != "0" && currentText.Length > 0 && currentText.Substring(0, 1) == "0";
        //                e.Handled = isCharAllowed;
        //                tb.SelectionLength = !canOverrides ? selectionLength == 0 ? 1 : selectionLength : 0;

        //            }
        //            if (!e.Handled) isCellValueChanged = true;
        //            return;
        //        }

        //        canOverrides = tb.Text.Length > 0 && tb.Text.Substring(0, 1) != "0" && currentText.Length > 0 && currentText.Substring(0, 1) == "0";
        //        if (canOverrides)
        //        {
        //            e.Handled = true;
        //        }
        //        //else if (OverrideMaxPrecisionExeceeds)
        //        //    tb.SelectionLength = OverrideMaxPrecisionExeceeds && !canOverrides ? selectionLength == 0 ? 1 : selectionLength : 0;

        //        isFirstCharZero = tb.Text.Length > 0 && tb.Text.Substring(0, 1) == "0";
        //        if (tb.SelectionLength == 0 && (!isFirstCharZero && OverrideFirstDegitIsZero))
        //            e.Handled = isValidKey(sender, e);

        //        if (!e.Handled && !isCellValueChanged) isCellValueChanged = true;
        //    }
        //    catch
        //    {
        //        e.Handled = true;
        //    }
        //}

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

        bool hasDecimalChar = false;

        //void OnPreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        //{
        //    try
        //    {
        //        TextBox tb = (sender as TextBox);
        //        if (tb == null) e.Handled = true;

        //        char[] Chars = e.Text.ToCharArray();
        //        if (Chars.Length == 0) e.Handled = false;

        //        char currentChar = Chars[0];
        //        bool isCharAllowed = !char.IsControl(currentChar) && (!char.IsDigit(currentChar) && MaxDecimalPlaces == 0) ||
        //                                                             ((!char.IsDigit(currentChar) || currentChar != '.') && MaxDecimalPlaces > 0);

        //        int selectionStart = tb.SelectionStart;
        //        int selectionLength = tb.SelectionLength;
        //        int currentLength = Convert.ToString(tb.Text).Length;
        //        string currentText = tb.Text.Insert(selectionStart, currentChar.ToString());

        //        bool isFirstCharZero = tb.Text.Length > 0 && tb.Text.Substring(0, 1) == "0" && currentText.Length > 0 && currentText.Substring(0, 1) == "0";

        //        if ((isFirstCharZero && OverrideFirstDegitIsZero) && tb.SelectionStart <= 1)
        //        {
        //            tb.SelectionStart = 0; tb.SelectionLength = 1;
        //            e.Handled = isCharAllowed;
        //            if (!e.Handled) isCellValueChanged = true;
        //            return;
        //        }

        //        bool canOverrides = false;
        //        dotPosition = currentText.IndexOf('.');
        //        int no_of_chars = (!hasDecimalChar || MaxDecimalPlaces == 0 ? MaxPrecision - MaxDecimalPlaces : MaxPrecision + MaxDecimalPlaces + 1);
        //        if (currentText.Length > no_of_chars)
        //        {
        //            e.Handled = true;
        //            //if (OverrideMaxPrecisionExeceeds && selectionStart < no_of_chars && selectionLength <= 1)
        //            //{
        //            //    canOverrides = tb.Text.Length > 0 && tb.Text.Substring(0, 1) != "0" && currentText.Length > 0 && currentText.Substring(0, 1) == "0";
        //            //    e.Handled = isCharAllowed;
        //            //    if (canOverrides)
        //            //    {
        //            //        //e.Handled = true;
        //            //        e.Handled = false;
        //            //        tb.SelectionLength = OverrideMaxPrecisionExeceeds && !canOverrides ? selectionLength == 0 ? 1 : selectionLength : canOverrides ? 1 : 0;
        //            //    }
        //            //    else if (OverrideMaxPrecisionExeceeds)
        //            //        tb.SelectionLength = OverrideMaxPrecisionExeceeds && !canOverrides ? selectionLength == 0 ? 1 : selectionLength : canOverrides ? 1 : 0;
        //            //}
        //            //else if (!OverrideMaxPrecisionExeceeds && currentText.Length > no_of_chars && selectionLength <= 1)
        //            //{
        //            //    canOverrides = tb.Text.Length > 0 && tb.Text.Substring(0, 1) != "0" && currentText.Length > 0 && currentText.Substring(0, 1) == "0";
        //            //    e.Handled = true;
        //            //    tb.SelectionLength = !canOverrides ? selectionLength == 0 ? 1 : selectionLength : 0;

        //            //}
        //            //else if (!OverrideMaxPrecisionExeceeds && selectionStart < no_of_chars && selectionLength > 0)
        //            //{
        //            //    canOverrides = tb.Text.Length > 0 && tb.Text.Substring(0, 1) != "0" && currentText.Length > 0 && currentText.Substring(0, 1) == "0";
        //            //    e.Handled = isCharAllowed;
        //            //    tb.SelectionLength = !canOverrides ? selectionLength == 0 ? 1 : selectionLength : 0;

        //            //}
        //            //if (!e.Handled) isCellValueChanged = true;
        //            return;
        //        }

        //        canOverrides = tb.Text.Length > 0 && tb.Text.Substring(0, 1) != "0" && currentText.Length > 0 && currentText.Substring(0, 1) == "0";
        //        if (canOverrides)
        //        {
        //            e.Handled = true;
        //        }
        //        //else if (OverrideMaxPrecisionExeceeds)
        //        //    tb.SelectionLength = OverrideMaxPrecisionExeceeds && !canOverrides ? selectionLength == 0 ? 1 : selectionLength : 0;

        //        isFirstCharZero = tb.Text.Length > 0 && tb.Text.Substring(0, 1) == "0";
        //        if (tb.SelectionLength == 0 && (!isFirstCharZero && OverrideFirstDegitIsZero))
        //            e.Handled = isValidKey(sender, e);
        //        else if (tb.SelectionLength == 0 && (isFirstCharZero && OverrideFirstDegitIsZero))
        //            e.Handled = isValidKey(sender, e);

        //        if (!e.Handled && !isCellValueChanged) isCellValueChanged = true;
        //    }
        //    catch
        //    {
        //        e.Handled = true;
        //    }
        //}

        void OnPreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            try
            {
                backspaceAction = 0;
                TextBox tb = (sender as TextBox);
                if (tb == null) e.Handled = true;

                char[] chars = e.Text.ToCharArray();
                if (chars.Length == 0) e.Handled = false;

                char currentChar = chars[0];
                bool isCharAllowed = !char.IsControl(currentChar) && (!char.IsDigit(currentChar) && MaxScale == 0) ||
                                                                     ((!char.IsDigit(currentChar) || currentChar != '.') && MaxScale > 0);
                int selectionStart = tb.SelectionStart;
                int selectionLength = tb.SelectionLength;
                string selectedText = tb.SelectedText;
                int currentLength = Convert.ToString(tb.Text).Length;

                //textBox.Text = textBox.Text.Remove(selectionStart, Math.Min(selectionLength, textBox.Text.Length - selectionStart)).
                //               Insert(selectionStart, currentChar.ToString());

                string currentText = tb.Text;

                dotPosition = currentText.IndexOf('.');
                bool isFirstCharZero = tb.Text.Length > 0 && tb.Text.Substring(0, 1) == "0" || (tb.SelectionStart == 0 && currentChar == '0');

                if (isFirstCharZero && selectionLength != Convert.ToString(tb.Text).Length)
                {
                    dotPosition = tb.Text.IndexOf('.');
                    if (hasDecimalChar)
                    {
                        if (selectionStart > dotPosition)
                        {
                            if (selectionStart > dotPosition) selectionLength = 1;
                            string remainingText = tb.Text.Remove(selectionStart, Math.Min(selectionLength, tb.Text.Length - selectionStart));
                            currentText = remainingText.Insert(Math.Min(selectionStart, remainingText.Length), currentChar.ToString());
                            e.Handled = !IsNumeric(currentText);
                            if (!e.Handled)
                            {
                                tb.Text = MaxScale == 0 ? TryParseInt(currentText) : TryParseDouble(currentText);

                                dotPosition = tb.Text.IndexOf('.');
                                tb.SelectionStart = hasDecimalChar ? (selectionStart > dotPosition ? selectionStart + 1 : dotPosition + 1) : 0;
                                //tb.SelectionLength = hasDecimalChar ? MaxScale : 0;
                                tb.SelectionLength = 1;
                                e.Handled = true;
                            }
                            else if (e.Handled && currentChar == '.')
                            {
                                dotPosition = tb.Text.IndexOf('.');
                                tb.SelectionStart = hasDecimalChar ? dotPosition + 1 : 0;
                                tb.SelectionLength = hasDecimalChar ? MaxScale : 0;
                            }
                            return;
                        }
                        else
                        {
                            currentText = tb.Text.Remove(selectionStart, Math.Min(selectionLength, tb.Text.Length - selectionStart)).Insert(selectionStart + 1, currentChar.ToString());
                            tb.SelectionStart = 1;
                            tb.SelectionLength = 1;
                        }

                    }
                }
                else if (selectionLength != Convert.ToString(tb.Text).Length && currentChar != '.' && isCharAllowed && tb.SelectionStart != currentLength)
                {
                    if (selectionStart > dotPosition) selectionLength = 1;
                    string remainingText = tb.Text.Remove(selectionStart, Math.Min(selectionLength, tb.Text.Length - selectionStart));
                    currentText = remainingText.Insert(Math.Min(selectionStart, remainingText.Length), currentChar.ToString());
                    //tb.Text = currentText;
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
                            tb.Text = currentText;
                            tb.SelectionStart = hasDecimalChar ? (selectionStart > dotPosition ? selectionStart + 1 : dotPosition + 1) : 0;
                            tb.SelectionLength = selectionLength;
                            return;
                        }
                    }

                    currentText = tb.Text.Remove(selectionStart, Math.Min(selectionLength, tb.Text.Length - selectionStart)).Insert(selectionStart, currentChar.ToString());
                    dotPosition = currentText.IndexOf('.');
                    if (selectionStart >= dotPosition && MaxScale > 0)
                    {
                        tb.SelectionLength = hasDecimalChar ? 1 : 0;
                    }
                }
                //else if (selectionLength == Convert.ToString(tb.Text).Length && currentChar == '.')
                else if (currentChar == '.' && MaxScale > 0)
                {
                    if (Convert.ToString(tb.Text).Length == 0)
                        tb.Text = MaxScale == 0 ? TryParseInt(tb.Text) : TryParseDouble(tb.Text);

                    dotPosition = tb.Text.IndexOf('.');
                    if (selectionStart < dotPosition && selectionLength == currentLength && isCharAllowed)
                    {
                        tb.Text = string.Empty;
                        tb.Text = MaxScale == 0 ? TryParseInt(tb.Text) : TryParseDouble(tb.Text);
                        tb.SelectionStart = hasDecimalChar ? (selectionStart > dotPosition ? selectionStart + 1 : dotPosition + 1) : 0;
                        tb.SelectionLength = 1;
                        e.Handled = true;
                        return;
                    }
                    else
                    {
                        tb.SelectionStart = hasDecimalChar ? dotPosition + 1 : 0;
                        //tb.SelectionLength = hasDecimalChar ? MaxScale : 0;
                        tb.SelectionLength = hasDecimalChar ? 1 : 0;
                    }
                    //tb.SelectionLength = 1;
                    e.Handled = hasDecimalChar;
                    return;
                }

                dotPosition = currentText.IndexOf('.');
                int no_of_chars = (!hasDecimalChar || MaxScale == 0 ? MaxPrecision - MaxScale : MaxPrecision + MaxScale + 1);
                if (currentText.Length > no_of_chars)
                {
                    e.Handled = isValidKey(sender, e);
                    return;
                }

                if (selectionLength > 1)
                {
                    e.Handled = !IsNumeric(currentText);
                }
                else
                {
                    e.Handled = isValidKey(sender, e);
                }

                //if (!e.Handled && !isCellValueChanged) isCellValueChanged = true;
            }
            catch
            {
                e.Handled = true;
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
            char[] Chars = e.Text.ToCharArray();
            if (Chars.Length == 0) return bReturnValue;

            char currentChar = Chars[0];
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
            //string currentText = tb.Text.Insert(selectionStart, currentChar.ToString());
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
                    //string decimalFormat = "0." + "".PadLeft(MaxScale, '0');
                    //if (Convert.ToString(tb.Text).Trim().Length == 0) currentText = "0";

                    //double doubleOutput;
                    //if (double.TryParse(currentText, out doubleOutput))
                    //{
                    //    tb.Text = doubleOutput.ToString(decimalFormat);
                    //    currentText = tb.Text;
                    //}

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

            //int dotPosition = -1;
            //if (!char.IsDigit(currentChar) && (currentChar == '.' && tb.SelectionStart == 0))
            //{
            //    try
            //    {
            //        string decimalFormat = "0." + "".PadLeft(MaxDecimalPlaces, '0');
            //        if (Convert.ToString(textBox.Text).Trim().Length == 0) textBox.Text = "0";

            //        double doubleOutput;
            //        if (double.TryParse(textBox.Text, out doubleOutput))
            //        {
            //            textBox.Text = doubleOutput.ToString(decimalFormat);
            //        }

            //        dotPosition = textBox.Text.IndexOf('.');
            //        tb.SelectionStart = dotPosition < 0 ? 0 : dotPosition + 1;
            //    }
            //    catch (Exception)
            //    {

            //    }
            //}

            //dotPosition = textBox.Text.IndexOf('.');

            //if (char.IsDigit(currentChar) && (tb.SelectionStart <= dotPosition) && dotPosition > -1 &&
            //    tb.Text.Length > dotPosition && tb.Text.Substring(dotPosition + 1).Length < MaxDecimalPlaces)
            //{
            //    try
            //    {
            //        string decimalFormat = "0." + "".PadLeft(MaxDecimalPlaces, '0');
            //        if (Convert.ToString(textBox.Text).Trim().Length == 0) textBox.Text = "0";

            //        double doubleOutput;
            //        if (double.TryParse(textBox.Text, out doubleOutput))
            //        {
            //            selectionStart = textBox.SelectionStart;
            //            textBox.Text = doubleOutput.ToString(decimalFormat);
            //            tb.SelectionStart = Math.Min(selectionStart, tb.Text.Length);
            //        }
            //        int no_of_chars = (MaxDecimalPlaces > 0 ? (MaxPrecision - MaxDecimalPlaces) : MaxPrecision);

            //        //dotPosition = textBox.Text.IndexOf('.');
            //        //tb.SelectionStart = dotPosition < 0 ? 0 : dotPosition + 1;
            //        if (tb.Text.Substring(0, dotPosition).Length >= no_of_chars - 1)
            //        {
            //            bReturnValue = true;
            //            return bReturnValue;
            //        }
            //    }
            //    catch (Exception)
            //    {

            //    }
            //}

            //#region Find . & Move Position
            //if (!char.IsDigit(currentChar) && (currentChar == '.' && dotPosition < 0 && tb.SelectionStart <= MaxPrecision - MaxDecimalPlaces) && tb.Text.Length > 0)
            //{
            //    if (Convert.ToString(textBox.Text).Trim().Length > 0 && dotPosition < 0)
            //    {
            //        int no_of_chars = (MaxDecimalPlaces > 0 ? (MaxPrecision - MaxDecimalPlaces) : MaxPrecision);
            //        dotPosition = textBox.Text.IndexOf('.');
            //        if (dotPosition == -1) no_of_chars--;
            //        if (textBox.Text.Length > no_of_chars && textBox.Text.Length > 0 &&
            //        textBox.Text.Length != textBox.SelectionLength)
            //        {
            //            textBox.Text = textBox.Text.Substring(0, no_of_chars);
            //        }
            //        textBox.Text = textBox.Text + currentChar + "".PadLeft(MaxDecimalPlaces, '0');
            //        dotPosition = textBox.Text.IndexOf('.');
            //        tb.SelectionStart = dotPosition < 0 ? 0 : dotPosition + 1;
            //        bReturnValue = true;
            //        return bReturnValue;
            //    }
            //    tb.SelectionStart = tb.Text.Length;
            //}

            //if (!char.IsDigit(currentChar) && (currentChar == '.' && dotPosition > 0 && tb.SelectionStart <= MaxPrecision - MaxDecimalPlaces) && tb.Text.Length > 0)
            //{
            //    if (Convert.ToString(textBox.Text).Trim().Length > 0 && dotPosition < 0)
            //    {
            //        dotPosition = textBox.Text.IndexOf('.');
            //        tb.SelectionStart = dotPosition < 0 ? 0 : dotPosition + 1;
            //        tb.SelectionLength = 1;
            //        bReturnValue = true;
            //        return bReturnValue;
            //    }
            //    tb.SelectionStart = tb.Text.Length;
            //}

            //if (!char.IsDigit(currentChar) && (currentChar == '.' && dotPosition > 0 && tb.SelectionStart > dotPosition) && tb.Text.Length > 0)
            //{
            //    if (Convert.ToString(textBox.Text).Trim().Length > 0)
            //    {
            //        dotPosition = textBox.Text.IndexOf('.');
            //        tb.SelectionStart = dotPosition < 0 ? 0 : dotPosition + 1;
            //        tb.SelectionLength = 1;
            //        bReturnValue = true;
            //        return bReturnValue;
            //    }
            //    tb.SelectionStart = tb.Text.Length;
            //}
            //#endregion
            //// only allow one decimal point
            //if ((currentChar == '.') && dotPosition > -1)
            //{
            //    bReturnValue = true;
            //    return bReturnValue;
            //}

            //bool b = (tb.Text + currentChar).Length >= (dotPosition > -1 ? MaxPrecision + MaxDecimalPlaces + 2 : MaxPrecision - MaxDecimalPlaces);

            //bool isFirstCharZero = tb.Text.Length > 0 && tb.Text.Substring(0, 1) == "0" || (tb.SelectionStart == 0 && currentChar == '0');
            //bool isFirstAndSecordCharZero = isFirstCharZero && (tb.Text.Length > 1 && tb.Text.Substring(1, 1) == "0" || (tb.SelectionStart == 1 && currentChar == '0'));

            //if (char.IsDigit(currentChar) && dotPosition > -1 &&
            //    tb.SelectionStart <= dotPosition &&
            //    tb.Text.Substring(0, dotPosition - 1 < 0 ? 0 : dotPosition - 1).Length < MaxPrecision - (MaxDecimalPlaces + 2))
            //{
            //    bReturnValue = false;
            //    if (char.IsDigit(currentChar) && dotPosition > -1 &&
            //        tb.SelectionStart < dotPosition &&
            //       (tb.Text.Substring(0, dotPosition - 1 < 0 ? 0 : dotPosition - 1).Length == MaxPrecision - (MaxDecimalPlaces + 2)
            //        || selectionLength > 0))
            //    {
            //        int no_of_chars = (MaxDecimalPlaces > 0 ? (MaxPrecision - MaxDecimalPlaces) : MaxPrecision);

            //        textBox.Text = textBox.Text.Remove(selectionStart, Math.Min(selectionLength, textBox.Text.Length - selectionStart)).
            //                       Insert(selectionStart, currentChar.ToString());

            //        dotPosition = textBox.Text.IndexOf('.');
            //        //if (keyPressed == "Backspace" && dotPosition >= -1)
            //        if (dotPosition >= -1)
            //        {
            //            no_of_chars--;
            //        }
            //        textBox.Text = textBox.Text.Substring(0, Math.Min(no_of_chars, textBox.Text.Length));
            //        textBox.SelectionStart = Math.Min(selectionStart, textBox.Text.Length) + 1;
            //        bReturnValue = true;
            //        return bReturnValue;
            //    }
            //    return bReturnValue;
            //}

            //if (char.IsDigit(currentChar) && (currentChar != '.') && (tb.SelectionStart > dotPosition) && !b)
            //{
            //    if (isFirstAndSecordCharZero)
            //    {
            //        bReturnValue = true;
            //        return bReturnValue;
            //    }
            //    tb.SelectionLength = 1;
            //    bReturnValue = (tb.SelectionStart - dotPosition > MaxDecimalPlaces) && dotPosition > -1;
            //    return bReturnValue;
            //}


            ////if (char.IsDigit(currentChar) && dotPosition > -1 &&
            ////    tb.SelectionStart <= dotPosition &&
            ////    tb.Text.Substring(0, dotPosition - 1 < 0 ? 0 : dotPosition - 1).Length < MaxPrecision - (MaxDecimalPlaces + 2) &&
            ////    tb.Text.Length > 0 &&
            ////    tb.Text.Substring(0, 1) == "0")
            ////{
            ////    bReturnValue = false;
            ////    return bReturnValue;
            ////}

            //if (char.IsDigit(currentChar) && dotPosition > -1 &&
            //    tb.Text.Length > dotPosition + MaxDecimalPlaces && tb.Text.Substring(dotPosition + 1, MaxDecimalPlaces).Length >= MaxDecimalPlaces)
            //{
            //    bReturnValue = true;
            //    if (char.IsDigit(currentChar) && dotPosition > -1 &&
            //         tb.SelectionStart < dotPosition &&
            //         tb.Text.Substring(0, dotPosition - 1 < 0 ? 0 : dotPosition - 1).Length == MaxPrecision - (MaxDecimalPlaces + 2) &&
            //         isFirstCharZero)
            //    {
            //        tb.SelectionLength = 1;
            //        bReturnValue = false;
            //        return bReturnValue;
            //    }

            //    if (char.IsDigit(currentChar) && dotPosition > -1 &&
            //        tb.SelectionStart < dotPosition &&
            //       (tb.Text.Substring(0, dotPosition - 1 < 0 ? 0 : dotPosition - 1).Length == MaxPrecision - (MaxDecimalPlaces + 2)
            //        || selectionLength > 0))
            //    {
            //        int no_of_chars = (MaxDecimalPlaces > 0 ? (MaxPrecision - MaxDecimalPlaces) : MaxPrecision);

            //        textBox.Text = textBox.Text.Remove(selectionStart, Math.Min(selectionLength, textBox.Text.Length - selectionStart)).
            //                       Insert(selectionStart, currentChar.ToString());

            //        dotPosition = tb.Text.IndexOf('.');
            //        if (dotPosition >= -1 && selectedText.IndexOf('.') == -1 && !b)
            //        {
            //            textBox.Text = textBox.Text.Remove(selectionStart, 1);

            //            textBox.SelectionStart = Math.Min(selectionStart, textBox.Text.Length);
            //            bReturnValue = true;
            //            return bReturnValue;
            //        }

            //        if (dotPosition >= -1)
            //        {
            //            no_of_chars--;
            //            textBox.Text = textBox.Text.Substring(0, Math.Min(no_of_chars, textBox.Text.Length));
            //        }
            //        textBox.SelectionStart = Math.Min(selectionStart, textBox.Text.Length) + 1;
            //        bReturnValue = true;
            //        return bReturnValue;
            //    }
            //    return bReturnValue;
            //}

            //if (char.IsDigit(currentChar) && b)
            //{
            //    bReturnValue = true; return bReturnValue;
            //}

            //return bReturnValue;
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

        private bool formatCellAfterCellEndEdit = true;
        public bool FormatCellAfterCellEndEdit
        {
            get { return formatCellAfterCellEndEdit; }
            set { formatCellAfterCellEndEdit = value; }
        }

        protected override bool CommitCellEdit(System.Windows.FrameworkElement editingElement)
        {
            TextBox tb = editingElement as TextBox;
            backspaceAction = 0;
            if (tb == null) return true;
            if (!isCellValueChanged) tb.Text = Convert.ToString(previouseCellValue);

            if (MaxScale > 0 && FormatCellAfterCellEndEdit)
            {
                if (Convert.ToString(tb.Text).Trim().Length == 0) tb.Text = "0";
                string decimalFormat = "0." + "".PadLeft(MaxScale, '0');
                tb.Text = Convert.ToDecimal(Convert.ToString(tb.Text)).ToString(decimalFormat);
            }

            BindingExpression binding = editingElement.GetBindingExpression(TextBox.TextProperty);
            if (binding != null)
                binding.UpdateSource();

            return true; //base.CommitCellEdit(editingElement);
        }

        public bool IsNumeric(string val, System.Globalization.NumberStyles numberStyle = System.Globalization.NumberStyles.Number)
        {
            if (Convert.ToString(val).Length == 0) return true;
            if (Convert.ToString(val).Split('.').Length - 1 > 1) return false;
            Double result;
            return Double.TryParse(val, numberStyle,
                System.Globalization.CultureInfo.CurrentCulture, out result);
        }


    }
}
