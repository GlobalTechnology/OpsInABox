<%@ Control Language="VB" AutoEventWireup="False" CodeFile="mpdDashboard.ascx.vb" Inherits="DotNetNuke.Modules.AgapeConnect.mpdDashboard" %>
<%@ Register Src="~/controls/labelcontrol.ascx" TagName="labelcontrol" TagPrefix="uc1" %>
<link href="/Portals/_default/Skins/AgapeBlue/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
<script src="/Portals/_default/Skins/AgapeBlue/bootstrap/js/bootstrap.min.js"></script>
<script type="text/javascript" src="https://www.google.com/jsapi"></script>
    <script type="text/javascript">
        google.load("visualization", "1", { packages: ["corechart"] });
        google.load('visualization', '1', { 'packages': ['geochart'] });
       
        google.setOnLoadCallback(drawChart);

     

        function drawChart() {


       


            var data3 = google.visualization.arrayToDataTable([
         ['Country', 'MPD Level'],
        <%= jsonMap %>
            ]);

            var options3 = {enableScrollWheel: true, colors: ['#FF0000', '#ff9b00', '#00FF00'], colorAxis: { minValue:50, maxValue: 110 } };

            var chart3 = new google.visualization.GeoChart(document.getElementById('mapchart'));

            google.visualization.events.addListener(chart3, "regionClick", function (eventData) {
                var countryISO2 = eventData["region"];
                window.location.href = '<%= EditUrl("countryDashboard") & "?country=" %>' + countryISO2;


            });

            chart3.draw(data3, options3);
            

        }

        $(function () {
            $('.countryRow').click(function (c) {
                var iso = $(this).find("input").val();
               
                window.location.href = '<%= EditUrl("countryDashboard") & "?country=" %>' + iso;
            });

        });
    </script>
<i>Select a country on the map, or from the list below the map, to drill-down to a more detailed country report:</i>

<div id="mapchart" style="width: 100%; height: 500px;">
    
</div>

<table width="100%" class="table table-striped table-hover">
    <thead>
    <tr valign="middle">
        <th rowspan="2">Country</th>
        <th rowspan="2">% of Goal Raised</th>
        <th rowspan="2">Staff with a budget (%)</th>
        <th colspan="4" align="center">% of Staff who have raised:</th>
        
        <th rowspan="2">% of Expense Budget Spent</th>
        <th rowspan="2">Support raised locally (%)</th>
        </tr>
    
   <tr>
       <th><50%</th>
       <th>50-80%</th>
       <th>80-100%</th>
       <th>>100%</th>
       
   </tr>
        </thead>
<asp:Repeater ID="rpCountriesSummaryData" runat="server">
    <ItemTemplate>
        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# EditUrl("countryDashboard") & "?country=" & Eval("ISO")%>'>
    
            <tr class="countryRow">
        <th><asp:HiddenField ID="hfISO" runat="server" Value='<%# Eval("ISO")%>' /><asp:Label ID="lblCountryName" runat="server" Text='<%# Eval("Name") %>' ></asp:Label></th>
                <td><asp:Label ID="lblSupYear" runat="server"  Text='<%# Eval("Year") %>' ></asp:Label></td>
                <td><asp:Label ID="lblBud" runat="server"  Text='<%# Eval("Budget") %>' ></asp:Label></td>
        
        <td><asp:Label ID="lblSupQuart" runat="server"  Text='<%# Eval("less50")%>' ></asp:Label></td>
                <td><asp:Label ID="Label1" runat="server"  Text='<%# Eval("from50to80")%>' ></asp:Label></td>
                 <td><asp:Label ID="Label2" runat="server"  Text='<%# Eval("from80to100")%>' ></asp:Label></td>
                 <td><asp:Label ID="Label3" runat="server"  Text='<%# Eval("more100")%>' ></asp:Label></td>
        <td><asp:Label ID="lblSupMonth" runat="server"  Text='<%# Eval("BudgetSpent")%>' ></asp:Label></td>
        
        <td><asp:Label ID="lblAccuracy" runat="server"  Text='<%# Eval("Local") %>' ></asp:Label></td>
                
    </tr>
            </asp:HyperLink>
        </ItemTemplate>
 </asp:Repeater>   </table>


