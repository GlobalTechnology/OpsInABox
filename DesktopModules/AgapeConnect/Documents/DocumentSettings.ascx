<%@ Control Language="vb" AutoEventWireup="false" CodeFile="DocumentSettings.ascx.vb" Inherits="DotNetNuke.Modules.Documents.DocumentSettings" %>
<%@ Register Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" TagPrefix="uc1" %>
<%--<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>--%>

<asp:HiddenField ID="hfPortalId" runat="server" />
<asp:HiddenField runat="server" ID="_repostcheckcode" />

<div>
    <div>
        <asp:Label ID="lblFolder" runat="server" ResourceKey="Root"></asp:Label>
        <asp:DropDownList ID="ddlRoot" runat="server"></asp:DropDownList>
    </div>

    <div>
        <hr />
        <asp:Label ID="lblStyle" runat="server" ResourceKey="Style"></asp:Label>

        <asp:RadioButtonList ID="rblStyle" runat="server" OnSelectedIndexChanged="rbStyle_SelectedIndexChanged" AutoPostBack="true">
            <asp:ListItem Text="Basic Search" Value="BasicSearch" />
            <asp:ListItem Text="Icon View" Value="Icons" />
            <asp:ListItem Text="Table View" Value="Table" />
            <asp:ListItem Text="Tree View" Value="Tree" />
        </asp:RadioButtonList>

        <hr />
        <asp:Label ID="lblOptions" runat="server" ResourceKey="Options"></asp:Label>

        <asp:UpdatePanel ID="updatePanelStyle" runat="server">
            <ContentTemplate></ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="rblStyle" EventName="SelectedIndexChanged" />
            </Triggers>
        </asp:UpdatePanel>
                
            <hr />
    </div>

    <div>
        <asp:Label ID="lblTags" runat="server" ResourceKey="Tags"></asp:Label>
        <br />
        
        <div>
            <%--<asp:ListBox ID="lbTags" runat="server" SelectionMode="Single" OnSelectedIndexChanged="lbTags_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>--%>
            <asp:ListBox ID="lbTags" runat="server" SelectionMode="Single"></asp:ListBox>
            <asp:TextBox ID="tbAddTag" runat="server"></asp:TextBox>
            <asp:Button ID="btnAddTag" runat="server" ResourceKey="BtnAddTag" />
            <br />
            <asp:TextBox ID="tbRemoveTag" runat="server" ReadOnly="true"></asp:TextBox>
            <asp:Button ID="btnRemoveTag" runat="server" ResourceKey="BtnRemoveTag" />
        </div>
        <div>
            
          
        </div>
        
       <%-- <asp:LinkButton ID="btnRemove" runat="server">Remove</asp:LinkButton><br />
        <asp:TextBox ID="tbNewTag" runat="server"></asp:TextBox>
        <asp:LinkButton ID="btnAddTag" runat="server">add</asp:LinkButton>--%>
    </div>
    
</div>




<hr />





<%--<table cellpadding="4px" border="1" class="SettingsTable">
    <tr valign="top">
        <td>
            <dnn:Label ID="Label1" runat="server" ResourceKey="Root" />
        </td>
        <td>
            <asp:DropDownList ID="ddlRoot" class="test" runat="server">
            </asp:DropDownList>
            <br />
            <asp:Panel ID="pnlSearch" runat="server">
                <fieldset>
                    <legend class="AgapeH5">Search Options:</legend>
                    <table>
                        <tr>
                            <td>
                                <dnn:Label ID="Label3" runat="server" ResourceKey="SearchType" />
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSearchType" runat="server">
                                    <asp:ListItem Text="Tags" />
                                    <asp:ListItem Text="Keywords" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <dnn:Label ID="Label4" runat="server" ResourceKey="SearchValue" />
                            </td>
                            <td>
                                
                                <asp:TextBox ID="tbSearchValue" runat="server"></asp:TextBox>

                            </td>
                        </tr>
                    </table>
                </fieldset>
            </asp:Panel>
        </td>
    </tr>
    <tr valign="top">
        <td>
            <dnn:Label ID="Label2" runat="server" ResourceKey="Style" />
        </td>
        <td>
            <asp:RadioButtonList ID="ddlStyle" class="displayStyle" runat="server">
                <asp:ListItem Text="Explorer" Value="Explorer" />
                <asp:ListItem Text="Table View " Value="Table" />
                <asp:ListItem Text="Tree" Value="GTree" />
            </asp:RadioButtonList>
           
            <asp:Panel ID="pnlExplorer" runat="server">
                <fieldset>
                    <legend class="AgapeH5">Explorer Options:</legend>
                    <table>
                        <tr>
                            <td>
                                Show Tree:
                            </td>
                            <td>
                                <asp:CheckBox ID="cbShowTree" runat="server" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </asp:Panel>
            <asp:Panel ID="pnlTable" runat="server">
                <fieldset>
                    <legend class="AgapeH5">Table Options:</legend>
                    <table>
                        <tr>
                            <td>
                                <dnn:Label ID="Label6" runat="server" ResourceKey="ColumnWidth" />
                            </td>
                            <td>
                                <asp:TextBox ID="tbWidth" runat="server" class="numeric"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </asp:Panel>
            <asp:Panel ID="pnlTree" runat="server">
                <fieldset>
                    <legend class="AgapeH5">Tree Options:</legend>
                    <table>
                        <tr>
                            <td>
                                <dnn:Label ID="Label8" runat="server" ResourceKey="TreeStyle" />
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlTreeStyle" runat="server">
                                    <asp:ListItem Text="Colored" Value="GTree" />
                                    <asp:ListItem Text="Explorer" Value="Tree" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <asp:Panel ID="pnlTreeColor" runat="server">
                        <table>
                            <tr>
                                <td>
                                    <dnn:Label ID="Label7" runat="server" ResourceKey="TextColor" />
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlColors" runat="server">
                                        <asp:ListItem Text="Turqoise" Value="#28686E" />
                                        <asp:ListItem Text="Olive" Value="#86bb41" />
                                        <asp:ListItem Text="Red" Value="#8c3b3b" />
                                        <asp:ListItem Text="Brown" Value="#876c49" />
                                        <asp:ListItem Text="Mustard" Value="#f1a519" />
                                        <asp:ListItem Text="Green" Value="#1f594f" />
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
      
</fieldset> </asp:Panel> </td> </tr>
<tr valign="top">
    <td>
        <dnn:Label ID="Label5" runat="server" ResourceKey="Tags" />
    </td>
    <td>
        <asp:ListBox ID="lbTags" runat="server"></asp:ListBox>
        <asp:LinkButton ID="btnRemove" runat="server">Remove</asp:LinkButton><br />
        <asp:TextBox ID="tbNewTag" runat="server"></asp:TextBox>
        <asp:LinkButton ID="btnAddTag" runat="server">add</asp:LinkButton>
    </td>
</tr>
</table>
<div style="width: 100%; text-align: center">
    <asp:LinkButton ID="SaveBtn" runat="server" class="aButton btn" ResourceKey="btnSave">Save</asp:LinkButton>
    &nbsp;
    <asp:LinkButton ID="CancelBtn" runat="server" class="aButton btn" ResourceKey="btnCancel">Cancel</asp:LinkButton>
</div>--%>
