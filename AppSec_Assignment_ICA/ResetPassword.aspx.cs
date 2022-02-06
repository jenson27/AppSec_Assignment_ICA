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
using System.Diagnostics;

namespace AppSec_Assignment_ICA
{
    public partial class ResetPassword : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void UpdateAccount()
        {
            using (SqlConnection con = new SqlConnection(MYDBConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UPDATE Account SET passwordHash = @PasswordHash, passwordSalt = @PasswordSalt WHERE email = @Email"))
                //using (SqlCommand cmd = new SqlCommand("INSERT INTO Account VALUES(@Email, @Mobile,@Nric,@PasswordHash,@PasswordSalt,@DateTimeRegistered,@MobileVerified,@EmailVerified)"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@PasswordSalt", salt);
                        cmd.Parameters.AddWithValue("@PasswordHash", finalHash);
                        cmd.Parameters.AddWithValue("@Email", Session["EmailPasswordReset"].ToString());
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
                            //lb_error1.Text = ex.ToString();
                        }
                        finally
                        {
                            con.Close();
                        }
                    }
                }

            }
        }

        /* Returns a score base on the password provided. */
        private int checkPassword(string password)
        {
            int score = 0;

            if (password.Length < 12)
            {
                return 1;
            }
            else
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
        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            /* If email exist send a send grid with the random generated number. */
            if (tb_password1.Text.Trim().Equals(tb_password2.Text.Trim()))
            {
                /* Password handler. */
                int scores = checkPassword(tb_password1.Text);
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

                /* Password Hashing. */
                string pwd = tb_password1.Text.ToString().Trim();

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

                //lb_error1.Text = "Salt:" + salt;
                //lb_error2.Text = "Hash with salt:" + finalHash;

                RijndaelManaged cipher = new RijndaelManaged();
                cipher.GenerateKey();
                Key = cipher.Key;
                IV = cipher.IV;

                UpdateAccount();
                Response.Redirect("Login.aspx", false);

            }
            else
            {
                lb_error.Text = "Password does not match";
            }
        }
    }
}