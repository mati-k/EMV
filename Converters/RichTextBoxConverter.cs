using EMV.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace EMV.Converters
{
    public class RichTextBoxConverter : IValueConverter
    {
        private char defaultBrush;

        private static readonly RichTextBoxConverter defaultInstace = new RichTextBoxConverter() { defaultBrush = 'W' };
        private static readonly RichTextBoxConverter blackDefault = new RichTextBoxConverter() { defaultBrush = 'b' };

        public static RichTextBoxConverter Default
        {
            get { return defaultInstace; }
        }

        public static RichTextBoxConverter Black
        {
            get { return blackDefault; }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Convert("");
            }

            else if (value is string)
            {
                return Convert((string)value);
            }

            else
            {
                throw new NotSupportedException(string.Format("{0} cannot convert from {1}.", this.GetType().FullName, value.GetType().FullName));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException(string.Format("{0} does not support converting back.", this.GetType().FullName));
        }

        public FlowDocument Convert(string text)
        {
            FlowDocument document = new FlowDocument();

            text = text.Replace("\\n", Environment.NewLine);
            string[] formats = text.Split('§');

            Stack<char> formatCharacters = new Stack<char>();

            for (int i = 0; i < formats.Length; i++)
            {
                if (formats[i].Length == 0)
                    continue;

                TextRange range = new TextRange(document.ContentEnd, document.ContentEnd);
                
                if (i == 0)
                {
                    range.Text = formats[i];
                    range.ApplyPropertyValue(TextElement.ForegroundProperty, FontColors.Instance.Colors.Where(i => i.Key == defaultBrush).First().Brush);
                    continue;
                }

                if (formats[i][0] == '!' && formatCharacters.Count > 0)
                {
                    formatCharacters.Pop();
                }

                else
                {
                    formatCharacters.Push(formats[i][0]);
                }

                range.Text = formats[i].Substring(1);

                if (formatCharacters.Count == 0 || !FontColors.Instance.Colors.Where(i => i.Key == formatCharacters.Peek()).Any())
                {
                    range.ApplyPropertyValue(TextElement.ForegroundProperty, FontColors.Instance.Colors.Where(i => i.Key == defaultBrush).First().Brush);
                }

                else
                {
                    range.ApplyPropertyValue(TextElement.ForegroundProperty, FontColors.Instance.Colors.Where(i => i.Key == formatCharacters.Peek()).First().Brush);
                }
            }

            return document;
        }
    }
}
