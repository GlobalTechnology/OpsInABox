<%@ Control Language="VB" AutoEventWireup="false" CodeFile="AddEditStory.ascx.vb" Inherits="DotNetNuke.Modules.Stories.AddEditStory" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register Src="../StaffAdmin/Controls/acImage.ascx" TagName="acImage" TagPrefix="uc1" %>

<script type="text/javascript" src='https://maps.googleapis.com/maps/api/js?key=<%= hfmapsKey.Value %>' async defer></script>
<script src="/js/jquery.locationpicker.js" type="text/javascript"></script>
<script type="text/javascript">
    /*globals jQuery, window, Sys */
    (function ($, Sys) {
        function setUpMyTabs() {
            $("#<%= tbLocation.ClientId %>").locationPicker({ map: 'before', showMap: 'always', width: '200px', padding: 0, border: 0 });
            $('.picker-search-button').button();
            $('.picker-search-button').click();
            $('.picker-search-button').css('font-size', 'x-small');

            var pickerOpts = {
                dateFormat: '<%= GetDateFormat() %>'
            };
        $('.datepicker').datepicker(pickerOpts);

        $('#<%= cbAutoGenerate.ClientId %>').click(function () {
                if ($(this).is(':checked')) {
                    $('#<%= tbSample.ClientID%>').hide();
                    $('#<%= lblSample.ClientID%>').show();
                }
                else {
                    $('#<%= tbSample.ClientID%>').show();
                    $('#<%= lblSample.ClientID%>').hide();
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
<asp:HiddenField ID="StoryIdHF" runat="server" />
<asp:HiddenField ID="ShortTextHF" runat="server" />
<asp:HiddenField ID="PhotoIdHF" runat="server" />
<asp:HiddenField ID='hfmapsKey' runat="server" />

<asp:Label ID="NotFoundLabel" runat="server" Text="Story Not Found" Font-Bold="True"
    ForeColor="Red"></asp:Label>



    <div id="divAddEditStory">
        <div id="divHeadline" class="FieldRow">
            <asp:Label ID="lblHeadline" runat="server" ResourceKey="lblHeadline" CssClass="FieldLabel"></asp:Label>
            <asp:TextBox ID="Headline"  runat="server" MaxLength="154"></asp:TextBox>
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

        <div id="divYouTube" class="FieldRow">
            <asp:Label ID="lblYouTube" runat="server" ResourceKey="lblYouTube" CssClass="FieldLabel"></asp:Label>
            <asp:TextBox ID="tbField1" runat="server"></asp:TextBox>
        </div>

        <div id="divExternalURL" class="FieldRow">
            <asp:Label ID="lblExternalURL" runat="server" ResourceKey="lblExternalURL" CssClass="FieldLabel"></asp:Label>
            <asp:TextBox ID="tbField2" runat="server" MaxLength="50"></asp:TextBox>
        </div>

        <div id="divField3" class="FieldRow">
            <asp:Label ID="lblField3" runat="server" ResourceKey="lblField3" CssClass="FieldLabel"></asp:Label>
            <asp:TextBox ID="tbField3" runat="server" MaxLength="50"></asp:TextBox>
        </div>
    



            <table>
                <tr>
                    <td>
                        <uc1:acImage ID="acImage1" runat="server" Aspect="1.3" SaveWidth="700" />
                        <br />
                        <table border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <b>Language:</b>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlLanguage" runat="server" Width="130px"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>Channel:</b>
                                </td>
                                <td>
                                    <asp:Label ID="lblChannel" runat="server" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <div style="float: right">
                            <asp:CheckBox ID="cbAutoGenerate" runat="server" Text="AutoGenerate" Font-Size="x-small" /></div>
                        <b>Sample:</b><div style="clear: both;"></div>


                        <asp:TextBox ID="tbSample" runat="server" Width="100%" Height="150px" TextMode="MultiLine" Text='<%# Bind("TemplateDescription") %>' Style="background-color: #fff9c8;"></asp:TextBox>
                        <asp:Label ID="lblSample" runat="server" Font-Size="X-Small" ForeColor="GrayText" Font-Italic="true" Text="The first 200 characters will be used."></asp:Label>


                        <asp:Panel ID="pnlLanguages" runat="server" Visible="false" Width="100%">
                            <br />
                            <b>Translations:</b><br />

                            <div style="margin: 4px 0 4px 0;">
                                <asp:DataList ID="dlLanuages" runat="server" RepeatDirection="Vertical" ItemStyle-HorizontalAlign="Left" Width="100%">
                                    <ItemTemplate>
                                        <table>
                                            <tr valign="middle">
                                                <td>
                                                    <asp:HyperLink ID="HyperLink2" runat="server" Target="_blank" ToolTip='<%# GetLanguageName(Eval("Language")) %>' ImageUrl='<%# GetFlag(Eval("Language"))  %>' NavigateUrl='<%# NavigateURL() & "?StoryId=" & Eval("StoryId") %>'>HyperLink</asp:HyperLink>

                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="LinkButton2" runat="server" CommandName="Translate" CommandArgument='<%# Eval("StoryId") %>' Font-Size="X-Small" Text=' <%# "AutoTranslate from " & GetLanguageName(Eval("Language"))%>'></asp:LinkButton>


                                                </td>
                                            </tr>
                                        </table>

                                    </ItemTemplate>
                                </asp:DataList>
                            </div>
                        </asp:Panel>


                        <br />

                        <asp:TextBox ID="tbLocation" runat="server" Width="130px" Style="margin-right: 3px;"></asp:TextBox>
                        <br />
                        <br />








                    </td>
                    <td>
                        <dnn:TextEditor ID="StoryText" runat="server" TextRenderMode="Raw" Width="100%" HtmlEncode="False" DefaultMode="Rich" Height="800" ChooseMode="True" ChooseRender="False" />
                    </td>
                </tr>
            </table>


      



    <div class="SubmitPanel">
        <asp:Button ID="btnSave" runat="server" resourcekey="btnSave" CssClass="button" />
        <asp:Button ID="btnCancel" runat="server" resourcekey="btnCancel" CssClass="button" />
    </div>
  </div>
