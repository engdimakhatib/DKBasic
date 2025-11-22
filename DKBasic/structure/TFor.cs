using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasic.structure
{
    internal class TFor
    {
        public TVar var;
        public Tinstruction L_inst;
        public Texp Exp_Begin;
        public Texp Exp_End;
        public Texp Exp_Step;
        public Boolean Is_Down;

        public static void Free(TFor For_Free )
        {
            TVar.Free(For_Free.var);
            Texp.Free(For_Free.Exp_Begin);
            Texp.Free(For_Free.Exp_End);
            Texp.Free(For_Free.Exp_Step);
            For_Free = null;
        }
    }
}
