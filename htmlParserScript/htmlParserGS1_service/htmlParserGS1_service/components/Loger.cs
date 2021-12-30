using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using htmlParserGS1_service.parser.models;
using htmlParserGS1_service.parser.components;

namespace htmlParserGS1_service.components
{
    public class Loger
    {
        FileSystemWatcher watcher;
        object obj = new object();
        bool enabled = true;
        string logFileDir;
        string analyzeFolder;

        public Loger(string pathTodir, string logFileDir)
        {
            watcher = new FileSystemWatcher(pathTodir);
           // watcher.Deleted += Watcher_Deleted;
            watcher.Created += Watcher_Created;
            //watcher.Changed += Watcher_Changed;
           // watcher.Renamed += Watcher_Renamed;

            this.logFileDir = logFileDir;
            analyzeFolder = pathTodir;
        }

        public void Start()
        {
            watcher.EnableRaisingEvents = true;
            while (enabled)
            {
                Thread.Sleep(1000);
            }
        }
        public void Stop()
        {
            watcher.EnableRaisingEvents = false;
            enabled = false;
        }
        // переименование файлов
        private void Watcher_Renamed(object sender, RenamedEventArgs e)
        {
            string fileEvent = "переименован в " + e.FullPath;
            string filePath = e.OldFullPath;
            RecordEntry(fileEvent, filePath);
        }
        // изменение файлов
        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            string fileEvent = "изменен";
            string filePath = e.FullPath;
            RecordEntry(fileEvent, filePath);
        }
        // создание файлов
        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            string fileEvent = "создан";
            string filePath = e.FullPath;
            RecordEntry(fileEvent, filePath);

            if (filePath.Contains(".html"))
            {
                parser.General pars = new parser.General(filePath);
                Thread parsThread = new Thread(new ThreadStart(pars.startParser));
                parsThread.Start();
            }

            if (filePath.Contains(".zip"))
            {
                decompress dec = new decompress(e.FullPath, analyzeFolder, logFileDir);
                Thread decompThread = new Thread(new ThreadStart(dec.startDecompress));
                decompThread.Start();
            }

        }
        // удаление файлов
        private void Watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            string fileEvent = "удален";
            string filePath = e.FullPath;
            RecordEntry(fileEvent, filePath);
        }

        private void RecordEntry(string fileEvent, string filePath)
        {
            lock (obj)
            {
                if (Directory.Exists(logFileDir))
                {
                    using (StreamWriter writer = new StreamWriter(logFileDir + "logs.txt", true))
                    {
                        writer.WriteLine(String.Format("{0} файл {1} был {2}",
                            DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), filePath, fileEvent));
                        writer.Flush();
                    }
                }
            }
        }
    }
}
