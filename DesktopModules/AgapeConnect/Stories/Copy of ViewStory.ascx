    <%@ Control Language="VB" AutoEventWireup="false" CodeFile="Copy of ViewStory.ascx.vb"
    Inherits="DotNetNuke.Modules.FullStory.ViewFullStory" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
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


                $('.boost').prop("checked", <%= CStr(IsBoosted).ToLower %>).change();
               $('.block').prop("checked", <%= CStr(IsBlocked).ToLower %>).change();
                   
                   

                function initialize() {
                    var myLatlng = new google.maps.LatLng(<%= location %>);
                
                    var mapOptions = {
                        zoom: 5,
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
            $('#<%= lblPowerStatus.ClientID %>').html(blocked ? 'This story has been blocked, and won\'t appear in the channel feed.' : (boosted ? 'Boosted until <%= today.AddDays(7).ToString("dd MMM yyy") %> ' : '' ));

            $.ajax({ type: 'POST', url: '<%= EditUrl("ViewStory") & "?StoryId=" & Request.QueryString("StoryId")  %>',
                data: ({ Boosted: boosted, Blocked: blocked })
            });


        }

    </script>
<style type="text/css">
      #map_canvas {
        margin-bottom: 10px;
        padding: 0;
        width: 100%;
        height: 200px;
      }
      
      
    </style>
<asp:HiddenField ID="StoryIdHF" runat="server" />
<asp:HiddenField ID="ShortTextHF" runat="server" />
<asp:HiddenField ID="PhotoIdHF" runat="server" />
<asp:HiddenField ID="TranslationGroupHF" runat="server" />

<table  style="width: 100%">
    <tr valign="top">
        <td style="width: 100%">
            <asp:Label ID="NotFoundLabel" runat="server" Text="Story Not Found" Font-Bold="True"
                ForeColor="Red"></asp:Label>
            <asp:Panel ID="PagePanel" runat="server" Style="margin-right: 0px; margin-left: 0px;
                padding-left 0px; ">
                <div class="Agape_Story_storymain"  style="width: 100%">
                    <div class="AgapeH2">
                        <asp:Label ID="Headline" runat="server" Text="Error, Story not found."></asp:Label>
                    </div>
                    <div class="Agape_Story_subtitle">
                        <table width="100%">
                            <tr>
                                <td class="Agape_Story_subtitle" align="left">
                                    By
                                    <asp:Label ID="Author" runat="server" Text="Error, Story not found"></asp:Label>
                                </td>
                                <td class="Agape_Story_subtitle" align="right" style="padding-right: 25px">
                                    <asp:Label ID="StoryDate" runat="server" Text="Error, Story not found"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="Agape_FullStory_bodytext" style="padding-right: 12px; margin-bottom: 10px; ">
                        <asp:Image ID="StoryImage" runat="server" ImageUrl="~/images/action_print.gif" CssClass="Agape_Story_picturebox"
                            BorderColor="#000000" BorderStyle="Solid" BorderWidth="1pt" />
                        <asp:Label ID="StoryText" runat="server" Font-Size="11pt" ></asp:Label>
                        <br />
                        
                    </div>
                </div>
                <div style="clear: both;"></div>
           <div id="fbComments" runat="server" class="fb-comments" data-href="www.agape.org.uk"
                        data-num-posts="3" data-width="400" style="text-align: left; width: 50%;">
                    </div>
                <div align="right" style="padding-right: 15px;">
                    <asp:HyperLink ID="HyperLink1" runat="server" Target="_blank" ImageUrl="/images/action_rss.gif"
                        NavigateUrl="http://www.agape.org.uk/Stories/Story/tabid/143/moduleid/464/RSS.aspx">HyperLink</asp:HyperLink>
                </div>
                <div style="clear: both;" />
            </asp:Panel>
        </td>
        <td>
             
             <div style="width: 200px;">
             <!-- AddThis Button BEGIN -->
