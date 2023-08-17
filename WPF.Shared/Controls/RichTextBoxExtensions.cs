using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace WPF.Shared
{
    public static class RichTextBoxExtensions
    {
        /// <summary>
        /// Gets the content of the <see cref="RichTextBox"/> as the actual RTF.
        /// </summary>
        public static string GetAsRTF(this RichTextBox richTextBox)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                TextRange textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                textRange.Save(memoryStream, DataFormats.Rtf);
                memoryStream.Seek(0, SeekOrigin.Begin);

                using (StreamReader streamReader = new StreamReader(memoryStream))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Gets the content of the <see cref="RichTextBox"/> as plain text only.
        /// </summary>
        public static string GetAsText(this RichTextBox richTextBox)
        {
            return new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text;
        }


        /// <summary>
        /// Gets the content of the <see cref="RichTextBox"/> as lines only.
        /// </summary>
        public static string[] GetEntriesLines(this RichTextBox richTextBox)
        {
            var textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text;
            return textRange.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        }
        public static string[] GetAllLines(this RichTextBox richTextBox)
        {
            var textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text;
            return textRange.Split(Environment.NewLine.ToCharArray());
        }
        public static void RemoveLastBlock(this RichTextBox richTextBox)
        {
            richTextBox.Document.Blocks.Remove(richTextBox.Document.Blocks.LastBlock);
        }

        public static void RemoveFirstBlock(this RichTextBox richTextBox)
        {
            richTextBox.Document.Blocks.Remove(richTextBox.Document.Blocks.FirstBlock);
        }
        public static void AppendText(this RichTextBox richTextBox, string text, SolidColorBrush brush)
        {
            TextRange tr = new TextRange(richTextBox.Document.ContentEnd, richTextBox.Document.ContentEnd);
            tr.Text = text;
            tr.ApplyPropertyValue(TextElement.ForegroundProperty, brush);
        }

        public static void AddLine(this RichTextBox richTextBox, string text, SolidColorBrush brush)
        {
            var paragraph = new Paragraph();
            var run = new Run(text)
            {
                Foreground = brush
            };
            paragraph.Inlines.Add(run);
            richTextBox.Document.Blocks.Add(paragraph);
        }

        public static void AddLine(this RichTextBox richTextBox, string text)
        {
            var paragraph = new Paragraph();
            paragraph.Inlines.Add(new Run(text));
            richTextBox.Document.Blocks.Add(paragraph);
        }
        public static void AddLines(this RichTextBox richTextBox, params string[] lines)
        {
            foreach (var line in lines)
            {
                var paragraph = new Paragraph();
                paragraph.Inlines.Add(new Run(line));
                richTextBox.Document.Blocks.Add(paragraph);
            }
        }

        /// <summary>
        /// Gets the number of lines in the <see cref="RichTextBox"/>.
        /// </summary>
        public static int GetLineCount(this RichTextBox richTextBox)
        {
            // Idea: Every paragraph in a RichTextBox ends with a \par.

            // Special handling for empty RichTextBoxes, because while there is
            // a \par, there is no line in the strict sense yet.
            if (String.IsNullOrWhiteSpace(richTextBox.GetAsText()))
            {
                return 0;
            }

            // Simply count the occurrences of \par to get the number of lines.
            // Subtract 1 from the actual count because the first \par is not
            // actually a line for reasons explained above.
            return Regex.Matches(richTextBox.GetAsRTF(), Regex.Escape(@"\par")).Count - 1;
        }
    }
}
