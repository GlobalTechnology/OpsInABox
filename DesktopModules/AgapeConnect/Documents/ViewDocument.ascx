<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ViewDocument.ascx.vb" Inherits="DotNetNuke.Modules.AgapeConnect.Documents.ViewDocument" %>

<div id="ViewDocument">

    <asp:Panel ID="pnlVideo" runat="server" Visible="false">
        <iframe src="https://www.youtube.com/embed/<%= YouTubeID%>?rel=0&amp;autohide=1&amp;color=white&amp;showinfo=0&amp;autoplay=1"
            frameborder="0" webkitallowfullscreen="true" mozallowfullscreen="true" allowfullscreen="true"></iframe>
    </asp:Panel>

</div>