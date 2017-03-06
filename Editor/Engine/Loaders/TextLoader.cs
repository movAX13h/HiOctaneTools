using System;
using System.IO;

namespace Lemmtris.Engine.Loaders
{
    class TextLoader
    {
        public TextLoader()
        {
        }

        public void LoadFromFile(String filename)
        {
            StreamReader streamReader = new StreamReader(filename);
            String text = streamReader.ReadToEnd();
            streamReader.Close();
            processText(text);
        }

        protected virtual void processText(String text)
        {
        }
    }
}
