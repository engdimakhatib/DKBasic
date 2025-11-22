using DKBasic.helper;
using DKBasic.structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasic.Compiling_Steps
{
    public class Free_Class
    {
        public static void INTIAl_VARS()
        {
            // تحرير المتغيرات العامة
            TVar Var_Aux = Global.G_Var;
            TVar.Free_G_VAR(Var_Aux);

           Global.G_Var = null;

            // تحرير بارامترات الإجراءات
            TProcedure Proc_Aux = Global.G_Procedure;
            while (Proc_Aux != null)
            {
                TVar.Free_G_VAR(Proc_Aux.Params_In1);
                Proc_Aux.Params_In1 = null;
                Proc_Aux = (TProcedure) Proc_Aux.next;
            }
            Global.G_Procedure = null;

        }

        public static void Free_ALL()
        {
                  INTIAl_VARS();

            Tinstruction.Free(Global.G_Main_Instruction);
            Global.G_Main_Instruction = null;

            // تحرير جميع الإجراءات
            while (Global.G_Procedure != null)
            {
                TProcedure Proc_Aux = (TProcedure)Global.G_Procedure.next;

                // تم تحرير Params_In1 بالفعل في INTIAl_VARS
                Tinstruction.Free(Global.G_Procedure.INS);
                TProcedure.Free(Global.G_Procedure);

                Global.G_Procedure = Proc_Aux;
            }
          
            TFile. Free_G_File(Global.G_File);
            Global.G_File = null;
            GC.Collect();
        }

      
    }
}


