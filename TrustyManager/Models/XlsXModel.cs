using ClosedXML.Excel;
using TrustyManager.Interfaces;
using System;
using System.Linq;
using System.Windows.Forms;
namespace TrustyManager.Models
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
            GridViewToXlsx(grid);
            Workbook.Save();
        }
        public void GridViewToXlsx(DataGridView grid)
        {
            XLWorkbook temporaryXlsx = new XLWorkbook();
            var worksheet = temporaryXlsx.Worksheets.Add("Sheet1");
            int filas = grid.Rows.Cast<DataGridViewRow>().Count(r => !r.IsNewRow);
            int columnas = grid.ColumnCount;
            int noVisibles = grid.Columns.Cast<DataGridViewColumn>().Count(c => !c.Visible);

            if (noVisibles > 0)
            {
                for (int i = 0; i < columnas; i++)
                {
                    if (!grid.Columns[i].Visible)
                    {
                        grid.Columns.RemoveAt(i);
                        columnas--;
                    }
                }
            }

            for (int j = 0; j < columnas; j++)
            {
                if (grid.Columns[j].Visible)
                {
                    worksheet.Cell(1, j + 1).Value = grid.Columns[j].HeaderText;
                }
                else
                {
                    grid.Columns.RemoveAt(j);
                }
            }
            int filaIndex = 2;

            for (int i = 0; i < grid.Rows.Count; i++)
            {
                if (grid.Rows[i].IsNewRow || !grid.Rows[i].Visible) continue;
                for (int j = 0; j < grid.Columns.Count; j++)
                {
                    var valor = grid.Rows[i].Cells[j].Value?.ToString() ?? "";
                    worksheet.Cell(filaIndex, j + 1).Value = valor;
                }
                filaIndex++;
            }
            this.Workbook = temporaryXlsx;
            Workbook.SaveAs(this.Source);           
        }
    }
}
