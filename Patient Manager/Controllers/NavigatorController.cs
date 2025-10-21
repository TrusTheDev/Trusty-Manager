using Patient_Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Patient_Manager.Interfaces;

namespace Patient_Manager.Controllers
{
    internal class NavigatorController
    {
        int Pointer {get;set;}
        DocumentModelList List { get;set;}
        public NavigatorController(DocumentModelList list)
        {
            List = list;
            Pointer = list.ReturnLastPosition();
        }

        public IFile getNextFile()
        {
            if(Pointer == List.ReturnLastPosition())
            {
                Pointer = 0;
            }
            else
            {
                Pointer++;
            }
            return List[Pointer];
        }

        public IFile getPreviousFile()
        {
            if (Pointer == List.ReturnFirstPosition())
            {
                Pointer = List.ReturnLastPosition();
            }
            else
            {
                Pointer--;
            }
            return List[Pointer];
        }

        public IFile currentFile()
        {
           return List[Pointer]; 
        }

    }
}
