<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Control Language="vb" AutoEventWireup="false" CodeFile="StoryMixer.ascx.vb"
    Inherits="DotNetNuke.Modules.Stories.StoryMixer" %>
<%@ Register Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" TagPrefix="uc1" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register src="../StaffAdmin/Controls/acImage.ascx" tagname="acImage" tagprefix="uc2" %>
<dnn:DnnJsInclude runat="server" FilePath="~/js/knobKnob/transform.js" />
<dnn:DnnJsInclude runat="server" FilePath="~/js/knobKnob/knobKnob.jquery.js" />
<dnn:DnnJsInclude runat="server" FilePath="~/js/jquery.jscrollpane.min.js" />
<dnn:DnnJsInclude runat="server" FilePath="~/js/jquery.mousewheel.js" />
<%--<dnn:DnnJsInclude runat="server" FilePath="~/js/jquery.mwheelIntent.js" />--%>
<dnn:DnnCssInclude runat="server" FilePath="~/js/jquery.jscrollpane.css" />
<dnn:DnnCssInclude runat="server" FilePath="~/js/knobKnob/knobKnob.css" />
<dnn:DnnCssInclude runat="server" FilePath="~/js/knobKnob/styles.css" />
<script type="text/javascript" src='https://maps.googleapis.com/maps/api/js?key=<%= hfmapsKey.Value %>' async defer></script>
<dnn:DnnJsInclude runat="server" FilePath="~/js/jquery.locationpicker.js" />

