using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    public class EmailURI : Uri
    {
        public const string EMAIL_URI_SCHEME = "mailto";

        public EmailURI(string uriString) : base(uriString)
        {
            if (!this.Scheme.Equals(EMAIL_URI_SCHEME, StringComparison.InvariantCultureIgnoreCase))
                throw new ArgumentException("Not a e-mail URI");
        }
    }
}
