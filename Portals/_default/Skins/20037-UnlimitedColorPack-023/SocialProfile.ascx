<%@ Control language="vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Skins.Skin" %>
<%@ Register TagPrefix="dnn" TagName="LOGO" Src="~/Admin/Skins/Logo.ascx" %>
<%@ Register TagPrefix="dnn" TagName="login" Src="~/Admin/Skins/login.ascx" %>
<%@ Register TagPrefix="dnn" TagName="USER" Src="~/Admin/Skins/User.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SEARCH" Src="~/Admin/Skins/Search.ascx" %>
<%@ Register TagPrefix="dnn" TagName="COPYRIGHT" Src="~/Admin/Skins/Copyright.ascx" %>
<%@ Register TagPrefix="dnn" TagName="PRIVACY" Src="~/Admin/Skins/Privacy.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TERMS" Src="~/Admin/Skins/Terms.ascx" %>
<%@ Register TagPrefix="dnn" TagName="STYLES" Src="~/Admin/Skins/Styles.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Meta" Src="~/Admin/Skins/Meta.ascx" %>
<%@ Register TagPrefix="dnn" TagName="LANGUAGE" Src="~/Admin/Skins/Language.ascx" %>
<%@ Register TagPrefix="dnn" TagName="GOMENU" Src="~/DesktopModules/dnngo_gomenu/ViewMenu.ascx" %>
<%@ Register TagPrefix="dnn" TagName="GOMENU2" Src="~/DesktopModules/dnngo_gomenu/ViewMenu.ascx" %>
<%@ Register TagPrefix="dnn" TagName="BREADCRUMB" Src="~/Admin/Skins/BreadCrumb.ascx" %>
<%@ Register TagPrefix="dnn" TagName="CURRRENTDATE" Src="~/Admin/Skins/Currentdate.ascx" %>
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
    <section id="dnn_content">
      <div class="bread_date  clearfix">
        <div class="date_style">
          <dnn:CURRRENTDATE runat="server" id="dnnCURRRENTDATE" CssClass="date" DateFormat="dddd , MMMM , dd yyyy"/>
        </div>
        <div class="bread_style">
          <dnn:BREADCRUMB runat="server" id="dnnBREADCRUMB" Separator="&nbsp;&nbsp;>>&nbsp;&nbsp;" CssClass="breadcrumb" RootLevel="0" />
        </div>
      </div>
      <div class="pane_layout">
        <div id="ContentPane" runat="server"></div>
        <div id="RightPane" class="rightPane dnnRight" runat="server"></div>
        <div class="ProfilePanes">
          <div id="LeftPaneProfile" class="LeftPaneProfile" runat="server"></div>
          <!--close LeftPaneProfile-->
          <div id="HeaderPaneProfile" class="HeaderPaneProfile" runat="server"></div>
          <!--close HeaderPaneProfile-->
          <div id="ContentPaneProfile" class="ContentPaneProfile" runat="server"></div>
          <!--close LeftPaneProfile-->
          <div id="RightPaneProfile" class="RightPaneProfile" runat="server"></div>
          <!--close LeftPaneProfile--> 
        </div>
        <!-- close ProfilePanes -->
        <div id="BottomPane" runat="server"></div>
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
<script type="text/javascript" src="<%= SkinPath %>scripts/custom.js"></script> 
<script type="text/javascript" src="<%= SkinPath %>scripts/jquery.visible.js"></script>



