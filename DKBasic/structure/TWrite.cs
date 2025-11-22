using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasic.structure
{
    internal class TWrite
    {
        public TListExp Lexp;
        public Boolean Is_Ln = false;

        public static void Free(TWrite Write_Free)
        {
            TListExp.FreeList(Write_Free.Lexp);

          //  Write_Free = null;
          //  GC.Collect();
        }

    }
}
