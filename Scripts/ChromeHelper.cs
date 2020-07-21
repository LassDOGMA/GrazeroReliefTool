using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrazeroReliefTool.Scripts
{
    class ChromeHelper
    {
        public static void KillAllProcesses(string proses)
        {
            System.Diagnostics.Process[] ps = System.Diagnostics.Process.GetProcessesByName(proses);
            foreach (System.Diagnostics.Process p in ps)
            {
                p.Kill();
            }
            Environment.Exit(0);
        }
    }
}
