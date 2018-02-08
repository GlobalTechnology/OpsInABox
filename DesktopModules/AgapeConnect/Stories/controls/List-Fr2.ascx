<%@ Control Language="VB" AutoEventWireup="false" CodeFile="List-Fr2.ascx.vb" Inherits="DotNetNuke.Modules.AgapeConnect.Stories.List_Fr" %>
<script src="/js/jquery.nivo.slider.js" type="text/javascript"></script>
<link href="/DesktopModules/AgapeConnect/Stories/themes/default/default.css" rel="stylesheet" type="text/css" media="screen" />
<link href="/js/nivo-slider.css" rel="stylesheet" type="text/css" media="screen" />

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

<div class="afnewslist">
    <asp:Repeater runat="server" ID="dlStories">
        <ItemTemplate>
            <div class="afnews col-6 col-m-6">
                <asp:HyperLink ID="lnkLink" runat="server" CssClass="CommandButton" NavigateUrl='<%# "javascript: registerClick(" & DataBinder.Eval(Container.DataItem, "CacheId") & ", """ &   CStr(DataBinder.Eval(Container.DataItem, "Link")) & """); "%>'>
                    <div>
                        <asp:Image ID="imgImage" runat="server" ImageUrl='<%# Eval("ImageId")  %>' CssClass="seachImage" />
                    </div>
                    <div style="clear: both;"></div>
                        <h4><asp:Label ID="HyperLink1" runat="server" CssClass="storyTitle"  Text='<%# Eval("Headline")%>' /></h4>
                </asp:HyperLink>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>

<div style="width: 100%; text-align: center; margin-top: 20px;">
<asp:Hyperlink ID="btnPrev" runat="server" Text="Previous" CssClass="button frpagearticle" resourceKey="btnPrevious" Visible="false"/>
<asp:Hyperlink ID="btnNext" runat="server" Text="Next" CssClass="button frpagearticle" resourceKey="btnNext"  Visible="false"/>
</div>
<asp:Literal ID="ltPagination" runat="server"></asp:Literal>