using DKBasic.Compiling_Steps.Execution_Tree;
using DKBasic.helper;
using DKBasic.structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasic.Compiling_Steps
{
    internal class Syntax_Analysis
    {
        public static void Compile_Main_Program(string File_Name)
        {
            Global.G_File = new TFile();
            Global.G_File.name = File_Name;
            Global.G_File.next = null;
            Global.G_Last_File = Global.G_File;
            Global.G_Current_File = Global.G_File;
            while (Global.G_Current_File != null)
            {
                try
                {
                    Compile_Current_File();
                }
                catch (Exception ex)
                {
                    Global.Compilation_Successful = false;
                }
                finally
                {
                    Global.G_Current_File_SR.Close();
                    Global.G_Current_File = Global.G_Current_File.next;
                }
            }
        }
        public static void Compile_Current_File()
        {
            Global.G_Current_File_SR = new StreamReader(Global.G_Current_File.name);
            if (!Lexical_Analysis.Read_New_Line()) return;
            if (!Lexical_Analysis.Skip_Spaces_And_Comments()) return;
            #region include
            while (Global.CL[Global.CI] == '#')
            {
                Global.CI++;
                if (!(Global.CI < Global.CL.Length && (Global.CL[Global.CI] == 'i' || Global.CL[Global.CI] == 'I')))
                {
                    Global.Message_Wrong = Error.Get_Error(10) + "\t" + "\t" + Error.Get_Type_Error(0);
                    throw new Exception();
                }
                Global.token = Lexical_Analysis.Lexical_Token();
                if (Global.token != Global.Type_Symbol.u_INCLUDE)
                {
                    Global.Message_Wrong = Error.Get_Error(11) + "\t" + "\t" + Error.Get_Type_Error(0);
                    throw new Exception();
                }
                int Line_Before_Include = Global.Current_Line_NB;
                Lexical_Analysis.Skip_Spaces();
                if (!(Global.CI < Global.CL.Length && Global.CL[Global.CI] == '\''))
                {
                    Global.Message_Wrong = Error.Get_Error(12) + "\t" + "\t" + Error.Get_Type_Error(0);
                    throw new Exception();
                }
                Global.token = Lexical_Analysis.Lexical_Token();
                if (Global.token != Global.Type_Symbol.u_CST_STR)
                {

                    Global.Message_Wrong = Error.Get_Error(12) + "\t" + "\t" + Error.Get_Type_Error(0);
                    throw new Exception();
                }
                int Line_After_Include = Global.Current_Line_NB;
                if (Line_Before_Include != Line_After_Include)
                {
                    Global.Message_Wrong = Error.Get_Error(13) + "\t" + "\t" + Error.Get_Type_Error(0);
                    throw new Exception();
                }
                Boolean Is_File_Exists = File.Exists(Global.G_Current_STR);
                if (!Is_File_Exists)
                {

                    Global.Message_Wrong = Error.Get_Error(0) + "\t" + "\t" + Error.Get_Type_Error(2);
                    throw new Exception();
                }
                else Global.Add_File(Global.G_Current_STR);
                Lexical_Analysis.Skip_Spaces();
                if (Global.CI < Global.CL.Length)
                {
                    Global.Message_Wrong = Error.Get_Error(14) + "\t" + "\t" + Error.Get_Type_Error(0);
                    throw new Exception();
                }
                if (!Lexical_Analysis.Read_New_Line()) return;
            }
            #endregion
            Global.token = Lexical_Analysis.Lexical_Token();
            #region Procedure
            while (Global.token == Global.Type_Symbol.u_PROCEDURE)
            {
                TVar G_Var_Temp = null;
                G_Var_Temp = Global.G_Var;
                Global.G_Var = null;
                TProcedure proc_Aux = null;
                TVar var_Aux = null;
                Global.token = Lexical_Analysis.Lexical_Token();
                if (Global.token == Global.Type_Symbol.u_UNKNOWN)
                {
                    proc_Aux = Global.Add_Procedure(Global.G_Current_STR);
                }
                else if (Global.token == Global.Type_Symbol.u_PROCEDURE)
                {
                    proc_Aux = (TProcedure)Global.G_Current_ID;
                    if (proc_Aux.Is_Define)
                    {

                        Global.Message_Wrong = Error.Get_Error(15) + "\t" + "\t" + Error.Get_Type_Error(3);
                        throw new Exception();
                    }
                }
                else
                {
                    Global.Message_Wrong = Error.Get_Error(16) + "\t" + "\t" + Error.Get_Type_Error(3);
                    throw new Exception();
                }
                proc_Aux.Is_Define = true;
                Global.token = Lexical_Analysis.Lexical_Token();
                if (Global.token != Global.Type_Symbol.u_OPENP)
                {
                    Global.Message_Wrong = Error.Get_Error(17) + "\t" + "\t" + Error.Get_Type_Error(3);
                    throw new Exception();
                }
                Global.token = Lexical_Analysis.Lexical_Token();
                while (!(Global.token == Global.Type_Symbol.u_CLOSEP || Global.token == Global.Type_Symbol.u_OUTPUT))
                {
                    if(Global.token == Global.Type_Symbol.u_VAR)
                    {
                        Global.Message_Wrong = Error.Get_Error(18) + "\t" + "\t" + Error.Get_Type_Error(3);
                        throw new Exception();
                    }
                    if (Global.token != Global.Type_Symbol.u_UNKNOWN)
                    {
                        Global.Message_Wrong = Error.Get_Error(18) + "\t" + "\t" + Error.Get_Type_Error(3);
                        throw new Exception();
                    }
                    var_Aux = Global.Add_Var(Global.G_Current_STR);
                    if(proc_Aux.Params_In1 == null)
                    {
                        proc_Aux.Params_In1 = var_Aux;
                    }
                    proc_Aux.Params_In2 = var_Aux;
                    Global.token = Lexical_Analysis.Lexical_Token();
                    if(Global.token == Global.Type_Symbol.u_COMMA)
                    {
                        Global.token = Lexical_Analysis.Lexical_Token();
                        if( (Global.token == Global.Type_Symbol.u_CLOSEP) || (Global.token == Global.Type_Symbol.u_OUTPUT))
                        {

                            Global.Message_Wrong = Error.Get_Error(19) + "\t" + "\t" + Error.Get_Type_Error(3);
                            throw new Exception();
                        }
                    
                    }
                    else
                    {
                        break;
                    }
                }
                if(Global.token == Global.Type_Symbol.u_OUTPUT)
                {
                    Global.token = Lexical_Analysis.Lexical_Token();
                    while (Global.token == Global.Type_Symbol.u_UNKNOWN)
                    {
                        var_Aux = Global.Add_Var(Global.G_Current_STR);
                        if (proc_Aux.Params_Out1 == null)
                        {
                            proc_Aux.Params_Out1 = var_Aux;
                        }
                        proc_Aux.Params_Out2 = var_Aux;
                        Global.token = Lexical_Analysis.Lexical_Token();
                        if (Global.token == Global.Type_Symbol.u_COMMA)
                        {
                            Global.token = Lexical_Analysis.Lexical_Token();
                        }
                        else
                        {
                            break;
                        }
                    }
                } 
                if( Global.token != Global.Type_Symbol.u_CLOSEP)
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
                proc_Aux.INS = build_Execution_Tree.Read_List_Of_Instruction();
                Global.G_Var = G_Var_Temp;
                G_Var_Temp = null;
                #endregion

            }

            #region Main

            if(Global.token  == Global.Type_Symbol.u_MAIN)
            {
                Global.G_Read_Main_Word = true;
                if( Global.G_Current_File != Global.G_File)
                {
                    Global.Message_Wrong = Error.Get_Error(22) + "\t" + "\t" + Error.Get_Type_Error(2);
                    throw new Exception();
                }
                Global.token = Lexical_Analysis.Lexical_Token();

               Global.G_Main_Instruction = build_Execution_Tree. Read_List_Of_Instruction();

                if (Global.token != Global.Type_Symbol.u_EOF)
                {

                    Global.Message_Wrong = Error.Get_Error(23) + "\t" + "\t" + Error.Get_Type_Error(2);
                    throw new Exception();
                }
            }
            #endregion
            if (Global.token != Global.Type_Symbol.u_EOF)
            {

                Global.Message_Wrong = Error.Get_Error(24) + "\t" + "\t" + Error.Get_Type_Error(2);
                throw new Exception();
            }
            if( Global.G_Current_File == Global.G_File && !Global.G_Read_Main_Word)
            {
                Global.Message_Wrong = Error.Get_Error(25) + "\t" + "\t" + Error.Get_Type_Error(2);
                throw new Exception();
            }
           
        }
    }
}
