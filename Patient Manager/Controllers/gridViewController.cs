using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xceed.Words.NET;
using Patient_Manager.Interfaces;
using ClosedXML.Excel;

namespace Patient_Manager.Controllers
{
    internal class GridViewController 
    {

        public static DataGridView documentToGridView(IFile document, DataGridView gridView)
        {
            switch (document.Format) { 
                case ".docx":
                    DocX docxDocument = (DocX)document.RepairFile();
                    return DocxToGridView(gridView, docxDocument);
                case ".xlsx":
                    XLWorkbook xlsxDocument = (XLWorkbook) document.RepairFile();
                    return xlsxToGridView(gridView, xlsxDocument);
                default:
                    throw new NotSupportedException($"El formato de archivo {document.Format} no es soportado.");
            }
        }

        public static DataGridView xlsxToGridView(DataGridView gridView, XLWorkbook xlsx)
        {
            foreach (var worksheet in xlsx.Worksheets)
            {
                bool isFirstRow = true;
                foreach (var row in worksheet.RowsUsed())
                {
                    if (isFirstRow)
                    {
                        foreach (var cell in row.Cells())
                        {
                            gridView.Columns.Add(cell.GetString(), cell.GetString());
                        }
                        isFirstRow = false;
                    }
                    else
                    {
                        List<string> rowData = new List<string>();
                        foreach (var cell in row.Cells())
                        {
                            rowData.Add(cell.GetString());
                        }
                        gridView.Rows.Add(rowData.ToArray());
                    }
                }
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

        public DataGridView DocxToGridView()
        {
            throw new NotImplementedException();
        }
    }
}
