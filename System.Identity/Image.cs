using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace System.Identity
{
    public abstract class Image
    {
        public abstract Stream Data { get; }
    }

    public class MemoryImage : Image
    {
        public MemoryImage(string byte64Data)
        {
            byte[] data = Convert.FromBase64String(byte64Data);
            this.data = new MemoryStream(data);
        }
        public MemoryImage(byte[] data) { this.data = new MemoryStream(data); }

        private readonly MemoryStream data;
        public override Stream Data => this.data;
    }

    public class RemoteImage : Image
    {
        public RemoteImage(Uri uri)
        {
            this.Uri = uri;
            this.webRequestData = WebRequest.Create(uri).GetResponse().GetResponseStream();
        }

        public readonly Uri Uri;
        private readonly Stream webRequestData;
        public override Stream Data => throw new NotImplementedException();
    }
}
