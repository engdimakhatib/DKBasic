using DKBasic.helper;
using DKBasic.structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasic.Compiling_Steps.Execution_Tree
{
    internal class build_Execution_Tree
    {

        public static Tinstruction Read_List_Of_Instruction()
        {
            if( Global.token != Global.Type_Symbol.u_BEGIN)
            {
                Global.Message_Wrong = Error.Get_Error(26) + "\t" + "\t" + Error.Get_Type_Error(3);
                throw new Exception();
            }
            Global.token = Lexical_Analysis.Lexical_Token();
            Tinstruction First_Ins = null;
            Tinstruction Last_Ins = null;
            while(Global.token != Global.Type_Symbol.u_END && Global.token != Global.Type_Symbol.u_EOF)  
            {
                Tinstruction New_Ins = new Tinstruction();
                New_Ins.INS = Read_One_Instruction();
                New_Ins.next = null;
                if(First_Ins == null )
                    First_Ins = New_Ins;
                else Last_Ins.next= New_Ins;
                Last_Ins = New_Ins;
            }

            if (Global.token != Global.Type_Symbol.u_END)
            {
                Global.Message_Wrong = Error.Get_Error(31) + "\t" + "\t" + Error.Get_Type_Error(3);
                throw new Exception();
            }
            Global.token= Lexical_Analysis.Lexical_Token();
            return First_Ins;
        }
        public static Tinstruction Read_One_Or_List_Of_Instruction()
        {
            Global.token = Lexical_Analysis.Lexical_Token();
            if( Global.token == Global.Type_Symbol.u_BEGIN)
            {
              return  Read_List_Of_Instruction();
            }
            Tinstruction New_Ins = new Tinstruction();
            New_Ins.INS = Read_One_Instruction();
            New_Ins.next = null;
            return New_Ins;
        }

        public static object Read_One_Instruction()
        {
            if( Global.token == Global.Type_Symbol.u_IF)
            {
                #region IF
                Tif IF_Aux = new Tif();
                Global.token = Lexical_Analysis.Lexical_Token();
                Texp last = null;
                IF_Aux.Cond = build_AST_Tree.Read_Condition(ref last);
                if(Global.token != Global.Type_Symbol.u_THEN)
                {

                    Global.Message_Wrong = Error.Get_Error(28) + "\t" + "\t" + Error.Get_Type_Error(3);
                    throw new Exception();
                }
                IF_Aux.Ins_If = Read_One_Or_List_Of_Instruction();
                IF_Aux.Ins_Else = null;
                if( Global.token == Global.Type_Symbol.u_ELSE)
                    IF_Aux.Ins_Else = Read_One_Or_List_Of_Instruction();
                return IF_Aux;
                #endregion
            }
            else if (Global.token == Global.Type_Symbol.u_VAR || Global.token == Global.Type_Symbol.u_UNKNOWN)
            {
                #region ASSIGN
                Tassign Assign_Aux = new Tassign();
                TVar varaux = null;
                if (Global.token == Global.Type_Symbol.u_UNKNOWN)
                {
                 varaux =   Global.Add_Var(Global.G_Current_STR);
                }
                else
                {
                    varaux =  (TVar)Global.G_Current_ID;
                }
                Assign_Aux.Var = varaux;
                Global.token = Lexical_Analysis.Lexical_Token();
                if (Global.token != Global.Type_Symbol.u_ASSIGN)
                {

                    Global.Message_Wrong = Error.Get_Error(29) + "\t" + "\t" + Error.Get_Type_Error(3);
                    throw new Exception();
                }
                Global.token = Lexical_Analysis.Lexical_Token();
                Texp last = null;
               Assign_Aux.exp =  build_AST_Tree.Read_Expression(ref last);
                if (Global.token != Global.Type_Symbol.u_SEMICOLON)
                {

                    Global.Message_Wrong = Error.Get_Error(21) + "\t" + "\t" + Error.Get_Type_Error(3);
                    throw new Exception();
                }
                Global.token = Lexical_Analysis.Lexical_Token();
                return Assign_Aux;
                #endregion
            }
            else if (Global.token == Global.Type_Symbol.u_WHILE)
            {
                #region WHILE
                Twhile While_Aux = new Twhile();
                Global.token = Lexical_Analysis.Lexical_Token();
                Texp last = null;
              While_Aux.Cond =  build_AST_Tree.Read_Condition(ref last);
                if (Global.token != Global.Type_Symbol.u_DO)
                {

                    Global.Message_Wrong = Error.Get_Error(30) + "\t" + "\t" + Error.Get_Type_Error(3);
                    throw new Exception();
                }
              While_Aux.Inst =  Read_One_Or_List_Of_Instruction();
                return While_Aux;
                #endregion
            }
            else if (Global.token == Global.Type_Symbol.u_READ)
            {
                #region READ
                TRead Read_Aux = new TRead();
                TListVar L_Var_New = null;
                TListVar L_Var_Last = null;
                Global.token = Lexical_Analysis.Lexical_Token();
                if (Global.token != Global.Type_Symbol.u_OPENP)
                {

                    Global.Message_Wrong = Error.Get_Error(17) + "\t" + "\t" + Error.Get_Type_Error(3);
                    throw new Exception();
                }
                while (true)
                {
                    L_Var_New = new TListVar();
                    Global.token = Lexical_Analysis.Lexical_Token();
                    if (Global.token == Global.Type_Symbol.u_UNKNOWN)
                    {
                        L_Var_New.Var = Global.Add_Var(Global.G_Current_STR);
                    }
                    else if (Global.token == Global.Type_Symbol.u_VAR)
                    {
                        L_Var_New.Var = (TVar)Global.G_Current_ID;
                    }
                    else {
                        Global.Message_Wrong = Error.Get_Error(32) + "\t" + "\t" + Error.Get_Type_Error(3);
                        throw new Exception();
                    }
                    if(Read_Aux.Lv == null )
                    {
                        Read_Aux.Lv = L_Var_New;
                    }
                    else
                    {
                        L_Var_Last.next = L_Var_New;
                    }
                    L_Var_Last = L_Var_New;
                    Global.token = Lexical_Analysis.Lexical_Token();
                    if (Global.token == Global.Type_Symbol.u_CLOSEP)
                    {
                        Global.token = Lexical_Analysis.Lexical_Token();
                        break;
                    }
                    if (Global.token != Global.Type_Symbol.u_COMMA && Global.token != Global.Type_Symbol.u_CLOSEP)
                    {
                        Global.Message_Wrong = Error.Get_Error(33) + "\t" + "\t" + Error.Get_Type_Error(3);
                        throw new Exception();
                    }
                }
                if (Global.token != Global.Type_Symbol.u_SEMICOLON)
                {
                    Global.Message_Wrong = Error.Get_Error(21) + "\t" + "\t" + Error.Get_Type_Error(3);
                    throw new Exception();
                }
                Global.token = Lexical_Analysis.Lexical_Token();
                return Read_Aux;
                #endregion
            }
            else if (Global.token == Global.Type_Symbol.u_WRITE || Global.token == Global.Type_Symbol.u_WRITELN)
            {
                #region WRITE/WRITELN
                TWrite Write_Aux = new TWrite();
                Texp last = null;
                Write_Aux.Lexp = null;
                TListExp lexplast = null;
                Write_Aux.Is_Ln = (Global.token == Global.Type_Symbol.u_WRITELN);
                Global.token = Lexical_Analysis.Lexical_Token();
                if (Global.token != Global.Type_Symbol.u_OPENP)
                {
                    Global.Message_Wrong = Error.Get_Error(17) + "\t" + "\t" + Error.Get_Type_Error(3);
                    throw new Exception();
                }
                while (true)
                {
                    Global.token = Lexical_Analysis.Lexical_Token();
                    if((Global.token == Global.Type_Symbol.u_CLOSEP) && (Write_Aux.Is_Ln))
                    {
                        break;
                    }
                    TListExp lexpnew = new TListExp();
                    lexpnew.exp = build_AST_Tree.Read_Expression(ref last);
                    if (Write_Aux.Lexp == null)
                    {
                        Write_Aux.Lexp = lexpnew;
                    }
                    else {
                    lexplast.next = lexpnew;
                    }
                    lexplast = lexpnew;
                    if (Global.token == Global.Type_Symbol.u_CLOSEP)
                        break;
                    if (Global.token != Global.Type_Symbol.u_COMMA)
                    {
                        Global.Message_Wrong = Error.Get_Error(34) + "\t" + "\t" + Error.Get_Type_Error(3);
                        throw new Exception();
                    }
                }
                Global.token = Lexical_Analysis.Lexical_Token();
                if (Global.token != Global.Type_Symbol.u_SEMICOLON)
                {
                    Global.Message_Wrong = Error.Get_Error(21) + "\t" + "\t" + Error.Get_Type_Error(3);
                    throw new Exception();
                }
                Global.token = Lexical_Analysis.Lexical_Token();
                return Write_Aux;
                #endregion
            }
            else if (Global.token == Global.Type_Symbol.u_CALL)
            {
                #region CALL
                TCall Call_Aux = new TCall();
                Global.token = Lexical_Analysis.Lexical_Token();
                if( Global.token == Global.Type_Symbol.u_UNKNOWN)
                {
                  Call_Aux.p =  Global.Add_Procedure(Global.G_Current_STR);
                    Call_Aux.p.Is_Define = false;
                }
                else if (Global.token == Global.Type_Symbol.u_PROCEDURE)
                {
                    Call_Aux.p = (TProcedure) Global.G_Current_ID;
                }
                else
                {
                    Global.Message_Wrong = Error.Get_Error(16) + "\t" + "\t" + Error.Get_Type_Error(3);
                    throw new Exception();
                }
                Global.token = Lexical_Analysis.Lexical_Token();
                if (Global.token != Global.Type_Symbol.u_OPENP)
                {

                    Global.Message_Wrong = Error.Get_Error(17) + "\t" + "\t" + Error.Get_Type_Error(3);
                    throw new Exception();
                }
                Global.token = Lexical_Analysis.Lexical_Token();
                Call_Aux.ParamsIN = null;
                Call_Aux.ParamsOut = null;
                Texp expLast = null;
                Texp exp0 = null;
                Texp last_Aux = null;
                TListVar last_L_Var = null;
                while (!( Global.token == Global.Type_Symbol.u_OUTPUT || Global.token == Global.Type_Symbol.u_CLOSEP))
                {
                    exp0 = build_AST_Tree.Read_Condition(ref expLast);
                    if(Call_Aux.ParamsIN == null )
                    {
                        Call_Aux.ParamsIN = exp0;
                    }
                    else
                    {
                        last_Aux.next = exp0;
                        exp0.prev = last_Aux;
                    }
                    last_Aux = expLast;
                    if (Global.token == Global.Type_Symbol.u_COMMA)
                    {
                        Global.token = Lexical_Analysis.Lexical_Token();
                        if (Global.token == Global.Type_Symbol.u_OUTPUT || Global.token == Global.Type_Symbol.u_CLOSEP)
                        {
                            Global.Message_Wrong = Error.Get_Error(19) + "\t" + "\t" + Error.Get_Type_Error(3);
                            throw new Exception();
                        }

                    }
                    else break;
                }
                if( Global.token == Global.Type_Symbol.u_OUTPUT)
                {
                    while (true)
                    {
                        Global.token = Lexical_Analysis.Lexical_Token();
                        TListVar new_L_Var = new TListVar();
                        new_L_Var.next = null;
                        if(Global.token == Global.Type_Symbol.u_UNKNOWN)
                        {
                         new_L_Var.Var =   Global.Add_Var(Global.G_Current_STR);
                        }
                        else if (Global.token == Global.Type_Symbol.u_VAR)
                        {
                            new_L_Var.Var = (TVar ) Global.G_Current_ID;
                        }
                        else
                        {

                            Global.Message_Wrong = Error.Get_Error(35) + "\t" + "\t" + Error.Get_Type_Error(3);
                            throw new Exception();
                        }
                        if(Call_Aux.ParamsOut == null)
                        {
                            Call_Aux.ParamsOut = new_L_Var;
                        }
                        else
                        {
                            last_L_Var.next = new_L_Var;
                        }
                        last_L_Var = new_L_Var;
                        Global.token = Lexical_Analysis.Lexical_Token();
                        if (Global.token != Global.Type_Symbol.u_COMMA)
                            break;
                    }
                }              
                if ( Global.token != Global.Type_Symbol.u_CLOSEP)
                {
                    Global.Message_Wrong = Error.Get_Error(20) + "\t" + "\t" + Error.Get_Type_Error(3);
                    throw new Exception();
                }
                Global.token = Lexical_Analysis.Lexical_Token();
                if (Global.token != Global.Type_Symbol.u_SEMICOLON)
                {
                    Global.Message_Wrong = Error.Get_Error(21) + "\t" + "\t" + Error.Get_Type_Error(3);
                    throw new Exception();
                }
                Global.token = Lexical_Analysis.Lexical_Token();
                return Call_Aux;
                #endregion
            }
            else if (Global.token == Global.Type_Symbol.u_SWITCH)
            {
                #region Switch
                TSwitch Switch_Aux = new TSwitch();
                Texp expLast = null;
                Tvalue last_Value = null;
                Global.token = Lexical_Analysis.Lexical_Token();
                if (Global.token == Global.Type_Symbol.u_UNKNOWN)
                {
                    Switch_Aux.var = Global.Add_Var(Global.G_Current_STR);
                }
                else if (Global.token == Global.Type_Symbol.u_VAR)
                {
                    Switch_Aux.var  = (TVar)Global.G_Current_ID;
                }
                else
                {
                    Global.Message_Wrong = Error.Get_Error(35) + "\t" + "\t" + Error.Get_Type_Error(3);
                    throw new Exception();
                }
               
                Global.token = Lexical_Analysis.Lexical_Token();
                if (Global.token != Global.Type_Symbol.u_OF)
                {

                    Global.Message_Wrong = Error.Get_Error(49) + "\t" + "\t" + Error.Get_Type_Error(3);
                    throw new Exception();
                }
                Global.token = Lexical_Analysis.Lexical_Token();
                if (Global.token != Global.Type_Symbol.u_2pts)
                {

                    Global.Message_Wrong = Error.Get_Error(50) + "\t" + "\t" + Error.Get_Type_Error(3);
                    throw new Exception();
                }
                Global.token = Lexical_Analysis.Lexical_Token();
                if (Global.token != Global.Type_Symbol.u_BEGIN)
                {

                    Global.Message_Wrong = Error.Get_Error(26) + "\t" + "\t" + Error.Get_Type_Error(3);
                    throw new Exception();
                }
                Global.token = Lexical_Analysis.Lexical_Token();
                while (Global.token != Global.Type_Symbol.u_EOF && Global.token != Global.Type_Symbol.u_DEFAULT &&
                    Global.token != Global.Type_Symbol.u_END
                    )
                {
                    Tvalue val_Aux = new Tvalue();
                    val_Aux.exp = build_AST_Tree.Read_Expression(ref expLast);
                    if (Global.token != Global.Type_Symbol.u_2pts)
                    {

                        Global.Message_Wrong = Error.Get_Error(50) + "\t" + "\t" + Error.Get_Type_Error(3);
                        throw new Exception();
                    }
                    if (Switch_Aux.values == null)
                        Switch_Aux.values = val_Aux;
                    else last_Value.next = val_Aux;
                    last_Value = val_Aux;
                    val_Aux.instruction = Read_One_Or_List_Of_Instruction();
                }
                if (Global.token == Global.Type_Symbol.u_DEFAULT)
                {
                    Global.token = Lexical_Analysis.Lexical_Token();
                    if (Global.token != Global.Type_Symbol.u_2pts)
                    {

                        Global.Message_Wrong = Error.Get_Error(50) + "\t" + "\t" + Error.Get_Type_Error(3);
                        throw new Exception();
                    }
                    Switch_Aux.Deafult_Instruction = Read_One_Or_List_Of_Instruction();
                }

                if (Global.token != Global.Type_Symbol.u_END)
                {

                    Global.Message_Wrong = Error.Get_Error(31) + "\t" + "\t" + Error.Get_Type_Error(3);
                    throw new Exception();
                }

                Global.token = Lexical_Analysis.Lexical_Token();
                return Switch_Aux;
                #endregion
            }
            else if (Global.token == Global.Type_Symbol.u_POST_DECR || Global.token == Global.Type_Symbol.u_POST_INCR
               || Global.token == Global.Type_Symbol.u_PRE_DECR || Global.token == Global.Type_Symbol.u_PRE_INCR
                )
            {
                #region PostPreVar
                TPost_Pre_Var post_Pre_Var_Aux = new TPost_Pre_Var();
                post_Pre_Var_Aux.Var = (TVar) Global.G_Current_ID;
                post_Pre_Var_Aux.isIncrement = Global.token == Global.Type_Symbol.u_POST_INCR || Global.token == Global.Type_Symbol.u_PRE_INCR;
                Global.token = Lexical_Analysis.Lexical_Token();
                if (Global.token != Global.Type_Symbol.u_SEMICOLON)
                {

                    Global.Message_Wrong = Error.Get_Error(21) + "\t" + "\t" + Error.Get_Type_Error(3);
                    throw new Exception();
                }

                Global.token = Lexical_Analysis.Lexical_Token();
                return post_Pre_Var_Aux;
                #endregion
            }
            else if (Global.token == Global.Type_Symbol.u_FOR  )
            {
                #region For
                TFor For_Aux = new TFor();
                Global.token = Lexical_Analysis.Lexical_Token();
                if (Global.token == Global.Type_Symbol.u_UNKNOWN)
                    Global.Add_Var(Global.G_Current_STR);
                else if(Global.token != Global.Type_Symbol.u_VAR)
                {
                    Global.Message_Wrong = Error.Get_Error(32) + "\t" + "\t" + Error.Get_Type_Error(3);
                    throw new Exception();
                }
                For_Aux.var = (TVar) Global.G_Current_ID;
                Global.token = Lexical_Analysis.Lexical_Token();
                if (Global.token != Global.Type_Symbol.u_ASSIGN)
                {
                    Global.Message_Wrong = Error.Get_Error(29) + "\t" + "\t" + Error.Get_Type_Error(3);
                    throw new Exception();
                }
                Global.token = Lexical_Analysis.Lexical_Token();
                Texp last1 = null;
                For_Aux.Exp_Begin =  build_AST_Tree.Read_Expression(ref last1);

                if (Global.token != Global.Type_Symbol.u_TO  && Global.token != Global.Type_Symbol.u_DOWN_TO )
                {
                    Global.Message_Wrong = Error.Get_Error(53) + "\t" + "\t" + Error.Get_Type_Error(3);
                    throw new Exception();
                }
                For_Aux.Is_Down = (Global.token == Global.Type_Symbol.u_DOWN_TO);
                Global.token = Lexical_Analysis.Lexical_Token();
                Texp last2 = null;
                For_Aux.Exp_End = build_AST_Tree.Read_Expression(ref last2);
                if (Global.token == Global.Type_Symbol.u_STEP)
                {
                    Global.token = Lexical_Analysis.Lexical_Token();
                    Texp last3 = null;
                    For_Aux.Exp_Step = build_AST_Tree.Read_Expression(ref last3);
                }
                if (Global.token != Global.Type_Symbol.u_DO )
                {
                    Global.Message_Wrong = Error.Get_Error(30) + "\t" + "\t" + Error.Get_Type_Error(3);
                    throw new Exception();
                }
                For_Aux.L_inst = Read_One_Or_List_Of_Instruction();

                return For_Aux;
                #endregion
            }
            else if (Global.token == Global.Type_Symbol.u_BREAK || Global.token == Global.Type_Symbol.u_CONTINUE)
            {
                #region TBreak_Continue
                TBreak_Continue TBreak_Continue_Aux = new TBreak_Continue();
                TBreak_Continue_Aux.token = Global.token;
                Global.token = Lexical_Analysis.Lexical_Token();

                if (Global.token != Global.Type_Symbol.u_SEMICOLON)
                {
                    Global.Message_Wrong = Error.Get_Error(21) + "\t" + "\t" + Error.Get_Type_Error(3);
                    throw new Exception();
                }
                Global.token = Lexical_Analysis.Lexical_Token();

                return TBreak_Continue_Aux;
                #endregion
            }
            else
            {
                Global.Message_Wrong = Error.Get_Error(24) + "\t" + "\t" + Error.Get_Type_Error(3);
                throw new Exception();
            }
        }
    }
}
