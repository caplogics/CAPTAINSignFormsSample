<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="optin.aspx.cs" Inherits="Captain.optin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="padding: 100px;">

            <span>
                <asp:Image ID="imgStatus" runat="server" Width="60px" Height="60px" style="position: relative;" />
            </span>
            <span>
                <asp:Label ID="lblMessage" runat="server" Font-Size="40px" Style="font-family: calibri; position:absolute;" ForeColor="#59af51"></asp:Label>
            </span>
        </div>
    </form>
</body>
</html>
