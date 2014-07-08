<%@ Control Language="VB" AutoEventWireup="false" CodeFile="EditMenu.ascx.vb" Inherits="DotNetNuke.Modules.Menu.EditMenu" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<script src="/js/jquery.numeric.js" type="text/javascript"></script>
<script type="text/javascript">

    function setUpMyTabs() {
        $('.numeric').numeric();
        $('.iType').change(function () {
            var t =  $(this).val();

            switch (t) {
                case "0": //URL
                    $('.iURL').show();
                    $('.iTabs').hide();
                    $('.iFiles').hide();
                    $('.iFolders').hide();
                    $('.iNewTab').show();
                    $('.iDisplayName').show();
                    break;
                case "1": //Page
                   
                    $('.iTabs').show();
                    $('.iFiles').hide();
                    $('.iURL').hide();
                    $('.iFolders').hide();
                    $('.iNewTab').show();
                    $('.iDisplayName').show();
                    break;
                case "2": //Files
                    $('.iFiles').show();
                    $('.iTabs').hide();
                    $('.iURL').hide();
                    $('.iFolders').hide();
                    $('.iNewTab').show();
                    $('.iDisplayName').show();
                    break;
                case "3": //Folders
                    $('.iFolders').show();
                    $('.iTabs').hide();
                    $('.iFiles').hide();
                    $('.iURL').hide();
                    $('.iNewTab').show();
                    $('.iDisplayName').show();
                    break;
                case "20": //Blank
                    $('.iFolders').hide();
                    $('.iTabs').hide();
                    $('.iFiles').hide();
                    $('.iURL').hide();
                    $('.iNewTab').hide();
                    $('.iDisplayName').hide();

                    break;

            }

        });
        $('.iType').change();

        $('#<%= ddlType.ClientId %>').change(function () {
       
            if ($(this).val() == "Normal") {
                $('#divManualLinks').show();
            }
            else {
                $('#divManualLinks').hide();

            }

        });
        $('#<%= ddlType.ClientId %>').change();

        $('.aButton').button();

    }

    $(document).ready(function () {
        setUpMyTabs();


        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () { setUpMyTabs(); });
    });
   


    



   

</script>



