<%@ Control Language="vb" AutoEventWireup="false" Explicit="True" %>
<%@ Register TagPrefix="dnn" TagName="LOGO" Src="~/Admin/Skins/Logo.ascx" %>
<%@ Register TagPrefix="dnn" TagName="COPYRIGHT" Src="~/Admin/Skins/Copyright.ascx" %>
<%@ Register TagPrefix="ddr" Namespace="DotNetNuke.Web.DDRMenu.TemplateEngine" Assembly="DotNetNuke.Web.DDRMenu" %>
<%@ Register TagPrefix="ddr" TagName="MENU" src="~/DesktopModules/DDRMenu/Menu.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TEXT" Src="~/Admin/Skins/Text.ascx" %>


<script runat="server">
    Protected Function Translate(ResourceKey As String) As String   
        Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
        Dim strFile As String = System.IO.Path.GetFileName(Server.MapPath(PS.ActiveTab.SkinSrc))
        strFile = PS.ActiveTab.SkinPath + Localization.LocalResourceDirectory + "/" + strFile
        Return Localization.GetString(ResourceKey, strFile)
    End Function
</script>


<div class="bar">
	<div class="globalbox">
		<div id="bar5" class="bar">
		</div>		
		<div id="bar6" class="bar">
			<div id="footer1" class="centeredbox">
                <div id="logoFooter">
					<dnn:LOGO runat="server" ID="dnnLOGOFOOTER" />
				</div>
                <ddr:MENU ID="SITEMAP" MenuStyle="/templates/AgapeFRSitemap/" NodeSelector="*" runat="server" />	
			</div>
		</div>
		<div id="bar7" class="bar">
			<div id="footer2" class="centeredbox">
                <div id="socialmedia"><dnn:TEXT runat="server" ID="txtSocialMedia" ResourceKey="SocialMedia" CssClass=""/></div>
                <a href="http://www.facebook.com/agapefrance" target="_blank" id="facebook" title="Facebook"></a>
                <a href="http://www.youtube.com/user/VideosAgapeFrance" target="_blank" id="youtube" title="YouTube"></a>
			</div>		
		</div>
		<div id="bar8" class="bar">
			<div id="footer3" class="centeredbox">
				<div id="contactAndSlogan">
				    <div id="postalAddress">
                        <dnn:TEXT runat="server" ID="txtPostalAddress" ResourceKey="OzoirAddress" />
				    </div>
                    <div id="telFaxEmail">
                        <div id="tel">
                            <div id="telIcon"></div>
                            <div id="telText"><dnn:TEXT runat="server" ID="txtTel" ResourceKey="OzoirPhone" CssClass="" /></div>
                        </div>
                        <div id="fax">
                            <div id="faxIcon"></div>
                            <div id="faxText"><dnn:TEXT runat="server" ID="txtFax" ResourceKey="OzoirFax" CssClass="" /></div>
                        </div>
                        <div id="email">
                            <div id="emailIcon"></div>                      
                            <div id="emailText"><a href="mailto:<%=Translate("OzoirEmail.Text")%>?subject=<%=Translate("OzoirEmailSubject.Text")%>" target="_blank"><%=Translate("OzoirEmail.Text")%></a></div>
                        </div>
				    </div>
                    <div id="slogan">
                        <dnn:TEXT runat="server" ID="txtSlogan" ResourceKey="Slogan" CssClass="" />
                    </div>
                </div>
                <div id="infosLegales">
					<div id="copyright"><dnn:COPYRIGHT runat="server" ID="dnnCOPYRIGHT" CssClass="" /></div>
                    <div id="mentionslegales"><a href="/mentionslegales" rel="nofollow"><%=Translate("MentionsLegales.Text")%></a></div>
				</div>
			</div>
		</div>
	</div>
</div>