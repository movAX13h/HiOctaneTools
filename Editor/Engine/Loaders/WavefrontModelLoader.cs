using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lemmtris.Engine.Loaders
{
    class WavefrontModelLoader : TextLoader
    {
        public WavefrontModelLoader()
        {

        }

        protected override void processText(String text)
        {
            Console.WriteLine("[WavefrontModelLoader] loaded text '" + text + "'");

        }
    }
}
