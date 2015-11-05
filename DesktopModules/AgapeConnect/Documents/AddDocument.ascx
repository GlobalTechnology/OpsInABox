﻿<%@ Control Language="VB" AutoEventWireup="false" CodeFile="AddDocument.ascx.vb" Inherits="DotNetNuke.Modules.AgapeConnect.Documents.AddDocument" %>
<script type="text/javascript">
    function setUpMyTabs() {
        $("input:radio").click(function () {
            $(".docOption").hide();
            $('#<%= tbURL.ClientID %>').hide();
            $('#<%= ddlPages.ClientID %>').hide();
            $('#<%= FileUpload1.ClientID %>').hide();
            switch (this.value) {
                case "0": $("#divUpload").show(); $('#<%= FileUpload1.ClientID %>').show(); break;
                case "1": $("#divGoogle").show(); $('#<%= tbURL.ClientID %>').show(); break;
                case "2": $("#divURL").show(); $('#<%= tbURL.ClientID %>').show(); break;
                case "3": $("#divPage").show(); $('#<%= ddlPages.ClientID %>').show(); break;
                case "4": $("#divYouTube").show(); $('#<%= tbURL.ClientID %>').show(); break;                
            }
        });
    }
    $(document).ready(function () {
        setUpMyTabs();
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () { setUpMyTabs(); });
    });
</script>
<div id="divAddEditResource" class="Documents">
    <div id="divResName" class="FieldRow">
        <asp:Label ID="lblName" runat="server" CssClass="FieldLabel">Document Name:</asp:Label>
        <asp:TextBox ID="tbName" runat="server"></asp:TextBox>
        <hr />
    </div>
    <div id="divResDesc" class="FieldRow">
        <asp:Label ID="lblDescription" runat="server" CssClass="FieldLabel">Document Description:</asp:Label>
        <asp:TextBox TextMode="MultiLine" ID="tbDescription" runat="server" CssClass="tbResDesc" ></asp:TextBox>
        <hr />
    </div>
    <div id="divResType" class="FieldRow">
        <asp:Label ID="lblType" runat="server" CssClass="FieldLabel">Resource Type:</asp:Label>
        <asp:RadioButtonList ID="rbLinkType" CssClass="rbLinkType" runat="server">
            <asp:ListItem Text="Upload a new file" Value="0" Selected="True" />
            <asp:ListItem Text="Google Doc" Value="1" />
            <asp:ListItem Text="External URL" Value="2" />
            <asp:ListItem Text="A Page on this site" Value="3" />
            <asp:ListItem Text="YouTube Video" Value="4" />
        </asp:RadioButtonList>
        <hr />
    </div>
    <div id="divResDef" class="FieldRow">

        <asp:DropDownList ID="ddlPages" runat="server" Style="display: none;" />
        <asp:TextBox ID="tbURL" runat="server" Style="display: none;" />
        <asp:FileUpload ID="FileUpload1" runat="server" />
        <div id="divUpload" class="docOption">
            <asp:Label ID="lblUpload" runat="server" ResourceKey="lblUpload" />upload.
        </div>
        <div id="divGoogle" class="docOption" style="display: none">
            <asp:Label ID="lblGoogle" runat="server" ResourceKey="lblGoogle" />googledoc.
        </div>
        <div id="divURL" class="docOption" style="display: none">
            <asp:Label ID="lblURL" runat="server" ResourceKey="lblURL" />url.
        </div>
        <div id="divPage" class="docOption" style="display: none">
            <asp:Label ID="lblPage" runat="server" ResourceKey="lblPage" />page.
        </div>
        <div id="divYouTube" class="docOption" style="display: none">
            <asp:Label ID="lblYouTube" runat="server" ResourceKey="lblYouTube" />youtube.
        </div>
        <hr />
    </div>
    <div id="divAddEditButtons" class="SubmitPanel">
        <asp:Button ID="btnOk" runat="server" CssClass="button" Text="OK" />
        <asp:Button ID="btnCancel" runat="server" CssClass="button" Text="Cancel" />
    </div>
</div>
