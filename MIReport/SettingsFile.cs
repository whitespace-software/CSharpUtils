using System;
using System.IO;
using Newtonsoft.Json;

namespace WSShared
{
    public class SettingsFile
    {
        public string root;
        public string bucket;
        public string tokenFile;
        public string renewableToken;

        public static SettingsFile Load( string filename )
        {
            if (!File.Exists(filename))
                throw new Exception( String.Format("No settings file {0}", filename));

            string settingsjson = File.ReadAllText(filename);
            SettingsFile settings = JsonConvert.DeserializeObject<SettingsFile>(settingsjson);
            if( settings.tokenFile.Trim() == String.Empty )
                throw new Exception(String.Format("tokenFile required in {0}", settings.tokenFile));
            string directory = Path.GetDirectoryName(filename);
            string fullfilename = Path.Combine(directory, settings.tokenFile);
            if (!File.Exists(fullfilename))
                throw new Exception(String.Format("No token file {0}", fullfilename));
            settings.renewableToken = File.ReadAllText(fullfilename);
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
    }
}
