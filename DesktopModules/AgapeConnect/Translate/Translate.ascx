<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Translate.ascx.vb" Inherits="DotNetNuke.Modules.AgapeConnect.Translate" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register Src="Controls/LanguageEditor.ascx" TagName="LanguageEditor" TagPrefix="uc1" %>
<script type="text/javascript">
    /*globals jQuery, window, Sys */
    (function ($, Sys) {
        function setUpMyTabs() {
            var selectedTabIndex = $('#<%= theHiddenTabIndex.ClientID  %>').attr('value');
            //alert(selectedTabIndex);
            $('#tabs').tabs({

                show: function () {
                    var newIdx = $('#tabs').tabs('option', 'selected');
                    $('#<%= theHiddenTabIndex.ClientID  %>').val(newIdx);
                },
                selected: selectedTabIndex
            });

            $('.aButton').button();
            $('.aGoogleButton').button({ icons: { primary: "ui-icon-pencil" } }).css('background-image', "url('/DesktopModules/AgapeConnect/Translate/images/googleLogo.png')")
               .css('background-repeat', 'no-repeat').css('width', '40px').css('height', '40px');

            window.setInterval(saveDraft, 10000);

        }

        $(document).ready(function () {
            setUpMyTabs();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                setUpMyTabs();
            });
        });

        function saveDraft() {

            var data = ""
            var changedControls = $(".Translation")
            changedControls.each(function () {

                data += $(this).attr("TagKey") + "::" + $(this).val() + ";;";
                $(this).removeClass("Translation");
                try {
                    if ($('#<%= cbSaveAll.ClientId() %>').is(':checked')) {
                        var s = $(this).attr("TagKey").substring($(this).attr("TagKey").lastIndexOf("::") + 2);
                        // alert('hello');
                        var t = $(this).val()

                        $('[tagkey$="' + s + '"]').each(function () {
                            $(this).val(t)
                            data += $(this).attr("TagKey") + "::" + t + ";;";
                        });
                    }
                }
                catch (ex) { };

            });
            if (data != "") {
                $('#<%= lblSaveStatus.ClientId() %>').html("auto-saving...");
                $('#<%= lblSaveStatus.ClientId() %>').css('color', 'Gray');
                $.ajax({
                    type: 'POST', url: "<%= NavigateURL() %>",
                    data: ({ UpdateItems: data }),
                    success: function (response) {

                        if ($('#<%= lblSaveStatus.ClientId() %>').html != "Changes pending...") {
                            $('#<%= lblSaveStatus.ClientId() %>').html("Saved");
                            $('#<%= lblSaveStatus.ClientId() %>').css('color', 'Green');
                        }

                    },
                    error: function (response) {
                        $('#<%= lblSaveStatus.ClientId() %>').html("ERROR SAVING");
                        $('#<%= lblSaveStatus.ClientId() %>').css('color', 'Red');
                        //  alert('Save Error');
                        changedControls.each(function () {

                            $(this).addClass("Translation");
                        });
                    }
                });
            }
        }

    }(jQuery, window.Sys));
    function pendingChanges() {

        $('#<%= lblSaveStatus.ClientId() %>').html("Changes pending...");
        $('#<%= lblSaveStatus.ClientId() %>').css('color', 'Gray');
    }

    function bingTranslate(text, foreignName) {
        var from = "en"
        var to = $('#<%= ddlLanguages.ClientId() %>').val().substring(0, 2);

        $(".waitingForBing").each(function (index) {
            $(this).removeClass("waitingForBing");
        });

        $('.foreign').each(function (index) {

            var fn = ($(this).attr("TagKey"));

            if (fn == foreignName) {
                $(this).addClass("waitingForBing");

            }
        });

        var s = document.createElement("script");
        s.src = 'https://www.googleapis.com/language/translate/v2?key=AIzaSyBCSoev7-yyoFLIBOcsnbJqcNifaLwOnPc&source=' + from + '&target=' + to + '&callback=mycallback&q=' + encodeURIComponent(text);

        //        s.src = "http://api.microsofttranslator.com/V2/Ajax.svc/Translate" +
        //                "?appId=Bearer " + encodeURIComponent($('#<%= hfBingToken.ClientID  %>').attr('value')) +
        //                "&from=" + encodeURIComponent(from) +
        //                "&to=" + encodeURIComponent(to) +
        //                "&text=" + encodeURIComponent(text) +
        //                "&oncomplete=mycallback";
        document.body.appendChild(s);

    }

    function mycallback(response) {
        $(".waitingForBing").each(function (index) {
            $(this).val(response.data.translations[0].translatedText);
            $(this).removeClass("waitingForBing");
            $(this).addClass('Translation');
            pendingChanges();
        });
        // alert(response);
    }

