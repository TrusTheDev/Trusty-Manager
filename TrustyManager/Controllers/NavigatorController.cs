using TrustyManager.Interfaces;
using TrustyManager.Models;
using System;
namespace TrustyManager.Controllers
{
    internal class NavigatorController
    {
        int Pointer { get; set; }
        DocumentModelList List { get; set; }
        public void AssignFile(DocumentModelList list)
        {
            List = list;
            Pointer = list.ReturnLastPosition();
        }
        
        public IFile GetNextFile()
        {
            if (Pointer == List.ReturnLastPosition())
            {
                Pointer = 0;
            }
            else
            {
                Pointer++;
            }
            return List[Pointer];
        }

        public IFile GetPreviousFile()
        {
            if (Pointer == List.ReturnFirstPosition())
            {
                GetLastFile();
            }
            else
            {
                Pointer--;
            }
            return List[Pointer];
        }

        public IFile GetcurrentFile()
        {
            return List[Pointer];
        }

        public IFile GetLastFile()
        {
            Pointer = List.ReturnLastPosition();
            return List[Pointer];
        }
    }
}
