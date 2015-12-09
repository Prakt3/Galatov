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
        
        static public string Web(string m, string a, string por, string da)
        {
            var p = (HttpWebRequest)WebRequest.Create(string.Format("{0}:{1}/{2}", a, por, m));
            p.Method = "POST";
            if (da!=null)
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(da);
                p.ContentLength = byteArray.Length;
                System.IO.Stream strmPostData = p.GetRequestStream();
                strmPostData.Write(byteArray, 0, byteArray.Length);
            }
            HttpWebResponse response = (HttpWebResponse)p.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            var dataStream = response.GetResponseStream();
            StreamReader read = new StreamReader(dataStream);
            string responseFromServer = read.ReadToEnd();
            reader.Close();
            dataStream.Close();
            response.Close();
            return responseFromServer;

        }
        static void Main(string[] args)
        {
            string ad = "http://127.0.0.1";
            string port = Console.ReadLine();
            string m1 = "Ping";
            string m2 = "GetInputData";
            string m3 = "WriteAnswer";

            while (Web(m1, ad, port, string.Empty).Length > 0)
            {
                //-------------------------------------------  
                string p = Web(m2, ad, port, string.Empty);
                Input json = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Input>(p);
                //------------------------------------          
                Output go = new Output(json.K, json.Sums, json.Muls);
                for (int i = 0; i < go.SortedInputs.Length; i++)
                {
                    go.SortedInputs[i] += 0.0m;
                }
                JavaScriptSerializer jss = new JavaScriptSerializer();
                string ee = jss.Serialize(go);
                Web(m3, ad, port, ee);

            }
        }
        }

    
}