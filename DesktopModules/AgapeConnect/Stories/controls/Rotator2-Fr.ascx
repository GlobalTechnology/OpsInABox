<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Rotator2-Fr.ascx.vb" Inherits="DotNetNuke.Modules.AgapeConnect.Stories.Rotator_Fr" %>
<script src="/js/jquery.nivo.slider.js" type="text/javascript"></script>
<link href="/js/nivo-slider.css" rel="stylesheet" type="text/css" media="screen"  />
<link href="/DesktopModules/AgapeConnect/Stories/themes/default/france.css" rel="stylesheet" type="text/css" media="screen"  />

 <script type="text/javascript">
     (function ($, Sys) {
         function setUpMyTabs() {
             $('#slider<%= hfChannelId.Value %>').css({
                 'visibility':'visible',
                 'height': <%= divHeight%>,
                 'max-height': <%= divHeight%>}).nivoSlider({
                 effect: 'fade',
                 pauseTime: <%= PauseTime %>,
                 width: <%= divWidth %>,
                 manualAdvance: <%= manualAdvance %>,
                // beforeChange: function() {hlBeforeChange()},
                 //afterChange: function (){hlAfterChange()}
             });

             var w= $('#rotatorContainer<%= hfChannelId.Value %>').width();
             var scale = w/<%= divWidth %>;
             var offset = (1.0- scale) *50;
             var newH = <%= divHeight%> * scale;
             $('#rotatorContainer<%= hfChannelId.Value %>').css({
                 'transform': 'translate(-' + offset + '%, -' + offset + '%) scale(' + scale + ')' ,
                 '-ms-transform': 'translate(-' + offset + '%, -' + offset + '%) scale(' + scale + ')' ,
                 '-moz-transform': 'translate(-' + offset + '%, -' + offset + '%) scale(' + scale + ')' ,
                 '-webkit-transform': 'translate(-' + offset + '%, -' + offset + '%) scale(' + scale + ')' ,
                 '-o-transform': 'translate(-' + offset + '%, -' + offset + '%) scale(' + scale + ')' ,
                 'width':<%= divWidth %> +'px', 'height':(newH-8) + 'px' });
         }

         $(document).ready(function () {
             setUpMyTabs();
             Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                 setUpMyTabs();
             });
         });
     } (jQuery, window.Sys));
   

     function hlBeforeChange()
     {
             $('.theme-default a.nivo-imageLink.playbutton').css({opacity:0})
     }

     function hlAfterChange()
     {
         $('.theme-default a.nivo-imageLink.playbutton').css({opacity:0})
     }

   function registerClick(c)
   {
        $.ajax({ type: 'POST', url: "<%= NavigateURL() %>",
                        data: ({ StoryLink: c })
                    });
   }
</script>

<asp:HiddenField ID="hfChannelId" runat="server" />

<div id="rotatorContainer<%= hfChannelId.Value %>" class="theme-default">
    <div id="slider<%= hfChannelId.Value %>" class="nivoSlider">
        <asp:Repeater ID="SliderImageList" runat="server">
            <ItemTemplate>
            <asp:HyperLink 
                href=<%# Eval("sliderLink") %>
                ID="hlImageSlider"
                CssClass = <%# Eval("sliderLinkImageCSS") %>
                runat="server">
                <asp:Image
                    src=<%# Eval("sliderImage") %> 
                    alt=<%# Eval("sliderImageAltText") %> 
                    title=<%# Eval("sliderImageTitle") %> 
                    style=<%# Eval("sliderImageStyle") %> 
                    runat="server" />
            </asp:HyperLink>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</div>

