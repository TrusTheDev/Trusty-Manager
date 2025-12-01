using ClosedXML.Excel;
using Patient_Manager.Interfaces;
using System;
using System.Linq;
using System.Windows.Forms;
namespace Patient_Manager.Models
{
    internal class XlsXModel : IFile
    {
        public string CreationDate { get; set; }
        public string MonthName { get; set; }
        public string Format { get; set; }
        public string Source { get; set; }
        public XLWorkbook Workbook { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; }

        public XlsXModel(string creationDate, string monthName, string format, string source, string fileName, string filePath)
        {
            CreationDate = creationDate;
            MonthName = monthName;
            Format = format;
            Source = source;
            Workbook = (XLWorkbook)RepairFile();
            FileName = fileName;
            FilePath = filePath;
        }
        public Object RepairFile()
        {
            XLWorkbook workbook = new XLWorkbook(this.Source);
            return workbook;
        }
        public void SaveFile(DataGridView grid)
        {
            gridViewToXlsx(grid);
            Workbook.Save();
        }

        public void createFile(string fileName)
        {
            throw new NotImplementedException();
        }

        public void gridViewToXlsx(DataGridView grid)
        {
            if(grid.Rows.Count != 0) 
            {
                XLWorkbook temporaryXlsx = new XLWorkbook();
                temporaryXlsx.Worksheets.Add("Sheet1");

                int filas = grid.Rows.Cast<DataGridViewRow>().Count(r => !r.IsNewRow);
                int columnas = grid.ColumnCount;

                for(int i = 0; i < columnas; i++)
                {
                    if (grid.Columns[i].Visible)
                    {
                        temporaryXlsx.Worksheet(1).Cell(1, i + 1).Value= grid.Columns[i].HeaderText;
                    }
                    else
                    {
                        grid.Columns.RemoveAt(i);
                    }
                }
                int filaIndex = 2;

                for(int i = 0; i < grid.Rows.Count; i++)
                {
                    if (grid.Rows[i].IsNewRow || !grid.Rows[i].Visible) continue;
                    for (int j = 0; j < grid.Columns.Count; j++)
                    {
                        var valor = grid.Rows[i].Cells[j].Value?.ToString() ?? "";
                        temporaryXlsx.Worksheet(1).Cell(filaIndex, j + 1).Value = valor;
                    }
                    filaIndex++;
                }
                this.Workbook = temporaryXlsx;
                Workbook.SaveAs(this.Source);

            }
            
        }
    }
}