<script type="text/javascript">

    function setUpMyTabs() {
        $("#AddChannel").dialog({
            dialogClass: "no-close",
            closeOnEscape: false,
            autoOpen: false,
            height: 600,
            width: 750,
            modal: true,
            title: "Add New Channel"
        });
        $("#AddChannel").parent().appendTo($("form:first"));
        $('.aButton').button();
        $('.scroll-pane').jScrollPane();
        $("#popular").slider({
            value: $("#<%= hfPopular.ClientId %>").val(),
            orientation: "vertical",
            range: "min",
            animate: true,
            change: function (event, ui) { $("#<%= hfPopular.ClientId %>").val(ui.value); }
        });
        $("#regional").slider({
            value: $("#<%= hfRegional.ClientId %>").val(),
             orientation: "vertical",
             range: "min",
             animate: true,
             change: function (event, ui) { $("#<%= hfRegional.ClientId %>").val(ui.value); }
         });
        $("#recent").slider({
            value: $("#<%= hfRecent.ClientId %>").val(),
             orientation: "vertical",
             range: "min",
             animate: true,
             change: function (event, ui) { $("#<%= hfRecent.ClientId %>").val(ui.value); }
         });
        $("#numberOfStories").slider({
            value: $("#<%= hfNumberOfStories.ClientId %>").val(),
            orientation: "horizontal",
            range: "min",
            animate: true,
            min: 1,
            max: 50,
            step: 1,
            slide: function (event, ui) {
                $("#<%= lblNumberOfStories.ClientId %>").html(ui.value);
                $("#<%= hfNumberOfStories.ClientId %>").val(ui.value);
            }
        });
        $("#<%= lblNumberOfStories.ClientId %>").html($("#numberOfStories").slider("value"));

        
    }
    function initlocationpicker() {
        $("#<%= tbLocation.ClientId %>").locationPicker();
        $('.picker-search-button').button();
        $('.picker-search-button').css('font-size', 'x-small');
    }
    $(document).ready(function () {
        setUpMyTabs();
        setDials();
        initlocationpicker()
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () { initlocationpicker(); });
    });

    function pageLoad() {
        setUpMyTabs();
        setDials();
    }

    function setDials() {
        var colors = [
            '26e000', '2fe300', '37e700', '45ea00', '51ef00',
            '61f800', '6bfb00', '77ff02', '80ff05', '8cff09',
            '93ff0b', '9eff09', 'a9ff07', 'c2ff03', 'd7ff07',
            'f2ff0a', 'fff30a', 'ffdc09', 'ffce0a', 'ffc30a',
            'ffb509', 'ffa808', 'ff9908', 'ff8607', 'ff7005',
            'ff5f04', 'ff4f03', 'f83a00', 'ee2b00', 'e52000'
        ];

        var rad2deg = 180 / Math.PI;
        var bars = new Array();
        var deg = new Array();
        var numBars = new Array();
        var lastNum = new Array();
        var colorBars = new Array();

        $('.aKnobIndicator').each(function () {
            var id = parseInt(this.id.replace('ind', ''));
            bars[id] = $(this);
            deg[id] = 0;
            for (var i = 0; i < colors.length; i++) {
                deg[id] = i * 12;
                // Create the colorbars
                $('<div class="colorBar">').css({
                    backgroundColor: '#' + colors[i],
                    transform: 'rotate(' + deg[id] + 'deg)',
                    top: -Math.sin(deg[id] / rad2deg) * 42 + 50,
                    left: Math.cos((180 - deg[id]) / rad2deg) * 42 + 65
                }).appendTo(bars[id]);
            }
            colorBars[id] = bars[id].find('.colorBar');
            numBars[id] = 0, lastNum[id] = -1;
        });
        //        var deg = 0;
        //        var bars = $('.aKnobIndicator');
        $('.aKnob').knobKnob({
            snap: 10,
            volumesid: '<%= hfLoadVolumes.ClientId %>',
            turn: function (ratio, id) {
                // alert(id);
                numBars[id] = Math.round(colorBars[id].length * ratio);
                // Update the dom only when the number of active bars
                // changes, instead of on every move
                if (numBars[id] == lastNum[id]) {
                    return false;
                }
                lastNum[id] = numBars[id];
                colorBars[id].removeClass('active').slice(0, numBars[id]).addClass('active');
                // $(this).prev("input[type = 'hidden']").val(ratio);
                var s = '';
                for (var i = 0; i < numBars.length; i++) {
                    if (!(typeof (numBars[i]) === 'undefined'))
                        s = s + i + '=' + numBars[i] + ';';
                }
                $("#<%= hfVolumes.ClientId %>").val(s);
            }
        });
    }

    function showPopup() {
        $("#AddChannel").dialog("open"); return false;
    }
    function closePopup() { $("#AddChannel").dialog("close"); }

    function boost(sender, CacheId)
    {
        if(sender.src.indexOf('_off.png') <0)
        {
            
             sender.src='/DesktopModules/AgapeConnect/Stories/images/thumb_up_off.png';
             $("#<%= hfboosts.ClientId %>").val( $("#<%= hfboosts.ClientId %>").val().replace(';' + CacheId + ';', ';'));
        }
        else
        {
            sender.src='/DesktopModules/AgapeConnect/Stories/images/thumb_up.png' ;  
            $("#<%= hfboosts.ClientId %>").val($("#<%= hfboosts.ClientId %>").val() + CacheId + ';' );

        }

        $(sender).next().attr('src','/DesktopModules/AgapeConnect/Stories/images/thumb_down_off.png'); 
          $("#<%= hfblocks.ClientId %>").val( $("#<%= hfblocks.ClientId %>").val().replace(';' + CacheId + ';', ';'));
       
    }
   function block(sender, CacheId)
   {
        if(sender.src.indexOf('_off.png') <0 )
        {
            sender.src='/DesktopModules/AgapeConnect/Stories/images/thumb_down_off.png' ; 
             $("#<%= hfblocks.ClientId %>").val( $("#<%= hfblocks.ClientId %>").val().replace(';' + CacheId + ';', ';'));
        }
        else
        {
            sender.src='/DesktopModules/AgapeConnect/Stories/images/thumb_down.png' ; 
            $("#<%= hfblocks.ClientId %>").val($("#<%= hfblocks.ClientId %>").val() + CacheId + ';' );
        }
    
         $(sender).prev().attr('src','/DesktopModules/AgapeConnect/Stories/images/thumb_up_off.png'); 
           $("#<%= hfboosts.ClientId %>").val( $("#<%= hfboosts.ClientId %>").val().replace(';' + CacheId + ';', ';'));
   }

   function isBoosted(sender, CacheId)
   {
        return ($("#<%= hfboosts.ClientId %>").val().indexOf(';' + CacheId + ';', ';') >=0 );
   }
   function isBlocked(sender, CacheId)
   {
        return ($("#<%= hfBlocks.ClientId %>").val().indexOf(';' + CacheId + ';', ';') >=0 );
   }
