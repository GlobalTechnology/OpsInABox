<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Documents.ascx.vb" Inherits="DotNetNuke.Modules.AgapeConnect.Documents.Documents" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register Src="~/controls/urlcontrol.ascx" TagName="urlcontrol" TagPrefix="uc1" %>
<%@ Register Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" TagPrefix="cc1" %>

<script>

    (function ($, Sys) {

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
            setUpMyModule();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                setUpMyModule();
            });
        });

    }(jQuery, window.Sys)); // pass in the globals. Note the safe access of the jQuery object.

</script>

<div id="DocumentsMain" class="documents">
    <asp:UpdatePanel ID="upFolderView" runat="server">
            <ContentTemplate>
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
                                    NavigateUrl='<%# EditUrl("", "", "AddDocument", "edit", Eval("DocId"))%>'></asp:HyperLink>
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
