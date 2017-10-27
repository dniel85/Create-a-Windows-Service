using System;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;




namespace NessusService
{
    static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static void Main(string[] args)
        {
#if DEBUG
            Scheduler myService = new Scheduler();
            myService.OnDebug();
            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);

#else

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Scheduler()
            };
            ServiceBase.Run(ServicesToRun);
#endif
            CreateConnectors();
            RmDirLoop();
            unZip();
            DisplayUpdates();
        }
        public static string Zippy()
        {
            var now = DateTime.Now;
            var thisMonth = now.ToString("MMMyyyy");
            string zipFile = @"c:\nessusdata\NSOC\" + thisMonth;
            return zipFile;
        }

        public static void CreateConnectors()
        {
            Console.WriteLine("create connectrs");
            Console.WriteLine(Zippy());

            foreach (string filePath in ProgramDirectories.conectorPaths)
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
        }
        public static void RmDirLoop()
        {
            Console.WriteLine("removing DIRS");
            DirectoryInfo getUpdates = new DirectoryInfo(ProgramDirectories.conectorPaths[0]);

            FileInfo[] Files = getUpdates.GetFiles("*.*");
            // if directorys have no files the return is 0 else int0<
            if (Files.Length == 0)
            {
                Console.WriteLine("No Superseded updates Extracting files now");
            }
            else
            {
                ProgramDirectories.CM1 = ProgramDirectories.conectorPaths[0];
                Directory.Delete(ProgramDirectories.CM1, true);

                ProgramDirectories.CM2 = ProgramDirectories.conectorPaths[0];
                Directory.Delete(ProgramDirectories.CM2, true);

                ProgramDirectories.CM3 = ProgramDirectories.conectorPaths[0];
                Directory.Delete(ProgramDirectories.CM3, true);

                ProgramDirectories.CM4 = ProgramDirectories.conectorPaths[0];
                Directory.Delete(ProgramDirectories.CM4, true);

                ProgramDirectories.CM5 = ProgramDirectories.conectorPaths[0];
                Directory.Delete(ProgramDirectories.CM5, true);

                ProgramDirectories.CM6 = ProgramDirectories.conectorPaths[0];
                Directory.Delete(ProgramDirectories.CM6, true);


            }

        }
        public static void unZip()
        {
            Console.WriteLine("unzip");
            if (File.Exists(Zippy()))
            {
                Console.WriteLine(Zippy());
                foreach (string destPath in ProgramDirectories.conectorPaths)
                {
                    ZipFile.ExtractToDirectory(Zippy(), destPath);
                }
            }
        }
        public static void DisplayUpdates()
        {
            Console.WriteLine("DISPLAY UPDATES");
            Console.WriteLine("Finishing Updates at: " + DateTime.Now);

            //Print out the avaliable updates from the .zip file 
            DirectoryInfo getNewUpdates = new DirectoryInfo(@"C:\nessusdata\cm1\connector.remote.nessusV2Compliance\new");
            FileInfo[] newFiles = getNewUpdates.GetFiles("*.*");
            string currentUpdates = "";
            Console.WriteLine("The following updates have been Imported:");
            foreach (FileInfo file in newFiles)
            {
                currentUpdates = currentUpdates + "," + file.Name;
                Console.WriteLine("<------------" + file.Name + "------------>");
            }
            Console.WriteLine("cleaning up files");
        }
    }
}