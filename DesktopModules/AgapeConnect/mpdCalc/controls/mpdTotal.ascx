<%@ Control Language="VB" AutoEventWireup="false" CodeFile="mpdTotal.ascx.vb"
    Inherits="DotNetNuke.Modules.AgapeConnect.mpdTotal" %>


    <asp:Panel ID="pnlGroup" runat="server" class="control-group mpdTotal">
    <asp:Label ID="lblItemId" runat="server" class="version-number"></asp:Label>
    <asp:Label ID="lblItemName" runat="server" class="control-label">Total Salary & Paryoll</asp:Label>
    <div class="controls">
        
        <div class="span2 mpdColumn">
            
                <asp:Label ID="lblCur" runat="server">$</asp:Label><asp:Label ID="lblTotalSalMonthly" runat="server" CssClass="section-total-monthly"></asp:Label>
          
        </div>
        <div class="span2 mpdColumn">
          
                <asp:Label ID="lblCur2" runat="server">$</asp:Label><asp:Label ID="lblTotalSalYearly" runat="server" CssClass="section-total-yearly"></asp:Label>
            
        </div>

        <asp:Label ID="lblHelp" runat="server" class="help-inline mpd-help"></asp:Label>
    </div>
</asp:Panel>
