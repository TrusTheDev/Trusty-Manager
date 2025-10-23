using Patient_Manager.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Words.NET;

namespace Patient_Manager.Models
{
    internal class DocXModel : IFile
    {
        public DocX Document { get; set; }
        public string CreationDate { get; set; }
        public string MonthName { get; set; }
        public string Format { get; set; }
        public string Source { get; set; } 


        public DocXModel(string creationDate, string monthName, string format)
        {
            CreationDate = creationDate;
            MonthName = monthName;
            Format = format;
            Source = MonthName + Format;
            Document = (DocX) RepairFile();
        }

        public Object RepairFile()
        {
            string rutaTemporal = "doc_temp.docx";

            // Descomprimir temporalmente
            ZipFile.ExtractToDirectory(this.Source, "doc_temp");

            // Reemplazar en todos los archivos XML
            foreach (var xml in Directory.GetFiles("doc_temp", "*.xml", SearchOption.AllDirectories))
            {
                string contenido = File.ReadAllText(xml);
                contenido = contenido.Replace("w:jc w:val=\"start\"", "w:jc w:val=\"left\"");
                File.WriteAllText(xml, contenido);
            }

            // Volver a comprimir
            if (File.Exists(rutaTemporal)) File.Delete(rutaTemporal);
            ZipFile.CreateFromDirectory("doc_temp", rutaTemporal);

            // Limpiar carpeta temporal
            Directory.Delete("doc_temp", true);

            // Cargar el documento reparado
            var doc = Xceed.Words.NET.DocX.Load(rutaTemporal);
            return doc;
        }

        public void SaveFile()
        {
            Document.Save();
        }
    }
}
