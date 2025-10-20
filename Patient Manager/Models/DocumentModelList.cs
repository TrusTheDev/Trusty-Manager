using Patient_Manager.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xceed.Words.NET;

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
                switch (Path.GetExtension(file).ToLower())
                {
                    case ".docx":
                        var docxModel = new DocXModel(Convert.ToString(File.GetCreationTime(path).Year), Path.Combine(path, Path.GetFileNameWithoutExtension(file)), ".docx");
                        AddDocument(docxModel);
                        break;

                    default:
                        throw new NotSupportedException($"El formato de archivo {Path.GetExtension(file)} no es soportado.");
                }
            }
        }

        public IFile ReturnLastFile()
        {
            return DocumentList.Last();
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
