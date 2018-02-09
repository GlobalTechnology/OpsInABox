<%@ Control Language="VB" AutoEventWireup="false" CodeFile="TagList-Fr.ascx.vb" Inherits="DotNetNuke.Modules.AgapeConnect.Stories.TagList_Fr" %>

<div id="TagList">
    <asp:Repeater runat="server" ID="dlTags">
        <ItemTemplate>
            <asp:HyperLink ID="lnkLink" runat="server" CssClass="CommandButton col-4 col-m-6" 
                NavigateUrl='<%# NavigateURL() & "?tags=" + HttpUtility.UrlEncode(Eval("TagName"))%>'>
                <div class="container">
                    <div class="item">
                        <asp:Image ID="imgPhoto" runat="server" ImageUrl='<%# GetImageURL(Eval("PhotoId"))%>' class="tagThumbnail" />
                    </div>
                    <div class="item">
                        <h4><asp:Label ID="HyperLink1" runat="server" Text='<%# Eval("TagName")%>' /></h4>
                    </div>
                </div>
            </asp:HyperLink>
        </ItemTemplate>
    </asp:Repeater>
</div>
