<%@ Control Language="vb" AutoEventWireup="false" CodeFile="StaffProfileSettings.ascx.vb" Inherits="DotNetNuke.Modules.StaffProfile.StaffProfileSettings" %>
<%@ Register Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" TagPrefix="uc1" %>

<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx"%>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>

<table>
    <tr>
        <td><dnn:Label ID="lblPPP" runat="server" Text="Personal Profile Properties" HelpText="Select the personal profile properties you would like to display in this control. Personal Profile Properties are unique to the individual. (You can add/edit these propertys under User Accounts module.)" /></td>
        <td>
            <fieldset>
                <asp:CheckBoxList ID="cblProfileProps" runat="server" RepeatColumns="2"></asp:CheckBoxList>
            </fieldset>
        </td>
    </tr>
    <tr>
        <td><dnn:Label ID="lblSPP" runat="server" Text="Staff Profile Properties" HelpText="Select the Staff Profile Propertiees you would like to display in this control. Staff Profile Properties are unique to the Staff Couple/Memeber and can only be editted by administrators on the Staff Admin page. (You can add/edit these propertys under Admin->AgapeConnect->Staff. Then Select 'Edit Staff Profile Propertes' at the bottom of the page." /></td>
        <td>
            <fieldset>
                <asp:CheckBoxList ID="cblStaffProps" runat="server" RepeatColumns="2"></asp:CheckBoxList>
            </fieldset>
        </td>
    </tr>
    <tr>
        <td><dnn:Label ID="Label2" runat="server" Text="Display Giving Tab" HelpText="Allows staff to specify their giving options and customize their giving page" /></td>
        <td>
            <asp:CheckBox ID="cbGiving" runat="server" AutoPostBack="true" />
           
        </td>
    </tr>
    <tr id="rowGiving" runat="server">
        <td><dnn:Label ID="Label1" runat="server" Text="Giving Options" HelpText="Select the Staff Profile Properties you would like to display in this giving section of this control. Staff Profile Properties are unique to the Staff Couple/Memeber and can be editted by the staff memeber. (You can add/edit these propertys under Admin->AgapeConnect->Staff. Then Select 'Edit Staff Profile Propertes' at the bottom of the page." /></td>
        <td>
            <fieldset>
             <asp:CheckBoxList ID="cblGivingProps" runat="server" RepeatColumns="2"></asp:CheckBoxList>
            </fieldset> 
        </td>
    </tr>
</table>





<asp:LinkButton ID="SaveBtn" runat="server" ResourceKey="btnSave">Save</asp:LinkButton> &nbsp; <asp:LinkButton ID="CancelBtn" runat="server" ResourceKey="btnCancel">Cancel</asp:LinkButton>