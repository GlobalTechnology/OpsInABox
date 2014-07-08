<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RmbPrintout.aspx.vb" Inherits="DesktopModules_StaffRmb_RmbPrintout" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <style type="text/css">
.alert-danger, .alert-error {
color: #b94a48;
background-color: #f2dede;
border-color: #eed3d7;
}
.alert, .alert h4 {
color: #c09853;
}
.alert {
padding: 8px 35px 8px 14px;
margin-bottom: 20px;
text-shadow: 0 1px 0 rgba(255, 255, 255, 0.5);

border: 1px solid #fbeed5;
-webkit-border-radius: 4px;
-moz-border-radius: 4px;
border-radius: 4px;
}

    </style>
    <div>
        <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
    </div>
        <asp:Panel ID="pnlAccessDenied" runat="server" Visible="False" Width="100%" HorizontalAlign="Center">
            <asp:Label ID="lblAccessDenied" runat="server" ></asp:Label>
            <asp:Button ID="btnLogin" runat="server" Text="Login" Visible="false" />
        </asp:Panel>
    </form>
</body>
</html>
