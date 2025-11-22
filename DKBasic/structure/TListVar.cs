using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasic.structure
{
    public class TListVar
    {
        public TVar Var;
        public TListVar next;


        // طريقة لتحرير قائمة TListVar كاملة
        public static void FreeList(TListVar list)
        {
            TListVar current = list; // العقدة الحالية
            while (current != null) // طالما هناك عقد
            {
                TListVar next = current.next; // حفظ المرجع للعقدة التالية

                // تحرير العقدة الحالية
                Free(current);

                // الانتقال للعقدة التالية
                current = next;
            }
        }

        // طريقة لتحرير عقدة مفردة من TListVar
        public static void Free(TListVar node)
        {
            if (node == null) return; // إذا كانت العقدة فارغة لا تفعل شيئاً

            // فصل جميع الروابط
            node.Var = null; // تحرير المرجع للمتغير
            node.next = null; // فصل الرابط للعقدة التالية

            // هنا يمكن إضافة أي تحرير إضافي للذاكرة إذا لزم الأمر
            // Note: في بعض الحالات قد لا نريد تحرير Var إذا كان مشتركاً في جدول الرموز
        }
    }
}
