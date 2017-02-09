<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Rotator-Fr.ascx.vb" Inherits="DotNetNuke.Modules.AgapeConnect.Stories.Rotator_Fr" %>
<script src="/js/jquery.nivo.slider.js" type="text/javascript"></script>
<link href="/DesktopModules/AgapeConnect/Stories/themes/default/france.css" rel="stylesheet" type="text/css" media="screen"  />
<link href="/js/nivo-slider.css" rel="stylesheet" type="text/css" media="screen"  />

 <script type="text/javascript">
     (function ($, Sys) {
         function setUpMyTabs() {
             $('#slider<%= hfChannelId.Value %>').css({'visibility':'visible'}).nivoSlider({
                 effect: 'fade',
                 pauseTime: <%= PauseTime %>,
                 width: <%= divWidth %>,
                 manualAdvance: <%= manualAdvance %>,
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
   

   function registerClick(c)
   {
        $.ajax({ type: 'POST', url: "<%= NavigateURL() %>",
                        data: ({ StoryLink: c })
                    });
   }
  
</script>

<style type="text/css">
/*.overlay {
    position: absolute; 
    top: 0; 
    left: 10px;
    height: <%= divHeight +40 %>px;
    width: <%= divWidth %>px;
    background: rgba(255, 0, 0, 0.5);
    z-index: 100;
    background-image: url(/DesktopModules/AgapeConnect/Stories/images/thumb_down.png);
    background-repeat: no-repeat;
}*/

.slider-wrapper theme-default {
    width: <%= divWidth %>px;
    left:0;
    height: <%= divHeight + 40%>px !important;
}

.nivoSlider {
    height: <%= divHeight%>px;
    max-height: <%= divHeight%>px;
    background-color: Black;
    visibility: hidden;
    z-index: 3;
}

.slider-image-text {
    opacity: 1.0 !important;
    line-height: 42px;
}
    </style>

<asp:HiddenField ID="hfChannelId" runat="server" />

<div id="rotatorContainer<%= hfChannelId.Value %>">
    <div class="slider-wrapper theme-default">
        <div id="slider<%= hfChannelId.Value %>" class="nivoSlider">
            <asp:Repeater ID="SliderImageList" runat="server">
            <ItemTemplate>
            <asp:HyperLink Url=<%# Eval("sliderLink") %> CssClass="nivo-imageLink" ID="hlImageSlider" runat="server">
                <asp:Image 
                    src=<%# Eval("sliderImage") %> 
                    alt=<%# Eval("sliderImageAltText") %> 
                    data-title=<%# Eval("sliderImageDataTitle") %> 
                    style=<%# Eval("sliderImageStyle") %> 
                    runat="server" />
           <%--     <asp:Image <%# Eval("sliderOverlay") %> runat="server" />--%>
       <%--         ID=<%# Eval("sliderLink.ID") %> CssClass=<%# Eval("sliderLink.CssClass") %>--%>
            </asp:HyperLink>
                    </ItemTemplate>
                </asp:Repeater>
           <asp:Literal ID="ltStories" runat="server"></asp:Literal>
        </div>
    </div>
</div>

