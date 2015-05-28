<%@ Control Language="VB" AutoEventWireup="false" CodeFile="AddEditStory.ascx.vb"
    Inherits="DotNetNuke.Modules.Stories.AddEditStory" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register Src="../StaffAdmin/Controls/acImage.ascx" TagName="acImage" TagPrefix="uc1" %>


<script src="/js/jquery.watermarkinput.js" type="text/javascript"></script>
<script type="text/javascript" src='http://maps.google.com/maps/api/js?sensor=false'></script>
<script src="/js/jquery.locationpicker.js" type="text/javascript"></script>
<script type="text/javascript">
    /*globals jQuery, window, Sys */
    (function ($, Sys) {
        function setUpMyTabs() {
            $('#<%= Headline.ClientId %>').Watermark('Headline');
            $('#<%= Author.ClientId %>').Watermark('Author');
            $('#<%= Subtitle.ClientID%>').Watermark('Subtitle');
            $('.aButton').button();
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

<style type="text/css">
  
</style>

<asp:HiddenField ID="StoryIdHF" runat="server" />
<asp:HiddenField ID="ShortTextHF" runat="server" />
<asp:HiddenField ID="PhotoIdHF" runat="server" />



<asp:Label ID="NotFoundLabel" runat="server" Text="Story Not Found" Font-Bold="True"
    ForeColor="Red"></asp:Label>
<asp:Panel ID="PagePanel" runat="server" Style="margin-right: 0px; margin-left: 0px; padding-left 0px;">
    <div class="Agape_Story_storymain">
        <h1 class="AgapeH2">
            <asp:TextBox ID="Headline" CssClass="AgapeH2" Style="border-bottom-style: none; width: 100%;" runat="server"></asp:TextBox>
        </h1>
        <div class="Agape_Story_subtitle">
            <table width="100%">
                <tr>
                    <td class="Agape_Story_subtitle" align="left" style="width: 25%; white-space: nowrap">By
                        <asp:TextBox ID="Author" runat="server" class="Agape_Story_subtitle" Style="width: 90%; display: inline;"></asp:TextBox>
                        <asp:DropDownList ID="ddlAuthor" runat="server" Style="width: 90%; display: inline;" Visible="false"></asp:DropDownList>
                    </td>
                    <td style="width: 65%;">
                        <asp:TextBox ID="Subtitle" runat="server" class="Agape_Story_subtitle" Style="width: 90%; display: inline;"></asp:TextBox>
                    </td>
                    <td class="Agape_Story_subtitle" align="right" style="padding-right: 25px">
                        <%-- <asp:Label ID="StoryDate2" runat="server"></asp:Label>--%>
                        <asp:TextBox ID="StoryDate" Width="75px" runat="server" CssClass="datepicker"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
        <div class="Agape_FullStory_bodytext" style="padding-right: 12px; margin-bottom: 10px;">
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




                        <table border="0" cellpadding="2" cellspacing="2">
                            <tr>
                                <td>
                                    <strong>Tags:</strong>

                                </td>
                                <td>
                                    <asp:CheckBoxList ID="cblTags" runat="server" Font-Size="Small"></asp:CheckBoxList>
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <strong>Keywords:</strong>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbKeywords" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>Field1:</b>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbField1" runat="server" TextMode="MultiLine" Width="100%"></asp:TextBox>
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <b>Field2:</b>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbField2" runat="server" TextMode="MultiLine" Width="100%" MaxLength="50"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>
                                        <asp:Label ID="lblField3" runat="server" Text="Field3:"></asp:Label></b>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbField3" runat="server" TextMode="MultiLine" Width="100%" MaxLength="50"></asp:TextBox>
                                </td>
                            </tr>
                        </table>



                    </td>
                    <td>
                        <dnn:TextEditor ID="StoryText" runat="server" TextRenderMode="Raw" Width="100%" HtmlEncode="False" DefaultMode="Rich" Height="800" ChooseMode="True" ChooseRender="False" />
                    </td>
                </tr>
            </table>


        </div>
    </div>
    <div style="clear: both;" />
    <div align="center">
        <asp:Button ID="btnSave" runat="server" Text="Save" class="aButton btn" />
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="aButton btn" />
    </div>
</asp:Panel>
