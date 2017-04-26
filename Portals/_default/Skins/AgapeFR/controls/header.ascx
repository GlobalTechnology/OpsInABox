<%@ Control Language="vb" AutoEventWireup="false" Explicit="True" %>
<%@ Register TagPrefix="dnn" TagName="LOGO" Src="~/Admin/Skins/Logo.ascx" %>
<%@ Register TagPrefix="dnn" TagName="USER" Src="~/Admin/Skins/User.ascx" %>
<%@ Register TagPrefix="dnn" TagName="LOGIN" Src="~/Admin/Skins/Login.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SEARCH" Src="~/DesktopModules/AgapeFR/Search/Search.ascx" %>
<%@ Register TagPrefix="ddr" Namespace="DotNetNuke.Web.DDRMenu.TemplateEngine" Assembly="DotNetNuke.Web.DDRMenu" %>
<%@ Register TagPrefix="ddr" TagName="MENU" src="~/DesktopModules/DDRMenu/Menu.ascx" %>

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
<script>
function openNav() {
    document.getElementById("mySidenav").style.width = "500px";
}

function closeNav() {
    document.getElementById("mySidenav").style.width = "0";
}
</script>

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
            <div id="menubuttons">
                <img id="searchicon" src="Portals/_default/Skins/AgapeFR/images/searchblue.svg" height="50" />
                <img id="menuicon" src="Portals/_default/Skins/AgapeFR/images/menublue.svg" onclick="openNav()" height="50" />
            </div>
        </div>
    </div>
</div>
<div id="mySidenav" class="sidenav">
    <a href="javascript:void(0)" class="closebtn" onclick="closeNav()">&#9776;</a>
    <%--demo menu--%>
    <ul>
        <li><a href="#">Faire un don</a></li>
        <li><a href="#">Boutique</a></li>
        <li><a href="#">Nous connaitre</a></li>
        <li><a href="#">S'impliquer</a>
            <ul>
                <li><a href="#">Acceuil S'impliquer</a></li>
                <li><a href="#">Partenariats</a></li>
                <li><a href="#">Devenir permanent</a></li>
                <li><a href="#">Faire un stage</a></li>
                <li><a href="#">Nous aider benevolement</a></li>
                <li><a href="#">Faire un don</a></li>
            </ul>
        </li>
        <li><a href="#">Activites</a></li>
        <li><a href="#">Actualites</a></li>
        <li><a href="#">Ressources</a></li>
        <li><a href="#">Intranet</a></li>
        <li><a href="#">May Workman</a>
            <ul>
                <li><a href="#">Deconnexion</a></li>
            </ul>
        </li>
    </ul>
    <%--/demo menu--%>
</div>