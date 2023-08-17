using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Utils
    {
        public static void RegisterInStartup(bool isChecked, String applicationName, string executablePath)
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey
                    ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (isChecked)
                registryKey.SetValue(applicationName, executablePath);
            else
                registryKey.DeleteValue(applicationName);
        }
    }
}
