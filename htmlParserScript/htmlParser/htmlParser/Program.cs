using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using htmlParser.components;
using htmlParser.models;


namespace htmlParser
{
    class Program
    {
        static List<parsedTable> parsedTablesList = new List<parsedTable>();
        static param scriptsParam = new param() { host = "",dir= "", pwd="" };
        static StringBuilder logs = new StringBuilder();
        static void Main(string[] args)
        {
            if(argsCheck.argumentsCheckFunc(args, ref scriptsParam))
            {
                parsedTablesList = parser.startParser(scriptsParam.dir, ref logs);

                if (parsedTablesList.Count != 0)
                {
                    string jsonDoc = jsonCreate.getJson(parsedTablesList);
                    sendToServer.sendData(jsonDoc, scriptsParam, ref logs);

                    logs.AppendLine("##" + DateTime.Now.ToString() + " - End ##");
                    logs.AppendLine("-------------------------------------------------------------------------------");
                }


                if (logs != null)
                {
                    try
                    {
                        File.AppendAllText(scriptsParam.dir+"logs.txt",logs.ToString());
                    }
                    catch
                    {
                        Console.WriteLine("Error: Не удалось записать логи!");
                    }
                }
            }

            /*serverConnectionParam servParam = new serverConnectionParam();
            servParam.servName = "https://webhook.site/6614269a-329e-475e-943a-78ab2baf3850";
            servParam.authorizationCode = "0KHQuNC90YXRgNC+0L3QuNC30LDRhtC40Y86YWRtaW4wMDA=";*/
            //http://192.168.0.20/ERP_rasplav_copy/hs/gs/post/


        }
    }
}
