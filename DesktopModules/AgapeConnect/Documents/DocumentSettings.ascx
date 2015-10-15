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
            <ContentTemplate>
                <asp:checkbox 
                    ID="cbshowtree"
                    runat="server"
                    ResourceKey="TreeOption"
                    Visible="false">
                </asp:checkbox>
                <asp:Label 
                    ID="lblColumnWidth"
                    runat="server"
                    ResourceKey="ColumnWidthLabel"
                    Visible="false">
                </asp:Label>
                <asp:textbox id="tbcolumnwidth" runat="server" visible="false"></asp:textbox>
                <asp:RegularExpressionValidator 
                    ID="intOnly" 
                    runat="server" 
                    ControlToValidate="tbColumnWidth" 
                    ValidationExpression = "\d*" 
                    ErrorMessage="InvalidInteger"
                    ResourceKey="InvalidInteger"
                    Display="Dynamic"
                    Enabled="false">
                </asp:RegularExpressionValidator>
                 <asp:Label 
                    ID="lblColors"
                    runat="server"
                    ResourceKey="TextColor"
                    Visible="false">
                </asp:Label>
                <asp:dropdownlist ID="ddlColors" runat="server" Visible="false"></asp:dropdownlist>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="rblStyle" EventName="SelectedIndexChanged" />
            </Triggers>
        </asp:UpdatePanel>
    </div>

    <div>
        <hr />
        <asp:Label ID="lblTags" runat="server" ResourceKey="Tags"></asp:Label>
        
        <div>
            
            <asp:UpdatePanel ID="updatePanelRemoveTag" runat="server">
                <ContentTemplate>
                    <asp:ListBox ID="lbTags" runat="server" SelectionMode="Single" Rows="10" OnSelectedIndexChanged="lbTags_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
                    <asp:Button ID="btnRemoveTag" runat="server" ResourceKey="BtnRemoveTag" Enabled="false"/>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="lbTags" EventName="SelectedIndexChanged" />
                </Triggers>
            </asp:UpdatePanel>
            
            <asp:TextBox ID="tbAddTag" runat="server"></asp:TextBox>
            <asp:Button ID="btnAddTag" runat="server" ResourceKey="BtnAddTag" />
        </div>
    </div>

     <div>
        <hr />
        <asp:Button ID="SaveBtn" runat="server" ResourceKey="btnSave"></asp:Button>
        <asp:Button ID="CancelBtn" runat="server" ResourceKey="btnCancel"></asp:Button>
    </div>
    
</div>
