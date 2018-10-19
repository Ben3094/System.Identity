using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    public class URLUri : Uri
    {
        public const string URL_URI_PREFIX = "http";

        public URLUri(string uri) : base(uri)
        {
            if (!uri.StartsWith(URL_URI_PREFIX, StringComparison.InvariantCultureIgnoreCase))
                throw new ArgumentException("Not an URL URI");
        }
    }
}
