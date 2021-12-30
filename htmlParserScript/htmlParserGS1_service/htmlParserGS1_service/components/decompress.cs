using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Threading;

namespace htmlParserGS1_service.components
{
    public class decompress
    {
        string compressedFile;
        string targetFolder;
        string logFileDir;
        public decompress(string file, string folder, string logDir)
        {
            compressedFile = file;
            targetFolder = folder;
            logFileDir = logDir;
        }
        public void startDecompress()
        {
            try
            {
                if (File.Exists(compressedFile))
                {
                    Thread.Sleep(3000);

                    ZipFile.ExtractToDirectory(compressedFile, targetFolder);

                    File.Delete(compressedFile);

                    if (Directory.Exists(logFileDir))
                    {
                        using (StreamWriter writer = new StreamWriter(logFileDir + "logs.txt", true))
                        {
                            writer.WriteLine(String.Format("{0} файл {1} был распакован и удален",
                                DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), compressedFile));
                            writer.Flush();
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                if (Directory.Exists(logFileDir))
                {
                    using (StreamWriter writer = new StreamWriter(logFileDir + "logs.txt", true))
                    {
                        writer.WriteLine(String.Format("{0} - ERROR: Ошибка во время распаковки файла {1} [{2}]",
                            DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), compressedFile,ex.Message));
                        writer.Flush();
                    }
                }
            }
        }
    }
}
