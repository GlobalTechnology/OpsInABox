<%@ Control Language="VB" AutoEventWireup="False" CodeFile="GMA.ascx.vb" Inherits="DotNetNuke.Modules.GMA.GMA" %>
<%@ Register Src="~/controls/labelcontrol.ascx" TagName="labelcontrol" TagPrefix="uc1" %>
<link href="/Portals/_default/Skins/AgapeBlue/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
<script src="/Portals/_default/Skins/AgapeBlue/bootstrap/js/bootstrap.min.js"></script>
<script src="http://code.jquery.com/jquery-1.9.1.js"></script>
<script src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>
<link rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
<script type="text/javascript" src="https://www.google.com/jsapi"></script>
<script src="/js/jquery.mousewheel.js"></script>
<script src="/js/jquery.numeric.js"></script>
<script type="text/javascript">
    google.load("visualization", "1", { packages: ["corechart"] });
    google.setOnLoadCallback(function () { drawVisualization() });
    (function ($, Sys) {

        function setUpMyTabs() {
            $('.aButton').button();

            //$('.dropdown-toggle').dropdown();
            $('.spinner').spinner({ min: 0 });
            $('.numeric').numeric();
            var selectedTabIndex = $('#<%= theHiddenTabIndex.ClientID  %>').attr('value');
            //alert(selectedTabIndex);
            $('#tabs').tabs({

                activate: function () {

                    var newIdx = $('#tabs').tabs('option', 'active');
                    $('#<%= theHiddenTabIndex.ClientID  %>').val(newIdx);
                },
                active: selectedTabIndex
            });


        }

        $(document).ready(function () {
            setUpMyTabs();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                setUpMyTabs();
            });
        });
    }(jQuery, window.Sys));


    function drawVisualization() {

        // create and populate the data table.
        var options = {
            width: "100",
            height: "60",
            chartArea: { width: '100%', height: '100%' },
            legend: { position: 'none' },
            lineWidth: 4,
            titlePosition: 'none', axisTitlesPosition: 'none',
            hAxis: { textPosition: 'none', baselineColor: 'white', gridlines: { count: 0 } }, vAxis: { textPosition: 'none', baselineColor: 'white', gridlines: { count: 0 } }
        };

       
        <%= GetReportString()%>
      




      }

</script>
<style type="text/css">
    ul.nav li {
        list-style-type: none;
    }

    .spinner.ui-spinner-input {
        border: none;
        -moz-box-shadow: none;
        -webkit-box-shadow: none;
        box-shadow: none;
        -webkit-transition: none;
        -moz-transition: none;
        -o-transition: none;
        transition: none;
        margin: 0;
    }

    .pagination ul > li > a[disabled="disabled"] {
        background-color: rgb(221, 221, 221);
    }

    .brand {
        display: block;
        float: left;
        padding: 10px 20px 10px;
        margin-left: -20px;
        font-size: 30px;
        font-weight: 200;
        color: #777;
        text-shadow: 0 1px 0 #fff;
    }
    .rotate {
  -webkit-transform: rotate(-90deg);
  -moz-transform: rotate(-90deg);
  -ms-transform: rotate(-90deg);
  -o-transform: rotate(-90deg);
  transform: rotate(-90deg);
  text-align: center;
  vertical-align: middle;

  /* also accepts left, right, top, bottom coordinates; not required, but a good idea for styling */
  -webkit-transform-origin: 50% 50%;
  -moz-transform-origin: 50% 50%;
  -ms-transform-origin: 50% 50%;
  -o-transform-origin: 50% 50%;
  transform-origin: 50% 50%;

  /* Should be unset in IE9+ I think. */
  filter: progid:DXImageTransform.Microsoft.BasicImage(rotation=3);
}
    .lmiGrid td.rotate {
        padding: 0;
    }

    .lmiGrid td {
        padding:8px;
        font-size:small;
    }
    .lmiGrid tr {
        vertical-align: middle;
    }
    .lmiGrid .control-label {
        width:140px
    }
    .lmiGrid .controls {
margin-left: 160px;
}
    .graph {
        float: left;
        margin-right: 5px;
    }
    .gMin, .gMax {
       width: 50%;
    }
    
    .gLabel {
        
        font-weight: bold;

    }
    
</style>

<asp:HiddenField ID="theHiddenTabIndex" runat="server" Value="0" ViewStateMode="Enabled" />