<div class="addthis_toolbox addthis_default_style " >
<a class="addthis_button_preferred_1"></a>
<a class="addthis_button_preferred_2"></a>
<a class="addthis_button_preferred_3"></a>
<a class="addthis_button_preferred_4"></a>
<a class="addthis_button_compact"></a>
<a class="addthis_counter addthis_bubble_style"></a>
</div>
<script type="text/javascript" src="https://s7.addthis.com/js/250/addthis_widget.js#pubid=xa-500677234debf3af"></script>
<!-- AddThis Button END -->

                  
                </div>
               

         
                <div>
                    <div id="fb-root" style="text-align: left;">
                    </div>
                    <script type="text/javascript">                        (function (d, s, id) {
                            var js, fjs = d.getElementsByTagName(s)[0];
                            if (d.getElementById(id)) { return; }
                            js = d.createElement(s); js.id = id;
                            js.src = "//connect.facebook.net/en_US/all.js#xfbml=1";
                            fjs.parentNode.insertBefore(js, fjs);
                        } (document, 'script', 'facebook-jssdk'));</script>
                    <br />
                   
                    
                </div>
                <div style="clear: both;"> </div>
              
            <asp:Panel ID="pnlLanguages" runat="server" Visible="false" Width="100%"  >

                <i>This story is also available in:</i>
                <div style="margin: 4px 0 12px 0;">
             <asp:DataList ID="dlLanuages" runat="server" RepeatDirection="Horizontal" ItemStyle-HorizontalAlign="Center"  Width="100%" >
                    <ItemTemplate>
                       <asp:HyperLink ID="HyperLink2" runat="server" ToolTip='<%# GetLanguageName(Eval("Language")) %>' ImageUrl='<%# GetFlag(Eval("Language"))  %>' NavigateUrl ='<%# NavigateURL() & "?StoryId=" & Eval("StoryId") %>'>HyperLink</asp:HyperLink>
                    </ItemTemplate>
             </asp:DataList>
                    </div>
           </asp:Panel>
                <div id="map_canvas"></div>
                
        


            <asp:Panel ID="SuperPowers" runat="server" Visible="false">
                    <br />
                    <table style="border-style: groove; border-width: thin">
                        <tr>
                            <td style="font-family: verdana; font-size: 14pt;">
                                Boost Story
                            </td>
                            <td colspan="2" valign="middle">
                                <asp:Button ID="BoostButton" runat="server" Text="Boost" />
                                &nbsp
                                <asp:Label ID="BoostLabel" runat="server" Text="BoostLabel" ForeColor="Gray" Font-Italic="True"
                                    Font-Names="Verdana" Font-Size="10pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-family: verdana; font-size: 14pt;">
                                Is Editable
                            </td>
                            <td style="text-align: left">
                                <asp:CheckBox ID="Editable" runat="server" Checked="True" />
                            </td>
                            <td>
                                <asp:LinkButton ID="UpdatePowerBtn" runat="server">Update</asp:LinkButton>
                                &nbsp;
                                <asp:LinkButton ID="CancelPowerBtn" runat="server">Cancel</asp:LinkButton>

                            </td>
                        </tr>
                    </table>

                </asp:Panel>
                
              <div style="white-space: nowrap; ">
                <asp:Button ID="btnTranslate" runat="server" Text="Translate"  Font-Size="X-Small" class="aButton btn" style="float: left;"  />
              
                 <asp:Button ID="btnEdit" runat="server" Text="Edit" Font-Size="X-Small" class="aButton btn" style="float: left;" />
              
                  <asp:Button ID="btnNew" runat="server" Text="New"  Font-Size="X-Small" class="aButton btn" style="float: left;"  />
              
              <input type="checkbox" id="boost" class="boost" style="height:20px;" /><label for="boost" style="height:20px; float: left;" >Boost</label>
	            <input type="checkbox" id="block" class="block" style="height:20px;"  /><label for="block" style="height:20px; float: left;" >Block</label>
              
              
               <br />
                  <div style="clear: both;"></div>
                
               
               </div>
                 <asp:Label ID="lblPowerStatus" runat="server" Text="" ForeColor="Red" Font-Italic="true" ></asp:Label>
               
        </td>
    </tr>
</table>