using System;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace LevelEditor.Utils
{
    class Log
    {
        public static string LOG_ERROR = "!";
        public static string LOG_WARNING = "?";
        public static string LOG_INFO = "i";
        public static string LOG_DEBUG = "D";
        public static string LOG_TRACE = " ";

        public Log()
        {
        }

        public static string Filename;
        private static StreamWriter writer;

        public static void WriteLine(string line)
        {
            WriteLine(LOG_TRACE, line, true);
        }

        public static void WriteLine(string type, string line)
        {
            WriteLine(type, line, true);
        }

        public static void WriteLine(string type, string line, bool append)
        {
            if (Filename == null)
            {
                Debug.WriteLine("[Log] prior to writing logs, a filename must be set");
                return;
            }

            if (writer == null || !append)
            {
                if (writer != null) writer.Close();
                try
                {
                    writer = new StreamWriter(Filename, false);
                }
                catch
                {
                    MessageBox.Show("Failed writing log file!\nCheck config.");
                    return;
                }
            }

            writer.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss.fff") + "] [" + type + "] " + line);
            writer.Flush();
        }
    }
}
