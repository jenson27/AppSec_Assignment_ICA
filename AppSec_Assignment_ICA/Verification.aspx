<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Verification.aspx.cs" Inherits="AppSec_Assignment_ICA.Verification" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
       <form id="form1" runat="server">
    <h2>
        <br />
        <asp:Label ID="Label1" runat="server" Text="Enter your verification"></asp:Label>
        <br />
        <br />
   </h2>
        <table class="style1">
            <tr>
                <td class="style3">
                    <asp:Label ID="VerificationCode" runat="server" Text="Verification Code:"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="tb_verification" runat="server" Height="36px" Width="280px"></asp:TextBox>
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
