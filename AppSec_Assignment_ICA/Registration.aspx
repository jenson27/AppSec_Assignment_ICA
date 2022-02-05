<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="AppSec_Assignment_ICA.Registration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="https://www.google.com/recaptcha/api.js?render=6Le5jNodAAAAAEbfWDvfn8CtebB_fKqj9ysPiXEF"></script>
    <script type="text/javascript">
            function validate() {
                var str = document.getElementById('<%=tb_password.ClientID %>').value;

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
    <form id="form1" runat="server" method="post" enctype="multipart/form-data">
        <div>
            <h2>
                <br />
                    <asp:Label ID="Label1" runat="server" Text="Account Registration"></asp:Label>
                <br />
                <br />
           </h2>
        <table class="style1">
            <tr>
                <td class="style3">
                    <asp:Label ID="firstName" runat="server" Text="First Name:"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="tb_firstName" runat="server" Height="36px" Width="280px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style3">
                    <asp:Label ID="lastName" runat="server" Text="Last Name:"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="tb_lastName" runat="server" Height="36px" Width="280px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style3">
                    <asp:Label ID="Label3" runat="server" Text="Credit Card Number:"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="tb_creditCard" runat="server" Height="32px" Width="281px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style3">
                    <asp:Label ID="email" runat="server" Text="Email Address:"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="tb_email" runat="server" Height="32px" Width="281px"></asp:TextBox>
                    <asp:RegularExpressionValidator
                        id="regEmail"
                        ControlToValidate="tb_email"
                        Text="Enter valid email address"
                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                        Runat="server" />   

                </td>
            </tr>
            <tr>
                <td class="style6">
                    <asp:Label ID="password" runat="server" Text="Password: "></asp:Label>
                </td>
                <td class="style7">
                    <asp:TextBox ID="tb_password" runat="server" Height="32px" Width="281px" onkeyup="javascript:validate()"></asp:TextBox>
                    <asp:Label ID="lbl_pwdchecker" runat="server" Text="pwdchecker"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style3">
                    <asp:Label ID="Label6" runat="server" Text="Date Of Birth"></asp:Label>
                </td>
                <td class="style2">
                    <EditItemTemplate>  
                        <asp:TextBox ID="DOB" runat="server" Text='' TextMode="Date" Height="32px" Width="281px"></asp:TextBox>  
                    </EditItemTemplate>
                </td>
            </tr>
            <tr>
                <td class="style3">
                    <asp:Label ID="Label2" runat="server" Text="Image"></asp:Label>
                </td>
                <td>
                    <fieldset>
                        <legend> Upload Image </legend>
                        <label for="Image">Image</label>
                        <input type="file" name="Image" />
                    </fieldset>
                </td>
            </tr>
            <tr>
                <td class="style4">
                </td>
                <td class="style5">
                    <asp:Button ID="btn_Submit" runat="server" Height="48px" 
                        onclick="btn_Submit_Click" Text="Submit" Width="288px" />
                </td>
            </tr>
    </table>

    <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response"/>

&nbsp;<br />
        <asp:Label ID="lbl_gScore" runat="server"></asp:Label>
        <br />
        <asp:Label ID="lb_error1" runat="server"></asp:Label>
        <br />
        <asp:Label ID="lb_error2" runat="server"></asp:Label>
    <br />
        <br />
    
    </div>
    </form>

    <script>
        grecaptcha.ready(function () {
            grecaptcha.execute('6Le5jNodAAAAAEbfWDvfn8CtebB_fKqj9ysPiXEF', { action: 'Login' }).then(function (token) {
                document.getElementById("g-recaptcha-response").value = token;
            });
        });
    </script>
</body>
</html>
