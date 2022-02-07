using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.Helpers;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace AppSec_Assignment_ICA
{
    public class MyObject
    {
        public string success { get; set; }
        public List<string> ErrorMessages { get; set; }
    }

    public partial class Registration : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;

        static string line = "\r";
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected bool getDBEmail(string email)
        {

            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select email FROM ACCOUNT WHERE Email=@EMAIL";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@EMAIL", email);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["EMAIL"].Equals(email))
                        {
                            connection.Close();
                            return true;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { connection.Close(); }
            return false;

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
                        lbl_gScore.Text = jsonResponse.ToString();

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

        /* Returns a score base on the password provided. */
        private int checkPassword(string password)
        {
            int score = 0;

            if (password.Length < 12)
            {
                return 1;
            } else
            {
                score++;
            }

            if (Regex.IsMatch(password, "[a-z]"))
            {
                score++;
            }

            if (Regex.IsMatch(password, "[A-Z]"))
            {
                score++;
            }

            if (Regex.IsMatch(password, "[0-9]"))
            {
                score++;
            }

            if (Regex.IsMatch(password, "^[a-z0-9\\.@#\\$%&]+$"))
            {
                score++;
            }


            return score;
        }

        protected byte[] encryptData(string data)
        {
            byte[] cipherText = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                ICryptoTransform encryptTransform = cipher.CreateEncryptor();
                //ICryptoTransform decryptTransform = cipher.CreateDecryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(data);
                cipherText = encryptTransform.TransformFinalBlock(plainText, 0, plainText.Length);


                //Encrypt
                //cipherText = encryptTransform.TransformFinalBlock(plainText, 0, plainText.Length);
                //cipherString = Convert.ToBase64String(cipherText);
                //Console.WriteLine("Encrypted Text: " + cipherString);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { }
            return cipherText;
        }

        /* Creates an entry in the db. */
        protected void createAccount(string imageName)
        {

            try
            {
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Account VALUES(@email, @firstName ,@lastName, @creditCard, @passwordHash,@passwordSalt,@dob, @imageName,@iv,@key)"))
                    //using (SqlCommand cmd = new SqlCommand("INSERT INTO Account VALUES(@Email, @Mobile,@Nric,@PasswordHash,@PasswordSalt,@DateTimeRegistered,@MobileVerified,@EmailVerified)"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@email", tb_email.Text.Trim());
                            cmd.Parameters.AddWithValue("@firstName", tb_firstName.Text.Trim());
                            cmd.Parameters.AddWithValue("@lastName", tb_lastName.Text.Trim());
                            cmd.Parameters.AddWithValue("@creditCard", Convert.ToBase64String(encryptData(tb_creditCard.Text.Trim())));
                            //cmd.Parameters.AddWithValue("@creditCard", tb_creditCard.Text.Trim());
                            cmd.Parameters.AddWithValue("@passwordHash", finalHash);
                            cmd.Parameters.AddWithValue("@passwordSalt", salt);
                            cmd.Parameters.AddWithValue("@dob", DateTime.Parse(DOB.Text));
                            cmd.Parameters.AddWithValue("@imageName", imageName.ToString());
                            cmd.Parameters.AddWithValue("@iv", Convert.ToBase64String(IV));
                            cmd.Parameters.AddWithValue("@key", Convert.ToBase64String(Key));
                            cmd.Connection = con;
                            //con.Open();
                            //cmd.ExecuteNonQuery();
                            //con.Close();

                            try
                            {
                                con.Open();
                                cmd.ExecuteNonQuery();
                                con.Close();
                            }
                            catch (Exception ex)
                            {
                                //throw new Exception(ex.ToString());
                                lb_error1.Text = ex.ToString();
                            }
                            finally
                            {
                                con.Close();
                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
                //lb_error1.Text = "Invalid input";
            }
        }


        /* Register Button. */
        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            WebImage photo = null;
            var newFileName = "";
            var imagePath = "";

            if (ValidateCaptcha())
            {
                /* Password handler. */
                int scores = checkPassword(tb_password.Text);
                string status = "";
                switch (scores)
                {
                    case 1:
                        status = "Very Weak";
                        break;
                    case 2:
                        status = "Weak";
                        break;
                    case 3:
                        status = "Medium";
                        break;
                    case 4:
                        status = "Strong";
                        break;
                    case 5:
                        status = "Very Strong";
                        break;
                    default:
                        break;
                }
                lbl_pwdchecker.Text = "Status : " + status;
                if (scores < 4)
                {
                    lbl_pwdchecker.ForeColor = Color.Red;
                    return;
                }
                lbl_pwdchecker.ForeColor = Color.Green;

                /* Session. */
                if (!getDBEmail(tb_email.Text.Trim()))
                {
                    Session["LoggedIn"] = tb_email.Text.Trim();

                    /* Creates a new GUID and save into the session. */
                    string guid = Guid.NewGuid().ToString();
                    Session["AuthToken"] = guid;

                    /* Creates new cookie with guid. */
                    Response.Cookies.Add(new HttpCookie("AuthToken", guid));

                    /* Photo handler. */
                    photo = WebImage.GetImageFromRequest();

                    if (photo != null)
                    {
                        newFileName = guid + "_" + Path.GetFileName(photo.FileName);
                        imagePath = @"image\" + newFileName;
                        photo.Save(@"~\" + imagePath);
                    }

                    /* Password Hashing. */
                    string pwd = tb_password.Text.ToString().Trim(); ;

                    //Generate random "salt" 
                    RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                    byte[] saltByte = new byte[8];

                    //Fills array of bytes with a cryptographically strong sequence of random values.
                    rng.GetBytes(saltByte);
                    salt = Convert.ToBase64String(saltByte);

                    SHA512Managed hashing = new SHA512Managed();

                    string pwdWithSalt = pwd + salt;
                    byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwd));
                    byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));

                    finalHash = Convert.ToBase64String(hashWithSalt);

                    lb_error1.Text = "Salt:" + salt;
                    lb_error2.Text = "Hash with salt:" + finalHash;

                    RijndaelManaged cipher = new RijndaelManaged();
                    cipher.GenerateKey();
                    Key = cipher.Key;
                    IV = cipher.IV;


                    createAccount(newFileName);

                    Response.Redirect("HomePage.aspx", false);
                } else
                {
                    lb_error1.Text = "Email already exist";
                }
            }
        }
    }
}