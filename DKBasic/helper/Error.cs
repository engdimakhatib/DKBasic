using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasic.helper
{
    internal class Error
    {
        public static void Read_Files_Error()
        {
            StreamReader File_Error = new StreamReader("Error.INI");
            while (!File_Error.EndOfStream)
            {
                string[] line = File_Error.ReadLine().Split(":");
                Global.Error_Message[Convert.ToInt32(  line[0])]=line[1];
            }
            File_Error.Close();
            StreamReader File_Type_Error = new StreamReader("Type_Error.INI");
            while (!File_Type_Error.EndOfStream)
            {
                string[] line = File_Type_Error.ReadLine().Split(":");
                Global.Type_Error[Convert.ToInt32(line[0])] = line[1];
            }
            File_Type_Error.Close();
        }
        public static string Get_Error(int NB_Error)
        {
            return Global.Error_Message[NB_Error];
        }
        public static string Get_Type_Error(int NB_Type_Error)
        {
            return Global.Type_Error[NB_Type_Error];
        }
    }
}
