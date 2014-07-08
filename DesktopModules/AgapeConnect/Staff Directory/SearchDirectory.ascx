<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SearchDirectory.ascx.vb" Inherits="DotNetNuke.Modules.SearchDirectory.SearchDirectory" %>


<table width="100%">
    <tr valign="middle">
        <td width="100%"><asp:TextBox ID="SearchBox" runat="server" Width ="130px" Font-Size="8pt" ></asp:TextBox></td>
        <td><asp:ImageButton ID="Button1" runat="server" ImageUrl="~/Portals/_default/Skins/AgapeBlue/images/search.gif" ToolTip="Search"  /></td>
    </tr>
</table>


 

<%--<ccsearch:TextBoxWatermarkExtender runat="server" ID="TBWE1" TargetControlID="SearchBox" 
    WatermarkText="Search Staff Directory" WatermarkCssClass="Agape_Watermarked_TextBox">
</ccsearch:TextBoxWatermarkExtender>--%>