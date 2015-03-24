<%@ Control language="vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Containers.Container" %>
<%@ Register TagPrefix="dnn" TagName="TITLE" Src="~/Admin/Containers/Title.ascx" %>

<div class="BluePromotionBox">
	<div class="titleContainer">
        <div class="titleRays"></div>
		<dnn:TITLE runat="server" id="dnnTITLE" CSSClass="title" />
	</div>
	<div class="contentContainer"> 	
		<div class="contentpane" id="ContentPane" runat="server"></div>
	</div>
</div>










