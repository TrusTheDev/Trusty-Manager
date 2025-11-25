using System;
using System.Windows.Forms;
namespace Patient_Manager.Interfaces
{
    internal interface IFile
    {
        string CreationDate { get; set; }
        string MonthName { get; set; }
        string Format { get; set; }
        string Source { get; set; }
        Object RepairFile();


        void SaveFile(DataGridView grid);
    }
}
