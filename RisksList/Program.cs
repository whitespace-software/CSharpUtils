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
using System.Threading.Tasks;
using WSShared;

namespace RisksList
{
    class Risk
    {
        public string id;
        public string rev;
        public string Status;
        public string InsuredName;
        public string type;
        public string createdAt;
        public string updatedAt;
        public string umr;
        public string placingBrokerChannel;
        public List<string> channels;
    }
    class Program
    {
        static void Main(string[] args)
        {
            RunAsync(args).GetAwaiter().GetResult();
        }

        static async Task RunAsync(string[] args)
        {
            if (args.Length < 1)
            {
                WSUtilities.PrintVersionMesssage("RisksList", "1.0");
                Console.WriteLine("RisksList.exe [settings file]");
                Console.WriteLine("RisksList.exe [settings file] ID (value)");
                Console.WriteLine("RisksList.exe [settings file] UMR (value)");
                Console.WriteLine("RisksList.exe [settings file] STATUS (value)");
                Console.WriteLine("RisksList.exe [settings file] INSUREDNAME (value)");
                Console.WriteLine("RisksList.exe --example [settings file]");
                return;
            }
            try
            {
                if (args[0].ToLower() == "--example" && args.Length > 1)
                {
                    Console.WriteLine(WSSettings.WriteExample(args[1]));
                    return;
                }


                Filter filter = new Filter( args );
                
                WSSettings settings = WSSettings.Load(args[0]);
                WSAPIClient client = WSAPIClient.ForToken(settings);
                _ = await client.DoOIDC(settings);

                client = WSAPIClient.ForJSON(settings);
                List<Risk> risks = await client.DoGet<List<Risk>>("/api/risks");

                Console.WriteLine("{0} risks loaded", risks.Count );
                foreach( var risk in risks )
                {
                    if (filter.Check(risk))
                        Console.WriteLine(String.Join(" : ",
                            new String[] { risk.id, risk.Status, risk.umr, risk.InsuredName }));
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }
    }

    class Filter {
        string operation = String.Empty;
        string grail = String.Empty;

        public Filter(string[] args)
        {
            if (args.Length < 3)
                return;
            var op = args[1].ToLower();
            if (op == "umr" || op == "id" || op == "status" || op == "insuredname" )
            {
                operation = op;
                grail = args[2].ToLower();
            }
            else
            {
                throw new Exception("Unrecognised filter operation " + args[2]);
            }
        }

        public bool Check( Risk risk )
        {
            if (operation == String.Empty)
                return true;
            if (operation == "id")
                return risk.id.ToLower().Contains(grail);
            if (operation == "umr")
                return risk.umr.ToLower().Contains(grail);
            if (operation == "status")
                return risk.Status.ToLower().Contains(grail);
            if (operation == "insuredname")
                return risk.InsuredName.ToLower().Contains(grail);
            return false;
        }
    }
}

