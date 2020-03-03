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
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WSShared;

namespace VerifyAttachments
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
                WSUtilities.PrintVersionMesssage("VerifyAttachments", "1.0");
                Console.WriteLine("VerifyAttachments.exe [settings file] [IC]");
                Console.WriteLine("Gets the number of channels in IC::ATCH and looks for more channels in /risks/root items");
                return;
            }
            try
            {
                WSSettings settings = WSSettings.Load(args[0]);
                WSAPIClient client = WSAPIClient.ForToken(settings);
                _ = await client.DoOIDC(settings);

                client = WSAPIClient.ForJSON(settings);
                string url = String.Format("/api/documents/{0}", args[1] + "::ATCH");
                PlatformDocument doc = await client.DoGet<PlatformDocument>(url);
                Console.WriteLine("{0} has {1} channel(s)", doc._id, doc.channels.Count);

                url = string.Format("/api/risks/root/{0}", args[1]);
                List<RisksRootResult> risksRootResult = await client.DoGet<List<RisksRootResult>>(url);
                Console.WriteLine("{0} results from {1}", risksRootResult.Count, url);
                foreach (var r in risksRootResult)
                {
                    string prefix = "";
                    if (r.channels.Count > doc.channels.Count)
                        prefix = "ERROR: ";
                    Console.WriteLine("{0}{1} has {2} channel(s)", prefix, r.id, r.channels.Count);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }
        }
    }
}
