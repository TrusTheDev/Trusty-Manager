
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
    // COSAS PARA HACER:
    // - Realizar tests 
    // - Empaquetar aplicación.
    // - Al cerrar el programa guardar todo y hacer desaparecer las columnas?
    public partial class Form1 : Form
    {
        static String PatientDocPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\PatientDocs\";
        static DocumentModelList documentList = new DocumentModelList();
        static NavigatorController navigator = new NavigatorController();

        bool Changed;
        int stackCount;
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
            Changed = false;
            stackCount = 0;

        }
        private bool somethingChanged()
        {
                return stackCount != 0 || Changed;
        }

        private void NextLastFile(bool isNext)
        {
            dataGridView.ClearSelection();
            undoStack.Clear();

            if (somethingChanged())
            {
                using (var form = new SaveForm())
                {
                    var result = form.ShowDialog();
                    if (result == DialogResult.Yes)
                    {
                        saveFile();
                    }
                }
            }
            if (isNext)
            {
                dataGridView = documentToGridView(navigator.getNextFile(), dataGridView);
            }
            else
            {
                dataGridView = documentToGridView(navigator.getPreviousFile(), dataGridView);
            }
            label1.Text = navigator.getcurrentFile().FileName;
            Changed = false;
            stackCount = 0;
        }
        private void btnFormer_Click(object sender, EventArgs e)
        {
            NextLastFile(false);
        }
        private void btnNextFile_Click(object sender, EventArgs e)
        {
            NextLastFile(true);
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

                int index = dataGridView.Rows.Count - (dataGridView.AllowUserToAddRows ? 1 : 0);
                dataGridView.Rows.Insert(index);
                Changed = true;
                return;
            }

            var owningRow = dataGridView.CurrentCell.OwningRow;
            if (owningRow.IsNewRow)
            {

                int index = dataGridView.Rows.Count - 1;
                dataGridView.Rows.Insert(index);
            }
            else
            {
                dataGridView.Rows.Insert(owningRow.Index + 1);
                Changed = true;
            }
        }
        private void btnRemoveCell(object sender, EventArgs e)
        {


            DataGridViewRow targetRow = null;
            if (dataGridView.CurrentCell != null)
                targetRow = dataGridView.CurrentCell.OwningRow;
            else if (dataGridView.SelectedRows.Count > 0)
                targetRow = dataGridView.SelectedRows[0];

            if (targetRow == null || targetRow.IsNewRow)
                return;

            int currentRow = targetRow.Index;
            int currentColumn = dataGridView.CurrentCell?.ColumnIndex ?? 0;
            originalValue = targetRow.Visible;
            targetRow.Visible = false;
            undoAction.AddChange(undoStack, currentRow, 0, originalValue, false);
            Changed = true;
        }

        private object originalValue;
        UndoAction undoAction = new UndoAction();
        private void KeyBindCTRLZ(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Z)
            {
                DataGridView gridResult = undoAction.UndoLast(undoStack, dataGridView);
                if (gridResult != null)
                {
                    dataGridView = gridResult;
                    stackCount--;
                }
            }
        }
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
            stackCount++;
        }
        private void btnOpenFileLocation_Click(object sender, EventArgs e)
        {
            string filePath = Path.Combine(PatientDocPath, navigator.getcurrentFile().FileName);
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
                    Changed = true;
                }
            }
        }
        private void btnDeleteFile_Click(object sender, EventArgs e)
        {
            DialogResult = MessageBox.Show("¿Estás seguro de que deseas eliminar el archivo?", "Si", MessageBoxButtons.YesNo);
            if (DialogResult == DialogResult.Yes)
            {
                documentList.RemoveFile(navigator.getcurrentFile());
                navigator.assignFile(documentList);
                dataGridView = documentToGridView(navigator.getcurrentFile(), dataGridView);
                label1.Text = navigator.getcurrentFile().FileName;
            }
        }
        private void saveFile()
        {
            navigator.getcurrentFile().SaveFile(dataGridView);
            Changed = false;
            stackCount = 0;
        }

        private void btnAddCol_Click(object sender, EventArgs e)
        {
            using (var form = new AddColumnForm())
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    dataGridView = GridViewController.addColumn(dataGridView, form.columnName);
                    Changed = true;
                }
            }

        }

        private void btnDeleteCol_Click(object sender, EventArgs e)
        {
            DataGridViewColumn targetCol = null;
            if (dataGridView.CurrentCell != null)
                targetCol = dataGridView.CurrentCell.OwningColumn;
            else if (dataGridView.SelectedColumns.Count > 0)
                targetCol = dataGridView.SelectedColumns[0];

            if (targetCol == null)
                return;

            int currentCol = targetCol.Index;

            originalValue = targetCol.Visible;
            targetCol.Visible = false;
            undoAction.AddChange(undoStack, 0, currentCol, originalValue, false);
            Changed = true;
        }
        

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            using (var form = new SaveOnExit())
            {
                var result = form.ShowDialog();
                if (result == DialogResult.Yes)
                {
                    saveFile();
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true; 
                }
            }
        }

        private void btnSaveFile(object sender, EventArgs e)
        {
            using (var form = new SaveForm())
            {
                var result = form.ShowDialog();
                if (result == DialogResult.Yes)
                {
                    saveFile();
                }
            }
        }
    }
}
