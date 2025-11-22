using DKBasic.Compiling_Steps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasic.structure
{
    internal class Tassign
    {
        public TVar Var;
        public Texp exp;

        public static void Free(Tassign Assign_Free)
        {
            TVar.Free_G_VAR(Assign_Free.Var);
          Texp.FreeList(Assign_Free.exp);

           // Assign_Free = null;
      //      GC.Collect();
        }
    }

}
