<%@ Control Language="vb" AutoEventWireup="false" CodeFile="DocumentSettings.ascx.vb" Inherits="DotNetNuke.Modules.AgapeConnect.Documents.DocumentSettings" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<asp:HiddenField ID="hfPortalId" runat="server" />

<div class="Documents DocumentSettings">
    <div  class="FieldRow">
        <asp:Label ID="lblFolder" runat="server" ResourceKey="Root" CssClass="FieldLabel"></asp:Label>
        <asp:DropDownList ID="ddlRoot" runat="server"></asp:DropDownList>
        <hr />
    </div>

    <div class="FieldRow">
        <asp:Label ID="lblStyle" runat="server" ResourceKey="Style" CssClass="FieldLabel"></asp:Label>
    </div>

    <div>
        <asp:UpdatePanel ID="updatePanelStyle" runat="server">
            <ContentTemplate>

                <div class="FormItem">
                    <asp:RadioButtonList 
                        ID="rblStyle" 
                        runat="server" 
                        OnTextChanged="rbStyle_SelectedIndexChanged" 
                        AutoPostBack="true">
                    
                        <asp:ListItem Text="Basic Search" Value="BasicSearch" />
                        <asp:ListItem Text="Icon View" Value="Icons" />
                        <asp:ListItem Text="Table View" Value="Table" />
                        <asp:ListItem Text="Tree View" Value="Tree" />
                    </asp:RadioButtonList>
                    <hr />
                </div>
                   
                <div class="FieldRow">
                    <asp:Label ID="lblOptions" runat="server" ResourceKey="Options" CssClass="FieldLabel"></asp:Label>
                </div>

                <div class="FormItem">
                    <asp:checkbox ID="cbshowtree" runat="server" ResourceKey="TreeOption" Visible="false"></asp:checkbox>
                </div>

                <div class="FieldRow">
                    <asp:Label ID="lblColumnWidth" runat="server" ResourceKey="ColumnWidthLabel" Visible="false" CssClass="FieldSubLabel"></asp:Label>
                </div>
                <div class="FieldSubRow">
                    <asp:textbox id="tbcolumnwidth" runat="server" visible="false"></asp:textbox>
                </div>

                <asp:RegularExpressionValidator 
                    ID="intOnly" 
                    runat="server" 
                    ControlToValidate="tbColumnWidth" 
                    ValidationExpression = "\d*" 
                    ErrorMessage="InvalidInteger"
                    ResourceKey="InvalidInteger"
                    Display="Dynamic"
                    Enabled="false"
                    class="MandatoryFieldErrorMsg">
                </asp:RegularExpressionValidator>
                
                <div class="FieldSubRow">
                    <asp:Label ID="lblColors" runat="server" ResourceKey="TextColor" Visible="false" CssClass="FieldSubLabel"></asp:Label>
                    <asp:dropdownlist ID="ddlColors" runat="server" Visible="false"></asp:dropdownlist>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="rblStyle" EventName="SelectedIndexChanged" />
            </Triggers>
        </asp:UpdatePanel>
        <hr />
    </div>

    <div class="FieldRow">
        <asp:Label ID="lblTags" runat="server" ResourceKey="Tags" CssClass="FieldLabel"></asp:Label>
    </div>
    <asp:UpdatePanel ID="updatePanelRemoveTag" runat="server">
        <ContentTemplate>
            <asp:ListBox ID="lbTags" runat="server" SelectionMode="Single" OnSelectedIndexChanged="lbTags_SelectedIndexChanged" AutoPostBack="true" CssClass="ListBox"></asp:ListBox>
            <asp:Button ID="btnRemoveTag" runat="server" ResourceKey="BtnRemoveTag" Enabled="false" CssClass="button"/>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="lbTags" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>

    <div class="FieldSubRow">
        <asp:TextBox ID="tbAddTag" runat="server"></asp:TextBox>
        <asp:Button ID="btnAddTag" runat="server" ResourceKey="BtnAddTag" CssClass="button" />
        <hr />
    </div>

     <div class="SubmitPanel"> 
        <asp:Button ID="SaveBtn" runat="server" ResourceKey="btnSave" CssClass="button"></asp:Button>
        <asp:Button ID="CancelBtn" runat="server" ResourceKey="btnCancel" CssClass="button"></asp:Button>
    </div>
    
</div>
