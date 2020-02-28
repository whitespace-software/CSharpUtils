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
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Standalone
{
    class OIDCResult {
        public string id_token;
    }

    class Program
    {
        static void Main(string[] args)
        {
            RunAsync().GetAwaiter().GetResult();
        }

        static async Task RunAsync()
        {
            var root = "https://sandbox.whitespace.co.uk";
            var bucket = "sandboxbucket";
            var renewableToken = "";

            var client = Program.ForToken( root );
            HttpResponseMessage response = await client.GetAsync(String.Format("/sync/{0}/_oidc_refresh?refresh_token={1}", bucket, renewableToken));
            string json = await response.Content.ReadAsStringAsync();
            var oidc_result = JsonConvert.DeserializeObject<OIDCResult>(json);

            client = Program.ForJSON(root, oidc_result.id_token);
            response = await client.GetAsync( "/api/user/myDetails" );
            json = await response.Content.ReadAsStringAsync();
            Console.WriteLine(json);

        }

        public static HttpClient ForToken( string root )
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri( root);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }
        public static HttpClient ForJSON(string root, string token )
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(root);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token );
            return client;
        }

    }
}
