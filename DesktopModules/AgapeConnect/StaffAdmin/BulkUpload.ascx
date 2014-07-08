<%@ Control Language="VB" AutoEventWireup="false" CodeFile="BulkUpload.ascx.vb" Inherits="DotNetNuke.Modules.StaffAdmin.BulkUpload" %>

 <%@ Register src="~/controls/labelcontrol.ascx" tagname="labelcontrol" tagprefix="uc1" %>
<script type="text/javascript">
    /*globals jQuery, window, Sys */
    (function ($, Sys) {
        function setUpMyTabs() {
            $('.aButton').button();
        }

        $(document).ready(function () {
            setUpMyTabs();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                setUpMyTabs();
            });
        });
    } (jQuery, window.Sys));
    </script>

<table>
    <tr>
        <td>File</td>
        <td><asp:FileUpload ID="fuUploadFile" runat="server"  /> </td>
        <td><asp:Button ID="btnProcess" runat="server" Text="Process" CssClass="aButton btn" /></td>
    </tr>
   
</table><br />
<asp:Label ID="lblResponse" runat="server" Text=""></asp:Label><br />
<asp:HyperLink ID="hlDownload" runat="server" Target="_self" NavigateUrl="/DesktopModules/AgapeConnect/StaffAdmin/UploadUsers.xls">Download Template</asp:HyperLink>
<asp:LinkButton ID="btnReturn" runat="server" Text="Back"  CssClass="aButton btn"/>
