using DKBasic.Compiling_Steps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasic.structure
{
    internal class Tif
    {
        public Texp Cond;
        public Tinstruction Ins_If;
        public Tinstruction Ins_Else;

        public static void Free(Tif If_Free)
        {
           Texp.Free(If_Free.Cond);
            Tinstruction.Free(If_Free.Ins_If);
            Tinstruction.Free(If_Free.Ins_Else);
          //  If_Free = null;
           // GC.Collect();
        }
    }
}
