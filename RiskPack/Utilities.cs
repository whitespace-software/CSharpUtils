using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WSShared
{
    public class Utilities
    {
        public static string MakeSafeFilename( string filename )
        {
            IList<char> invalidFileNameChars = Path.GetInvalidFileNameChars();
            StringBuilder result = new StringBuilder();
            foreach( var c in filename )
            {
                if (invalidFileNameChars.Contains(c))
                    result.Append('_');
                else
                    result.Append(c);
            }
            return result.ToString();
        }
    }
}
