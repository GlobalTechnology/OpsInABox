<%@ Control Language="C#" AutoEventWireup="true" Codefile="Account.ascx.cs" Inherits="DotNetNuke.Modules.Account.AccountReport"  %>
<link href="/Portals/_default/Skins/AgapeBlue/flick/jquery-ui-1.8.18.custom.css"
    rel="stylesheet" type="text/css" />
<script src="/Portals/_default/Skins/AgapeBlue/bootstrap/js/bootstrap.min.js"></script>
<script type="text/javascript" src="https://www.google.com/jsapi"></script>
<script src="/Scripts/linq.js" type="text/javascript"></script>
<link href="/Portals/_default/Skins/AgapeBlue/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
<script lang="javascript" type="text/javascript">
    google.load("visualization", "1", { packages: ["corechart"] });
    google.setOnLoadCallback(function () { drawVisualization() });
    $(function () {
        //$("#accordion").accordion({
        //    autoHeight: false,
        //    navigation: true,
        //    collapsible: true,
        //    active: false,
        //    change: function (event, ui) {
        //        var newIndex = $(ui.newHeader).index('h3');
        //        if (newIndex == 2) {
        //            var oldIndex = $(ui.oldHeader).index('h3');
        //            $(this).accordion("activate", oldIndex);
        //        }
        //    }
        //});
        $("#divTransDetail").dialog({
            autoOpen: false,
            height: 300,
            width: 500,
            modal: true,
            title: '<%= Translate("lblTransactionDetail") %>',
            close: function () {
                //  allFields.val("").removeClass("ui-state-error");
            }
        });
        $("#divTransDetail").parent().appendTo($("form:first"));


        $("#divAddCountry").dialog({
            autoOpen: false,
            height: 500,
            width: 320,
            modal: true,
            title: '<%= Translate("lblAddCountry") %>',
            close: function () {
                //  allFields.val("").removeClass("ui-state-error");
            }
         });
        $("#divAddCountry").parent().appendTo($("form:first"));

        $("#<%= openAddCountry.ClientID %>").tooltip();




        if ($("#<%= MyCountries.ClientID %>").val().indexOf('ADD') == 0) {
            $(".heading").hide();


        }
        else {
            $(".heading").unbind('click');
            $(".content").hide();
            $(".heading").click(function () {
                $(this).next(".content").slideToggle(500, function () {
                    if ($(this).is(':hidden')) {
                        $("#<%= lblChange.ClientID  %>").text("<%= Translate("lblShowDonations") %>");
                } else {
                    $("#<%= lblChange.ClientID %>").text("<%= Translate("lblHideDonations") %>");
                }

                return false;
            });


        });

        }

      
    });

            function drawVisualization() {

                // create and populate the data table.
                var data = new google.visualization.DataTable();
                data.addColumn('string', 'period');
                data.addColumn('number', '<%= Translate("lblIncome") %>');
                data.addColumn('number', '<%= Translate("lblAverageIncome") %>');
                data.addColumn({ type: 'string', role: 'annotation' });
                data.addColumn({ type: 'boolean', role: 'certainty' });
                data.addColumn('number', '<%= Translate("lblExpenses") %>');
                data.addColumn('number', '<%= Translate("lblBalance") %>');

                <%= getGoogleData() %>

         // create and draw the visualization.
         var chart = new google.visualization.LineChart(document.getElementById("IncExpGraph"));
         //  chart.draw(data,  {chartArea:{left:70,top:10,width:805,height:360}, legend: { position: 'in' }, pointSize: 5, vAxis:{gridLines: {color: '#333',format:'#,###'}}, hAxis:{font: 'Arial Bold'} ,   colors:['#3366cc','#3366cc','#dc3912','#dc3912','#ff9900','#ff9900'] });
         chart.draw(data, { chartArea: { left: 70, top: 10, width: 860, height: 360 }, legend: { position: 'in' }, pointSize: 5, vAxis: { gridLines: { color: '#333', format: '#,###' } }, hAxis: { font: 'Arial Bold' }, series: [{ color: '#3366cc' }, { color: '#b2c2e0', visibleInLegend: false, lineWidth: 2, pointSize: 0 }, { color: '#dc3912' }, { color: '#ff9900' }] });


     }

     function displayDetail(accountCode, period, title) {

         var data = jQuery.parseJSON(unescape(document.getElementById('<%= hfTransactions.ClientID %>').value));

         var output = "<h5 style=\"margin: 0;\">" + title + "</h5><br /><table cellpadding=\"4px\" style=\"margin: 0;\"><tr><td><b><%= Translate("lblDate") %></b></td><td><b><%= Translate("lblDesc") %></b></td><td><b><%= Translate("lblAmount") %></b></td></tr>";

                Enumerable.From(data).Where(function (x) { return x.AC == accountCode && x.Pe == period }).ForEach(function (i) {
                    var method = "";
                    if (i.Me != "")
                        method = " <span class=\"PVKEY\">" + i.Me + "</span>";
                    output += "<tr><td>" + i.Dt + "</td><td>" + i.De + method + "</td><td>" + i.Am + "</td></tr>";
                });

                output += "</table>";

                $('#DetailTable').html(output);
                $("#divTransDetail").dialog("open");
            }
