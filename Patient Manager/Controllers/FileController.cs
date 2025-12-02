using ClosedXML.Excel;
using Patient_Manager.Models;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Xceed.Words.NET;

namespace Patient_Manager.Controllers
{
    internal class FileController
    {
        public DocumentModelList DocumentModelList { get; set; }

        string FilePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\PatientDocs\";
        public FileController()
        {
            DocumentModelList = new DocumentModelList();
        }
        public void CreateFile(string fileName)
        {
            switch (fileName.Split('.').Last())
            {
                case "docx":
                    CreateDocX(fileName);
                    break;
                case "xlsx":
                    CreateXlsx(fileName);
                    break;
                default:
                    MessageBox.Show("El archivo no es soportado");
                    break;
            }
        }

        public void CreateDocX(string fileName)
        {
            DocX document = DocX.Create(fileName);
            string combinedFilePath = Path.Combine(FilePath, fileName);
            document.SaveAs(combinedFilePath);
            DocumentModelList.AddDocument(new DocXModel(Convert.ToString(File.GetCreationTime(combinedFilePath).Year), Path.GetFileNameWithoutExtension(combinedFilePath), ".docx", combinedFilePath, Path.GetFileName(combinedFilePath), Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\PatientDocs\"));
        }

        public void CreateXlsx(string fileName)
        {
            XLWorkbook document = new XLWorkbook();
            document.Worksheets.Add("Sheet1");
            string combinedFilePath = Path.Combine(FilePath, fileName);
            document.SaveAs(combinedFilePath);
            DocumentModelList.AddDocument(new XlsXModel(Convert.ToString(File.GetCreationTime(combinedFilePath).Year), Path.GetFileNameWithoutExtension(combinedFilePath), ".xlsx", combinedFilePath, Path.GetFileName(combinedFilePath), Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\PatientDocs\"));
        }

        public static void DeleteFile(string FilePath)
        {
            if (File.Exists(FilePath))
            {
                bool ok = RecycleBin.Send(FilePath);

                if (!ok)
                {
                    MessageBox.Show("No se pudo enviar el archivo a la Papelera.");
                }

            }
            else
            {
                MessageBox.Show("El archivo no existe.");
            }
        }
    }
}
