<%@ Control Language="vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Skins.Skin" %>
<%@ Register TagPrefix="dnn" TagName="Header" Src="~/Portals/_default/Skins/AgapeFR/controls/Header.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Footer" Src="~/Portals/_default/Skins/AgapeFR/controls/Footer.ascx" %>

<dnn:Header runat="server" ID="dnnHeader" />

<div id="bar3" class="bar accueil">
	<div class="centeredbox">
        <div id="ContentPane" class="ContentPane" runat="server">
		</div>
	</div>
</div>
<div id="bar4" class="bar accueil">
	<div class="centeredbox">
        <div id="BottomPane" class="BottomPane" runat="server">
		</div>
	</div>
</div>

<dnn:Footer runat="server" ID="dnnFooter" />

