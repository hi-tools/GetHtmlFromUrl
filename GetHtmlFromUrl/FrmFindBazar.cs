using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Web;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Threading;

namespace GetHtmlFromUrl
{
    public partial class FrmFindBazar : Form
    {
        public FrmFindBazar()
        {
            InitializeComponent();
        }
        WebBrowser wb = null;
        private void btnStart_Click(object sender, EventArgs e)
        {
            lst = new List<Asnaf>();
            txtRes.Text = "";
            wb = new WebBrowser();
            wb.ScriptErrorsSuppressed = true;
            this.Controls.Add(wb);
            From = int.Parse(txtFrom.Text.Trim());
            To = int.Parse(txtTo.Text.Trim());
            MaxProgress = To - From;
            progressBar1.Maximum = MaxProgress;
            Thread t = new Thread(LoadPage);
            t.IsBackground = true;
            t.Start();
        }
        int From, To, MaxProgress;
        void LoadPage()
        {
            string urlAddress = "";
            Complated = true;
            
            del = new DelCreateBrowser(CreateBrowser);
            
            int counter = 1;
            int MaxcounterTime = 1;
            for (int i = From; i <= To; i++)
            {
                ++MaxcounterTime;
                while (true)
                {
                    if (Complated)
                    {
                        break;
                    }
                    else
                    {
                        Thread.Sleep(2000);
                        if (MaxcounterTime == 30)
                        {
                            try
                            {
                                wb.Stop();
                                break;
                            }
                            catch (Exception ex)
                            {
                                break;
                            }
                        }
                    }
                }
                MaxcounterTime = 0;
                urlAddress = "http://findbazar.com/" + i.ToString() + "/";
                progressBar1.Invoke(new DelVoid(() =>
                {
                    if (counter <= progressBar1.Maximum)
                    {
                        progressBar1.Value = counter;
                        txtUrl.Text = urlAddress;
                    }
                }));
                wb.Invoke(del, urlAddress);
                Complated = false;
                ++counter;
            }
            Thread.Sleep((counter - lst.Count)*500);
            dataGridView1.Invoke(new DelVoid(() => { dataGridView1.DataSource = lst.ToList(); }) );
        }

        DelVoid delvoid;
        delegate void DelVoid();

        List<Asnaf> lst = null;
        delegate void DelCreateBrowser(string url);
        DelCreateBrowser del;

        void CreateBrowser(string url)
        {
            wb.Stop();
            wb.Navigate(url);
            wb.DocumentCompleted += Wb_DocumentCompleted;
        }

        private void btnCopyUrl_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtUrl.Text);
        }

        bool Complated = false;
        private void Wb_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                var temp = wb.Document.GetElementsByTagName("table");
                if (temp.Count == 0)
                {
                    return;
                }
                txtRes.Text = temp[0].OuterText;

                var entity = new Asnaf();
                entity.CompanyName = txtRes.Lines[0];
                entity.Code = txtRes.Lines[1].Replace("کد اختصاصی", "");
                if (entity.Code == "")
                {
                    entity.Code = "0";
                }
                entity.ViewCount = txtRes.Lines[2].Replace("تعداد بازدید", "");
                entity.TypeActivation = txtRes.Lines[3].Replace("نوع فعالیت", "");
                entity.NameFamily = txtRes.Lines[4].Replace("نام مالک", "");
                entity.Tel = txtRes.Lines[5].Replace("تلفن", "");
                entity.WebSite = txtRes.Lines[6].Replace("وب سایت", "");
                entity.Country = txtRes.Lines[7].Replace("کشور", "");
                entity.City = txtRes.Lines[8].Replace("شهر", "");
                entity.Adress = txtRes.Lines[9].Replace("آدرس", "");
                if (lst.Any(a => int.Parse(a.Code) == int.Parse(entity.Code)) == false)
                {
                    lst.Add(entity);
                }
                wb.Stop();
                Complated = true;
                dataGridView1.DataSource = lst.ToList();
            }
            catch (Exception ex)
            {
                Text = ex.Message;
                wb.Stop();
                Complated = true;
            }
        }
    }
    public class Asnaf
    {
        public string Code { get; set; }
        public string CompanyName { get; set; }
        public string ViewCount { get; set; }
        public string TypeActivation { get; set; }
        public string Mobile { get; set; }
        public string NameFamily { get; set; }
        public string Tel { get; set; }
        public string WebSite { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Adress { get; set; }
    }
}