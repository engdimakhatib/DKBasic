using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasic.structure
{
    internal class TSwitch
    {
        public TVar var;
        public Tvalue values;
        public Tinstruction Deafult_Instruction;

        public static void Free(TSwitch Switch_Free)
        {
         while(Switch_Free.values != null)
            {
                Tvalue value_next = Switch_Free.values.next;
                Texp.FreeList(Switch_Free.values.exp);
                Tinstruction.Free(Switch_Free.values.instruction);
                Switch_Free.values = null;
                Switch_Free.values = value_next;
            }
        }
    }
}
