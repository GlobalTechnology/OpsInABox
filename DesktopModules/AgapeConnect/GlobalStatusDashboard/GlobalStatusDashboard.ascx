<%@ Control Language="VB" AutoEventWireup="False" CodeFile="GlobalStatusDashboard.ascx.vb" Inherits="DotNetNuke.Modules.AgapeConnect.GlobalStatusDashboard" %>
<script type="text/javascript" src="https://www.google.com/jsapi"></script>
<script>

    google.load("visualization", "1", {packages:["corechart"]});
    google.setOnLoadCallback(drawChart);
    function drawChart() {
       

        var data = google.visualization.arrayToDataTable([
          ['Area', '% of Countries with Current Donations', '% of Countries with Current Ministry Measurements', '% of Countries with Current Financial Reports', {role:"annotationText"}],
          <%= json_area.TrimEnd(",") %>
         
        ]);

        var options = {
            titlePosition: "none",
            colors: ['#1dbdea', '#f6b900', '#64c827'],
            chartArea:
            {
                top: 10,
                height: "90%",
                left: "22%",
                width: "63%"
               
                
            },
            vAxis: {
                textStyle:
                    { bold: true }
            },
            backgroundColor: 'transparent'
       
        };

        var chart = new google.visualization.BarChart(document.getElementById('chart_div'));



        var selectHandler = function (e) {
            
           // alert(e.targetID);
            if (e.targetID.indexOf('vAxis') >= 0 || e.targetID.indexOf('bar') >= 0)
            {
                
               // alert(e.targetID.substring(e.targetID.lastIndexOf("#") + 1));
                var area = data.getValue(parseInt(e.targetID.substring(e.targetID.lastIndexOf("#") + 1)), 4);
                $('html, body').animate({
                          scrollTop: $("#" + area).offset().top -170
                       }, 2000);
            }
            
            //var selectedItem = chart.getSelection()[0];



            //if (selectedItem) {
            //    var area = data.getValue(selectedItem.row, 4);

            //    $('html, body').animate({
            //        scrollTop: $("#" + area).offset().top -170
            //    }, 2000);

            //}
        };

        google.visualization.events.addListener(chart, 'click', selectHandler);

        chart.draw(data, options);






    }
     
   
    $(function () {
        $('.stage0').hide();
            $('.systems-table img').tooltip();
            $('#hide0').click(function () {
                $('.stage0').toggle();
            });
      
            $('.sticky-header').css('width', $('#main-table').width());

            $(window).scroll(function(){
                if($(window).scrollTop() >= 750)
                    $('.sticky-header').addClass('fixed');
                else
                    $('.sticky-header').removeClass('fixed');
            })

            $("text[text-anchor='end'").click(function(){
                alert('hello');
            })
        });
     
      

    
</script>
<style type="text/css">
    .systems-table th {
        text-align: center;
        font-size: medium;
    }

    .systems-table td {
        vertical-align: middle;
        text-align: center;
        font-size: medium;
    }

    .National.stage0, .National_Region.stage0, .Area .gma {
        opacity: 0.4;
        filter: alpha(opacity=40); /* For IE8 and earlier */
    }

    .fcx-label {
        float: left;
        font-size: small;
        color: lightgrey;
    }

    .fixed {
        position: fixed;
        top: 50px;
    }

    .sticky-header {
        background-color: #1dbdea;
        margin-right: 80px;
    }

    .col1 {
        width: 201px;
    }

    .col2 {
        width: 138px;
    }

    .col3 {
        width: 138px;
    }

    .col4 {
        width: 138px;
    }
</style>
<asp:Label ID="lblDeny" runat="server" Visible="false" Text="You do not have permissions to view this page." CssClass="alert alert-error"></asp:Label>





<asp:Panel ID="pnlMain" runat="server">
    <div class="well well-large">
   <div style="text-align: center; "> <h2 style="margin-bottom: 3px;">&quot;Current&quot; Countries by Area </h2>
    <em">(Click on a row to see detail)</em></div>
    <div id="chart_div" style="width: 100%; height: 500px;"></div>
</div>
    <table class="table table-striped systems-table sticky-header">
        <tr>
            <th class="col1" style="text-align: left;">
                <a id="hide0" href="#" class="btn">Show/Hide Stage 0</a>

            </th>
            <%--Country--%>
            <th class="col1"></th>
            <th class="col2">Donations<br />
                (tntDataserver)</th>
            <th class="col3">Ministry Results<br />
                (GMA)</th>
            <th class="col4">Financial Reports</th>
        </tr>
    </table>
    <table id="main-table" class="table table-striped systems-table">
        <asp:Repeater ID="rpAreas" runat="server">
            <ItemTemplate>
                <tr  id='<%# Eval("area_code") %>'>
                    <th colspan="2" style="text-align: left;">
                        <br />
                        <asp:Label ID="lblAreaName" Font-Size="x-Large" runat="server" Text='<%# Eval("area_name") & ":"%>'></asp:Label></th>
                    <th></th>
                    <th></th>
                    <th></th>
                </tr>

                <asp:Repeater ID="Repeater1" runat="server" DataSource='<%# getCountriesForArea(Eval("area_code"),Eval("area_name")) %>'>
                    <ItemTemplate>
                        <tr class='<%# "stage" & Eval("stage") & " " & Eval("min_scope") %>'>

                            <td style="text-align: right">
                                <asp:Label ID="Label1" runat="server" CssClass="fcx-label col1" Text="FCX" Visible='<%# Eval("fcx")%>'></asp:Label><asp:Label ID="lblAreaName" runat="server" Text='<%# Eval("min_name")%>'></asp:Label></td>
                            <td>
                                <asp:Image ID="imgLogo" runat="server" Style="max-width: 150px; max-height: 40px;" ImageUrl='<%# Eval("min_logo")%>' /></td>
                            <td class="col2">
                                <asp:Image ID="Image2" CssClass="don" runat="server" Height="30px" data-toggle="tooltip" title='<%# Eval("donationMessage")%>' ImageUrl='<%# Eval("donationImg")%>' />
                            </td>
                            <td class="col3">
                                <asp:Image ID="Image1" CssClass="gma" runat="server" Height="30px" data-toggle="tooltip" title='<%# Eval("gmaMessage")%>' ImageUrl='<%# Eval("gmaImage")%>' /></td>
                            <td class="col4">
                                <asp:Image ID="Image3" class="finrep" runat="server" Height="30px" data-toggle="tooltip" title='<%# Eval("finRepMessage")%>' ImageUrl='<%# Eval("finRepImage")%>' /></td>

                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </ItemTemplate>

        </asp:Repeater>

    </table>
</asp:Panel>
