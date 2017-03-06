using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LevelEditor.Engine.Core
{
    public class TriangleIndices
    {
        public int v1;
        public int v2;
        public int v3;

        public TriangleIndices(int a, int b, int c)
        {
            v1 = a;
            v2 = b;
            v3 = c;
        }
    }
}
