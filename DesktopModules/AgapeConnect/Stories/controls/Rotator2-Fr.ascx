<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Rotator2-Fr.ascx.vb" Inherits="DotNetNuke.Modules.AgapeConnect.Stories.Rotator_Fr" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<dnn:DnnJsInclude runat="server" FilePath="/js/jquery.nivo.slider.js" />
<dnn:DnnJsInclude runat="server" FilePath="/DesktopModules/AgapeConnect/Stories/js/jquery.mobile.custom.touch.min.js" />
<dnn:DnnCssInclude runat="server" FilePath="/js/nivo-slider.css" />
<dnn:DnnCssInclude runat="server" FilePath="/DesktopModules/AgapeConnect/Stories/themes/default/france.css" />

 <script type="text/javascript">
     (function ($, Sys) {
         function setUpMyTabs() {
             $('#slider<%= hfChannelId.Value %>').css({
                 'visibility':'visible'}).nivoSlider({
                 effect: 'fade',
                 pauseTime: <%= hfPauseTime.Value %>,
                 manualAdvance: <%= hfManualAdvance.Value %>,
                 manualCaption: true,
                 channelID: <%= hfChannelId.Value %>,
                 beforeChange: function(){linkImageFadeOut('#slider<%= hfChannelId.Value %>');},
                 afterChange: function(){loadAddThis();},
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
     } (jQuery, window.Sys));
   
   function registerClick(c)
   {
        $.ajax({ type: 'POST', url: "<%= NavigateURL() %>",
                        data: ({ StoryLink: c })
                    });
   }

     // Fades out the play button immediately
     function linkImageFadeOut(sliderId) {
         $(sliderId + ' a.nivo-imageLink')
         .fadeOut(0)
         ;};

     function loadAddThis() {
         addthis.toolbox('.addthis_toolbox')
     }
     function popclosevideo() {
    $('.fr_video_popup').fadeOut();
    //player.pauseVideo();
    $('.ytopen iframe')[0].contentWindow.postMessage('{"event":"command","func":"' + 'pauseVideo' + '","args":""}', '*');
    $('.ytopen').removeClass('ytopen')
    if (document.getElementById('slider' + currentvidslider) === null) { }
    else {      //restart the slider after the video is closed
        setTimeout("$('#slider' + currentvidslider).data('nivoslider').start()", 1000); 
    }
}

function popupvideo(videoid, sliderid) {
    currentvidslider = sliderid;

    currentvidid = videoid;
    $('#'+videoid).fadeIn();
    $('#' + videoid).css("display", "flex");
    $('#'+videoid).addClass("ytopen");
    
    if (document.getElementById('slider' + sliderid) == null) { }
    else {   //stop the slider while the video is open
        $('#slider' + sliderid).data('nivoslider').stop();
    }
}

$(document).keyup(function (e) {
    if (e.which == 27) {
        popclosevideo();
    }
});

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
    <asp:Repeater ID="SliderVideoPopup" runat="server">
        <ItemTemplate>
            <div id="<%# Eval(ControlerConstants.VIDEOID) %>" class="fr_video_popup">
                    <div class="playerspace">
                        <div style="text-align: right;">
                            <a class="fr_video_popup_close" onclick="popclosevideo()">
                                <svg width="16" version="1.1" xmlns="http://www.w3.org/2000/svg" height="16" viewBox="0 0 64 64" xmlns:xlink="http://www.w3.org/1999/xlink" enable-background="new 0 0 64 64">
                                    <g>
                                        <path class="cancelbutton" fill="#FFFFFF" d="M28.941,31.786L0.613,60.114c-0.787,0.787-0.787,2.062,0,2.849c0.393,0.394,0.909,0.59,1.424,0.59   c0.516,0,1.031-0.196,1.424-0.59l28.541-28.541l28.541,28.541c0.394,0.394,0.909,0.59,1.424,0.59c0.515,0,1.031-0.196,1.424-0.59   c0.787-0.787,0.787-2.062,0-2.849L35.064,31.786L63.41,3.438c0.787-0.787,0.787-2.062,0-2.849c-0.787-0.786-2.062-0.786-2.848,0   L32.003,29.15L3.441,0.59c-0.787-0.786-2.061-0.786-2.848,0c-0.787,0.787-0.787,2.062,0,2.849L28.941,31.786z" />
                                    </g>
                                </svg>
                            </a>
                        </div>    
                            <div class="youtube_player" videoid="<%# Eval(ControlerConstants.VIDEOID) %>" rel="0" controls="1" showinfo="0"></div>
                    </div>
                </div>
        </ItemTemplate>
    </asp:Repeater>
</div>
    <div id="manual-nivo-caption<%= hfChannelId.Value %>" class="nivo-caption"></div>
    <div class="no-stories">
        <asp:Label ID="lblNoStories" runat="server" ResourceKey="lblNoStories" Visible="false"></asp:Label>
    </div>
</div>