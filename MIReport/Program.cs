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

namespace MIReport
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
                WSUtilities.PrintVersionMesssage("MIReport", "1.0");
                Console.WriteLine("MIReport.exe [settings file] [output file]");
                Console.WriteLine("MIReport.exe --example [settings file]");
                return;
            }
            if( args[0].ToLower() == "--example" )
            {
                Console.WriteLine(WSSettings.WriteExample(args[1]));
                return;
            }
            try
            {
                WSSettings settings = WSSettings.Load(args[0]);
                WSAPIClient client = WSAPIClient.ForToken(settings);
                _ = await client.DoOIDC(settings);

                client = WSAPIClient.ForMIMEType(settings, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                var bytes = await client.GetByteArrayAsync( "/api/mireport");
                using( StreamWriter sw = new StreamWriter( args[1]))
                {
                    sw.Write(bytes);
                    Console.WriteLine("{0} bytes written to {1}", bytes.Length, args[1]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }
    }
}
