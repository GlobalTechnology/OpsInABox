<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AdvPrintout.aspx.vb" Inherits="DesktopModules_StaffRmb_RmbPrintout" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
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