</script>


<style type="text/css">
    .CellHover {
        cursor: pointer;
    }
    .hide-plus{
        background-image: none !important;
    }
    .PVKEY {
        font-size: xx-small;
        color: #999;
        font-style: italic;
    }

    .heading {
        margin: 1px;
        padding: 3px 10px;
        cursor: pointer;
        position: relative;
        background-color: #E2CB9A;
        border-bottom-style: dashed;
        border-width: 1px;
        text-align: center;
        font-style: italic;
    }

    .content {
        padding: 5px 0;
        background-color: #fafafa;
    }
    .accordion-heading {
        font-size: medium;
        margin: 0;
    }
    .ui-accordion-header-icon{
        display:none;
    }
</style>
<div>
    <div id="leftddl" style="float:left; width:180px;">
        <div>
            <asp:Label ID="Label1" runat="server" Font-Bold="true" ResourceKey="lblCountry" Text="Country:"></asp:Label><br />
            <asp:DropDownList ID="MyCountries" runat="server" AutoPostBack="true" Font-Bold="true" Style="margin-bottom: 10px;"
                Width="100%" Font-Size="8pt" OnSelectedIndexChanged="MyCountries_SelectedIndexChanged">
            </asp:DropDownList><br />
            <span class="label label-success">New</span>
            <asp:HyperLink ID="openAddCountry" runat="server" data-placement="right" ToolTip="Do you have donations from a country not listed here (like USA)? Add this country here..." onclick=" $('#divAddCountry').dialog('open');" resourcekey="btnAddCountry" >Add Country...</asp:HyperLink>
        </div>
        <div>
            <asp:Label ID="Label2" runat="server" Font-Bold="true" ResourceKey="lblProfile" Text="Profile:"></asp:Label><br />
            <asp:DropDownList ID="MyProfiles" runat="server" AutoPostBack="true" Width="100%" Style="margin-bottom: 10px;"
                Font-Size="8pt" OnSelectedIndexChanged="MyProfiles_SelectedIndexChanged">
            </asp:DropDownList>
        </div>
        <div>
            <asp:Label ID="Label3" runat="server" Font-Bold="true" ResourceKey="lblRC" Text="Responsibility Center:"></asp:Label><br />
            <asp:DropDownList ID="MyAccounts" runat="server" AutoPostBack="true" Width="100%" Style="margin-bottom: 10px;"
                Font-Size="8pt" OnSelectedIndexChanged="MyAccounts_SelectedIndexChanged">
            </asp:DropDownList>
        </div>
        <div ID="pnlError" runat="server" class="alert  alert-error alert-block"  Visible="false" >
            <asp:Label ID="lblError" runat="server" Font-Size="smaller" ></asp:Label>
        </div>
        <div ID="lblDonationOnly" runat="server" class="alert alert-block"  Visible="false" >
            <asp:Label ID="lbl1" runat="server" Font-Size="smaller" ResourceKey="lblDonationOnly"></asp:Label>
        </div>
    </div>
    <div id="rightgraph" style="float:right">
        <asp:Label ID="lblMessage" runat="server" Text="" ForColor="#777" Visible="false" Font-Italic="true"></asp:Label>
        <div id="IncExpGraph" style="width:930px; height: 400px;"></div>
    </div>
    <div style="clear:both"></div>
</div>

