<%@ Control Language="vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Skins.Skin" %>
<%@ Register TagPrefix="dnn" TagName="Header" Src="~/Portals/_default/Skins/AgapeFR/controls/Header.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Footer" Src="~/Portals/_default/Skins/AgapeFR/controls/Footer.ascx" %>

<dnn:Header runat="server" ID="dnnHeader" />
<div id="bar2" class="bar">
	<div id="ContentContainer" class="centeredbox row">
        <div id="TopCarrousel" class="TopPane col-9" runat="server">
		</div>
        <div id="TopBlocks" class="TopPane col-3" runat="server">
		</div>
	</div>
</div>
<div id="bar3" class="bar">
	<div class="centeredbox row">
        <div id="ContentPane" class="ContentPane" runat="server">
		</div>
	</div>
</div>
<div id="bar4" class="bar">
	<div class="centeredbox row">
        <div id="BottomPane" class="BottomPane" runat="server">
		</div>
	</div>
</div>
<dnn:Footer runat="server" ID="dnnFooter" />