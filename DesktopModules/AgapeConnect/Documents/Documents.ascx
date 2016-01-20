<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Documents.ascx.vb" Inherits="DotNetNuke.Modules.AgapeConnect.Documents.Documents" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register Src="~/controls/urlcontrol.ascx" TagName="urlcontrol" TagPrefix="uc1" %>
<%@ Register Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" TagPrefix="cc1" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<dnn:DnnJsInclude runat="server" FilePath="~/js/watermark.js" />
<script>
    // watermark in the ressources search textbox
    function initWatermark() {
        $(function () {
            var watermark = '<%=LocalizeString("tbWatermark")%>'
            var tbSearch = $('#<%=tbSearch.ClientID%>');
            var btnLoupe = $('#<%=lbSearchNew.ClientID%>');
            // first time
            if (tbSearch.val().length == 0)
                tbSearch.val(watermark).addClass('watermark');
            // when not in focus and no text in textbox
            tbSearch.blur(function () {
                if (tbSearch.val().length == 0)
                    tbSearch.val(watermark).addClass('watermark');
            });
            // when in focus and text in textbox is watermark 
            tbSearch.focus(function () {
                if (tbSearch.val() == watermark)
                    tbSearch.val('').removeClass('watermark');
            });
            // prevents from using the watermark as search text when loupe is pressed
            btnLoupe.click(function () {
                if (tbSearch.val() == watermark)
                    tbSearch.val('').removeClass('watermark');
            });
        })
    }

    function setUpMyModule() {
        //Initialize confirmation popup for delete buttons
        $('.btnDelete').each(function(){
            $(this).dnnConfirm({
                text: '<%=LocalizeString("deleteResourceConfirmationQuestion")%>'.replace("[RESOURCE]", $(this).closest('.icons').find('.docTitle').find('span').text()),
                yesText: '<%=LocalizeString("deleteResourceConfirmationYes")%>',
                noText: '<%=LocalizeString("deleteResourceConfirmationNo")%>',
                title: '<%=LocalizeString("deleteResourceConfirmationTitle")%>'
            });
        })
    }

    $(document).ready(function () {
        setUpMyModule(); initWatermark();
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            setUpMyModule(); initWatermark();
        });
    });
</script>

<div id="DocumentsMain" class="documents">

    <asp:UpdatePanel ID="upFolderView" runat="server">
        <ContentTemplate>
            <div id="SearchRessource">
                <asp:Panel ID="PanelSearch" runat="server" DefaultButton="lbSearchNew">
                    <asp:TextBox ID="tbSearch" runat="server" EnableViewState="False" CssClass="NormalTextBox" MaxLength="255"></asp:TextBox>
                    <asp:LinkButton ID="lbSearchNew" runat="server" CssClass="SearchNew" OnClick="SearchNew_OnClick"></asp:LinkButton>
                </asp:Panel>
            </div>
            <asp:ListView ID="dlFolderView" runat="server">
                <ItemTemplate>
                    <div id="Icons" class="icons" runat="server">
                        <asp:HyperLink ID="HyperLink1" runat="server"
                            Target='<%# GetDocTarget(Eval("DocId")) %>'
                            NavigateUrl='<%# IIf(Eval("FileId") Is Nothing, NavigateURL() & "?FolderId=" & Eval("FolderId"), GetDocUrl(Eval("DocId")))%>'>
                            <asp:Image ID="icon" CssClass="icon" runat="server" ImageUrl='<%# DocumentsController.GetFileIcon(Eval("FileId"), Eval("LinkType"), Eval("CustomIcon"))%>' />
                            <div class="docInfo">
                                <div class="docTitle">
                                    <asp:Label ID="lblItemName" runat="server" Text='<%# Eval("DisplayName") %>'></asp:Label>
                                </div>
                                <div id="theDesc" runat="server" class="docText">
                                    <asp:Label ID="lblItemDesc" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                                </div>
                            </div>
                        </asp:HyperLink>
                        <div id="docButtons" class="docButtons" runat="server">
                            <asp:HyperLink ID="btnEditDoc" CssClass="btnEdit" runat="server" 
                                NavigateUrl='<%# EditUrl("", "", DocumentsControllerConstants.AddEditDocumentControlKey, DocumentsControllerConstants.DocIdParamKey, Eval("DocId"), DocumentsControllerConstants.SearchWordsParamKey, SearchWords)%>'></asp:HyperLink>
                            <asp:LinkButton ID="btnDeleteDoc" CssClass="btnDelete" runat="server" CommandArgument='<%# Eval("DocId")%>'></asp:LinkButton>
                        </div>
                    </div>
                </ItemTemplate>
                <EmptyDataTemplate>
                    <div class="noresource"><%=LocalizeString("lblNoResource")%></div>
                </EmptyDataTemplate>
            </asp:ListView>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="dlFolderView" EventName="ItemCommand" />
        </Triggers>
    </asp:UpdatePanel>
</div>
