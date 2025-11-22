using DKBasic.helper;
using DKBasic.structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasic.Compiling_Steps
{
    internal class Optimization
    {
        public static Texp Copy_Exp(Texp Exp)
        {
            Texp First_Exp = null;
            Texp Last_Exp = null;
            while ( Exp != null)
            {
                Texp New_Exp = new Texp();
                New_Exp.token = Exp.token;
                New_Exp.Val_NB = Exp.Val_NB;
                New_Exp.Val_STR = Exp.Val_STR;
                New_Exp.Val_Var = Exp.Val_Var;
                New_Exp.next = null;
                if(First_Exp == null)
                {
                    First_Exp = New_Exp;
                }
                else
                {
                    Last_Exp.next = New_Exp;
                    New_Exp.prev = Last_Exp;
                }
                Last_Exp = New_Exp;
                Exp = Exp.next;
            }
            return First_Exp;
        }

       public static Boolean Exaluate_Cond(Texp cond)
        {
          Texp exp =  Evaluate_Exp(cond);
            if( (exp.token != Global.Type_Symbol.u_TRUE )&& (exp.token != Global.Type_Symbol.u_FALSE))
            {
                Global.Message_Wrong = Error.Get_Error(39) + "\t" + "\t" + Error.Get_Type_Error(2);
                throw new Exception();
            }
            Boolean Result = (exp.token == Global.Type_Symbol.u_TRUE);
            Texp.FreeList(exp);
            return Result;
        }
        public static Texp Evaluate_Exp(Texp exp)
        {
            Texp exp0 = Copy_Exp(exp);
            Texp Current_Exp = exp0;
            while ( Current_Exp != null)
            {
                if(Current_Exp.token == Global.Type_Symbol.u_CST_INT || Current_Exp.token == Global.Type_Symbol.u_CST_REAL 
                    || Current_Exp.token == Global.Type_Symbol.u_CST_STR || Current_Exp.token == Global.Type_Symbol.u_TRUE
                   || Current_Exp.token == Global.Type_Symbol.u_FALSE)
                {
                    #region constants
                    Current_Exp = Current_Exp.next;
                    #endregion
                }
                else if (Current_Exp.token == Global.Type_Symbol.u_PRE_INCR || Current_Exp.token == Global.Type_Symbol.u_PRE_DECR 
                    || Current_Exp.token == Global.Type_Symbol.u_POST_INCR || Current_Exp.token == Global.Type_Symbol.u_POST_DECR
                    )
                {
                    #region ++v,v++,--v,v--

                    TVar var_Aux = Current_Exp.Val_Var;

                    if (var_Aux.token != Global.Type_Symbol.u_CST_INT && var_Aux.token != Global.Type_Symbol.u_CST_REAL)
                    {
                        Global.Message_Wrong = Error.Get_Error(36) + "\t" + "\t" + Error.Get_Type_Error(2);
                        throw new Exception();
                    }
                    double originalValue = var_Aux.Val_NB;
                    if(Current_Exp.token == Global.Type_Symbol.u_PRE_INCR || Current_Exp.token == Global.Type_Symbol.u_PRE_DECR)
                    {                             
                        if (Current_Exp.token == Global.Type_Symbol.u_PRE_INCR)
                            var_Aux.Val_NB += 1;
                        else
                            var_Aux.Val_NB -= 1;
                        Current_Exp.Val_NB = var_Aux.Val_NB;
                    }             
                    else if (Current_Exp.token == Global.Type_Symbol.u_POST_DECR || Current_Exp.token == Global.Type_Symbol.u_POST_INCR)
                    {
                        Current_Exp.Val_NB = originalValue;
                        if (Current_Exp.token == Global.Type_Symbol.u_POST_INCR)
                            var_Aux.Val_NB += 1;
                        else
                            var_Aux.Val_NB -= 1;
                    }
                    Current_Exp.token = Global.Type_Symbol.u_CST_REAL;
                    #endregion
                }

                else if(Current_Exp.token == Global.Type_Symbol.u_VAR)
                {
                    #region variables
                    TVar var_Aux = null;
                    var_Aux = Current_Exp.Val_Var;
                    Current_Exp.token = var_Aux.token;
                    Current_Exp.Val_NB = var_Aux.Val_NB;
                    Current_Exp.Val_STR = var_Aux.Val_STR;
                    Current_Exp = Current_Exp.next;
                    #endregion
                }
                //شرط للتحقق من وجود عملية أحادية ( ذات معامل وحيد) 
                else if(Current_Exp.token == Global.Type_Symbol.u_UNARY_MINUS ||
Current_Exp.token == Global.Type_Symbol.u_UNARY_PLUS ||
Current_Exp.token == Global.Type_Symbol.u_NOT ||
Current_Exp.token == Global.Type_Symbol.u_SIN ||
Current_Exp.token == Global.Type_Symbol.u_COS ||
Current_Exp.token == Global.Type_Symbol.u_TAN ||
Current_Exp.token == Global.Type_Symbol.u_ATAN ||
Current_Exp.token == Global.Type_Symbol.u_STRING_TO_INT ||
Current_Exp.token == Global.Type_Symbol.u_LENGTH ||
Current_Exp.token == Global.Type_Symbol.u_INT_TO_STRING ||
Current_Exp.token == Global.Type_Symbol.u_LOG ||
Current_Exp.token == Global.Type_Symbol.u_LN ||
Current_Exp.token == Global.Type_Symbol.u_SQRT ||
Current_Exp.token == Global.Type_Symbol.u_ABS)
                {
                    #region Functions with one parameter 
                    #region condition for type one parameter

                    //شرط لمنع تطبيق العمليات الأحادية الرياضية على قيم غير رقمية  
                    if ((Current_Exp.token == Global.Type_Symbol.u_UNARY_MINUS ||
                  Current_Exp.token == Global.Type_Symbol.u_SIN ||
                  Current_Exp.token == Global.Type_Symbol.u_TAN ||
                   Current_Exp.token == Global.Type_Symbol.u_ATAN ||
                  Current_Exp.token == Global.Type_Symbol.u_COS ||
                  Current_Exp.token == Global.Type_Symbol.u_UNARY_PLUS ||
                  Current_Exp.token == Global.Type_Symbol.u_LOG ||
                  Current_Exp.token == Global.Type_Symbol.u_LN ||
                  Current_Exp.token == Global.Type_Symbol.u_SQRT ||
                  Current_Exp.token == Global.Type_Symbol.u_ABS)
                   &&
                    !(Current_Exp.prev.token == Global.Type_Symbol.u_CST_INT ||
                  Current_Exp.prev.token == Global.Type_Symbol.u_CST_REAL))
                    {
                        Global.Message_Wrong = Error.Get_Error(36) + "\t" + "\t" + Error.Get_Type_Error(2);
                        throw new Exception();
                    }
                    /*
 شرط للعمليات الأحادية  علي السلسة النصية مثل : طول السلسة و تحويل السلسة النصية لرقم 
و بالتالي يجب التأكد من أن معامل العملية هو سلسة نصية 
*/
                    if ((Current_Exp.token == Global.Type_Symbol.u_LENGTH ||
                     Current_Exp.token == Global.Type_Symbol.u_STRING_TO_INT) &&
                    (Current_Exp.prev.token != Global.Type_Symbol.u_CST_STR))
                    {
                         Global.Message_Wrong = Error.Get_Error(37) + "\t" + "\t" + Error.Get_Type_Error(2);
                        throw new Exception();
                    }
                    //  شرط لعملية تحويل الرقم لنص  و بالتالي يجب التأكد من أن معامل العملية هو رقم صحيح
                    if (Current_Exp.token == Global.Type_Symbol.u_INT_TO_STRING &&
                    Current_Exp .prev.token != Global.Type_Symbol.u_CST_INT)
                            {

                        Global.Message_Wrong = Error.Get_Error(38) + "\t" + "\t" + Error.Get_Type_Error(2);
                        throw new Exception();

                    }
                    /*
شرط للعملية الأحادية المنطقية  Not  
وفي حال لم يكن المعامل السابق قيمة منطقية أي لم يكن التوكن u_TRUE , u_FALSE 
*/
                    if (Current_Exp.token == Global.Type_Symbol.u_NOT &&
                    !(Current_Exp.prev.token == Global.Type_Symbol.u_TRUE ||
                  Current_Exp.prev.token == Global.Type_Symbol.u_FALSE))
                    {
                        Global.Message_Wrong = Error.Get_Error(39) + "\t" + "\t" + Error.Get_Type_Error(2);
                        throw new Exception();
                    }


                    #endregion condition for type one parameter
                    #region evaluate functions with one parameter
                    if (Current_Exp.token == Global.Type_Symbol.u_SIN)
                    {
                        Current_Exp.prev.token = Global.Type_Symbol.u_CST_REAL;
                        Current_Exp.prev.Val_NB = Math.Sin(Current_Exp.prev.Val_NB);
                    }
                    else if (Current_Exp.token == Global.Type_Symbol.u_COS)
                    {
                        Current_Exp.prev.token = Global.Type_Symbol.u_CST_REAL;
                        Current_Exp.prev.Val_NB = Math.Cos(Current_Exp.prev.Val_NB);
                    }
                    else if (Current_Exp.token == Global.Type_Symbol.u_TAN)
                    {
                        Current_Exp.prev.token = Global.Type_Symbol.u_CST_REAL;
                        Current_Exp.prev.Val_NB = Math.Tan(Current_Exp.prev.Val_NB);
                    }
                    else if (Current_Exp.token == Global.Type_Symbol.u_ATAN)
                    {
                        Current_Exp.prev.token = Global.Type_Symbol.u_CST_REAL;
                        Current_Exp.prev.Val_NB = Math.Atan(Current_Exp.prev.Val_NB);
                    }
                    else if (Current_Exp.token == Global.Type_Symbol.u_LOG)
                    {
                        Current_Exp.prev.token = Global.Type_Symbol.u_CST_REAL;
                        Current_Exp.prev.Val_NB = Math.Log10(Current_Exp.prev.Val_NB);
                    }
                    else if (Current_Exp.token == Global.Type_Symbol.u_LN)
                    {
                        Current_Exp.prev.token = Global.Type_Symbol.u_CST_REAL;
                        Current_Exp.prev.Val_NB = Math.Log(Current_Exp.prev.Val_NB);
                    }
                    else if (Current_Exp.token == Global.Type_Symbol.u_SQRT)
                    {
                        if (Current_Exp.prev.Val_NB < 0)
                        {
                            Global.Message_Wrong = Error.Get_Error(42) + "\t" + "\t" + Error.Get_Type_Error(2);
                            throw new Exception();
                        }
                        Current_Exp.prev.token = Global.Type_Symbol.u_CST_REAL;
                        Current_Exp.prev.Val_NB = Math.Sqrt(Current_Exp.prev.Val_NB);
                    }
                    else if (Current_Exp.token == Global.Type_Symbol.u_ABS)
                    {
                        if (Current_Exp.prev.Val_NB < -1)
                        {
                            Current_Exp.prev.Val_NB = -1 * (Current_Exp.prev.Val_NB);
                        }

                    }
                    else if (Current_Exp.token == Global.Type_Symbol.u_LENGTH)
                    {
                        Current_Exp.prev.token = Global.Type_Symbol.u_CST_INT;
                        Current_Exp.prev.Val_NB = (Current_Exp.prev.Val_STR).Length;
                    }
                    else if (Current_Exp.token == Global.Type_Symbol.u_NOT)
                    {
                        if (Current_Exp.prev.token == Global.Type_Symbol.u_FALSE)
                        {
                            Current_Exp.prev.token = Global.Type_Symbol.u_TRUE;
                        }
                        else
                        {
                            Current_Exp.prev.token = Global.Type_Symbol.u_FALSE;
                        }
                    }
                    else if (Current_Exp.token == Global.Type_Symbol.u_UNARY_MINUS)
                    {
                        Current_Exp.prev.Val_NB = -1 * (Current_Exp.prev.Val_NB);
                    }
                    else if (Current_Exp.token == Global.Type_Symbol.u_UNARY_PLUS)
                    {
                        Current_Exp.prev.Val_NB = +1 * (Current_Exp.prev.Val_NB);
                    }
                    else if (Current_Exp.token == Global.Type_Symbol.u_INT_TO_STRING)
                    {
                        try
                        {
                            Current_Exp.prev.token = Global.Type_Symbol.u_CST_STR;
                            Current_Exp.prev.Val_STR = (Current_Exp.prev.Val_NB).ToString();
                        }
                        catch (Exception ex)
                        {
                            Global.Message_Wrong = Error.Get_Error(40) + "\t" + "\t" + Error.Get_Type_Error(2);
                            throw new Exception();
                        }
                    }
                    else if (Current_Exp.token == Global.Type_Symbol.u_STRING_TO_INT)
                    {
                        try
                        {
                            Current_Exp.prev.token = Global.Type_Symbol.u_CST_INT;
                            Current_Exp.prev.Val_NB = Convert.ToInt32(Current_Exp.prev.Val_STR);
                        }
                        catch (Exception ex)
                        {
                            Global.Message_Wrong = Error.Get_Error(41) + "\t" + "\t" + Error.Get_Type_Error(2);
                            throw new Exception();
                        }
                    }


                    #endregion evaluate functions with one parameter

                 if (Current_Exp.next != null)
                    Current_Exp.next.prev = Current_Exp.prev;
                    Current_Exp.prev.next = Current_Exp.next;

                    Texp Aux = Current_Exp.next;
                    Texp.Free(Current_Exp);
                    Current_Exp = Aux;

                    #endregion Functions with one parameter 
                }

                // يتحقق من وجود عملية ثنائية 
else if(Current_Exp.token == Global.Type_Symbol.u_AND ||
Current_Exp.token == Global.Type_Symbol.u_OR ||
Current_Exp.token == Global.Type_Symbol.u_PLUS ||
Current_Exp.token == Global.Type_Symbol.u_MINUS ||
Current_Exp.token == Global.Type_Symbol.u_MULTI ||
Current_Exp.token == Global.Type_Symbol.u_DIV ||
Current_Exp.token == Global.Type_Symbol.u_MOD ||
Current_Exp.token == Global.Type_Symbol.u_EQUAL ||
Current_Exp.token == Global.Type_Symbol.u_NOT_EQUAL ||
Current_Exp.token == Global.Type_Symbol.u_GT ||
Current_Exp.token == Global.Type_Symbol.u_GE ||
 Current_Exp.token == Global.Type_Symbol.u_LT ||
Current_Exp.token == Global.Type_Symbol.u_LE )
   {
                    #region Function with two parameters
                    #region condition Function with two parameters
                    //شرط2 
                    // الجزء الأول: (التحقق من المعاملات)
                    // إذا كان الرمز الحالي هو أحد المعاملات الحسابية.
                    if ((Current_Exp.token == Global.Type_Symbol.u_MINUS ||  // إذا كان الرمز الحالي هو عملية طرح (-)
                         Current_Exp.token == Global.Type_Symbol.u_MULTI ||   // أو عملية ضرب (*)
                         Current_Exp.token == Global.Type_Symbol.u_DIV ||    // أو عملية قسمة (/)
                         Current_Exp.token == Global.Type_Symbol.u_LE ||      // أو مقارنة (<=)
                         Current_Exp.token == Global.Type_Symbol.u_GT ||      // أو مقارنة (>)
                         Current_Exp.token == Global.Type_Symbol.u_LT ||      // أو مقارنة (<)
                         Current_Exp.token == Global.Type_Symbol.u_MOD ||     // أو عملية modulo (%)
                         Current_Exp.token == Global.Type_Symbol.u_GE)        // أو مقارنة (>=)
                        &&
                        // الجزء الثاني: (الاستثناء) - يتحقق من أن العملية ليست بين قيمتين ثابتتين
                        !((Current_Exp.prev.token == Global.Type_Symbol.u_CST_INT ||
                           // إذا لم يكن الرمز السابق عدداً صحيحاً
                           Current_Exp.prev.token == Global.Type_Symbol.u_CST_REAL) &&
                          // أو عدداً حقيقياً
                          (Current_Exp.prev.prev.token == Global.Type_Symbol.u_CST_INT ||
                           // والرمز قبل السابق عدداً صحيحاً
                           Current_Exp.prev.prev.token == Global.Type_Symbol.u_CST_REAL)))
                    // أو عدداً حقيقياً
                    {
                        // الكود هنا سينفذ إذا كانت العملية الحسابية ليست بين قيمتين ثابتتين
                        Global.Message_Wrong = Error.Get_Error(36) + "\t" + "\t" + Error.Get_Type_Error(2);
                        throw new Exception();
                    }
                    // الجزء الأول: (التحقق من المعاملات المنطقية)
                    // يتحقق من: إذا كان الرمز الحالي هو معامل منطقي (AND أو OR).
                    if ((Current_Exp.token == Global.Type_Symbol.u_AND || Current_Exp.token == Global.Type_Symbol.u_OR) &&
               //  الجزء الثاني: (الاستثناء - الحالة الصحيحة)
!((Current_Exp.prev.token == Global.Type_Symbol.u_FALSE || Current_Exp.prev.token == Global.Type_Symbol.u_TRUE)
&&
  (Current_Exp.prev.prev.token == Global.Type_Symbol.u_FALSE || Current_Exp.prev.prev.token == Global.Type_Symbol.u_TRUE)))
{
                        Global.Message_Wrong = Error.Get_Error(43) + "\t" + "\t" + Error.Get_Type_Error(2);
                        throw new Exception();

                    }
                    // =  , !=  , <> 
                    if ((Current_Exp.token == Global.Type_Symbol.u_EQUAL || Current_Exp.token == Global.Type_Symbol.u_NOT_EQUAL)
                    &&
                  // الجزء الثاني: (الاستثناء - الحالات الصحيحة)
                   !((((Current_Exp.prev.token == Global.Type_Symbol.u_CST_INT ||
                    Current_Exp.prev.token == Global.Type_Symbol.u_CST_REAL) &&
                    (Current_Exp.prev.prev.token == Global.Type_Symbol.u_CST_INT ||
                    Current_Exp.prev.prev.token == Global.Type_Symbol.u_CST_REAL)) ||
                    ((Current_Exp.prev.token == Global.Type_Symbol.u_TRUE ||
                    Current_Exp.prev.token == Global.Type_Symbol.u_FALSE) &&
                    (Current_Exp.prev.prev.token == Global.Type_Symbol.u_TRUE ||
                    Current_Exp.prev.prev.token == Global.Type_Symbol.u_FALSE))) ||
                    (Current_Exp.prev.token == Global.Type_Symbol.u_CST_STR &&
                    Current_Exp.prev.prev.token == Global.Type_Symbol.u_CST_STR)))
                    {
                        Global.Message_Wrong = Error.Get_Error(44) + "\t" + "\t" + Error.Get_Type_Error(2);
                        throw new Exception();
                    }
      if (Current_Exp.token == Global.Type_Symbol.u_PLUS &&
    // إذا كان الرمز الحالي هو علامة الجمع (+)
    !(
        // (  وليس (نفي للحالات الصحيحة 
        (Current_Exp.prev.token == Global.Type_Symbol.u_CST_STR &&
          // الحالة الأولى: إذا كان الرمز السابق نصاً
          Current_Exp.prev.prev.token == Global.Type_Symbol.u_CST_STR) ||
        // والرمز قبل السابق نصاً أيضاً (نص + نص)

        ((Current_Exp.prev.token == Global.Type_Symbol.u_CST_INT ||
           // الحالة الثانية: إذا كان الرمز السابق عدداً صحيحاً
           Current_Exp.prev.token == Global.Type_Symbol.u_CST_REAL)
          // أو عدداً حقيقياً
          &&
          (Current_Exp.prev.prev.token == Global.Type_Symbol.u_CST_INT ||
           // والرمز قبل السابق عدداً صحيحاً
           Current_Exp.prev.prev.token == Global.Type_Symbol.u_CST_REAL))))
                    // أو عدداً حقيقياً (رقم + رقم) 
                    {
                        // الكود هنا سينفذ إذا كانت علامة الجمع بين نوعين غير متوافقين

                        Global.Message_Wrong = Error.Get_Error(45) + "\t" + "\t" + Error.Get_Type_Error(2);
                        throw new Exception();
                    }


                    #endregion condition Function with two parameters

                    #region evaluate Function with Two parameters
                   if (Current_Exp.token == Global.Type_Symbol.u_AND)
                    {
                        if(Current_Exp.prev.token == Global.Type_Symbol.u_FALSE || Current_Exp.prev.prev.token == Global.Type_Symbol.u_FALSE)
                        {
                            Current_Exp.prev.prev.token = Global.Type_Symbol.u_FALSE; 
                        }
                        else Current_Exp.prev.prev.token = Global.Type_Symbol.u_TRUE ;
                    }

                    else if (Current_Exp.token == Global.Type_Symbol.u_OR)
                    {
                        if (Current_Exp.prev.token == Global.Type_Symbol.u_TRUE || Current_Exp.prev.prev.token == Global.Type_Symbol.u_TRUE)
                        {
                            Current_Exp.prev.prev.token = Global.Type_Symbol.u_TRUE;
                        }
                        else Current_Exp.prev.prev.token = Global.Type_Symbol.u_FALSE;
                    }
                   else if (Current_Exp.token == Global.Type_Symbol.u_PLUS)
                    {
                        if(Current_Exp.prev.token == Global.Type_Symbol.u_CST_STR)
                        {
                            Current_Exp.prev.prev.Val_STR = Current_Exp.prev.prev.Val_STR + Current_Exp.prev.Val_STR ;
                        }
                        else
                        {
                            Current_Exp.prev.prev.Val_NB = Current_Exp.prev.Val_NB + Current_Exp.prev.prev.Val_NB;
                            if(Current_Exp.prev.prev.token != Current_Exp.prev.token)
                            {
                                Current_Exp.prev.prev.token = Global.Type_Symbol.u_CST_REAL;
                            }
                        }
                    }

                    else if (Current_Exp.token == Global.Type_Symbol.u_MINUS)
                    {
                       
                            Current_Exp.prev.prev.Val_NB = Current_Exp.prev.prev.Val_NB - Current_Exp.prev.Val_NB;
                            if (Current_Exp.prev.prev.token != Current_Exp.prev.token)
                            {
                                Current_Exp.prev.prev.token = Global.Type_Symbol.u_CST_REAL;
                            }
                        
                    }

                    else if (Current_Exp.token == Global.Type_Symbol.u_MULTI)
                    {

                        Current_Exp.prev.prev.Val_NB = Current_Exp.prev.Val_NB * Current_Exp.prev.prev.Val_NB;
                        if (Current_Exp.prev.prev.token != Current_Exp.prev.token)
                        {
                            Current_Exp.prev.prev.token = Global.Type_Symbol.u_CST_REAL;
                        }

                    }
                    else if (Current_Exp.token == Global.Type_Symbol.u_DIV)
                    {
                        if(Current_Exp.prev.Val_NB == 0 )
                        {
                            Global.Message_Wrong = Error.Get_Error(46) + "\t" + "\t" + Error.Get_Type_Error(2);
                            throw new Exception();
                        }

                        Current_Exp.prev.prev.Val_NB = Current_Exp.prev.prev.Val_NB / Current_Exp.prev.Val_NB;
                        if (Current_Exp.prev.prev.token != Current_Exp.prev.token)
                        {
                            Current_Exp.prev.prev.token = Global.Type_Symbol.u_CST_REAL;
                        }

                    }
                    else if (Current_Exp.token == Global.Type_Symbol.u_MOD)
                    {
                        if (Current_Exp.prev.Val_NB == 0)
                        {
                            Global.Message_Wrong = Error.Get_Error(46) + "\t" + "\t" + Error.Get_Type_Error(2);
                            throw new Exception();
                        }
                        Current_Exp.prev.prev.token = Global.Type_Symbol.u_CST_INT;
                        Current_Exp.prev.prev.Val_NB = Convert.ToInt32( Current_Exp.prev.prev.Val_NB % Current_Exp.prev.Val_NB) ;
                       
                    }

                    else if (Current_Exp.token == Global.Type_Symbol.u_GT)
                    {
                        if(Current_Exp.prev.prev.Val_NB > Current_Exp.prev.Val_NB)
                        {
                            Current_Exp.prev.prev.token = Global.Type_Symbol.u_TRUE;
                        }
                        else
                        {
                            Current_Exp.prev.prev.token = Global.Type_Symbol.u_FALSE;
                        }
                    }
                    else if (Current_Exp.token == Global.Type_Symbol.u_GE)
                    {
                        if (Current_Exp.prev.prev.Val_NB >= Current_Exp.prev.Val_NB)
                        {
                            Current_Exp.prev.prev.token = Global.Type_Symbol.u_TRUE;
                        }
                        else
                        {
                            Current_Exp.prev.prev.token = Global.Type_Symbol.u_FALSE;
                        }
                    }
                    else if (Current_Exp.token == Global.Type_Symbol.u_LT)
                    {
                        if (Current_Exp.prev.prev.Val_NB < Current_Exp.prev.Val_NB)
                        {
                            Current_Exp.prev.prev.token = Global.Type_Symbol.u_TRUE;
                        }
                        else
                        {
                            Current_Exp.prev.prev.token = Global.Type_Symbol.u_FALSE;
                        }
                    }
                    else if (Current_Exp.token == Global.Type_Symbol.u_LE)
                    {
                        if (Current_Exp.prev.prev.Val_NB <= Current_Exp.prev.Val_NB)
                        {
                            Current_Exp.prev.prev.token = Global.Type_Symbol.u_TRUE;
                        }
                        else
                        {
                            Current_Exp.prev.prev.token = Global.Type_Symbol.u_FALSE;
                        }
                    }
                   else if (Current_Exp.token == Global.Type_Symbol.u_EQUAL)
                    {
                        if(Current_Exp.prev.token == Global.Type_Symbol.u_CST_INT || Current_Exp.prev.token == Global.Type_Symbol.u_CST_REAL)
                        { 
                            if(Current_Exp.prev.prev.Val_NB == Current_Exp.prev.Val_NB)
                            {
                                Current_Exp.prev.prev.token = Global.Type_Symbol.u_TRUE;
                            }
                            else
                            {
                                Current_Exp.prev.prev.token = Global.Type_Symbol.u_FALSE;
                            }
                        }
                        else if (Current_Exp.prev.token == Global.Type_Symbol.u_FALSE || Current_Exp.prev.token == Global.Type_Symbol.u_TRUE)
                        {
                            if (Current_Exp.prev.prev.token == Current_Exp.prev.token)
                            {
                                Current_Exp.prev.prev.token = Global.Type_Symbol.u_TRUE;
                            }
                            else
                            {
                                Current_Exp.prev.prev.token = Global.Type_Symbol.u_FALSE;
                            }
                        }
                        else
                        {
                           
                                if (Current_Exp.prev.prev.Val_STR == Current_Exp.prev.Val_STR)
                                {
                                    Current_Exp.prev.prev.token = Global.Type_Symbol.u_TRUE;
                                }
                                else
                                {
                                    Current_Exp.prev.prev.token = Global.Type_Symbol.u_FALSE;
                                }
                           
                        }
                    }
                    else if (Current_Exp.token == Global.Type_Symbol.u_NOT_EQUAL)
                    {
                        if (Current_Exp.prev.token == Global.Type_Symbol.u_CST_INT || Current_Exp.prev.token == Global.Type_Symbol.u_CST_REAL)
                        {
                            if (Current_Exp.prev.prev.Val_NB != Current_Exp.prev.Val_NB)
                            {
                                Current_Exp.prev.prev.token = Global.Type_Symbol.u_TRUE;
                            }
                            else
                            {
                                Current_Exp.prev.prev.token = Global.Type_Symbol.u_FALSE;
                            }
                        }
                        else if (Current_Exp.prev.token == Global.Type_Symbol.u_FALSE || Current_Exp.prev.token == Global.Type_Symbol.u_TRUE)
                        {
                            if (Current_Exp.prev.prev.token != Current_Exp.prev.token)
                            {
                                Current_Exp.prev.prev.token = Global.Type_Symbol.u_TRUE;
                            }
                            else
                            {
                                Current_Exp.prev.prev.token = Global.Type_Symbol.u_FALSE;
                            }
                        }
                        else
                        {

                            if (Current_Exp.prev.prev.Val_STR != Current_Exp.prev.Val_STR)
                            {
                                Current_Exp.prev.prev.token = Global.Type_Symbol.u_TRUE;
                            }
                            else
                            {
                                Current_Exp.prev.prev.token = Global.Type_Symbol.u_FALSE;
                            }

                        }
                    }
                    #endregion evaluate Function with Two parameters


                    Texp Exp_Aux = Current_Exp.prev.prev;
                    if(Current_Exp.next != null)
                    {
Current_Exp.next.prev = Exp_Aux;
                    }
                    Exp_Aux.next = Current_Exp.next;
                    Texp.Free(Current_Exp.prev);
                    Texp.Free(Current_Exp);
                    Current_Exp = Exp_Aux.next;

                    #endregion Function with two parameters
                }


            }
            return exp0;
        }
    }
}
