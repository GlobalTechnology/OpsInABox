<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Documents.ascx.vb" Inherits="DotNetNuke.Modules.AgapeConnect.Documents.Documents" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register Src="~/controls/urlcontrol.ascx" TagName="urlcontrol" TagPrefix="uc1" %>
<%@ Register Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" TagPrefix="cc1" %>
<div id="DocumentsMain" class="documents">
    <asp:ListView ID="dlFolderView" runat="server">
        <ItemTemplate>
            <div id="Icons" class="icons" runat="server">
                <asp:HyperLink ID="HyperLink1" runat="server"
                    Target='<%# IIF((Eval("LinkType") =0 or Eval("LinkType")=2) and not Eval("FileId") is nothing, "_blank", "_self") %>'
                    NavigateUrl='<%# IIf(Eval("FileId") Is Nothing, NavigateURL() & "?FolderId=" & Eval("FolderId"), GetFileUrl(Eval("DocId"), Eval("FileId")))%>'>
                    <div>
                        <asp:Image ID="icon" CssClass="icon" runat="server" ImageUrl='<%# DocumentsController.GetFileIcon(Eval("FileId"), Eval("LinkType"), Eval("CustomIcon"))%>' />
                        <div class="docInfo">
                            <div>
                                <asp:Label ID="lblItemName" CssClass="docTitle" runat="server" Text='<%# Eval("DisplayName") %>'></asp:Label>
                            </div>
                            <div id="theDesc" runat="server">
                                <asp:Label ID="Label21" runat="server" CssClass="docText" Text='<%# Eval("Description") %>'></asp:Label>
                            </div>
                        </div>
                    </div>
                </asp:HyperLink>
                <div id="docButtons" class="docButtons" runat="server">
                    <asp:HyperLink ID="btnEditDoc" CssClass="btnEditDoc" runat="server"></asp:HyperLink>
                    <asp:HyperLink ID="btnDeleteDoc" CssClass="btnDeleteDoc" runat="server"></asp:HyperLink>
                </div>
            </div>
        </ItemTemplate>
    </asp:ListView>
</div>
