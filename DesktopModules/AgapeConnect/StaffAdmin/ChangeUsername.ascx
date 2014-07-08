<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ChangeUsername.ascx.vb" Inherits="DotNetNuke.Modules.StaffAdmin.ChangeUsername" %>

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
        <td>        
           Change Username for:
        </td>
        <td>
            <asp:DropDownList ID="ddlOrigUser" runat="server"></asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>        
           Change Username to:
        </td>
        <td>
            <asp:TextBox ID="tbNewUsername" runat="server"></asp:TextBox>
        </td>
    </tr>
</table>

<asp:Label ID="lblLog" runat="server" Text=""></asp:Label><br /><br />
<asp:Button ID="btnChange" runat="server" Text="Change"  CssClass="aButton btn"/> 
<asp:Button ID="btnReturn" runat="server" Text="Back"  CssClass="aButton btn"/>
