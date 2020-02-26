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
using System.IO;
using Newtonsoft.Json;

namespace WSShared
{
    public class WSSettings
    {
        public string root;
        public string bucket;
        public string renewableToken;
        private string _session_token;

        public string sessionToken
        {
            get { return _session_token; }
            set { _session_token = value; }
        }

        public static WSSettings MakeExample()
        {
            var result = new WSSettings();
            result.root = "https://sandbox.whitespace.co.uk";
            result.bucket = "sandboxbucket";
            result.renewableToken = "Login to the platform and copy from the user settings panel, replacing this text";
            return result;
        }
        public static WSSettings Load( string filename )
        {
            if (!File.Exists(filename))
                throw new Exception( String.Format("No settings file {0}", filename));

            string settingsjson = File.ReadAllText(filename);
            WSSettings settings = JsonConvert.DeserializeObject<WSSettings>(settingsjson);
            settings.CheckValid();
            return settings;
        }

        public void CheckValid()
        {
            if (this.root.Trim() == String.Empty)
                throw new Exception("Settings root may not be empty");
            if (this.bucket.Trim() == String.Empty)
                throw new Exception("Settings bucket may not be empty");
        }

        public string MakeOIDC_URL()
        {
            return "/sync/" + this.bucket + "/_oidc_refresh?refresh_token=" + this.renewableToken;
        }
    }
}
