using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.Text;
using System.Xml.XPath;
using System.Net.Http;
using System.Net;
using System.Net.Sockets;

namespace HttpClient
{
    // класс и его члены объявлены как public
    [Serializable]
    public class Input
    {
        public int K { get; set; }
        public decimal[] Sums { get; set; }
        public int[] Muls { get; set; }
        public Input()
        {

        }

    }
    [Serializable]
    public class Output
    {
        public decimal SumResult { get; set; }
        public int MulResult { get; set; }
        public decimal[] SortedInputs { get; set; }

        public Output()
        {
        }
        public Output(int K, decimal[] Sums, int[] Muls)
        {
            int j = 1;

            SumResult = Sums.Sum() * K;
            foreach (int i in Muls) j = j * i;
            MulResult = j;
            SortedInputs = Sums.Concat(Array.ConvertAll(Muls, x => (decimal)x)).ToArray();



            Array.Sort(SortedInputs);
        }

    }

    class Program
    {

        static public void StartServer(string prefix)
        {
           
            HttpListener server;
           
            server = new HttpListener();

          
          if (string.IsNullOrEmpty(prefix))
                throw new ArgumentException("prefix");

            server.Prefixes.Add(prefix);

            //запускаем север
            server.Start();

            string ll;
            string ee = string.Empty;
            //сервер запущен? Тогда слушаем входящие соединения
            while (server.IsListening)
            {
                //ожидаем входящие запросы
                HttpListenerContext context = server.GetContext();

                //получаем входящий запрос
                HttpListenerRequest request = context.Request;

                //обрабатываем POST запрос

                var rawUrl = request.RawUrl;
                var trimmedUrl = rawUrl.Trim(new char[] { '/' });
                var indexOf = trimmedUrl.IndexOf('?');
                var methodName = indexOf >= 0 ? trimmedUrl.Substring(0, indexOf) : trimmedUrl;
                if (request.HttpMethod == "POST")
                {                    
                    if (!request.HasEntityBody) return;
                    using (Stream body = request.InputStream)
                    {
                        using (StreamReader reader = new StreamReader(body))
                        {
                            string text = reader.ReadToEnd();                            
                         
                                             
                            if (methodName == "Stop")
                            {
                                server.Stop();
                            
                            }
                            if (methodName == "Ping")
                            {
                                Out(string.Empty, context);
                             
                            }
                          if      (methodName == "PostInputData")
                            {
                                byte[] bytes = new byte[request.ContentLength64];
                                using (Stream stream = request.InputStream)
                                {
                                    stream.Read(bytes, 0, (int)request.ContentLength64);
                                }
                                ll = Encoding.UTF8.GetString(bytes);
                                Input json = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Input>(ll);
                                Output go = new Output(json.K, json.Sums, json.Muls);
                                for (int i = 0; i < go.SortedInputs.Length; i++)
                                {
                                    go.SortedInputs[i] += 0.0m;
                                }
                                JavaScriptSerializer jss = new JavaScriptSerializer();
                                ee = jss.Serialize(go);
                            }
                            if (methodName == "GetAnswer")
                            {
                                
                                Out(ee, context);
                            }
                        }
                    }

                    
                }                       
            }
        }       
        
      static  public void Out(string da, HttpListenerContext context)
        {
            HttpListenerResponse response = context.Response;
            response.ContentType = "text/html; charset=UTF-8";
            byte[] buffer = Encoding.UTF8.GetBytes(da);
            response.ContentLength64 = buffer.Length;

            using (Stream output = response.OutputStream)
            {
                output.Write(buffer, 0, buffer.Length);
            }
        }
        static void Main(string[] args)
        {
                       
            string ad = "http://127.0.0.1";
            string port = Console.ReadLine();
            string go = string.Format("{0}:{1}/", ad, port);
            StartServer(go);                      
                       
        }

    }
}