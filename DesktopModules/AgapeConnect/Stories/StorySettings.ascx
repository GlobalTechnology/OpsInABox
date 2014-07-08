<%@ Control Language="vb" AutoEventWireup="false" CodeFile="StorySettings.ascx.vb"
    Inherits="DotNetNuke.Modules.Stories.StorySettings" %>
    
<%@ Register Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" TagPrefix="uc1" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>


<script src="/js/jquery.numeric.js" type="text/javascript"></script>

<script type="text/javascript" src='http://maps.google.com/maps/api/js?sensor=false'></script>
<script src="/js/jquery.locationpicker.js" type="text/javascript"></script>


<script type="text/javascript">

    function setUpMyTabs() {

        $('.numeric').numeric();
        $('.aButton').button();
        $("#<%= resizable.ClientId %>").resizable({
            maxWidth: 320, maxHeight: 100, minWidth: 57, minHeight: 24, resize: function (event, ui) {
                $("#<%= lblAspect.ClientId %>").html((ui.size.width / ui.size.height).toFixed(2));
             $("#<%= hfAspect.ClientId %>").val((ui.size.width / ui.size.height).toFixed(2));

         }
         });
         
        $('.DisplayType').change(function () {
            //alert($(this).val());
            var v = $(this).val();

            var t = v.substring(0, v.indexOf(':'));


            $("[class^='Type']").hide();
            $('.Type' + t).show();


        });
        $('.DisplayType').change();

        $("#<%= ddlAspectMode.ClientID %>").change(function () {
            //alert($(this).val());
            var m = $(this).val();

            
            
            $("[class^='AspectMode']").hide();
            $('.AspectMode' + m).show();


        });
        $("#<%= ddlAspectMode.ClientID %>").change();


        $("#speed").slider({
            value: <%= lblSpeed.Text%>,
             orientation: "horizontal",
             range: "min",
             animate: true,
             min: 1,
             max: 30,
             step: 1,
             slide: function (event, ui) {
               
                 $("#<%= lblSpeed.ClientID%>").html( ui.value);
                $("#<%= hfSpeed.ClientID%>").val( ui.value);
            }
         });
        $("#<%= lblSpeed.ClientID%>").html($("#speed").slider("value"));

        $("#<%= tbLocation.ClientId %>").locationPicker();
        $('.picker-search-button').button();
        $('.picker-search-button').css('font-size','x-small');


        $('.RssName').keyup(function() {
            if (this.value.match(/[^a-zA-Z0-9_]/g)) {
                this.value = this.value.replace(/[^a-zA-Z0-9_]/g, '');
            }
        });

    }

    $(document).ready(function () {
        setUpMyTabs();


        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () { setUpMyTabs();
        });



    });

   


    
</script>


