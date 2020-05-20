using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace EchoBot.WebClientExtension
{
    public class WebClientExt : WebClient
    {
        public NameValueCollection PostParam { get; set; }
        public string NewConcatenatedURL { get; set; }

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest tmprequest = base.GetWebRequest(address);

            HttpWebRequest request = tmprequest as HttpWebRequest;

            if (request != null && PostParam != null && PostParam.Count > 0)
            {
                StringBuilder postBuilder = new StringBuilder();
                request.Method = "POST";
                //build the post string

                for (int i = 0; i < PostParam.Count; i++)
                {
                    postBuilder.AppendFormat("{0}={1}", Uri.EscapeDataString(PostParam.GetKey(i)),
                                             Uri.EscapeDataString(PostParam.Get(i)));
                    if (i < PostParam.Count - 1)
                    {
                        postBuilder.Append("&");
                    }
                }
                NewConcatenatedURL = tmprequest.RequestUri.ToString() + postBuilder.ToString();
                byte[] postBytes = Encoding.ASCII.GetBytes(postBuilder.ToString());
                request.ContentLength = postBytes.Length;
                request.ContentType = "application/x-www-form-urlencoded";

                var stream = request.GetRequestStream();
                stream.Write(postBytes, 0, postBytes.Length);
                stream.Close();
                stream.Dispose();

            }

            return tmprequest;
        }
    }
}
