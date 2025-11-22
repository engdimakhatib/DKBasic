using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasic.structure
{
    public class Tinstruction
    {
        public object INS;
        public Tinstruction next;

        public static void Free(Tinstruction Inst_Free)
        {
            while (Inst_Free != null)
            {
                Tinstruction Aux = Inst_Free.next;

                #region Free

                if (Inst_Free.INS is Tassign)
                {
                    Tassign.Free(Inst_Free.INS as Tassign);
                }

                else
              if (Inst_Free.INS is Twhile)
                {
                    Twhile.Free(Inst_Free.INS as Twhile);
                }
                else
            if (Inst_Free.INS is Tif)
                {
                    Tif.Free(Inst_Free.INS as Tif);
                }

                else
                if (Inst_Free.INS is TRead)
                {
                    TRead.Free(Inst_Free.INS as TRead);
                }
                else
               if (Inst_Free.INS is TWrite)
                {
                    TWrite.Free(Inst_Free.INS as TWrite);
                }
                else
              if (Inst_Free.INS is TCall)
                {
                    TCall.Free(Inst_Free.INS as TCall);
                }
                else
              if (Inst_Free.INS is TSwitch)
                {
                    TSwitch.Free(Inst_Free.INS as TSwitch);
                }
                else
              if (Inst_Free.INS is TPost_Pre_Var)
                {
                    TPost_Pre_Var.Free(Inst_Free.INS as TPost_Pre_Var);
                }
                else
              if (Inst_Free.INS is TFor)
                {
                    TFor.Free(Inst_Free.INS as TFor);
                }
                else
              if (Inst_Free.INS is TBreak_Continue)
                {
                    TBreak_Continue.Free(Inst_Free.INS as TBreak_Continue);
                }
                else
                    Inst_Free = null;

                #endregion free

                Inst_Free = Aux;
                //GC.Collect();
            }
        }

    }



}
