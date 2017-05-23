<%@ Control Language="VB" AutoEventWireup="false" CodeFile="AddEditStory.ascx.vb" Inherits="DotNetNuke.Modules.Stories.AddEditStory" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="../StaffAdmin/Controls/acImage.ascx" TagName="acImage" TagPrefix="uc1" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>

<script type="text/javascript" src='https://maps.googleapis.com/maps/api/js?key=<%= hfmapsKey.Value %>' async defer></script>
<script src="/js/jquery.locationpicker.js" type="text/javascript"></script>
<script type="text/javascript">

    (function ($, Sys) {
        function setUpMyTabs() {
            $("#<%= tbLocation.ClientId %>").locationPicker({ map: 'before', showMap: 'always', padding: 0, border: 0 });
            $('.picker-search-button').button();
            $('.picker-search-button').click();
            $('.picker-search-button').css('font-size', 'x-small');

            var pickerOpts = {
                dateFormat: '<%= GetDateFormat() %>'
            };
            $('.datepicker').datepicker(pickerOpts);

             $('#<%= cblTags.ClientID %>').find('input:checkbox').click(function () {
                showAppropriateOptions();
            });
        }

        function showAppropriateOptions() {
            $(".storyOption").hide();
            
            var chblist = $('#<%= cblTags.ClientID %>').find('input:checkbox');
            var tagdict = JSON.parse('<%= hftagDict.Value %>');
            var showEditor = true;

            chblist.each(function (index) {
                var item = $(this)
                if (item.is(':checked') && (tagdict[item.val()] == 'ExternalPage' || tagdict[item.val()] == 'Popup')) {
                    showEditor = false;
                }
                if (item.is(':checked') && tagdict[item.val()] == 'ExternalPage') {
                    $(".ExternalURL").show();
                }
                else if (item.is(':checked') && tagdict[item.val()] == 'Popup') {
                    $(".YouTube").show();
                }
            });
            if (showEditor) {
                $(".textEditor").show();
            }
            if ($(".ExternalURL").is(':visible') && $(".YouTube").is(':visible')) {
                $(".TagErrorMsg").show();
            }
        }

        $(document).ready(function () {
            showAppropriateOptions()
            setUpMyTabs();            
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                setUpMyTabs();
            });
        });
    }(jQuery, window.Sys));
</script>

<asp:HiddenField ID='hfmapsKey' runat="server" />
<asp:HiddenField ID='hftagDict' runat="server" />

