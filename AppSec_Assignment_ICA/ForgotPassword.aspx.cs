using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SendGrid;
using System.Net;
using System.Net.Mail;
using SendGrid.Helpers.Mail;
using System.Data.SqlClient;

namespace AppSec_Assignment_ICA
{
    public partial class ForgotPassword : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /* Check if email exist in database*/
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

        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            /* If email exist send a send grid with the random generated number. */
            if (getDBEmail(tb_email.Text.Trim()))
            {

                var apiKey = "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";
                var client = new SendGridClient(apiKey);

                Random rnd = new Random();
                int token = rnd.Next(1000, 9999);

                /* Initiate email contents. */
                var senders = new EmailAddress("testing@AppSec.com", "AppSec");
                var subject = "Social Lyfe Payment";
                var recipient = new EmailAddress(tb_email.Text.Trim(), "Jen");
                var plainTextContent = "Test content";
                var htmlContent = string.Format("Your verification Code is {0}", token);

                /* Sends email through sendgrid API. */
                var msg = MailHelper.CreateSingleEmail(senders, recipient, subject, plainTextContent, htmlContent);
                var response = client.SendEmailAsync(msg).Result;


            }
            else
            {
                lb_error.Text = "Email does not exist";
            }
        }
    }
}