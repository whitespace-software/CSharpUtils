using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WSShared;

namespace RiskPack
{
    class Program
    {
        static void Main(string[] args)
        {
            RunAsync(args).GetAwaiter().GetResult();
            
        }

        static async Task RunAsync( string [] args )
        {
            if( args.Length < 2 )
            {
                Console.WriteLine("riskpack.exe [settings file] [riskID]");
                Console.WriteLine("riskpack.exe --example [settings file]");
                return;
            }
            if( args[0].ToLower() == "--example")
            {
                WSSettings example = WSSettings.MakeExample();
                try
                {
                    using( StreamWriter sw = new StreamWriter(args[1]))
                    {
                        String str = JsonConvert.SerializeObject(example,Formatting.Indented);
                        sw.WriteLine(str);
                        Console.WriteLine("{0} written", args[1]);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERROR {0}", ex.Message);
                    return;
                }
                return;
            }
            WSSettings settings;
            try
            {
                settings = WSSettings.Load(args[0]);
            }
            catch( Exception ex )
            {
                Console.WriteLine("ERROR {0}", ex.Message);
                return;
            }
            WSAPIClient client = WSAPIClient.ForToken(settings);
            string json = String.Empty, req = String.Empty, folder = String.Empty;
            try
            {
                folder = Utilities.MakeSafeFilename(args[1]) + DateTime.Now.ToString("_ddMMyy_HHmmss");
                Directory.CreateDirectory(folder);

                req = "/sync/" + settings.bucket + "/_oidc_refresh?refresh_token=" + settings.renewableToken;
                HttpResponseMessage response = await client.GetAsync(req);
                json = await response.Content.ReadAsStringAsync();

                WSOIDCResult oidc = JsonConvert.DeserializeObject<WSOIDCResult>(json);
                settings.sessionToken = oidc.id_token;

                req = "/export/pdf/" + args[1];
                client = WSAPIClient.ForPDF(settings);

                var bytes = await client.GetByteArrayAsync(req);
                string filename = Path.Combine(folder, Utilities.MakeSafeFilename(args[1] + ".pdf"));
                using (var fs = new FileStream(filename, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(bytes, 0, bytes.Length);
                }
                Console.WriteLine("{0} written, {1} bytes", filename, bytes.Length);

                client = WSAPIClient.ForJSON(settings);
                req = String.Format("/api/attachments/{0}", MakeATCH(args[1]) );
                Console.WriteLine(req);
                json = await client.GetStringAsync(req);
                Console.WriteLine(json);

                Dictionary<String, WSAttachment> dict = JsonConvert.DeserializeObject<Dictionary<String, WSAttachment>>(json);
                foreach (string key in dict.Keys)
                {
                    WSAttachment att = dict[key];
                    Console.WriteLine("{0} {1}", key, att.content_type);

                    client = WSAPIClient.ForMIMEType(settings, att.content_type);
                    req = string.Format("/api/attachments/{0}/{1}", MakeATCH(args[1]), key);
                    bytes = await client.GetByteArrayAsync(req);
                    filename = Path.Combine(folder, Utilities.MakeSafeFilename(key));
                    using (var fs = new FileStream(filename, FileMode.Create, FileAccess.Write))
                    {
                        fs.Write(bytes, 0, bytes.Length);
                    }
                    Console.WriteLine("{0} written, {1} bytes", filename, bytes.Length);

                }
            } 
            catch( Exception ex )
            {
                Console.WriteLine(ex.Message);
            }

        }

        static string MakeATCH( string riskid )
        {
            int pos = riskid.IndexOf("::");
            if (pos > 0)
                return riskid.Substring(0, pos) + "::ATCH";
            else
                return riskid + "::ATCH";
        }
    }
}
