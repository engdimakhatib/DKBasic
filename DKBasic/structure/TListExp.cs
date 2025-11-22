using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasic.structure
{
    internal class TListExp
    {
        public Texp exp;
        public TListExp next;


        // طريقة لتحرير قائمة TListExp كاملة مع التعبيرات الداخلية
        public static void FreeList(TListExp list)
        {
            TListExp current = list; // العقدة الحالية في القائمة
            while (current != null) // طالما هناك عقد في القائمة
            {
                TListExp next = current.next; // حفظ المرجع للعقدة التالية

                // تحرير العقدة الحالية
              //  Free(current);
                Texp.FreeList(current.exp);
                // الانتقال للعقدة التالية
                current = next;
            }
        }

        // طريقة لتحرير عقدة مفردة من TListExp
        public static void Free(TListExp node)
        {
            if (node == null) return; // إذا كانت العقدة فارغة لا تفعل شيئاً

            // تحرير التعبير إذا كان موجوداً
            if (node.exp != null)
            {
                Texp.FreeList(node.exp); // تحرير التعبير باستخدام الطريقة الموجودة في Texp
                node.exp = null; // تعيين التعبير إلى فارغ
            }

            // فصل الرابط للعقدة التالية
            node.next = null;

            // هنا يمكن إضافة أي تحرير إضافي للذاكرة إذا لزم الأمر
        }


}
}