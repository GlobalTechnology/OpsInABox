<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ListFullWidth-Fr.ascx.vb" Inherits="DotNetNuke.Modules.AgapeConnect.Stories.ListFullWidth_Fr" %>
<link href="/DesktopModules/AgapeConnect/Stories/themes/default/default.css" rel="stylesheet" type="text/css" media="screen" />

<script type="text/javascript">
    (function ($, Sys) {
        function setUpMyTabs() {
            $(".tagFilter").click(function () {
                var querystring = "";
                $(".tagFilter input:checked").each(function () {
                    querystring += $(this).next().text() + ",";
                });
                window.location.href = "<%= NavigateURL() & "?tags="%>" + querystring;
            });
        }

        $(document).ready(function () {
            setUpMyTabs();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                setUpMyTabs();
            });
        });
    }(jQuery, window.Sys));

    function registerClick(c, l) {
        $.ajax({
            type: 'POST', url: "<%= NavigateURL() %>",
            data: ({ StoryLink: c })
        });
        var target = "_blank"
        if (l.indexOf("<%= PortalSettings.DefaultPortalAlias %>") >= 0)
            target = "_self"
        window.open(l, target);
    }
</script>

<asp:DataList runat="server" ID="dlStories" AllowPaging="true" RepeatColumns="2" BorderStyle="None" CellSpacing="4" CellPadding="4" ShowHeader="False" GridLines="None" PagerStyle-Visible="false" ItemStyle-Width="50%" CssClass="StoriesList">
    <ItemStyle CssClass="dnnGridItem" HorizontalAlign="Left" VerticalAlign="Top" Width="50%"  />
    <AlternatingItemStyle CssClass="dnnGridItem" />
    <FooterStyle CssClass="dnnGridFooter" />
    <ItemTemplate>
        <asp:HyperLink ID="lnkLink" runat="server" CssClass="CommandButton" NavigateUrl='<%# "javascript: registerClick(" & DataBinder.Eval(Container.DataItem, "CacheId") & ", """ & CStr(DataBinder.Eval(Container.DataItem, "Link")) & """); "%>'>
            <div>
                <asp:Image ID="imgImage" runat="server" ImageUrl='<%# Eval("ImageId")  %>' CssClass="seachImage" />
            </div>
            <div style="clear: both;"></div>
            <h4><asp:Label ID="HyperLink1" runat="server" CssClass="storyTitle"  Text='<%# Eval("Headline")%>' /></h4>
        </asp:HyperLink>
    </ItemTemplate>
</asp:DataList>

<div style="width: 100%; text-align: center; margin-top: 20px;">
    <asp:Hyperlink ID="btnPrev" runat="server" Text="Previous" CssClass="button" resourceKey="btnPrevious" Visible="false" Width="80px"/>
    <asp:Hyperlink ID="btnNext" runat="server" Text="Next" CssClass="button" resourceKey="btnNext"  Visible="false" Width="80px"/>
</div>
<div style="clear: both;"></div>
<asp:Literal ID="ltPagination" runat="server"></asp:Literal>

<%--<style type="text/css">
    
    a.button, a.button:visited{
       text-decoration: none;
    }
    #<%= dlStories.ClientId%> td{

    width: 50%;
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
    .storyTitle {
        
    }
        <%--.dnnGridItem:hover .storyTitle,.dnnGridAltItem:hover .storyTitle  {
            text-decoration: underline;
        }

    .dnnGridItem:hover, .dnnGridAltItem:hover {
        border: 2px solid lightgrey;
           text-decoration: none;
           background-color: #f1f1f1;
        }

    .dnnGridItem, .dnnGridAltItem {
        border: 2px inset transparent;
        max-width: 50%;
    }

    .mynavbar-inner {
        min-height: 30px;
        padding-right: 20px;
        padding-left: 20px;
        padding-top: 5px;
        background-color: #fafafa;
        background-image: -moz-linear-gradient(top, #ffffff, #f2f2f2);
        background-image: -webkit-gradient(linear, 0 0, 0 100%, from(#ffffff), to(#f2f2f2));
        background-image: -webkit-linear-gradient(top, #ffffff, #f2f2f2);
        background-image: -o-linear-gradient(top, #ffffff, #f2f2f2);
        background-image: linear-gradient(to bottom, #ffffff, #f2f2f2);
        background-repeat: repeat-x;
        border: 1px solid #d4d4d4;
        -webkit-border-radius: 4px;
        -moz-border-radius: 4px;
        border-radius: 4px;
        filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#ffffffff', endColorstr='#fff2f2f2', GradientType=0);
        -webkit-box-shadow: 0 1px 4px rgba(0, 0, 0, 0.065);
        -moz-box-shadow: 0 1px 4px rgba(0, 0, 0, 0.065);
        box-shadow: 0 1px 4px rgba(0, 0, 0, 0.065);
    }

    .mynavbar-search .search-query {
        padding: 4px 14px;
        margin-bottom: 0;
        font-family: "Helvetica Neue", Helvetica, Arial, sans-serif;
        font-size: 13px;
        font-weight: normal;
        line-height: 1;
        -webkit-border-radius: 15px;
        -moz-border-radius: 15px;
        border-radius: 15px;
    }

    .pagination ul > li:first-child > a, .pagination ul > li:first-child > span {
        border-left-width: 1px;
        -webkit-border-bottom-left-radius: 4px;
        border-bottom-left-radius: 4px;
        -webkit-border-top-left-radius: 4px;
        border-top-left-radius: 4px;
        -moz-border-radius-bottomleft: 4px;
        -moz-border-radius-topleft: 4px;
    }

    .pagination ul > li > a:hover, .pagination ul > li > a:focus, .pagination ul > .active > a, .pagination ul > .active > span {
        background-color: #f5f5f5;
    }

    .pagination ul > li > a, .pagination ul > li > span {
        float: left;
        padding: 4px 12px;
        line-height: 20px;
        text-decoration: none;
        background-color: #fff;
        border: 1px solid #ddd;
        border-left-width: 0;
    }

    .pagination ul > li {
        display: inline;
    }

    .pagination ul {
        display: inline-block;
        margin-bottom: 0;
        margin-left: 0;
        -webkit-border-radius: 4px;
        -moz-border-radius: 4px;
        border-radius: 4px;
        -webkit-box-shadow: 0 1px 2px rgba(0,0,0,0.05);
        -moz-box-shadow: 0 1px 2px rgba(0,0,0,0.05);
        box-shadow: 0 1px 2px rgba(0,0,0,0.05);
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

    Inherited from div.pagination.pagination-centered .pagination-centered {
        text-align: center;
    }

    .pagination-centered {
        text-align: center;
    }

    .pagination {
        margin: 20px 0;
    }

        .pagination ul > .active > a, .pagination ul > .active > span {
            color: #999;
            cursor: default;
        }

        .pagination ul > li > a:hover, .pagination ul > li > a:focus, .pagination ul > .active > a, .pagination ul > .active > span {
            background-color: #f5f5f5;
        }

        .pagination ul > .disabled > span, .pagination ul > .disabled > a, .pagination ul > .disabled > a:hover, .pagination ul > .disabled > a:focus {
            color: #999;
            cursor: default;
            background-color: transparent;
        }

    .tagFilter label {
        margin-left: 5px;
    }
</style>--%>

