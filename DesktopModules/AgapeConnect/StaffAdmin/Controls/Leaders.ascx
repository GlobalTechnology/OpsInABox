<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Leaders.ascx.vb" Inherits="DesktopModules_AgapePortal_StaffBroker_Leaders" %>
<%@ Register src="~/controls/labelcontrol.ascx" TagName="labelcontrol" TagPrefix="uc1" %>

<asp:HiddenField ID="hfUserId" runat="server" />
<asp:DataList ID="DataList2" runat="server" DataSource='<%# StaffBrokerFunctions.GetReportsTo(hfUserID.value) %>' Width="300px">
    <ItemTemplate>
    <div style="margin-bottom: 3px;">
        <asp:Label ID="lblLeaderName" runat="server" Font-Bold="true" Text='<%# Eval("LeaderName") %>'  ></asp:Label>
        <asp:LinkButton ID="btnRemoveLeader" runat="server"  Visible='<%#  Not isReadOnly %>' CommandName="DeleteLeader" CommandArgument='<%# Eval("LeaderId") %>'>(Remove)</asp:LinkButton>
        <asp:Panel ID="Panel1" runat="server" Visible='<%# Eval("DelegateId")  >0 And Not isReadOnly %>'>
            <asp:Label ID="Label3" runat="server" ForeColor="Gray" Text=" --delegated to-> "></asp:Label>
            <asp:Label ID="Label4" runat="server" ForeColor="Gray" Font-Bold="true" Text='<%# Eval("DelegateName") %>'></asp:Label>
            <asp:LinkButton ID="btnRemoveDelegate" runat="server" ForeColor="Gray"  Font-Size="10pt"   CommandName="DeleteDelegate" CommandArgument='<%# Eval("LeaderId") %>'>Remove</asp:LinkButton>
        </asp:Panel>
        <asp:Panel ID="Panel2" runat="server" Visible='<%# Eval("DelegateId") < 0 And Not isReadOnly %>'>
            <asp:Label ID="Label1" runat="server" ForeColor="Gray" Text="delegate to "></asp:Label>
           <asp:DropDownList ID="ddlDelegate" runat="server" Font-Size="XX-Small"  DataSource='<%#  StaffBrokerFunctions.GetStaff(Eval("UserId") ) %>'
                 DataTextField="DisplayName" DataValueField="UserID" AppendDataBoundItems="true">
                 <asp:ListItem Value="0">Not Set</asp:ListItem>
            </asp:DropDownList>
            <asp:LinkButton ID="LinkButton1" runat="server" ForeColor="Gray" Font-Size="10pt"  CommandName="AddDelegate" CommandArgument='<%# Eval("LeaderId") %>'>Set</asp:LinkButton>
       
        </asp:Panel>
        </div>

    </ItemTemplate>
    
</asp:DataList>
<asp:Label ID="lblEmpty" runat="server" Font-Italic="true" ForeColor="Gray">Not Set</asp:Label>
<br />
<asp:DropDownList ID="ddladd" runat="server"  DataSource='<%#  StaffBrokerFunctions.GetStaff(hfUserId.value ) %>'
    DataTextField="DisplayName" DataValueField="UserID" Visible='<%#  Not isReadOnly %>'>
</asp:DropDownList>
<asp:LinkButton ID="btnAdd" runat="server" ForeColor="Gray"  Font-Size="10pt"  Visible='<%#  Not isReadOnly %>' >Add</asp:LinkButton>
