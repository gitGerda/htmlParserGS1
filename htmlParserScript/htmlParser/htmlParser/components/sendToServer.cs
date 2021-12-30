using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using htmlParser.models;

namespace htmlParser.components
{
    public static class sendToServer
    {
        public static int sendData(string data,param serverParam, ref StringBuilder logs)
        {
            try
            {
                logs.AppendLine();
                logs.AppendLine("   # " + DateTime.Now.ToString() + "--> Sending data to server...");

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serverParam.host);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("Authorization", "Basic " + serverParam.pwd);

                using (var streamWriter = new System.IO.StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(data);
                }

                HttpWebResponse httpResponse = (HttpWebResponse)request.GetResponse();

                logs.AppendLine("   # " + DateTime.Now.ToString() + "--> Status code : ["+httpResponse.StatusCode.ToString()+"]");
            }
            catch (Exception ex)
            {
                logs.AppendLine("   # " + DateTime.Now.ToString() + "--> Sending errors {"+ex.Message+"}");
                logs.AppendLine();
                //Console.WriteLine(ex.Message);
                return 0;
            }

            logs.AppendLine();

            return 1;
        }
    }
}
