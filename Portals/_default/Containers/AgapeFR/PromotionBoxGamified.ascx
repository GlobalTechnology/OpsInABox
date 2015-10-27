<%@ Control language="vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Containers.Container" %>
<%@ Register TagPrefix="dnn1" TagName="TITLE" Src="~/Admin/Containers/Title.ascx" %>
<%@ Register TagPrefix="dnn2" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<dnn2:DnnJsInclude ID="PromotionBoxGamifiedJs" runat="server" FilePath="~/Portals/_default/Containers/AgapeFR/js/PromotionBoxGamified.js" />

<div class="PromotionBoxGamified">
	<div class="titleContainer">
        <div class="titleRays"></div>
		<dnn1:TITLE runat="server" id="dnnTITLE" CSSClass="title" />
	</div>
	<div class="contentContainer"> 	
		<div class="contentpane" id="ContentPane" runat="server"></div>
	</div>
</div>










