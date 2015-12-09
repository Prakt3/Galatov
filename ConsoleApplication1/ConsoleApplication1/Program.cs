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
namespace Serialization
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
       
        static void Main(string[] args)
        {
           // объект для сериализации
              string serializeType = Console.ReadLine();
            string obj = Console.ReadLine();

            if (serializeType == "Xml")
            {
                
                XmlSerializer x = new XmlSerializer(typeof(Input));
                Input myTest = (Input)x.Deserialize(new StringReader(obj));
                Output go = new Output(myTest.K, myTest.Sums, myTest.Muls);

                //XmlSerializer y = new XmlSerializer(go.GetType());
                XmlSerializer serializer = new XmlSerializer(typeof(Output));
                StringWriter writer = new StringWriter();
                serializer.Serialize(writer, go);
                string serializedXML = writer.ToString();
                serializedXML = serializedXML.Remove(0, serializedXML.IndexOf("<Out"));
                serializedXML = serializedXML.Remove(7, serializedXML.IndexOf(">")-7);
                serializedXML = serializedXML.Replace(" ", ""); 
                    serializedXML = serializedXML.Replace(System.Environment.NewLine, "");
                Console.Write(serializedXML);
                // y.Serialize(Console.Out, go);


            }
            if (serializeType == "Json")
            {
                Input json = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Input>(obj);
                Output go = new Output(json.K, json.Sums, json.Muls);
                for (int i = 0; i < go.SortedInputs.Length; i++)
                {
                    go.SortedInputs[i] += 0.0m;
                }
               
                JavaScriptSerializer jss = new JavaScriptSerializer();

                string ee = jss.Serialize(go);
                Console.WriteLine(ee);
            }
            Console.ReadLine();
        }
        
    }
}