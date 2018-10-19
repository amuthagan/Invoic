using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Controls.Primitives;
using System.Text.RegularExpressions;

namespace ProcessDesigner.UICommon
{

    public partial class NumericTextBoxBehavior : Behavior<TextBox>
    {
        private bool _allowDecimal = true;
        private int _decimalLimit = 0;
        private bool _allowNegative = true;
        private string _pattern = string.Empty;

        /// <summary>
        /// Initialize a new instance of <see cref="NumericTextBoxBehavior"/>.
        /// </summary>
        public NumericTextBoxBehavior()
        {
            AllowDecimal = true;
            AllowNegatives = true;
            DecimalLimit = 0;
        }

        /// <summary>
        /// Get or set whether the input allows decimal characters.
        /// </summary>
        public bool AllowDecimal
        {
            get
            {
                return _allowDecimal;
            }
            set
            {
                if (_allowDecimal == value) return;
                _allowDecimal = value;
                SetText();
            }
        }
        /// <summary>
        /// Get or set the maximum number of values to appear after
        /// the decimal.
        /// </summary>
        /// <remarks>
        /// If DecimalLimit is 0, then no limit is applied.
        /// </remarks>
        public int DecimalLimit
        {
            get
            {
                return _decimalLimit;
            }
            set
            {
                if (_decimalLimit == value) return;
                _decimalLimit = value;
                SetText();
            }
        }
        /// <summary>
        /// Get or set whether negative numbers are allowed.
        /// </summary>
        public bool AllowNegatives
        {
            get
            {
                return _allowNegative;
            }
            set
            {
                if (_allowNegative == value) return;
                _allowNegative = value;
                SetText();
            }
        }

        #region Overrides
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.PreviewTextInput += new TextCompositionEventHandler(AssociatedObject_PreviewTextInput);
#if !SILVERLIGHT
            DataObject.AddPastingHandler(AssociatedObject, OnClipboardPaste);
#endif
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PreviewTextInput -= new TextCompositionEventHandler(AssociatedObject_PreviewTextInput);
#if !SILVERLIGHT
            DataObject.RemovePastingHandler(AssociatedObject, OnClipboardPaste);
#endif
        }
        #endregion

        #region Private methods
        private void SetText()
        {
            _pattern = string.Empty;
            GetRegularExpressionText();
        }

#if !SILVERLIGHT
        /// <summary>
        /// Handle paste operations into the textbox to ensure that the behavior
        /// is consistent with directly typing into the TextBox.
        /// </summary>
        /// <param name="sender">The TextBox sender.</param>
        /// <param name="dopea">Paste event arguments.</param>
        /// <remarks>This operation is only available in WPF.</remarks>
        private void OnClipboardPaste(object sender, DataObjectPastingEventArgs dopea)
        {
            string text = dopea.SourceDataObject.GetData(dopea.FormatToApply).ToString();

            if (!string.IsNullOrWhiteSpace(text) && !Validate(text))
                dopea.CancelCommand();
        }
#endif

        /// <summary>
        /// Preview the text input.
        /// </summary>
        /// <param name="sender">The TextBox sender.</param>
        /// <param name="e">The composition event arguments.</param>
        void AssociatedObject_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            if (e.Text.ToString() == ".")
            {
                e.Handled = true;
                //base.OnChanged()
                // return false;
            }
            // e.Handled = Regex.Match(e.Text, "^[0-9]*[.][0-9]*$").Success;

            //Regex regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
            //  e.Handled= regex.IsMatch(e.Text);
            // Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
            //  e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));


            //e.Handled = !Validate(e.Text);
            //  e.Handled = !Validate1(e.Text);

