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
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WSShared;

namespace WSShared
{
    public class WSAPIClient : HttpClient
    {
        public static WSAPIClient ForToken(WSSettings settings)
        {
            var client = new WSAPIClient();
            client.BaseAddress = new Uri(settings.root);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }
        public static WSAPIClient ForJSON(WSSettings settings)
        {
            var client = new WSAPIClient();
            client.BaseAddress = new Uri(settings.root);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", settings.sessionToken);
            return client;
        }
        public static WSAPIClient ForPDF(WSSettings settings)
        {
            return WSAPIClient.ForMIMEType(settings, "application/pdf");
        }
        public static WSAPIClient ForMIMEType(WSSettings settings, string mimetype)
        {
            var client = new WSAPIClient();
            client.BaseAddress = new Uri(settings.root);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mimetype));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", settings.sessionToken);
            return client;
        }

        public async Task<String> DoOIDC(WSSettings settings)
        {
            HttpResponseMessage response = await this.GetAsync(settings.MakeOIDC_URL());
            string json = await response.Content.ReadAsStringAsync();

            WSOIDCResult oidc = JsonConvert.DeserializeObject<WSOIDCResult>(json);
            settings.sessionToken = oidc.id_token;

            return json;
        }

        public async Task<String> DoGetJSON(string url)
        {
            HttpResponseMessage response = await this.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<T> DoGet<T>(string url)
        {
            string json = await this.DoGetJSON(url);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
