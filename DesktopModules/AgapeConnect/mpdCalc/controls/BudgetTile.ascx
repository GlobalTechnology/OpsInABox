<%@ Control Language="VB" AutoEventWireup="false" CodeFile="BudgetTile.ascx.vb" Inherits="DesktopModules_AgapeConnect_mpdCalc_controls_BudgetTile" %>

<asp:HyperLink ID="hlTile" runat="server" Target="_self">
<asp:Panel ID="pnlTile" runat="server" class="alert tile">
   
<h3 style="margin-top:0;">
    <asp:Label ID="lblStart" runat="server" CssClass="span6"></asp:Label>
      <asp:Label ID="lblStatus" runat="server"  style="float:right" ></asp:Label>
</h3>
 <div class="clearfix"></div>
   <%-- <table style="font-weight: bold; font-size: small;">
        <tr valign="middle" >
            <td width="60px" >From:</td>
            <td style="font-size: medium;">
                

            </td>
        </tr>
        
    </table>--%>
    <asp:Label ID="Label1" runat="server" ResourceKey="Goal"></asp:Label> <asp:Label ID="lblGoal" runat="server" > </asp:Label><br />
    <asp:Label ID="lblNote" runat="server" > </asp:Label>

</asp:Panel>
</asp:HyperLink>


        