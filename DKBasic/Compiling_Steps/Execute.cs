using DKBasic.Forms;
using DKBasic.helper;
using DKBasic.structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasic.Compiling_Steps
{
    internal class Execute
    {
        public static void Assign(TVar var , Texp exp)
        {
            var.token = exp.token;
            var.Val_NB = exp.Val_NB;
            var.Val_STR = exp.Val_STR;
        }

        public static Global.Type_Symbol Execute_List_Of_Instruction(Tinstruction Linst)
        {
         while(Linst != null)
            {
                if (Linst.INS is Tif)
                {
                    #region If
                    Tif If_Aux = Linst.INS as Tif;
                    if (Optimization.Exaluate_Cond(If_Aux.Cond))
                    {
                        Execute_List_Of_Instruction(If_Aux.Ins_If);
                        if(Global.token == Global.Type_Symbol.u_BREAK || Global.token == Global.Type_Symbol.u_CONTINUE)
                            return Global.token;
                    }
                    else if(If_Aux.Ins_Else != null )
                    {
                        Execute_List_Of_Instruction(If_Aux.Ins_Else);
                        if (Global.token == Global.Type_Symbol.u_BREAK || Global.token == Global.Type_Symbol.u_CONTINUE)
                            return Global.token;
                    }
                      
                    #endregion If
                }
                else if (Linst.INS is Twhile)
                {
                    #region While
                    Twhile While_Aux = Linst.INS as Twhile;
                    while (Optimization.Exaluate_Cond(While_Aux.Cond))
                    {
                        Execute_List_Of_Instruction(While_Aux.Inst);
                        if (Global.token == Global.Type_Symbol.u_BREAK)
                            break;
                    }

                    #endregion While
                }
                else if (Linst.INS is Tassign)
                {
                    #region Assign
                    Tassign Assign_Aux = Linst.INS as Tassign;
                    Texp exp0 = Optimization.Evaluate_Exp(Assign_Aux.exp);
                    Assign_Aux.Var.token = exp0.token;
                    Assign_Aux.Var.Val_NB = exp0.Val_NB;
                    Assign_Aux.Var.Val_STR = exp0.Val_STR;
                    Texp.Free(exp0);

                    #endregion Assign
                }

                else if (Linst.INS is TRead)
                {
                    #region Read
                    TRead Read_Aux = Linst.INS as TRead;
                    InputForm Input_Form = new InputForm();
                    string value_Form_Input = "";
                    TListVar lv = Read_Aux.Lv;
                    while (lv != null)
                    {
                        Input_Form.ShowDialog();
                        value_Form_Input = (Input_Form.textBox1.Text).Trim();
                        if (Global.Is_Input_Integer)
                        {
                            lv.Var.token = Global.Type_Symbol.u_CST_INT;
                            lv.Var.Val_NB = Convert.ToInt32(value_Form_Input);
                        }
                        else if (Global.Is_Input_Real)
                        {
                            lv.Var.token = Global.Type_Symbol.u_CST_REAL;
                            lv.Var.Val_NB = Convert.ToSingle(value_Form_Input);
                        }
                        else if (Global.Is_Input_String)
                        {
                            lv.Var.token = Global.Type_Symbol.u_CST_STR;
                            lv.Var.Val_STR = Convert.ToString(value_Form_Input);
                        }
                        else if (Global.Is_Input_Boolean)
                        {
                            lv.Var.token = value_Form_Input.ToLower().Equals("true") ?
                                Global.Type_Symbol.u_TRUE : Global.Type_Symbol.u_FALSE;
                        }
                        Input_Form.textBox1.Clear();
                        lv = lv.next;
                    }

                    #endregion Read
                }
                else if (Linst.INS is TWrite)
                {
                    #region Write/WriteLn
                    TWrite Write_Aux = Linst.INS as TWrite;
                    string write_In_Output = "";
                    TListExp lexp = Write_Aux.Lexp;

                    while (lexp != null)
                    {
                        Texp exp_Last = null;
                        exp_Last = lexp.exp;
                        Texp Exp = Optimization.Evaluate_Exp(exp_Last);
                        if (Exp.token == Global.Type_Symbol.u_CST_INT || Exp.token == Global.Type_Symbol.u_CST_REAL)
                        {
                            write_In_Output += Exp.Val_NB.ToString();
                        }
                        else if (Exp.token == Global.Type_Symbol.u_CST_STR)
                        {
                            write_In_Output += Exp.Val_STR;
                        }
                        else if (Exp.token == Global.Type_Symbol.u_TRUE)
                        {
                            write_In_Output += "True";
                        }
                        else if (Exp.token == Global.Type_Symbol.u_FALSE)
                        {
                            write_In_Output += "False";
                        }
                        else
                        {
                            write_In_Output += "Unknown";
                        }
                        Texp.Free(Exp);
                        lexp = lexp.next;
                    }
                    if (Write_Aux.Is_Ln)
                    {
                        write_In_Output += "\n";
                    }
                    Global.Output_Write_Form.richTextBox1.Text += write_In_Output.ToString();
                    Global.Output_Write_Form.Show();
                    #endregion Write/WriteLn
                }
                else if (Linst.INS is TCall)
                {
                    #region Call
                    TCall Call_Aux = Linst.INS as TCall;
                    Texp actualExp = Optimization.Evaluate_Exp(Call_Aux.ParamsIN);
                    TVar formalParam = null;
                    formalParam = Call_Aux.p.Params_In1;
                    while (actualExp!=null && formalParam !=null && formalParam != Call_Aux.p.Params_In2.next)
                    {
                        formalParam.token = actualExp.token;
                        formalParam.Val_NB = actualExp.Val_NB;
                        formalParam.Val_STR = actualExp.Val_STR;
                        actualExp = actualExp.next;
                        formalParam =(TVar) formalParam.next;
                    }
                    if(actualExp != null)
                    {
                        Global.Message_Wrong = Error.Get_Error(47) + "\t" + "\t" + Error.Get_Type_Error(2);
                        throw new Exception();
                    }
                    if (formalParam != null && formalParam != Call_Aux.p.Params_In2.next)
                    {
                        Global.Message_Wrong = Error.Get_Error(48) + "\t" + "\t" + Error.Get_Type_Error(2);
                        throw new Exception();
                    }
                    Texp.FreeList(actualExp);
                    formalParam = Call_Aux.p.Params_Out1;
                    TListVar actualOutList = Call_Aux.ParamsOut;
                    while (actualOutList != null && formalParam !=null && formalParam != Call_Aux.p.Params_Out2.next)
                    {
                        formalParam.token = actualOutList.Var.token;
                        formalParam.Val_NB = actualOutList.Var.Val_NB;
                        formalParam.Val_STR = actualOutList.Var.Val_STR;
                        formalParam = (TVar)formalParam.next;
                        actualOutList = actualOutList.next;
                    }
                    if (actualOutList != null)
                    {
                        Global.Message_Wrong = Error.Get_Error(47) + "\t" + "\t" + Error.Get_Type_Error(2);
                        throw new Exception();
                    }
                    if (formalParam != null)
                    {
                        Global.Message_Wrong = Error.Get_Error(48) + "\t" + "\t" + Error.Get_Type_Error(2);
                        throw new Exception();
                    }
                    Execute_List_Of_Instruction(Call_Aux.p.INS);
                    formalParam = Call_Aux.p.Params_Out1;
                    actualOutList = Call_Aux.ParamsOut;
                    while (actualOutList != null && formalParam !=null && formalParam != Call_Aux.p.Params_Out2.next)
                    {
                        actualOutList.Var.token = formalParam.token;
                        actualOutList.Var.Val_NB = formalParam.Val_NB;
                        actualOutList.Var.Val_STR = formalParam.Val_STR;
                        formalParam =(TVar) formalParam.next;
                        actualOutList = actualOutList.next;
                    }

                    #endregion Call
                }
                else if (Linst.INS is TSwitch)
                {
                    #region Switch
                    TSwitch Switch_Aux = Linst.INS as TSwitch;
                    Boolean execute_Default_Instruction = true;
                    Tvalue value_Aux = Switch_Aux.values;
                    while(value_Aux != null)
                    {
                        Texp evaluateExp = Optimization.Evaluate_Exp(value_Aux.exp);
                        Boolean execute_Exp_Instruction = false;
                        if(Switch_Aux.var.token == evaluateExp.token)
                        {
                            execute_Exp_Instruction = true;
                            if( (evaluateExp.token == Global.Type_Symbol.u_CST_INT || evaluateExp.token == Global.Type_Symbol.u_CST_REAL)
                                
                               && evaluateExp.Val_NB != Switch_Aux.var.Val_NB)
                                execute_Exp_Instruction = false;
                            if (evaluateExp.token == Global.Type_Symbol.u_CST_STR  && evaluateExp.Val_STR != Switch_Aux.var.Val_STR)
                                execute_Exp_Instruction = false;
                        }
                        Texp.FreeList(evaluateExp);
                        if(execute_Exp_Instruction)
                        {
                            execute_Default_Instruction = false;
                            Execute_List_Of_Instruction(value_Aux.instruction);
                        }
                        value_Aux = value_Aux.next;
                    }
                    if (execute_Default_Instruction)
                    {
                        Execute_List_Of_Instruction(Switch_Aux.Deafult_Instruction);
                    }


                    #endregion Switch
                }
                else if (Linst.INS is TPost_Pre_Var)
                {
                    #region TPost_Pre_Var
                    TPost_Pre_Var post_Pre_Var_Aux = Linst.INS as TPost_Pre_Var;
                  if(post_Pre_Var_Aux.Var.token != Global.Type_Symbol.u_CST_INT && post_Pre_Var_Aux.Var.token != Global.Type_Symbol.u_CST_REAL)
                    {
                        Global.Message_Wrong = Error.Get_Error(36) + "\t" + "\t" + Error.Get_Type_Error(2);
                        throw new Exception();
                    }
                    if (post_Pre_Var_Aux.isIncrement)
                        post_Pre_Var_Aux.Var.Val_NB += 1;
                    else
                        post_Pre_Var_Aux.Var.Val_NB -= 1;
                    #endregion TPost_Pre_Var
                }
                else if (Linst.INS is TFor)
                {
                    #region TFor
                    TFor For_Aux = Linst.INS as TFor;

                    Texp Exp_B = Optimization.Evaluate_Exp(For_Aux.Exp_Begin);
                    Texp Exp_E = Optimization.Evaluate_Exp(For_Aux.Exp_End);
                    Texp Exp_S = null;
                    if(For_Aux.Exp_Step != null )
                        Exp_S = Optimization.Evaluate_Exp(For_Aux.Exp_Step);
                    else
                    {
                        Exp_S = new Texp();
                        Exp_S.Val_NB = 1;
                        Exp_S.token = Global.Type_Symbol.u_CST_INT;
                    }
                    Assign(For_Aux.var, Exp_B);
                    if (!For_Aux.Is_Down)
                    {
                        //up
                        while (Exp_B.Val_NB <= Exp_E.Val_NB)
                        {
                            Execute_List_Of_Instruction(For_Aux.L_inst);
                            if (Global.token == Global.Type_Symbol.u_BREAK )
                            {
                                break;
                            }
                            if(Global.token == Global.Type_Symbol.u_CONTINUE)
                            {
                                Exp_B.Val_NB += Exp_S.Val_NB;
                                Assign(For_Aux.var, Exp_B);
                                continue;
                            }
                                Exp_B.Val_NB += Exp_S.Val_NB;
                            Assign(For_Aux.var, Exp_B);
                        }
                    }
                    else
                    {
                        //down
                        while (Exp_B.Val_NB >= Exp_E.Val_NB)
                        {
                            Execute_List_Of_Instruction(For_Aux.L_inst);
                            if (Global.token == Global.Type_Symbol.u_BREAK)
                            {
                                break;
                            }
                            if (Global.token == Global.Type_Symbol.u_CONTINUE)
                            {
                                Exp_B.Val_NB -= Exp_S.Val_NB;
                                Assign(For_Aux.var, Exp_B);
                                continue;
                            }
                            Exp_B.Val_NB -= Exp_S.Val_NB;
                            Assign(For_Aux.var, Exp_B);
                        }
                    }

                    Texp.Free(Exp_B);
                    Texp.Free(Exp_E);
                    Texp.Free(Exp_S);

                    #endregion TFor
                }
                //TBreak_Continue
                else if (Linst.INS is TBreak_Continue)
                {
                    #region TBreak_Continue
                    TBreak_Continue TBreak_Continue_Aux = Linst.INS as TBreak_Continue;
                    Global.token = TBreak_Continue_Aux.token;
                    return Global.token;

                    #endregion TBreak_Continue
                }
                Linst = Linst.next;
            }
            return Global.Type_Symbol.u_NORMAL;
        }
    }
}
