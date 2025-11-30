using DocumentFormat.OpenXml.Wordprocessing;
using Patient_Manager.Controllers;
using Patient_Manager.Features;
using Patient_Manager.Forms;
using Patient_Manager.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static Patient_Manager.Controllers.GridViewController;
namespace Patient_Manager
{
    public partial class Form1 : Form
    {
        static String PatientDocPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\PatientDocs\";
        static DocumentModelList documentList = new DocumentModelList();
        static NavigatorController navigator = new NavigatorController();

        Stack<UndoAction> undoStack = new Stack<UndoAction>();
        public Form1()
        {
            documentList.AddDocumentsFromFile(PatientDocPath);
            navigator.assignFile(documentList);
            this.KeyPreview = true;  
            InitializeComponent();
            var document = navigator.getLastFile();
            label1.Text = document.FileName;
            dataGridView = documentToGridView(document, dataGridView);

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            addRowbtn.Location = new Point(addRowbtn.Location.X, addRowbtn.Parent.PointToClient(dataGridView.PointToScreen(dataGridView.GetCellDisplayRectangle(dataGridView.CurrentCell.ColumnIndex, dataGridView.CurrentCell.RowIndex, false).Location)).Y);
        }
        private void btnFormer_Click(object sender, EventArgs e)
        {
            navigator.getcurrentFile().SaveFile(dataGridView);
            dataGridView = documentToGridView(navigator.getPreviousFile(), dataGridView);
            label1.Text = navigator.getcurrentFile().FileName;
        }
        private void btnNextFile_Click(object sender, EventArgs e)
        {
            navigator.getcurrentFile().SaveFile(dataGridView);
            dataGridView = documentToGridView(navigator.getNextFile(), dataGridView);
            label1.Text = navigator.getcurrentFile().FileName;
        }
        private void CurrentCell(object sender, EventArgs e)
        {
            addRowbtn.Top = getSelectedmiddlePoint(dataGridView, addRowbtn);
        }
        private void BtnAddRow_Click(object sender, EventArgs e)
        {
            if (dataGridView.Columns.Count == 0)
            {
                MessageBox.Show("ATENCION: Columna vacía, agrega una.");
                return;
            }

            if (dataGridView.CurrentCell == null)
            {
                // No current cell: append before new row (if present) or at end
                int index = dataGridView.Rows.Count - (dataGridView.AllowUserToAddRows ? 1 : 0);
                dataGridView.Rows.Insert(index);
                return;
            }

            var owningRow = dataGridView.CurrentCell.OwningRow;
            if (owningRow.IsNewRow)
            {
                // Insert before the new row (can't insert after it)
                int index = dataGridView.Rows.Count - 1;
                dataGridView.Rows.Insert(index);
            }
            else
            {
                dataGridView.Rows.Insert(owningRow.Index + 1);
            }
        }
        private void onRowHeight(object sender, DataGridViewRowEventArgs e)
        {
            addRowbtn.Top = getSelectedmiddlePoint(dataGridView, addRowbtn);
        }
        private void btnRemoveCell(object sender, EventArgs e)
        {
            // Determine the target row defensively
          
            DataGridViewRow targetRow = null;
            if (dataGridView.CurrentCell != null)
                targetRow = dataGridView.CurrentCell.OwningRow;
            else if (dataGridView.SelectedRows.Count > 0)
                targetRow = dataGridView.SelectedRows[0];

            if (targetRow == null || targetRow.IsNewRow)
                return; // nothing to remove

            int currentRow = targetRow.Index;
            int currentColumn = dataGridView.CurrentCell?.ColumnIndex ?? 0;
            originalValue = targetRow.Visible;
            targetRow.Visible = false;
            undoAction.AddChange(undoStack, currentRow, 0, originalValue, false);

        }

        private object originalValue;
        UndoAction undoAction = new UndoAction();
        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            originalValue = dataGridView[e.ColumnIndex, e.RowIndex].Value;
        }
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var newValue = dataGridView[e.ColumnIndex, e.RowIndex].Value;
            if (Equals(originalValue, newValue))
                return;

            undoAction.AddChange(undoStack, e.RowIndex, e.ColumnIndex, originalValue, newValue);
        }


        private void KeyBindCTRLZ(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Z)
            {
                DataGridView gridResult = undoAction.UndoLast(undoStack, dataGridView);
                if(gridResult != null)
                {
                    Console.WriteLine("WHATS UP PEOPLE");
                    dataGridView = gridResult;
                }
            }
        }

        private void btnOpenFileLocation_Click(object sender, EventArgs e)
        {
            string filePath = Path.Combine(PatientDocPath,navigator.getcurrentFile().FileName);
            Process.Start("explorer.exe", $"/select,\"{filePath}\"");
        }

        private void btnAddFile_Click(object sender, EventArgs e)
        {
            using (var form = new AddDocumentForm())
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    documentList.combineLists(form.FileModelList);
                    navigator.assignFile(documentList);
                    dataGridView = documentToGridView(navigator.getLastFile(), dataGridView);
                    label1.Text = navigator.getcurrentFile().FileName;
                }
            }
        }

        private void btnAddCol_Click(object sender, EventArgs e)
        {
            using (var form = new AddColumnForm())
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    dataGridView = addColumn(dataGridView, form.columnName);
                }
            }
            
        }

        private void btnDeleteCol_Click(object sender, EventArgs e)
        {
            // Determine the target row defensively

            DataGridViewColumn targetCol = null;
            if (dataGridView.CurrentCell != null)
                targetCol = dataGridView.CurrentCell.OwningColumn;
            else if (dataGridView.SelectedColumns.Count > 0)
                targetCol = dataGridView.SelectedColumns[0];

            if (targetCol == null)
                return; // nothing to remove

            int currentCol = targetCol.Index;
            int currentRow = dataGridView.CurrentCell?.RowIndex?? 0;
            originalValue = targetCol.Visible;
            targetCol.Visible = false;
            undoAction.AddChange(undoStack, 0, currentCol, originalValue, false);
        }

        private void btnDeleteFile_Click(object sender, EventArgs e)
        {
            DialogResult = MessageBox.Show("¿Estás seguro de que deseas eliminar el archivo?","Si", MessageBoxButtons.YesNo);
            if (DialogResult == DialogResult.Yes)
            {
                documentList.RemoveFile(navigator.getcurrentFile());
                navigator.assignFile(documentList);
                dataGridView = documentToGridView(navigator.getcurrentFile(), dataGridView);
                label1.Text = navigator.getcurrentFile().FileName;
            }
        }
    }
}
