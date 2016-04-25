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

<div id="ListFullWidth">
<asp:DataList runat="server" ID="dlStories" AllowPaging="true" Width="100%">
    <ItemTemplate>
        <asp:HyperLink ID="hlStory" runat="server"
            NavigateUrl='<%# "javascript: registerClick(" & DataBinder.Eval(Container.DataItem, "CacheId") & ", """ & CStr(DataBinder.Eval(Container.DataItem, "Link")) & """); "%>'>
            <div class="items" runat="server">
                <div class="item">
                    <h4><asp:Label ID="lblStoryTitle" runat="server"  Text='<%# Eval("Headline")%>' /></h4>
                    <p><asp:Label ID="lblStoryPreview" runat="server" Text='<%# Eval("Description")%>' /></p>
                </div>
                <div class="item">
                    <asp:Image ID="StoryThumbnail" runat="server" CssClass="thumbnail" ImageUrl='<%# Eval("ImageId")  %>' />
                </div>
            </div>
        </asp:HyperLink>
    </ItemTemplate>
</asp:DataList>
</div>

<div style="width: 100%; text-align: center; margin-top: 20px;">
    <asp:Hyperlink ID="btnPrev" runat="server" Text="Previous" CssClass="button" resourceKey="btnPrevious" Visible="false" Width="80px"/>
    <asp:Hyperlink ID="btnNext" runat="server" Text="Next" CssClass="button" resourceKey="btnNext"  Visible="false" Width="80px"/>
</div>
<div style="clear: both;"></div>
<asp:Literal ID="ltPagination" runat="server"></asp:Literal>
