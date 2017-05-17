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
        </div>
    </div>
</div>
<div id="fr_popup" runat="server">
    <div id="my_popup">
        <div>
            <div class="my_popup_close">
                <a id="popupclose">&#x2716</a>
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