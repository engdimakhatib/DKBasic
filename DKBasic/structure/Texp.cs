using DKBasic.helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasic.structure
{
    public class Texp
    {
        public Global.Type_Symbol token;
        public string Val_STR;
        public double Val_NB;
        public TVar Val_Var;
        public Texp next;
        public Texp prev;

        public static void Free (Texp Exp_Free)
        {
            if (Exp_Free == null) return;
            Exp_Free.token = 0;
            Exp_Free.Val_NB = 0;
            if (Exp_Free.Val_Var != null)
                Exp_Free.Val_Var = null;
            if (Exp_Free.next != null)
                Exp_Free.next = null;
            if (Exp_Free.prev != null)
                Exp_Free.prev = null;
        }

        public static void FreeList(Texp head)
        {
            Texp current = head;
            while (current != null)
            {
                Texp next = current.next;
                Free(current);
                current = next;
            }
        }
    }
  

}