            //e.Handled = !Validate2(e.Text);
            //if ( e.Handled ==false )
            //{
            //    if (e.Text.ToString() ==".")
            //    {
            //       // textBox.Text =textBox 
            //    }
            //}
        }


        protected bool Validate2(string value)
        {
            TextBox textBox = AssociatedObject;
            foreach (char ch in value)
            {
                if (Char.IsDigit(ch))
                {
                    return true;
                }
                else if (ch.ToString() == ".")
                {
                    return false;
                }
                else
                {
                    return false;
                }
            }

            return true;

            string pattern = GetRegularExpressionText();
            //TextBox textBox = AssociatedObject;
            if (value.ToString() == ".")
            {
                if (textBox.Text.ToCharArray().Where(x => x == '.').Count() > 1)
                {
                    return new Regex(pattern).IsMatch(value);
                }
                else if (textBox.Text.ToCharArray().Where(x => x == '.').Count() == 0)
                {
                    return true;
                    //if (value.ToString() == ".")
                    //{
                    //    selStart = textBox.Text.ToString().Length;
                    //    test = string.Concat(textBox.Text.ToString(), ".00");
                    //    return true;
                    //    // textBox.Text = textBox.Text + "00";
                    //}

                }
            }
            else
            {
                return new Regex(pattern).IsMatch(textBox.Text);
                // return true;
            }
            return true;

            //if (value.ToCharArray().Where(x => x == '.').Count() > 1)
            //{
            //    return new Regex(pattern).IsMatch(value);
            //}
            //else if (value.ToCharArray().Where(x => x == '.').Count() = 1)
            //{

            //}


        }
        /// <summary>
        /// Validate the contents of the textbox with the new content to see if it is
        /// valid.
        /// </summary>
        /// <param name="value">The text to validate.</param>
        /// <returns>True if this is valid, false otherwise.</returns>

        protected bool Validate1(string value)
        {
            TextBox textBox = AssociatedObject;
            string pre = string.Empty;
            string post = string.Empty;
            string test = string.Empty;
            int selStart;
            if (!string.IsNullOrWhiteSpace(textBox.Text))
            {
                selStart = textBox.SelectionStart;
                if (selStart > textBox.Text.Length)
                    selStart--;


                if (value.ToString() == ".")
                {
                    if (test.ToCharArray().Where(x => x == '.').Count() > 1)
                    {
                        return false;
                    }
                    else
                    {
                        if (value.ToString() == ".")
                        {
                            selStart = textBox.Text.ToString().Length;
                            test = string.Concat(textBox.Text.ToString(), ".00");
                            return true;
                            // textBox.Text = textBox.Text + "00";
                        }

                    }
                }

                pre = textBox.Text.Substring(0, selStart);
                post = textBox.Text.Substring(selStart + textBox.SelectionLength, textBox.Text.Length - (selStart + textBox.SelectionLength));
            }
            else
            {
                pre = textBox.Text.Substring(0, textBox.CaretIndex);
                post = textBox.Text.Substring(textBox.CaretIndex, textBox.Text.Length - textBox.CaretIndex);
            }

            test = string.Concat(pre, value, post);


            // textBox.Text = test;
            //string.Concat(pre, value, post);

            //else
            //{
            //    
            //}


            return true;

        }


        protected bool Validate(string value)
        {

            TextBox textBox = AssociatedObject;
            string pre = string.Empty;
            string post = string.Empty;

            if (!string.IsNullOrWhiteSpace(textBox.Text))
            {
                int selStart = textBox.SelectionStart;
                if (selStart > textBox.Text.Length)
                    selStart--;
                pre = textBox.Text.Substring(0, selStart);
                post = textBox.Text.Substring(selStart + textBox.SelectionLength, textBox.Text.Length - (selStart + textBox.SelectionLength));
            }
            else
            {
                pre = textBox.Text.Substring(0, textBox.CaretIndex);
                post = textBox.Text.Substring(textBox.CaretIndex, textBox.Text.Length - textBox.CaretIndex);
            }

            string test = string.Concat(pre, value, post);

            string pattern = GetRegularExpressionText();

            return new Regex(pattern).IsMatch(test);
        }

        private string GetRegularExpressionText()
        {
            if (!string.IsNullOrWhiteSpace(_pattern))
            {
                return _pattern;
            }
            _pattern = GetPatternText();
            return _pattern;
        }

        private string GetPatternText()
        {
            string pattern = string.Empty;
            string signPattern = "[{0}+]";

            // If the developer has chosen to allow negative numbers, the pattern will be [-+].
            // If the developer chooses not to allow negatives, the pattern is [+].
            if (AllowNegatives)
            {
                signPattern = string.Format(signPattern, "-");
            }
            else
            {
                signPattern = string.Format(signPattern, string.Empty);
            }

            // If the developer doesn't allow decimals, return the pattern.
            if (!AllowDecimal)
            {
                return string.Format(@"^({0}?)(\d*)$", signPattern);
            }

            // If the developer has chosen to apply a decimal limit, the pattern matches
            // on a
            if (DecimalLimit > 0)
            {
                pattern = string.Format(@"^({2}?)(\d*)([{0}]?)(\d{{0,{1}}})$",
                  NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator,
                  DecimalLimit,
                  signPattern);
            }
            else
            {
                pattern = string.Format(@"^({1}?)(\d*)([{0}]?)(\d*)$", NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator, signPattern);
            }

            return pattern;
        }
        #endregion
    }
}

