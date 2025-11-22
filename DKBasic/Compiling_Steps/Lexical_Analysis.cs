using DKBasic.helper;
using DKBasic.structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasic.Compiling_Steps
{
    internal class Lexical_Analysis
    {
        public static Boolean Read_New_Line()
        {
            while (!Global.G_Current_File_SR.EndOfStream)
            {
                Global.CL = Global.G_Current_File_SR.ReadLine().Trim();
                Global.Current_Line_NB++;
                if (Global.CL.Length > 0)
                {
                    Global.CI = 0;
                    return true;
                }
            }
            return false;
        }
        public static Boolean Skip_Spaces_And_Comments()
        {
            while (true)
            {
                while (Global.CI < Global.CL.Length && Global.CL[Global.CI] == ' ')
                {
                    Global.CI++;
                }
                if (Global.CI == Global.CL.Length)
                    if (!Read_New_Line())
                        return false;
                if (Global.CI + 1 < Global.CL.Length && Global.CL[Global.CI] == '/' && Global.CL[Global.CI + 1] == '/')
                {
                    if (!Read_New_Line())
                        return false;
                }
                if (Global.CI + 1 < Global.CL.Length && Global.CL[Global.CI] == '/' && Global.CL[Global.CI + 1] == '*')
                {
                    Global.CI += 2;
                    while (true)
                    {
                        while (Global.CI + 1 < Global.CL.Length && !(Global.CL[Global.CI] == '*' && Global.CL[Global.CI + 1] == '/'))
                        {
                            Global.CI++;
                        }
                        if (Global.CI == Global.CL.Length || Global.CI + 1 == Global.CL.Length)
                        {
                            if (!Read_New_Line())
                            {
                                Global.Message_Wrong = Error.Get_Error(1) + "\t" + "\t" + Error.Get_Type_Error(1);
                                throw new Exception();
                            }
                        }
                        else
                        {
                            Global.CI += 2;
                            break;
                        }
                    }
                }
                else return true;
            }
        }
        public static void Skip_Spaces()
        {
            while (Global.CI < Global.CL.Length && Global.CL[Global.CI] == ' ')
            {
                Global.CI++;
            }
        }
        public static Global.Type_Symbol Lexical_Token()
        {
            if (!Skip_Spaces_And_Comments())
            {
                return Global.Type_Symbol.u_EOF;
            }
            if (Char.IsDigit(Global.CL[Global.CI]) || Global.CL[Global.CI] == '.')
            {
                #region number
                Boolean Is_Correct = false;
                Boolean Is_Real = false;
                Boolean Single_Point = true;
                Boolean Is_Negative = false;
                Double double_Part = 0;
                Double p = 10;
                Global.G_Current_NB = 0;
                Global.G_Buffer = "";


                while (Global.CI < Global.CL.Length && Char.IsDigit(Global.CL[Global.CI]))
                {
                    Single_Point = false;
                    Global.G_Current_NB = Global.G_Current_NB * 10 + Convert.ToInt32(Global.CL[Global.CI]) - Convert.ToInt32('0');
                    Global.CI++;
                }
                if (Global.CI < Global.CL.Length && Global.CL[Global.CI] == '.')
                {
                    Is_Real = true;
                    Global.CI++;
                    while (Global.CI < Global.CL.Length && Char.IsDigit(Global.CL[Global.CI]))
                    {
                        Single_Point = false;
                        double_Part = double_Part + (Convert.ToInt32(Global.CL[Global.CI]) - Convert.ToInt32('0')) / p;
                        p *= 10;
                        Global.CI++;
                    }
                    Global.G_Current_NB += double_Part;
                }
                if (Single_Point)
                {
                    Global.G_Buffer = Global.G_Current_NB.ToString();
                    Global.Message_Wrong = Error.Get_Error(2) + "\t" + "\t" + Error.Get_Type_Error(1);
                    throw new Exception();
                }
                if (Global.CI < Global.CL.Length && (Global.CL[Global.CI] == 'e' || Global.CL[Global.CI] == 'E'))
                {
                    Is_Real = true;
                    Global.CI++;
                    if (Global.CI < Global.CL.Length && (Global.CL[Global.CI] == '+' || Global.CL[Global.CI] == '-'))
                    {
                        if (Global.CL[Global.CI] == '-')
                        {
                            Is_Negative = true;
                        }
                        Global.CI++;
                        if (Global.CI < Global.CL.Length && Char.IsDigit(Global.CL[Global.CI]))
                        {
                            double_Part = 0;
                            Is_Correct = true;
                            while (Global.CI < Global.CL.Length && Char.IsDigit(Global.CL[Global.CI]))
                            {

                                double_Part = double_Part * 10 + Convert.ToInt32(Global.CL[Global.CI]) - Convert.ToInt32('0');
                                Global.CI++;
                            }
                            if (Is_Negative)
                            {
                                double_Part = -double_Part;
                            }
                            Global.G_Current_NB *= Math.Pow(10, double_Part);
                        }
                        else
                        {
                            Global.G_Buffer = Global.G_Current_NB.ToString();
                            Global.Message_Wrong = Error.Get_Error(4) + "\t" + "\t" + Error.Get_Type_Error(1);
                            throw new Exception();
                        }

                    }
                    else
                    {
                        Global.G_Buffer = Global.G_Current_NB.ToString();
                        Global.Message_Wrong = Error.Get_Error(3) + "\t" + "\t" + Error.Get_Type_Error(1);
                        throw new Exception();
                    }
                }
                Global.G_Buffer = Global.G_Current_NB.ToString();
                return Is_Real ? Global.Type_Symbol.u_CST_REAL : Global.Type_Symbol.u_CST_INT;
            }
            #endregion


            else if (Global.CL[Global.CI] == '\'')
            {
                #region string
                Global.CI++;
                Global.G_Current_STR = "";
                while ((Global.CI < Global.CL.Length && Global.CL[Global.CI] != '\'')
                    || (Global.CI < Global.CL.Length - 1) && (Global.CL[Global.CI] == '\'') && (Global.CL[Global.CI + 1] == '\''))
                {
                    Global.G_Current_STR += Global.CL[Global.CI];
                    if (Global.CL[Global.CI] == '\'') Global.CI++;
                    Global.CI++;
                    if (Global.CI >= Global.CL.Length)
                    {
                        Global.Message_Wrong = Error.Get_Error(5) + "\t" + Error.Get_Type_Error(1);
                        throw new Exception();
                    }
                }
                Global.CI++;
                Global.G_Buffer = Global.G_Current_STR;
                return Global.Type_Symbol.u_CST_STR;
            }

            #endregion

            #region preIncrement
            else if(Global.CL[Global.CI] == '+')
            {
                Global.CI++;
                if(Global.CL[Global.CI] =='+' && Char.IsLetter(Global.CL[Global.CI + 1]))
                {
                    Global.CI++;
                    if( Global.CI < Global.CL.Length && (Char.IsLetter(Global.CL[Global.CI ]) || Global.CL[Global.CI] == '_'))
                    {
                        Global.G_Buffer = "";
                        while (Global.CI < Global.CL.Length && (Char.IsLetter(Global.CL[Global.CI ])
                            || Global.CL[Global.CI] == '_'
                            ||Char.IsDigit( Global.CL[Global.CI] )
                            ))
                        {
                            Global.G_Buffer += Global.CL[Global.CI];
                            Global.CI++;
                        }
                        TSymbol Temp;
                        Temp = Global.Find_Symbol(Global.G_Buffer);
                        if(Temp != null)
                        {

                            Global.Message_Wrong = Error.Get_Error(51) + "\t" + Error.Get_Type_Error(1);
                            throw new Exception();
                        }
                        TIdentifier GID;
                        GID = Global.Find_Identifier(Global.G_Buffer, Global.G_Procedure);
                        if (GID != null)
                        {

                            Global.Message_Wrong = Error.Get_Error(51) + "\t" + Error.Get_Type_Error(1);
                            throw new Exception();
                        }
                        else
                        {
                            GID = Global.Find_Identifier(Global.G_Buffer, Global.G_Var);
                            if (GID == null)
                            {
                                Global.G_Current_STR = Global.G_Buffer;
                                Global.Add_Var(Global.G_Buffer);
                            }
                            else
                            {
                                Global.G_Current_STR = Global.G_Buffer;
                                Global.G_Current_ID = GID;
                            }
                            return Global.Type_Symbol.u_PRE_INCR;
                        }
                    }
                }
                else
                {
                    return Global.Type_Symbol.u_PLUS;
                }
            }
            #endregion 

            #region preDecrement
            else if (Global.CL[Global.CI] == '-')
            {
                Global.CI++;
                if (Global.CL[Global.CI] == '-' && Char.IsLetter(Global.CL[Global.CI + 1]))
                {
                    Global.CI++;
                    if (Global.CI < Global.CL.Length && (Char.IsLetter(Global.CL[Global.CI]) || Global.CL[Global.CI] == '_'))
                    {
                        Global.G_Buffer = "";
                        while (Global.CI < Global.CL.Length && (Char.IsLetter(Global.CL[Global.CI ])
                            || Global.CL[Global.CI] == '_'
                            || Char.IsDigit(Global.CL[Global.CI])
                            ))
                        {
                            Global.G_Buffer += Global.CL[Global.CI];
                            Global.CI++;
                        }
                        TSymbol Temp;
                        Temp = Global.Find_Symbol(Global.G_Buffer);
                        if (Temp != null)
                        {

                            Global.Message_Wrong = Error.Get_Error(51) + "\t" + Error.Get_Type_Error(1);
                            throw new Exception();
                        }
                        TIdentifier GID;
                        GID = Global.Find_Identifier(Global.G_Buffer, Global.G_Procedure);
                        if (GID != null)
                        {

                            Global.Message_Wrong = Error.Get_Error(51) + "\t" + Error.Get_Type_Error(1);
                            throw new Exception();
                        }
                        else
                        {
                            GID = Global.Find_Identifier(Global.G_Buffer, Global.G_Var);
                            if (GID == null)
                            {
                                Global.G_Current_STR = Global.G_Buffer;
                                Global.Add_Var(Global.G_Buffer);
                            }
                            else
                            {
                                Global.G_Current_STR = Global.G_Buffer;
                                Global.G_Current_ID = GID;
                            }
                            return Global.Type_Symbol.u_PRE_DECR;
                        }
                    }
                }
                else
                {
                    return Global.Type_Symbol.u_MINUS;
                }
            }
            #endregion 
            else if (Global.CI < Global.CL.Length && (Char.IsLetter(Global.CL[Global.CI]) || Global.CL[Global.CI] == '_'))
            {
              
                Global.G_Buffer = "";
                while (Global.CI < Global.CL.Length && (Char.IsLetter(Global.CL[Global.CI]) || Global.CL[Global.CI] == '_'
                    || Char.IsDigit(Global.CL[Global.CI])))
                {
                    Global.G_Buffer += Global.CL[Global.CI];
                    Global.CI++;
                }



                if( Global.CI < Global.CL.Length &&( ( Global.CL[Global.CI] =='+' && Global.CL[Global.CI+1] == '+')
                   || (Global.CL[Global.CI] == '-' && Global.CL[Global.CI + 1] == '-')
                    ))
                {
                    #region Post_Incr_Derc
                    bool isIncrement = (Global.CL[Global.CI] =='+');
                    Global.CI += 2;
                    TSymbol Temp;
                    Temp = Global.Find_Symbol(Global.G_Buffer);
                    if (Temp != null)
                    {

                        Global.Message_Wrong = Error.Get_Error(52) + "\t" + Error.Get_Type_Error(1);
                        throw new Exception();
                    }
                    TIdentifier GID;
                    GID = Global.Find_Identifier(Global.G_Buffer, Global.G_Procedure);
                    if (GID != null)
                    {

                        Global.Message_Wrong = Error.Get_Error(52) + "\t" + Error.Get_Type_Error(1);
                        throw new Exception();
                    }
                    else
                    {
                        GID = Global.Find_Identifier(Global.G_Buffer, Global.G_Var);
                        if (GID == null)
                        {
                            Global.G_Current_STR = Global.G_Buffer;
                            Global.Add_Var(Global.G_Buffer);
                        }
                        else
                        {
                            Global.G_Current_STR = Global.G_Buffer;
                            Global.G_Current_ID = GID;
                        }
                        if(isIncrement)
                        return Global.Type_Symbol.u_POST_INCR;
                        else
                            return Global.Type_Symbol.u_POST_DECR;
                    }
                    #endregion
                }

                else {
                    #region Identifier&&ReservedWords
                    TSymbol Temp;
                Temp = Global.Find_Symbol(Global.G_Buffer);
                    if (Temp != null)
                    {
                        Global.G_Buffer = Temp.name;
                        return Temp.token;
                    }
                    else
                    {
                        TIdentifier GID;
                        GID = Global.Find_Identifier(Global.G_Buffer, Global.G_Var);
                        if (GID != null)
                        {
                            Global.G_Current_STR = Global.G_Buffer;
                            Global.G_Current_ID = GID;
                            return Global.Type_Symbol.u_VAR;
                        }
                        else
                        {
                            GID = Global.Find_Identifier(Global.G_Buffer, Global.G_Procedure);
                            if (GID != null)
                            {
                                Global.G_Current_STR = Global.G_Buffer;
                                Global.G_Current_ID = GID;
                                return Global.Type_Symbol.u_PROCEDURE;
                            }
                            else
                            {
                                Global.G_Current_STR = Global.G_Buffer;
                                return Global.Type_Symbol.u_UNKNOWN;
                            }
                        }
                    }
                    #endregion
                }
            }
        

            #region Sign

            if (Global.CL[Global.CI] == ':')
            {
                Global.CI++;
                if (Global.CI < Global.CL.Length && Global.CL[Global.CI] == '=')
                {
                    Global.CI++;
                    Global.G_Buffer = ":=";
                    return Global.Type_Symbol.u_ASSIGN;
                }
                else
                {
                    return Global.Type_Symbol.u_2pts;
                }
            }
            if (Global.CL[Global.CI] == '+')
            {
                    Global.CI++;
                    Global.G_Buffer = "+";
                    return Global.Type_Symbol.u_PLUS;
               
            }
            if (Global.CL[Global.CI] == '-')
            {
                Global.CI++;
                    Global.G_Buffer = "-";
                    return Global.Type_Symbol.u_MINUS;
  
            }
            if (Global.CL[Global.CI] == '=')
            {
                Global.CI++;
                Global.G_Buffer = "=";
                return Global.Type_Symbol.u_EQUAL;
            }
            if (Global.CL[Global.CI] == '!')
            {
                Global.CI++;
                if (Global.CI < Global.CL.Length && Global.CL[Global.CI] == '=')
                {
                    Global.CI++;
                    Global.G_Buffer = "!=";
                    return Global.Type_Symbol.u_NOT_EQUAL;
                }
                else
                {
                    Global.G_Buffer = "!";
                    return Global.Type_Symbol.u_NOT;
                }
            }
            if (Global.CL[Global.CI] == '*')
            {
                Global.CI++;
                Global.G_Buffer = "*";
                return Global.Type_Symbol.u_MULTI;
            }
            if (Global.CL[Global.CI] == '/')
            {
                Global.CI++;
                Global.G_Buffer = "/";
                return Global.Type_Symbol.u_DIV;
            }
            if (Global.CL[Global.CI] == '&')
            {
                Global.CI++;
                if (Global.CI < Global.CL.Length && Global.CL[Global.CI] == '&')
                {
                    Global.CI++;
                    Global.G_Buffer = "&&";
                    return Global.Type_Symbol.u_AND;
                }
                else
                {
                    Global.Message_Wrong = Error.Get_Error(7) + "\t" + "\t" + Error.Get_Type_Error(1);
                    throw new Exception();
                }
            }
            if (Global.CL[Global.CI] == '|')
            {
                Global.CI++;
                if (Global.CI < Global.CL.Length && Global.CL[Global.CI] == '|')
                {
                    Global.CI++;
                    Global.G_Buffer = "||";
                    return Global.Type_Symbol.u_OR;
                }
                else
                {
                    Global.Message_Wrong = Error.Get_Error(8) + "\t" + "\t" + Error.Get_Type_Error(1);
                    throw new Exception();
                }
            }
            if (Global.CL[Global.CI] == '<')
            {
                Global.CI++;
                if (Global.CI < Global.CL.Length && Global.CL[Global.CI] == '=')
                {
                    Global.CI++;
                    Global.G_Buffer = "<=";
                    return Global.Type_Symbol.u_LE;
                }
                if (Global.CI < Global.CL.Length && Global.CL[Global.CI] == '>')
                {
                    Global.CI++;
                    Global.G_Buffer = "<>";
                    return Global.Type_Symbol.u_NOT_EQUAL;
                }
                Global.G_Buffer = "<";
                return Global.Type_Symbol.u_LT;
            }
            if (Global.CL[Global.CI] == '>')
            {
                Global.CI++;
                if (Global.CI < Global.CL.Length && Global.CL[Global.CI] == '=')
                {
                    Global.CI++;
                    Global.G_Buffer = ">=";
                    return Global.Type_Symbol.u_GE;
                }
                Global.G_Buffer = ">";
                return Global.Type_Symbol.u_GT;
            }
            if (Global.CL[Global.CI] == '(')
            {
                Global.CI++;
                Global.G_Buffer = "(";
                return Global.Type_Symbol.u_OPENP;
            }
            if (Global.CL[Global.CI] == ')')
            {
                Global.CI++;
                Global.G_Buffer = ")";
                return Global.Type_Symbol.u_CLOSEP;
            }
            if (Global.CL[Global.CI] == '[')
            {
                Global.CI++;
                Global.G_Buffer = "[";
                return Global.Type_Symbol.u_OPENB;
            }
            if (Global.CL[Global.CI] == ']')
            {
                Global.CI++;
                Global.G_Buffer = "]";
                return Global.Type_Symbol.u_CLOSEB;
            }
            if (Global.CL[Global.CI] == '{')
            {
                Global.CI++;
                Global.G_Buffer = "{";
                return Global.Type_Symbol.u_BEGIN;
            }
            if (Global.CL[Global.CI] == '}')
            {
                Global.CI++;
                Global.G_Buffer = "}";
                return Global.Type_Symbol.u_END;
            }
            if (Global.CL[Global.CI] == ';')
            {
                Global.CI++;
                Global.G_Buffer = ";";
                return Global.Type_Symbol.u_SEMICOLON;
            }
            if (Global.CL[Global.CI] == ',')
            {
                Global.CI++;
                Global.G_Buffer = ",";
                return Global.Type_Symbol.u_COMMA;
            }
            if (Global.CL[Global.CI] == '%')
            {
                Global.CI++;
                Global.G_Buffer = "%";
                return Global.Type_Symbol.u_MOD;
            }
            #endregion
            else
            {
                Global.Message_Wrong = Error.Get_Error(9) + "\t" + "\t" + Error.Get_Type_Error(1);
                throw new Exception();
            }
        }
    }
}