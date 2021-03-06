﻿using NotePad_Metro.Logical;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NotePad_Metro
{
    class Utility
    {
        private static RichTextBox editor;
        private static RichTextBox errorLog;
        private static ListBox suggestionList;

        private static string space = " ";

        public static void Init(RichTextBox ed, RichTextBox er, ListBox sg)
        {
            editor = ed;
            errorLog = er;
            suggestionList = sg;
        }

        public static void FocusEditor()
        {
            editor.Focus();
        }

        public static void InsertWordToCurrentEditorPosition(string word)
        {
            int currentIndex = editor.SelectionStart;
            editor.Text = editor.Text.Insert(currentIndex, word);
            editor.SelectionStart = currentIndex + word.Length;
        }

        public static void AppendWordToEditor(string word)
        {
            editor.AppendText(word);
        }

        public static void RemoveTextFromEditor(int index)
        {
            editor.Text = editor.Text.Remove(index);
        }

        public static void AddSpaceAfterText()
        {
            editor.AppendText(space);
        }

        public static void AddNewLineObj()
        {
            try
            {
                int lineNo = editor.Lines.Length - 1;
                string lineText = editor.Lines[editor.Lines.Length - 2];
                string lineType = "none";

                string token = TokenGenerator.GenerateToken(lineText);
                switch (token)
                {
                    case "variableDecleration":
                        lineType = "variable";
                        break;
                    case "methodDecleration":
                        lineType = "method";
                        break;
                    case "classDecleration":
                        lineType = "class";
                        break;
                    default:
                        break;
                }

                LineCollection.Add(new Line() { lineNumber = lineNo, text = lineText, type = lineType });
            }
            catch (Exception) { }

        }

        public static Line GetLastLine()
        {
            return LineCollection.GetLastLine();
        }

        public static void AppendToErrorLog(Error error)
        {
            errorLog.AppendText("On Line: " + error.lineNumber + " Error Type: " + error.errorType + "\n");
        }

        public static void GenerateError(Line line)
        {
            Error error = new Error() { lineNumber = line.lineNumber, errorType = line.type + " Decleration Error" };
            ErrorList.Add(error);
            AppendToErrorLog(error);
        }

        public static string GetLastWord()
        {
            int wordEndPosition = editor.SelectionStart;
            int currentPosition = wordEndPosition;

            while (currentPosition > 0 && editor.Text[currentPosition - 1] != ' ' && editor.Text[currentPosition - 1] != '\n')
            {
                currentPosition--;
            }

            string word = editor.Text.Substring(currentPosition, wordEndPosition - currentPosition);
            return word;
        }

        public static void CommentCode()
        {
            string s = "";
            string[] lines = editor.SelectedText.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                if (line.StartsWith("//"))
                {
                    s += line.TrimStart('/') + "\n";
                }
                else
                {
                    s += "//" + line + "\n";
                }
            }
            if (s.Length>0)
            {
                s = s.Remove(s.Length - 1);
                editor.SelectedText = editor.SelectedText.Replace(editor.SelectedText, s);
            }
        }

        public static void FocusSuggestionList()
        {
            suggestionList.Focus();
            suggestionList.SelectedIndex = 0;
        }

        public static void ClearSuggestionList()
        {
            suggestionList.Items.Clear();
        }

        public static void AddTabToEditor()
        {
            string tab = "\t";
            int currentSelection = editor.SelectionStart;
            editor.Text = editor.Text.Insert(editor.SelectionStart, tab);
            editor.SelectionStart = currentSelection + tab.Length;

        }

        public static void AppendText(RichTextBox box, Color color, string text)
        {
            int start = box.SelectionStart;
            //box.AppendText(text);
            box.Select(editor.SelectionStart, 0);
            Clipboard.SetText(text);
            box.Paste();
            int end = start + text.Length;


            box.Select(start, end - start);
            {
                box.SelectionColor = color;
            }
            box.SelectionLength = 0; // clear
        }

    }
}
