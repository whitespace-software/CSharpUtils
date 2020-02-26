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

namespace HelloWorld
{
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
                WSUtilities.PrintVersionMesssage("HelloWorld", "1.0");
                Console.WriteLine("HelloWorld.exe [settings file]");
                return;
            }
            try
            {
                WSSettings settings = WSSettings.Load(args[0]);
                WSAPIClient client = WSAPIClient.ForToken(settings);
                _ = await client.DoOIDC(settings);

                client = WSAPIClient.ForJSON(settings);
                WSUserMyDetailsResult user = await client.DoGet<WSUserMyDetailsResult>("/api/user/myDetails");

                Console.WriteLine("username:{0} companyId:{1} uniqueID:{2} {3} {4}",
                    user.username, user.companyId, user.uniqueID, user.teams.Count, user.teams.Count == 1 ? "team" : "teams" );
                foreach(var t in user.teams)
                    Console.WriteLine("teamId:{0} name:{1} channel:{2}", t.teamId, t.name, t.channel);
            }
            catch( Exception ex )
            {
                Console.WriteLine(ex.Message);

            }
        }
    }
}
