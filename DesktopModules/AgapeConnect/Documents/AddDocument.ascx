<%@ Control Language="VB" AutoEventWireup="false" CodeFile="AddDocument.ascx.vb" Inherits="DotNetNuke.Modules.AgapeConnect.Documents.AddDocument" %>
    function setUpMyTabs() {
        $("input:radio").click(function () {
            $(".docOption").hide();
            $('#<%= tbURL.ClientID %>').hide();
            $('#<%= ddlFiles.ClientID %>').hide();
            $('#<%= ddlPages.ClientID %>').hide();
            $('#<%= ddlPages.ClientID %>').hide();
            switch (this.value) {
                case "0": $("#divURL").show(); $('#<%= tbURL.ClientID %>').show(); break;
                case "1": $("#divYouTube").show(); $('#<%= tbURL.ClientID %>').show(); break;
                case "2": $("#divGoogle").show(); $('#<%= tbURL.ClientID %>').show(); break;
                case "3": $("#divPage").show(); $('#<%= ddlPages.ClientID %>').show(); break;
                case "4": $("#divFile").show(); $('#<%= ddlFiles.ClientID %>').show(); break;
                case "5": $("#divUpload").show(); break;
            }
        });
    $(document).ready(function () {
        setUpMyTabs();
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () { setUpMyTabs(); });
    });
<div id="divAddDocument">
    <asp:Label ID="lblName" runat="server">Document Name</asp:Label>
</script>
<div id="divAddDocument">
    <asp:Label ID="lblName" runat="server">Document Name</asp:Label>
    <asp:TextBox ID="tbName" runat="server"></asp:TextBox>
    <br />
    <asp:Label ID="lblDescription" runat="server">Document Description</asp:Label>
    <asp:TextBox ID="tbDescription" runat="server"></asp:TextBox>
    <asp:RadioButtonList ID="rbLinkType" runat="server">
        <asp:ListItem Text="External URL" Value="0" Selected="True" />
        <asp:ListItem Text="YouTube Video" Value="1" />
        <asp:ListItem Text="Google Doc" Value="2" />
        <asp:ListItem Text="A Page on this site" Value="3" />
        <asp:ListItem Text="A File on this site" Value="4" />
        <asp:ListItem Text="Upload a new file" Value="5" />
    </asp:RadioButtonList>
    <div id="divURL" class="docOption">
        <asp:Label ID="lblURL" runat="server" ResourceKey="lblURL" />
    <div id="divYouTube" class="docOption" style="display: none">
        <asp:Label ID="lblYouTube" runat="server" ResourceKey="lblYouTube" />
    </div>
    <div id="divGoogle" class="docOption" style="display: none">
        <asp:Label ID="lblGoogle" runat="server" ResourceKey="lblGoogle" />
    </div>
    <div id="divPage" class="docOption" style="display: none">
        <asp:Label ID="lblPage" runat="server" ResourceKey="lblPage" />
    </div>
    <div id="divFile" class="docOption" style="display: none">
        <asp:Label ID="lblFile" runat="server" ResourceKey="lblFile" />
    </div>
    <div id="divUpload" class="docOption" style="display: none">
        <asp:FileUpload ID="FileUpload1" runat="server" />
    </div>

    <asp:DropDownList ID="ddlFiles" runat="server" Style="display: none;">
    </asp:DropDownList>
    <asp:DropDownList ID="ddlPages" runat="server" Style="display: none;">
    </asp:DropDownList>
    <asp:TextBox ID="tbURL" runat="server"></asp:TextBox>

    <div style="margin-top: 12px">
        <asp:Button ID="btnUploadFiles" runat="server" Text="Upload" />
        <asp:Button ID="btnNewLink" runat="server" Text="Add Link" />
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" />
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" />
    </div>
</div>
