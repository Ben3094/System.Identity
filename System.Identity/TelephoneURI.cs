using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    public class TelephoneUri : Uri
    {
        public const string TEL_URI_PREFIX = "tel";

        public TelephoneUri(string uri) : base(uri)
        {
            if (!uri.StartsWith(TEL_URI_PREFIX, StringComparison.InvariantCultureIgnoreCase))
                throw new ArgumentException("Not a telephone URI");
        }
    }
}
