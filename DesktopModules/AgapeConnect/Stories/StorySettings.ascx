<%@ Control Language="vb" AutoEventWireup="false" CodeFile="StorySettings.ascx.vb"
    Inherits="DotNetNuke.Modules.Stories.StorySettings" %>
    
<%@ Register Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" TagPrefix="uc1" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register Src="../StaffAdmin/Controls/acImage.ascx" TagName="acImage" TagPrefix="uc1" %>


<script src="/js/jquery.numeric.js" type="text/javascript"></script>

<script type="text/javascript">

    function setUpMyTabs() {

        $('.numeric').numeric();
        $('.aButton').button();
        $("#<%= resizableStoryPhotoAspect.ClientId %>").resizable({
            maxWidth: 320, maxHeight: 100, minWidth: 57, minHeight: 24, resize: function (event, ui) {
                $("#<%= lblStoryPhotoAspect.ClientId %>").html((ui.size.width / ui.size.height).toFixed(2));
             $("#<%= hfStoryPhotoAspect.ClientId %>").val((ui.size.width / ui.size.height).toFixed(2));

         }
        });
        $("#<%= resizableTagPhotoAspect.ClientId %>").resizable({
            maxWidth: 320, maxHeight: 100, minWidth: 57, minHeight: 24, resize: function (event, ui) {
                $("#<%= lblTagPhotoAspect.ClientId %>").html((ui.size.width / ui.size.height).toFixed(2));
             $("#<%= hfTagPhotoAspect.ClientId %>").val((ui.size.width / ui.size.height).toFixed(2));

         }
         });
        
        $('.TagsDisplayType').change(function () {
            var v = $(this).val();

            if(v != "") {
                //Show parameters used for tag list
                $("[class^='TagListParam']").show();
            } else {
                //Hide parameters used for tag list
                $("[class^='TagListParam']").hide();
            }
        });
        $('.TagsDisplayType').change();

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
        width: inherit;
  
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

      .SubmitPanel
    {
    margin-top: 10px;
    padding-left: 200px;
    width: 100%; 
    text-align: center; 
    }
      .AddTagPanel
    {
    display: inline-block;
    margin-top: 15px;
    }
       .DeleteTagWarning
    {
    display: inline-block;
    margin-top: 10px;
    }
    </style>

<div style="width:100%; text-align: center;">
<asp:HiddenField id='hfStoryPhotoAspect' runat="server"    />
    <asp:HiddenField id='hfTagPhotoAspect' runat="server"    />
<asp:HiddenField id='hfSpeed' runat="server"  Value="3"  />

<table cellpadding="4px" border="1" class="SettingsTable" style="margin: 0 auto;">
    
    <tr>
        <td>
        <dnn:Label ID="lblTagsDisplayType" runat="server" ResourceKey="lblTagsDisplayType" />
        </td>
        <td style="text-align: center ;">
            <asp:DropDownList ID="ddlTagsDisplayTypes" runat="server" CssClass="TagsDisplayType" AppendDataBoundItems="true">
                <asp:ListItem Text="Don't show tag list" Value="" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr valign="middle" class="TagListParam" style="display: none;" >
        <td>
            <dnn:Label ID="lblTagPhotoAspectTitle" runat="server" ResourceKey="lblTagPhotoAspect" />
        </td>
        <td style="text-align: center;" >
            
            <asp:Panel ID="resizableTagPhotoAspect" class="resizable" runat="server" style="text-align: center; vertical-align: middle; display: table-cell;  ">
           
                <asp:Label ID="lblTagPhotoAspect" runat="server"    style=" font-weight: bold; font-size: large; text-align: center;  display: inline-block ;"></asp:Label>

            </asp:Panel>
            <i>(drag the bottom-right corner to change)</i>
            
        </td>
    </tr>
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
            <dnn:Label ID="lblStoryPhotoAspectTitle" runat="server" ResourceKey="lblStoryPhotoAspect" />
        </td>
        <td style="text-align: center; " >
            
            <asp:Panel ID="resizableStoryPhotoAspect" class="resizable" runat="server" style="text-align: center; vertical-align: middle; display: table-cell;  ">
           
           
              <asp:Label ID="lblStoryPhotoAspect" runat="server"    style=" font-weight: bold; font-size: large; text-align: center;  display: inline-block ;"></asp:Label>
           

             </asp:Panel>
             <i>(drag the bottom-right corner to change)</i>
            
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
<div class="SubmitPanel">
    <asp:LinkButton ID="SaveBtn" runat="server" Cssclass="button" ResourceKey="btnSave">Save</asp:LinkButton>
    <asp:LinkButton ID="CancelBtn" runat="server" Cssclass="button" ResourceKey="btnCancel">Cancel</asp:LinkButton>
</div>
</div>