<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ReceiptEditor.aspx.vb" Inherits="DesktopModules_AgapeConnect_StaffRmb_ReceiptEditor" %>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="/Resources/Shared/Scripts/jquery/jquery.min.js?cdv=34" type="text/javascript"></script>
    <script src="/Resources/Shared/Scripts/jquery/jquery-ui.min.js?cdv=34" type="text/javascript"></script>
    <link href="/Portals/_default/Skins/AgapeBlue/skinPopup.css?cdv=34" type="text/css" rel="stylesheet">
    <script>
        $(function () {



            $(".aButton")
              .button()
              ;
        });
    </script>
    <style>
        .aButton {
            padding: .4em !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div style="width: 100%; text-align: left;">
                <span style="background-color: #EEE; padding: 8px 0 10px 5px;">
                    <asp:FileUpload ID="fuReceipt" runat="server" />

                    <asp:Button ID="btnUploadReceipt" runat="server" Text="Upload Selected File" CssClass="aButton btn" Font-Size="small" />
                </span>
                <asp:Button ID="btnRotateLeft" runat="server" Visible="false" CssClass="aButton btn" Text="↺" Font-Size="Small" Style="margin-left: 20px;" />
                <asp:Button ID="btnRotatRight" runat="server" Visible="false" CssClass="aButton btn" Text="↻" Font-Size="Small" />
                <div>

                   
                <asp:HyperLink ID="hlimg" runat="server"  BorderStyle="Solid" BorderColor="DarkGray" BorderWidth="1pt" Target="_blank" Visible="False" style="text-align: center;" >

                    <asp:Image ID="imgReceipt" runat="server"  BorderStyle="Solid" BorderColor="DarkGray" BorderWidth="1pt" ToolTip="Click to open fullsize in new tab..."  />
                    <div>
                        
                        <asp:Label ID="lblOpenNewTab" runat="server"  style="font-style: italic; font-size:small;">Click to view in new tab...</asp:Label>
                    </div>
                </asp:HyperLink>
                    </div>
                <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
            </div>


        
        </div>
    </form>
</body>
</html>
