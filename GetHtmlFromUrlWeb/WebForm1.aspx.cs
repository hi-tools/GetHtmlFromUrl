using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace GetHtmlFromUrlWeb
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Encod(Download("http://www.sharj.ir/"));
            //string html = Download("http://findbazar.com/6000/");
            Session.Add("Test", "123");
        }
        string Download(string urlAddress)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;
                //Encoding encode = System.Text.Encoding.GetEncoding("windows-1255");
                if (response.CharacterSet == null)
                {
                    readStream = new StreamReader(receiveStream,Encoding.UTF8 );
                }
                else
                {
                    readStream = new StreamReader(receiveStream, Encoding.UTF8);
                }

                string data = readStream.ReadToEnd();

                response.Close();
                readStream.Close();
                return data;
            }
            return "";
        }
        string Encod(string text)
        {
            // This could mess up HTML.
            //string text = "you & me > them"; // 1

            // Replace > with >
            string htmlEncoded = Server.HtmlEncode(text); // 2

            // Now has the > again.
            string original = Server.HtmlDecode(htmlEncoded); // 3

            // This is how you can access the Server in any class.
            string alsoEncoded = HttpContext.Current.Server.HtmlEncode(text); // 4

            StringWriter stringWriter = new StringWriter();
            using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
            {
                // Write a DIV with encoded text.
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.WriteEncodedText(text);
                writer.RenderEndTag();
            }
            string html = stringWriter.ToString(); // 5
            return html;
        }
    }
}