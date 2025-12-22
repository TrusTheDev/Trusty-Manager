using System;
using System.Windows.Forms;
namespace TrustyManager.Interfaces
{
    public interface IFile
    {
        string FilePath { get; }
        string CreationDate { get; set; }
        //clean up for base branch
        string MonthName { get; set; }
        string FileName { get; set; }
        string Format { get; set; }
        string Source { get; set; }
        Object RepairFile();
        void SaveFile(DataGridView grid);
    }
}
