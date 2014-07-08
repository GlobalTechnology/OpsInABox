<%@ Control Language="VB" AutoEventWireup="false" CodeFile="StaffProfileFields.ascx.vb" Inherits="DotNetNuke.Modules.StaffAdmin.StaffProfileDefinition" %>
<link href="/Portals/_default/Skins/AgapeBlue/skinPopup.css" rel="stylesheet"
    type="text/css" />
 <%@ Register src="../../../controls/labelcontrol.ascx" tagname="labelcontrol" tagprefix="uc1" %>

 <style type="text/css">
 .ForceWhite th
 {
     color: White;
     font-weight: bold;
 }
 </style>

 <asp:HiddenField ID="hfPortalId" runat="server" />
Use this page to add/edit Staff Profile Fields. These are fields for collecting informaiton about a Staff Member/Couple. 
(Please note that couples have a joint staff profile. If you wish to collect information about them as an individual, it should be stored in the user profile.<br />

<br />


<div align="center">
<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
    CellPadding="4" DataKeyNames="StaffPropertyDefinitionId" DataSourceID="dsStaffTypes" 
    ForeColor="#333333" GridLines="None" ShowFooter="True"
        ShowHeaderWhenEmpty="True">
    <AlternatingRowStyle BackColor="White" />   
    
    <Columns>
     <asp:TemplateField HeaderText="PropertyName" SortExpression="FixedFieldName">
            <EditItemTemplate>
               <asp:Label ID="lblFixedField" runat="server" Text='<%# Eval("FixedFieldName") %>'></asp:Label>
            </EditItemTemplate>
            <ItemTemplate >
                <asp:Label ID="lblFixedField" runat="server" Text='<%# Eval("FixedFieldName") %>'></asp:Label>
            </ItemTemplate>
            <FooterTemplate>
               
            </FooterTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="ViewOrder" HeaderText="ViewOrder" 
            SortExpression="ViewOrder" />

        <asp:TemplateField HeaderText="PropertyName" SortExpression="PropertyName">
            <EditItemTemplate>
                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("PropertyName") %>'></asp:TextBox>
            </EditItemTemplate>
            <ItemTemplate >
                <asp:Label ID="Label1" runat="server" Text='<%# Bind("PropertyName") %>'></asp:Label>
            </ItemTemplate>
            <FooterTemplate>
                <asp:TextBox ID="tbPropertyName" runat="server" ></asp:TextBox>
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="PropertyHelp" SortExpression="PropertyHelp">
            <EditItemTemplate>
                <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("PropertyHelp") %>' Width="300px" Height="60px" TextMode="MultiLine"></asp:TextBox>
            </EditItemTemplate>
            <ItemTemplate>
                <asp:Label ID="Label2" runat="server" Text='<%# Bind("PropertyHelp") %>' Width="300px"></asp:Label>
            </ItemTemplate>
             <FooterTemplate>
                <asp:TextBox ID="tbPropertyHelp" runat="server"  Width="300px" Height="60px" TextMode="MultiLine"></asp:TextBox>
             
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Type">
            <EditItemTemplate>
                <asp:DropDownList ID="ddlTypeEdit" runat="server" selectedValue='<%# Bind("Type") %>'>
                    <asp:ListItem Text="Text" Value="0"></asp:ListItem>
                    <asp:ListItem Text="Number" Value="1"></asp:ListItem>
                    <asp:ListItem Text="True/False" Value="2"></asp:ListItem>
            </asp:DropDownList>
            </EditItemTemplate>
             <ItemTemplate>
               <asp:DropDownList ID="ddlTypeInsert" runat="server" Enabled="false" selectedValue='<%# Bind("Type") %>'>
                    <asp:ListItem Text="Text" Value="0"></asp:ListItem>
                    <asp:ListItem Text="Number" Value="1"></asp:ListItem>
                    <asp:ListItem Text="True/False" Value="2"></asp:ListItem>
            </asp:DropDownList>
            </ItemTemplate>
        <FooterTemplate>
                <asp:DropDownList ID="ddlType" runat="server" selectedValue='<%# Bind("Type") %>'>
                    <asp:ListItem Text="Text" Value="0"></asp:ListItem>
                    <asp:ListItem Text="Number" Value="1"></asp:ListItem>
                    <asp:ListItem Text="True/False" Value="2"></asp:ListItem>
            </asp:DropDownList>
            </FooterTemplate>
        
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Display" SortExpression="Display">
            <EditItemTemplate>
                <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("Display") %>' />
            </EditItemTemplate>
            <ItemTemplate>
                <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("Display") %>' 
                    Enabled="false" />
            </ItemTemplate>
            <FooterTemplate>
                <asp:CheckBox ID="cbDisplay" runat="server" Checked='<%# Bind("Display") %>' />
            </FooterTemplate>
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
            </ItemTemplate>
            <FooterTemplate>
            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" 
                    CommandName="myinsert" Text="Update"></asp:LinkButton>
            </FooterTemplate>
        </asp:TemplateField>
    </Columns>
    <EmptyDataTemplate>
        <table>
        <tr>
            <td></td>
            <td>
                <asp:TextBox ID="tbPropertyNameE" runat="server"></asp:TextBox>
            </td>
            <td>
                 <asp:TextBox ID="tbPropertyHelpE" runat="server"  Width="300px" Height="60px" TextMode="MultiLine"></asp:TextBox>
            </td>
            <td>
             <asp:DropDownList ID="ddlTypeE" runat="server" selectedValue='<%# Bind("Type") %>'>
                    <asp:ListItem Text="Text" Value="0"></asp:ListItem>
                    <asp:ListItem Text="Number" Value="1"></asp:ListItem>
                    <asp:ListItem Text="True/False" Value="2"></asp:ListItem>
            </asp:DropDownList>
            </td>
            <td>
                <asp:CheckBox ID="cbDisplayE" runat="server" />
            </td>
            <td>
                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="emptyinsert"   ></asp:LinkButton>
            </td>
        </tr>
       </table>
    </EmptyDataTemplate>
     <FooterStyle CssClass="ui-widget-header dnnGridFooter" />
                   <HeaderStyle CssClass="ui-widget-header dnnGridHeader"    />
                   
                    <EmptyDataRowStyle CssClass="ui-widget-header dnnGridHeader" />
                    <PagerStyle CssClass="dnnGridPager" />
                    <RowStyle CssClass="dnnGridItem" />
                    <SelectedRowStyle CssClass="dnnFormError" />
                    <SortedAscendingCellStyle BackColor="#FDF5AC" />
                    <SortedAscendingHeaderStyle BackColor="#4D0000" />
                    <SortedDescendingCellStyle BackColor="#FCF6C0" />
                    <SortedDescendingHeaderStyle BackColor="#820000" />
  
</asp:GridView>
<asp:LinqDataSource ID="dsStaffTypes" runat="server" 
    ContextTypeName="StaffBroker.StaffBrokerDataContext" EnableDelete="True" 
    EnableInsert="True" EnableUpdate="True" EntityTypeName="" OrderBy="ViewOrder" 
    TableName="AP_StaffBroker_StaffPropertyDefinitions" 
        Where="PortalId == @PortalId">
    <WhereParameters>
        <asp:ControlParameter ControlID="hfPortalId" Name="PortalId" 
            PropertyName="Value" Type="Int32" />
    </WhereParameters>
</asp:LinqDataSource>

</div>