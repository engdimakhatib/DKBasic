using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasic.structure
{
    public class TProcedure : TIdentifier
    {
        public TVar Params_In1;
        public TVar Params_In2;
        public TVar Params_Out1;
        public TVar Params_Out2;
        public Boolean Is_Define;
        public Tinstruction INS; 
        public TProcedure()
        {
            Params_In1 = null;
            Params_In2 = null;
            Params_Out1 = null;
            Params_Out2 = null;
            Is_Define = false;
            INS = null;
        }
        public static void Free(TProcedure Procedure_Free)
        {
            Procedure_Free.Is_Define = false;
            Procedure_Free = null;
          //  GC.Collect();
        }
    }
}
