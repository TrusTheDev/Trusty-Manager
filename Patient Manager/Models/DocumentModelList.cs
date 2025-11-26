using Patient_Manager.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace Patient_Manager.Models
{
    internal class DocumentModelList
    {
        public List<IFile> DocumentList { get; set; }

        public DocumentModelList(String path)
        {
            DocumentList = new List<IFile>();
            AddDocumentsFromFile(path);
        }

        public void AddDocumentsFromFile(String path)
        {
            foreach (var file in System.IO.Directory.GetFiles(path))
            {
                Console.WriteLine(Path.GetExtension(file).ToLower());
                switch (Path.GetExtension(file).ToLower())
                {
                    case ".docx":
                        var docxModel = new DocXModel(Convert.ToString(File.GetCreationTime(path).Year),Path.GetFileNameWithoutExtension(file), ".docx", Path.Combine(path, Path.GetFileName(file)), Path.GetFileName(file));
                        AddDocument(docxModel);
                        break;
                    case ".xlsx":
                        var xlsxModel = new XlsXModel(Convert.ToString(File.GetCreationTime(path).Year), Path.GetFileNameWithoutExtension(file), ".xlsx", Path.Combine(path, Path.GetFileName(file)), Path.GetFileName(file));
                        AddDocument(xlsxModel);
                        break;

                    default:
                        throw new NotSupportedException($"El formato de archivo {Path.GetExtension(file)} no es soportado.");   
                }
            }
            sortByDate();
        }
        public void sortByDate()
        {
            int[] values = getMonthValues();
            DocumentList.Sort((x, y) => values[DocumentList.IndexOf(x)].CompareTo(values[DocumentList.IndexOf(y)]));
        }
        public int[] getMonthValues()
        {
            int[] values = new int[DocumentList.Count];
            foreach (var document in DocumentList)
            {
                switch (document.MonthName.ToLower())
                {
                    case "enero":
                        values[DocumentList.IndexOf(document)] = 1;
                        break;
                    case "febrero":
                        values[DocumentList.IndexOf(document)] = 2;
                        break;
                    case "marzo":
                        values[DocumentList.IndexOf(document)] = 3;
                        break;
                    case "abril":
                        values[DocumentList.IndexOf(document)] = 4;
                        break;
                    case "mayo":
                        values[DocumentList.IndexOf(document)] = 5;
                        break;
                    case "junio":
                        values[DocumentList.IndexOf(document)] = 6;
                        break;
                    case "julio":
                        values[DocumentList.IndexOf(document)] = 7;
                        break;
                    case "agosto":
                        values[DocumentList.IndexOf(document)] = 8;
                        break;
                    case "septiembre":
                        values[DocumentList.IndexOf(document)] = 9;
                        break;
                    case "octubre":
                        values[DocumentList.IndexOf(document)] = 10;
                        break;
                    case "noviembre":
                        values[DocumentList.IndexOf(document)] = 11;
                        break;
                    case "December":
                        values[DocumentList.IndexOf(document)] = 12;
                        break;
                }
            }
            return values;
        }
        public int ReturnLastPosition()
        {
            return DocumentList.LastIndexOf(DocumentList.Last());
        }
        public int ReturnFirstPosition()
        {
            return DocumentList.LastIndexOf(DocumentList.First());
        }
        public IFile this[int index]
        {
            get { return DocumentList[index]; }
        }
        public void AddDocument(IFile document)
        {
            DocumentList.Add(document);
        }

    }
}
