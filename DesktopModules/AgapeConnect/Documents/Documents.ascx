<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Documents.ascx.vb" Inherits="DotNetNuke.Modules.AgapeConnect.Documents.Documents" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register Src="~/controls/urlcontrol.ascx" TagName="urlcontrol" TagPrefix="uc1" %>
<%@ Register Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" TagPrefix="cc1" %>

<div id="DocumentsMain" class="documents">
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
                            <asp:Label ID="Label21" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                        </div>
                    </div>
                </asp:HyperLink>
                <div id="docButtons" class="docButtons" runat="server">
                    <asp:HyperLink ID="btnEditDoc" CssClass="btnEdit" runat="server"></asp:HyperLink>
                    <asp:HyperLink ID="btnDeleteDoc" CssClass="btnDelete" runat="server"></asp:HyperLink>
                </div>
            </div>
        </ItemTemplate>
    </asp:ListView>
</div>
