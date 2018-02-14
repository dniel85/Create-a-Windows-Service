using System.ServiceProcess;
using NessusFileTransfer;
using System.IO;


//KNOWN WORKING VERSION!!!!!


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

            

            int ZipCount = Directory.GetFiles(ProgramDirectories.NSOC, "*.zip",
                SearchOption.TopDirectoryOnly).Length;

            int FileCount = Directory.GetFiles(ProgramDirectories.NSOC, "*.nessus",
                SearchOption.TopDirectoryOnly).Length;


            if (FileCount != 0 || ZipCount !=0)
            {
                Logging.WriteLog("Updates Found Extracting now");
                ProgramFunctions.IntegrityCheck(ProgramDirectories.NSOC);
                Archiving.CopyToArchive();
                ProgramFunctions.IMnotAzip();
                UnZip.unZipper();
            }
            
                ProgramFunctions.CopyToConnectors(ProgramDirectories.NewUpdates, ProgramDirectories.CM1);
                ProgramFunctions.IntegrityCheck(ProgramDirectories.CM1);
                ProgramFunctions.CopyToConnectors(ProgramDirectories.NewUpdates, ProgramDirectories.CM2);
                ProgramFunctions.IntegrityCheck(ProgramDirectories.CM2);
                ProgramFunctions.CopyToConnectors(ProgramDirectories.NewUpdates, ProgramDirectories.CM3);
                ProgramFunctions.IntegrityCheck(ProgramDirectories.CM3);
                ProgramFunctions.CopyToConnectors(ProgramDirectories.NewUpdates, ProgramDirectories.CM4);
                ProgramFunctions.IntegrityCheck(ProgramDirectories.CM4);
                ProgramFunctions.CopyToConnectors(ProgramDirectories.NewUpdates, ProgramDirectories.CM5);
                ProgramFunctions.IntegrityCheck(ProgramDirectories.CM5);
                ProgramFunctions.CopyToConnectors(ProgramDirectories.NewUpdates, ProgramDirectories.CM6);
                ProgramFunctions.IntegrityCheck(ProgramDirectories.CM6);
            
           


            Logging.DisplayUpdates();

            ProgramFunctions.RmTempFile();

            Archiving.ZipOldUpdate();
        }
    }
}

