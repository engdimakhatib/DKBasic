using DKBasic.helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasic.structure
{
    public class TSymbol
    {
        public string name;
        public Global.Type_Symbol token;
        public TSymbol next;
        public static void Free(TSymbol Symbol_Free)
        {
            Symbol_Free.name = null;
            Symbol_Free.token = 0;
            Symbol_Free.next = null;
            Symbol_Free = null;
         //   GC.Collect();
        }
    }
}
