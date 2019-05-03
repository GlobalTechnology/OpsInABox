<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ViewStory.ascx.vb" Inherits="DotNetNuke.Modules.FullStory.ViewFullStory" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="~/DesktopModules/AgapeConnect/Stories/controls/SuperPowers.ascx" TagPrefix="uc1" TagName="SuperPowers" %>

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
<asp:Panel ID="PagePanel" runat="server" >
    <asp:Literal ID="ltStory1" runat="server"></asp:Literal>
    <uc1:SuperPowers runat="server" ID="SuperPowers" Visible="False" />
    <asp:Literal ID="ltStory2" runat="server"></asp:Literal>
</asp:Panel>
</div>