<div id="divAddEditStory">
    <div id="divHeadline" class="FieldRow">
        <asp:Label ID="lblHeadline" runat="server" ResourceKey="lblHeadline" CssClass="FieldLabel"></asp:Label>
        <asp:TextBox ID="Headline"  runat="server" MaxLength="75"></asp:TextBox>
    </div>
    <div class="FieldErrorMsg">
        <asp:RequiredFieldValidator ID="rfHeadline" runat="server" resourcekey="rfHeadline" ControlToValidate="Headline" Display="Dynamic"></asp:RequiredFieldValidator>
    </div>

    <div id="divAuthor" class="FieldRow">
        <asp:Label ID="lblAuthor" runat="server" ResourceKey="lblAuthor" CssClass="FieldLabel"></asp:Label>
        <asp:TextBox ID="Author" runat="server" MaxLength="50"></asp:TextBox>
        <asp:DropDownList ID="ddlAuthor" runat="server" Visible="false"></asp:DropDownList>
    </div>
    
    <div id="divSubtitle" class="FieldRow">
        <asp:Label ID="lblSubtitle" runat="server" ResourceKey="lblSubtitle" CssClass="FieldLabel"></asp:Label>
        <asp:TextBox ID="Subtitle" runat="server" MaxLength="80"></asp:TextBox>
    </div>

    <div id="divStoryDate" class="FieldRow">
        <asp:Label ID="lblStoryDate" runat="server" ResourceKey="lblStoryDate" CssClass="FieldLabel"></asp:Label>
        <asp:TextBox ID="StoryDate" runat="server" CssClass="datepicker"></asp:TextBox>
    </div>

    <div id="divKeywords" class="FieldRow">
        <asp:Label ID="lblKeywords" runat="server" ResourceKey="lblKeywords" CssClass="FieldLabel"></asp:Label>
        <asp:TextBox ID="tbKeywords" runat="server" MaxLength="50"></asp:TextBox>
    </div>

    <div id="divTags" class="FieldRow">
        <asp:Label ID="lblTags" runat="server" ResourceKey="lblTags" CssClass="FieldLabel"></asp:Label>
        <asp:CheckBoxList ID="cblTags" CssClass="cblTags" runat="server"></asp:CheckBoxList>
    </div>
    <div class="TagErrorMsg storyOption">
        <asp:Label ID="lblTagErrorMsg" runat="server" ResourceKey="lblTagErrorMsg"></asp:Label>
    </div>

    <div id="divYouTube" class="storyOption YouTube FieldRow">
       <dnn:Label ID="lblYouTube" runat="server" ResourceKey="lblYouTube" CssClass="tooltipLabel"></dnn:Label>
        <asp:TextBox ID="tbField1" runat="server"></asp:TextBox>
    </div>

    <div id="divExternalURL" class="storyOption ExternalURL FieldRow">
        <asp:Label ID="lblExternalURL" runat="server" ResourceKey="lblExternalURL" CssClass="FieldLabel"></asp:Label>
        <asp:TextBox ID="tbField2" runat="server" MaxLength="50"></asp:TextBox>
    </div>

    <div id="divField3" class="FieldRow">
        <asp:Label ID="lblField3" runat="server" ResourceKey="lblField3" CssClass="FieldLabel"></asp:Label>
        <asp:TextBox ID="tbField3" runat="server" MaxLength="50"></asp:TextBox>
    </div>

    <div id="divLanguage" class="FieldRow">
        <asp:Label ID="lblLanguage" runat="server" ResourceKey="lblLanguage" CssClass="FieldLabel"></asp:Label>
        <asp:DropDownList ID="ddlLanguage" runat="server" Width="130px"></asp:DropDownList>
    </div>

    <div id="divChannel" class="FieldRow">
        <asp:Label ID="lblChannel1" runat="server" ResourceKey="lblChannel" CssClass="FieldLabel"></asp:Label>
        <asp:Label ID="lblChannel" runat="server" />
    </div> 
           
    <div id="divSample" class="FieldRow">
        <dnn:Label ID="lblSample" runat="server" ResourceKey="lblSample" CssClass="tooltipLabel"></dnn:Label>
        <asp:TextBox ID="tbSample" runat="server" TextMode="MultiLine" Text='<%# Bind("TemplateDescription") %>' ></asp:TextBox>
    </div>

    <div id="divAutoGenerate" class="FieldRow">
        <asp:Label ID="lblAutoGenerate" runat="server" ResourceKey="lblAutoGenerate" CssClass="FieldLabel"></asp:Label>
        <asp:CheckBox ID="cbAutoGenerate" runat="server" Text="AutoGenerate" />
    </div>
        
    <div id="divCropLocation" class="VisualRow">
        <div class="imageCropper">
            <uc1:acImage ID="acImage1" runat="server" SaveWidth="700" />
        </div>
        <div class="locationPicker">
            <asp:TextBox ID="tbLocation" CssClass="tbLocation" runat="server" ></asp:TextBox>
        </div>
    </div>  
        
    <div id="divTextEditor" class="storyOption textEditor FieldRow">
        <dnn:TextEditor ID="StoryText" runat="server" TextRenderMode="Raw" Width="100%" HtmlEncode="False" DefaultMode="Rich" Height="400" ChooseMode="True" ChooseRender="False" />
    </div>      

    <div class="SubmitPanel">
        <asp:Button ID="btnSave" runat="server" resourcekey="btnSave" CssClass="button" />
        <asp:Button ID="btnCancel" runat="server" resourcekey="btnCancel" CssClass="button" CausesValidation="false" />
    </div>
</div>
