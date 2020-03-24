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

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WSShared;
using System.Net.Http.Headers;

namespace EvidenceOfCover
{
    class Program
    {
        static void Main(string[] args)
        {
            RunAsync(args).GetAwaiter().GetResult();
        }

        static async Task RunAsync(string[] args)
        {
            if (args.Length < 2)
            {
                WSUtilities.PrintVersionMesssage("EvidenceOfCover", "1.0");
                Console.WriteLine("EvidenceOfCover.exe [settings file] [json template] [riskID]");
                Console.WriteLine("EvidenceOfCover.exe --example [settings file]");
                return;
            }
            if (args[0].ToLower() == "--example")
            {
                Console.WriteLine(WSSettings.WriteExample(args[1]));
                return;
            }
            if (args.Length < 3)
            {
                Console.WriteLine("EvidenceOfCover.exe [settings file] [json template] [riskID]");
                return;
            }
            WSSettings settings;
            try
            {
                settings = WSSettings.Load(args[0]);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR {0}", ex.Message);
                return;
            }

            string json = String.Empty, req = String.Empty;
            try
            {
                if (!File.Exists(args[1]))
                {
                    Console.WriteLine("No template file " + args[1]);
                    return;
                }
                String template = "";
                using( StreamReader sr = new StreamReader( args[1]))
                {
                    template = sr.ReadToEnd();
                }
                Regex rgx = new Regex(@"\[\[(?<heading>[^\]]*)\]\]");
                MatchCollection matches = rgx.Matches(template);
                WSAPIClient client = WSAPIClient.ForToken(settings);
                _ = await client.DoOIDC(settings);

                client = WSAPIClient.ForJSON(settings);
                foreach (Match m in matches)
                {
                    String heading = m.Groups["heading"].Value;
                    req = String.Format("/api/risks/{0}/lineitem/{1}/crlf", args[2], heading );
                    RisksLineItemCRLFResponse text = await client.DoGet<RisksLineItemCRLFResponse>( req );
                    Console.WriteLine( "{0} = {1}", heading, text.text );
                    String grail = "[[" + heading + "]]";
                    template = template.Replace(grail, text.text);
                }
                String pdffile = "tmp" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".pdf";

                // https://stackoverflow.com/questions/36625881/how-do-i-pass-an-object-to-httpclient-postasync-and-serialize-as-a-json-body
                var buffer = System.Text.Encoding.UTF8.GetBytes(template);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                req = "/pdfmake/";
                HttpResponseMessage responseMessage = await client.PostAsync(req, byteContent);
                var bytes = await responseMessage.Content.ReadAsByteArrayAsync();
                WriteFile(pdffile, bytes);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Last URL was {0}", req);
            }
        }

        static void WriteFile( string fullfilename, Byte[] bytes)
        {
            using (var fs = new FileStream(fullfilename, FileMode.Create, FileAccess.Write))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            Console.WriteLine("{0} written, {1} bytes", fullfilename, bytes.Length);
        }
    }

    public class RisksLineItemCRLFResponse {
        public String text;
    }
}
