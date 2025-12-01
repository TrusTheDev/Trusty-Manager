using ClosedXML.Excel;
using Patient_Manager.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Xceed.Words.NET;

namespace Patient_Manager.Controllers
{
    internal class GridViewController
    {
        public static DataGridView documentToGridView(IFile document, DataGridView gridView)
        {
            switch (document.Format)
            {
                case ".docx":
                    DocX docxDocument = (DocX)document.RepairFile();
                    return DocxToGridView(gridView, docxDocument);
                case ".xlsx":
                    XLWorkbook xlsxDocument = (XLWorkbook)document.RepairFile();
                    return xlsxToGridView(gridView, xlsxDocument);
                default:
                    throw new NotSupportedException($"El formato de archivo {document.Format} no es soportado.");
            }
        }
        public static DataGridView xlsxToGridView(DataGridView gridView, XLWorkbook xlsx)
        {
            gridView.Columns.Clear();
            gridView.Rows.Clear();
            var sheet = xlsx.Worksheet(1);
            var range = sheet.RangeUsed();
            if (range == null) return gridView;

            int firstRow = range.FirstRow().RowNumber();
            int lastRow = range.LastRow().RowNumber();
            int firstCol = range.FirstColumn().ColumnNumber();
            int lastCol = range.LastColumn().ColumnNumber();

            var headerRow = sheet.Row(firstRow);
            for (int col = firstCol; col <= lastCol; col++)
            {
                string header = headerRow.Cell(col).GetValue<string>();
                if (string.IsNullOrWhiteSpace(header)) header = $"Col {col}";
                gridView.Columns.Add(header, header);
            }

            for (int row = firstRow + 1; row <= lastRow; row++)
            {
                var excelRow = sheet.Row(row);
                List<string> rowData = new List<string>();

                for (int col = firstCol; col <= lastCol; col++)
                {
                    var value = excelRow.Cell(col).GetValue<string>();
                    rowData.Add(value);
                }

                gridView.Rows.Add(rowData.ToArray());
            }

            return gridView;

        }
        public static DataGridView DocxToGridView(DataGridView gridView, DocX document)
        {
            gridView.Columns.Clear();
            foreach (var table in document.Tables)
            {
                foreach (var row in table.Rows)
                {
                    if (gridView.Columns.Count == 0)
                    {
                        foreach (var cell in row.Cells)
                        {
                            if (String.IsNullOrWhiteSpace(cell.Paragraphs[0].Text)) continue;
                            gridView.Columns.Add(cell.Paragraphs[0].Text, cell.Paragraphs[0].Text);
                        }
                    }
                    else
                    {
                        List<string> rowData = new List<string>();
                        foreach (var cell in row.Cells)
                        {
                            rowData.Add(cell.Paragraphs[0].Text);
                        }
                        gridView.Rows.Add(rowData.ToArray());
                    }
                }
            }
            return gridView;
        }
        static public int getSelectedmiddlePoint(DataGridView dataGridView, Button btn)
        {
            if (dataGridView.CurrentCell != null)
            {
                Rectangle cellRectangle = dataGridView.GetCellDisplayRectangle(dataGridView.CurrentCell.ColumnIndex, dataGridView.CurrentCell.RowIndex, false);
                Point cellLocation = dataGridView.PointToScreen(cellRectangle.Location);
                Point parentLocation = btn.Parent.PointToClient(cellLocation);
                int middleY = parentLocation.Y + (cellRectangle.Height / 2) - (btn.Height / 2);
                return middleY;
            }
            else
            {
                return btn.Location.Y;
            }
        }

        public static DataGridView addColumn(DataGridView gridView, string columnName)
        {
            if (gridView == null)
            {
                throw new ArgumentNullException(nameof(gridView));
            }

            if (string.IsNullOrWhiteSpace(columnName))
            {
                return gridView;
            }

            var names = columnName.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var raw in names)
            {
                var name = raw.Trim();
                if (string.IsNullOrEmpty(name))
                {
                    continue;
                }

                // Avoid adding duplicate columns (checks by column Name)
                if (!gridView.Columns.Contains(name))
                {
                    gridView.Columns.Add(name, name);
                }
            }
            return gridView;
        }
    }
}
