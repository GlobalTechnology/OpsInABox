<%@ Control Language="vb" AutoEventWireup="false" CodeFile="StaffProfileSettings.ascx.vb" Inherits="DotNetNuke.Modules.StaffProfile.StaffProfileSettings" %>
<%@ Register Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" TagPrefix="uc1" %>

<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx"%>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<asp:HiddenField ID="hfPortalId" runat="server" />
<table>
    <tr>
        <td><dnn:Label ID="Label2" runat="server" Text="Staff Types" ResourceKey="lblStaffTypes"  /></td>
        <td>
            <fieldset>
                <asp:ListBox ID="lbStaffTypes" runat="server" SelectionMode="Multiple" Height="150px"
                    DataSourceID="dsStaffTypes" DataTextField="Name" DataValueField="StaffTypeId">
                </asp:ListBox>
               

                <asp:LinqDataSource ID="dsStaffTypes" runat="server" 
                    ContextTypeName="StaffBroker.StaffBrokerDataContext" EntityTypeName="" 
                    OrderBy="Name" TableName="AP_StaffBroker_StaffTypes" 
                    Where="PortalId == @PortalId">
                    <WhereParameters>
                        <asp:ControlParameter ControlID="hfPortalId" DefaultValue="-1" Name="PortalId" 
                            PropertyName="Value" Type="Int32" />
                    </WhereParameters>
                </asp:LinqDataSource>
               

            </fieldset>
        </td>
    </tr>

    
    <tr>
        <td><dnn:Label ID="lblPPP" runat="server" ResourceKey="lblPPP" Text="Personal Profile Properties"  /></td>
        <td>
            <fieldset>
                <asp:CheckBoxList ID="cblProfileProps" runat="server" RepeatColumns="2"></asp:CheckBoxList>
            </fieldset>
        </td>
    </tr>
    <tr>
        <td><dnn:Label ID="lblSPP" runat="server" ResourceKey="lblSPP" Text="Staff Profile Properties"  /></td>
        <td>
            <fieldset>
                <asp:CheckBoxList ID="cblStaffProps" runat="server" RepeatColumns="2"></asp:CheckBoxList>
            </fieldset>
        </td>
    </tr>
    <tr>
        <td><dnn:Label ID="Label3" runat="server" ResourceKey="lblReportsTo" Text="Display 'Reports To'" /></td>
        <td>
            <fieldset>
               <asp:CheckBox ID="cbReportsTo" runat="server" />
            </fieldset>
            
        </td>
    </tr>
</table>





<asp:LinkButton ID="SaveBtn" runat="server" ResourceKey="btnSave">Save</asp:LinkButton> &nbsp; <asp:LinkButton ID="CancelBtn" runat="server" ResourceKey="btnCancel">Cancel</asp:LinkButton>