<%@ Control Language="VB" AutoEventWireup="false" CodeFile="DocumentViewer.ascx.vb" Inherits="DotNetNuke.Modules.AgapeConnect.Documents.DocumentViewer" %>

<div style="text-align: center; width: 100%;" >
 <asp:Label ID="lblError" runat="server" class="ui-state-error ui-corner-all" 
                Style="padding: 3px; margin-top: 3px; display: inline-block; width: 50%;" Visible ="false" ></asp:Label>
                </div>

<asp:Panel ID="theMainPanel" runat="server">


<table>
    <tr valign="top">
        <td width="1100px" align="center">
        <div style="text-align: left; margin-bottom: 5px;">
        <asp:Label ID="lblFileName" runat="server" CssClass="AgapeH2" style="font-size: 27pt"></asp:Label><br />
<asp:Label ID="lblFileUrl" runat="server" style="font-size: x-small; font-style: italic; color: #AAA;"></asp:Label>
</div>
            <iframe id="Viewer" runat="server" width="750px" height="1100px"></iframe>
        </td>
    </tr>
</table>


</asp:Panel>