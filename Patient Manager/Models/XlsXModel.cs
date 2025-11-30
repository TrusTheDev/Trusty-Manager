using ClosedXML.Excel;
using Patient_Manager.Interfaces;
using System;
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
            Workbook.Save();
        }

        public void createFile(string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
