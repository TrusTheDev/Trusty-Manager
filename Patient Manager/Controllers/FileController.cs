using ClosedXML.Excel;
using Patient_Manager.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Xceed.Words.NET;
namespace Patient_Manager.Controllers
{
    internal class FileController
    {
        public DocumentModelList documentModelList { get; set; }

        string FilePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\PatientDocs\";
        public FileController()
        {
            documentModelList = new DocumentModelList();
        }
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
                    MessageBox.Show("El archivo no es soportado");
                    break;
            }
        }

        public void createDocX(string fileName)
        {
            DocX document = DocX.Create(fileName);
            string combinedFilePath = Path.Combine(FilePath, fileName);
            document.SaveAs(combinedFilePath);
            documentModelList.AddDocument(new DocXModel(Convert.ToString(File.GetCreationTime(combinedFilePath).Year), Path.GetFileNameWithoutExtension(combinedFilePath), ".docx", combinedFilePath, Path.GetFileName(combinedFilePath), Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\PatientDocs\"));
        }

        public void createXlsx(string fileName)
        {
            XLWorkbook document = new XLWorkbook();
            document.Worksheets.Add("Sheet1");
            string combinedFilePath = Path.Combine(FilePath, fileName);
            document.SaveAs(combinedFilePath);
            documentModelList.AddDocument(new DocXModel(Convert.ToString(File.GetCreationTime(combinedFilePath).Year), Path.GetFileNameWithoutExtension(combinedFilePath), ".docx", combinedFilePath, Path.GetFileName(combinedFilePath), Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\PatientDocs\"));
        }
    }
}
