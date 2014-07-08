<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Financial.ascx.vb" Inherits="DotNetNuke.Modules.tntSuperSite.Financial" %>
<script type="text/javascript" src="http://www.google.com/jsapi"></script>
<script type="text/javascript">
    google.load("visualization", "1", {packages:["corechart"]});
    google.setOnLoadCallback(drawVisualization);
    function drawVisualization() {
        // Create and populate the data table.
        var data = new google.visualization.DataTable();
        data.addColumn('string', 'Period');
        data.addColumn('number', 'Income');
        data.addColumn('number', 'Expense');
        data.addColumn('number', 'Balance');

        <%=GetGoogleData() %>
 
        // Create and draw the visualization.
        var chart = new google.visualization.LineChart(document.getElementById("IncExpGraph"));
        chart.draw(data, {width: 1050, height: 300, title: 'Income/Expense Graph'});
    }
</script>

<div align="center">
<table width="900" cellpadding="0" cellspacing="0" border="0">
    <tr>
        <td align="center"><asp:LinkButton ID="btnHome" runat="server" class="Agape_Red_H3">Home</asp:LinkButton></td>
        <%--<td align="center"><asp:LinkButton ID="btnDonations" runat="server" class="Agape_Red_H3">Donations</asp:LinkButton></td>
        <td align="center"><asp:LinkButton ID="btnDonors" runat="server" class="Agape_Red_H3">Donors</asp:LinkButton></td>
        --%><td align="center"><asp:Label ID="lblFinancial" runat="server" class="Agape_Red_H3">Accounts</asp:Label></td>
    </tr>
    <tr><td colspan="4"><hr /></td></tr>
    <tr><td colspan="4"><div id="IncExpGraph"></div><asp:Label ID="lbltest" runat="server" Visible="true"></asp:Label></td></tr>
    <tr>
        <td colspan="4"> 
            <table width="900" cellpadding="0" cellspacing="0">
                <tr valign="top">
                  <td align="left">
                        <table width="600">
                            <tr>
                                <td align="center" colspan="3"><asp:Label ID="lblDate" runat="server" class="Agape_Red_H4"><%=GetDateRange() %></asp:Label></td>
                            </tr>
                        </table><br />
                      <asp:Label ID="lblCostCenter" runat="server" class="Agape_Red_H4" ></asp:Label>
                 </td>
                    <td align="right">
                      <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="right" style="font-weight: bold ; font-size: 8pt;  " >Country:&nbsp;&nbsp;</td>
                        <td> <asp:DropDownList ID="MyCountries" runat="server" AutoPostBack="true" Font-Bold="true" Width="200px" Font-Size="8pt"></asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td align="right" style="font-weight: bold ; font-size: 8pt; " >Profile:&nbsp;&nbsp;</td>
                        <td> <asp:DropDownList ID="MyProfiles" runat="server" AutoPostBack="true" Width="200px" Font-Size="8pt"></asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td align="right" style="font-weight: bold ; font-size: 8pt; white-space: nowrap;" >Cost Centre:&nbsp;&nbsp;</td>
                        <td> <asp:DropDownList ID="MyAccounts" runat="server" AutoPostBack="true" Width="200px" Font-Size="8pt"></asp:DropDownList></td>
                    </tr>
                </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr><td colspan="4"><hr /></td></tr>
    <tr>
        <td align="right" style="font-size: small">Starting Balance: <asp:Label ID="StartingBalance" runat="server" /></td>
        <td></td>
        <td></td>
        <td align="left" style="font-size: small">Ending Balance: <asp:Label ID="EndingBalance" runat="server" /></td>
    </tr>
</table>
</div>

