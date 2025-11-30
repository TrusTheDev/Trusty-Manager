using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Patient_Manager.Features
{
    public class UndoAction
    {

        public List<CellChange> Changes { get; set; } = new List<CellChange>();
        
        
        public void AddChange(Stack<UndoAction> undoStack,int rowIndex, int columnIndex, object oldValue, object newValue)
        {
            /*
                        undoStack.Push(new UndoAction{ Changes.Add(new CellChange
                        {
                            RowIndex = rowIndex,
                            ColumnIndex = columnIndex,
                            OldValue = oldValue,
                            NewValue = newValue
                        });
            */
            undoStack.Push(new UndoAction
            {
                Changes = { new CellChange {
            RowIndex = rowIndex,
            ColumnIndex = columnIndex,
            OldValue = oldValue,
            NewValue = newValue
                }}
            });

        }
        public DataGridView UndoLast(Stack<UndoAction> undoStack, DataGridView dataGridView)
        {
            if (undoStack.Count == 0) return null;
            dataGridView.ClearSelection();

            var action = undoStack.Pop();

            if (action.Changes.Last().NewValue is bool && action.Changes.Last().ColumnIndex == 0)
            {
                dataGridView.Rows[action.Changes.Last().RowIndex].Visible = true;
                dataGridView.Rows[action.Changes.Last().RowIndex].Selected = true;
                dataGridView.FirstDisplayedScrollingRowIndex = action.Changes.Last().RowIndex;
            }
            else if (action.Changes.Last().NewValue is bool && action.Changes.Last().RowIndex == 0)
            {
                dataGridView.Columns[action.Changes.Last().ColumnIndex].Visible = true;
                int col = action.Changes.Last().ColumnIndex;

                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    if (!row.IsNewRow)
                        row.Cells[col].Selected = true;
                }
                dataGridView.CurrentCell = dataGridView[action.Changes.Last().ColumnIndex, 0];
                dataGridView.FirstDisplayedScrollingColumnIndex = action.Changes.Last().ColumnIndex;
            }
            else
            {
                // Revertimos
                dataGridView[action.Changes.Last().ColumnIndex, action.Changes.Last().RowIndex].Value = action.Changes.Last().OldValue;
                dataGridView.CurrentCell = dataGridView[action.Changes.Last().ColumnIndex, action.Changes.Last().RowIndex];
            }
            return dataGridView;

        }
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

