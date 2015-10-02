<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ResultsOnlineForm.ascx.vb" Inherits="DotNetNuke.Modules.OnlineForm.ResultsOnlineForm" %>
<asp:HiddenField ID="ModuleHF" runat="server" />
<asp:HiddenField ID="FormIdHF" runat="server" />
<table border="1">

<asp:PlaceHolder ID="TablePH" runat="server"></asp:PlaceHolder>
</table>

<asp:LinkButton ID="ReturnButton" runat="server">Return</asp:LinkButton>