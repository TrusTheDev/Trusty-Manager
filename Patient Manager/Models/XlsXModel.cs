using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Patient_Manager.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Words.NET;

namespace Patient_Manager.Models
{
    internal class XlsXModel : IFile
    {
        public string CreationDate { get; set; }
        public string MonthName { get; set; }
        public string Format { get; set; }
        public string Source { get; set; }
        public XLWorkbook Workbook { get; set; }

        public XlsXModel(string creationDate, string monthName, string format)
        {
            CreationDate = creationDate;
            MonthName = monthName;
            Format = format;
            Source = MonthName + Format;
            Workbook = (XLWorkbook)RepairFile();
        }

        public Object RepairFile()
        {
            XLWorkbook workbook = new XLWorkbook(this.Source);
            return  workbook;
        }

        public void SaveFile()
        {
            Workbook.Save();
        }
    }
}
