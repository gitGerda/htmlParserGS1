using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using htmlParserGS1_service.components;

namespace htmlParserGS1_service
{
    public partial class Service1 : ServiceBase
    {
        Loger loger;
        public Service1()
        {
            InitializeComponent();
            this.CanStop = true;
            this.CanPauseAndContinue = true;
            this.AutoLog = true;
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                string pathToAnalyzeDir = System.Configuration.ConfigurationSettings.AppSettings["analyzeFolder"].ToString();

                if (Directory.Exists(pathToAnalyzeDir))
                {
                    string logFileDir = System.Configuration.ConfigurationSettings.AppSettings["logFileDir"].ToString();

                    if (Directory.Exists(logFileDir))
                    {
                        loger = new Loger(pathToAnalyzeDir,logFileDir);
                        Thread loggerThread = new Thread(new ThreadStart(loger.Start));
                        loggerThread.Start();
                    }
                }
            }
            catch
            {
                OnStop();
            }
            
        }

        protected override void OnStop()
        {
            try
            {
                loger.Stop();
                Thread.Sleep(1000);
            }
            catch { }
        }
    }
}
