<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Rotator2-Fr.ascx.vb" Inherits="DotNetNuke.Modules.AgapeConnect.Stories.Rotator_Fr" %>
<script src="/js/jquery.nivo.slider.js" type="text/javascript"></script>
<link href="/js/nivo-slider.css" rel="stylesheet" type="text/css" media="screen"  />
<link href="/DesktopModules/AgapeConnect/Stories/themes/default/france.css" rel="stylesheet" type="text/css" media="screen"  />

 <script type="text/javascript">
     (function ($, Sys) {
         function setUpMyTabs() {
             $('#slider<%= hfChannelId.Value %>').css({
                 'visibility':'visible',
                 'height': <%= hfDivHeight.Value %>,
                 'max-height': <%= hfDivHeight.Value %>}).nivoSlider({
                 effect: 'fade',
                 pauseTime: <%= hfPauseTime.Value %>,
                 width: <%= hfDivWidth.Value %>,
                 manualAdvance: <%= hfManualAdvance.Value %>,
                 manualCaption: true,
             });

             var w= $('#rotatorContainer<%= hfChannelId.Value %>').width();
             var scale = w/<%= hfDivWidth.Value %>;
             var offset = (1.0- scale) *50;
             var newH = <%= hfDivHeight.Value %> * scale;
             $('#rotatorContainer<%= hfChannelId.Value %>').css({
                 'transform': 'translate(-' + offset + '%, -' + offset + '%) scale(' + scale + ')' ,
                 '-ms-transform': 'translate(-' + offset + '%, -' + offset + '%) scale(' + scale + ')' ,
                 '-moz-transform': 'translate(-' + offset + '%, -' + offset + '%) scale(' + scale + ')' ,
                 '-webkit-transform': 'translate(-' + offset + '%, -' + offset + '%) scale(' + scale + ')' ,
                 '-o-transform': 'translate(-' + offset + '%, -' + offset + '%) scale(' + scale + ')' ,
                 'width':<%= hfDivWidth.Value %> +'px', 'height':(newH-8) + 'px' });
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
</script>
<asp:HiddenField ID="hfManualAdvance" runat="server" />
<asp:HiddenField ID="hfPauseTime" runat="server" />
<asp:HiddenField ID="hfDivWidth" runat="server" />
<asp:HiddenField ID="hfDivHeight" runat="server" />
<asp:HiddenField ID="hfChannelId" runat="server" />

<div id="rotator<%= hfChannelId.Value %>" class="rotator2">
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
                    style=<%# Eval(RotatorConstants.SLIDEIMAGESTYLE) %> 
                    runat="server" />
            </asp:HyperLink>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    
</div>
    <div id="manual-nivo-caption<%= hfChannelId.Value %>" class="nivo-caption"></div>
</div>
