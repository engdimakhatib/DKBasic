
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasic.structure
{
    public class TCall
    {
        public TProcedure p;
        public Texp ParamsIN;
        public TListVar ParamsOut;

        public static void Free(TCall Call_Free)
        {
            Texp.FreeList(Call_Free.ParamsIN);
            while (Call_Free.ParamsOut != null)
            {
                TListVar LV_Aux = Call_Free.ParamsOut.next;
                TListVar.Free(Call_Free.ParamsOut);
                Call_Free.ParamsOut = LV_Aux;
            }
       
        }
    }
}
