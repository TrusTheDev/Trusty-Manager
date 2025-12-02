using Patient_Manager.Interfaces;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows.Forms;
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
        public string FileName { get; set; }
        public string FilePath { get; }

        public DocXModel(string creationDate, string monthName, string format, string source, string fileName, string filePath)
        {
            CreationDate = creationDate;
            MonthName = monthName;
            Format = format;
            Source = source;
            Document = (DocX)RepairFile();
            FileName = fileName;
            FilePath = filePath;
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

        public void gridViewToDocX(DataGridView grid)
        {
            if (grid.Rows.Count != 0)
            {
                DocX temporaryDocX = DocX.Create("doc_temporary.docx");

                int filas = grid.Rows.Cast<DataGridViewRow>().Count(r => !r.IsNewRow);
                int columnas = grid.ColumnCount;
                int noVisibles = grid.Columns.Cast<DataGridViewColumn>().Count(c => !c.Visible);

                if(noVisibles > 0)
                {
                    for(int i=0; i < columnas; i++)
                    {
                        if (!grid.Columns[i].Visible)
                        {
                            grid.Columns.RemoveAt(i);
                            columnas--;
                        }
                    }
                }

                var tabla = temporaryDocX.AddTable(filas + 1, columnas);

                for (int j = 0; j < columnas; j++)
                {
                    if (grid.Columns[j].Visible)
                    {
                        tabla.Rows[0].Cells[j].Paragraphs[0].Append(grid.Columns[j].HeaderText).Bold();
                    }
                    else
                    {
                        grid.Columns.RemoveAt(j);
                    }
                }

                int filaIndex = 1;

                for (int i = 0; i < grid.Rows.Count; i++)
                {
                    if (grid.Rows[i].IsNewRow || !grid.Rows[i].Visible) continue;

                    for (int j = 0; j < grid.Columns.Count; j++)
                    {
                        var valor = grid.Rows[i].Cells[j].Value?.ToString() ?? "";
                        tabla.Rows[filaIndex].Cells[j].Paragraphs[0].Append(valor);
                    }
                    filaIndex++;
                }
                temporaryDocX.InsertTable(tabla);
                temporaryDocX.SaveAs(this.Source);
            }
        }
        public void SaveFile(DataGridView grid)
        {
            gridViewToDocX(grid);
            Document.Save();
        }
    }
}
