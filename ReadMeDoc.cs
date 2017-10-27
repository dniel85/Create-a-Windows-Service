using System;
using System.IO;

namespace NessusService
{
    class ReadMeDoc
    {

            public static void README()
            {
                string path = @"D:\nessusdata\README.md";
                if (!File.Exists(path))
                {
                    // Create a file to write to.
                    using (StreamWriter sw = File.CreateText(path))
                    {
                    sw.WriteLine("****README****");
                    sw.WriteLine("This service Provides updates for all Six connectors. The basics of this service is it recieves a yearMonth.zip");
                    sw.WriteLine("file, unzips it and distributes it to the connectors.  It also keeps a 60 day archive under the archive directories");
                    sw.WriteLine("used the DTG format.  Refer to the logs for troubleshooting this service.");
                    sw.WriteLine("Log files can be found under the LogDir directory.  Here as well the service stores 60 days worth of logs.");
                    sw.WriteLine("");
                    sw.WriteLine("If you have any questions please refer to Authors at the end of this document.");
                    sw.WriteLine("### Prerequisites ###");
                    sw.WriteLine(".NET Framework 4.7 must be enabled for this service to work. ");
                    sw.WriteLine("");
                    sw.WriteLine("•••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••");
                    sw.WriteLine("### Installing ###");
                    sw.WriteLine("Step One");
                    sw.WriteLine("•••••••••••••••");
                    sw.WriteLine("Save the NessusService folder to a location such as your home directory or desktop");
                    sw.WriteLine("");
                    sw.WriteLine("Step Two");
                    sw.WriteLine("•••••••••••••••");
                    sw.WriteLine("Open a cmd prompt as administrator and navigate to the following path:");
                    sw.WriteLine(@"cd C:\Windows\Microsoft.net\Framework\v4.30319>");
                    sw.WriteLine("");
                    sw.WriteLine("Step Three");
                    sw.WriteLine("•••••••••••••••");
                    sw.WriteLine("Install the service by using InstallUtil.exe");
                    sw.WriteLine(@"C:\Windows\Microsoft.NET\Framework\v4.30319>  InstallUtil.exe <filepath to");
                    sw.WriteLine(@"NessusService Folder>\NessusFileTransfer.exe");
                    sw.WriteLine("");
                    sw.WriteLine("Step Four");
                    sw.WriteLine("•••••••••••••••");
                    sw.WriteLine("After running the InstallUtil.exe ensure you have successfully installed the 'Nessus File Service'");
                    sw.WriteLine("Open a cmd prompt and type 'services.msc'");
                    sw.WriteLine("Scroll Down until you see 'Nessus File service'");
                    sw.WriteLine("RIGHT-CLICK Start Service");
                    sw.WriteLine("•••••••••••••••");
                    sw.WriteLine("");
                    sw.WriteLine("");
                    sw.WriteLine("EXAMPLE");
                    sw.WriteLine("•••••••••••••••");
                    sw.WriteLine("1. Save NessusService folder to C:temp");
                    sw.WriteLine("2. [WINDOWS-KEY] 'CMD' RIGHT-CLICK Run as Administrator");
                    sw.WriteLine("");
                    sw.WriteLine(@"C:\Windows\system32>cd C:\Windows\Microsoft.NET\Framework\v4.0.30319");
                    sw.WriteLine("[ENTER]");
                    sw.WriteLine("");
                    sw.WriteLine(@"3. C:\Windows\Microsoft.NET\Framework\v4.0.30319>InstallUtil.exe");
                    sw.WriteLine(@"c:\temp\NessusService\NessusFiletransfer.exe");
                    sw.WriteLine("[ENTER]");
                    sw.WriteLine("");
                    sw.WriteLine("4. services.msc");
                    sw.WriteLine("[ENTER]");
                    sw.WriteLine("");
                    sw.WriteLine("In the right pane search for the service called Nessus File Transfer RIGHT+CLICK");
                    sw.WriteLine("start the service");
                    sw.WriteLine("");
                    sw.WriteLine("Authors");
                    sw.WriteLine("Darrell NIelsen**  dnielsen@oasysic.com");
                    sw.WriteLine("Contact DLVATeamOasys@oasysic.com if you have any aditional questions.");
                    }
                
                }

                // Open the file to read from.
                using (StreamReader sr = File.OpenText(path))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(s);
                    }
                }
            }
        }
    }

