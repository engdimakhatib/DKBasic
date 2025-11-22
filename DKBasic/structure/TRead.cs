using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasic.structure
{
    internal class TRead
    {
        public TListVar Lv;
        public TRead()
        {
            Lv = null;
        }
        public static void Free(TRead Read_Free)
        {
            TListVar.FreeList(Read_Free.Lv);
     
            Read_Free = null;
           // GC.Collect();
        }
    }
}
