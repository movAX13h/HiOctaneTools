using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LevelEditor.Games.HiOctane.Resources
{
    abstract public class TableItem
    {
        public byte[] Bytes { get; protected set; }
        public int ID { get; protected set; }
        public int Offset { get; protected set; }

        abstract public bool WriteChanges();
    }
}



