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
        
        public static void Error(string message, bool popup = true, Exception exception = null)
        {
            if (exception != null)
            {
                var timestamp = DateTime.Now.ToString(DateTimeFormatInfo.InvariantInfo);
                string log =
                    $"[{timestamp}] {message}\n" +
                    $"[{timestamp}] {exception.Message}\n" +
                    $"{exception.StackTrace}\n";
                File.AppendAllText("log.txt", log);
            }

            if (popup)
            {
                MessageBox.Show(message, "Error");
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