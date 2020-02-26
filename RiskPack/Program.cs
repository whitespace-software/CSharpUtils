/*
MIT License

Copyright (c) 2020 Whitespace Software Limited

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
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
                WSUtilities.PrintVersionMesssage("RiskPack", "1.0");
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
            string json = String.Empty, req = String.Empty;
            try
            {
                string folder = WSUtilities.MakeSafeFilename(args[1]) + DateTime.Now.ToString("_ddMMyy_HHmmss");
                Directory.CreateDirectory(folder);

                req = "/sync/" + settings.bucket + "/_oidc_refresh?refresh_token=" + settings.renewableToken;
                HttpResponseMessage response = await client.GetAsync(req);
                json = await response.Content.ReadAsStringAsync();

                WSOIDCResult oidc = JsonConvert.DeserializeObject<WSOIDCResult>(json);
                settings.sessionToken = oidc.id_token;

                req = String.Format("/export/pdf/{0}", args[1]);
                client = WSAPIClient.ForPDF(settings);

                var bytes = await client.GetByteArrayAsync(req);
                WriteFile(folder, args[1] + ".pdf", bytes);

                client = WSAPIClient.ForJSON(settings);
                req = String.Format("/api/attachments/{0}", MakeATCH(args[1]) );
                json = await client.GetStringAsync(req);

                Dictionary<String, WSAttachment> dict = JsonConvert.DeserializeObject<Dictionary<String, WSAttachment>>(json);
                foreach (string key in dict.Keys)
                {
                    WSAttachment att = dict[key];
                    client = WSAPIClient.ForMIMEType(settings, att.content_type);
                    req = string.Format("/api/attachments/{0}/{1}", MakeATCH(args[1]), key);
                    bytes = await client.GetByteArrayAsync(req);
                    WriteFile(folder, key, bytes);
                }
            } 
            catch( Exception ex )
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Last URL was {0}", req);
            }
        }

        static void WriteFile( string folder, string filename, Byte[] bytes )
        {
            string fullfilename = Path.Combine(folder, WSUtilities.MakeSafeFilename(filename));
            using (var fs = new FileStream(fullfilename, FileMode.Create, FileAccess.Write))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            Console.WriteLine("{0} written, {1} bytes", fullfilename, bytes.Length);
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
