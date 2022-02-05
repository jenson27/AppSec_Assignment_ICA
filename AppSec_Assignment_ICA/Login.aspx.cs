using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using System.Text.RegularExpressions;
using System.Drawing;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.Helpers;

namespace AppSec_Assignment_ICA
{
    public partial class Login : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        static string errorMsg = "";
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public bool ValidateCaptcha()
        {
            bool result = true;

            //When user submits the recaptcha form, the user gets a response POST parameter. 
            //captchaResponse consist of the user click pattern. Behaviour analytics! AI :) 
            string captchaResponse = Request.Form["g-recaptcha-response"];

            //To send a GET request to Google along with the response and Secret key.
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.google.com/recaptcha/api/siteverify?secret=6Le5jNodAAAAAFH4D-I1oKKW6dGHELlQsjv74clv &response=" + captchaResponse);

            try
            {

                //Codes to receive the Response in JSON format from Google Server
                using (WebResponse wResponse = req.GetResponse())
                {
                    using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                    {
                        //The response in JSON format
                        string jsonResponse = readStream.ReadToEnd();

                        //To show the JSON response string for learning purpose
                        //lbl_gScore.Text = jsonResponse.ToString();

                        JavaScriptSerializer js = new JavaScriptSerializer();

                        //Create jsonObject to handle the response e.g success or Error
                        //Deserialize Json
                        MyObject jsonObject = js.Deserialize<MyObject>(jsonResponse);

                        //Convert the string "False" to bool false or "True" to bool true
                        result = Convert.ToBoolean(jsonObject.success);//

                    }
                }

                return result;
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            //Response.Write("<script>window.alert('before getDBHash.')</script>");         
            string pwd = tb_pwd.Text.ToString().Trim();
            string userid = tb_userid.Text.ToString().Trim();

            SHA512Managed hashing = new SHA512Managed();
            string dbHash = getDBHash(userid);
            string dbSalt = getDBSalt(userid);

            try
            {
                if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                {
                    string pwdWithSalt = pwd + dbSalt;
                    byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                    string userHash = Convert.ToBase64String(hashWithSalt);

                    if (userHash.Equals(dbHash))
                    {
                        Session["UserID"] = userid;
                        Session["LoggedIn"] = userid;

                        /* Creates a new GUID and save into the session. */
                        string guid = Guid.NewGuid().ToString();
                        Session["AuthToken"] = guid;

                        /* Creates new cookie with guid. */
                        Response.Cookies.Add(new HttpCookie("AuthToken", guid));

                        Response.Redirect("HomePage.aspx", false);
                    }
                    else
                    {
                        errorMsg = "Userid or password is not valid. Please try again.";
                        lb_error.Text = errorMsg;
                        //Response.Redirect("Login.aspx", false);
                    }
                } else
                {
                    errorMsg = "Userid or password is not valid. Please try again.";
                    lb_error.Text = errorMsg;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { }
        }


        protected string getDBSalt(string userid)
        {

            string s = null;

            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PASSWORDSALT FROM ACCOUNT WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["PASSWORDSALT"] != null)
                        {
                            if (reader["PASSWORDSALT"] != DBNull.Value)
                            {
                                s = reader["PASSWORDSALT"].ToString();
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { connection.Close(); }
            return s;

        }

        protected string getDBHash(string userid)
        {

            string h = null;

            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PasswordHash FROM Account WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["PasswordHash"] != null)
                        {
                            if (reader["PasswordHash"] != DBNull.Value)
                            {
                                h = reader["PasswordHash"].ToString();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { connection.Close(); }
            return h;
        }
    }
}