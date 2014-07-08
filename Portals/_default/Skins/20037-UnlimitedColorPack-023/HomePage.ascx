<%@ Control language="vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Skins.Skin" %>
<%@ Register TagPrefix="dnn" TagName="LOGO" Src="~/Admin/Skins/Logo.ascx" %>
<%@ Register TagPrefix="dnn" TagName="login" Src="~/Admin/Skins/login.ascx" %>
<%@ Register TagPrefix="dnn" TagName="USER" Src="~/Admin/Skins/User.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SEARCH" Src="~/Admin/Skins/Search.ascx" %>
<%@ Register TagPrefix="dnn" TagName="BREADCRUMB" Src="~/Admin/Skins/BreadCrumb.ascx" %>
<%@ Register TagPrefix="dnn" TagName="COPYRIGHT" Src="~/Admin/Skins/Copyright.ascx" %>
<%@ Register TagPrefix="dnn" TagName="PRIVACY" Src="~/Admin/Skins/Privacy.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TERMS" Src="~/Admin/Skins/Terms.ascx" %>
<%@ Register TagPrefix="dnn" TagName="STYLES" Src="~/Admin/Skins/Styles.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Meta" Src="~/Admin/Skins/Meta.ascx" %>
<%@ Register TagPrefix="dnn" TagName="LANGUAGE" Src="~/Admin/Skins/Language.ascx" %>
<%@ Register TagPrefix="dnn" TagName="GOMENU" Src="~/DesktopModules/dnngo_gomenu/ViewMenu.ascx" %>
<%@ Register TagPrefix="dnn" TagName="GOMENU2" Src="~/DesktopModules/dnngo_gomenu/ViewMenu.ascx" %>
<%@ Register TagPrefix="dnn" TagName="STYLESWICTH" Src="~/DesktopModules/DNNGo_StyleSwicth/View_Index.ascx" %>
<dnn:STYLESWICTH runat="server" id="STYLESWICTH1" />
<dnn:Meta runat="server" Name="viewport" Content="width=device-width, minimum-scale=1.0, maximum-scale=2.0" />

<!--[if lt IE 9]>

<script src="https://html5shim.googlecode.com/svn/trunk/html5.js"></script>

<![endif]-->

