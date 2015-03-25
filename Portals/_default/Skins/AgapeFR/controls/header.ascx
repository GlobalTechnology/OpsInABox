<%@ Control Language="vb" AutoEventWireup="false" Explicit="True"  %>
<%@ Register TagPrefix="dnn" TagName="LOGO" Src="~/Admin/Skins/Logo.ascx" %>
<%@ Register TagPrefix="dnn" TagName="USER" Src="~/Admin/Skins/User.ascx" %>
<%@ Register TagPrefix="dnn" TagName="LOGIN" Src="~/Admin/Skins/Login.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SEARCH" Src="~/DesktopModules/AgapeFR/Search/Search.ascx" %>
<%@ Register TagPrefix="ddr" Namespace="DotNetNuke.Web.DDRMenu.TemplateEngine" Assembly="DotNetNuke.Web.DDRMenu" %>
<%@ Register TagPrefix="ddr" TagName="MENU" src="~/DesktopModules/DDRMenu/Menu.ascx" %>

<!--[if lte IE 7]><script src="js/ie7/warning.js"></script><script>window.onload=function(){e("js/ie7/")}</script><![endif]-->

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
	<div class="globalbox">
		<div id="bar1" class="bar bar1">
			<div id="header" class="centeredbox bar1">
				<div id="headerContent" class="bar1">
					<div id="logoHeader">
						<dnn:LOGO runat="server" ID="dnnLOGOHEADER" />
					</div>	
					<div id="toplinks">
						<%'<div id="RegisterContainer" class="needMargin"><dnn:USER runat="server" ID="dnnUSER" CssClass="user" /></div>%>
						<div id="LoginContainer" class="needMargin"><dnn:LOGIN runat="server" ID="dnnLOGIN" CssClass="user" /></div>
						<%'<div id="MinicartContainer" class="needMargin"><dnn:MINICART runat="server" ID="dnnMINICART" /></div>%>
						<div id="SearchContainer" class="needMargin"><dnn:SEARCH runat="server" ID="dnnSEARCH" UseDropDownList="False" ShowWeb="False" ShowSite="False" Submit="<div id=&quot;SearchSubmit&quot;></div>" /></div>
					</div>
					<div id="faireundon">
						<a href="/simpliquer/faireundon" title="<%=Translate("GiveLinkTitle.Text")%>"></a>
					</div>
					<div id="menu1">
						<ddr:MENU ID="MENU1" MenuStyle="/templates/AgapeFRMenu/" NodeSelector="*" runat="server" ExcludeNodes="" includehidden="true" />
					</div>
				</div>
			</div>
		</div>
		<div id="bar2" class="bar">
			<div id="menu2" class="centeredbox">
				<ddr:MENU ID="MENU2" MenuStyle="/templates/AgapeFRMenu/" NodeSelector="RootChildren" runat="server" />            
			</div>
		</div>
	</div>
</div>
<div id="boutique_fixed_tag">
    <a href="http://www.agapemedia.fr" target="_blank" title="<%=Translate("ShopLinkTitle.Text")%>"></a>
</div>
