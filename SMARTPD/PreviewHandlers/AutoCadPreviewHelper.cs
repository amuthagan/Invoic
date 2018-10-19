/*
 * FileName: WordPreviewHelper.cs
 * Author: Alexei Bouravtsev
 * Profiles: http://www.linkedin.com/pub/alexei-bouravtsev/13/201/293
 *           http://aleksey-buravtsev.moikrug.ru/
 */

using System.Collections.Generic;
using System.Diagnostics;

namespace PreviewHandlers
{
    public class AutoCadPreviewHelper : AppPreviewHelperBase
    {
        public AutoCadPreviewHelper()
        {
            ProcessName = "acad.exe";
            Extensions = new List<string>(){"dwg","DWG"};
            DoPreviewOnResize = true;
        }

        public override void Startup()
        {
            var processes = Process.GetProcessesByName(ProcessName);
            foreach (var process in processes)
            {
                var parentProcess = process.Parent();
                if (parentProcess.ProcessName == "svchost")
                    process.Kill();
            }
        }

        public override void Cleanup()
        {
            var processes = Process.GetProcessesByName(ProcessName);
            foreach (var process in processes)
            {
                var parentProcess = process.Parent();
                if (parentProcess.ProcessName == "svchost")
                    process.Kill();
            }
        }
    }
}
