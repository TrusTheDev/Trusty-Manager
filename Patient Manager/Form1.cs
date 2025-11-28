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
using System.Windows.Forms;
using static Patient_Manager.Controllers.GridViewController;
namespace Patient_Manager
{
    public partial class Form1 : Form
    {
        static String PatientDocPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\PatientDocs\";
        static DocumentModelList documentList = new DocumentModelList();
        static NavigatorController navigator = new NavigatorController();

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
        private void anteriorbtn_Click(object sender, EventArgs e)
        {
            navigator.getcurrentFile().SaveFile(dataGridView);
            dataGridView = documentToGridView(navigator.getPreviousFile(), dataGridView);
            label1.Text = navigator.getcurrentFile().FileName;
        }
        private void siguientebtn_Click(object sender, EventArgs e)
        {
            navigator.getcurrentFile().SaveFile(dataGridView);
            dataGridView = documentToGridView(navigator.getNextFile(), dataGridView);
            label1.Text = navigator.getcurrentFile().FileName;
        }
        private void CurrentCell(object sender, EventArgs e)
        {
            addRowbtn.Top = getSelectedmiddlePoint(dataGridView, addRowbtn);
        }
        private void addRowbtn_Click(object sender, EventArgs e)
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
        private void removerBtn(object sender, EventArgs e)
        {
            dataGridView.Rows.RemoveAt(dataGridView.CurrentCell.RowIndex);
        }

        private object originalValue;
        Stack<UndoAction> undoStack = new Stack<UndoAction>();
        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            originalValue = dataGridView[e.ColumnIndex, e.RowIndex].Value;
        }
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var newValue = dataGridView[e.ColumnIndex, e.RowIndex].Value;
            if (Equals(originalValue, newValue))
                return;

            undoStack.Push(new UndoAction(
                e.RowIndex,
                e.ColumnIndex,
                originalValue,
                newValue
            ));
        }
        private void UndoLast()
        {
            if (undoStack.Count == 0) return;

            var action = undoStack.Pop();

            // Revertimos
            dataGridView[action.ColumnIndex, action.RowIndex].Value = action.OldValue;

            // Opcional: Enfocar la celda para mostrar claramente el cambio
            dataGridView.CurrentCell = dataGridView[action.ColumnIndex, action.RowIndex];
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Z)
            {
                UndoLast();
            }
        }

        private void btnPlanilla(object sender, EventArgs e)
        {
            string filePath = Path.Combine(PatientDocPath,navigator.getcurrentFile().FileName);
            Process.Start("explorer.exe", $"/select,\"{filePath}\"");
        }

        private void btnAgregarArchivo(object sender, EventArgs e)
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

        private void btnAgregarCol_Click(object sender, EventArgs e)
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

        private void button6_Click(object sender, EventArgs e)
        {
            if (dataGridView.Columns.Count == 0)
            {
                MessageBox.Show("ATENCION: Columna vacía, no se puede eliminar.");
                return;
            }
            else dataGridView.Columns.RemoveAt(dataGridView.CurrentCell.ColumnIndex);
        }
    }
}