<div id="accordion" class="accordion">
    <div class="accordion-group">
    <h3 class="accordion-heading">
      
         <a class="accordion-toggle color1-bg-h hide-plus" data-toggle="collapse" data-parent="#accordion" href="#income-detail">
        <asp:GridView ID="gvIncome" runat="server"
            ShowHeader="False" GridLines="None" CellPadding="0" Width="100%">
            <RowStyle BorderStyle="None" HorizontalAlign="Right" />

        </asp:GridView>
    </a>
            </h3></div>
  
    <div id="income-detail" class="accordion-body collapse" style="margin: 0px 0px 0px 0px; padding: 5px 6px 5px 29px">
        <div class="accordion-inner color1-link">
         <asp:GridView ID="gvIncomeGLSummary" runat="server" ShowHeader="False"
            GridLines="None" RowStyle-BorderStyle="None" CellPadding="0" Width="100%"
            OnRowDataBound="gvIncomeGLSummary_RowDataBound">
            <AlternatingRowStyle BackColor="White" />

            <RowStyle BackColor="#EFF3FB" BorderStyle="None" HorizontalAlign="Right" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" />


        </asp:GridView>
        <div class="heading" style="">
            <asp:Label ID="lblChange" runat="server" ResourceKey="lblShowDonations" Text="(Click here to show Donations...)"></asp:Label>

        </div>
        <div id="Donations" class="content">
            <asp:GridView ID="gvDonationSummary" runat="server" ShowHeader="False" BorderStyle="Solid" BorderColor="#999" BorderWidth="1px"
                GridLines="None" RowStyle-BorderStyle="None" CellPadding="0" Width="100%"
                OnRowDataBound="gvDonationSummary_RowDataBound">
                <AlternatingRowStyle BackColor="White" />

                <RowStyle BackColor="#fff8c8" BorderStyle="None" HorizontalAlign="Right" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" />


            </asp:GridView>

        </div>
            </div>

    </div>
     <div class="accordion-group">
    <h3 class="accordion-heading">
        
             <a   class="accordion-toggle color3-bg-h  hide-plus" data-toggle="collapse" data-parent="#accordion" href="#expense-detail">
        <asp:GridView ID="gvExpenses" AutoGenerateColumns="true" runat="server" ShowHeader="False" GridLines="None" CellPadding="0" Width="100%">
            <RowStyle BorderStyle="None" HorizontalAlign="Right" />
        </asp:GridView>
    </a>
            </h3> </div>
    <div id="expense-detail" class="accordion-body collapse" style="margin: 0px 0px 0px 0px; padding: 5px 6px 5px 29px">
        <div class="accordion-inner color3-link">
         <asp:GridView ID="gvExpensesGLSummary" runat="server" ShowHeader="False"
            GridLines="None" CellPadding="0" Width="100%"
            OnRowDataBound="gvExpensesGLSummary_RowDataBound">
            <AlternatingRowStyle BackColor="White" />
            <RowStyle BackColor="#EFF3FB" HorizontalAlign="Right" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" />

        </asp:GridView>
</div>
    </div>
     <div class="accordion-group">
    <h3 class="accordion-heading"><a  class="accordion-toggle color2-bg-h  hide-plus" >
        <asp:GridView ID="gvBalance" AutoGenerateColumns="true" runat="server" ShowHeader="False" GridLines="None" CellPadding="0" Width="100%">
            <RowStyle BorderStyle="None" HorizontalAlign="Right" />
        </asp:GridView>
    </a></h3></div>
    
</div>
<div style="color: Gray; font-size: smaller;">
       <asp:Label ID="Label4" runat="server" Text="Starting Balance" ResourceKey="lblStartBal" />
    <asp:Label ID="StartingBalance" runat="server" />
    &nbsp; &nbsp;
    <asp:Label ID="Label5" runat="server" Text="Ending Balance" ResourceKey="lblEndBal" />
    <asp:Label ID="EndingBalance" runat="server" />
</div>


<asp:HiddenField ID="hfTransactions" runat="server" />
<div id="divTransDetail">
    <div id="DetailTable">
    </div>
</div>



<div id="divAddCountry" tabindex="-1">
    
    <div >
        <div class="control-group">
            <asp:Label ID="Label6" runat="server" class="control-label">Country</asp:Label>
            <div class="controls">
                <asp:DropDownList ID="ddlAddCountries" runat="server" DataSourceID="dsAddCountries" DataTextField="CountryName" DataValueField="CountryId"></asp:DropDownList>

                <asp:LinqDataSource ID="dsAddCountries" runat="server" ContextTypeName="MinistryView.MinistryViewDataContext" EntityTypeName="" OrderBy="CountryName" TableName="MinistryView_AdditionalCountries">
                </asp:LinqDataSource>

            </div>
        </div>
        <div class="control-group">
            <asp:Label ID="lblUsername" runat="server" class="control-label" ResourceKey="lblEmail">Email</asp:Label>
            <div class="controls">
                <asp:TextBox ID="tbUsername" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="control-group">
            <asp:Label ID="lblPasswords" runat="server" class="control-label"  ResourceKey="Password">Password</asp:Label>
            <div class="controls">
                <asp:TextBox ID="tbPassword" runat="server" TextMode="Password"></asp:TextBox>
            </div>
        </div>

        <div class="control-group">
            <div class="controls">
                <asp:Button ID="btnAdd" runat="server" CssClass="btn" ResourceKey="btnAdd" OnClick="btnLogin_Click" />
            </div>
        </div>


        <div class="alert alert-info alert-block">
            <asp:Label ID="lblAddCountryFooter" runat="server" font-size="Smaller" ResourceKey="lblAddCountryFooter">Email</asp:Label>
        </div>
    </div>
   






</div>
