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
                Console.WriteLine("HelloWorld.exe --example [settings file]");
                return;
            }
            if (args[0].ToLower() == "--example")
            {
                WSSettings example = WSSettings.MakeExample();
                try
                {
                    using (StreamWriter sw = new StreamWriter(args[1]))
                    {
                        String str = JsonConvert.SerializeObject(example, Formatting.Indented);
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
            try
            {
                WSSettings settings = WSSettings.Load(args[0]);
                WSAPIClient client = WSAPIClient.ForToken(settings);
                string req = "/sync/" + settings.bucket + "/_oidc_refresh?refresh_token=" + settings.renewableToken;
                HttpResponseMessage response = await client.GetAsync(req);
                string json = await response.Content.ReadAsStringAsync();

                WSOIDCResult oidc = JsonConvert.DeserializeObject<WSOIDCResult>(json);
                settings.sessionToken = oidc.id_token;

                client = WSAPIClient.ForJSON(settings);
                response = await client.GetAsync("/api/user/myDetails");
                json = await response.Content.ReadAsStringAsync();
                Console.WriteLine(json);
                WSUserMyDetailsResult user = JsonConvert.DeserializeObject<WSUserMyDetailsResult>(json);
                Console.WriteLine("username:{0} companyId:{1} uniqueID:{2} teams:{3}",
                    user.username, user.companyId, user.uniqueID, user.teams.Count );
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
