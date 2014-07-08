<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Approve.aspx.vb" Inherits="DesktopModules_StaffRmb_AdvApprove" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reimbursement Approval</title>

     <script src="//code.jquery.com/jquery-1.10.2.js"></script>
  <script src="//code.jquery.com/ui/1.11.0/jquery-ui.js"></script>
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.0/themes/smoothness/jquery-ui.css">
    <script>
        $(function () {
          
            $('.btn').button();
           
        });
    </script>
    <style>
        .ApprovalMessage {
            font-size: x-large;
            font-family: 'Open Sans', Arial;
            margin-top: 80px;
            display: block;
        }
         .ApprovalSubtitle {
            font-size: large;
            font-family: 'Open Sans', Arial;
            color: #AAC;
            font-style: italic;
        }
        .ApprovalTitle {
            font-size: xx-large;
            font-family: 'Open Sans', Arial;
            margin-top: 28px;
            font-weight: bold;
            display: block;
        }

        .logo {
            float: right;
            width: 250px;
            margin-top: -10px;
        }

        .container {
            width: 800px;
            text-align: left;
            margin-left:auto;
            margin-right: auto;
        }
        .outercontainer{
            width: 100%;
            text-align: center;
        }
        .button-panel{
            width: 100%;
            text-align: center;
            margin-top: 30px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <asp:HiddenField ID="hfSBNo" runat="server" />
        <asp:HiddenField ID="hfUserId" runat="server" />

        <asp:Panel ID="pnlApprove" runat="server" Visible="False"  CssClass="outercontainer" >
            <div class="container">
                <asp:Image ID="imgLogo" runat="server" CssClass="logo" ImageUrl="/sso/GetLogo.aspx" />
                <asp:Label ID="lblTitle" runat="server" CssClass="ApprovalTitle"></asp:Label>
                 <asp:Label ID="lblSubTitle" runat="server" CssClass="ApprovalSubtitle"></asp:Label>
                <div style="clear:both" />
                <div>
                    <asp:Label ID="lblApprove" runat="server" CssClass="ApprovalMessage"></asp:Label>
                </div>
                <div class="button-panel">

               
                <asp:Button ID="btnUndo" runat="server" CssClass="btn" Text="Undo Approval" Visible="false" />
                <asp:Button ID="btnApprove" runat="server" CssClass="btn"  Text="Approve" Visible="false" />
                <asp:Button ID="btnLogin" runat="server" CssClass="btn"  Text="Login" Visible="true" />
                     </div>
            </div>

        </asp:Panel>
    </form>
</body>
</html>
