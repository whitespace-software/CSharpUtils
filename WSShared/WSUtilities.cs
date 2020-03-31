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
using System.Text;

namespace WSShared
{
    public class WSUtilities
    {
        public static void PrintVersionMesssage( string appname, string version )
        {
            Console.WriteLine("{0} version {1} (c)2020 Whitespace Software Limited", appname, version);
            Console.WriteLine("For conditions of use see https://github.com/whitespace-software/CSharpUtils/blob/master/LICENSE");
        }
        public static string MakeSafeFilename( string filename )
        {
            IList<char> invalidFileNameChars = Path.GetInvalidFileNameChars();
            StringBuilder result = new StringBuilder();
            foreach( var c in filename )
            {
                if ( c == ':' || invalidFileNameChars.Contains(c))
                    result.Append('_');
                else
                    result.Append(c);
            }
            return result.ToString();
        }

        public static string GetRoot( String docid )
        {
            var pos = docid.IndexOf("::");
            if (pos >= 0)
                return docid.Substring(0, pos);
            return docid;
        }
    }
}
