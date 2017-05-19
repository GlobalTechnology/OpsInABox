<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Rotator1-Fr.ascx.vb" Inherits="DotNetNuke.Modules.AgapeConnect.Stories.Rotator_Fr" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<dnn:DnnJsInclude runat="server" FilePath="/js/jquery.nivo.slider.js" />
<dnn:DnnJsInclude runat="server" FilePath="/DesktopModules/AgapeConnect/Stories/js/videopopup.js" />
<dnn:DnnCssInclude runat="server" FilePath="/js/nivo-slider.css" />
<dnn:DnnCssInclude runat="server" FilePath="/DesktopModules/AgapeConnect/Stories/themes/default/france.css" />

 <script type="text/javascript">
     (function ($, Sys) {
         function setUpMyTabs() {
             $('#slider<%= hfChannelId.Value %>').css({
                 'visibility':'visible'}).nivoSlider({
                 effect: 'fade',
                 pauseTime: <%= hfPauseTime.Value %>,
                 width: <%= hfDivWidth.Value %>,
                 manualAdvance: <%= hfManualAdvance.Value %>,
                 manualCaption: false,
                 channelID: <%= hfChannelId.Value %>,
                 beforeChange: function(){linkImageFadeOut('#slider<%= hfChannelId.Value %>');},
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

</script>

<asp:HiddenField ID="hfManualAdvance" runat="server" />
<asp:HiddenField ID="hfPauseTime" runat="server" />
<asp:HiddenField ID="hfDivWidth" runat="server" />
<asp:HiddenField ID="hfChannelId" runat="server" />

<div id="rotatorContainer<%= hfChannelId.Value %>" class="theme-default">
    <div id="slider<%= hfChannelId.Value %>" class="nivoSlider">
        <asp:Repeater ID="SliderImageList" runat="server">
            <ItemTemplate>
            <asp:HyperLink 
                href=<%# Eval(RotatorConstants.SLIDELINK) %>
                ID="hlImageSlider"
                CssClass = <%# Eval(RotatorConstants.SLIDEIMAGECSS) %>
                runat="server">
                <asp:Image
                    src=<%# Eval(RotatorConstants.SLIDEIMAGE) %> 
                    alt=<%# Eval(RotatorConstants.SLIDEIMAGEALTTEXT) %> 
                    title=<%# Eval(RotatorConstants.SLIDEIMAGETITLE) %> 
                    runat="server" />
            </asp:HyperLink>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</div>
<div class="no-stories">
    <asp:Label ID="lblNoStories" runat="server" ResourceKey="lblNoStories" Visible="false"></asp:Label>
</div>