using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
//using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace GetHtmlFromUrl
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            //InternetSetCookie(url, "09363306252", "g12zom5lnou2rewqlsxsyo3m");
            wb.Navigate(url);
            wb.DocumentCompleted += Wb_DocumentCompleted;
        }
        //string url = "http://senf.ir/Company/2642448/%D8%B5%D9%81%D8%A7";
        string url = "http://senf.ir/";
        private void Wb_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            richTextBox1.Text = wb.Document.Body.InnerText;
            var temp = wb.Document.GetElementsByTagName("table");
            if (temp.Count == 0)
            {
                return;
            }
            var Text = temp[0].OuterHtml;
        }


        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool InternetSetCookie(string lpszUrlName, string lpszCookieName, string lpszCookieData);


        //private void usingWebBrowserWithWebClientCookies(string url)
        //{
        //    //InternetSetCookie(url, "Name", "g12zom5lnou2rewqlsxsyo3m");
        //    wb.Navigate(url);
        //}

        public class CookedWebClient : WebClient
        {
            CookieContainer cookies = new CookieContainer();
            public CookieContainer Cookies { get { return cookies; } }
            protected override WebRequest GetWebRequest(Uri address)
            {
                WebRequest request = base.GetWebRequest(address);
                if (request.GetType() == typeof(HttpWebRequest))
                    ((HttpWebRequest)request).CookieContainer = cookies;
                return request;
            }
        }


        private void getCookies()
        {
            var c = wb.Document.Cookie;

            //// request
            //HttpWebRequest request = CreateWebRequestObject(url);
            //request.CookieContainer = this.cookies; // protected member

            //request.Method = "GET";
            //request.UserAgent = "Mozilla/5.0 (Windows NT 5.1; rv:10.0.2) Gecko/20100101 irefox/10.0.2";

            //// response
            //using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            //{
            //    foreach (Cookie c in response.Cookies)
            //    {

            //        // add cookies to my CookieContainer
            //        this.cookies.Add(new Cookie(c.Name, c.Value, c.Path, c.Domain));
            //    }
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            getCookies();
        }
    }
}
