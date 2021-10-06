using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace GLToolsGUI.Utils
{
    public static class Popup
    {
        public const string LogfileName = "log.txt";
        private static Label _errorText;
        
        public static void Error(string message, bool popup = true, string title = "Error", Exception exception = null)
        {
            if (exception != null)
            {
                var timestamp = DateTime.Now.ToString(DateTimeFormatInfo.InvariantInfo);
                string log =
                    $"[{timestamp}] {message}\n" +
                    $"[{timestamp}] {exception.Message}\n" +
                    $"{exception.StackTrace}\n";
                File.AppendAllText(LogfileName, log);
            }

            if (popup)
            {
                string croppedTitle = "..." + title[Math.Max(0, title.Length - 50)..];
                MessageBox.Show(message + "\n" + exception?.Message, croppedTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (_errorText == null)
                return;
            
            _errorText.Text = message;
        }
        
        public static void Success(string message)
        {
            MessageBox.Show(message, "Success");
            
            if (_errorText == null)
                return;
            
            _errorText.Text = "";
        }
        
        public static void SetErrorLabel(in Label errorText)
        {
            _errorText = errorText;
        }
    }
}