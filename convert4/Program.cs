using System;
using System.IO;
using System.Net;
using System.Text.Json;

namespace convert4
{
    class Program
    {
        static void Main(string[] args)
        {

            if (args.Length != 2)
            {
                Console.WriteLine("Invalid args");
                return;
            }

            string baseCurrency = args[0];

            string exchangeCurrency = args[1];
            
            var URLString = $"https://v6.exchangerate-api.com/v6/66ad1c754e0867995844b973/pair/{baseCurrency}/{exchangeCurrency}";
            
            string json = "";
            
            try
            {
                json = new WebClient().DownloadString(URLString);
            }
            catch (WebException ex)
            {
                var webResponseCode = (ex.Response as HttpWebResponse)?.StatusCode;
                if (webResponseCode == HttpStatusCode.NotFound)
                {
                    json = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                }
            }
            
            JsonDocument jsonDocument = JsonDocument.Parse(json);

            var jsonDocumentRootElement = jsonDocument.RootElement;

            
            if (jsonDocumentRootElement.ValueKind == JsonValueKind.Object)
            {

                if (jsonDocumentRootElement.GetProperty("result").ToString() == "success")
                {

                    Console.WriteLine(jsonDocumentRootElement.GetProperty("conversion_rate").GetDouble());

                }
                else if (jsonDocumentRootElement.GetProperty("result").ToString() == "error")
                {

                    Console.WriteLine(jsonDocumentRootElement.GetProperty("error-type").GetString());

                }

            }

        }
    }
}
