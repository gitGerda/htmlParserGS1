using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using htmlParserGS1_service.parser.models;

namespace htmlParserGS1_service.parser.components
{
    public static class argsCheck
    {
        public static bool argumentsCheckFunc(ref param paramObj, ref StringBuilder logs)
        {
            bool result = true;
            try
            {
                paramObj.dir = ConfigurationSettings.AppSettings["analyzeFolder"].ToString();
                paramObj.host = ConfigurationSettings.AppSettings["serverAddress"].ToString();
                paramObj.pwd = ConfigurationSettings.AppSettings["serverAuthData"].ToString();
                paramObj.logFile = ConfigurationSettings.AppSettings["logFileDir"].ToString();

                if(!Directory.Exists(paramObj.dir))
                {
                    result = false;
                    logs.AppendLine("   # " + DateTime.Now.ToString() + @"--> ERROR: Ошибка чтения параметра [analyzeFolder]. Возможно задан некорректный путь или отсутсвуют нужные права доступа.");
                }
                if(!Directory.Exists(paramObj.logFile))
                {
                    result = false;
                    logs.AppendLine("   # " + DateTime.Now.ToString() + @"--> ERROR: Ошибка чтения параметра [logFileDir]. Возможно задан некорректный путь или отсутсвуют нужные права доступа.");
                }

                if(String.IsNullOrEmpty(paramObj.host) || String.IsNullOrEmpty(paramObj.pwd))
                {
                    result = false;
                    logs.AppendLine("   # " + DateTime.Now.ToString() + @"--> ERROR: Ошибка чтения параметра [serverAddress,serverAuthData]");
                }
            }
            catch
            {
                logs.AppendLine("   # " + DateTime.Now.ToString() + @"--> ERROR: Проверьте файл ""App.config."".");
                result = false;
            }

            return result;
        }
    }
}
