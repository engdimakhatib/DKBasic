using DKBasic.helper;
using DKBasic.structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasic.Compiling_Steps.Execution_Tree
{
    internal class build_AST_Tree
    {

        public static Texp Read_Condition(ref Texp expLast)
        {
            Texp last1 = null;
            Texp exp0 = Read_CTerm(ref expLast);
            while (Global.token == Global.Type_Symbol.u_OR)
            {
                Global.token = Lexical_Analysis.Lexical_Token();
                Texp exp1 = Read_CTerm(ref last1);
                exp1.prev = expLast;
                expLast.next = exp1;

                // OR بنية 
                Texp expaux = new Texp();
                expaux.token = Global.Type_Symbol.u_OR;
                expaux.prev = last1;
                last1.next = expaux;
                expaux.next = null;
                expLast = expaux;
            }
            return exp0;
        }
        public static Texp Read_CTerm(ref Texp expLast)
        {
            Texp last1 = null;
            Texp exp0 = Read_CFact(ref expLast);
            while (Global.token == Global.Type_Symbol.u_AND)
            {
                Global.token = Lexical_Analysis.Lexical_Token();
                Texp exp1 = Read_CFact(ref last1);
                exp1.prev = expLast;
                expLast.next = exp1;

                // AND بنية 
                Texp expaux = new Texp();
                expaux.token = Global.Type_Symbol.u_AND;
                expaux.prev = last1;
                last1.next = expaux;
                expaux.next = null;
                expLast = expaux;
            }
            return exp0;
        }
        public static Texp Read_CFact(ref Texp expLast)
        {
            Texp last1 = null;
            Texp exp0 = Read_Expression(ref expLast);
            while (Global.token == Global.Type_Symbol.u_GE || Global.token == Global.Type_Symbol.u_GT
                || Global.token == Global.Type_Symbol.u_LE || Global.token == Global.Type_Symbol.u_LT
                || Global.token == Global.Type_Symbol.u_EQUAL || Global.token == Global.Type_Symbol.u_NOT_EQUAL)
            {
                Global.Type_Symbol token_AUX = Global.token;
                Global.token = Lexical_Analysis.Lexical_Token();
                Texp exp1 = Read_Expression(ref last1);
                exp1.prev = expLast;
                expLast.next = exp1;

                // compare بنية 
                Texp expaux = new Texp();
                expaux.token = token_AUX;
                expaux.prev = last1;
                last1.next = expaux;
                expaux.next = null;
                expLast = expaux;
            }
            return exp0;
        }
        public static Texp Read_Expression(ref Texp expLast)
        {
            Texp last1 = null;
            Texp exp0 = Read_Term(ref expLast);
            while (Global.token == Global.Type_Symbol.u_PLUS || Global.token == Global.Type_Symbol.u_MINUS)
            {
                Global.Type_Symbol token_AUX = Global.token;
                Global.token = Lexical_Analysis.Lexical_Token();
                Texp exp1 = Read_Term(ref last1);
                exp1.prev = expLast;
                expLast.next = exp1;

                // +,- بنية 
                Texp expaux = new Texp();
                expaux.token = token_AUX;
                expaux.prev = last1;
                last1.next = expaux;
                expaux.next = null;
                expLast = expaux;
            }
            return exp0;
        }
        public static Texp Read_Term(ref Texp expLast)
        {
            Texp last1 = null;
            Texp exp0 = Read_Fact(ref expLast);
            while (Global.token == Global.Type_Symbol.u_MULTI || Global.token == Global.Type_Symbol.u_DIV
                || Global.token == Global.Type_Symbol.u_MOD)
            {
                Global.Type_Symbol token_AUX = Global.token;
                Global.token = Lexical_Analysis.Lexical_Token();
                Texp exp1 = Read_Fact(ref last1);
                exp1.prev = expLast;
                expLast.next = exp1;

                // *./,% بنية 
                Texp expaux = new Texp();
                expaux.token = token_AUX;
                expaux.prev = last1;
                last1.next = expaux;
                expaux.next = null;
                expLast = expaux;
            }
            return exp0;
        }
        public static Texp Read_Fact(ref Texp expLast)
        {
            Texp exp = null;
            if( Global.token == Global.Type_Symbol.u_OPENP)
            {
                #region (cond)
                Global.token = Lexical_Analysis.Lexical_Token();
                exp = Read_Condition(ref expLast);
                if (Global.token != Global.Type_Symbol.u_CLOSEP)
                {
                    Global.Message_Wrong = Error.Get_Error(20) + "\t" + "\t" + Error.Get_Type_Error(3);
                    throw new Exception();
                }
                Global.token = Lexical_Analysis.Lexical_Token();
                return exp;
                #endregion
            }
            else if (Global.token == Global.Type_Symbol.u_PLUS || Global.token == Global.Type_Symbol.u_MINUS
                || Global.token == Global.Type_Symbol.u_NOT)
            {
                #region ++,--,!

                Texp expaux = new Texp();
                if (Global.token == Global.Type_Symbol.u_PLUS)
                {
                    expaux.token = Global.Type_Symbol.u_UNARY_PLUS;
                }
                else if (Global.token == Global.Type_Symbol.u_MINUS)
                {
                    expaux.token = Global.Type_Symbol.u_UNARY_MINUS;
                }
                else expaux.token = Global.token;
                Global.token = Lexical_Analysis.Lexical_Token();
                exp = Read_Fact(ref expLast);
                expLast.next = expaux;
                expaux.prev = expLast;
                expLast = expaux;
                return exp;

                #endregion
            }
            else if (Global.token == Global.Type_Symbol.u_VAR || Global.token == Global.Type_Symbol.u_UNKNOWN||
                Global.token == Global.Type_Symbol.u_PRE_DECR || Global.token == Global.Type_Symbol.u_PRE_INCR||
                    Global.token == Global.Type_Symbol.u_POST_DECR || Global.token == Global.Type_Symbol.u_POST_INCR
                )
            {
                #region variable
                exp = new Texp();
                exp.next = null;
                exp.prev = null;

                Global.Type_Symbol opToken = Global.token;
                if (Global.token == Global.Type_Symbol.u_VAR || Global.token == Global.Type_Symbol.u_UNKNOWN)
                exp.token = Global.Type_Symbol.u_VAR;

                else
                    exp.token = opToken;

                if (Global.token == Global.Type_Symbol.u_UNKNOWN)
                {
                    exp.Val_Var = Global.Add_Var(Global.G_Current_STR);
                }
                else
                    exp.Val_Var =(TVar) Global.G_Current_ID;

                Global.token = Lexical_Analysis.Lexical_Token();
                expLast = exp;
                return exp;
                #endregion
            }
            else if (Global.token == Global.Type_Symbol.u_CST_STR || Global.token == Global.Type_Symbol.u_CST_REAL ||
                Global.token == Global.Type_Symbol.u_CST_INT || Global.token == Global.Type_Symbol.u_TRUE ||
                Global.token == Global.Type_Symbol.u_FALSE)
            {
                #region constants
                exp = new Texp();
                exp.token = Global.token;
                exp.next = null;
                exp.prev = null;
                if(Global.token == Global.Type_Symbol.u_CST_STR)
                {
                    exp.Val_STR = Global.G_Current_STR;
                }
                else if (Global.token == Global.Type_Symbol.u_CST_REAL || Global.token == Global.Type_Symbol.u_CST_INT)
                {
                    exp.Val_NB = Global.G_Current_NB;
                }
                Global.token = Lexical_Analysis.Lexical_Token();
                expLast = exp;
                return exp;
                #endregion
            }
            else if (Global.token == Global.Type_Symbol.u_POWER || Global.token == Global.Type_Symbol.u_SIN || Global.token == Global.Type_Symbol.u_COS ||
             Global.token == Global.Type_Symbol.u_LN || Global.token == Global.Type_Symbol.u_LOG || Global.token == Global.Type_Symbol.u_ATAN ||
             Global.token == Global.Type_Symbol.u_SQRT || Global.token == Global.Type_Symbol.u_TAN || Global.token == Global.Type_Symbol.u_LENGTH
            || Global.token == Global.Type_Symbol.u_STRING_TO_INT || Global.token == Global.Type_Symbol.u_INT_TO_STRING
            || Global.token == Global.Type_Symbol.u_ABS) {

                #region func1 
                Global.Type_Symbol token_Aux = Global.token;
                Global.token = Lexical_Analysis.Lexical_Token();
                if( Global.token != Global.Type_Symbol.u_OPENP)
                {
                    Global.Message_Wrong = Error.Get_Error(17) + "\t" + "\t" + Error.Get_Type_Error(3);
                    throw new Exception();
                }
                Global.token = Lexical_Analysis.Lexical_Token();
                exp = Read_Expression(ref expLast);

                if (Global.token != Global.Type_Symbol.u_CLOSEP)
                {
                    Global.Message_Wrong = Error.Get_Error(20) + "\t" + "\t" + Error.Get_Type_Error(3);
                    throw new Exception();
                }
                Texp expaux = new Texp();
                expaux.token = token_Aux;
                expLast.next = expaux;
                expaux.prev = exp;
                expaux.next = null;
                Global.token = Lexical_Analysis.Lexical_Token();
                expLast = expaux;
                return exp;
                #endregion
            }
       
        else
            {
                Global.Message_Wrong = Error.Get_Error(27) + "\t" + "\t" + Error.Get_Type_Error(3);
                throw new Exception();
            }
        }
    }
}