<div id="dnn_wrapper">
  <div class="dnn_main dnn_layout">
    <header class="clearfix">
      <div class="dnn_logo">
        <dnn:LOGO runat="server" id="dnnLOGO" BorderWidth="0" />
      </div>
      <div class="headr">
        <div id="login_style" class="clearfix">
          <dnn:LOGIN runat="server" id="dnnLOGIN" CssClass="login" />
          <dnn:USER runat="server" id="dnnUSER" CssClass="user" />
        </div>
        <div class="social">
          <div class="SocialPane" id="SocialPane" runat="server"></div>
        </div>
      </div>
    </header>
    <div id="roll_nav">
      <div class="dnn_layout">
        <nav class="hidden-phone clearfix">
          <div id="top_menu">
            <dnn:GOMENU runat="server" id="dnnGOMENU" Effect="Hside" ViewLevel="0"/>
          </div>
          <div class="search_icon">
            <div class="search_bg" id="search">
              <dnn:SEARCH runat="server" id="dnnSEARCH" CssClass="search" Submit="Search"   ShowSite="False" ShowWeb="False" />
            </div>
            <a href="javascript:animatedcollapse.toggle('search')"><img src="<%= SkinPath %>images/search_icon.png"></a> </div>
        </nav>
      </div>
    </div>
    <div class="dnn_layout">
      <div class="mobile_nav  visible-phone clearfix" style="display:none;">
        <div class="mobile_icon">
          <div class="menu_icon"><a href="javascript:animatedcollapse.toggle('mobile_menu')"><img src="<%= SkinPath %>images/menu_icon.png" alt="" title="Menu" /></a></div>
        </div>
        <div id="search2" class="search_bg2">
          <dnn:SEARCH runat="server" id="dnnSEARCH2" CssClass="search"   ShowSite="False" ShowWeb="False" />
        </div>
      </div>
      <div id="mobile_menu" style="display:none;">
        <dnn:GOMENU runat="server" id="dnnGOMENU2" Effect="MultiMenu" ViewLevel="0" />
      </div>
    </div>
    <div id="dnn_banner">
      <div class="BannerPane" id="BannerPane" runat="server"></div>
    </div>
    <section id="dnn_content">
      <section class="row-fluid toppane">
        <div class="TopPane_A" id="TopPane_A" runat="server"></div>
        <div class="TopPane_B" id="TopPane_B" runat="server"></div>
        <div class="TopPane_C" id="TopPane_C" runat="server"></div>
        <div class="TopPane_D" id="TopPane_D" runat="server"></div>
      </section>
      <div class="pane_layout">
        <section class="row-fluid">
          <div class="span12">
            <div class="TopPane" id="TopPane" runat="server"></div>
          </div>
        </section>
        <section class="row-fluid">
          <div class="span3">
            <div class="RowOne_Grid3_Pane" id="RowOne_Grid3_Pane" runat="server"></div>
          </div>
          <div class="span9">
            <div class="RowOne_Grid9_Pane" id="RowOne_Grid9_Pane" runat="server"></div>
          </div>
        </section>
        <section class="row-fluid">
          <div class="span4">
            <div class="RowTwo_Grid4_Pane" id="RowTwo_Grid4_Pane" runat="server"></div>
          </div>
          <div class="span8">
            <div class="RowTwo_Grid8_Pane" id="RowTwo_Grid8_Pane" runat="server"></div>
          </div>
        </section>
        <section class="row-fluid">
          <div class="span5">
            <div class="RowThree_Grid5_Pane" id="RowThree_Grid5_Pane" runat="server"></div>
          </div>
          <div class="span7">
            <div class="RowThree_Grid7_Pane" id="RowThree_Grid7_Pane" runat="server"></div>
          </div>
        </section>
        <section class="row-fluid">
          <div class="span6">
            <div class="RowFour_Grid6_Pane1" id="RowFour_Grid6_Pane1" runat="server"></div>
          </div>
          <div class="span6">
            <div class="RowFour_Grid6_Pane2" id="RowFour_Grid6_Pane2" runat="server"></div>
          </div>
        </section>
        <section class="row-fluid">
          <div class="span7">
            <div class="RowFive_Grid7_Pane" id="RowFive_Grid7_Pane" runat="server"></div>
          </div>
          <div class="span5">
            <div class="RowFive_Grid5_Pane" id="RowFive_Grid5_Pane" runat="server"></div>
          </div>
        </section>
        <section class="row-fluid">
          <div class="span8">
            <div class="RowSix_Grid8_Pane" id="RowSix_Grid8_Pane" runat="server"></div>
          </div>
          <div class="span4">
            <div class="RowSix_Grid4_Pane" id="RowSix_Grid4_Pane" runat="server"></div>
          </div>
        </section>
        <section class="row-fluid">
          <div class="span9">
            <div class="RowSeven_Grid9_Pane" id="RowSeven_Grid9_Pane" runat="server"></div>
          </div>
          <div class="span3">
            <div class="RowSeven_Grid3_Pane" id="RowSeven_Grid3_Pane" runat="server"></div>
          </div>
        </section>
        <section class="row-fluid">
          <div class="span4">
            <div class="RowEight_Grid4_Pane1" id="RowEight_Grid4_Pane1" runat="server"></div>
          </div>
          <div class="span4">
            <div class="RowEight_Grid4_Pane2" id="RowEight_Grid4_Pane2" runat="server"></div>
          </div>
          <div class="span4">
            <div class="RowEight_Grid4_Pane3" id="RowEight_Grid4_Pane3" runat="server"></div>
          </div>
        </section>
        <section class="row-fluid">
          <div class="span3">
            <div class="RownNine_Grid3_Pane1" id="RowNine_Grid3_Pane1" runat="server"></div>
          </div>
          <div class="span3">
            <div class="RowNine_Grid3_Pane2" id="RowNine_Grid3_Pane2" runat="server"></div>
          </div>
          <div class="span3">
            <div class="RowNine_Grid3_Pane3" id="RowNine_Grid3_Pane3" runat="server"></div>
          </div>
          <div class="span3">
            <div class="RowNine_Grid3_Pane4" id="RowNine_Grid3_Pane4" runat="server"></div>
          </div>
        </section>
        <section class="row-fluid">
          <div class="span12">
            <div class="ContentPane" id="ContentPane" runat="server"></div>
          </div>
        </section>
        <section class="row-fluid">
          <div class="span4">
            <div class="RowTen_Grid4_Pane1" id="RowTen_Grid4_Pane1" runat="server"></div>
          </div>
          <div class="span4">
            <div class="RowTen_Grid4_Pane2" id="RowTen_Grid4_Pane2" runat="server"></div>
          </div>
          <div class="span4">
            <div class="RowTen_Grid4_Pane3" id="RowTen_Grid4_Pane3" runat="server"></div>
          </div>
        </section>
        <section class="row-fluid">
          <div class="span9">
            <div class="RowEleven_Grid9_Pane" id="RowEleven_Grid9_Pane" runat="server"></div>
          </div>
          <div class="span3">
            <div class="RowEleven_Grid3_Pane" id="RowEleven_Grid3_Pane" runat="server"></div>
          </div>
        </section>
        <section class="row-fluid">
          <div class="span8">
            <div class="RowTwelve_Grid8_Pane" id="RowTwelve_Grid8_Pane" runat="server"></div>
          </div>
          <div class="span4">
            <div class="RowTwelve_Grid4_Pane" id="RowTwelve_Grid4_Pane" runat="server"></div>
          </div>
        </section>
        <section class="row-fluid">
          <div class="span7">
            <div class="RowThirteen_Grid7_Pane" id="RowThirteen_Grid7_Pane" runat="server"></div>
          </div>
          <div class="span5">
            <div class="RowThirteen_Grid5_Pane" id="RowThirteen_Grid5_Pane" runat="server"></div>
          </div>
        </section>
        <section class="row-fluid">
          <div class="span6">
            <div class="RowFourteen_Grid6_Pane1" id="RowFourteen_Grid6_Pane1" runat="server"></div>
          </div>
          <div class="span6">
            <div class="RowFourteen_Grid6_Pane2" id="RowFourteen_Grid6_Pane2" runat="server"></div>
          </div>
        </section>
        <section class="row-fluid">
          <div class="span5">
            <div class="RowFifteen_Grid5_Pane" id="RowFifteen_Grid5_Pane" runat="server"></div>
          </div>
          <div class="span7">
            <div class="RowFifteen_Grid7_Pane" id="RowFifteen_Grid7_Pane" runat="server"></div>
          </div>
        </section>
        <section class="row-fluid">
          <div class="span4">
            <div class="RowSixteen_Grid4_Pane" id="RowSixteen_Grid4_Pane" runat="server"></div>
          </div>
          <div class="span8">
            <div class="RowSixteen_Grid8_Pane" id="RowSixteen_Grid8_Pane" runat="server"></div>
          </div>
        </section>
        <section class="row-fluid">
          <div class="span3">
            <div class="RowSeventeen_Grid3_Pane" id="RowSeventeen_Grid3_Pane" runat="server"></div>
          </div>
          <div class="span9">
            <div class="RowSeventeen_Grid9_Pane" id="RowSeventeen_Grid9_Pane" runat="server"></div>
          </div>
        </section>
        <div class="BotOutPane" id="BotOutPane" runat="server"></div>
      </div>
    </section>
    <section id="dnn_bottom">
      <section class="row-fluid botpane">
        <div class="span3">
          <div class="BotPane_A" id="FooterPane_A" runat="server"></div>
        </div>
        <div class="span3">
          <div class="BotPane_B" id="FooterPane_B" runat="server"></div>
        </div>
        <div class="span3">
          <div class="BotPane_C" id="FooterPane_C" runat="server"></div>
        </div>
        <div class="span3">
          <div class="BotPane_D" id="FooterPane_D" runat="server"></div>
        </div>
      </section>
      <div class="dnn_footer clearfix">
        <div class="copyright_style">
          <dnn:COPYRIGHT runat="server" id="dnnCOPYRIGHT" CssClass="footer" />
          <span class="sep">|</span>
          <dnn:PRIVACY runat="server" id="dnnPRIVACY" CssClass="terms" />
          <span class="sep">|</span>
          <dnn:TERMS runat="server" id="dnnTERMS" CssClass="terms" />
          <dnn:STYLES runat="server" id="dnnSTYLES" Name="IE6Minus" StyleSheet="ie.css" Condition="LT IE 9" UseSkinPath="True" />
        </div>
        <div id="to_top"><img src="<%= SkinPath %>images/backtotop.png" alt="" title="Back To Top" /></div>
      </div>
    </section>
  </div>
</div>
<script type="text/javascript" src="http://maps.google.com/maps/api/js?sensor=false"></script> 
<script type="text/javascript" src="<%= SkinPath %>scripts/bootstrap.js"></script> 
<script type="text/javascript" src="<%= SkinPath %>scripts/scrolltop.js"></script> 
<script type="text/javascript" src="<%= SkinPath %>scripts/animatedcollapse.js"></script> 
<script type="text/javascript" src="<%= SkinPath %>scripts/jquery.clingify.js"></script> 
<script type="text/javascript" src="<%= SkinPath %>scripts/jquery.gmap.min.js"></script>
<script type="text/javascript" src="<%= SkinPath %>scripts/jquery.visible.js"></script>
<script type="text/javascript" src="<%= SkinPath %>scripts/custom.js"></script> 



