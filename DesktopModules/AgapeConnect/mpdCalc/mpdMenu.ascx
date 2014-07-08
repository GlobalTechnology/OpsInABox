<%@ Control Language="VB" AutoEventWireup="False" CodeFile="mpdMenu.ascx.vb" Inherits="DotNetNuke.Modules.AgapeConnect.mpdMenu" %>
<%@ Register Src="~/controls/labelcontrol.ascx" TagName="labelcontrol" TagPrefix="uc1" %>
<%@ Register Src="~/DesktopModules/AgapeConnect/mpdCalc/controls/staffTreeview.ascx" TagPrefix="uc1" TagName="staffTreeview" %>
<%@ Register Src="~/DesktopModules/AgapeConnect/mpdCalc/controls/BudgetTile.ascx" TagPrefix="uc1" TagName="BudgetTile" %>
<%@ Register Src="~/DesktopModules/AgapeConnect/mpdCalc/controls/MenuDetail.ascx" TagPrefix="uc1" TagName="MenuDetail" %>
<%@ Register Src="~/DesktopModules/AgapeConnect/mpdCalc/controls/mpdAdmin.ascx" TagPrefix="uc1" TagName="mpdAdmin" %>






<script src="/js/jquery.numeric.js" type="text/javascript"></script>
<link href="/Portals/_default/Skins/AgapeBlue/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
<script src="/Portals/_default/Skins/AgapeBlue/bootstrap/js/bootstrap.min.js"></script>

<script type="text/javascript">


    (function ($, Sys) {



        $(document).ready(function () {
            $('.numeric').numeric();


            $('.aButton').button();
            $("#accordion").accordion({
                header: "> div > h3",
                navigate: false
            });

            $('.sLevel').each(function (c) {
                console.log($(this).attr("data-value"));
                $('.' + $(this).attr("data-value")).text(' (' + $(this).text() + ')');
            });

            <%= IIf(IsEditable, " $('.myTab').hide();", "")%>
           

           
        });

    }(jQuery, window.Sys));








</script>
<style type="text/css">
    .detail-button {
        font-size: large;
        margin-bottom: 5px;
        padding: 10px;
    }
    .AcPane {
        height: 280px;
    }

    .mpd-year {
        margin-top: 10px;
    }

    .mpd-menu-tvtitle {
        font-size: large;
        font-weight: bold;
        font-style: italic;
    }

    .alert-expired {
        color: #999999;
        background-color: #EEEEEE;
        border-color: #DDDDDD;
    }

        .alert-expired h4 {
            color: #999999;
        }

    a .tile:hover {
        border-width: 3px;
    }

    .nav li {
        list-style: none;
    }
    .tile {
        width:83%;
       
    }
        .mpd-help {
        padding-left: 10px;
          font-weight: normal;
color: #777;
    }
</style>



<div class="container-fluid">

    <div class="tabbable tabs-left">
        <ul class="nav nav-tabs">
            <li id="pnlMyBudgets" runat="server" class="active">

                <a id="myTabTitle" href="#myTab<%= StaffId %>"  data-toggle="tab" class="myTab">
                    <asp:Label ID="Label1" runat="server" resourcekey="YourBudgets"></asp:Label>
                </a>
            </li>
            <li id="pnlToProcess" runat="server" class="active">

                <a id="toProcessTitle" href="#toProcessTab"  data-toggle="tab" >
                 <asp:Label ID="Label2" runat="server" resourcekey="ToProcess"></asp:Label>
                </a>
            </li>
            <asp:Repeater ID="rpTeam" runat="server">
                <ItemTemplate>
                    <li>

                        <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl='<%# "#Tab" & Eval("StaffId")%>' data-toggle="tab">

                            <asp:Label ID="lblDisplayName" runat="server" Text='<%# Eval("DisplayName")%>'  ></asp:Label>
                            <asp:Label ID="lblSupportlevel" runat="server"  CssClass='<%# "sl" & Eval("StaffId")%>'></asp:Label>
                        <asp:HiddenField ID="hfStaffId" runat="server" Value='<%# Eval("StaffId")%>' />
                        </asp:HyperLink>
                    </li>
                </ItemTemplate>
            </asp:Repeater>

        </ul>
        <div  class="tab-content">
            <div id="pnlAdminHelp" runat="server" class="alert alert-info">
            <button type="button" class="close" data-dismiss="alert">&times;</button>
            <strong><asp:Label ID="Label4" runat="server" ResourceKey="HeadsUp"></asp:Label></strong>
            <asp:Label ID="lblEditMode" runat="server" ResourceKey="EditMode"></asp:Label>
            <asp:Label ID="lblViewMode" runat="server" ResourceKey="ViewMode"></asp:Label>

        </div>


            <div class="tab-pane active myTab" id="myTab<%= StaffId %>">
                
                         <uc1:MenuDetail runat="server" ID="myMenuDetail"   />
                 
                    </div>

            <div class="tab-pane active" id="toProcessTab">
                    
                          <asp:Label ID="Label3" runat="server" ResourceKey="ToProcessIntro"></asp:Label>
                 <ul class="nav nav-tabs nav-stacked">

                <asp:Repeater ID="rpToProcess" runat="server">
                    <ItemTemplate>
                        <li>
                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# EditUrl("mpdCalc") & "?sb=" & Eval("StaffBudgetId") %>' Text='<%#Eval("DisplayName")%>'></asp:HyperLink>

                        </li>
                    </ItemTemplate>

                </asp:Repeater>
                </ul>

                    </div>
            <asp:Repeater ID="rpMenuDetail" runat="server">
                <ItemTemplate>
                    <asp:Literal ID="Literal1" runat="server" Text='<%# "<div id=""Tab" & Eval("StaffId") & """  class=""tab-pane"">"%>'></asp:Literal>
                   
                        
                
                    <uc1:MenuDetail runat="server" ID="MenuDetail" MpdDefId='<%# mpdDefId%>' PortalId='<%#PortalId %>' EditUrl='<%#EditUrl("mpdCalc") %>'   DisplayName='<%# Eval("DisplayName")%>'   StaffId='<%# Eval("StaffId")%>' ShowCreate="true" />
                    
                    
                    
                    </div>
                    


                </ItemTemplate>
                
            </asp:Repeater>
           
           


        </div>
    </div>







</div>
<div style="clear: both; height: 20px;"></div>


 <uc1:mpdAdmin runat="server" ID="mpdAdminPanel" Visible="false"  />










