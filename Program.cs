using System;
using System.IO;
using System.Net;

namespace Dynu4Win
{
    class Program
    {
        static void Main()
        {
            string CurrIP;
            string ProxyServer = null;
            string ResContent;
            int RetryTimes = 0;
            bool IsSucceed = false;

            HttpWebRequest CheckIPRequest = (HttpWebRequest)WebRequest.Create("http://[2001:19f0:5c01:134:5400:ff:fe36:33bf]/");
            CheckIPRequest.Proxy = new WebProxy(ProxyServer);
            do
            {

                HttpWebResponse CheckIPResponse = (HttpWebResponse)CheckIPRequest.GetResponse();
                Stream DataStream = CheckIPResponse.GetResponseStream();
                StreamReader ReadData = new(DataStream);
                ResContent = ReadData.ReadToEnd();
                CheckIPResponse.Close();
                ReadData.Close();
                

            }
            while (true);

            Console.WriteLine(ResContent + " , retried " + RetryTimes + " time(s).");
            CurrIP = ResContent.Remove(0, 20);
            Console.WriteLine(CurrIP);



        }
    }
}
