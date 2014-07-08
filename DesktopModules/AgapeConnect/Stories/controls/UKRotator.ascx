<%@ Control Language="VB" AutoEventWireup="false" CodeFile="UKRotator.ascx.vb" Inherits="DotNetNuke.Modules.AgapeConnect.Stories.controls.UKRotator" %>
<script type="text/javascript" src="../../js/cSlider/c_slider.js"></script>
<link href="/DesktopModules/AgapeConnect/Stories/themes/default/default.css" rel="stylesheet" type="text/css" media="screen" />
<link rel="stylesheet" type="text/css" href="/js/cSlider/chris_slider.css" media="screen" />
<script type="text/javascript">
    (function ($, Sys) {
        function setUpMySlider() {
            $('#slider').cSlider({
                animSpeed: '1000',
                pauseTime: '<%= PauseTime %>'
            });
        }
        $(document).ready(function () {
            setUpMySlider();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                setUpMySlider();
            });
        });
    }(jQuery, window.Sys));
    function registerClick(c) {
        $.ajax({
            type: 'POST', url: "<%= NavigateURL() %>",
            data: ({ StoryLink: c })
        });
    }
</script>


<div style="width: <%= divWidth %>px; height: <%= divHeight%>px;">
    <div id="slider">
        <asp:Literal ID="ltStories" runat="server"></asp:Literal>
    </div>
</div>

