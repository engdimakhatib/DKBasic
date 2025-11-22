using DKBasic.Forms;
using DKBasic.structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasic.helper
{
    public class Global
    {
        #region Global_Variables 
        public static TSymbol G_Symbol;
        public static TSymbol G_Last_Symbol;
        public static TFile G_File;
        public static TFile G_Last_File;
        public static Dictionary<int , string > Error_Message = new Dictionary<int , string >();
        public static Dictionary<int , string> Type_Error = new Dictionary<int , string >();
        public static string Message_Wrong = "";
        public static TFile G_Current_File;
        public static StreamReader G_Current_File_SR;
        public static string CL = "";
        public static int CI = 0;
        public static Double G_Current_NB = 0;
        public static string G_Buffer = "";
        public static string G_Current_STR = "";
        public static TIdentifier G_Current_ID;
        public static TVar G_Var;
        public static TProcedure G_Procedure;
        public static Type_Symbol token;
        public static bool Compilation_Successful = true;
        public static int Current_Line_NB = 0;
        public static Boolean G_Read_Main_Word = false;
        public static Tinstruction G_Main_Instruction = null;



        public static Boolean Is_Input_Integer = true;
        public static Boolean Is_Input_Real = false;
        public static Boolean Is_Input_String = false;
        public static Boolean Is_Input_Boolean = false;

        public static OutputForm Output_Write_Form = new OutputForm();

        public enum Type_Symbol
        {
            u_IF,
            u_DO,
            u_WHILE,
            u_EOF,
            u_CST_REAL,
            u_CST_INT,
            u_CST_STR,
            u_ERROR,
            u_VAR,
            u_UNKNOWN,
            u_EQUAL,
            u_NOT_EQUAL,
            u_PLUS,
            u_MINUS,
            u_NOT,
            u_MULTI,
            u_DIV,
            u_LT,
            u_GT,
            u_LE,
            u_GE,
            u_AND,
            u_OR,
            u_POWER_SIGHN,
            u_SEMICOLON,
            u_COMMA,
            u_OPENP,
            u_CLOSEP,
            u_CLOSEB,
            u_OPENB,
            u_BEGIN,
            u_END,
            u_SHARP,
            u_MOD,
            u_ASSIGN,
            u_INCLUDE,
            u_PROCEDURE,
            u_FUNCTION,
            u_OUTPUT,
            u_THEN,
            u_ELSE,
            u_IDIV,
            u_CALL,
            u_READ,
            u_WRITE,
            u_WRITELN,
            u_FALSE,
            u_TRUE,
            u_SIN,
            u_COS,
            u_TAN,
            u_ATAN,
            u_LOG,
            u_LN,
            u_POWER,
            u_SQRT,
            u_UNARY_PLUS,
            u_UNARY_MINUS,
            u_NORMAL,
            u_LENGTH,
            u_INT_TO_STRING,
            u_STRING_TO_INT,
            u_MAIN,
            u_ABS,
            u_SWITCH,
            u_DEFAULT,
            u_OF,
            u_2pts,
            u_POST_INCR,
            u_POST_DECR,
            u_PRE_INCR,
            u_PRE_DECR,
            u_FOR,
            u_TO,
            u_DOWN_TO,
            u_STEP,
            u_BREAK,
            u_CONTINUE
        };
        #endregion
        #region Global_Methods
        public static void Read_Key_Word()
        {
            StreamReader keyWordFile = new StreamReader("keywords.INI");
            int index = 0;
            string currentLine = "";
         while((   currentLine = keyWordFile.ReadLine()) != null)
            {
                TSymbol new_Symbol = new TSymbol();
                new_Symbol.name = currentLine;
                new_Symbol.token =(Type_Symbol) index;
                new_Symbol.next = null;
                if (G_Symbol == null)
                    G_Symbol = new_Symbol;
                else G_Last_Symbol.next = new_Symbol;
                G_Last_Symbol = new_Symbol;
                index++;
            }
         keyWordFile.Close();
        }
        public static TSymbol Find_Symbol(string Word_To_Find)
        {
            TSymbol current_Symbol = G_Symbol;
            Word_To_Find = Word_To_Find.ToUpper();
            while (current_Symbol!=null && current_Symbol.name !=Word_To_Find)
            {
                current_Symbol = current_Symbol.next;   
            }
            return current_Symbol;
        }
        public static void Add_File(string File_Name)
        {
            string full_Path = Path.GetFullPath(File_Name);
            if (!File.Exists(full_Path))
            {
                Message_Wrong = Error.Get_Error(0) +"\t"+"\t" +Error.Get_Type_Error(2);
                throw new Exception("الملف غير موجود");
            }
            
            TFile new_File = new TFile();
            TFile current_File = G_File;
            while (current_File!=null && current_File.name != full_Path )
            {
                current_File = current_File.next;
            }
            if (current_File == null)
            {
                new_File.name = full_Path;
                new_File.next = null;
                if (G_File == null)
                    G_File = new_File;
                else G_Last_File.next = new_File;
                G_Last_File = new_File;
            }
           
        }
        public static TIdentifier Find_Identifier( string Word_To_Find , TIdentifier GID)
        {
            while ( GID != null && GID.name != Word_To_Find)
            {
                GID = GID.next;
            }
            return GID;
        }
        public static TVar Add_Var( string buffer)
        {
            TVar Var_Aux = new TVar();
            Var_Aux.name = buffer;
            Var_Aux.token = Type_Symbol.u_VAR;
            Var_Aux.next = null;
            if(G_Var == null)
            {
                G_Var = Var_Aux;
            }
            else
            {
                TVar temp = G_Var;
                while (temp.next != null)
                {
                    temp =(TVar) temp.next;
                }
                temp.next = Var_Aux;
            }
            G_Current_ID = Var_Aux;
            return Var_Aux;
        }

        public static TProcedure Add_Procedure(string buffer)
        {
            TProcedure Proc_Aux = new TProcedure();
            Proc_Aux.name = buffer;
            Proc_Aux.next = null;
            if (G_Procedure == null)
            {
                G_Procedure = Proc_Aux;
            }
            else
            {
                TProcedure temp = G_Procedure;
                while (temp.next != null)
                {
                    temp = (TProcedure)temp.next;
                }
                temp.next = Proc_Aux;
            }
            G_Current_ID = Proc_Aux;
            return Proc_Aux;
        }

        #endregion
    }
}
