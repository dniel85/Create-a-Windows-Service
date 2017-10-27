using System;
using System.IO;
using NessusService;
using System.IO.Compression;

namespace NessusFileTransfer
{
    class UnZip
    {
        public static void unZipper()
        {
            if (!Directory.Exists(ProgramDirectories.NewUpdates))
            {
                Directory.CreateDirectory(ProgramDirectories.NewUpdates);
            }

            string[] zipDir = Directory.GetFiles(ProgramDirectories.NSOC, "*.zip", SearchOption.TopDirectoryOnly);

            foreach (string zips in zipDir)
            {
                try
                {
                    ZipFile.ExtractToDirectory(zips, ProgramDirectories.NewUpdates);
                    File.Delete(zips);
                }
                catch (Exception e)
                {
                    Logging.WriteLog(" The process failed: {34}" + e.ToString());


                }
            }
        }
    }
}


              
     

            

    

        
      