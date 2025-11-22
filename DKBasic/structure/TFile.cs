using DKBasic.helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasic.structure
{
    public class TFile
    {
        public string name;
        public TFile next;
        public static void Free_G_File(TFile File_Free)
        {
            while (Global.G_File != null)
            {
                TFile Aux = Global.G_File.next;
                TFile.Free(Global.G_File);
                Global.G_File = Aux;
            }
        }
       
    public static void Free(TFile File_Free)
        {
            File_Free.name = null;
            File_Free.next = null;
         //   File_Free = null;
        //    GC.Collect();
        }
    }
}
