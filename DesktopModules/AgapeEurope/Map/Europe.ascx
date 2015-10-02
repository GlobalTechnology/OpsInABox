<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Europe.ascx.vb" Inherits="DotNetNuke.Modules.AgapeConnect.Europe" %>
<script src="/DesktopModules/AgapeEurope/Map/jqvmap/jquery.vmap.js" type="text/javascript"></script>
<script src="/DesktopModules/AgapeEurope/Map/jqvmap/maps/jquery.vmap.europe.js" type="text/javascript"></script>
<script src="/DesktopModules/AgapeEurope/Map/jqvmap/data/AreaData.js"></script>
<link href="/DesktopModules/AgapeEurope/Map/jqvmap/jqvmap.css" rel="stylesheet" type="text/css" />
<link href="/Portals/_default/Containers/AC-Furniture/Container.css" rel="stylesheet" />
<script src="/js/jquery.mousewheel.js" type="text/javascript"></script>
<script src="/js/mwheelIntent.js" type="text/javascript"></script>
<script src="/Scripts/linq.js" type="text/javascript"></script>
<style>
    div.on {
        border: 3px outset blue;
        cursor: pointer;
    }

    div.off {
        border: 3px outset transparent;
         cursor: default;
    }
      .scroll-pane
{

	
	overflow: auto;
	
}

     a.link-disabled:hover {
        cursor: default;
         text-decoration: none;
         
    }

    a.link-disabled #countryImage:hover {
        
        border: 1px solid #000000; max-width: 263px;max-height:177px;
    }
    a.link-disabled h2:hover {
        
        text-decoration: none;
    }

   a #countryImage:hover {
        border: 4px inset #7AA8E0; max-width: 256px; max-height:170px;

    }
   a  #countryImage {
        border: 1px inset #000000; max-width: 263px; max-height:177px;
    }

    

</style>


<script type="text/javascript">
    (function ($, Sys) {
        function setUpMyTabs() {
            //$('.scroll-pane').jScrollPane();

            //$('#countryPanel img').click(function () {
            //    var url = $('#<%= hfURL.ClientID %>').val();
            //    if(url !="")window.open($('#<%= hfURL.ClientID %>').val() ,'_blank');
            //});
            //$('#countryPanel h2').click(function () {
            //    var url = $('#<%= hfURL.ClientID %>').val();
            //    if (url != "") window.open($('#<%= hfURL.ClientID %>').val(), '_blank');
            //});
            $('#countryPanel').hide();
            $('#countryImage').error(function () {
                "PersonPlaceholder.jpg"
                var url = $(this).attr("src");
                if (url.length > 6 && url.indexOf("PersonPlaceholder.jpeg") == -1) {
                    url = url.substring(0, url.length - 6);
                    $(this).attr("src", url + "PersonPlaceholder.jpeg");
                }
            });

            var colors = {};
            for (var key in sample_data) {
                if (sample_data[key] < 0)
                    colors[key] = '#DDDDDD' ;
                else if (sample_data[key] >0)
                    colors[key] = '#DDDDDD';


            }


            $('#vmap').vectorMap({
                map: 'europe_en', colors: colors, backgroundColor: '#b9d1ef', hoverColor: '#9c0303',selectedColor:'#9c0303', borderWidth: 1, 
                enableZoom: false,
                showTooltip: true,
                onRegionClick: function (element, code, region) {

                    area = sample_data[code];
                    if (area >= 0) {
                        if (area == 1) {
                            code = 'eeu';

                        }

                        var data = jQuery.parseJSON(unescape(document.getElementById('<%= hfCountryData.ClientID %>').value));

                        Enumerable.From(data).Where(function (x) { return x.Code == code.toUpperCase() }).ForEach(function (i) {
                            $('#countryTitle').text(i.Name);

                            $('#countryText').html(i.Text);
                            $('#<%= hfURL.ClientID%>').val(i.URL);
                            
                            if (i.URL == "") {
                                $('#countryLink').removeAttr("href");
                                $('#countryLink').addClass("link-disabled");
                            }
                            else {
                                $('#countryLink').attr("href", i.URL);
                                $('#countryLink').removeClass("link-disabled");
                            }

                          

                        });

                        $('#countryImage').attr("src", "<%= PortalSettings.HomeDirectory %>" + "CountryImages/" + code.toUpperCase() + ".jpg");
                        

                        $('#countryPanel').show();
                        $('#countryText').css("overflow", "-moz-scrollbars-vertical");
                        $('#countryText').css("overflow-y", "auto");
                        $('#countryText').css("overflow-x", "hidden");
                        $('#countryText').scrollTop(0);
                        var message = 'You clicked "'
                + region
                + '" which has the code: '
                + code.toUpperCase();
                    }
                    //alert(message);
                }
            });


        }

        $(document).ready(function () {
            setUpMyTabs();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                setUpMyTabs();
            });
        });
    }(jQuery, window.Sys));


</script>

<asp:HiddenField ID="hfURL" runat="server" Value="" />

<table style="width:100%">
    <tr>
        <td style="width: 66%">
             <div id="vmap" style="width: 100%; height: 550px;"></div>
        </td>
        <td  style="width: 34%;">
              <div id="countryPanel">
           
             <div class="Container-10393-1" style="padding: 0; margin: 0">
                <div class="contentmain1" style="padding-left: 10px; padding-right: 10px; padding-top: 10px;">
                    <div style="height: 533px;">
                       <a id="countryLink" href="" target="_blank">
                           <div style="width: 100%; text-align: center;">
                         <h2><span id="countryTitle"></span></h2>
                           
                                <img alt="" id="countryImage" src=""   />
                           </div>
                       </a>
                            <div id="countryText" class="scroll-pane" style="overflow: scroll; height: 310px; margin-top:5px; font-size: small; padding-right: 5px;"></div>

                    </div>
                </div>
            </div>
             
        </div>

        </td>
    </tr>

</table>




<asp:HiddenField ID="hfCountryData" runat="server" />
