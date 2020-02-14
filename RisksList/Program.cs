using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace RisksList
{
    public class OIDCResult
    {
        public string id_token;
        public string session_id;
        public string name;
    }

    public class RisksResult
    {
        public string id;
        public string umr;
        public string Status;
        public string InsuredName;
    }

    class Program
    {
        static HttpClient client = new HttpClient();

        static void Main(string[] args)
        {
            RunAsync(args).GetAwaiter().GetResult();
        }

        static async Task RunAsync(string[] args)
        {
            if( args.Length < 1 )
            {
                Console.WriteLine("requires more arguments");
                return;
            }
            SettingsFile settings;
            try
            {
                settings = SettingsFile.Load(args[0]);
            }
            catch( Exception ex )
            {
                Console.WriteLine("ERROR {0}", ex.Message);
                return;
            }
            client.BaseAddress = new Uri(settings.root);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string json = String.Empty, req = String.Empty;

            try
            {
                req = "/sync/" + settings.bucket + "/_oidc_refresh?refresh_token=" + settings.renewableToken;
                HttpResponseMessage response = await client.GetAsync(req);
                json = await response.Content.ReadAsStringAsync();

                OIDCResult oidc = JsonConvert.DeserializeObject<OIDCResult>(json);

                req = "/api/risks";
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", oidc.id_token);

                response = await client.GetAsync(req);
                json = await response.Content.ReadAsStringAsync();

                List<RisksResult> list = JsonConvert.DeserializeObject<List<RisksResult>>(json);
                Console.WriteLine(list.Count + " risks returned");
                foreach (var item in list)
                {
                    Console.WriteLine("{0} {1} {2} {3}", item.InsuredName, item.Status, item.umr, item.id);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(req);
                Console.WriteLine(json);
            }
        }
    }
}