<asp:HiddenField ID="hfNodeId" runat="server" Value="-1" />
<asp:HiddenField ID="hfURL" runat="server" Value="" />
<div class="container-fluid">
    <div class="row-fluid">
        <div class="span2">
            <!--Sidebar content-->





            <asp:Repeater ID="rpGmaServers" runat="server">

                <HeaderTemplate>
                    <ul class="nav nav-list">
                </HeaderTemplate>
                <ItemTemplate>

                    <li class="nav-header">
                        <asp:Label ID="Label2" runat="server" Text='<%# Eval("name")  %>'></asp:Label>

                    </li>
                    <asp:Repeater ID="rpNodes" runat="server" DataSource='<%# Eval("nodes")  %>' OnItemCommand="rpNodes_ItemCommand">

                        <ItemTemplate>
                            <li <%# IIf(DataBinder.Eval(Container.DataItem, "nodeId") = hfNodeId.value, "class='active'", "")%>>
                                <asp:LinkButton ID="LinkButton1" runat="server" Text='<%# Eval("shortName")  %>' CommandName="nodeSelected" CommandArgument='<%# Eval("url") & ":::" & Eval("nodeId")  %>'></asp:LinkButton>


                            </li>

                        </ItemTemplate>

                    </asp:Repeater>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
            </asp:Repeater>


        </div>

        <div class="span10">
            <!--Body content-->

            <asp:Panel ID="pnlMain" runat="server">
                <div class="container" style="width: auto;">
                    <div class="brand">

                        <asp:Label ID="lblNodeTitle" runat="server"></asp:Label>
                    </div>
                </div>


                <div id="tabs">
                    <!-- Only required for left/right tabs -->
                    <ul>
                        <li>
                            <asp:HyperLink ID="tabStaff" runat="server" href="#tab1" Visible="false">Staff Report</asp:HyperLink></li>

                        <li>
                            <asp:HyperLink ID="tabDirector" runat="server" href="#tab2" Visible="false">Director Report</asp:HyperLink>
                        </li>
                        <li>
                            <asp:HyperLink ID="Analysis" runat="server" href="#tab3" Visible="true">Analysis</asp:HyperLink>
                        </li>
                    </ul>




                    <div class="tab-content">
                        <div class="tab-pane" id="tab1">
                            <div style="width: 100%; text-align: center;">
                                <div class="pagination" style="margin-top: 0;">

                                    <ul>
                                        <li>
                                            <asp:LinkButton ID="lbPrevPeriod" runat="server">&laquo;</asp:LinkButton>
                                        </li>

                                        <li>
                                            <a style="padding: 0;">
                                                <asp:DropDownList ID="ddlPeriods" runat="server" DataTextField="LabelName" DataValueField="ReportId" AutoPostBack="true" Style="margin: 0;"></asp:DropDownList>
                                            </a>

                                        </li>
                                        <li>
                                            <asp:LinkButton ID="lbNextPeriod" runat="server">&raquo;</asp:LinkButton></li>
                                    </ul>



                                </div>
                            </div>
                            <asp:Panel ID="pnlLmiGrid" runat="server">

                            <table class="well lmiGrid form-horizontal" border="1">
                                <tr>
                                    <td></td>
                                    <td><h3>Faith Actions</h3></td>
                                    <td><h3>Fruit</h3></td>
                                    <td><h3>Outcome</h3></td>
                                </tr>
                                <tr>
                                    <td rowspan="3" class="rotate">
                                        <h4>Win</h4>
                                    </td>
                                    <td><div runat="server" id="dvMass"><span class="control-label">Mass Exposures:</span><div class="controls"> <asp:TextBox ID="tbMass" runat="server" CssClass='spinner numeric'  Width="60px" ></asp:TextBox></div>
                                        <asp:HiddenField ID="hfMass" runat="server" />
                                    </div></td>
                                    <td rowspan="3">
                                        <div runat="server" id="dvNewBel"><span class="control-label">New Believers:</span><div class="controls"> <asp:TextBox ID="tbNewBel" runat="server" CssClass='spinner numeric'  Width="60px" ></asp:TextBox></div><asp:HiddenField ID="hfNewBel" runat="server" /></div>
                                    </td>
                                    <td rowspan="8">
                                        <div runat="server" id="dvmovement"><span class="control-label" style="width: 90px;">Movement:</span><div class="controls" style="margin-left:110px;"> <asp:TextBox ID="tbmovement" runat="server" CssClass='spinner numeric'  Width="60px" ></asp:TextBox></div><asp:HiddenField ID="hfmovement" runat="server" /></div>
                                    </td>
                                </tr>
                                <tr><td><div runat="server" id="dvExposures"><span class="control-label">Personal Exposures:</span><div class="controls"> <asp:TextBox ID="tbExposures" runat="server" CssClass='spinner numeric'  Width="60px" ></asp:TextBox></div><asp:HiddenField ID="hfExposures" runat="server" /></div></td></tr>
                                <tr><td><div runat="server" id="dvPresGosp"><span class="control-label">Presenting the Gospel:</span><div class="controls"> <asp:TextBox ID="tbPresGosp" runat="server" CssClass='spinner numeric'  Width="60px" ></asp:TextBox></div><asp:HiddenField ID="hfPresGosp" runat="server" /></div></td></tr>
                                <tr> 
                                    <td rowspan="2" class="rotate">
                                        <h4>Build</h4>
                                    </td>
                                     <td><div runat="server" id="dvFollowup"><span class="control-label">Following Up:</span><div class="controls"> <asp:TextBox ID="tbFollowup" runat="server" CssClass='spinner numeric'  Width="60px" ></asp:TextBox></div><asp:HiddenField ID="hfFollowup" runat="server" /></div></td>
                                     <td rowspan="2">
                                       <div runat="server" id="dvEngagedDisc"><span class="control-label">Engaged Disciples:</span><div class="controls"> <asp:TextBox ID="tbEngagedDisc" runat="server" CssClass='spinner numeric'  Width="60px" ></asp:TextBox></div><asp:HiddenField ID="hfEngagedDisc" runat="server" /></div>
                                    </td>
                                   
                                </tr>
                                <tr><td><div runat="server" id="dvHSPres"><span class="control-label">Holy Spirit Presentations:</span><div class="controls"> <asp:TextBox ID="tbHSPres" runat="server" CssClass='spinner numeric'  Width="60px" ></asp:TextBox></div><asp:HiddenField ID="hfHSPres" runat="server" /></div></td></tr>
                                <tr> 
                                    <td rowspan="3" class="rotate">
                                        <h4>Send</h4>
                                    </td>
                                     <td>
                                       <div runat="server" id="dvTraining"><span class="control-label">Training For action:</span><div class="controls"> <asp:TextBox ID="tbTraining" runat="server" CssClass='spinner numeric'  Width="60px" ></asp:TextBox></div><asp:HiddenField ID="hfTraining" runat="server" /></div>
                                    </td>
                                    <td rowspan="2"><div runat="server" id="dvMultDisc"><span class="control-label">Multiplying Disciples:</span><div class="controls"> <asp:TextBox ID="tbMultDisc" runat="server" CssClass='spinner numeric'  Width="60px" ></asp:TextBox></div><asp:HiddenField ID="hfMultDisc" runat="server" /></div></td>
                                </tr>
                                <tr><td><div runat="server" id="dvSendLifeLab"><span class="control-label">Sending Lifetime Labourors:</span><div class="controls"> <asp:TextBox ID="tbSendLifeLab" runat="server" CssClass='spinner numeric'  Width="60px" ></asp:TextBox></div><asp:HiddenField ID="hfSendLifeLab" runat="server" /></div></td></tr>
                                <tr><td><div runat="server" id="dvDevLocRes"><span class="control-label">Developed Local Resources:</span><div class="controls"> <asp:TextBox ID="tbDevLocRes" runat="server" CssClass='spinner numeric'  Width="60px" ></asp:TextBox></div><asp:HiddenField ID="hfDevLocRes" runat="server" /></div></td>
                                    <td><div runat="server" id="dvLocGenRes"><span class="control-label">Locally Generated Resourses:</span><div class="controls"> <asp:TextBox ID="tbLocGenRes" runat="server" CssClass='spinner numeric'  Width="60px" ></asp:TextBox></div><asp:HiddenField ID="hfLocGenRes" runat="server" /></div></td>
                                </tr>
                            </table>
                            </asp:Panel>

                                 <div class="row-fluid">

                        <asp:DataList ID="rpStaffMeasurements" runat="server" Width="100%" CssClass="form-horizontal">
                            <ItemTemplate>
                                <div class="control-group">
                                    <div class="span4">

                                        <asp:Label ID="lblQuestion" runat="server" Font-Bold="True" Text='<%# Eval("measurementName")%>'></asp:Label>
                                    </div>
                                    <div class="span8">
                                        <asp:HiddenField ID="hfAnswerType" runat="server" Value='<%# Eval("measurementType") %>' />
                                        <asp:HiddenField ID="hfMeasurementId" runat="server" Value='<%# Eval("measurementId")%>' />
                                        <asp:TextBox ID="tbAnswer" runat="server" Text='<%# Eval("measurementValue")%>' CssClass='<%#IIf(Eval("measurementType") = "numeric", "spinner numeric", "")%>' Enabled='<%# Eval("measurementType") = "numeric"%>' Visible='<%# {"numeric", "calculated"}.Contains(Eval("measurementType"))%>' Width="50px"></asp:TextBox>
                                        <asp:TextBox ID="tbAnswerText" runat="server" Text='<%# Eval("measurementValue")%>' Visible='<%# Eval("measurementType") = "text"%>' Width="160px" TextMode="MultiLine"></asp:TextBox>

                                    </div>
                                </div>


                            </ItemTemplate>
                            <FooterTemplate>
                                <div class="control-group">
                                    <div class="span4"></div>
                                    <div class="span8">
                                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn" CommandName="SaveReport" />
                                    </div>
                                </div>
                            </FooterTemplate>
                        </asp:DataList>


                    </div>






                </div>


        <div class="tab-pane" id="tab2">

            <div style="width: 100%; text-align: center;">
                <div class="pagination" style="margin-top: 0;">

                    <ul>
                        <li>
                            <asp:LinkButton ID="lbPrevPeriodD" runat="server">&laquo;</asp:LinkButton>
                        </li>

                        <li>
                            <a style="padding: 0;">
                                <asp:DropDownList ID="ddlPeriodsD" runat="server" DataTextField="LabelName" DataValueField="ReportId" AutoPostBack="true" Style="margin: 0;"></asp:DropDownList>
                            </a>

                        </li>
                        <li>
                            <asp:LinkButton ID="lbNextPeriodD" runat="server">&raquo;</asp:LinkButton></li>
                    </ul>



                </div>
            </div>
            <div class="row-fluid">

                <asp:DataList ID="rpDirectorMeasuremts" runat="server" Width="100%" CssClass="form-horizontal">
                    <ItemTemplate>
                        <div class="control-group">
                            <div class="span4">

                                <asp:Label ID="lblQuestion" runat="server" Font-Bold="True" Text='<%# Eval("measurementName")%>'></asp:Label>
                            </div>
                            <div class="span8">
                                <asp:HiddenField ID="hfAnswerType" runat="server" Value='<%# Eval("measurementType") %>' />
                                <asp:HiddenField ID="hfMeasurementId" runat="server" Value='<%# Eval("measurementId")%>' />
                                <asp:TextBox ID="tbAnswer" runat="server" Text='<%# Eval("measurementValue")%>' CssClass='<%#IIf(Eval("measurementType") = "numeric", "spinner numeric", "")%>' Enabled='<%# Eval("measurementType") = "numeric"%>' Visible='<%# {"numeric", "calculated"}.Contains(Eval("measurementType"))%>' Width="50px"></asp:TextBox>
                                <asp:TextBox ID="tbAnswerText" runat="server" Text='<%# Eval("measurementValue")%>' Visible='<%# Eval("measurementType") = "text"%>' Width="160px" TextMode="MultiLine"></asp:TextBox>

                            </div>
                        </div>


                    </ItemTemplate>
                    <FooterTemplate>
                        <div class="control-group">
                            <div class="span4"></div>
                            <div class="span8">
                                <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn" CommandName="SaveReport" />
                            </div>
                        </div>
                    </FooterTemplate>
                </asp:DataList>





            </div>
        </div>


                          <div class="tab-pane" id="tab3">
                           
                            <asp:Panel ID="Panel1" runat="server">

                            <table class="well lmiGrid " border="1" width="100%">
                                <tr>
                                    <td></td>
                                    <td><h3>Faith Actions</h3></td>
                                    <td><h3>Fruit</h3></td>
                                    <td><h3>Outcome</h3></td>
                                </tr>
                                <tr>
                                    <td rowspan="3" class="rotate">
                                        <h4>Win</h4>
                                    </td>
                                    <td>
                                        <div id="gMass" class="graph" ></div>
                                        <div class="gLabel">Mass Exposures</div>
                                        <span id="hMass" class="gMax"></span>
                                         <span id="lMass" class="gMin"></span>
                                       
                                         </td>
                                    <td rowspan="3">
                                        <div id="gNewBel" class="graph" ></div>
                                        <div class="gLabel">New Believers</div>
                                        <span id="hNewBel" class="gMax"></span>
                                        <span id="lNewBel" class="gMin"></span>
                                     
                                      
                                    </td>
                                    <td rowspan="8">
                                        <div id="gMovement" class="graph" ></div>
                                        <div class="gLabel">Movements</div>
                                        <span id="hMovement" class="gMax"></span>
                                        <span id="lMovement" class="gMin"></span>
                                    </td>
                                </tr>
                                <tr><td><div id="gExposures" class="graph" ></div>
                                        <div class="gLabel">Personal Exposures</div>
                                        <span id="hExposures" class="gMax"></span>
                                        <span id="lExposures" class="gMin"></span>
                                    </td></tr>
                                <tr><td>
                                    <div id="gPresGosp" class="graph" ></div>
                                        <div class="gLabel">Presenting the Gospel</div>
                                        <span id="hPresGosp" class="gMax"></span>
                                        <span id="lPresGosp" class="gMin"></span>
                                    </td></tr>
                                <tr> 
                                    <td rowspan="2" class="rotate">
                                        <h4>Build</h4>
                                    </td>
                                     <td>
                                         <div id="gFollowup" class="graph" ></div>
                                        <div class="gLabel">Following Up</div>
                                         <span id="hFollowup" class="gMax"></span>
                                        <span id="lFollowup" class="gMin"></span>
                                     </td>
                                     <td rowspan="2">
                                      <div id="gEngagedDisc" class="graph" ></div>
                                        <div class="gLabel">Engaged Disciples</div>
                                         <span id="hEngagedDisc" class="gMax"></span>
                                        <span id="lEngagedDisc" class="gMin"></span>
                                    </td>
                                   
                                </tr>
                                <tr><td>
                                    <div id="gHSPres" class="graph" ></div>
                                        <div class="gLabel">Holy Spirit Presentations</div>
                                        <span id="hHSPres" class="gMax"></span>
                                        <span id="lHSPres" class="gMin"></span>
                                    </td></tr>
                                <tr> 
                                    <td rowspan="3" class="rotate">
                                        <h4>Send</h4>
                                    </td>
                                     <td>
                                      <div id="gTraining" class="graph" ></div>
                                        <div class="gLabel">Training for Action</div>
                                         <span id="hTraining" class="gMax"></span>
                                        <span id="lTraining" class="gMin"></span>
                                    </td>
                                    <td rowspan="2"><div id="gMultDisc" class="graph" ></div>
                                        <div class="gLabel">Multiplying Disciples</div>
                                        <span id="hMultDisc" class="gMax"></span>
                                        <span id="lMultDisc" class="gMin"></span>
                                    </td>
                                </tr>
                                <tr><td>
                                    <div id="gSendLifeLab" class="graph" ></div>
                                        <div class="gLabel">Sending Lifetime Laborers</div>
                                    <span id="hSendLifeLab" class="gMax"></span>
                                        <span id="lSendLifeLab" class="gMin"></span>

                                    </td></tr>
                                <tr><td>
                                     <div id="gDevLocRes" class="graph" ></div>
                                        <div class="gLabel">Developed Local Resources</div>
                                    <span id="hDevLocRes" class="gMax"></span>
                                        <span id="lDevLocRes" class="gMin"></span>

                                    </td>
                                    <td>
                                         <div id="gLocGenRes" class="graph" ></div>
                                        <div class="gLabel">Locally Generated Resources</div>
                                        <span id="hLocGenRes" class="gMax"></span>
                                        <span id="lLocGenRes" class="gMin"></span>

                                    </td>
                                </tr>
                            </table>
                            </asp:Panel>


                    </div>






                </div>

    </div>












</asp:Panel>
    </div>
</div>
<asp:Label ID="Label1" runat="server" Text="Test"></asp:Label>