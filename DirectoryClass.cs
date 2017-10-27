using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace NessusService
{
    class ProgramDirectories
    {
        //Core Directories
        public static string Root = @"D:\";
        public static string nessusData = Root + @"\nessusdata\";
        public static string NSOC = nessusData + @"\NSOC\";
        public static string logDir = nessusData + @"\logdir\";
        public static string archives = nessusData + @"\archives\";
        public static string dailyArchive = archives +@"\"+ CreateAdate();
        public static string NewUpdates = nessusData + @"\TEMPUpdates\";

        //Connector Paths
        
        public static string CM1 = nessusData + @"\cm1\connector.remote.nessusV2Compliance\new\";
        public static string CM2 = nessusData + @"\cm2\connector.remote.nessusV2Compliance\new\";
        public static string CM3 = nessusData + @"\cm3\connector.remote.nessusV2Compliance\new\";
        public static string CM4 = nessusData + @"\cm4\connector.remote.nessusV2Compliance\new\";
        public static string CM5 = nessusData + @"\cm5\connector.remote.nessusV2Compliance\new\";
        public static string CM6 = nessusData + @"\cm6\connector.remote.nessusV2Compliance\new\";
       
        //All Supporting Directories 
        public static string[] conectorPaths = { CM1, CM2, CM3, CM4, CM5, CM6 };
        public static string[] OasysisPlaces = { nessusData, NSOC, logDir, archives, NewUpdates, dailyArchive};

        //Strings of Dates
        public static string CreateAdate()
        {
            var now = DateTime.Now;
            var thisMonth = now.ToString("MMMdd");
            string datePath = thisMonth;
            return datePath;
        }
    }

    class ProgramFunctions
    {
        public static void CreateDirectories()
        {   
            foreach (string filePath in ProgramDirectories.conectorPaths)

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);

                foreach (string otherDirs in ProgramDirectories.OasysisPlaces)

                    if (!Directory.Exists(otherDirs))
                    {
                        Directory.CreateDirectory(otherDirs);
                    }
                }

         }
               
        public static void CopyToConnectors(string tempUpdateDir, string Cpath)
        { 
            foreach (string srcPath in Directory.GetFiles(ProgramDirectories.NewUpdates))//////////////////////////////////////////////////////////////////////////
            {
                if (File.Exists(srcPath))
                {
                    File.Copy(srcPath, srcPath.Replace(tempUpdateDir, Cpath), true);                 
                }
                
            }

        }

        public static void IMnotAzip()
        {
            try
            {
                string[] xml = Directory.GetFiles(ProgramDirectories.NSOC, "*.xml", SearchOption.TopDirectoryOnly);

                foreach (string xmls in xml)
                {
                    File.Copy(xmls, xmls.Replace(ProgramDirectories.NSOC, ProgramDirectories.NewUpdates), true);
                    File.Delete(xmls);
                }

            }
            catch (Exception e)
                {
                    Logging.WriteLog("The process failed @ L93 " + e);
                }

        }

        public static void IntegrityCheck(string targetdir)
        {
            if (Directory.Exists(targetdir))
            {
                try
                {
                    DirectoryInfo di = new DirectoryInfo(targetdir);
                    
                    foreach(DirectoryInfo dir in di.GetDirectories())
                    {
                        Logging.WriteLog("Removing! " + dir + " This Directory can not be processed");
                        dir.Delete(true);
                    }

                    List<string> notXml_Or_Zipfiles = Directory.EnumerateFiles(targetdir, "*.*", SearchOption.AllDirectories)
                        .Where(n => Path.GetExtension(n) != ".xml").Where(n => Path.GetExtension(n) != ".zip").ToList();
                    string[] notXmlarray = notXml_Or_Zipfiles.Select(i => i.ToString()).ToArray();

                    foreach (string eronfile in notXmlarray)
                    {
                        File.Delete(eronfile);
                        Logging.WriteLog("removing! " + eronfile + " This file can not be processed");
                    }
                }
                finally
                {
                    if (targetdir == ProgramDirectories.NSOC)
                    {
                        Logging.WriteLog("Files In Directory " + targetdir.Remove(0, 14) + " Directory status: OK");

                        if (targetdir != ProgramDirectories.NSOC)
                        {
                            Logging.WriteLog("Files in Directory" + targetdir.Remove(0, 14) + " Directory status OK");
                        }
                    }
                }
            }
        }
                       
        public static void RmTempFile()
        {
            if(!Directory.Exists(ProgramDirectories.dailyArchive))
            {
                Directory.CreateDirectory(ProgramDirectories.dailyArchive);
            }

            string[] oldZipdir = Directory.GetFiles(ProgramDirectories.NewUpdates, "*.zip", SearchOption.TopDirectoryOnly);

            foreach (string rmZips in oldZipdir)
            {
                File.Copy(rmZips, Path.Combine(ProgramDirectories.dailyArchive, rmZips), true);
                File.Delete(rmZips);
            }
  
            if (oldZipdir.Length >= 1)
            {
                Directory.Delete(ProgramDirectories.NewUpdates, true);
                Logging.WriteLog("Removing 'Temp' Directory");
            }
        }

        public static void OldUpdates(string cmPath)
        {
            string[] cmPaths = Directory.GetFiles(cmPath);
  
            try
            {
                foreach (string file in cmPaths)
                {
                    FileInfo fi = new FileInfo(file);
                    if (fi.CreationTime < DateTime.Now.AddMinutes(-10))
                    {
                        Logging.WriteLog("Removing " + fi.Name + " from "+cmPath.Remove(0,14));
                        fi.Delete();
                    }

                }
            }
            catch (Exception e)
            {
                Logging.WriteLog("The process failed: {0}" + e.ToString());
            }

        }

    }


    class Logging
    {
        public static void WriteLog(string strLog)
        {
            StreamWriter log;
            FileStream fileStream = null;
            DirectoryInfo logDirInfo = null;
            FileInfo logFileInfo;

            string logFilePath = ProgramDirectories.logDir;

            logFilePath = logFilePath + "Log-" + DateTime.Today.ToString("MM-dd-yyyy") + "." + "log";
            logFileInfo = new FileInfo(logFilePath);
            logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
            if (!logDirInfo.Exists) logDirInfo.Create();

            if (!logFileInfo.Exists)
            {
                fileStream = logFileInfo.Create();
            }

            else
            {
                fileStream = new FileStream(logFilePath, FileMode.Append);
            }

            log = new StreamWriter(fileStream);
            log.WriteLine(strLog + " at " + DateTime.Now);
            log.Close();
        }

        public static void TailLogPowerShellScript()
        {
            string path = @"D:\nessusdata\LogTail.ps1";

            string createLine = "Get-Content D:\\nessusdata\\logdir\\Log-" + DateTime.Today.ToString("MM-dd-yyyy") + "." + "log -Tail 2 -Wait" + Environment.NewLine;
            File.WriteAllText(path, createLine);
        }

        public static void DisplayUpdates()
        {
            if (Directory.Exists(ProgramDirectories.NewUpdates))
            {
                DirectoryInfo getNewUpdates = new DirectoryInfo(ProgramDirectories.NewUpdates);
                FileInfo[] newFiles = getNewUpdates.GetFiles("*.*");

                if (newFiles.Length >= 1)
                {

                    string currentUpdates = "";
                    Logging.WriteLog("The following updates have been unzipped to Connector Paths: CM1, CM2,CM3, CM4, CM5, CM6");
                    WriteLog("-----------------------------------");

                    foreach (FileInfo file in newFiles)
                    {
                        if (file.CreationTime < DateTime.Now.AddMinutes(5))
                        {
                            currentUpdates = currentUpdates + "," + file.Name;
                            Logging.WriteLog("<-----" + file.Name + "----->");
                        }

                    }
                    WriteLog("Unzip Complete");
                }
                else
                {
                    Logging.WriteLog("No Updates Avaliable");
                }

            }
        }
    }

    class Archiving
    {
        public static void ZipOldUpdate()
        {
            TimeSpan start = new TimeSpan(23, 0, 0); //10 o'clock
            TimeSpan end = new TimeSpan(02, 0, 0); //12 o'clock
            TimeSpan now = DateTime.Now.TimeOfDay;
            //Logging.WriteLog(now.ToString());

            if ((now > start) && (now < end))
            {
            Logging.WriteLog("Creating a backup");

            //string archiveDrive = @"D:\nessusdata\archives\" + DateTime.Now + ".zip";

            Logging.WriteLog("Creating a backup under " + ProgramDirectories.dailyArchive);

            ZipFile.CreateFromDirectory(ProgramDirectories.dailyArchive, ProgramDirectories.archives, CompressionLevel.Optimal, true);
            //Directory.Delete(ProgramDirectories.dailyArchive, true);

            string[] files = Directory.GetFiles(ProgramDirectories.archives);
            // Foreach update that is older than 3 months delete it out of Archive

                foreach (string file in files)
                {
                    FileInfo fi = new FileInfo(file);
                    if (fi.CreationTime < DateTime.Now.AddMonths(-4))
                        Logging.WriteLog("Deleting "+ fi +" the archived Directory is older than 3 months");
                    fi.Delete();
                }

                string[] logfiles = Directory.GetFiles(ProgramDirectories.logDir);
                foreach(string logfile in logfiles)
                {

                    FileInfo fi = new FileInfo(logfile);
                    if (fi.CreationTime < DateTime.Now.AddMonths(-4))
                        Logging.WriteLog("Deleting " + fi + " the archived log is older than 3 months");
                }

            }
            else
            {
                Logging.WriteLog("Archive Not createed at this time.  Archive times are between "+now.ToString("HH:mm"+" and " +end.ToString("HH:mm")+"."));
            }
        }
    }
}