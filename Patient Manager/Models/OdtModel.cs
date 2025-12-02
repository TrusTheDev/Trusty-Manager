using AODL.Document.TextDocuments;
using Patient_Manager.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Patient_Manager.Models
{
    internal class OdtModel : IFile
    {
        public TextDocument odtDoc { get; set; }
        public string CreationDate { get; set; }
        public string MonthName { get; set; }
        public string FileName { get; set; }
        public string Format { get; set; }
        public string Source { get; set; }
        public string FilePath { get; }

        public OdtModel(string creationDate, string monthName, string format, string source, string fileName, string filePath)
        {
            CreationDate = creationDate;
            MonthName = monthName;
            Format = format;
            Source = source;
            odtDoc = (TextDocument)RepairFile();
            FileName = fileName;
            FilePath = filePath;
        }
        public object RepairFile()
        {
            string rutaTemporal = "odt_temp.odt";
            // Aquí iría la lógica para reparar el archivo ODT si es necesario.
            return null;
        }

        public void gridViewToFile(DataGridView grid)
        {
            if (grid.Rows.Count != 0)
            {
                TextDocument temporaryODT = new TextDocument();
                int filas = grid.Rows.Cast<DataGridViewRow>().Count(r => !r.IsNewRow);

            }
        }

        public void SaveFile(DataGridView grid)
        {
            throw new NotImplementedException();
        }
    }
}
