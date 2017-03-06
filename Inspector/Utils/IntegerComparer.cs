using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LevelInspector.Utils
{
    public class IntegerComparer : IComparer
    {
        private int _colIndex;

        public IntegerComparer(int colIndex)
        {
            _colIndex = colIndex;
        }

        public int Compare(object x, object y)
        {
            string a = (x as ListViewItem).SubItems[_colIndex].Text;
            string b = (y as ListViewItem).SubItems[_colIndex].Text;

            decimal na, nb;
            if (decimal.TryParse(a, out na) && decimal.TryParse(b, out nb)) return na.CompareTo(nb);
            else return String.Compare(a, b);
        }
    }
}
