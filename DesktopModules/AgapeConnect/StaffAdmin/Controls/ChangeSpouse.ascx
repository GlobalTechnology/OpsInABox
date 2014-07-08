<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ChangeSpouse.ascx.vb" Inherits="DesktopModules_AgapeConnect_StaffAdmin_Controls_ChangeSpouse" %>

 <%@ Register src="~/controls/labelcontrol.ascx" tagname="labelcontrol" tagprefix="uc1" %>
 
<script type="text/javascript">
    /*globals jQuery, window, Sys */
    (function ($, Sys) {
        function setUpMyTabs() {
            $('#<%= GetSpouseClientId("tbNewFirstName") %>').Watermark('First name');
            $('#<%= GetSpouseClientId("tbNewLastName") %>').Watermark('Last name');
            $('#<%= GetSpouseClientId("tbNewSpouseGCX") %>').Watermark('GCX Username');
        }
        $(document).ready(function () {
            setUpMyTabs();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                setUpMyTabs();
            });
        });
    } (jQuery, window.Sys));
    function SetStaffId(thisStaffId) {
        var _thisStaffid = thisStaffId;
        $('#<%= GetSpouseClientId("hfOrigStaffId")  %>').val(_thisStaffid);
    }
    function OnStaffNot() {

        if ($('#<%= GetSpouseClientId("ddlCurrent")  %>').val() == "0") {
            $('#<%= GetSpouseClientId("pnlNewNot")  %>').show();
            $('#<%= GetSpouseClientId("pnlNewOn")  %>').hide();
        } else if ($('#<%= GetSpouseClientId("ddlCurrent")  %>').val() == "1") {

            $('#<%= GetSpouseClientId("pnlNewNot")  %> ').hide();
            $('#<%= GetSpouseClientId("pnlNewOn")  %>').show();

        } else {
            $('#<%= GetSpouseClientId("pnlNewNot")  %>').hide();
            $('#<%= GetSpouseClientId("pnlNewOn") %>').hide();
        }
    }
</script>
<asp:HiddenField ID="hfPortalId" runat="server" />
<asp:HiddenField ID="hfOrigStaffId" runat="server" />
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
<table>
<tr>
<td>
<uc1:labelcontrol ID="labelcontrol1" runat="server" Text="New Spouse on Staff?" HelpText="Please select whether or not the new spouse is currently on our website as staff." />
</td>
<td>
<asp:DropDownList ID="ddlCurrent" runat="server" onChange="OnStaffNot();">
    <asp:ListItem Selected="True" Value="0">new spouse not on staff</asp:ListItem>
    <asp:ListItem Value="1">new spouse on staff</asp:ListItem>
</asp:DropDownList>
</td>
</tr>


<tbody id="pnlNewNot" runat="server">

<tr>
<td>
<uc1:labelcontrol ID="labelcontrol2" runat="server" Text="New Spouse's details" HelpText="Enter the wife/husband's GCX Username, first name and last name." />
</td>
<td>
<asp:TextBox ID="tbNewSpouseGCX" runat="server" Width="220px"></asp:TextBox>
</td>
</tr>
<tr>
<td>
&nbsp;
</td>
<td>
<asp:TextBox ID="tbNewFirstName" runat="server"></asp:TextBox>&nbsp;<asp:TextBox ID="tbNewLastName" runat="server"></asp:TextBox>
</td>
</tr>

</tbody>
<tbody id="pnlNewOn" runat="server">

<tr>
<td>
<uc1:labelcontrol ID="labelcontrol3" runat="server" Text="Select Staff Member" HelpText="Select the staff member's new spouse from the drop down list." />
</td>
<td>
    <asp:DropDownList ID="ddlAllStaff" runat="server">
    </asp:DropDownList>
</td>
</tr>
</tbody>

</table>
<asp:Label ID="lblNewError" runat="server" ForeColor="Red"></asp:Label><br />
<asp:Button ID="btnNewSpouse" runat="server" Text="Change Spouse" class="aButton btn" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnNewSpouse" EventName="Click" />
</Triggers>
</asp:UpdatePanel>