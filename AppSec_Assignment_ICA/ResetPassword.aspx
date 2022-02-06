<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="AppSec_Assignment_ICA.ResetPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
        <script type="text/javascript">
            function validate() {
                var str = document.getElementById('<%=tb_password1.ClientID %>').value;

                if (str.length < 12) {
                    document.getElementById("lbl_pwdchecker").innerHTML = "Password Length must e at least 12 characters";
                    document.getElementById("lbl_pwdchecker").style.color = "Red";
                    return ("too_short");
                }

                else if (str.search(/[0-9]/) == -1) {
                    document.getElementById("lbl_pwdchecker").innerHTML = "Password require at least 1 number";
                    document.getElementById("lbl_pwdchecker").style.color = "Red";
                    return ("no_number");
                }


                document.getElementById("lbl_pwdchecker").innerHTML = "Excellent!";
                document.getElementById("lbl_pwdchecker").style.color = "Blue";
            }
        </script>
</head>
<body>
       <form id="form1" runat="server">
    <h2>
        <br />
        <asp:Label ID="Label1" runat="server" Text="Enter password"></asp:Label>
        <br />
        <br />
   </h2>
        <table class="style1">
            <tr>
                <td class="style3">
                    <asp:Label ID="NewPassword" runat="server" Text="New Password"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="tb_password1" runat="server" Height="36px" Width="280px" onkeyup="javascript:validate()"></asp:TextBox>
                    <asp:Label ID="lbl_pwdchecker" runat="server" Text="pwdchecker"></asp:Label>
                </td>
            </tr>
             <tr>
                <td class="style3">
                    <asp:Label ID="Label2" runat="server" Text="Confirm Password"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="tb_password2" runat="server" Height="36px" Width="280px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style2">
    <asp:Button ID="btn_Submit" runat="server" Height="48px" 
        onclick="btn_Submit_Click" Text="Submit" Width="288px" />
                </td>
            </tr>
    </table>
&nbsp;&nbsp;&nbsp;
    <br />
           <br />
        <asp:Label ID="lb_error" runat="server"></asp:Label>
           <br />
           <br />
           <br />
        <br />
        <br />
   
    <div>
    
    </div>
    </form>
</body>
</html>
