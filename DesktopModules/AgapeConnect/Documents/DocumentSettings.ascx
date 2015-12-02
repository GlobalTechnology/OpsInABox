<%@ Control Language="vb" AutoEventWireup="false" CodeFile="DocumentSettings.ascx.vb" Inherits="DotNetNuke.Modules.AgapeConnect.Documents.DocumentSettings" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Register Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" TagPrefix="cc1" %>

<%-- Validator Section --%>

<asp:RequiredFieldValidator 
    ID="RequiredFieldValidator1" 
    runat="server" 
    ControlToValidate="tbEditSubFolder"
    ErrorMessage="RequiredFieldValidator"
    ResourceKey="requiredField"
    Display="Dynamic"
    class="MandatoryFieldErrorMsg"
    ValidationGroup="vgEdit">
</asp:RequiredFieldValidator>

<asp:RequiredFieldValidator 
    ID="RequiredFieldValidator2" 
    runat="server" 
    ControlToValidate="tbAddSubFolder"
    ErrorMessage="RequiredFieldValidator"
    ResourceKey="requiredField"
    Display="Dynamic"
    class="MandatoryFieldErrorMsg"
    ValidationGroup="vgAdd">
</asp:RequiredFieldValidator>

<asp:RegularExpressionValidator
    ID="validFolderNameEdit"
    runat="server"
    ControlToValidate="tbEditSubFolder"
    ValidationExpression = "^[^\\\/]+$"
    ErrorMessage="validFolderName"
    ResourceKey="validFolderName"
    Display="Dynamic"
    class="MandatoryFieldErrorMsg"
    ValidationGroup="vgEdit">
</asp:RegularExpressionValidator>

<asp:RegularExpressionValidator
    ID="validFolderNameAdd"
    runat="server"
    ControlToValidate="tbAddSubFolder"
    ValidationExpression = "^[^\\\/]+$"
    ErrorMessage="validFolderName"
    ResourceKey="validFolderName"
    Display="Dynamic"
    class="MandatoryFieldErrorMsg"
    ValidationGroup="vgAdd">
</asp:RegularExpressionValidator>

<asp:CustomValidator 
    ID="cvFolderEdit" 
    runat="server"
    ControlToValidate="tbEditSubFolder"
    OnServerValidate="EditFolder"
    ErrorMessage="folderExists"
    ResourceKey="folderExists"
    Display="Dynamic"
    class="MandatoryFieldErrorMsg"
    ValidationGroup="vgEdit">
</asp:CustomValidator>

<asp:CustomValidator 
    ID="cvFolderAdd" 
    runat="server"
    ControlToValidate="tbAddSubFolder"
    OnServerValidate="AddFolder"
    ErrorMessage="folderExists"
    ResourceKey="folderExists"
    Display="Dynamic"
    class="MandatoryFieldErrorMsg"
    ValidationGroup="vgAdd">
</asp:CustomValidator>

<asp:CustomValidator 
    ID="cvIsFolderRenamable" 
    runat="server" 
    ControlToValidate="ddlRoot"
    OnServerValidate="IsFolderRenamable"
    ErrorMessage="folderNotRenamable"
    ResourceKey="folderNotRenamable"
    Display="Dynamic"
    class="MandatoryFieldErrorMsg"
    ValidationGroup="vgRename">
</asp:CustomValidator>

<asp:CustomValidator 
    ID="cvIsFolderDeletable" 
    runat="server"
    ControlToValidate="ddlRoot"
    OnServerValidate="IsFolderDeletable"
    ErrorMessage="folderNotDeletable"
    ResourceKey="folderNotDeletable"
    Display="Dynamic"
    class="MandatoryFieldErrorMsg"
    ValidationGroup="vgDelete">
</asp:CustomValidator>

<%-- Validator Section End --%>

<div id="DocumentSettings" class="Documents">

        <asp:UpdatePanel ID="upFolderList" runat="server">
            <ContentTemplate>
                <div id="divFolder" class="FieldRow">
                    <asp:Label ID="lblFolder" runat="server" ResourceKey="lblRoot" CssClass="FieldLabel"></asp:Label>
                    <asp:DropDownList ID="ddlRoot" runat="server" AutoPostBack="true"></asp:DropDownList>
                    <asp:LinkButton ID="btnEdit" CssClass="btnEdit" runat="server" ValidationGroup="vgRename"></asp:LinkButton>
                    <asp:LinkButton ID="btnDelete" CssClass="btnDelete" runat="server" ValidationGroup="vgDelete"></asp:LinkButton>
                    <asp:LinkButton ID="btnAdd" CssClass="btnAdd" runat="server"></asp:LinkButton>     
                </div>

                <div id="divButtonEdit">
                    <asp:UpdatePanel ID="upEdit" runat="server" Visible="false">
                        <ContentTemplate>
                            <div class="FieldSubRow">
                                <asp:Label ID="lblEditSubFolder" runat="server" ResourceKey="lblEditSubFolder" CssClass="FieldSubLabel"></asp:Label>
                                <asp:textbox ID="tbEditSubFolder" runat="server"></asp:textbox>
                                <asp:Button ID="btnEditSubFolder" runat="server" ResourceKey="btnEditSubFolder" CssClass="button" ValidationGroup="vgEdit"></asp:Button>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

                <div id="divButtonAdd">
                    <asp:UpdatePanel ID="upAdd" runat="server" Visible="false">
                        <ContentTemplate>
                            <div class="FieldSubRow">
                                <asp:Label ID="lblAddSubFolder" runat="server" ResourceKey="lblAddSubFolder" CssClass="FieldSubLabel"></asp:Label>
                                <asp:textbox ID="tbAddSubFolder" runat="server"></asp:textbox>
                                <asp:Button ID="btnAddSubFolder" runat="server" ResourceKey="btnAddSubFolder" CssClass="button" ValidationGroup="vgAdd"></asp:Button>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

                <div class="FieldSubRow">
                    <asp:ValidationSummary ID="vsRename" runat="server" DisplayMode="List"
                        CssClass="MandatoryFieldErrorMsg" ValidationGroup="vgRename">
                    </asp:ValidationSummary>
                    <asp:ValidationSummary ID="vsDelete" runat="server" DisplayMode="List"
                        CssClass="MandatoryFieldErrorMsg" ValidationGroup="vgDelete">
                    </asp:ValidationSummary>
                    <asp:ValidationSummary ID="vsEdit" runat="server" DisplayMode="List"
                        CssClass="MandatoryFieldErrorMsg" ValidationGroup="vgEdit">
                    </asp:ValidationSummary>
                     <asp:ValidationSummary ID="vsAdd" runat="server" DisplayMode="List"
                        CssClass="MandatoryFieldErrorMsg" ValidationGroup="vgAdd">
                    </asp:ValidationSummary>
                </div>
                
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnEdit" EventName = "Click"/>
                <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName = "Click"/>
                <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName = "Click"/>
                <asp:AsyncPostBackTrigger ControlID="btnEditSubFolder" EventName = "Click"/>
                <asp:AsyncPostBackTrigger ControlID="btnAddSubFolder" EventName = "Click"/>
                <asp:AsyncPostBackTrigger ControlID="ddlRoot" EventName = "SelectedIndexChanged"/>
            </Triggers>
        </asp:UpdatePanel>
        <hr />   
     
    <div class="SubmitPanel">
        <asp:Button ID="btnSave" runat="server" ResourceKey="btnSave" CssClass="button"></asp:Button>
        <asp:Button ID="btnCancel" runat="server" ResourceKey="btnCancel" CssClass="button"></asp:Button>
    </div>

</div>
