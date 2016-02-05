<%@ Control Language="VB" AutoEventWireup="false" CodeFile="TagList-Fr.ascx.vb" Inherits="DotNetNuke.Modules.AgapeConnect.Stories.TagList_Fr" %>

<style type="text/css">
    
    a.button, a.button:visited{
       text-decoration: none;
    }
    #<%= dlTags.ClientID%> td{

<%--    width: 50%;--%>
    }

    .StoriesList {
        width: 100%;
    }


    .dnnGridItem a:hover,.dnnGridAltItem a:hover  {
        text-decoration: none;
    }
    .dnnGridItem, .dnnGridAltItem {
        padding: 10px;
    }
    .buttonLeft {
        float: left;
    }
    .buttonRight {
        float: Right;
    }


    .seachImage {
        width: 100%;
        border: 0;
        margin-right: 5px;
    }

    .dnnGridItem:hover, .dnnGridAltItem:hover {
        border: 2px solid lightgrey;
           text-decoration: none;
           background-color: #f1f1f1;
        }

    .dnnGridItem, .dnnGridAltItem {
        border: 2px inset transparent;
        <%--max-width: 50%;--%>
    }

    ul, ol {
        padding: 0;
        margin: 0 0 10px 25px;
    }

    ul, ol {
        padding: 0;
        margin: 0 0 9px 25px;
    }

    user agent stylesheetul, menu, dir {
        display: block;
        list-style-type: disc;
        -webkit-margin-before: 1em;
        -webkit-margin-after: 1em;
        -webkit-margin-start: 0px;
        -webkit-margin-end: 0px;
        -webkit-padding-start: 40px;
    }

</style>

<asp:DataList runat="server" ID="dlTags" AllowPaging="false" RepeatColumns="3" RepeatDirection="Horizontal" BorderStyle="None" CellSpacing="4" CellPadding="4" ShowHeader="False" GridLines="None" PagerStyle-Visible="false" CssClass="StoriesList">
    <ItemStyle CssClass="dnnGridItem" HorizontalAlign="Left" VerticalAlign="Top"  />
    <AlternatingItemStyle CssClass="dnnGridItem" />
    <FooterStyle CssClass="dnnGridFooter" />
    <ItemTemplate>
        
        <asp:HyperLink ID="lnkLink" runat="server" CssClass="CommandButton" NavigateUrl=''>

            <div>
            <asp:Image ID="imgImage" runat="server" ImageUrl='<%# Eval("PhotoId")%>' CssClass="seachImage" />
<%--                Dim _theFile = FileManager.Instance.GetFile(_FileId)
        If _theFile Is Nothing Then
            theImage.ImageUrl = "/images/no_avatar.gif"
            _FileId = 0
            hfFileId.Value = 0
        ElseIf imgExt.Contains(_theFile.Extension.ToLower) Then
            theImage.ImageUrl = FileManager.Instance.GetUrl(_theFile)--%>
        </div>

        <div style="clear: both;"></div>
        <h4><asp:Label ID="HyperLink1" runat="server" CssClass="storyTitle"  Text='<%# Eval("TagName")%>' /></h4>



        </asp:HyperLink>


    </ItemTemplate>

</asp:DataList>
