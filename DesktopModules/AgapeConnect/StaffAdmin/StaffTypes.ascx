<%@ Control Language="VB" AutoEventWireup="false" CodeFile="StaffTypes.ascx.vb" Inherits="DotNetNuke.Modules.StaffAdmin.StaffTypes" %>

 <%@ Register src="../../../controls/labelcontrol.ascx" tagname="labelcontrol" tagprefix="uc1" %>
 <asp:HiddenField ID="hfPortalId" runat="server" />
Use this page to add/edit Staff Types. Please note that it is not possible to delete Staff Types that are being used. However you can rename a Staff Type, and all members of this Staff Type will retain their membership. <br />

<br />


<div align="center">
<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
    CellPadding="4" DataKeyNames="StaffTypeId" DataSourceID="dsStaffTypes" 
    ForeColor="#333333" GridLines="None" ShowFooter="True" EnableModelValidation="True"
        ShowHeaderWhenEmpty="True">
    <AlternatingRowStyle BackColor="White" />
    
    <Columns>
        <asp:TemplateField HeaderText="Staff Type" SortExpression="Name">
            <EditItemTemplate>
                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Name") %>'></asp:TextBox>
            </EditItemTemplate>
            <ItemTemplate>
                <asp:Label ID="Label1" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
            </ItemTemplate>
             <FooterTemplate>
                 <asp:TextBox ID="NameI" runat="server"></asp:TextBox>
            </FooterTemplate>
            <ItemStyle Font-Bold="True" />
        </asp:TemplateField>
        <asp:TemplateField ShowHeader="False">
            <EditItemTemplate>
                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" 
                    CommandName="Update" Text="Update"></asp:LinkButton>
                &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" 
                    CommandName="Cancel" Text="Cancel"></asp:LinkButton>
            </EditItemTemplate>
            <ItemTemplate>
                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" 
                    CommandName="Edit" Text="Edit"></asp:LinkButton>
                &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" 
                    CommandName="Delete" Text="Delete" Visible='<%# CanDelete(Eval("StaffTypeId")) %>' ></asp:LinkButton>
            </ItemTemplate>
            <FooterTemplate>
                  <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" 
                    CommandName="myinsert" Text="Insert" ForeColor="White"></asp:LinkButton>
            </FooterTemplate>
        </asp:TemplateField>
    </Columns>
    <EmptyDataTemplate>
        <table>
        <tr>
            <td>
                <asp:TextBox ID="NameE" runat="server"></asp:TextBox>
            </td>
            <td>
                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" 
                    CommandName="emptyinsert" Text="Insert" ></asp:LinkButton>
            </td>
        </tr>
       </table>
    </EmptyDataTemplate>

    <FooterStyle BackColor="#660000" Font-Bold="True" ForeColor="White" />
    <HeaderStyle BackColor="#660000" Font-Bold="True" ForeColor="White" />
    <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
    <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
    <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
  
</asp:GridView>
<asp:LinqDataSource ID="dsStaffTypes" runat="server" 
    ContextTypeName="StaffBroker.StaffBrokerDataContext" EnableDelete="True" 
    EnableInsert="True" EnableUpdate="True" EntityTypeName="" OrderBy="StaffTypeId" 
    TableName="AP_StaffBroker_StaffTypes" Where="PortalId == @PortalId">
    <WhereParameters>
        <asp:ControlParameter ControlID="hfPortalId" Name="PortalId" 
            PropertyName="Value" Type="Int32" />
    </WhereParameters>
</asp:LinqDataSource>

</div>