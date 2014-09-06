using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RestSharp;
using System.Data.OleDb;
using System.Collections.Specialized;
using RestSharp.Serializers;
using Newtonsoft;
using Newtonsoft.Json.Linq;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            textBox1.Text = openFileDialog1.FileName;
        }
        public class CustomFields {
           public string securitycode { get;set;}
            public string email2 {get;set; }
            public string emailprivateflag { get; set; }
            public string isspeaker { get; set; }
            public string allowaccess { get; set; }
            public string linkedinurl { get; set; }
            public string twitterhandle { get; set; }
            public string companyname { get; set; }
            public string companyurl { get; set; }
            public string designation { get; set; }
            public string location { get; set; }

        }
       private void button2_Click(object sender, EventArgs e)
        {
           /*LJOVi9A95Y3r6TqlFmxS314v6ox4xaPf*/
            int success = 0,fail=0;
            var client = new RestClient("http://api.cloud.appcelerator.com/v1/users/login.json");
            client.CookieContainer = new System.Net.CookieContainer();
            var request = new RestRequest("?key="+textBox2.Text, Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("login",textBox3.Text);
            request.AddParameter("password", textBox4.Text);
            IRestResponse response = client.Execute(request);

            var client1 = new RestClient("http://api.cloud.appcelerator.com/v1/users/create.json");
            client1.CookieContainer = new System.Net.CookieContainer();
            var request1 = new RestRequest("?key="+textBox2.Text, Method.POST);
            request1.RequestFormat = DataFormat.Json;

            string con = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + openFileDialog1.FileName + ";Extended Properties='Excel 8.0;HDR=Yes;'";
            using (OleDbConnection connection = new OleDbConnection(con))
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand("select * from [Sheet1$]", connection);
                using (OleDbDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        CustomFields pd = new CustomFields()
                        {
                            securitycode = dr["securitycode"].ToString(),
                            email2 = dr["email"].ToString(),
                            emailprivateflag = dr["emailprivateflag"].ToString(),
                            isspeaker = dr["isspeaker"].ToString(),
                            allowaccess = dr["allowaccess"].ToString(),
                            linkedinurl = dr["linkedinurl"].ToString(),
                            twitterhandle = dr["twitterhandle"].ToString(),
                            companyname = dr["companyname"].ToString(),
                            companyurl = dr["companyurl"].ToString(),
                            designation = dr["designation"].ToString(),
                            location = dr["location"].ToString(),
                        };  
                        
                      //  var row1Col0 = dr["last_name"];
                        //MessageBox.Show(row1Col0.ToString());
                       request1.AddParameter("email", dr["email"]);
                        request1.AddParameter("password","12345");
                        request1.AddParameter("password_confirmation", "12345");
                        request1.AddParameter("first_name", dr["fname"]);
                        request1.AddParameter("last_name", dr["last_name"]);
                        request1.AddParameter( "custom_fields",request1.JsonSerializer.Serialize(pd));

                       
                        

                        IRestResponse response1 = client1.Execute(request1);
                        var json = response1.Content;
                        JObject jb = JObject.Parse(json);
                      // MessageBox.Show(jb["meta"].ToString());
                       if (jb["response"]!= null)
                            success++;
                        else
                            fail++;
                        
                        label2.Text = success.ToString();
                        label4.Text = fail.ToString();
                    }
                }
                connection.Close();
            }
        }

    }
}
