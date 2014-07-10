<%@ Control Language="VB"  AutoEventWireup="false" CodeFile="AgapeIconAdmin.ascx.vb" Inherits="DotNetNuke.Modules.aAgapeIconAdmin.myAgapeIconAdmin" %>
<%@ Register src="~/controls/urlcontrol.ascx" tagname="urlcontrol" tagprefix="uc1" %>
<script src="/js/jquery.numeric.js" type="text/javascript"></script>
<script type="text/javascript">
    /*globals jQuery, window, Sys */
    (function ($, Sys) {
        function setUpMyTabs() {
            $('.numeric').numeric();
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

<style type="text/css">
    .urlControl
    {
        font-size: xx-small ;   
        
    }
</style>
<table>
    <tr>
        <td>
            <b>
                <asp:Label ID="Label3" runat="server" resourcekey="lblIconHeight" Text="IconHeight"></asp:Label></b>
        </td>
        <td>
            <asp:TextBox ID="HeightTB" runat="server" Width="75px" CssClass="numeric"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <b>
                <asp:Label ID="Label4" runat="server" resourcekey="lblPadding" Text="Padding"></asp:Label></b>
        </td>
        <td>
            <asp:TextBox ID="tbPadding" runat="server" Width="75px" CssClass="numeric"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <b>
                <asp:Label ID="Label5" runat="server" resourcekey="lblPadding" Text="Show Title?"></asp:Label></b>
        </td>
        <td>
            <asp:CheckBox ID="cbTitle" runat="server" />
        </td>
    </tr>
</table>
<asp:Button ID="UpdateBtn" runat="server" Text="Update" resourcekey="Update" CssClass="aButton btn" />
<br /><br />
<i>
    <asp:Label ID="Label1" runat="server" resourcekey="lblAlsoF5"></asp:Label></i>
<br /><br />
<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
    DataKeyNames="AgapeIconid" DataSourceID="AgapeIconsDS" ShowFooter="True">
    <Columns>
    <asp:TemplateField ShowHeader="False">
            <ItemTemplate>
               <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" 
                    CommandName="Promote"  Text="Up" 
                    CommandArgument='<%# CInt(Eval("AgapeIconid")) %>' ImageUrl="~/images/up.gif"/>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField ShowHeader="False">
            <ItemTemplate>
                <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" 
                    CommandName="Demote"  Text="Down" 
                    CommandArgument='<%# CInt(Eval("AgapeIconid")) %>' ImageUrl="~/images/dn.gif"/>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Title">
            <ItemTemplate>
                <asp:Label ID="Label1" runat="server" Text='<%# Bind("Title") %>'></asp:Label>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Title") %>'></asp:TextBox>
            </EditItemTemplate>
             <FooterTemplate>
             <asp:TextBox ID="tbTitleInsert" runat="server" Text='<%# Bind("Title") %>'></asp:TextBox>
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Icon" SortExpression="IconFile">
            <EditItemTemplate>
            <uc1:urlcontrol ID="EIconChooser" runat="server" FileFilter="jpg,gif,png"
            ShowFiles="true" ShowTrack="false" ShowLog="false" ShowTabs="false"  />
                
            </EditItemTemplate>
            <ItemTemplate>
                <asp:Image ID="Image1" runat="server" Width="100px" ImageUrl='<%# GetUrlFromFileId(Eval("IconFile")) %>' />
            
            </ItemTemplate>
            <FooterTemplate>
             <uc1:urlcontrol ID="IIconChooser" runat="server" FileFilter="jpg,gif,png"
                ShowFiles="true" ShowTrack="false" ShowLog="false" ShowTabs="false"   />
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Icon-Hover" SortExpression="HovrIconFile">
            <EditItemTemplate>
            <uc1:urlcontrol ID="EHoverIconChooser" runat="server" FileFilter="jpg,gif,png"
            ShowFiles="true" ShowTrack="false" ShowLog="false" ShowTabs="false"  />
                
            </EditItemTemplate>
            <ItemTemplate>
                <asp:Image ID="Image2" runat="server" Width="100px" ImageUrl='<%# GetUrlFromFileId(Eval("HovrIconFile")) %>' />
            
            </ItemTemplate>
            <FooterTemplate>
             <uc1:urlcontrol ID="IHoverIconChooser" runat="server" FileFilter="jpg,gif,png"
                ShowFiles="true" ShowTrack="false" ShowLog="false" ShowTabs="false"   />
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Link" SortExpression="LinkLoc">
            <EditItemTemplate>
                 <uc1:urlcontrol ID="EUrlChooser" runat="server" 
            ShowFiles="false" ShowTrack="false" ShowLog="false"   ShowUpLoad="False" />
            </EditItemTemplate>
            <ItemTemplate>  
                <div style="line-height:8pt;">
                <asp:Label ID="Label2" runat="server" Text='<%# WordWrap(GetUrl(Eval("LinkType"),Eval("LinkLoc")), 45) %>'  Font-Size="8pt" ></asp:Label>
                </div>
            </ItemTemplate>
            <FooterTemplate>
            <div style="font-size: x-small;">
            <uc1:urlcontrol ID="IUrlChooser" runat="server"  
            ShowFiles="false" ShowTrack="false" ShowLog="false"  ShowUpLoad="False" /></div>
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField ShowHeader="False">
            <EditItemTemplate>
                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" 
                    CommandName="Update" resourcekey="Update"></asp:LinkButton>
                &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" 
                    CommandName="Cancel" resourcekey="Cancel"></asp:LinkButton>
            </EditItemTemplate>
            <ItemTemplate>
               <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" 
                    CommandName="Delete" resourcekey="Delete"></asp:LinkButton>
            </ItemTemplate>
            <FooterTemplate>
             <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" 
                    CommandName="Insert" resourcekey="Insert" ></asp:LinkButton>
            </FooterTemplate>
        </asp:TemplateField>
    </Columns>
    <EmptyDataTemplate>
    Create New Icon:
        <table>
            <tr>
            <td><b>Title</b></td>
                <td><b>Icon</b></td>
                <td><b>Hover Icon</b></td>
                <td><b>Link</b></td>
                <td></td>
            </tr>
            <tr> <td>
                 <asp:TextBox ID="tbTitleEInsert" runat="server" ></asp:TextBox>
            </td>
                <td><uc1:urlcontrol ID="EIconChooser" runat="server" FileFilter="jpg,gif,png"
            ShowFiles="true" ShowTrack="false" ShowLog="false" ShowTabs="false" /></td>
            <td><uc1:urlcontrol ID="EHoverIconChooser" runat="server" FileFilter="jpg,gif,png"
            ShowFiles="true" ShowTrack="false" ShowLog="false" ShowTabs="false" /></td>
                <td>
                <uc1:urlcontrol ID="EUrlChooser" runat="server" 
            ShowFiles="false" ShowTrack="false" ShowLog="false"   ShowUpLoad="False" />
                    </td>
                    <td>
                  
                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" 
                    CommandName="EInsert" resourcekey="Insert" Text="Insert"></asp:LinkButton>
                    </td>
            </tr>
        </table>
    </EmptyDataTemplate>
</asp:GridView>
<asp:LinqDataSource ID="AgapeIconsDS" runat="server" 
    ContextTypeName="AgapeIconAdmin.AgapeIconsDataContext" 
    TableName="Agape_Skin_AgapeIcons" EnableDelete="True" EnableInsert="True" 
    EnableUpdate="True" OrderBy="ViewOrder" Where="PortalId == @PortalId" 
    EntityTypeName="">
    <WhereParameters>
        <asp:ControlParameter ControlID="PortalIdHF" Name="PortalId" 
            PropertyName="Value" Type="Int32" />
    </WhereParameters>
</asp:LinqDataSource>
<asp:HiddenField ID="PortalIdHF" runat="server" />

<div class="well">
Quick Add:  
<asp:DropDownList ID="ddlInsertType" runat="server">
    <asp:ListItem Text="Expenses" Value="acStaffRmb" />
     <asp:ListItem Text="MPD Calculator"  Value="acMpdCalc"/>
     <asp:ListItem Text="Accounts" Value="acAccounts" />
</asp:DropDownList>
    <asp:LinkButton ID="btnQuickAdd" runat="server">Add</asp:LinkButton>
    </div>
