<%@ Control Language="VB" AutoEventWireup="false" CodeFile="StaffReportingRelationships.ascx.vb" Inherits="DotNetNuke.Modules.StaffAdmin.StaffReportingRelationships" %>

 <%@ Register src="../../../controls/labelcontrol.ascx" tagname="labelcontrol" tagprefix="uc1" %>

 <table>
    <tr valign="top">
        <td style="min-width: 150px">
       <asp:CheckBoxList ID="cblStaffType" runat="server" >
</asp:CheckBoxList><br />
<asp:CheckBox ID="cbIncDelegates" runat="server" Checked="True" Text="Include Delegates" /><br />   
<asp:CheckBox ID="cbIncDelLeaders" runat="server" Checked="True" Text="Include Leader who have delegated" /><br /> 
<asp:DropDownList ID="ddlType" runat="server" AutoPostBack="True">
    <asp:ListItem Value="0">View Leaders</asp:ListItem>
    <asp:ListItem Value="1">View Followers </asp:ListItem>
</asp:DropDownList><br />
<asp:Button ID="btnLoad" runat="server" Text="Load" /><br /><br />
<fieldset> <legend><b>Key</b></legend>


Leader<br />
<span style="font-style: italic;">Delegate</span><br />
<span style="color: Gray; ">(Leader who has delegated)</span>

</fieldset>
        </td>
        <td>
        <asp:DataList ID="dlStaff" runat="server" >
    <ItemTemplate>
        <asp:Label ID="Label1" runat="server"  Font-Bold="true"
    Text='<%# Eval("LastName") & ", " & Eval("FirstName") %>'></asp:Label>
      <asp:Label ID="lblLeaders" runat="server" 
    Text='<%# GetLeaders(Eval("UserId")) %>'></asp:Label>

    </ItemTemplate>
</asp:DataList>
        </td>
    </tr>
 </table>




