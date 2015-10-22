<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Documents.ascx.vb" Inherits="DotNetNuke.Modules.AgapeConnect.Documents.Documents" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register Src="~/controls/urlcontrol.ascx" TagName="urlcontrol" TagPrefix="uc1" %>
<%@ Register Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" TagPrefix="cc1" %>

<div style="width: 100%">
    <div id="cover" style="position: absolute; width: 100%; height: 100%; z-index: 1;
        top: 0px; left: 0px; cursor: move; background: red; display: none;">
        &nbsp;
    </div>
    <div id="MySplitter">
       
        <div id="RightPane" class="aBlank">
           <div style="clear: both;">
            </div>
            <br />
            <asp:ListView ID="dlFolderView" runat="server">
                <ItemTemplate>
                    <div id="Icons" runat="server" style="width:1000px">
                        <asp:HyperLink ID="HyperLink1" runat="server" ToolTip='<%# Eval("Description") & IIF(Eval("FileId") is nothing, "", vbnewline & "Author: " & Eval("Author"))  %>'
                            Target='<%# IIF((Eval("LinkType") =0 or Eval("LinkType")=2) and not Eval("FileId") is nothing, "_blank", "_self") %>'
                            NavigateUrl='<%#  IIF(Eval("FileId") is nothing, NavigateURL() & "?FolderId=" & Eval("FolderId"), GetFileUrl(Eval("DocId"), Eval("FileId")) ) %>'>
                            <div>
                                <asp:Image ID="icon" runat="server"
                                    ImageUrl='<%# GetFileIcon(Eval("FileId"), Eval("LinkType"), Eval("CustomIcon") ) %>'
                                    ToolTip='<%# IIf(Eval("FileId") Is Nothing, "", vbNewLine & "Author: " & Eval("Author"))%>' />
                                
                                <div style="padding: 0; margin: 0;">
                                    <asp:Label ID="lblItemName" runat="server"
                                        Text='<%# Eval("DisplayName") %>'></asp:Label>
                                </div>
                                <div id="theDesc" runat="server">
                                    <asp:Label ID="Label21" runat="server" CssClass="tblText" Text='<%# Eval("Description") %>'
                                        Style="word-wrap: break-word;"></asp:Label>
                                </div>
                            </div>
                        </asp:HyperLink>
                    </div>
                </ItemTemplate>
            </asp:ListView>
        </div>
    </div>
</div>
<div style="clear: both;" />
