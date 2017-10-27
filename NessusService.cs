using System.ServiceProcess;
using NessusFileTransfer;
using System.IO;

namespace NessusService
{
    static class Program
    {
        public static void Main()
        {         
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
            new Scheduler()
            };
            ServiceBase.Run(ServicesToRun);

            ProgramFunctions.CreateDirectories();

            Logging.WriteLog("Checking File Structure integrity");

            ProgramFunctions.IntegrityCheck(ProgramDirectories.NSOC);
            ProgramFunctions.IntegrityCheck(ProgramDirectories.CM1);
            ProgramFunctions.IntegrityCheck(ProgramDirectories.CM2);
            ProgramFunctions.IntegrityCheck(ProgramDirectories.CM3);
            ProgramFunctions.IntegrityCheck(ProgramDirectories.CM4);
            ProgramFunctions.IntegrityCheck(ProgramDirectories.CM5);
            ProgramFunctions.IntegrityCheck(ProgramDirectories.CM6);

            UnZip.unZipper();

            ProgramFunctions.IMnotAzip();

            ProgramFunctions.CopyToConnectors(ProgramDirectories.NewUpdates, ProgramDirectories.CM1);
            ProgramFunctions.CopyToConnectors(ProgramDirectories.NewUpdates, ProgramDirectories.CM2);
            ProgramFunctions.CopyToConnectors(ProgramDirectories.NewUpdates, ProgramDirectories.CM3);
            ProgramFunctions.CopyToConnectors(ProgramDirectories.NewUpdates, ProgramDirectories.CM4);
            ProgramFunctions.CopyToConnectors(ProgramDirectories.NewUpdates, ProgramDirectories.CM5);
            ProgramFunctions.CopyToConnectors(ProgramDirectories.NewUpdates, ProgramDirectories.CM6);
            ProgramFunctions.CopyToConnectors(ProgramDirectories.NewUpdates, ProgramDirectories.dailyArchive);

            Logging.DisplayUpdates();

            ProgramFunctions.RmTempFile();

            Archiving.ZipOldUpdate();
        }
    }
}

