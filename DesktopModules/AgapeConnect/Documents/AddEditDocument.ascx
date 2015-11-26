﻿<%@ Control Language="VB" AutoEventWireup="false" CodeFile="AddEditDocument.ascx.vb" Inherits="DotNetNuke.Modules.AgapeConnect.Documents.AddEditDocument" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<script type="text/javascript">

    // Show/hide the right option div depending on selected radio button
    function showAppropriateOptionPanel() {
        $(".docOption").hide();
        switch ($(".rbLinkType input:checked").val()) {
            case "<%= DocumentConstants.LinkTypeFile %>": $("#divUpload").show(); break;
                case "<%= DocumentConstants.LinkTypeGoogleDoc %>": $("#divGoogle").show(); break;
                case "<%= DocumentConstants.LinkTypeUrl %>": $("#divURL").show(); break;
                case "<%= DocumentConstants.LinkTypePage %>": $("#divPage").show(); break;
                case "<%= DocumentConstants.LinkTypeYouTube %>": $("#divYouTube").show(); break;
            }
    }

    function setUpMyPage() {
        
        $(".rbLinkType input:radio").click(function () {
            showAppropriateOptionPanel();
        });
    }
    $(document).ready(function () {
        setUpMyPage();
        showAppropriateOptionPanel()
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () { setUpMyPage(); showAppropriateOptionPanel() });
    });
</script>

<div id="divAddEditDocument" class="Documents">
    <div id="divResName" class="FieldRow">
        <asp:Label ID="lblName" runat="server" resourcekey="lblName.Text" CssClass="FieldLabel" />
        <asp:TextBox ID="tbName" runat="server" />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" resourcekey="rfName" ControlToValidate="tbName" class="MandatoryFieldErrorMsg" ValidationGroup="vgAddEdit" Display="Dynamic"></asp:RequiredFieldValidator>
        <hr />
    </div>
    <div id="divResDesc" class="FieldRow">
        <asp:Label ID="lblDescription" runat="server" resourcekey="lblDescription.Text" CssClass="FieldLabel" />
        <asp:TextBox TextMode="MultiLine" ID="tbDescription" runat="server" CssClass="tbResDesc" ></asp:TextBox>
        <hr />
    </div>
    <div id="divResType" class="FieldRow">
        <asp:Label ID="lblType" runat="server" resourcekey="lblType.Text" CssClass="FieldLabel" />
        <asp:RadioButtonList ID="rbLinkType" CssClass="rbLinkType" runat="server" />
        <hr />
    </div>
    <div id="divResDef" class="FieldRow">
        <div id="divUpload" class="docOption" style="display: none">
            <asp:Label ID="lblUpload" runat="server" resourcekey="lblUpload.Text" CssClass="FieldLabel" />
            <asp:FileUpload ID="FileUpload1" runat="server" />
            <asp:CustomValidator
                ID="cvUpload"
                runat="server"
                ControlToValidate="FileUpload1"
                OnServerValidate="ValidateUpload"
                ErrorMessage="cvUpload"
                ResourceKey="cvUpload"
                Display="Dynamic"
                class="DocTypeErrorMsg"
                ValidateEmptyText="true"
                ValidationGroup="vgAddEdit">
            </asp:CustomValidator>
            <p class="FieldHelp">
                <asp:Label ID="lblUploadHelp" runat="server" resourcekey="lblUploadHelp.Text" />
            </p>
        </div>
        <div id="divGoogle" class="docOption" style="display: none">
            <asp:Label ID="lblGoogle" runat="server" resourcekey="lblGoogle.Text" CssClass="FieldLabel" />
            <asp:TextBox ID="tbGoogle" runat="server" />
            <asp:CustomValidator
                ID="cvGoogle"
                runat="server"
                ControlToValidate="tbGoogle"
                OnServerValidate="ValidateGoogle"
                ErrorMessage="cvGoogle"
                ResourceKey="cvGoogle"
                Display="Dynamic"
                class="DocTypeErrorMsg"
                ValidateEmptyText="true"
                ValidationGroup="vgAddEdit">
            </asp:CustomValidator>
            <p class="FieldHelp">
                <asp:Label ID="lblGoogleHelp" runat="server" resourcekey="lblGoogleHelp.Text" />
            </p>
        </div>
        <div id="divURL" class="docOption" style="display: none">
            <asp:Label ID="lblURL" runat="server" resourcekey="lblURL.Text" CssClass="FieldLabel" />
            <asp:TextBox ID="tbURL" runat="server" />
            <asp:CustomValidator
                ID="cvUrl"
                runat="server"
                ControlToValidate="tbUrl"
                OnServerValidate="ValidateUrl"
                ErrorMessage="cvUrl"
                ResourceKey="cvUrl"
                Display="Dynamic"
                class="DocTypeErrorMsg"
                ValidateEmptyText="true"
                ValidationGroup="vgAddEdit">
            </asp:CustomValidator>
            <p class="FieldHelp">
                <asp:Label ID="lblURLHelp" runat="server" resourcekey="lblURLHelp.Text" />
            </p>
        </div>
        <div id="divPage" class="docOption" style="display: none">
            <asp:Label ID="lblPage" runat="server" resourcekey="lblPage.Text" CssClass="FieldLabel" />
            <telerik:RadComboBox ID="ddlPages" runat="server" />
            <p class="FieldHelp">
                <asp:Label ID="lblPageHelp" runat="server" resourcekey="lblPageHelp.Text" />
            </p>
        </div>
        <div id="divYouTube" class="docOption" style="display: none">
            <asp:Label ID="lblYouTube" runat="server" resourcekey="lblYouTube.Text" CssClass="FieldLabel" />
            <asp:TextBox ID="tbYouTube" runat="server" />
            <asp:CustomValidator
                ID="cvYouTube"
                runat="server"
                ControlToValidate="tbYouTube"
                OnServerValidate="ValidateYouTube"
                ErrorMessage="cvYouTube"
                ResourceKey="cvYouTube"
                Display="Dynamic"
                class="DocTypeErrorMsg"
                ValidateEmptyText="true"
                ValidationGroup="vgAddEdit">
            </asp:CustomValidator>
            <p class="FieldHelp">
                <asp:Label ID="lblYouTubeHelp" runat="server" resourcekey="lblYouTubeHelp.Text" />
            </p>
        </div>
    </div>
    <div id="divAddEditButtons" class="SubmitPanel">
        <asp:Button ID="btnOk" runat="server" resourcekey="btnOk.Text" CssClass="button" ValidationGroup="vgAddEdit" />
        <asp:Button ID="btnCancel" runat="server" resourcekey="btnCancel.Text" CssClass="button" />
    </div>
</div>