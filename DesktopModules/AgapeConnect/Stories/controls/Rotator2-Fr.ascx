<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Rotator2-Fr.ascx.vb" Inherits="DotNetNuke.Modules.AgapeConnect.Stories.Rotator_Fr" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<dnn:DnnJsInclude runat="server" FilePath="/js/jquery.nivo.slider.js" />
<dnn:DnnJsInclude runat="server" FilePath="/DesktopModules/AgapeConnect/Stories/js/jquery.mobile.custom.touch.min.js" />
<dnn:DnnCssInclude runat="server" FilePath="/js/nivo-slider.css" />

<script type="text/javascript">
    (function ($, Sys) {
        function setUpMyTabs() {
            $('#slider<%= hfChannelId.Value %>').css({
                 'visibility': 'visible'
             }).nivoSlider({
                 effect: 'fade',
                 pauseTime: <%= hfPauseTime.Value %>,
                     manualAdvance: <%= hfManualAdvance.Value %>,
                     manualCaption: true,
                     channelID: <%= hfChannelId.Value %>,
                     beforeChange: function () { linkImageFadeOut('#slider<%= hfChannelId.Value %>'); },
                    afterChange: function () { loadAddThis(); },
                });
            $(".rotator2").on("swipeleft", function () {
                $(".rotator2 .nivo-nextNav").trigger("click"); //next slide
            });
            $(".rotator2").on("swiperight", function () {
                $(".rotator2 .nivo-prevNav").trigger("click"); //previous slide
            });
        }

        $(document).ready(function () {
            setUpMyTabs();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                setUpMyTabs();
            });
        });
    }(jQuery, window.Sys));

    function registerClick(c) {
        $.ajax({
            type: 'POST', url: "<%= NavigateURL() %>",
            data: ({ StoryLink: c })
        });
    }

    // Fades out the play button immediately
    function linkImageFadeOut(sliderId) {
        $(sliderId + ' a.nivo-imageLink')
            .fadeOut(0)
            ;
    };

    function loadAddThis() {
        addthis.toolbox('.addthis_toolbox')
    }
</script>

<asp:HiddenField ID="hfManualAdvance" runat="server" />
<asp:HiddenField ID="hfPauseTime" runat="server" />
<asp:HiddenField ID="hfChannelId" runat="server" />

<div id="rotator<%= hfChannelId.Value %>" class="rotator2">
<div id="rotatorContainer<%= hfChannelId.Value %>" class="theme-default">
    <div id="slider<%= hfChannelId.Value %>" class="nivoSlider">
        <asp:Repeater ID="SliderImageList" runat="server">
            <ItemTemplate>
            <asp:HyperLink 
                href=<%# Eval(ControlerConstants.SLIDERAWURL) %>
                Onclick=<%# Eval(ControlerConstants.SLIDELINK) %>
                ID="hlImageSlider"
                CssClass = <%# Eval(ControlerConstants.SLIDEIMAGECSS) %>
                runat="server">
                <asp:Image
                    src=<%# Eval(ControlerConstants.SLIDEIMAGE) %> 
                    alt=<%# Eval(ControlerConstants.SLIDEIMAGEALTTEXT) %>
                    title=<%# Eval(ControlerConstants.SLIDEIMAGETITLE) %> 
                    runat="server" />
            </asp:HyperLink>
                
            </ItemTemplate>
        </asp:Repeater>
    </div>
</div>
    <div id="manual-nivo-caption<%= hfChannelId.Value %>" class="nivo-caption"></div>
    <div class="no-stories">
        <asp:Label ID="lblNoStories" runat="server" ResourceKey="lblNoStories" Visible="false"></asp:Label>
    </div>
</div>