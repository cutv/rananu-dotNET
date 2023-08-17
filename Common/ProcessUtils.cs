using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
   public class ProcessUtils
    {

        public static void KillProcess(string processName)
        {
            try
            {
                Process[] AllProcesses = Process.GetProcesses();
                foreach (var process in AllProcesses)
                {
                    // if (process.MainWindowTitle != "")
                    {
                        string s = process.ProcessName.ToLower();
                        if (s.StartsWith(processName))
                            process.Kill();
                    }
                }
            }
            catch (Exception)
            {

            }
          
        }
    }
}