</script>

<asp:HiddenField ID="theHiddenTabIndex" runat="server" Value="0" ViewStateMode="Enabled" />
<asp:HiddenField ID="hfBingToken" runat="server" />
<table width="100%" cellpadding="3px">
    <tr valign="top">
        <td>
            <asp:DropDownList ID="ddlLanguages" runat="server" AutoPostBack="true">
            </asp:DropDownList>
            <br />
            <asp:CheckBox ID="cbLocalNames" runat="server" Text="Show Native Names" AutoPostBack="true" />
            <br />
            <br />
            <asp:ListBox ID="lbModules" runat="server" AutoPostBack="true" Height="150px"></asp:ListBox>
            <br />
            <br />
            <table id="pnlExpenses" runat="server" visible="false">
                <tr>
                    <td>
                        <dnn:Label ID="lblSaveAll" runat="server" Text="Auto Update:" Width="70px" HelpText="Many of the expenses types contain identical translations. To save you the work of typing in each translation by hand, the system can automatically update the other modules for you. When you edit a value (triggering the autosave), it will also look for identical keys (eg &quot;lblAmount.Text&quot;) in the other expense types and save them." />
                    </td>
                    <td>
                        <asp:CheckBox ID="cbSaveAll" runat="server" />
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td><b>Show:</b> </td>
                    <td>
                        <asp:DropDownList ID="ddlRoot" runat="server" AutoPostBack="True">
                            <asp:ListItem Text="AgapeConnect" Value="/DesktopModules/AgapeConnect/" Selected="True" />
                            <asp:ListItem Text="AgapeFR" Value="/DesktopModules/AgapeFR/" />
                             <asp:ListItem Text="Core" Value="Core" />
                            <asp:ListItem Text="All Modules" Value="/DesktopModules/" />
                            <asp:ListItem Text="Everything" Value="/" />



                        </asp:DropDownList></td>
                </tr>
            </table>




            <br />
            <br />
        </td>
        <td width="100%">
            <asp:Panel ID="pnlTranslation" runat="server">
                <asp:Label ID="Label1" runat="server" Font-Size="XX-Small" ForeColor="Gray" Text="This page will autosave every 10 seconds."></asp:Label>
                <asp:Label ID="lblSaveStatus" runat="server" Font-Size="XX-Small" Font-Bold="true"
                    Text=""></asp:Label><br />
                <div id="tabs" width="100%" style="width: 90%; text-align: Left;">
                    <ul>
                        <asp:PlaceHolder ID="phHeaders" runat="server"></asp:PlaceHolder>
                    </ul>
                    <div style="width: 100%; min-height: 350px; background-color: #FFFFFF;">
                        <asp:PlaceHolder ID="phTabs" runat="server"></asp:PlaceHolder>
                    </div>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlDnnTranslation" runat="server" Visible="false">
                There are many translatable elements in DNN, but here are just a few of the essential items visible to public users.
                For this section, you must click save, for your translatins to be stored.

                <table>
                    <tr>
                        <td><asp:TextBox ID="TextBox2" runat="server" Enabled="False">Login</asp:TextBox></td>
                        <td><asp:TextBox ID="tbLogin" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td><asp:TextBox ID="TextBox1" runat="server" Enabled="False">You are here:</asp:TextBox></td>
                        <td><asp:TextBox ID="tbBreadcrumb" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td><asp:TextBox ID="TextBox3" runat="server" Enabled="False">Search:</asp:TextBox></td>
                        <td><asp:TextBox ID="tbSearch" runat="server"></asp:TextBox></td>
                    </tr>
                </table>
                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="aButton btn" />
            </asp:Panel>
        </td>
    </tr>
</table>
