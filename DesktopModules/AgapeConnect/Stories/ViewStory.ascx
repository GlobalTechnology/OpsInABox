<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ViewStory.ascx.vb" Inherits="DotNetNuke.Modules.FullStory.ViewFullStory" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="~/DesktopModules/AgapeConnect/Stories/controls/SuperPowers.ascx" TagPrefix="uc1" TagName="SuperPowers" %>

<script type="text/javascript" src="https://s7.addthis.com/js/250/addthis_widget.js#pubid=xa-500677234debf3af"></script>
<script type="text/javascript" src='https://maps.googleapis.com/maps/api/js?key=<%= hfmapsKey.Value %>' async defer></script>

<script type="text/javascript">

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

                function initialize() {
                    var myLatlng = new google.maps.LatLng(<%= location %>);
                
                    var mapOptions = {
                        zoom: <%= zoomLevel%>,
                        center: myLatlng,
                        mapTypeId: google.maps.MapTypeId.ROADMAP
                    };

                    var map = new google.maps.Map(document.getElementById('map_canvas'), mapOptions);

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
            $('.PowerStatus').html(blocked ? '<%=LocalizeString("blockedMsg")%>' : (boosted ? '<%=LocalizeString("boostedMsg")%> <%= Today.AddDays(StoryFunctions.GetBoostDuration(PortalId)).ToString("dd MMM yyy")%> ' : '' ));
            $.post(window.location.href,{ Boosted:  boosted, Blocked: blocked});
        }
</script>

<div id="ViewStory">
<asp:HiddenField ID='hfmapsKey' runat="server" />

<asp:Panel ID="PagePanel" runat="server" >
    <asp:Literal ID="ltStory1" runat="server"></asp:Literal>
    <uc1:SuperPowers runat="server" ID="SuperPowers" Visible="False" />
    <asp:Literal ID="ltStory2" runat="server"></asp:Literal>
</asp:Panel>
</div>