</script>
<style type="text/css">
    .resizable { color: White; background-color: #1482b4; }
    .resizable .ui-resizable-se { color: White;}
    .pickermap
    {
        z-index: 999;   
    }
   .no-close div.ui-dialog-titlebar > .ui-dialog-titlebar-close {
  display: none;
}    
</style>

<div id="StoryMixer">
<asp:HiddenField ID="hfStoryModuleId" runat="server" Value="-1" />
<asp:HiddenField ID='hfVolumes' runat="server" />
<asp:HiddenField ID='hfLoadVolumes' runat="server" />
<asp:HiddenField ID='hfNumberOfStories' runat="server" />
<asp:HiddenField ID='hfBlocks' runat="server" Value=";" />
<asp:HiddenField ID='hfBoosts' runat="server" Value=";" />
<asp:HiddenField ID='hfmapsKey' runat="server" />

<asp:Panel ID="pnlChannelMixer" runat="server" BackColor="Black">
<div style="overflow-y: hidden; overflow-x: auto;  width: 1050px; " >
    <asp:DataList ID="dlChannelMixer" runat="server" RepeatDirection="Horizontal" BorderStyle="Dashed"
        GridLines="Vertical" ItemStyle-ForeColor="White">
        <ItemTemplate>
            <div style="width: 180px; text-align: center;">
            <div style=" vertical-align: middle; min-height: 25px; background-color: #555;"> 
                <asp:ImageButton ID="btnDeleteChannel" CommandArgument='<%# Eval("ChannelId") %>' Visible='<%# Eval("Type")<>2 %>' CommandName="DeleteChannel" runat="server" ImageUrl="/Icons/Sigma/Delete_16X16_Standard.png" style="float: right;"  />
                
                <asp:Label ID="Label3" runat="server" Font-Bold="true" Text='<%# Eval("ChannelTitle") %>'></asp:Label>
               <div style="clear: both; "></div>
            </div>
                <div style="height: 230px; overflow-y: scroll; overflow-x: hidden; " class="scroll-pane">
                    <asp:DataList ID="dlStories" runat="server" DataSource='<%# GetCache(Eval("AP_Stories_Module_Channel_Caches")) %>'>
                        <ItemTemplate>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" style="border-bottom: 1pt solid grey;">
                                <tr>
                                    <td style="width: 75%">
                                      <asp:HyperLink ID="HyperLink1" runat="server" Target="_blank" NavigateUrl='<%# Eval("Link") %>'>
                                <table border="0" cellpadding="0" cellspacing="0" style="margin-bottom: 3px; ">
                                    <tr>
                                        <td>
                                            <asp:Image ID="imgStory" runat="server" ImageUrl='<%# Eval("ImageId") %>' Width="40px" />
                                        </td>
                                        <td align="left" style="padding-left: 3px; line-height: 1em;">
                                            <asp:Label ID="Label4" runat="server" Font-Size="X-Small" ForeColor="White"  Text='<%# Eval("Headline") %>'></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </asp:HyperLink>
                                    </td>
                                    <td>
                                     
                                    <img id="thumbup"  src='<%# IsBoosted(Eval("CacheId"), Eval("BoostDate")) %>' onclick="boost(this,<%# Eval("CacheId") %>);" />
                                   
                                    <img id="thumbdown" src='<%# IsBlocked(Eval("CacheId"), Eval("Block")) %>' onclick="block(this,<%# Eval("CacheId") %>);" />
                                        
                                    </td>
                                </tr>
                            </table>
                          
                        </ItemTemplate>
                    </asp:DataList>
                   
                </div>
                

                <div id='<%# "ind" & Eval("ChannelId") %>' class="aKnobIndicator">
                    <div id='<%# "knob" & Eval("ChannelId") %>' class="aKnob">
                    </div>
                </div>
                 <asp:ImageButton ID="ImageButton1" CommandArgument='<%# Eval("ChannelId") %>' CommandName="EditChannel" runat="server" ImageUrl="/Icons/Sigma/Edit_16X16_Standard.png" style="margin-top: -20px;float: right;"  />
               
                <div style="clear: both;"> </div>
            </div>
        </ItemTemplate>
    </asp:DataList>

    </div>
    
</asp:Panel>
 
<div class="Settings">
    <dnn:Label ID="lblSettings" runat="server" ResourceKey="lblSettings" CssClass="FieldLabel"/>
    <div class="divPriorities">
        <div class="divPriority">
            <asp:HiddenField ID="hfPopular" runat="server" />
            <div id="popular" class=itemPriority1></div>
            <div class=itemPriority2><asp:Label ID="lblPopular" runat="server" ResourceKey="lblPopular"></asp:Label></div>
        </div>
        <div class="divPriority">
            <asp:HiddenField ID="hfRegional" runat="server" />
            <div id="regional"  class=itemPriority1></div>
            <div class=itemPriority2><asp:Label ID="lblLocal" runat="server" ResourceKey="lblLocal"></asp:Label></div>
        </div>
        <div class="divPriority">
            <asp:HiddenField ID="hfRecent" runat="server" />
            <div id="recent"  class=itemPriority1></div>
            <div class=itemPriority2><asp:Label ID="lblRecent" runat="server" ResourceKey="lblRecent"></asp:Label></div>
        </div>
    </div>
    <dnn:Label ID="lblNumberStories" runat="server" ResourceKey="lblNumberOfStories"  CssClass="FieldLabel" />
    <div class="divNumber">
        <div class=itemNumber1><asp:Label ID="lblNumberOfStories" runat="server"></asp:Label></div>
        <div class=itemNumber2 id="numberOfStories"></div>
        <div style="clear: both;"></div>
    </div>
</div>  <%--END Settings--%>


<div class="submitPanel">    
    <asp:UpdatePanel runat="server"> 
        <ContentTemplate>
            <asp:LinkButton ID="SaveBtn" runat="server" class="Button" ResourceKey="btnSave"></asp:LinkButton>
            <asp:LinkButton ID="CancelBtn" runat="server" class="Button" ResourceKey="btnCancel"></asp:LinkButton>
            <asp:LinkButton ID="btnCache" runat="server" class="Button" ResourceKey="btnRefresh"></asp:LinkButton>
            <input type="button" value='<%= Translate("btnNewChannel") %>'  onclick="showPopup();" class="Button" />
         </ContentTemplate> 
    </asp:UpdatePanel>
</div>


<div id="AddChannel">
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
                <dnn:Label ID="labelcontrol1" runat="server" Text="RSS Feed:" HelpText="Enter the URL of the RSS Feed." />
                <asp:TextBox ID="tbRssFeed" runat="server" Width="380px"></asp:TextBox>
                <asp:LinkButton ID="lbVerifyURL" CssClass="abutton" Style="margin: 5px 0px 5px 250px;" runat="server">Verify</asp:LinkButton>
                <br />
                <asp:Label ID="lblFeedError" runat="server" ForeColor="Red"></asp:Label>
    <div id="channeltable">
        <table class="SettingsTable" style="width: 600px;">
        <%--<asp:Panel ID="pnlloaded" runat="server" Visible="false">--%>
            <tr>
                <td>
                    <dnn:Label ID="label6" runat="server" Text="Title:" HelpText="Enter the name you wish to refer to this feed by" />
                </td>
                <td>
                    <asp:TextBox ID="tbTitle" runat="server" Width="250px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <dnn:Label ID="label7" runat="server" Text="Language:" HelpText="The language of the feed." />
                </td>
                <td>
                    <asp:DropDownList ID="ddlLanguages" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <dnn:Label ID="label9" runat="server" Text="AutoDetect Story Language:" HelpText="Select this option if your feed published in multiple langauges, and has not been generated using AgapeConnect. The system will automatically detect the language of each story." />
                </td>
                <td>
                    <asp:CheckBox ID="cbAutoDetectLanguage" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <dnn:Label ID="Label8" runat="server" Text="Image:" HelpText="The feed image is used when there is no Story Image, or the supplied story image is too low quality" />
                </td>
                <td>
                    <uc2:acImage ID="icImage" runat="server" Width="200" />
                </td>
            </tr>
            <tr>
                <td>
                    <dnn:Label ID="lbLocation" runat="server" Text="Location:" HelpText="The Location of this feed. Enter any part of your address and click search (to convert it to a longitude/latitude location)" />
                </td>
                <td>
                    <asp:TextBox ID="tbLocation" runat="server" Width="200px"></asp:TextBox>
                    <div style="font-size: xx-small; color: #AAA; font-style: italic;">(City, country,region or  postocode etc)</div>
                </td>
            </tr>
        <%--</asp:Panel>--%>
    </table>
    </div>
    <div style="width: 100%; text-align: center; margin-top: 1em;">
     <asp:Button ID="btnAddChannel" runat="server" class="aButton btn" Enabled="false" Text="Add Channel"></asp:Button>
     <asp:Button ID="btnEditChannel" runat="server" class="aButton btn" visible="false" Text="Save"></asp:Button>
 &nbsp;
  <asp:Button ID="btnAddCancel" runat="server" class="aButton btn"  Text="Cancel"></asp:Button>
    </div>
    </ContentTemplate>  
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="lbVerifyURL" EventName="Click" />
        
        <asp:PostBackTrigger ControlID="btnAddChannel" />
         <asp:PostBackTrigger ControlID="btnEditChannel" />
          <asp:PostBackTrigger ControlID="btnAddCancel" />
          <asp:PostBackTrigger ControlID="dlChannelMixer"  />
    </Triggers>
                </asp:UpdatePanel>
</div> <%--END AddChannel--%>


<div class="GridView">
    <asp:GridView ID="gvPreview" 
        runat="server"
        BorderStyle="None"
        GridLines="Vertical"
        AutoGenerateColumns="False"
        EmptyDataText="">
        <AlternatingRowStyle BackColor="#F7F7DE" />
        <HeaderStyle BackColor="#6B696B" ForeColor="White" CssClass="GridViewHeader" />
        <RowStyle CssClass="GridViewRows" />
        <Columns>
            <asp:BoundField DataField="Headline" HeaderText="Headline" />
            <asp:BoundField DataField="Score" DataFormatString="{0:0.000000}" HeaderText="Score"/>
            <asp:BoundField DataField="Clicks" HeaderText="Clicks" />
            <asp:BoundField DataField="Age" HeaderText="Age" />
            <asp:BoundField DataField="Distance" HeaderText="Distance" />
            <asp:BoundField DataField="Precal" HeaderText="Precal"  DataFormatString="{0:0.000000}" />
        </Columns>
    </asp:GridView>
</div>

</div> <%--END StoryMixer--%>