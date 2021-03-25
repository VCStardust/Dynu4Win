using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dynu4Win
{
    class Program
    {
        
        public static async Task Main()
        {
            string CurrIP;
            string ProxyServer = null;
            string ResContent = null;
            int RetryTimes = 0;
            bool Faulted;

            HttpWebRequest CheckIPRequest = (HttpWebRequest)WebRequest.Create("https://ipcheckv6.dynu.com/");
            CheckIPRequest.Proxy = new WebProxy(ProxyServer);

            do
            {
                try
                {
                    HttpWebResponse CheckIPResponse = (HttpWebResponse)CheckIPRequest.GetResponse();
                    Stream DataStream = CheckIPResponse.GetResponseStream();
                    StreamReader ReadData = new(DataStream);
                    ResContent = ReadData.ReadToEnd();
                    CheckIPResponse.Close();
                    ReadData.Close();
                    Faulted = false;
                }
                catch
                {
                    Faulted = true;
                    RetryTimes++;
                }
            }
            while (Faulted);


            Console.WriteLine(ResContent + " , retried " + RetryTimes + " time(s).");
            CurrIP = ResContent.Remove(0, 20);

            RetryTimes = 0;
            Console.WriteLine("Enter your password below. End with Enter.");
            string Password = Console.ReadLine();
            Console.WriteLine("Enter the hostname you want to update.");
            string Hostname = Console.ReadLine();

            byte[] TmpMD5 = new MD5CryptoServiceProvider().ComputeHash(ASCIIEncoding.ASCII.GetBytes(Password));
            string PasswordMD5 = ByteArrayToString(TmpMD5);

            string UpdateIPURL = "https://api.dynu.com/nic/update?hostname=" + Hostname + "&myip=no&myipv6=" + CurrIP + "&password=" + PasswordMD5;
            Console.WriteLine(UpdateIPURL);
 
            HttpClient client = new();
            do
            {
                try
                {
                    string responseBody = await client.GetStringAsync(UpdateIPURL);
                    Faulted = false;
                }
                catch
                {
                    Faulted = true;
                    RetryTimes++;
                }
            }
            while (Faulted);

            Console.WriteLine("Update succeed, retried " + RetryTimes + " times.");
            Thread.Sleep(10000);



            static string ByteArrayToString(byte[] arrInput)
            {
                int i;
                StringBuilder sOutput = new(arrInput.Length);
                for (i = 0; i < arrInput.Length; i++)
                {
                    sOutput.Append(arrInput[i].ToString("X2"));
                }
                return sOutput.ToString();
            }
        }
    }
}
