using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AppSec_Assignment_ICA
{
    public partial class Verification : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            /* If email exist send a send grid with the random generated number. */
            if (Session["VerificationToken"].ToString().Equals(tb_verification.Text.Trim()))
            {
                Response.Redirect("ResetPassword.aspx", false);

            }
            else
            {
                lb_error.Text = "Wrong Verification Code";
            }
        }
    }
}