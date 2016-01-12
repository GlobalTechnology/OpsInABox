<%@ Control Language="vb" AutoEventWireup="false" CodeFile="StoryMixer.ascx.vb"
    Inherits="DotNetNuke.Modules.Stories.StoryMixer" %>
<%@ Register Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" TagPrefix="uc1" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register src="../StaffAdmin/Controls/acImage.ascx" tagname="acImage" tagprefix="uc2" %>
<script src="/js/knobKnob/transform.js" type="text/javascript"></script>
<script src="/js/knobKnob/knobKnob.jquery.js" type="text/javascript"></script>
<script src="/js/jquery.numeric.js" type="text/javascript"></script>
<script src="/js/jquery.jscrollpane.min.js" type="text/javascript"></script>
<link href="/js/jquery.jscrollpane.css" rel="stylesheet" type="text/css" />
<script src="/js/jquery.mousewheel.js" type="text/javascript"></script>
<script src="/js/mwheelIntent.js" type="text/javascript"></script>
<link href="/js/knobKnob/knobKnob.css" rel="stylesheet" type="text/css" />
<link href="/js/knobKnob/styles.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src='https://maps.google.com/maps/api/js'></script>
<script src="/js/jquery.locationpicker.js" type="text/javascript"></script>

<script type="text/javascript">

    function setUpMyTabs() {
        $('.numeric').numeric();
        $("#AddChannel").dialog({
            autoOpen: false,
            height: 600,
            width: 650,
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
            change: function (event, ui) {$("#<%= hfPopular.ClientId %>").val(ui.value);}
        });
         $("#regional").slider({
             value: $("#<%= hfRegional.ClientId %>").val(),
            orientation: "vertical",
            range: "min",
            animate: true,
            change: function (event, ui) {$("#<%= hfRegional.ClientId %>").val(ui.value);}
        });
         $("#recent").slider({
             value: $("#<%= hfRecent.ClientId %>").val(),
            orientation: "vertical",
            range: "min",
            animate: true,
            change: function (event, ui) {$("#<%= hfRecent.ClientId %>").val(ui.value);}
        });
        $("#numberOfStories").slider({
            value: $("#<%= hfNumberOfStories.ClientId %>").val(),
            orientation: "horizontal",
            range: "min",
            animate: true,
            min: 5,
            max: 50,
            step: 1,
            slide: function (event, ui) {
                $("#<%= lblNumberOfStories.ClientId %>").html( ui.value);
                 $("#<%= hfNumberOfStories.ClientId %>").val( ui.value);
            }
        });

        $("#<%= lblNumberOfStories.ClientId %>").html($("#numberOfStories").slider("value"));
      
         $("#<%= tbLocation.ClientId %>").locationPicker();
          $('.picker-search-button').button();
           $('.picker-search-button').css('font-size','x-small');
    }

	//function pageLoad() {
	//	setUpMyTabs();
	//	setDials();
	//}
	
    $(document).ready(function () {
        setUpMyTabs();
        setDials();
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () { setUpMyTabs(); setDials(); });
    });

    function setDials()
    {
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
                    if(!(typeof(numBars[i]) === 'undefined'))
                        s = s + i + '=' +  numBars[i] + ';';}
                
                $("#<%= hfVolumes.ClientId %>").val(s);


            }
        });

    }


    function showPopup() { $("#AddChannel").dialog("open"); return false; }
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
    .SettingsTable
    {
        border-top: 1px solid #e5eff8;
        border-right: 1px solid #e5eff8;
       
        border-collapse: collapse;
        width: 500px;
  
    }
    .SettingsTable td
    {
        color: #678197;
        border-bottom: 1px solid #e5eff8;
        border-left: 1px solid #e5eff8;
        padding: .3em 1em;
    }
    .SettingsTable td td
    {
        border-style: none;
    }
    
    
    #trapezoid
    {
        border-top: 40px solid   #999;
        border-left: 270px inset transparent;
        border-right: 270px inset transparent;
      
        width: 500px;
        margin: 0 auto ;
        
    }
    #eq span {
		height:120px; float:left; margin:15px;
	}
    .resizable { color: White; background-color: #1482b4; }
    .resizable .ui-resizable-se { color: White;}
    .pickermap
    {
        z-index: 999;   
    }
   
    
</style>



<asp:HiddenField ID="hfStoryModuleId" runat="server" Value="-1" />
<asp:HiddenField id='hfVolumes' runat="server"    />
<asp:HiddenField id='hfLoadVolumes' runat="server"    />

<asp:HiddenField id='hfNumberOfStories' runat="server"    />
<asp:HiddenField id='hfBlocks' runat="server" Value=";" />
<asp:HiddenField id='hfBoosts' runat="server" Value=";" />
<div>
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

<div id="trapezoid" />
  
<table cellpadding="4px" border="1" class="SettingsTable">
    <tr>
        <td>
        <dnn:Label ID="Label5" runat="server" ResourceKey="lblSettings" />
        </td>
        <td align="center">
        <table>
            <tr>
                <td>
                    <asp:HiddenField ID="hfPopular" runat="server" />
                <div id="popular" class="eq" style="height:100px; margin:15px;"></div>
                Popular
                </td>
                <td>
                <asp:HiddenField ID="hfRegional" runat="server" />
                 <div id="regional" class="eq" style="height:100px; margin:15px;"></div>
                 Local
                </td>
                <td>
                <asp:HiddenField ID="hfRecent" runat="server" />
                 <div id="recent" class="eq" style="height:100px; margin:15px;"></div>
                 Recent
                </td>
                
            </tr>
        </table>
        
       
        </td>
    </tr>
    <tr valign="middle">
        <td>
            <dnn:Label ID="Label2" runat="server" ResourceKey="lblNumberOfStories" />
        </td>
        <td align="center">
            <asp:Label ID="lblNumberOfStories" runat="server" style="float: right; margin-top: 12px;"></asp:Label>
         <div id="numberOfStories" style="width:200px; margin:15px;"></div>
         
           
            <div style="clear: both;"></div>
        </td>
    </tr>
    
</table>

</div>
<br /><br />
<div style="width: 100%; text-align: center">
    <asp:LinkButton ID="SaveBtn" runat="server" class="aButton btn" ResourceKey="btnSave">Save</asp:LinkButton>
    &nbsp;
    <asp:LinkButton ID="CancelBtn" runat="server" class="aButton btn" ResourceKey="btnCancel">Done</asp:LinkButton>
    &nbsp;
    <asp:LinkButton ID="btnCache" runat="server" class="aButton btn">Refresh Cache</asp:LinkButton>
 &nbsp;
    <input type="button" value="Add New Channel"  onclick="showPopup();" class="aButton btn" style="font-size: 8pt" />
 
</div>

 

<div id="AddChannel">
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>



    <table class="SettingsTable" style="width: 600px;">
        <tr>
            <td>
                
                 <dnn:label ID="labelcontrol1" runat="server" Text="RSS Feed:" HelpText="Enter the URL of the RSS Feed." />
               
            </td>
            <td>
                <asp:TextBox ID="tbRssFeed" runat="server" Width="380px"></asp:TextBox>
                <asp:LinkButton ID="lbVerifyURL" runat="server">Verify</asp:LinkButton>
                <br />
                <asp:Label ID="lblFeedError" runat="server" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <asp:Panel ID="pnlloaded" runat="server" Visible="false" >
        <tr>
            <td>
                <dnn:label ID="label6" runat="server" Text="Title:" HelpText="Enter the name you wish to refer to this feed by" />
            </td>   
            <td>
                <asp:TextBox ID="tbTitle" runat="server" Width="250px"></asp:TextBox>
               
            </td>
        </tr>
        <tr>
            <td>
                <dnn:label ID="label7" runat="server" Text="Language:" HelpText="The language of the feed." />
            </td>   
            
            <td>
                
                <asp:DropDownList ID="ddlLanguages" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
       
             <tr>
            <td>
                <dnn:label ID="label9" runat="server" Text="AutoDetect Story Language:" HelpText="Select this option if your feed published in multiple langauges, and has not been generated using AgapeConnect. The system will automatically detect the language of each story." />
            </td>   
            
            <td>
                <asp:CheckBox ID="cbAutoDetectLanguage" runat="server" />
                
            </td>
        </tr>
        <tr>
            <td>
                <dnn:label ID="Label8" runat="server" Text="Image:" HelpText="The feed image is used when there is no Story Image, or the supplied story image is too low quality" />
            </td>   
            <td>
               <uc2:acImage ID="icImage" runat="server" Width="200" />
            </td>
        </tr>
         <tr>
            <td>
                <dnn:label ID="lbLocation" runat="server" Text="Location:" HelpText="The Location of this feed. Enter any part of your address and click search (to convert it to a longitude/latitude location)" />
            </td>   
            <td>
                <asp:TextBox ID="tbLocation" runat="server" Width="200px"></asp:TextBox>
               <div style="font-size: xx-small; color: #AAA; font-style: italic;">(City, country,region or  postocode etc)</div>
            </td>
        </tr>
        </asp:Panel>
    </table>

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
</div>

<fieldset>
    <legend><h4>Preview Results</h4></legend>



    <asp:GridView ID="gvPreview" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Vertical">
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:BoundField DataField="Headline" HeaderText="Headline" />
            <asp:BoundField DataField="Score" DataFormatString="{0:0.000000}" HeaderText="Score">
            <ItemStyle HorizontalAlign="Right" />
            </asp:BoundField>
            <asp:BoundField DataField="Clicks" HeaderText="Clicks" />
            <asp:BoundField DataField="Age" HeaderText="Age" />
            <asp:BoundField />
            <asp:BoundField DataField="Distance" HeaderText="Distance" />
            <asp:BoundField DataField="Precal" HeaderText="Precal"  DataFormatString="{0:0.000000}" >
                <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>

        </Columns>
        <FooterStyle BackColor="#CCCC99" />
        <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
        <RowStyle BackColor="#F7F7DE" />
        <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
        <SortedAscendingCellStyle BackColor="#FBFBF2" />
        <SortedAscendingHeaderStyle BackColor="#848384" />
        <SortedDescendingCellStyle BackColor="#EAEAD3" />
        <SortedDescendingHeaderStyle BackColor="#575357" />
    </asp:GridView>


</fieldset>