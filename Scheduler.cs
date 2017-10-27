using System;
using System.IO;
using System.ServiceProcess;
using System.Timers;

namespace NessusService
{
    public partial class Scheduler : ServiceBase
    {
        private Timer timer1 = null;
      //  private Timer timer2 = null;


        /*Milliseconds to Minutes Ref
           
            MILLISECONDS           MINUTES
             60000                 1
            120000                 2 
            180000                 3
            300000                 5
            600000                 10

            MILLSECONDS            HOURS
            3600000                1
            21600000               6
            43200000               12
            86400000               24
             */

        public Scheduler()
        {
            InitializeComponent();
        }


        protected override void OnStart(string[] args)
        {
            
            ReadMeDoc.README();
            Logging.TailLogPowerShellScript();
            ProgramFunctions.CreateDirectories();
           

            timer1 = new Timer();
            timer1.Interval = 60000;  // <-- Poll every 2 Min
            Logging.WriteLog("*************** Starting Service");
            Logging.WriteLog("Updates being checked every " + ((timer1.Interval / 1000) / 60) + " Minute(S) starting");
            timer1.Elapsed += new ElapsedEventHandler(timer1_tick);
            timer1.Enabled = true;

            timer1.AutoReset = false;

        }
        
            private void timer1_tick(object sender, ElapsedEventArgs e)    
            {

            try
            {
                Logging.WriteLog("");
                Logging.WriteLog("_____________________________________________");

                Logging.WriteLog("checking for updates");

                Program.Main();
            }

            finally
            {
                timer1.Enabled = true;
                DirectoryInfo getUpdates = new DirectoryInfo(ProgramDirectories.NSOC);
                FileInfo[] Files = getUpdates.GetFiles("*.*");

                if (Files.Length == 0)
                {
                    if (!Directory.Exists(ProgramDirectories.NewUpdates))
                    {
                       
                        ProgramFunctions.OldUpdates(ProgramDirectories.CM1);
                        ProgramFunctions.OldUpdates(ProgramDirectories.CM2);
                        ProgramFunctions.OldUpdates(ProgramDirectories.CM3);
                        ProgramFunctions.OldUpdates(ProgramDirectories.CM4);
                        ProgramFunctions.OldUpdates(ProgramDirectories.CM5);
                        ProgramFunctions.OldUpdates(ProgramDirectories.CM6);

                        Logging.WriteLog("Finishing updates");
                     
                        Logging.WriteLog("_____________________________________________");
                        Logging.WriteLog("");
                    }
                }
               
            }
        }
      
        //Program.Main();

        
        protected override void OnStop()
        {
            Logging.WriteLog("*************** Stopping Service");
            this.ExitCode = 0;
            base.OnStop();
        } 
    }
}

        
    
    

