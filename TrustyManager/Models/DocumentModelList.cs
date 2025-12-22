using TrustyManager.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static TrustyManager.Controllers.FileController;
namespace TrustyManager.Models
{
    public class DocumentModelList
    {
        public List<IFile> DocumentList { get; set; }
        public DocumentModelList()
        {
            DocumentList = new List<IFile>();
        }

        public void AddDocumentsFromFile(String path)
        {
            foreach (var file in System.IO.Directory.GetFiles(path))
            {
                switch (Path.GetExtension(file).ToLower())
                {
                    case ".docx":
                        AddDocument(new DocXModel(Convert.ToString(File.GetCreationTime(path).Year), Path.GetFileNameWithoutExtension(file), ".docx", Path.Combine(path, Path.GetFileName(file)), Path.GetFileName(file), Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Files\"));
                        break;
                    case ".xlsx":
                        AddDocument(new XlsXModel(Convert.ToString(File.GetCreationTime(path).Year), Path.GetFileNameWithoutExtension(file), ".xlsx", Path.Combine(path, Path.GetFileName(file)), Path.GetFileName(file), Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Files\"));
                        break;

                    default:
                        throw new NotSupportedException($"El formato de archivo {Path.GetExtension(file)} no es soportado.");
                }
            }
        }

        public void CombineLists(DocumentModelList otherList)
        {
            foreach (var document in otherList.DocumentList)
            {
                DocumentList.Add(document);
            }
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
        public void RemoveFile(IFile document)
        {
            DocumentList.Remove(document);
            DeleteFile(document.Source);
        }

        public bool IsEmpty()
        {
            return !DocumentList.Any();
        }
    }
}
