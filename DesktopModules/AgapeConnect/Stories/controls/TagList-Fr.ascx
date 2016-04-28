<%@ Control Language="VB" AutoEventWireup="false" CodeFile="TagList-Fr.ascx.vb" Inherits="DotNetNuke.Modules.AgapeConnect.Stories.TagList_Fr" %>

<div id="TagList">
    <asp:DataList runat="server" ID="dlTags" AllowPaging="false" RepeatColumns="3" Width="100%">
        <ItemTemplate>
            <asp:HyperLink ID="lnkLink" runat="server" CssClass="CommandButton" 
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
    </asp:DataList>
</div>
