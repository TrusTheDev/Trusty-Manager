
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
        static readonly String patientDocPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\PatientDocs\";
        static readonly DocumentModelList documentList = new DocumentModelList();
        static readonly NavigatorController navigator = new NavigatorController();

        bool changed;
        int stackCount;
        readonly Stack<UndoAction> undoStack = new Stack<UndoAction>();
        public Form1()
        {
            documentList.AddDocumentsFromFile(patientDocPath);
            navigator.AssignFile(documentList);
            this.KeyPreview = true;
            InitializeComponent();
            var document = navigator.GetLastFile();
            label1.Text = document.FileName;
            dataGridView = documentToGridView(document, dataGridView);
            changed = false;
            stackCount = 0;

        }
        private bool SomethingChanged()
        {
                return stackCount != 0 || changed;
        }

        private void NextLastFile(bool isNext)
        {
            dataGridView.ClearSelection();
            undoStack.Clear();

            if (SomethingChanged())
            {
                using (var form = new SaveForm())
                {
                    var result = form.ShowDialog();
                    if (result == DialogResult.Yes)
                    {
                        SaveFile();
                    }
                }
            }
            if (isNext)
            {
                dataGridView = documentToGridView(navigator.GetNextFile(), dataGridView);
            }
            else
            {
                dataGridView = documentToGridView(navigator.GetPreviousFile(), dataGridView);
            }
            label1.Text = navigator.GetcurrentFile().FileName;
            changed = false;
            stackCount = 0;
        }
        private void BtnFormer_Click(object sender, EventArgs e)
        {
            NextLastFile(false);
        }
        private void BtnNextFile_Click(object sender, EventArgs e)
        {
            NextLastFile(true);
        }
        private void BtnRemoveCell_Click(object sender, EventArgs e)
        {
            DataGridViewRow targetRow = null;
            if (dataGridView.CurrentCell != null)
                targetRow = dataGridView.CurrentCell.OwningRow;
            else if (dataGridView.SelectedRows.Count > 0)
                targetRow = dataGridView.SelectedRows[0];

            if (targetRow == null || targetRow.IsNewRow)
                return;

            int currentRow = targetRow.Index;
            originalValue = targetRow.Visible;
            targetRow.Visible = false;
            undoAction.AddChange(undoStack, currentRow, 0, originalValue, false);
            changed = true;
        }

        private object originalValue;
        readonly UndoAction undoAction = new UndoAction();
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
        private void DataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            originalValue = dataGridView[e.ColumnIndex, e.RowIndex].Value;
        }
        private void DataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var newValue = dataGridView[e.ColumnIndex, e.RowIndex].Value;
            if (Equals(originalValue, newValue))
                return;

            undoAction.AddChange(undoStack, e.RowIndex, e.ColumnIndex, originalValue, newValue);
            stackCount++;
        }
        private void BtnOpenFileLocation_Click(object sender, EventArgs e)
        {
            string filePath = Path.Combine(patientDocPath, navigator.GetcurrentFile().FileName);
            Process.Start("explorer.exe", $"/select,\"{filePath}\"");
        }
        private void BtnAddFile_Click(object sender, EventArgs e)
        {
            using (var form = new AddDocumentForm())
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    documentList.CombineLists(form.FileModelList);
                    navigator.AssignFile(documentList);
                    dataGridView = documentToGridView(navigator.GetLastFile(), dataGridView);
                    label1.Text = navigator.GetcurrentFile().FileName;
                    changed = true;
                }
            }
        }
        private void BtnDeleteFile_Click(object sender, EventArgs e)
        {
            DialogResult = MessageBox.Show("¿Estás seguro de que deseas eliminar el archivo?", "Si", MessageBoxButtons.YesNo);
            if (DialogResult == DialogResult.Yes)
            {
                documentList.RemoveFile(navigator.GetcurrentFile());
                navigator.AssignFile(documentList);
                dataGridView = documentToGridView(navigator.GetcurrentFile(), dataGridView);
                label1.Text = navigator.GetcurrentFile().FileName;
            }
        }
        private void SaveFile()
        {
            navigator.GetcurrentFile().SaveFile(dataGridView);
            changed = false;
            stackCount = 0;
        }

        private void BtnAddCol_Click(object sender, EventArgs e)
        {
            using (var form = new AddColumnForm())
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    dataGridView = GridViewController.addColumn(dataGridView, form.columnName);
                    changed = true;
                }
            }

        }

        private void BtnAddRow_Click(object sender, EventArgs e)
        {
            dataGridView = GridViewController.addRow(dataGridView);
            changed = true;
        }

        private void BtnDeleteCol_Click(object sender, EventArgs e)
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
            changed = true;
        }
        
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            using (var form = new SaveOnExit())
            {
                var result = form.ShowDialog();
                if (result == DialogResult.Yes)
                {
                    SaveFile();
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true; 
                }
            }
        }

        private void BtnSaveFile_Click(object sender, EventArgs e)
        {
            using (var form = new SaveForm())
            {
                var result = form.ShowDialog();
                if (result == DialogResult.Yes)
                {
                    SaveFile();
                }
            }
        }


    }
}
