<%@ Control Language="vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Skins.Skin" %>
<%@ Register TagPrefix="dnn" TagName="Header" Src="~/Portals/_default/Skins/AgapeFR/controls/Header.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Footer" Src="~/Portals/_default/Skins/AgapeFR/controls/Footer.ascx" %>

<script type="text/javascript">
    jQuery(function ($) {

        // Init tabs with first tab selected
        $('#ongletsdon').dnnTabs({ selected: 0 });

    });
</script>  

<script runat="server">
       
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        'Needed for the dnnTabs to work
        DotNetNuke.Framework.jQuery.RequestDnnPluginsRegistration()
        
    End Sub

</script>

<dnn:Header runat="server" ID="dnnHeader" />

<div id="bannerbar" class="bar">
	<div id="BannerContainer" class="centeredbox">
        <div id="BannerPane" class="BannerPane" runat="server">
		</div>
	</div>
</div>	
<div id="bar3" class="bar">
	<div id="ContentContainer" class="centeredbox">
        <div id="ContentPane" class="TopPane" runat="server">
		</div>
        <div class="LeftMenu">
            <div id="TopImg">
		    </div>
            <div class="LeftMenuPane">
                <a id="LeftMenu0" href="http://dons.agapefrance.org/equipiers" target="_blank">
                    <div>
                        <h1>Soutenir un missionnaire</h1>
                        <h2>Ou une famille.</h2>
                    </div>
                </a>
                <a id="LeftMenu1" href="http://dons.agapefrance.org/ministeres" target="_blank">
                    <div>
                        <h1>Soutenir un minist&egrave;re</h1>
                        <h2>Et une &eacute;quipe.</h2>
                    </div>
                </a>
                <a id="LeftMenu2" href="http://dons.agapefrance.org/projets" target="_blank">
                    <div>
                        <h1>Soutenir un projet</h1>
                        <h2>Ou un &eacute;v&eacute;nement.</h2>
                    </div>
                </a>
                <div id="BottomImg">
		        </div>
            </div>
		</div>
        <div class="CenterPane">
            <div id="ongletsdon">
                <ul>
                    <li><a href="#2">Pourquoi donner ?</a></li>
                    <li><a href="#3">Moyens de paiement</a></li>
                    <li><a href="#4">D&eacute;duction fiscale</a></li>
                    <li><a href="#5">Infos l&eacute;gales</a></li>
                </ul>
                <div id="2" class="dnnClear">
                    <div id="Tab2Pane" runat="server"></div>
                </div>
                <div id="3" class="dnnClear">
                    <div id="Tab3Pane" runat="server"></div>
                </div>
                <div id="4" class="dnnClear">
                    <div id="Tab4Pane" runat="server"></div>
                </div>
                <div id="5" class="dnnClear">
                    <div id="Tab5Pane" runat="server"></div>
                </div>
            </div> 
        </div>       
      </div>
</div>	

<dnn:Footer runat="server" ID="dnnFooter" />
