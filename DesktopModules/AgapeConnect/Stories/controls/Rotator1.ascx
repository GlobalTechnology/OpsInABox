<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Rotator1.ascx.vb" Inherits="DotNetNuke.Modules.AgapeConnect.Stories.Rotator1" %>
<script src="/js/jquery.nivo.slider.js" type="text/javascript"></script>
<link href="/DesktopModules/AgapeConnect/Stories/themes/default/default.css" rel="stylesheet" type="text/css" media="screen"  />
<link href="/js/nivo-slider.css" rel="stylesheet" type="text/css" media="screen"  />

 <script type="text/javascript">
     (function ($, Sys) {
         function setUpMyTabs() {
             $('#slider').nivoSlider({
                 effect: 'fade',
                 pauseTime: <%= PauseTime %>,
                 width: <%= divWidth %>
             });

             var w= $('#rotatorContainer').width();
             var scale = w/<%= divWidth %>;
             var offset = (1.0- scale) *50;
             var newH = <%= divHeight%> * scale;
             $('#rotatorContainer').css({
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


<div id="sampleDiv" width="100%" style="height:0;"></div>
<div id="rotatorContainer" >
<div style="width: <%= divWidth %>px; height: <%= divHeight%>px; left:0; ">
    <div class="slider-wrapper theme-default">
        
        <div id="slider" class="nivoSlider" style="height: <%= divHeight%>px; max-height: <%= divHeight%>px;  background-color: Black; overflow: hidden;">
          <%--  <div style="height: 420px; width: 420px; #position: absolute; #top: 50%;display: table-cell; vertical-align: middle;">
              <div class="greenBorder" style="#position: relative; #top: -50%">--%>
            <asp:Literal ID="ltStories" runat="server"></asp:Literal>
            </div>
            </div>
       
        </div>
      </div>


