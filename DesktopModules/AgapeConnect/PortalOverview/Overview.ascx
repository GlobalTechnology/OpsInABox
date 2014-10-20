<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Overview.ascx.vb" Inherits="DotNetNuke.Modules.Portals.Overview"%>
<table>
    <tr>
        <td>Sort by:</td>
        <td><asp:RadioButtonList ID="rblSort" runat="server" RepeatDirection="Horizontal" AutoPostBack="true">
    <asp:ListItem Selected ="true" Text="PortalId" Value="PortalId" />
    <asp:ListItem Text="Name" Value="PortalName" />
       

</asp:RadioButtonList></td>
    </tr>
</table>



<asp:GridView ID="gvPortals" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Vertical">
    <AlternatingRowStyle BackColor="White" />
    <Columns>
        <asp:BoundField DataField="PortalId" HeaderText="PortalId" />
         <asp:BoundField DataField="PortalName" HeaderText="Name"  ItemStyle-CssClass="AgapeH5" />
        
        <asp:TemplateField HeaderText="Defaul Portal Alias" >
            <ItemTemplate>
                <asp:Label ID="Label1" runat="server" Text='<%# GetAliases(Eval("PortalId"))%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="Users" HeaderText="Users"  />
        <asp:BoundField DataField="Pages" HeaderText="Pages" />
           <asp:TemplateField HeaderText="Rmb" >
            <ItemTemplate>
                <asp:Label ID="Label1" runat="server" Text='<%# IsUsingRmb(Eval("PortalId"))%>'></asp:Label>

            </ItemTemplate>
        </asp:TemplateField>
        
           <asp:TemplateField HeaderText="Last Proc Rmb" >
            <ItemTemplate>
                <asp:Label ID="Label1" runat="server" Text='<%# LastProcDate(Eval("PortalId"))%>'></asp:Label>

            </ItemTemplate>
        </asp:TemplateField>
           <asp:TemplateField HeaderText="Rmbs (ever)" >
            <ItemTemplate>
                <asp:Label ID="Label1" runat="server" Text='<%# RmbsEver(Eval("PortalId"))%>'></asp:Label>

            </ItemTemplate>
        </asp:TemplateField>
           <asp:TemplateField HeaderText="Rmbs (month)" >
            <ItemTemplate>
                <asp:Label ID="Label1" runat="server" Text='<%# RmbsMonth(Eval("PortalId"))%>'></asp:Label>

            </ItemTemplate>
        </asp:TemplateField>
            <asp:TemplateField HeaderText="Rmbs Trans (ever)" >
            <ItemTemplate>
                <asp:Label ID="Label1" runat="server" Text='<%# RmbTrans(Eval("PortalId"))%>'></asp:Label>

            </ItemTemplate>
        </asp:TemplateField>
      
    </Columns>
    <FooterStyle BackColor="#CCCC99" />
    <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
    <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
    <RowStyle BackColor="#F7F7DE" />
    <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
    <SortedAscendingCellStyle BackColor="#FBFBF2" />
    <SortedAscendingHeaderStyle BackColor="#848384" />
    <SortedDescendingCellStyle BackColor="#EAEAD3" />
    <SortedDescendingHeaderStyle BackColor="#575357" />

</asp:GridView>
