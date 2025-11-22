using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasic.structure
{
    internal class TPost_Pre_Var
    {
        public TVar Var;
        public Boolean isIncrement;

        public static void Free (TPost_Pre_Var post_Pre_Var_Free)
        {
            TVar.Free(post_Pre_Var_Free.Var);
            post_Pre_Var_Free = null;
        }
    }
}