<style type="text/css">
    .DisplayType 
    {
        font-size: large;
    }
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
     .resizable { color: White; background-color: #1482b4; }
    .resizable .ui-resizable-se { color: White;}

    .RssPrefix
    {
        font-family:'Courier New';
        color: #999;
    }
     .RssName
    {
        font-family:'Courier New';
        color: Black;
        font-weight: bold;
    }
      .pickermap
    {
        z-index: 999;   
    }
    </style>

<div style="width:100%; text-align: center;">
<asp:HiddenField id='hfAspect' runat="server"    />
<asp:HiddenField id='hfSpeed' runat="server"  Value="3"  />

<table cellpadding="4px" border="1" class="SettingsTable" style="margin: 0 auto;">
    <tr>
        <td>
        <dnn:Label ID="Label5" runat="server" ResourceKey="lblDisplayType" />
        </td>
        <td style="text-align: center ;">
            <asp:DropDownList ID="ddlDisplayTypes" runat="server" CssClass="DisplayType"></asp:DropDownList>
        
            
        </td>
    </tr>


    <tr valign="middle" class="Type1">
        <td>
            <dnn:Label ID="Label2" runat="server" ResourceKey="lblSpeed" />
        </td>
        <td align="center">
           <span style="white-space: nowrap;"<asp:Label ID="lblSpeed" runat="server" style="float: right; margin-top: 12px;">3</asp:Label> Seconds</span>
         <div id="speed" style="width:400px; margin:15px;"></div>
         
           
            <div style="clear: both;"></div>
       
        </td>


    </tr>

    <tr valign="middle" style="display:none;">
        <td>
            <dnn:Label ID="Label3" runat="server" ResourceKey="lblShow" />
        </td>
        <td align="center">
          
            <asp:CheckBoxList ID="cblShow" runat="server" RepeatColumns="2" >
                <asp:ListItem Text="Image" />
                <asp:ListItem Text="Title" />
                <asp:ListItem Text="Sample" />
                <asp:ListItem Text="Date" />
                <asp:ListItem Text="Field1" />
                <asp:ListItem Text="Field2" />
                <asp:ListItem Text="Field3" />

                    
            </asp:CheckBoxList>
        </td>
    </tr>

    <tr valign="middle" class="Type1" >
        <td>
            <dnn:Label ID="Label8" runat="server" ResourceKey="lblPhotoSize" />
        </td>
        <td align="center">
          <asp:TextBox ID="tbPhotoSize" runat="server" CssClass="numeric"></asp:TextBox> Pixels
        </td>
    </tr>
    <tr valign="middle" >
        <td>
            <dnn:Label ID="Label6" runat="server" ResourceKey="lblLocation" />
        </td>
        <td align="center">
          <asp:TextBox ID="tbLocation" runat="server" Width="200px"></asp:TextBox>
               <div style="font-size: xx-small; color: #AAA; font-style: italic;">(City, country,region or  postocode etc)</div>
            <asp:Label ID="lblFeedError" runat="server" ForeColor="Red"></asp:Label>
             </td>
    </tr>
    <tr valign="middle" >
        <td>
            <dnn:Label ID="Label7" runat="server" ResourceKey="lblRssName" />
        </td>
        
        <td align="center" style=" white-space: nowrap;">
          <asp:Label ID="lblRssPrefix" runat="server" CssClass="RssPrefix" ></asp:Label>

        <asp:TextBox ID="tbRssName" runat="server" CssClass="RssName"></asp:TextBox>
        </td>
    </tr>
    
    <tr valign="middle"  style="display: none;" >
        <td>
            <dnn:Label ID="Label9" runat="server" ResourceKey="lblAspectMode" />
        </td>
        <td align="center">
            <asp:DropDownList ID="ddlAspectMode" runat="server">
                <asp:ListItem Text="Zoom In" Value="0" />
                <asp:ListItem Text="Zoom Out" Value="1" />
                <asp:ListItem Text="Stretch" Value="2" />
                <asp:ListItem Text="Variable Height" Value="3" />
                <asp:ListItem Text="Variable Width" Value="4" />
            </asp:DropDownList>
            <asp:Label ID="Label10" runat="server" CssClass="AspectMode0" Font-Italic="true" Font-Size="xx-small" ResourceKey="AspectMode0"></asp:Label>
            <asp:Label ID="Label11" runat="server" CssClass="AspectMode1" Font-Italic="true" Font-Size="xx-small"  ResourceKey="AspectMode1"></asp:Label>
            <asp:Label ID="Label12" runat="server" CssClass="AspectMode2" Font-Italic="true" Font-Size="xx-small"  ResourceKey="AspectMode2"></asp:Label>
            <asp:Label ID="Label13" runat="server" CssClass="AspectMode3" Font-Italic="true" Font-Size="xx-small"  ResourceKey="AspectMode3"></asp:Label>
            <asp:Label ID="Label14" runat="server" CssClass="AspectMode4" Font-Italic="true" Font-Size="xx-small"  ResourceKey="AspectMode4"></asp:Label>
        </td>
    </tr>

    <tr valign="middle">
        <td>
            <dnn:Label ID="Label1" runat="server" ResourceKey="lblAspect" />
        </td>
        <td style="text-align: center; " >
            
            <asp:Panel ID="resizable" class="resizable" runat="server" style="text-align: center; vertical-align: middle; display: table-cell;  ">
           
           
              <asp:Label ID="lblAspect" runat="server"    style=" font-weight: bold; font-size: large; text-align: center;  display: inline-block ;"></asp:Label>
           

             </asp:Panel>
             <i>(drag the bottom-right corner to change)</i>
        </td>
    </tr>
     <tr valign="middle">
        <td>
            <dnn:Label ID="Label16" runat="server" ResourceKey="lblTags" />
        </td>
        <td style="text-align: left; " >
        
     

            <asp:GridView ID="GridView1" runat="server" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Vertical" DataSourceID="dsTags" AutoGenerateColumns="False" DataKeyNames="StoryTagId">
                <AlternatingRowStyle BackColor="White" />
                <FooterStyle BackColor="#CCCC99" />
                <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                <RowStyle BackColor="#F7F7DE" />
                <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                <SortedAscendingCellStyle BackColor="#FBFBF2" />
                <SortedAscendingHeaderStyle BackColor="#848384" />
                <SortedDescendingCellStyle BackColor="#EAEAD3" />
                <SortedDescendingHeaderStyle BackColor="#575357" />
                <Columns>

                    <asp:BoundField DataField="TagName" HeaderText="TagName" SortExpression="TagName" />
                    <asp:BoundField DataField="Keywords" HeaderText="Keywords" SortExpression="Keywords" />
                    <asp:CheckBoxField DataField="Master" HeaderText="Master" SortExpression="Master" />
                    <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />

                </Columns>
                
            </asp:GridView>

<asp:HiddenField runat="server" ID="hfPortalId"></asp:HiddenField>

              <asp:LinqDataSource ID="dsTags" runat="server" EntityTypeName="" ContextTypeName="Stories.StoriesDataContext" EnableDelete="True" EnableInsert="True" EnableUpdate="True" OrderBy="TagName" TableName="AP_Stories_Tags" Where="PortalId == @PortalId">
                  <WhereParameters>
                      <asp:ControlParameter ControlID="hfPortalId" Name="PortalId" PropertyName="Value" Type="Int32" />
                  </WhereParameters>
            </asp:LinqDataSource>



              <asp:TextBox ID="tbAddTag" runat="server"></asp:TextBox><asp:Button ID="btnAddTag" runat="server" Text="Add" CssClass="aButton" Font-Size="X-Small" /> 
            <br />*Warning: Deleting a tag will remove this tag from all stories. This cannot be undone!
        </td>
    </tr>
    <tr valign="middle" >
        <td>
            <dnn:Label ID="Label17" runat="server" ResourceKey="lblMode" />
        </td>
        
        <td align="center" style=" white-space: nowrap;">
         
            <asp:DropDownList ID="ddlMode" runat="server">
                <asp:ListItem Text="Direct Publish" Value="Direct" />
                <asp:ListItem Text="Content Staging" Value="Staged" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr valign="middle" >
        <td>
            <dnn:Label ID="Label18" runat="server" ResourceKey="lblBoostLength" />
        </td>
        
        <td align="center" style=" white-space: nowrap;">
            <asp:TextBox ID="tbBoostLength" runat="server" CssClass="numeric" Width="60px">30</asp:TextBox> Days
        </td>
    </tr>
     <tr valign="middle" >
        <td>
            <dnn:Label ID="Label15" runat="server" ResourceKey="lblStoriesPage" />
        </td>
        
        <td align="center" style=" white-space: nowrap;">
         
            <asp:DropDownList ID="ddlTabs" runat="server" AppendDataBoundItems="true">
                <asp:ListItem Text="Default" Value="0" />

            </asp:DropDownList>
        </td>
    </tr>


     <tr valign="middle" >
        <td>
            <dnn:Label ID="Label4" runat="server" ResourceKey="lblAdvancedSettings" />
        </td>
        
        <td align="center" style=" white-space: nowrap;">
         
        <asp:TextBox ID="tbAdvanceSettings" runat="server" TextMode="MultiLine" Rows="5" Width="80%" Font-Names="Courier"></asp:TextBox>
        </td>
    </tr>
    

</table>

 <br /><br />
<div style="width: 100%; text-align: center">
    <asp:LinkButton ID="SaveBtn" runat="server" class="aButton" ResourceKey="btnSave">Save</asp:LinkButton>
    &nbsp;
    <asp:LinkButton ID="CancelBtn" runat="server" class="aButton" ResourceKey="btnCancel">Cancel</asp:LinkButton>
 
</div>
    </div>