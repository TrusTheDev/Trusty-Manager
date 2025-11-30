using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient_Manager.Features
{
    public class UndoAction
    {
        public List<CellChange> Changes { get; set; } = new List<CellChange>();
    }

    public class CellChange
    {
        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }
        public object OldValue { get; set; }
        public object NewValue { get; set; }
    }

    public class RowColChange
    {
        public bool show { get; set; }
    }

}
