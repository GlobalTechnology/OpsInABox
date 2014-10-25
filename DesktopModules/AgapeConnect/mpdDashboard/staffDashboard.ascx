<%@ Control Language="VB" AutoEventWireup="False" CodeFile="staffDashboard.ascx.vb" Inherits="DotNetNuke.Modules.AgapeConnect.staffDashboard" %>
<%@ Register Src="~/controls/labelcontrol.ascx" TagName="labelcontrol" TagPrefix="uc1" %>
<%@ Register Src="~/DesktopModules/AgapeConnect/mpdDashboard/controls/mpdDashboardMenu.ascx" TagPrefix="uc1" TagName="mpdDashboardMenu" %>

<link href="/Portals/_default/Skins/AgapeBlue/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
<script src="/Portals/_default/Skins/AgapeBlue/bootstrap/js/bootstrap.min.js"></script>
<script type="text/javascript" src="https://www.google.com/jsapi"></script>
<script type="text/javascript">
    var sData = [true, true, true, true, true];
    google.load("visualization", "1", { packages: ["corechart"] });
    google.setOnLoadCallback(drawChart);

    function drawChart() {
        var data = google.visualization.arrayToDataTable([
          ['Month', 'Balance', 'Income', 'To Raise', 'Expenses', 'Expense Budget'],
         <%= jsonLi %>
        ]);

        
        var chart = new google.visualization.LineChart(document.getElementById('chart_div'));

        refreshChart();
       // chart.draw(data, options);
        google.visualization.events.addListener(chart, 'select', selectHandler);

        function selectHandler(e) {
            var s = chart.getSelection()[0];
            try {
                if (s.row == undefined) {
                    sData[s.column - 1] = !sData[s.column - 1];

                    refreshChart();
                }
            } catch (e) {

            }

        }
        function refreshChart() {
            var options = {
                title: 'Budget vs Actual',
                series: [{ color: '#ff9900', lineWidth: sData[0] ? 2 : 0 }, { color: '#3366cc', lineWidth: sData[1] ? 2 : 0 }, { color: '#109618', lineWidth: sData[2] ? 2 : 0 }, { color: '#dc3912', lineWidth: sData[3] ? 2 : 0 }, { color: '#808080', lineWidth: sData[4] ? 2 : 0 }]

            }
            chart.draw(data, options);
        }



        var data2 = google.visualization.arrayToDataTable([
         ['Income Type', 'Amount'],
<%= jsonPI %>        
        
        ]);

        var options2 = {
            title: 'Ave Income - by type (over year)',
            is3D: true,
        };

        var chart2 = new google.visualization.PieChart(document.getElementById('piechart_3d'));
        chart2.draw(data2, options2);





    }
    $(function () {
        $('.dropdown-toggle').dropdown();
        //$('#addStaffAccount').appendTo("body");
    });
   
</script>
<style type="text/css">
    [stroke="#109618"], [stroke="#808080"]{
        stroke-dasharray: 10px;
    }
</style>

 <asp:Panel  ID="pnlError" runat="server" CssClass="alert" Visible="false">
      <asp:Label ID="lblError" runat="server" Text="You do not have permissions to view this page"></asp:Label>
    </asp:Panel>

<div ID="pnlMain" runat="server">
    <uc1:mpdDashboardMenu runat="server" ID="mpdDashboardMenu"/>
    <asp:HyperLink ID="hlBack" runat="server"></asp:HyperLink>
<h2> <asp:Label ID="lblStaffName" runat="server" Text=""></asp:Label></h2>
<div id="chart_div" style="width: 100%; height: 500px;"></div>

<div class="span8">


<div id="piechart_3d" style="width: 100%; height: 500px;"></div>
    </div>

<div class="span3">
     <h3> MPD Analysis:</h3>
    <div>
         <asp:Image ID="imgMpdHealth" runat="server"  CssClass="span3"/>
            <asp:Label ID="lblMpdHealth" runat="server" CssClass="span9"></asp:Label>

      </div>
    <div style="clear: both;" />

   <h3> Average Support:</h3>
    
    Over the past:
    <table style="font-size: x-large;" cellpadding="10">
        <tr>
            <td width="120px">Year</td>
            <td>
                <asp:Label ID="lblYear" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td>Quarter</td>
            <td>
                <asp:Label ID="lblQuarter" runat="server" Text=""></asp:Label>
            </td>
        </tr>
         <tr>
            <td>Month</td>
            <td>
                <asp:Label ID="lblMonth" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>
    
    <asp:Label ID="lblEstimates" runat="server" Visible="false" ForeColor="Gray" Font-Italic="True" Text="*Based on estimated budget - as no budget has been submitted."></asp:Label>

</div>
    </div>



