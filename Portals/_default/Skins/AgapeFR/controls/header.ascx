<%@ Control Language="vb" AutoEventWireup="false" Explicit="True" %>
<%@ Register TagPrefix="dnn" TagName="LOGO" Src="~/Admin/Skins/Logo.ascx" %>
<%@ Register TagPrefix="dnn" TagName="USER" Src="~/Admin/Skins/User.ascx" %>
<%@ Register TagPrefix="dnn" TagName="LOGIN" Src="~/Admin/Skins/Login.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SEARCH" Src="~/DesktopModules/AgapeFR/Search/Search.ascx" %>

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

<div id="controlPanelContainer">
    <div id="ControlPanel" runat="server" />
</div>
<div class="bar">
    <div id="header" class="centeredbox bar1">
        <div id="headerContent" class="bar1">
            <div id="logoHeader">
                <dnn:LOGO runat="server" ID="dnnLOGOHEADER" />
            </div>
            <div id="menu">
                <img src="Portals/_default/Skins/AgapeFR/images/searchblue.svg" alt="Menu!" height="50" />
                <img src="Portals/_default/Skins/AgapeFR/images/menublue.svg" alt="Menu!" height="50" />
            </div>
        </div>
    </div>
</div>
