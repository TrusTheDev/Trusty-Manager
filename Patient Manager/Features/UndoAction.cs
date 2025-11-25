using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient_Manager.Features
{
    internal class UndoAction
    {
        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }
        public object OldValue { get; set; }
        public object NewValue { get; set; }
        public UndoAction(int rowIndex, int columnIndex, object oldValue, object newValue)
        {
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
            OldValue = oldValue;
            NewValue = newValue;
        }

    }
}
