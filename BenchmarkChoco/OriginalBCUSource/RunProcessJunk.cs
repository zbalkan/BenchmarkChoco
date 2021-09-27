using System;
using System.Diagnostics;

namespace BenchmarkChoco
{
    public class RunProcessJunk : JunkResultBase
    {
        public ProcessStartCommand ProcessToStart { get; }

        private readonly string _junkName;

        public RunProcessJunk(ApplicationUninstallerEntry application, IJunkCreator source, ProcessStartCommand processToStart, string junkName) : base(application, source)
        {
            _junkName = junkName;
            ProcessToStart = processToStart;
        }

        public override void Backup(string backupDirectory)
        {

        }

        public override void Delete()
        {
            try
            {
                var info = ProcessToStart.ToProcessStartInfo();
                info.WindowStyle = ProcessWindowStyle.Minimized;
                info.UseShellExecute = true;
                Process.Start(info)?.WaitForExit();
            }
            catch (SystemException ex)
            {
                Console.WriteLine(ex);
            }
        }

        public override string GetDisplayName()
        {
            return _junkName;
        }

        public override void Open()
        {
            try
            {
                WindowsTools.OpenExplorerFocusedOnObject(ProcessToStart.FileName);
            }
            catch (SystemException ex)
            {
                //PremadeDialogs.GenericError(ex);
            }
        }
    }
}