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
using System.Threading.Tasks;
using System.Text;
using System.Net.Mime;
using System.Net.Http;
using System.Diagnostics;

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
                SendEmail(tb_email.Text.Trim());

            }
            else
            {
                lb_error.Text = "Email does not exist";
            }
        }

        static void SendEmail(string email)
        {
            Random rnd = new Random();
            int token = rnd.Next(1000, 9999);
            /*
            var apiKey = "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";
            var client = new SendGridClient(apiKey);

            Random rnd = new Random();
            int token = rnd.Next(1000, 9999);

            //Initiate email contents.
            var senders = new EmailAddress("jenson27@hotmail.com", "AppSec");
            var subject = "Social Lyfe Payment";
            var recipient = new EmailAddress(email, "Jen");
            var plainTextContent = "Test content";
            var htmlContent = string.Format("Your verification Code is {0}", token);

            //Sends email through sendgrid API.
            var msg = MailHelper.CreateSingleEmail(senders, recipient, subject, plainTextContent, htmlContent);
            client.SendEmailAsync(msg);
            */

            // Specify the from and to email address
            MailMessage mailMessage = new MailMessage("from_email@gmail.com", email);
            // Specify the email body
            mailMessage.Body = string.Format("Your verification Code is {0}", token);
            // Specify the email Subject
            mailMessage.Subject = "Forgot Password APPSEC";


            // No need to specify the SMTP settings as these 
            // are already specified in web.config
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            //SmtpClient.EnableSsl = true;
            smtpClient.Credentials = new System.Net.NetworkCredential()
            {
                UserName = "astraltest2727@gmail.com",
                Password = "Astral2727@"
            };
            // Finall send the email message using Send() method
            smtpClient.Send(mailMessage);
        }
    }
}