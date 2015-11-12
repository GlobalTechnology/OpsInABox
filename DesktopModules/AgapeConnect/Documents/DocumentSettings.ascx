<%@ Control Language="vb" AutoEventWireup="false" CodeFile="DocumentSettings.ascx.vb" Inherits="DotNetNuke.Modules.AgapeConnect.Documents.DocumentSettings" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Register Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" TagPrefix="cc1" %>

<%--<script>

    (function ($, Sys) {

            //Initialize confirmation popup for delete button
            $('.btnDelete').dnnConfirm({
                    text: '<%=LocalizeString("deleteRepertoireConfirmationQuestion")%>',
                    yesText: '<%=LocalizeString("deleteRepertoireConfirmationYes")%>',
                    noText: '<%=LocalizeString("deleteRepertoireConfirmationNo")%>',
                    title: '<%=LocalizeString("deleteRepertoireConfirmationTitle")%>'
                });

    }(jQuery, window.Sys)); // pass in the globals. Note the safe access of the jQuery object.

</script>--%>

<%-- Validator Section --%>

<asp:RegularExpressionValidator
    ID="validFolderName"
    runat="server"
    ControlToValidate="tbAddSubFolder"
    ValidationExpression = "^[^\\\/]+$"
    ErrorMessage="validFolderName"
    ResourceKey="validFolderName"
    Display="Dynamic"
    class="MandatoryFieldErrorMsg">
</asp:RegularExpressionValidator>

<asp:CustomValidator 
    ID="cvIsFolder" 
    runat="server"
    ControlToValidate="tbAddSubFolder"
    OnServerValidate="IsFolder"
    ErrorMessage="folderExists"
    ResourceKey="folderExists"
    Display="Dynamic"
    class="MandatoryFieldErrorMsg">
</asp:CustomValidator>

<%-- Validator Section End --%>

<div id="DocumentSettings" class="Documents">

    <div id="divFolder" class="FieldRow">
        <asp:Label ID="lblFolder" runat="server" ResourceKey="lblRoot" CssClass="FieldLabel"></asp:Label>
        <asp:DropDownList ID="ddlRoot" runat="server" AutoPostBack="true"></asp:DropDownList>
        <asp:HyperLink ID="btnEdit" CssClass="btnEdit" runat="server"></asp:HyperLink>
        <asp:LinkButton ID="btnDelete" CssClass="btnDelete" runat="server"></asp:LinkButton>
        <asp:LinkButton ID="btnAdd" CssClass="btnAdd" runat="server"></asp:LinkButton>     
    </div>

    <div id="divButtonAdd">
        <asp:UpdatePanel ID="upButtonAdd" runat="server">
            <ContentTemplate>
                <div class="FieldSubRow">
                    <asp:Label ID="lblAddSubFolder" runat="server" ResourceKey="lblAddSubFolder" CssClass="FieldSubLabel" visible="false"></asp:Label>
                    <asp:textbox ID="tbAddSubFolder" runat="server" visible="false"></asp:textbox>
                    <asp:Button ID="btnAddSubFolder" runat="server"
                        ResourceKey="btnAddSubFolder" visible="false" CssClass="button">
                    </asp:Button>
                </div>
                <div class="FieldSubRow">
                    <asp:ValidationSummary ID="ValSum1" runat="server" DisplayMode="List" CssClass="MandatoryFieldErrorMsg"></asp:ValidationSummary>
                    <asp:Label ID="lblFolderExistsError" runat="server" ResourceKey="lblFolderExistsError" CssClass="FieldSubLabel" visible="false"></asp:Label>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName = "Click"/>

            </Triggers>
        </asp:UpdatePanel>
        <hr />   
    </div>
     
    <div class="SubmitPanel">
        <asp:Button ID="btnSave" runat="server" ResourceKey="btnSave" CssClass="button"></asp:Button>
        <asp:Button ID="btnCancel" runat="server" ResourceKey="btnCancel" CssClass="button"></asp:Button>
    </div>

</div>
