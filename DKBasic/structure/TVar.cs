using DKBasic.helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasic.structure
{
    public class TVar : TIdentifier
    {
        public Global.Type_Symbol token;
        public double Val_NB;
        public string Val_STR;


        public static void Free_G_VAR(TVar GV_Free)
        {
            while (GV_Free != null)
            {
                TVar nextVar = (TVar)GV_Free.next;
               Free(GV_Free);
                GV_Free = nextVar;
            }

        }
        public static void Free(TVar Var_Free)
        {
            if(Var_Free != null)
            Var_Free = null;
           // GC.Collect();
        }
    }
}