<asp:HiddenField ID="ModuleHF" runat="server" />
<asp:HiddenField ID="OldNewHF" runat="server" />
<asp:HiddenField ID="PortalIdHF" runat="server" />
<table>
    <tr>
        <td width="100px">
            <dnn:Label ID="lblMenuPrefix" runat="server" ResourceKey="lblMenuPrefix" />
        </td>
        <td>
            <asp:TextBox ID="tbPrefix" runat="server" Rows="3" TextMode="MultiLine" Width="300px"></asp:TextBox>
        </td>
    </tr>
    <%--<tr>
        <td width="100px">
            <dnn:Label ID="Label4" runat="server" ResourceKey="lblMenuWidth" />
        </td>
        <td>
            <asp:TextBox ID="tbMenuWidth" runat="server" Rows="3" class="numeric" ></asp:TextBox>
        </td>
    </tr>--%>
    <tr>
        <td width="100px">
            <dnn:Label ID="Label2" runat="server" ResourceKey="lblType" />
        </td>
        <td>
            <asp:DropDownList ID="ddlType" runat="server">
                <asp:ListItem Text="Normal" Value="Normal" />
                <asp:ListItem Text="Document Tags" Value="Tags" />
            </asp:DropDownList>
        </td>
    </tr>
   
    <tr id="divManualLinks">
        <td width="100px">
            <dnn:Label ID="Label3" runat="server" ResourceKey="lblLinks" />
        </td>
        <td>
            
            <asp:GridView ID="gvLinks" runat="server" AutoGenerateColumns="False"  ShowFooter="True"
                DataSourceID="LinqDataSource1">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                             <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False"  Visible='<%# Eval("ViewOrder")<>0 %>'
                        CommandArgument='<%# Eval("LinkId") %>' CommandName="Promote" ImageUrl="~/images/up.gif"/>
                   
                        </ItemTemplate>
                        <EditItemTemplate>
                           
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                             <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" 
                        CommandArgument='<%# Eval("LinkId") %>' CommandName="Demote"  ImageUrl="~/images/dn.gif" Visible='<%# IsLastRow(Eval("ViewOrder") )%>'  />
                        </ItemTemplate>
                        <EditItemTemplate>
                            
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="DisplayName">
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("DisplayName") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="tbDisplayName" runat="server" Text='<%# Eval("DisplayName") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="tbfDisplayName" runat="server" class="iDisplayName" ></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Type">
                        <ItemTemplate>
                            <asp:DropDownList ID="DropDownList1" runat="server" Enabled="false" SelectedValue='<%# Eval("LinkType") %>' Font-Size="X-Small">
                                <asp:ListItem Text="URL" Value="0" />
                                <asp:ListItem Text="Page" Value="1" />
                                <asp:ListItem Text="Document" Value="2" />
                                <asp:ListItem Text="Folder" Value="3" />
                                <asp:ListItem Text="Resource" Value="4" />
                                <asp:ListItem Text="Conference" Value="5" />
                                <asp:ListItem Text="Story" Value="6" />
                                <asp:ListItem Text="Blank" Value="20" />
                            </asp:DropDownList>
                        </ItemTemplate>
                        <EditItemTemplate>
                           <asp:DropDownList ID="ddlType" runat="server" Enabled="false" SelectedValue='<%# Eval("LinkType") %>' Font-Size="X-Small">
                                <asp:ListItem Text="URL" Value="0" />
                                <asp:ListItem Text="Page" Value="1" />
                                <asp:ListItem Text="Document" Value="2" />
                                <asp:ListItem Text="Folder" Value="3" />
                               <%-- <asp:ListItem Text="Resource" Value="4" />
                                <asp:ListItem Text="Conference" Value="5" />
                                <asp:ListItem Text="Story" Value="6" />--%>
                                <asp:ListItem Text="Blank" Value="20" />
                            </asp:DropDownList>
                        </EditItemTemplate>
                        <FooterTemplate>
                        <asp:DropDownList ID="ddlfType" runat="server" Font-Size="X-Small" class="iType">
                                <asp:ListItem Text="URL" Value="0" />
                                <asp:ListItem Text="Page" Value="1" />
                                <asp:ListItem Text="Document" Value="2" />
                                <asp:ListItem Text="Folder" Value="3" />
                               <%-- <asp:ListItem Text="Resource" Value="4" />
                                <asp:ListItem Text="Conference" Value="5" />
                                <asp:ListItem Text="Story" Value="6" />--%>
                                <asp:ListItem Text="Blank" Value="20" />
                            </asp:DropDownList>
                        </EditItemTemplate>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="New Tab">
                        <ItemTemplate>
                            <asp:CheckBox ID="CheckBox1" runat="server" Enabled="false" Checked='<%# IIF(Eval("Target")="_blank", "true", "false") %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:CheckBox ID="cbNewTab" runat="server" Checked='<%# IIF(Eval("Target")="_blank", "true", "false") %>' />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:CheckBox ID="cbfNewTab" runat="server" class="iNewTab"  />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Reference">
                        <ItemTemplate>
                            <asp:Label ID="Label3" runat="server" Text='<%# Eval("Ref") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="tbRef" runat="server" Text='<%# Eval("Ref") %>' Visible='<%# Eval("LinkType")=0 %>'></asp:TextBox>
                               <asp:DropDownList ID="ddlETabs" runat="server" DataSource='<%# GetTabs(Eval("Ref") ) %>' SelectedValue='<%# Eval("Ref") %>' DataTextField="Value" DataValueField="Key" Visible='<%# Eval("LinkType")=1 %>' >
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddlEFiles" runat="server"  DataSource='<%# GetFiles(Eval("Ref") ) %>' SelectedValue='<%# Eval("Ref") %>' DataTextField="Value" DataValueField="Key" Visible='<%# Eval("LinkType")=2 %>' >
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddlEFolders" runat="server"  DataSource='<%# GetFolders(Eval("Ref") ) %>' SelectedValue='<%# Eval("Ref") %>' DataTextField="Value" DataValueField="Key" Visible='<%# Eval("LinkType")=3 %>' >
                            </asp:DropDownList>
                        </EditItemTemplate>
                        <FooterTemplate>
                                <asp:TextBox ID="tbfRef" runat="server" class="iURL" ></asp:TextBox>
                            <asp:DropDownList ID="ddlTabs" runat="server" DataSource='<%# GetTabs() %>' DataTextField="Value" DataValueField="Key" class="iTabs" >
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddlFiles" runat="server"  DataSource='<%# GetFiles() %>' DataTextField="Value" DataValueField="Key" class="iFiles">
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddlFolders" runat="server"  DataSource='<%# GetFolders() %>' DataTextField="Value" DataValueField="Key" class="iFolders">
                            </asp:DropDownList>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="False" ControlStyle-Font-Size="X-Small" >
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" 
                                CommandName="Edit" Text="Edit"></asp:LinkButton>&nbsp;
                                 <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" 
                                CommandName="myDelete" CommandArgument='<%# Eval("LinkId") %>' Text="Delete"></asp:LinkButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" 
                                CommandName="myUpdate" CommandArgument='<%# Eval("LinkId") %>' Text="Update"></asp:LinkButton>
                            &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" 
                                CommandName="Cancel" Text="Cancel"></asp:LinkButton>
                        </EditItemTemplate>
                        <FooterTemplate>
                         <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" 
                                CommandName="myInsert" Text="Insert" Font-Size="X-Small" ></asp:LinkButton>
                        </FooterTemplate>
                    </asp:TemplateField>
                   
                </Columns>
                <EmptyDataTemplate>
                    <table>
                        <tr style="font-weight: bold;">
                            <td></td>
                            <td></td>
                            <td>Display Name</td>
                            <td>Type</td>
                            <td>New Tab</td>
                            <td>Reference</td>
                            <td></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td><asp:TextBox ID="biDisplayName" runat="server" class="iDisplayName" ></asp:TextBox></td>
                            <td><asp:DropDownList ID="biType" runat="server" Enabled="true" Font-Size="X-Small" class="iType">
                                <asp:ListItem Text="URL" Value="0" />
                                <asp:ListItem Text="Page" Value="1" />
                                <asp:ListItem Text="Document" Value="2" />
                                <asp:ListItem Text="Folder" Value="3" />
                               <%-- <asp:ListItem Text="Resource" Value="4" />
                                <asp:ListItem Text="Conference" Value="5" />
                                <asp:ListItem Text="Story" Value="6" />--%>
                                <asp:ListItem Text="Blank" Value="20" />
                            </asp:DropDownList></td>
                            <td><asp:CheckBox ID="biNewTab" runat="server" class="iNewTab" /></td>
                            <td>
                                <asp:TextBox ID="biRef" runat="server" class="iURL"></asp:TextBox>
                                 <asp:DropDownList ID="biTabs" runat="server" DataSource='<%# GetTabs() %>' DataTextField="Value" DataValueField="Key" class="iTabs" >
                            </asp:DropDownList>
                            <asp:DropDownList ID="biFiles" runat="server"  DataSource='<%# GetFiles() %>' DataTextField="Value" DataValueField="Key" class="iFiles">
                            </asp:DropDownList>
                            <asp:DropDownList ID="biFolders" runat="server"  DataSource='<%# GetFolders() %>' DataTextField="Value" DataValueField="Key" class="iFolders">
                            </asp:DropDownList>
                            </td>
                            <td>
                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" 
                                CommandName="BInsert" Text="Insert"></asp:LinkButton>
                            </td>
                        </tr>
                        
                    </table>
                
                </EmptyDataTemplate>

            </asp:GridView>
            <asp:LinqDataSource ID="LinqDataSource1" runat="server" 
                ContextTypeName="StaffBroker.StaffBrokerDataContext" EnableDelete="True" 
                EnableInsert="True" EnableUpdate="True" EntityTypeName="" 
                TableName="AP_Menu_Links" 
                Where="PortalId == @PortalId &amp;&amp; TabModuleId == @TabModuleId" 
                OrderBy="ViewOrder">
                <WhereParameters>
                    <asp:ControlParameter ControlID="PortalIdHF" DefaultValue="-1" Name="PortalId" 
                        PropertyName="Value" Type="Int32" />
                    <asp:ControlParameter ControlID="ModuleHF" DefaultValue="-1" Name="TabModuleId" 
                        PropertyName="Value" Type="Int32" />
                </WhereParameters>
            </asp:LinqDataSource>

           
        </td>
    </tr>
     
    <tr>
        <td width="100px">
            <dnn:Label ID="lblMenuSuffix" runat="server" ControlName="SuffixBox" ResourceKey="lblMenuSuffix" />
        </td>
        <td>
            <asp:TextBox ID="tbSuffix" runat="server" Rows="3" TextMode="MultiLine" Width="300px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <asp:LinkButton ID="SaveMenuButton" runat="server" resourcekey="Update" Text="Update" class="aButton btn"></asp:LinkButton>
        </td>
        <td>
            <asp:LinkButton ID="CancelButton" runat="server" resourcekey="Cancel" Text="Cancel" class="aButton btn"></asp:LinkButton>
        </td>
    </tr>
</table>
