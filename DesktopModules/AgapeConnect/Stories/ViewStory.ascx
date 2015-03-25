    <%@ Control Language="VB" AutoEventWireup="false" CodeFile="ViewStory.ascx.vb"
    Inherits="DotNetNuke.Modules.FullStory.ViewFullStory" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="~/DesktopModules/AgapeConnect/Stories/controls/SuperPowers.ascx" TagPrefix="uc1" TagName="SuperPowers" %>



<script type="text/javascript" src="https://s7.addthis.com/js/250/addthis_widget.js#pubid=xa-500677234debf3af"></script>

<script src="https://maps.googleapis.com/maps/api/js?sensor=false"></script>
    <script type="text/javascript">
        /*globals jQuery, window, Sys */
        (function ($, Sys) {
            function setUpMyTabs() {
               
                $('.aButton').button();

                $('.block').button({icons: {primary: "ui-icon-closethick"},
                    text: false });


                $('.boost').button({
                    icons: {primary: "ui-icon-check"},
                    text: false});

                $('.boost').click(function() {
                    $('.block').prop("checked", false).change();
                    savePowerStates();
                });
               
                $('.block').click(function() {
                    $('.boost').prop("checked", false).change();
                    savePowerStates();
                });


                $('.boost').prop("checked", <%= CStr(SuperPowers.IsBoosted).ToLower%>).change();
               $('.block').prop("checked", <%= CStr(SuperPowers.IsBlocked).ToLower%>).change();
                  /* 
                var height = parseInt($(".Agape_FullStory_bodytext").css("height"));
                if(height>420)
                {
                    $('.Agape_FullStory_bodytext').addClass('preview');
                    $('#more').click(function() {
                   
                        $('.Agape_FullStory_bodytext').removeClass("preview", 2000);
                        $('#more').hide();
                    });}
                else
                {

                    $('#more').hide();
                }*/



                function initialize() {
                    var myLatlng = new google.maps.LatLng(<%= location %>);
                
                    var mapOptions = {
                        zoom: <%= zoomLevel%>,
                        center: myLatlng,
                        mapTypeId: google.maps.MapTypeId.ROADMAP
                    };


                    var map = new google.maps.Map(document.getElementById('map_canvas'),
            mapOptions);



                    var marker = new google.maps.Marker({
                        position: myLatlng,
                        map: map
                    });

                }

                google.maps.event.addDomListener(window, 'load', initialize);




            }

            $(document).ready(function () {
                setUpMyTabs();
                Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                    setUpMyTabs();
                });
            });



        } (jQuery, window.Sys));

        function savePowerStates()
        {
            var boosted = $('.boost').attr("checked") == 'checked' ;
            var blocked = $('.block').attr("checked") == 'checked' ;
            $('.PowerStatus').html(blocked ? 'This story has been blocked, and won\'t appear in the channel feed.' : (boosted ? 'Boosted until <%= Today.AddDays(StoryFunctions.GetBoostDuration(PortalId)).ToString("dd MMM yyy")%> ' : '' ));
            $.post(window.location.href,{ Boosted:  boosted, Blocked: blocked});

            //$.ajax({ type: 'POST', url: '<%= NavigateURL() & "?StoryId=" & Request.QueryString("StoryId")  %>',
           //     data: ({ Boosted: "'" + boosted +"'", Blocked: "'" + blocked + "'" })
            //});


        }

    </script>



<style type="text/css">
      #map_canvas {
        margin-bottom: 10px;
        padding: 0;
        width: 100%;
        height: 200px;
      }
      
    .textAreaSmall {
        height: 400px;
        overflow: hidden;

    }

      .tooltip {
  border-bottom: 1px dotted #000000;
  color: #000000; outline: none;
  cursor: help; text-decoration: none;
  position: relative;
}
.tooltip span {
  margin-left: -999em;
  position: absolute;
}
      .tooltip:hover span {
  font-family: Calibri, Tahoma, Geneva, sans-serif;
  position: absolute;
  left: 1em;
  top: 2em;
  z-index: 99;
  margin-left: 0;
  width: 250px;
}
.tooltip:hover img {
  border: 0;
  margin: -10px 0 0 -55px;
  float: left;
  position: absolute;
}
.tooltip:hover em {
  font-family: Candara, Tahoma, Geneva, sans-serif;
  font-size: 1.2em;
  font-weight: bold;
  display: block;
  padding: 0.2em 0 0.6em 0;
}
.tooltip:hover span {
			border-radius: 5px 5px; -moz-border-radius: 5px; -webkit-border-radius: 5px; 
			box-shadow: 5px 5px 5px rgba(0, 0, 0, 0.1); -webkit-box-shadow: 5px 5px rgba(0, 0, 0, 0.1); -moz-box-shadow: 5px 5px rgba(0, 0, 0, 0.1);
			font-family: Calibri, Tahoma, Geneva, sans-serif;
			position: absolute; left: 1em; top: 2em; z-index: 99;
			margin-left: 0; width: 250px;
		}

.classic { padding: 0.8em 1em; }
.classic {background: #FFFFAA; border: 1px solid #FFAD33; }

.custom { padding: 0.5em 0.8em 0.8em 2em; }
* html a:hover { background: transparent; }
 #more {
        width: 100%;
        text-align: center;
        font-style:italic;
    }
    #more:hover {
        cursor: pointer;
        text-decoration: underline;
    }
    .preview {
        height: 400px;
        overflow: hidden;
    }
    [for=block].ui-state-active,  [for=block].ui-widget-content  [for=block].ui-state-active,   [for=block].ui-widget-header   [for=block].ui-state-active {
        background: red !important;
        border-color: red !important;
    }
    </style>
<asp:HiddenField ID="StoryIdHF" runat="server" />
<asp:HiddenField ID="ShortTextHF" runat="server" />
<asp:HiddenField ID="PhotoIdHF" runat="server" />

<asp:Label ID="NotFoundLabel" runat="server" Text="Story Not Found" Font-Bold="True" Visible="False"
                ForeColor="Red"></asp:Label>

<asp:Panel ID="PagePanel" runat="server" >
    
    <asp:Literal ID="ltStory1" runat="server"></asp:Literal>
    <uc1:SuperPowers runat="server" ID="SuperPowers" Visible="False" />
        <asp:Literal ID="ltStory2" runat="server"></asp:Literal>
    

</asp:Panel>
