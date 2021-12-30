using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using htmlParserGS1_service.parser.models;
using htmlParserGS1_service.parser.components;

namespace htmlParserGS1_service.parser.components
{
    public class sendToServer
    {
        public int sendData(string data, param serverParam, ref StringBuilder logs)
        {
            HttpWebRequest request;
            HttpWebResponse httpResponse;

            try
            {
                logs.AppendLine();
                logs.AppendLine("   # " + DateTime.Now.ToString() + "--> Sending data to server...");

                request = (HttpWebRequest)WebRequest.Create(serverParam.host);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("Authorization", "Basic " + serverParam.pwd);

                using (var streamWriter = new System.IO.StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(data);
                }

                httpResponse = (HttpWebResponse)request.GetResponse();

                
                logs.AppendLine("   # " + DateTime.Now.ToString() + "--> Status code : [" + httpResponse.StatusCode.ToString() + "]");
            }
            catch (Exception ex)
            {
                logs.AppendLine("   # " + DateTime.Now.ToString() + "--> Sending errors {" + ex.Message + "}");
                logs.AppendLine();
                //Console.WriteLine(ex.Message);
                return 0;
            }

            try
            {
                request.Abort();
                httpResponse.Close();
            }
            catch { }


            logs.AppendLine();

            return 1;
        }
    }
}
