<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ListFullWidth-Fr.ascx.vb" Inherits="DotNetNuke.Modules.AgapeConnect.Stories.ListFullWidth_Fr" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<dnn:DnnCssInclude runat="server" FilePath="/DesktopModules/AgapeConnect/Stories/themes/default/france.css" />

<script type="text/javascript">

   function registerClick(c)
   {
        $.ajax({ type: 'POST', url: "<%= NavigateURL() %>",
                        data: ({ StoryLink: c })
                    });
    }

    function popclosevideo() {
        $('.fr_video_popup').fadeOut();
        $('.ytopen iframe')[0].contentWindow.postMessage('{"event":"command","func":"' + 'pauseVideo' + '","args":""}', '*');
        $('.ytopen').removeClass('ytopen')
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

<div id="ListFullWidth">
<asp:Repeater runat="server" ID="dlStories">
    <ItemTemplate>
        <asp:HyperLink ID="hlStory" runat="server" href='<%# Eval(ControlerConstants.URL) %>' OnClick='<%# Eval(ControlerConstants.OPENLINK) %>'>
            <h4><asp:Label ID="lblStoryTitle" runat="server"  Text='<%# Eval(ControlerConstants.HEADLINE) %>' class="storyTitle" /></h4>
            <div class="frresourceimg">
                <asp:Image ID="StoryThumbnail" runat="server" CssClass="thumbnail" ImageUrl='<%# Eval(ControlerConstants.LINKIMAGE) %>' />
                <asp:Image ID="playbutton" visible='<%# Eval(ControlerConstants.LINKIMAGECSS) %>' runat="server" CssClass="playButton" ImageUrl='/DesktopModules/AgapeConnect/Stories/themes/default/playAspect2.png' />
            </div>
            <p><asp:Label ID="lblStoryPreview" runat="server" Text='<%# Eval(ControlerConstants.DESCRIPTION) %>' class="storyPreview" /></p>
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

<div class="afpagebuttons">
    <asp:Hyperlink ID="btnPrev" runat="server" Text="Previous" CssClass="button" resourceKey="btnPrevious" Visible="false" Width="80px"/>
    <asp:Hyperlink ID="btnNext" runat="server" Text="Next" CssClass="button" resourceKey="btnNext"  Visible="false" Width="80px"/>
</div>
