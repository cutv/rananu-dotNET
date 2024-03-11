using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rananu.Shared
{
    public static class Logs
    {
        public static void Error(Exception e)
        {
            string filepath = "Logs/";
            var line = Environment.NewLine + Environment.NewLine;
            string ErrorlineNo = string.Empty;
            if (e.StackTrace != null)
                ErrorlineNo = e.StackTrace;
            string Errormsg = e.GetType().Name.ToString();
            string extype = e.GetType().ToString();
            string ErrorLocation = e.Message.ToString();
            string ErrorSource = e.Source;
            if (!Directory.Exists(filepath))
                Directory.CreateDirectory(filepath);
            filepath = filepath + DateTime.Today.ToString("dd-MM-yy") + ".txt";   //Text File Name
            if (!File.Exists(filepath))
                File.Create(filepath).Dispose();
            using (StreamWriter sw = File.AppendText(filepath))
            {
                string error = "Log Written Date:" + " " + DateTime.Now.ToString() + line + "Error Line No :" + " " + ErrorlineNo + line + "Error Message:" + " " + Errormsg + line + "Exception Type:" + " " + extype + line + "Error Location :" + " " + ErrorLocation + line + "Error Source :" + " " + ErrorSource + line;
                sw.WriteLine("-----------Exception Details on " + " " + DateTime.Now.ToString() + "-----------------");
                sw.WriteLine("-------------------------------------------------------------------------------------");
                sw.WriteLine(line);
                sw.WriteLine(error);
                sw.WriteLine("--------------------------------*End*------------------------------------------");
                sw.WriteLine(line);
                sw.Flush();
                sw.Close();
            }
        }


        public static void Error(string message)
        {
            Error(new Exception(message));
        }
    }
}
