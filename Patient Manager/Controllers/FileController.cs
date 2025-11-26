using ClosedXML.Excel;
using System;
using System.IO;
using System.Linq;
using Xceed.Words.NET;

namespace Patient_Manager.Controllers
{
    internal class FileController
    {
        string FilePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\PatientDocs\";
        public FileController() { }
        public void createFile(string fileName)
        {
            switch (fileName.Split('.').Last())
            {
                case "docx":
                    createDocX(fileName);
                    break;
                case "xlsx":
                    createXlsx(fileName);
                    break;
                default:
                    throw new NotSupportedException("File type not supported.");
            }
        }

        public void createDocX(string fileName)
        {
            DocX document = DocX.Create(fileName);
            document.AddTable(1, 1);
            document.SaveAs(Path.Combine(FilePath, fileName));
        }

        public void createXlsx(string fileName)
        {
            XLWorkbook document = new XLWorkbook();
            document.Worksheets.Add("Sheet1");
            document.SaveAs(Path.Combine(FilePath, fileName));
        }
    }
}
