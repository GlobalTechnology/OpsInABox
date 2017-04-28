<%@ Control Language="vb" AutoEventWireup="false" Explicit="True" %>
<%@ Register TagPrefix="dnn" TagName="LOGO" Src="~/Admin/Skins/Logo.ascx" %>
<%@ Register TagPrefix="dnn" TagName="USER" Src="~/Admin/Skins/User.ascx" %>
<%@ Register TagPrefix="dnn" TagName="LOGIN" Src="~/Admin/Skins/Login.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SEARCH" Src="~/DesktopModules/AgapeFR/Search/Search.ascx" %>
<%@ Register TagPrefix="ddr" Namespace="DotNetNuke.Web.DDRMenu.TemplateEngine" Assembly="DotNetNuke.Web.DDRMenu" %>
<%@ Register TagPrefix="ddr" TagName="MENU" src="~/DesktopModules/DDRMenu/Menu.ascx" %>
<meta name="viewport" content="width=device-width, initial-scale=1.0">
<script runat="server">
    Protected Function Translate(ResourceKey As String) As String
        Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
        Dim strFile As String = System.IO.Path.GetFileName(Server.MapPath(PS.ActiveTab.SkinSrc))
        strFile = PS.ActiveTab.SkinPath + Localization.LocalResourceDirectory + "/" + strFile
        Return Localization.GetString(ResourceKey, strFile)
    End Function

</script>
<script src="/js/jquery.watermarkinput.js" type="text/javascript"></script>
<script type="text/javascript">
    (function ($, Sys) {
        function initSearchText() {
            $('#SearchContainer span input[type=text]').Watermark('<%=Translate("SearchWatermark.Text")%>');
        }
        $(document).ready(function () {
            initSearchText();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                initSearchText();
            });
        });
    }(jQuery, window.Sys));
</script>
<div id="mySidenav" class="sidenav">
    <ddr:MENU ID="MENU1" MenuStyle="/templates/AgapeFRMenu/" NodeSelector="*,0,+1" runat="server" includehidden="false" />
</div>
<div id="controlPanelContainer">
    <div id="ControlPanel" runat="server" />
</div>
<div class="bar">
    <div id="header" class="centeredbox bar1">
        <div id="logoHeader">
            <dnn:LOGO runat="server" ID="dnnLOGOHEADER" />
        </div>
        <div id="menu">
            <div id="featuredlinks">
                <span>Faire un Don | Boutique</span>
            </div>
            <div id="menubuttons" class="openbtn">
                <svg class="searchicon" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="50px" version="1.1" height="50px" viewBox="0 0 64 64" enable-background="new 0 0 64 64">
                  <g>
                    <path class="stripe" d="M25.915,52.205c6.377,0,12.206-2.344,16.708-6.196L59.98,63.365c0.392,0.391,0.904,0.587,1.418,0.587   c0.513,0,1.025-0.196,1.418-0.587c0.392-0.393,0.588-0.904,0.588-1.418s-0.196-1.027-0.588-1.419L45.459,43.172   c3.853-4.5,6.197-10.331,6.197-16.707c0-14.194-11.549-25.741-25.741-25.741c-14.194,0-25.742,11.547-25.742,25.741   C0.173,40.658,11.721,52.205,25.915,52.205z M25.915,4.735c11.98,0,21.729,9.747,21.729,21.729c0,11.98-9.749,21.729-21.729,21.729   c-11.981,0-21.73-9.748-21.73-21.729C4.185,14.482,13.934,4.735,25.915,4.735z"/>
                  </g>
                </svg>
                <svg class="menubtn" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="50px" version="1.1" height="50px" viewBox="0 0 64 64" enable-background="new 0 0 64 64">
                    <g>
                        <path class="stripe" d="M2.252,10.271h58.871c1.124,0,2.034-0.91,2.034-2.034c0-1.123-0.91-2.034-2.034-2.034H2.252    c-1.124,0-2.034,0.911-2.034,2.034C0.218,9.36,1.128,10.271,2.252,10.271z"/>
                        <path class="stripe" d="m61.123,30.015h-58.871c-1.124,0-2.034,0.912-2.034,2.035 0,1.122 0.91,2.034 2.034,2.034h58.871c1.124,0 2.034-0.912 2.034-2.034-7.10543e-15-1.123-0.91-2.035-2.034-2.035z"/>
                        <path class="stripe" d="m61.123,53.876h-58.871c-1.124,0-2.034,0.91-2.034,2.034 0,1.123 0.91,2.034 2.034,2.034h58.871c1.124,0 2.034-0.911 2.034-2.034-7.10543e-15-1.124-0.91-2.034-2.034-2.034z"/>
                    </g>
                </svg>
            </div>
        </div>
    </div>
</div>
<script>
$(".menubtn").click(function() { //hide and show nav menu
  $("#mySidenav").toggleClass("opensidenav");
  $("#menubuttons").toggleClass("closebtn");
  $("#menubuttons").toggleClass("openbtn");
});

$(".menudrop").click(function () { //hide and show second level menu
    $(".parent").not($(this).parent()).removeClass("menuopen");
    $(this).parent().toggleClass("menuopen");
    
});</script>