//public class TextBoxInputBehavior : Behavior<TextBox> 
//{
//    const NumberStyles validNumberStyles = NumberStyles.AllowDecimalPoint |
//                                               NumberStyles.AllowThousands |
//                                               NumberStyles.AllowLeadingSign;
//    public TextBoxInputBehavior()
//    {
//        this.InputMode = TextBoxInputMode.None;
//        this.JustPositivDecimalInput = false;
//    }

//    public TextBoxInputMode InputMode { get; set; }


//    public static readonly DependencyProperty JustPositivDecimalInputProperty =
//     DependencyProperty.Register("JustPositivDecimalInput", typeof(bool),
//     typeof(TextBoxInputBehavior), new FrameworkPropertyMetadata(false));

//    public bool JustPositivDecimalInput
//    {
//        get { return (bool)GetValue(JustPositivDecimalInputProperty); }
//        set { SetValue(JustPositivDecimalInputProperty, value); }
//    }

//    protected override void OnAttached()
//    {
//        base.OnAttached();
//        AssociatedObject.PreviewTextInput += AssociatedObjectPreviewTextInput;
//        AssociatedObject.PreviewKeyDown += AssociatedObjectPreviewKeyDown;

//        DataObject.AddPastingHandler(AssociatedObject, Pasting);

//    }

//    protected override void OnDetaching()
//    {
//        base.OnDetaching();
//        AssociatedObject.PreviewTextInput -= AssociatedObjectPreviewTextInput;
//        AssociatedObject.PreviewKeyDown -= AssociatedObjectPreviewKeyDown;

//        DataObject.RemovePastingHandler(AssociatedObject, Pasting);
//    }

//    private void Pasting(object sender, DataObjectPastingEventArgs e)
//    {
//        if (e.DataObject.GetDataPresent(typeof(string)))
//        {
//            var pastedText = (string)e.DataObject.GetData(typeof(string));

//            if (!this.IsValidInput(this.GetText(pastedText)))
//            {
//                System.Media.SystemSounds.Beep.Play();
//                e.CancelCommand();
//            }
//        }
//        else
//        {
//            System.Media.SystemSounds.Beep.Play();
//            e.CancelCommand();
//        }
//    }

//    private void AssociatedObjectPreviewKeyDown(object sender, KeyEventArgs e)
//    {
//        if (e.Key == Key.Space)
//        {
//            if (!this.IsValidInput(this.GetText(" ")))
//            {
//                System.Media.SystemSounds.Beep.Play();
//                e.Handled = true;
//            }
//        }
//    }

//    private void AssociatedObjectPreviewTextInput(object sender, TextCompositionEventArgs e)
//    {
//        if (!this.IsValidInput(this.GetText(e.Text)))
//        {
//            System.Media.SystemSounds.Beep.Play();
//            e.Handled = true;
//        }
//    }

//    private string GetText(string input)
//    {
//        var txt = this.AssociatedObject;

//        int selectionStart = txt.SelectionStart;
//        if (txt.Text.Length < selectionStart)
//            selectionStart = txt.Text.Length;

//        int selectionLength = txt.SelectionLength;
//        if (txt.Text.Length < selectionStart + selectionLength)
//            selectionLength = txt.Text.Length - selectionStart;

//        var realtext = txt.Text.Remove(selectionStart, selectionLength);

//        int caretIndex = txt.CaretIndex;
//        if (realtext.Length < caretIndex)
//            caretIndex = realtext.Length;

//        var newtext = realtext.Insert(caretIndex, input);

//        return newtext;
//    }

//    private bool IsValidInput(string input)
//    {
//        switch (InputMode)
//        {
//            case TextBoxInputMode.None:
//                return true;
//            case TextBoxInputMode.DigitInput:
//                return CheckIsDigit(input);

//            case TextBoxInputMode.DecimalInput:
//                decimal d;
//                //wen mehr als ein Komma
//                if (input.ToCharArray().Where(x => x == ',').Count() > 1)
//                    return false;


//                if (input.Contains("-"))
//                {
//                    if (this.JustPositivDecimalInput)
//                        return false;


//                    if (input.IndexOf("-", StringComparison.Ordinal) > 0)
//                        return false;

//                    if (input.ToCharArray().Count(x => x == '-') > 1)
//                        return false;

//                    //minus einmal am anfang zulässig
//                    if (input.Length == 1)
//                        return true;
//                }

//                var result = decimal.TryParse(input, validNumberStyles, CultureInfo.CurrentCulture, out d);
//                return result;



//            default: throw new ArgumentException("Unknown TextBoxInputMode");

//        }
//        return true;
//    }

//    private bool CheckIsDigit(string wert)
//    {
//        return wert.ToCharArray().All(Char.IsDigit);
//    }
//}

//public enum TextBoxInputMode
//{
//    None,
//    DecimalInput,
//    DigitInput
//}
//}