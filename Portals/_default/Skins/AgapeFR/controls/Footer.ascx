<%@ Control Language="VB" AutoEventWireup="false" Explicit="True" CodeFile="Footer.ascx.vb" Inherits="Portals__default_Skins_AgapeFR_controls_Footer" %>
<%@ Register TagPrefix="dnn" TagName="COPYRIGHT" Src="~/Admin/Skins/Copyright.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TEXT" Src="~/Admin/Skins/Text.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<dnn:DnnJsInclude runat="server" FilePath="js/popup-fr.js" PathNameAlias="SkinPath" />

<div id="bar7" class="bar">
    <div id="footer2" class="centeredbox">
        <div id="socialmedia">
            <dnn:TEXT runat="server" ID="txtSocialMedia" ResourceKey="SocialMedia" CssClass="" />
        </div>
        <a href="/nousconnaitre/newsletter" id="newsletter" title="Newsletter" onclick="ga('send', 'event', 'buttons', 'click', 'footer-newsletter');"></a>
        <a href="http://www.facebook.com/agapefrance" target="_blank" id="facebook" title="Facebook"></a>
        <a href="http://www.youtube.com/user/VideosAgapeFrance" target="_blank" id="youtube" title="YouTube"></a>
        <a href="/nousconnaitre/nouscontacter" id="contact" title="Nous Contacter"></a>
    </div>
</div>
<div id="bar8" class="bar">
    <div id="footer3" class="centeredbox">
        <div id="contactAndSlogan">
            <div id="slogan">
                <dnn:TEXT runat="server" ID="txtSlogan" ResourceKey="Slogan" CssClass="" />
            </div>
        </div>
        <div id="infosLegales">
            <div id="copyright">
                <dnn:COPYRIGHT runat="server" ID="dnnCOPYRIGHT" CssClass="" />
            </div>
            <div id="mentionslegales"><a href="/mentionslegales" rel="nofollow"><%=Translate("MentionsLegales.Text")%></a></div>
<div id="privacypolicy"><a href="/mentionslegales/protectiondesdonneespersonnelles" rel="nofollow"><%=Translate("PrivacyPolicy.Text")%></a></div>
        </div>
    </div>
</div>
<div id="fr_popup" runat="server">
    <div id="my_popup">
        <div>
            <div class="my_popup_close">
                <a id="popupclose">
                    <svg width="16" version="1.1" xmlns="http://www.w3.org/2000/svg" height="16" viewBox="0 0 64 64" xmlns:xlink="http://www.w3.org/1999/xlink" enable-background="new 0 0 64 64">
                        <g>
                            <path class="cancelbutton" fill="#0e71b4" d="M28.941,31.786L0.613,60.114c-0.787,0.787-0.787,2.062,0,2.849c0.393,0.394,0.909,0.59,1.424,0.59   c0.516,0,1.031-0.196,1.424-0.59l28.541-28.541l28.541,28.541c0.394,0.394,0.909,0.59,1.424,0.59c0.515,0,1.031-0.196,1.424-0.59   c0.787-0.787,0.787-2.062,0-2.849L35.064,31.786L63.41,3.438c0.787-0.787,0.787-2.062,0-2.849c-0.787-0.786-2.062-0.786-2.848,0   L32.003,29.15L3.441,0.59c-0.787-0.786-2.061-0.786-2.848,0c-0.787,0.787-0.787,2.062,0,2.849L28.941,31.786z" />
                        </g>
                    </svg>
                </a>
            </div>
            <h3><%=Translate("PopupHeader.Text")%></h3>
            <p><%=Translate("PopupParagraph.Text")%></p>
        </div>
        <div>
            <a id="frsignup" class="button" href="/nousconnaitre/newsletter"><%=Translate("PopupButton.Text")%></a>
        </div>
    </div>
</div>
<asp:Label ID="myURL" runat="server" />
<script type="text/javascript">_satellite.pageBottom();</script>