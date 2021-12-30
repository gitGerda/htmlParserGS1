using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using htmlParserGS1_service.parser.models;
using htmlParserGS1_service.parser.components;
using System.Threading;

namespace htmlParserGS1_service.parser
{
    public class General
    {
        List<parsedTable> parsedTablesList = new List<parsedTable>();
        param scriptsParam = new param() { host = "", dir = "", pwd = "" };
        StringBuilder logs = new StringBuilder();
        string filePath;

        public General(string g)
        {
            filePath = g;
        }

        public void startParser()
        {
            if(argsCheck.argumentsCheckFunc(ref scriptsParam, ref logs))
            {
                //parser.components.parser htmlParser = new parser.components.parser();
                Thread.Sleep(3000);
                parser.components.parser g = new parser.components.parser();
                parsedTablesList = g.goParser(filePath, ref logs);

                if (parsedTablesList.Count != 0)
                {
                    jsonCreate jsCr = new jsonCreate();
                    string jsonDoc = jsCr.getJson(parsedTablesList);

                    sendToServer sendToS = new sendToServer();
                    sendToS.sendData(jsonDoc, scriptsParam, ref logs);

                    logs.AppendLine("##" + DateTime.Now.ToString() + " - End Parsing ##");
                    logs.AppendLine("---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                }

                if (logs != null)
                {
                    try
                    {
                        File.AppendAllText(scriptsParam.logFile + "logs.txt", logs.ToString());
                    }
                    catch
                    {
                        //Console.WriteLine("Error: Не удалось записать логи!");
                    }
                }
            }
            else
            {
                if(Directory.Exists(scriptsParam.logFile) && logs!=null)
                {
                    try
                    {
                        File.AppendAllText(scriptsParam.logFile + "logs.txt", logs.ToString());
                    }
                    catch { }
                }
            }
        }
    }
}
