using DKBasic.Compiling_Steps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasic.structure
{
    internal class Twhile
    {
        public Texp Cond;
        public Tinstruction Inst;

        public static void Free(Twhile While_Free)
        {
           Texp.FreeList(While_Free.Cond);
            Tinstruction.Free(While_Free.Inst);
           // While_Free = null;
         //   GC.Collect();
        }
    }
}
