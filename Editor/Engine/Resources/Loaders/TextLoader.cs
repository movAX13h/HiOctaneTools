using System;
using System.IO;

namespace LevelEditor.Engine.Resources.Loaders
{
    class TextLoader
    {
        public static string Load(String filename)
        {
            if (!File.Exists(filename)) return "";
            StreamReader streamReader = new StreamReader(filename);
            String text = streamReader.ReadToEnd();
            streamReader.Close();
            return text;
        }
    }
}