<%--<div align="center">
    <asp:Panel id="IEPanel" style="POSITION: relative" runat="server" width="900px" bordercolor="Tan">
        <span class="Agape_Red_H4"></span><br />
        <asp:GridView ID="IEBReport" runat="server"
            AutoGenerateColumns="True" ShowFooter="True" EnableModelValidation="True" 
            Width="950px" BackColor="LightGoldenrodYellow" BorderColor="Tan" 
            BorderWidth="1px" CellPadding="2" ForeColor="Black" GridLines="None" HorizontalAlign="Right" >
            <AlternatingRowStyle BackColor="PaleGoldenrod" />
            <RowStyle Font-Size="Small" />
            <AlternatingRowStyle BackColor="PaleGoldenrod" Font-Size="Small" />
            
            <FooterStyle BackColor="Tan" Font-Size="Small" />
            <HeaderStyle BackColor="Tan" Font-Size="Medium" Font-Bold="True" />
            <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" 
                HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
        </asp:GridView>
    </asp:Panel>
</div>--%>

<div align="center">
    <asp:Panel id="TrxPanel" style="POSITION: relative" runat="server" width="900px" bordercolor="Tan">
        <span class="Agape_Red_H4">Income</span><br />
        <asp:GridView ID="FinancialIncomeSummary" runat="server"
            AutoGenerateColumns="False" ShowFooter="True" EnableModelValidation="True" 
            Width="900px" BackColor="LightGoldenrodYellow" BorderColor="Tan" 
            BorderWidth="1px" CellPadding="2" ForeColor="Black" GridLines="None">
            <AlternatingRowStyle BackColor="PaleGoldenrod" />
            <RowStyle Font-Size="Small" />
            <AlternatingRowStyle BackColor="PaleGoldenrod" Font-Size="Small" />
            <Columns>
                <asp:BoundField DataField="glAccount" HeaderText="GL Account">
                    <FooterStyle horizontalalign="Left"></FooterStyle>
			        <HeaderStyle horizontalalign="Left"></HeaderStyle>
			        <ItemStyle horizontalalign="Left"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="glSubTotal" HeaderStyle-HorizontalAlign="Right" HeaderText="Amount">
                    <FooterStyle horizontalalign="Right"></FooterStyle>
			        <HeaderStyle horizontalalign="Right"></HeaderStyle>
			        <ItemStyle horizontalalign="Right"></ItemStyle>
                </asp:BoundField>
            </Columns>
             <FooterStyle BackColor="Tan" Font-Size="Small" />
            <HeaderStyle BackColor="Tan" Font-Size="Medium" Font-Bold="True" />
            <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" 
                HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
        </asp:GridView>

        <br /><br />

        <span class="Agape_Red_H4">Expenses</span><br />
        <asp:GridView ID="FinancialExpenseSummary" runat="server"
            AutoGenerateColumns="False" ShowFooter="True" EnableModelValidation="True" 
            Width="900px" BackColor="LightGoldenrodYellow" BorderColor="Tan" 
            BorderWidth="1px" CellPadding="2" ForeColor="Black" GridLines="None">
            <AlternatingRowStyle BackColor="PaleGoldenrod" />
            <RowStyle Font-Size="Small" />
            <AlternatingRowStyle BackColor="PaleGoldenrod" Font-Size="Small" />
            <Columns>
                <asp:BoundField DataField="glAccount" HeaderText="GL Account">
                    <FooterStyle horizontalalign="Left"></FooterStyle>
			        <HeaderStyle horizontalalign="Left"></HeaderStyle>
			        <ItemStyle horizontalalign="Left"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="glSubTotal" HeaderStyle-HorizontalAlign="Right" HeaderText="Amount">
                    <FooterStyle horizontalalign="Right"></FooterStyle>
			        <HeaderStyle horizontalalign="Right"></HeaderStyle>
			        <ItemStyle horizontalalign="Right"></ItemStyle>
                </asp:BoundField>
            </Columns>
             <FooterStyle BackColor="Tan" Font-Size="Small" />
            <HeaderStyle BackColor="Tan" Font-Size="Medium" Font-Bold="True" />
            <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" 
                HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
        </asp:GridView>
    </asp:Panel>
</div>