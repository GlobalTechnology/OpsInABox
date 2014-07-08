<%@ Control Language="vb" AutoEventWireup="false" CodeFile="RmbSettings.ascx.vb"
    Inherits="DotNetNuke.Modules.StaffRmb.RmbSettings" %>
<%@ Register Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" TagPrefix="cc1" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>

<script type="text/javascript" src="/js/engage.itoggle/engage.itoggle.js"></script>
<script type="text/javascript" src="/js/engage.itoggle/jquery.easing.1.3.js"></script>

<link href="/js/engage.itoggle/engage.itoggle.css" rel="stylesheet" />
<script src="/js/jquery.watermarkinput.js" type="text/javascript"></script>
<script src="/js/jquery.numeric.js" type="text/javascript"></script>
<script type="text/javascript">

    (function ($, Sys) {
        function setUpMyTabs() {
            var selectedTabIndex = $('#<%# theHiddenTabIndex.ClientID  %>').attr('value');
            $('#tabs').tabs({
                show: function () {
                    var newIdx = $('#tabs').tabs('option', 'selected');
                    $('#<%# theHiddenTabIndex.ClientID  %>').val(newIdx);
                },
                selected: selectedTabIndex
            });

            $('.numeric').numeric();
            $('.pdName').Watermark('Rate Name');
            $('.pdValue').Watermark('Rate Value');
            $('.aButton').button();


            $('span.iPhoneSwitch input:checkbox').iToggle({
                easing: 'easeOutExpo',
                keepLabel: true,
                easing: 'easeInExpo',
                speed: 300,
                onClick: function () {
                    //Function here
                },
                onClickOn: function () {
                    //Function here
                    alert('On');
                },
                onClickOff: function () {
                    //Function here
                },
                onSlide: function () {
                    //Function here
                },
                onSlideOn: function () {
                    //Function here
                },
                onSlideOff: function () {
                    //Function here
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
    .shortBox {
    width: 50px;
    }

</style>
<div id="tabs" style="width: 90%; text-align: Left;">
    <ul>
        <li><a href='#Tab1-tab'>Settings</a></li>
        <li><a href='#Tab2-tab'>Rates</a></li>
        <li><a href='#Tab3-tab'>Roles</a></li>
        <li><a href='#Tab4-tab'>Expense Types</a></li>
        <li><a href='#Tab5-tab'>Help</a></li>
    </ul>
    <div style="width: 100%; min-height: 350px; background-color: #FFFFFF;">
        <div id='Tab1-tab'>

            <table>
                <tr style="vertical-align: top;">
                    <td>
                        <table style="font-size: 9pt;">
                            <tr>
                                <td>
                                    <b>
                                        <dnn:Label ID="lblNoReceipt" runat="server" ControlName="tbNoReceipt" ResourceKey="lblNoReceipt" />
                                    </b>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbNoReceipt" runat="server" Width="80px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>
                                        <dnn:Label ID="lblElecRec" runat="server" ControlName="cbElectronicReceipts" ResourceKey="lblElecRec" />
                                    </b>
                                </td>
                                <td>
                                    <asp:CheckBox ID="cbElectronicReceipts" runat="server" Checked="False" />
                                </td>
                            </tr>
                            <tr >
                                <td>
                                    <b>
                                        <dnn:Label ID="lblVAT" runat="server" ControlName="cbVAT" ResourceKey="lblVAT" />
                                    </b>
                                </td>
                                <td>
                                    <asp:CheckBox ID="cbVAT" runat="server"  />
                                   
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>
                                        <dnn:Label ID="lblExpire" runat="server" ControlName="tbExpire" ResourceKey="lblExpire" />
                                    </b>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbExpire" runat="server" Width="80px"></asp:TextBox>
                                    <asp:Label ID="Label21" runat="server" resourcekey="Months"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>
                                        <dnn:Label ID="lblTeamLeaderLimit" runat="server" ControlName="tbTeamLeaderLimit"
                                            ResourceKey="lblTeamLeaderLimit" />
                                    </b>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbTeamLeaderLimit" runat="server" Width="80px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label9" runat="server" resourcekey="lblDistanceUnit"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlDistance" runat="server">
                                        <asp:ListItem Text="Miles" Value="miles" />
                                        <asp:ListItem Text="Kilometers" Value="km" />
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>
                                        <dnn:Label ID="Label39" runat="server" ControlName="tbDescriptionLength" ResourceKey="lblDescriptionLength" />
                                    </b>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbDescriptionLength" runat="server" Width="80px" CssClass="numeric"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>
                                        <dnn:Label ID="lblMenuSize" runat="server" ControlName="tbMenuSize" ResourceKey="lblMenuSize" />
                                    </b>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbMenuSize" runat="server" Width="80px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>
                                        <dnn:Label ID="Label30" runat="server" ControlName="cbRemBal" ResourceKey="lblShowRemBal" />
                                    </b>
                                </td>
                                <td>
                                    <asp:CheckBox ID="cbRemBal" runat="server" />
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <b>
                                        <dnn:Label ID="Label31" runat="server" ControlName="cbRemBal" ResourceKey="lblWarnIfNegative" />
                                    </b>
                                </td>
                                <td>
                                    <asp:CheckBox ID="cbWarnIfNegative" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>
                                        <dnn:Label ID="Label8" runat="server" ControlName="cbCurConverter" ResourceKey="lblCurConverter" />
                                    </b>
                                </td>
                                <td>
                                    <asp:CheckBox ID="cbCurConverter" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>
                                        <dnn:Label ID="Label5" runat="server" ControlName="ddlDownloadFormat" ResourceKey="lblDownloadFormat" />
                                    </b>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlDownloadFormat" runat="server">
                                        <asp:ListItem Text="Solomon: Desc, Debit, Credit" Value="DDC" />
                                        <asp:ListItem Text="Solomon: Debit, Credit, Description" Value="DCD" />
                                        <asp:ListItem Text="Solomon: Company, Desc, Debit, Credit" Value="CDDC" />
                                        <asp:ListItem Text="Solomon: Company, Debit, Credit, Description" Value="CDCD" />
                                        <asp:ListItem Text="Generic - Detailed" Value="GEN" />
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>
                                        <dnn:Label ID="lblControlAccount" runat="server" ControlName="ddlControlAccount"
                                            ResourceKey="lblControlAccount" />
                                    </b>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbControlAccount" runat="server" Visible='false'></asp:TextBox>
                                    <asp:DropDownList ID="ddlControlAccount" runat="server" DataSourceID="dsCostCenters"
                                        DataTextField="DisplayName" DataValueField="CostCentreCode" AppendDataBoundItems="true">
                                        <asp:ListItem Text="" Value="" />
                                    </asp:DropDownList>
                                    <asp:Label ID="oopsControlAccount" runat="server" Text="" ForeColor="Red"></asp:Label>
                                    <asp:LinqDataSource ID="dsCostCenters" runat="server" ContextTypeName="StaffBroker.StaffBrokerDataContext"
                                        EntityTypeName="" OrderBy="CostCentreCode" Select="new (CostCentreCode,CostCentreCode + ' ' + '-' + ' ' + CostCentreName as DisplayName)"
                                        TableName="AP_StaffBroker_CostCenters" Where="PortalId == @PortalId">
                                        <WhereParameters>
                                            <asp:ControlParameter ControlID="hfPortalId" DefaultValue="-1" Name="PortalId" PropertyName="Value"
                                                Type="Int32" />
                                        </WhereParameters>
                                    </asp:LinqDataSource>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>
                                        <dnn:Label ID="lblAccountsReceivable" runat="server" ControlName="ddlAccountsReceivable"
                                            ResourceKey="lblAcctRec" />
                                    </b>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbAccountsReceivable" runat="server" Visible='false'></asp:TextBox>
                                    <asp:DropDownList ID="ddlAccountsReceivable" runat="server" Width="60px" DataSourceID="dsAccountCodes2"
                                        DataTextField="DisplayName" DataValueField="AccountCode">
                                    </asp:DropDownList>
                                    <asp:Label ID="oopsAccountsReceivable" runat="server" Text="" ForeColor="Red"></asp:Label>
                                    <asp:LinqDataSource ID="dsAccountCodes2" runat="server" ContextTypeName="StaffRmb.StaffRmbDataContext"
                                        EntityTypeName="" Select="new (AccountCode,  AccountCode + ' ' + '-' + ' ' + AccountCodeName  as DisplayName )"
                                        TableName="AP_StaffBroker_AccountCodes" OrderBy="AccountCode" Where="PortalId == @PortalId &amp;&amp; AccountCodeType == @AccountCodeType">
                                        <WhereParameters>
                                            <asp:ControlParameter ControlID="hfPortalId" DefaultValue="-1" Name="PortalId" PropertyName="Value"
                                                Type="Int32" />
                                            <asp:Parameter DefaultValue="1" Name="AccountCodeType" Type="Byte" />
                                        </WhereParameters>
                                    </asp:LinqDataSource>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>
                                        <dnn:Label ID="lblTaxAccountsReceivable" runat="server" ControlName="ddlTaxAccountsReceivable"
                                            ResourceKey="lblTaxAccRec" />
                                    </b>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbTaxAccountsReceivable" runat="server" Visible="false"></asp:TextBox>
                                    <asp:DropDownList ID="ddlTaxAccountsReceivable" runat="server" Width="60px" DataSourceID="dsAccountCodes2"
                                        DataTextField="DisplayName" DataValueField="AccountCode">
                                    </asp:DropDownList>
                                    <asp:Label ID="oopsTaxAccountsReceivable" runat="server" Text="" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>
                                        <dnn:Label ID="Label4" runat="server" ControlName="ddlAccountsPayable" ResourceKey="lblAccPay" />
                                    </b>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbAccountsPayable" runat="server" Visible="false"></asp:TextBox>
                                    <asp:DropDownList ID="ddlAccountsPayable" runat="server" Width="60px" DataSourceID="dsAccountCodes3"
                                        DataTextField="DisplayName" DataValueField="AccountCode">
                                    </asp:DropDownList>
                                    <asp:Label ID="oopsAccountsPayable" runat="server" Text="" ForeColor="Red"></asp:Label>
                                    <asp:LinqDataSource ID="dsAccountCodes3" runat="server" ContextTypeName="StaffRmb.StaffRmbDataContext"
                                        EntityTypeName="" Select="new (AccountCode,  AccountCode + ' ' + '-' + ' ' + AccountCodeName  as DisplayName )"
                                        TableName="AP_StaffBroker_AccountCodes" OrderBy="AccountCode" Where="PortalId == @PortalId &amp;&amp; AccountCodeType == @AccountCodeType">
                                        <WhereParameters>
                                            <asp:ControlParameter ControlID="hfPortalId" DefaultValue="-1" Name="PortalId" PropertyName="Value"
                                                Type="Int32" />
                                            <asp:Parameter DefaultValue="2" Name="AccountCodeType" Type="Byte" />
                                        </WhereParameters>
                                    </asp:LinqDataSource>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>
                                        <dnn:Label ID="Label7" runat="server" ControlName="ddlPayrollPayable" ResourceKey="lblPayroll" />
                                    </b>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbPayrollPayable" runat="server" Visible="false"></asp:TextBox>
                                    <asp:DropDownList ID="ddlPayrollPayable" runat="server" Width="60px" DataSourceID="dsAccountCodes3"
                                        DataTextField="DisplayName" DataValueField="AccountCode">
                                    </asp:DropDownList>
                                    <asp:Label ID="lblOopsPayroll" runat="server" Text="" ForeColor="Red"></asp:Label>

                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>
                                        <dnn:Label ID="Label25" runat="server" ControlName="ddlSalaryAccount" ResourceKey="lblSalary" />
                                    </b>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbSalaryAccount" runat="server" Visible="false"></asp:TextBox>
                                    <asp:DropDownList ID="ddlSalaryAccount" runat="server" Width="60px" DataSourceID="dsAccountCodes"
                                        DataTextField="DisplayName" DataValueField="AccountCode">
                                    </asp:DropDownList>
                                    <asp:Label ID="lblOopsSalary" runat="server" Text="" ForeColor="Red"></asp:Label>

                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>
                                        <dnn:Label ID="Label26" runat="server" ControlName="ddlBankAccount" ResourceKey="lblBankAccount" />
                                    </b>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbBankAccount" runat="server" Visible="false"></asp:TextBox>
                                    <asp:DropDownList ID="ddlBankAccount" runat="server" Width="60px" DataSourceID="dsAccountCodes2"
                                        DataTextField="DisplayName" DataValueField="AccountCode">
                                    </asp:DropDownList>
                                    <asp:Label ID="lblOopsBank" runat="server" Text="" ForeColor="Red"></asp:Label>

                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>
                                        <dnn:Label ID="Label36" runat="server" ControlName="ddlHolding" ResourceKey="lblHoldingAccount"  Text="Donation Holding Account"/>
                                    </b>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbHoldingAccount" runat="server" Visible="false"></asp:TextBox>
                                    <asp:DropDownList ID="ddlHoldingAccount" runat="server" Width="60px" DataSourceID="dsAccountCodes2"
                                        DataTextField="DisplayName" DataValueField="AccountCode">
                                    </asp:DropDownList>
                                    <asp:Label ID="lblOopsHoldingAccount" runat="server" Text="" ForeColor="Red"></asp:Label>

                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>

                        <asp:Panel ID="pnlDatapump" runat="server" Visible="True">
                            <!-- Datapump Manager -->
                            <fieldset>
                                <legend class="AgapeH5">Datapump Manager</legend>


                                <table>
                                    <tr>
                                        <td>
                                            <b>
                                                <dnn:Label ID="Label28" runat="server" ControlName="cbDatapump" Text="Autopump Enabled:" HelpText="When checked (recommended), the datapump will automatically insert your reimbursements (as unreleased batches). The datapump runs every 5 minutes (or so)" />
                                            </b>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cbDatapump" runat="server" class="iPhoneSwitch" />
                                        </td>
                                    </tr>
                                    <tr id="pnlSingle" runat="server">
                                        <td>
                                            <dnn:Label ID="Label29" runat="server" Text="Download Once:" HelpText="If the datapump is disabled, can have the datapump donwload pending transactins (just once) the next time it runs" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btnDownload" runat="server" Text="Button" Font-Size="x-small" CssClass="aButton btn" />
                                            <asp:Label ID="lblDownloading" runat="server" Visible="false" Font-Size="X-Small" Font-Italic="true" ForeColor="Gray" Text="Pending expenses will download within 5 minutes."></asp:Label>
                                        </td>
                                    </tr>
                                </table>



                            </fieldset>
                        </asp:Panel>

                        <fieldset>
                            <legend class="AgapeH5">Email Reminders (Nagap&eacute;)</legend>
                            <table>
                                <tr>
                                    <td>
                                        <b>
                                            <dnn:Label ID="Label32" runat="server" ControlName="cbNagape" Text="Email Reminders:" HelpText="Nagape is a system which will send reminder emails to approvers" />
                                        </b>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cbNagape" runat="server" class="iPhoneSwitch" />
                                    </td>


                                </tr>
                                 <tr>
                                    <td>
                                        <b>
                                            <dnn:Label ID="Label33" runat="server" ControlName="cbNagape" Text="First Reminder:" HelpText="After how long, since the reimbursement/advance was submitted, should the first reminder be sent out?" />
                                        </b>
                                    </td>
                                    <td style="white-space: nowrap;">
                                        <asp:TextBox ID="tbReminder1" runat="server" CssClass="numeric shortBox" >2</asp:TextBox> days
                                    </td>


                                </tr>
                                 <tr>
                                    <td>
                                        <b>
                                            <dnn:Label ID="Label34" runat="server" ControlName="cbNagape" Text="Second Reminder:" HelpText="After how long, since the reimbursement/advance was submitted, should the second reminder be sent out?" />
                                        </b>
                                    </td>
                                    <td style="white-space: nowrap;">
                                        <asp:TextBox ID="tbReminder2" runat="server" CssClass="numeric shortBox">4</asp:TextBox> days
                                    </td>


                                </tr>
                                 <tr>
                                    <td>
                                        <b>
                                            <dnn:Label ID="Label35" runat="server" ControlName="cbNagape" Text="Give-Up Message:" HelpText="After how long, since the reimbursement/advance was submitted, should the final 'give-up' message be sent out?" />
                                        </b>
                                    </td>
                                    <td style="white-space: nowrap;">
                                        <asp:TextBox ID="tbGiveUp" runat="server" CssClass="numeric shortBox">7</asp:TextBox> days
                                    </td>


                                </tr>
                            </table>

                        </fieldset>

                    </td>
                </tr>
            </table>
        </div>
        <div id='Tab2-tab'>
            <table style="font-size: 9pt;">
                <tr valign="top">
                    <td>
                        <b>
                            <dnn:Label ID="lblMileage" runat="server" ControlName="tbTeamLeaderLimit" ResourceKey="lblMileage" />
                        </b>
                    </td>
                    <td>
                        <table style="font-size: 9pt">
                            <tr>
                                <td>
                                    <asp:Label ID="Label22" runat="server" resourcekey="lblRate1"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbMRate1Name" runat="server" Width="80px" CssClass="pdName"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbMRate1" runat="server" Width="80px" CssClass="numeric pdValue"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label6" runat="server" resourcekey="lblRate2"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbMRate2Name" runat="server" Width="80px" CssClass="pdName"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbMRate2" runat="server" Width="80px" CssClass="numeric pdValue"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label23" runat="server" resourcekey="lblRate3"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbMRate3Name" runat="server" Width="80px" CssClass="pdName"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbMRate3" runat="server" Width="80px" CssClass="numeric pdValue"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label24" runat="server" resourcekey="lblRate4"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbMRate4Name" runat="server" Width="80px" CssClass="pdName"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbMRate4" runat="server" Width="80px" CssClass="numeric pdValue"></asp:TextBox>
                                </td>
                            </tr>

                        </table>
                    </td>
                </tr>
                <tr valign="top">
                    <td>
                        <b>
                            <dnn:Label ID="Label1" runat="server" ControlName="tbSubBreakfast" ResourceKey="lblPerDiem" />
                        </b>
                    </td>
                    <td>
                        <table style="font-size: 9pt">
                            <tr>
                                <td>
                                    <asp:Label ID="Label10" runat="server" resourcekey="lblRate1"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbPD1Name" runat="server" Width="80px" CssClass="pdName"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbPD1Value" runat="server" Width="80px" CssClass="numeric pdValue"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label11" runat="server" resourcekey="lblRate2"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbPD2Name" runat="server" Width="80px" CssClass="pdName"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbPD2Value" runat="server" Width="80px" CssClass="numeric pdValue"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label12" runat="server" resourcekey="lblRate3"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbPD3Name" runat="server" Width="80px" CssClass="pdName"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbPD3Value" runat="server" Width="80px" CssClass="numeric pdValue"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label13" runat="server" resourcekey="lblRate4"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbPD4Name" runat="server" Width="80px" CssClass="pdName"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbPD4Value" runat="server" Width="80px" CssClass="numeric pdValue"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label14" runat="server" resourcekey="lblRate5"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbPD5Name" runat="server" Width="80px" CssClass="pdName"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbPD5Value" runat="server" Width="80px" CssClass="numeric pdValue"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label15" runat="server" resourcekey="lblRate6"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbPD6Name" runat="server" Width="80px" CssClass="pdName"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbPD6Value" runat="server" Width="80px" CssClass="numeric pdValue"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr valign="top">
                    <td>
                        <b>
                            <dnn:Label ID="Label27" runat="server" ResourceKey="lblPerDiemMulti" Text="PerDiem (Multi)" />
                        </b>
                    </td>
                    <td>
                        <asp:GridView ID="gvPerDiemMulti" runat="server" AutoGenerateColumns="False" DataKeyNames="PerDiemTypeId" DataSourceID="dsPerdiemMulti" ShowFooter="True"
                            CellPadding="4" ForeColor="#333333" GridLines="None" CssClass="dnnGrid" ShowHeaderWhenEmpty="True">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:TemplateField HeaderText="Name" SortExpression="Name">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Name") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="tbNameI" runat="server"></asp:TextBox>

                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Value" SortExpression="Value">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox2" runat="server" class="numeric" Text='<%# Bind("Value", "{0:0.00}") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("Value", "{0:0.00}") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="tbAmountI" runat="server"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Currency" SortExpression="Currency">
                                    <EditItemTemplate>

                                        <asp:DropDownList ID="ddlCurrenciesI" runat="server" class="ddlCur" SelectedValue='<%# Bind("Currency") %>'>
                                            <asp:ListItem Value="ALL">Albanian Lek</asp:ListItem>
                                            <asp:ListItem Value="DZD">Algerian Dinar</asp:ListItem>
                                            <asp:ListItem Value="ARS">Argentine Peso</asp:ListItem>
                                            <asp:ListItem Value="AWG">Aruba Florin</asp:ListItem>
                                            <asp:ListItem Value="AUD">Australian Dollar</asp:ListItem>
                                            <asp:ListItem Value="BSD">Bahamian Dollar</asp:ListItem>
                                            <asp:ListItem Value="BHD">Bahraini Dinar</asp:ListItem>
                                            <asp:ListItem Value="BDT">Bangladesh Taka</asp:ListItem>
                                            <asp:ListItem Value="BBD">Barbados Dollar</asp:ListItem>
                                            <asp:ListItem Value="BYR">Belarus Ruble</asp:ListItem>
                                            <asp:ListItem Value="BZD">Belize Dollar</asp:ListItem>
                                            <asp:ListItem Value="BMD">Bermuda Dollar</asp:ListItem>
                                            <asp:ListItem Value="BTN">Bhutan Ngultrum</asp:ListItem>
                                            <asp:ListItem Value="BOB">Bolivian Boliviano</asp:ListItem>
                                            <asp:ListItem Value="BWP">Botswana Pula</asp:ListItem>
                                            <asp:ListItem Value="BRL">Brazilian Real</asp:ListItem>
                                            <asp:ListItem Value="GBP">British Pound</asp:ListItem>
                                            <asp:ListItem Value="BND">Brunei Dollar</asp:ListItem>
                                            <asp:ListItem Value="BGN">Bulgarian Lev</asp:ListItem>
                                            <asp:ListItem Value="BIF">Burundi Franc</asp:ListItem>
                                            <asp:ListItem Value="KHR">Cambodia Riel</asp:ListItem>
                                            <asp:ListItem Value="CAD">Canadian Dollar</asp:ListItem>
                                            <asp:ListItem Value="CVE">Cape Verde Escudo</asp:ListItem>
                                            <asp:ListItem Value="KYD">Cayman Islands Dollar</asp:ListItem>
                                            <asp:ListItem Value="XOF">CFA Franc (BCEAO)</asp:ListItem>
                                            <asp:ListItem Value="XAF">CFA Franc (BEAC)</asp:ListItem>
                                            <asp:ListItem Value="CLP">Chilean Peso</asp:ListItem>
                                            <asp:ListItem Value="CNY">Chinese Yuan</asp:ListItem>
                                            <asp:ListItem Value="COP">Colombian Peso</asp:ListItem>
                                            <asp:ListItem Value="KMF">Comoros Franc</asp:ListItem>
                                            <asp:ListItem Value="CRC">Costa Rica Colon</asp:ListItem>
                                            <asp:ListItem Value="HRK">Croatian Kuna</asp:ListItem>
                                            <asp:ListItem Value="CUP">Cuban Peso</asp:ListItem>
                                            <asp:ListItem Value="CZK">Czech Koruna</asp:ListItem>
                                            <asp:ListItem Value="DKK">Danish Krone</asp:ListItem>
                                            <asp:ListItem Value="DJF">Dijibouti Franc</asp:ListItem>
                                            <asp:ListItem Value="DOP">Dominican Peso</asp:ListItem>
                                            <asp:ListItem Value="XCD">East Caribbean Dollar</asp:ListItem>
                                            <asp:ListItem Value="ECS">Ecuador Sucre</asp:ListItem>
                                            <asp:ListItem Value="EGP">Egyptian Pound</asp:ListItem>
                                            <asp:ListItem Value="SVC">El Salvador Colon</asp:ListItem>
                                            <asp:ListItem Value="ERN">Eritrea Nakfa</asp:ListItem>
                                            <asp:ListItem Value="EEK">Estonian Kroon</asp:ListItem>
                                            <asp:ListItem Value="ETB">Ethiopian Birr</asp:ListItem>
                                            <asp:ListItem Value="EUR">Euro</asp:ListItem>
                                            <asp:ListItem Value="FKP">Falkland Islands Pound</asp:ListItem>
                                            <asp:ListItem Value="FJD">Fiji Dollar</asp:ListItem>
                                            <asp:ListItem Value="GMD">Gambian Dalasi</asp:ListItem>
                                            <asp:ListItem Value="GHC">Ghanian Cedi</asp:ListItem>
                                            <asp:ListItem Value="GIP">Gibraltar Pound</asp:ListItem>
                                            <asp:ListItem Value="GTQ">Guatemala Quetzal</asp:ListItem>
                                            <asp:ListItem Value="GNF">Guinea Franc</asp:ListItem>
                                            <asp:ListItem Value="GYD">Guyana Dollar</asp:ListItem>
                                            <asp:ListItem Value="HTG">Haiti Gourde</asp:ListItem>
                                            <asp:ListItem Value="HNL">Honduras Lempira</asp:ListItem>
                                            <asp:ListItem Value="HKD">Hong Kong Dollar</asp:ListItem>
                                            <asp:ListItem Value="HUF">Hungarian Forint</asp:ListItem>
                                            <asp:ListItem Value="ISK">Iceland Krona</asp:ListItem>
                                            <asp:ListItem Value="INR">Indian Rupee</asp:ListItem>
                                            <asp:ListItem Value="IDR">Indonesian Rupiah</asp:ListItem>
                                            <asp:ListItem Value="IRR">Iran Rial</asp:ListItem>
                                            <asp:ListItem Value="IQD">Iraqi Dinar</asp:ListItem>
                                            <asp:ListItem Value="ILS">Israeli Shekel</asp:ListItem>
                                            <asp:ListItem Value="JMD">Jamaican Dollar</asp:ListItem>
                                            <asp:ListItem Value="JPY">Japanese Yen</asp:ListItem>
                                            <asp:ListItem Value="JOD">Jordanian Dinar</asp:ListItem>
                                            <asp:ListItem Value="KZT">Kazakhstan Tenge</asp:ListItem>
                                            <asp:ListItem Value="KES">Kenyan Shilling</asp:ListItem>
                                            <asp:ListItem Value="KWD">Kuwaiti Dinar</asp:ListItem>
                                            <asp:ListItem Value="LAK">Lao Kip</asp:ListItem>
                                            <asp:ListItem Value="LVL">Latvian Lat</asp:ListItem>
                                            <asp:ListItem Value="LBP">Lebanese Pound</asp:ListItem>
                                            <asp:ListItem Value="LSL">Lesotho Loti</asp:ListItem>
                                            <asp:ListItem Value="LRD">Liberian Dollar</asp:ListItem>
                                            <asp:ListItem Value="LYD">Libyan Dinar</asp:ListItem>
                                            <asp:ListItem Value="LTL">Lithuanian Lita</asp:ListItem>
                                            <asp:ListItem Value="MOP">Macau Pataca</asp:ListItem>
                                            <asp:ListItem Value="MKD">Macedonian Denar</asp:ListItem>
                                            <asp:ListItem Value="MWK">Malawi Kwacha</asp:ListItem>
                                            <asp:ListItem Value="MYR">Malaysian Ringgit</asp:ListItem>
                                            <asp:ListItem Value="MVR">Maldives Rufiyaa</asp:ListItem>
                                            <asp:ListItem Value="MTL">Maltese Lira</asp:ListItem>
                                            <asp:ListItem Value="MRO">Mauritania Ougulya</asp:ListItem>
                                            <asp:ListItem Value="MUR">Mauritius Rupee</asp:ListItem>
                                            <asp:ListItem Value="MXN">Mexican Peso</asp:ListItem>
                                            <asp:ListItem Value="MDL">Moldovan Leu</asp:ListItem>
                                            <asp:ListItem Value="MNT">Mongolian Tugrik</asp:ListItem>
                                            <asp:ListItem Value="MAD">Moroccan Dirham</asp:ListItem>
                                            <asp:ListItem Value="MMK">Myanmar Kyat</asp:ListItem>
                                            <asp:ListItem Value="NAD">Namibian Dollar</asp:ListItem>
                                            <asp:ListItem Value="NPR">Nepalese Rupee</asp:ListItem>
                                            <asp:ListItem Value="ANG">Neth Antilles Guilder</asp:ListItem>
                                            <asp:ListItem Value="NZD">New Zealand Dollar</asp:ListItem>
                                            <asp:ListItem Value="NIO">Nicaragua Cordoba</asp:ListItem>
                                            <asp:ListItem Value="NGN">Nigerian Naira</asp:ListItem>
                                            <asp:ListItem Value="KPW">North Korean Won</asp:ListItem>
                                            <asp:ListItem Value="NOK">Norwegian Krone</asp:ListItem>
                                            <asp:ListItem Value="OMR">Omani Rial</asp:ListItem>
                                            <asp:ListItem Value="PKR">Pakistani Rupee</asp:ListItem>
                                            <asp:ListItem Value="PAB">Panama Balboa</asp:ListItem>
                                            <asp:ListItem Value="PGK">Papua New Guinea Kina</asp:ListItem>
                                            <asp:ListItem Value="PYG">Paraguayan Guarani</asp:ListItem>
                                            <asp:ListItem Value="PEN">Peruvian Nuevo Sol</asp:ListItem>
                                            <asp:ListItem Value="PHP">Philippine Peso</asp:ListItem>
                                            <asp:ListItem Value="PLN">Polish Zloty</asp:ListItem>
                                            <asp:ListItem Value="QAR">Qatar Rial</asp:ListItem>
                                            <asp:ListItem Value="RON">Romanian New Leu</asp:ListItem>
                                            <asp:ListItem Value="RUB">Russian Rouble</asp:ListItem>
                                            <asp:ListItem Value="RWF">Rwanda Franc</asp:ListItem>
                                            <asp:ListItem Value="WST">Samoa Tala</asp:ListItem>
                                            <asp:ListItem Value="STD">Sao Tome Dobra</asp:ListItem>
                                            <asp:ListItem Value="SAR">Saudi Arabian Riyal</asp:ListItem>
                                            <asp:ListItem Value="SCR">Seychelles Rupee</asp:ListItem>
                                            <asp:ListItem Value="SLL">Sierra Leone Leone</asp:ListItem>
                                            <asp:ListItem Value="SGD">Singapore Dollar</asp:ListItem>
                                            <asp:ListItem Value="SKK">Slovak Koruna</asp:ListItem>
                                            <asp:ListItem Value="SIT">Slovenian Tolar</asp:ListItem>
                                            <asp:ListItem Value="SBD">Solomon Islands Dollar</asp:ListItem>
                                            <asp:ListItem Value="SOS">Somali Shilling</asp:ListItem>
                                            <asp:ListItem Value="ZAR">South African Rand</asp:ListItem>
                                            <asp:ListItem Value="KRW">South Korean Won</asp:ListItem>
                                            <asp:ListItem Value="LKR">Sri Lanka Rupee</asp:ListItem>
                                            <asp:ListItem Value="SHP">St Helena Pound</asp:ListItem>
                                            <asp:ListItem Value="SDG">Sudanese Pound</asp:ListItem>
                                            <asp:ListItem Value="SZL">Swaziland Lilageni</asp:ListItem>
                                            <asp:ListItem Value="SEK">Swedish Krona</asp:ListItem>
                                            <asp:ListItem Value="CHF">Swiss Franc</asp:ListItem>
                                            <asp:ListItem Value="SYP">Syrian Pound</asp:ListItem>
                                            <asp:ListItem Value="TWD">Taiwan Dollar</asp:ListItem>
                                            <asp:ListItem Value="TZS">Tanzanian Shilling</asp:ListItem>
                                            <asp:ListItem Value="THB">Thai Baht</asp:ListItem>
                                            <asp:ListItem Value="TOP">Tonga Pa'ang</asp:ListItem>
                                            <asp:ListItem Value="TTD">Trinidad Tobago Dollar</asp:ListItem>
                                            <asp:ListItem Value="TND">Tunisian Dinar</asp:ListItem>
                                            <asp:ListItem Value="TRY">Turkish Lira</asp:ListItem>
                                            <asp:ListItem Value="AED">UAE Dirham</asp:ListItem>
                                            <asp:ListItem Value="UGX">Ugandan Shilling</asp:ListItem>
                                            <asp:ListItem Value="UAH">Ukraine Hryvnia</asp:ListItem>
                                            <asp:ListItem Value="USD">United States Dollar</asp:ListItem>
                                            <asp:ListItem Value="UYU">Uruguayan New Peso</asp:ListItem>
                                            <asp:ListItem Value="VUV">Vanuatu Vatu</asp:ListItem>
                                            <asp:ListItem Value="VEF">Venezuelan Bolivar Fuerte</asp:ListItem>
                                            <asp:ListItem Value="VND">Vietnam Dong</asp:ListItem>
                                            <asp:ListItem Value="YER">Yemen Riyal</asp:ListItem>
                                            <asp:ListItem Value="ZMK">Zambian Kwacha</asp:ListItem>
                                            <asp:ListItem Value="ZWD">Zimbabwe Dollar</asp:ListItem>

                                        </asp:DropDownList>

                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("Currency") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddlCurrenciesI" runat="server" class="ddlCur">
                                            <asp:ListItem Value="ALL">Albanian Lek</asp:ListItem>
                                            <asp:ListItem Value="DZD">Algerian Dinar</asp:ListItem>
                                            <asp:ListItem Value="ARS">Argentine Peso</asp:ListItem>
                                            <asp:ListItem Value="AWG">Aruba Florin</asp:ListItem>
                                            <asp:ListItem Value="AUD">Australian Dollar</asp:ListItem>
                                            <asp:ListItem Value="BSD">Bahamian Dollar</asp:ListItem>
                                            <asp:ListItem Value="BHD">Bahraini Dinar</asp:ListItem>
                                            <asp:ListItem Value="BDT">Bangladesh Taka</asp:ListItem>
                                            <asp:ListItem Value="BBD">Barbados Dollar</asp:ListItem>
                                            <asp:ListItem Value="BYR">Belarus Ruble</asp:ListItem>
                                            <asp:ListItem Value="BZD">Belize Dollar</asp:ListItem>
                                            <asp:ListItem Value="BMD">Bermuda Dollar</asp:ListItem>
                                            <asp:ListItem Value="BTN">Bhutan Ngultrum</asp:ListItem>
                                            <asp:ListItem Value="BOB">Bolivian Boliviano</asp:ListItem>
                                            <asp:ListItem Value="BWP">Botswana Pula</asp:ListItem>
                                            <asp:ListItem Value="BRL">Brazilian Real</asp:ListItem>
                                            <asp:ListItem Value="GBP">British Pound</asp:ListItem>
                                            <asp:ListItem Value="BND">Brunei Dollar</asp:ListItem>
                                            <asp:ListItem Value="BGN">Bulgarian Lev</asp:ListItem>
                                            <asp:ListItem Value="BIF">Burundi Franc</asp:ListItem>
                                            <asp:ListItem Value="KHR">Cambodia Riel</asp:ListItem>
                                            <asp:ListItem Value="CAD">Canadian Dollar</asp:ListItem>
                                            <asp:ListItem Value="CVE">Cape Verde Escudo</asp:ListItem>
                                            <asp:ListItem Value="KYD">Cayman Islands Dollar</asp:ListItem>
                                            <asp:ListItem Value="XOF">CFA Franc (BCEAO)</asp:ListItem>
                                            <asp:ListItem Value="XAF">CFA Franc (BEAC)</asp:ListItem>
                                            <asp:ListItem Value="CLP">Chilean Peso</asp:ListItem>
                                            <asp:ListItem Value="CNY">Chinese Yuan</asp:ListItem>
                                            <asp:ListItem Value="COP">Colombian Peso</asp:ListItem>
                                            <asp:ListItem Value="KMF">Comoros Franc</asp:ListItem>
                                            <asp:ListItem Value="CRC">Costa Rica Colon</asp:ListItem>
                                            <asp:ListItem Value="HRK">Croatian Kuna</asp:ListItem>
                                            <asp:ListItem Value="CUP">Cuban Peso</asp:ListItem>
                                            <asp:ListItem Value="CZK">Czech Koruna</asp:ListItem>
                                            <asp:ListItem Value="DKK">Danish Krone</asp:ListItem>
                                            <asp:ListItem Value="DJF">Dijibouti Franc</asp:ListItem>
                                            <asp:ListItem Value="DOP">Dominican Peso</asp:ListItem>
                                            <asp:ListItem Value="XCD">East Caribbean Dollar</asp:ListItem>
                                            <asp:ListItem Value="ECS">Ecuador Sucre</asp:ListItem>
                                            <asp:ListItem Value="EGP">Egyptian Pound</asp:ListItem>
                                            <asp:ListItem Value="SVC">El Salvador Colon</asp:ListItem>
                                            <asp:ListItem Value="ERN">Eritrea Nakfa</asp:ListItem>
                                            <asp:ListItem Value="EEK">Estonian Kroon</asp:ListItem>
                                            <asp:ListItem Value="ETB">Ethiopian Birr</asp:ListItem>
                                            <asp:ListItem Value="EUR">Euro</asp:ListItem>
                                            <asp:ListItem Value="FKP">Falkland Islands Pound</asp:ListItem>
                                            <asp:ListItem Value="FJD">Fiji Dollar</asp:ListItem>
                                            <asp:ListItem Value="GMD">Gambian Dalasi</asp:ListItem>
                                            <asp:ListItem Value="GHC">Ghanian Cedi</asp:ListItem>
                                            <asp:ListItem Value="GIP">Gibraltar Pound</asp:ListItem>
                                            <asp:ListItem Value="GTQ">Guatemala Quetzal</asp:ListItem>
                                            <asp:ListItem Value="GNF">Guinea Franc</asp:ListItem>
                                            <asp:ListItem Value="GYD">Guyana Dollar</asp:ListItem>
                                            <asp:ListItem Value="HTG">Haiti Gourde</asp:ListItem>
                                            <asp:ListItem Value="HNL">Honduras Lempira</asp:ListItem>
                                            <asp:ListItem Value="HKD">Hong Kong Dollar</asp:ListItem>
                                            <asp:ListItem Value="HUF">Hungarian Forint</asp:ListItem>
                                            <asp:ListItem Value="ISK">Iceland Krona</asp:ListItem>
                                            <asp:ListItem Value="INR">Indian Rupee</asp:ListItem>
                                            <asp:ListItem Value="IDR">Indonesian Rupiah</asp:ListItem>
                                            <asp:ListItem Value="IRR">Iran Rial</asp:ListItem>
                                            <asp:ListItem Value="IQD">Iraqi Dinar</asp:ListItem>
                                            <asp:ListItem Value="ILS">Israeli Shekel</asp:ListItem>
                                            <asp:ListItem Value="JMD">Jamaican Dollar</asp:ListItem>
                                            <asp:ListItem Value="JPY">Japanese Yen</asp:ListItem>
                                            <asp:ListItem Value="JOD">Jordanian Dinar</asp:ListItem>
                                            <asp:ListItem Value="KZT">Kazakhstan Tenge</asp:ListItem>
                                            <asp:ListItem Value="KES">Kenyan Shilling</asp:ListItem>
                                            <asp:ListItem Value="KWD">Kuwaiti Dinar</asp:ListItem>
                                            <asp:ListItem Value="LAK">Lao Kip</asp:ListItem>
                                            <asp:ListItem Value="LVL">Latvian Lat</asp:ListItem>
                                            <asp:ListItem Value="LBP">Lebanese Pound</asp:ListItem>
                                            <asp:ListItem Value="LSL">Lesotho Loti</asp:ListItem>
                                            <asp:ListItem Value="LRD">Liberian Dollar</asp:ListItem>
                                            <asp:ListItem Value="LYD">Libyan Dinar</asp:ListItem>
                                            <asp:ListItem Value="LTL">Lithuanian Lita</asp:ListItem>
                                            <asp:ListItem Value="MOP">Macau Pataca</asp:ListItem>
                                            <asp:ListItem Value="MKD">Macedonian Denar</asp:ListItem>
                                            <asp:ListItem Value="MWK">Malawi Kwacha</asp:ListItem>
                                            <asp:ListItem Value="MYR">Malaysian Ringgit</asp:ListItem>
                                            <asp:ListItem Value="MVR">Maldives Rufiyaa</asp:ListItem>
                                            <asp:ListItem Value="MTL">Maltese Lira</asp:ListItem>
                                            <asp:ListItem Value="MRO">Mauritania Ougulya</asp:ListItem>
                                            <asp:ListItem Value="MUR">Mauritius Rupee</asp:ListItem>
                                            <asp:ListItem Value="MXN">Mexican Peso</asp:ListItem>
                                            <asp:ListItem Value="MDL">Moldovan Leu</asp:ListItem>
                                            <asp:ListItem Value="MNT">Mongolian Tugrik</asp:ListItem>
                                            <asp:ListItem Value="MAD">Moroccan Dirham</asp:ListItem>
                                            <asp:ListItem Value="MMK">Myanmar Kyat</asp:ListItem>
                                            <asp:ListItem Value="NAD">Namibian Dollar</asp:ListItem>
                                            <asp:ListItem Value="NPR">Nepalese Rupee</asp:ListItem>
                                            <asp:ListItem Value="ANG">Neth Antilles Guilder</asp:ListItem>
                                            <asp:ListItem Value="NZD">New Zealand Dollar</asp:ListItem>
                                            <asp:ListItem Value="NIO">Nicaragua Cordoba</asp:ListItem>
                                            <asp:ListItem Value="NGN">Nigerian Naira</asp:ListItem>
                                            <asp:ListItem Value="KPW">North Korean Won</asp:ListItem>
                                            <asp:ListItem Value="NOK">Norwegian Krone</asp:ListItem>
                                            <asp:ListItem Value="OMR">Omani Rial</asp:ListItem>
                                            <asp:ListItem Value="PKR">Pakistani Rupee</asp:ListItem>
                                            <asp:ListItem Value="PAB">Panama Balboa</asp:ListItem>
                                            <asp:ListItem Value="PGK">Papua New Guinea Kina</asp:ListItem>
                                            <asp:ListItem Value="PYG">Paraguayan Guarani</asp:ListItem>
                                            <asp:ListItem Value="PEN">Peruvian Nuevo Sol</asp:ListItem>
                                            <asp:ListItem Value="PHP">Philippine Peso</asp:ListItem>
                                            <asp:ListItem Value="PLN">Polish Zloty</asp:ListItem>
                                            <asp:ListItem Value="QAR">Qatar Rial</asp:ListItem>
                                            <asp:ListItem Value="RON">Romanian New Leu</asp:ListItem>
                                            <asp:ListItem Value="RUB">Russian Rouble</asp:ListItem>
                                            <asp:ListItem Value="RWF">Rwanda Franc</asp:ListItem>
                                            <asp:ListItem Value="WST">Samoa Tala</asp:ListItem>
                                            <asp:ListItem Value="STD">Sao Tome Dobra</asp:ListItem>
                                            <asp:ListItem Value="SAR">Saudi Arabian Riyal</asp:ListItem>
                                            <asp:ListItem Value="SCR">Seychelles Rupee</asp:ListItem>
                                            <asp:ListItem Value="SLL">Sierra Leone Leone</asp:ListItem>
                                            <asp:ListItem Value="SGD">Singapore Dollar</asp:ListItem>
                                            <asp:ListItem Value="SKK">Slovak Koruna</asp:ListItem>
                                            <asp:ListItem Value="SIT">Slovenian Tolar</asp:ListItem>
                                            <asp:ListItem Value="SBD">Solomon Islands Dollar</asp:ListItem>
                                            <asp:ListItem Value="SOS">Somali Shilling</asp:ListItem>
                                            <asp:ListItem Value="ZAR">South African Rand</asp:ListItem>
                                            <asp:ListItem Value="KRW">South Korean Won</asp:ListItem>
                                            <asp:ListItem Value="LKR">Sri Lanka Rupee</asp:ListItem>
                                            <asp:ListItem Value="SHP">St Helena Pound</asp:ListItem>
                                            <asp:ListItem Value="SDG">Sudanese Pound</asp:ListItem>
                                            <asp:ListItem Value="SZL">Swaziland Lilageni</asp:ListItem>
                                            <asp:ListItem Value="SEK">Swedish Krona</asp:ListItem>
                                            <asp:ListItem Value="CHF">Swiss Franc</asp:ListItem>
                                            <asp:ListItem Value="SYP">Syrian Pound</asp:ListItem>
                                            <asp:ListItem Value="TWD">Taiwan Dollar</asp:ListItem>
                                            <asp:ListItem Value="TZS">Tanzanian Shilling</asp:ListItem>
                                            <asp:ListItem Value="THB">Thai Baht</asp:ListItem>
                                            <asp:ListItem Value="TOP">Tonga Pa'ang</asp:ListItem>
                                            <asp:ListItem Value="TTD">Trinidad Tobago Dollar</asp:ListItem>
                                            <asp:ListItem Value="TND">Tunisian Dinar</asp:ListItem>
                                            <asp:ListItem Value="TRY">Turkish Lira</asp:ListItem>
                                            <asp:ListItem Value="AED">UAE Dirham</asp:ListItem>
                                            <asp:ListItem Value="UGX">Ugandan Shilling</asp:ListItem>
                                            <asp:ListItem Value="UAH">Ukraine Hryvnia</asp:ListItem>
                                            <asp:ListItem Value="USD">United States Dollar</asp:ListItem>
                                            <asp:ListItem Value="UYU">Uruguayan New Peso</asp:ListItem>
                                            <asp:ListItem Value="VUV">Vanuatu Vatu</asp:ListItem>
                                            <asp:ListItem Value="VEF">Venezuelan Bolivar Fuerte</asp:ListItem>
                                            <asp:ListItem Value="VND">Vietnam Dong</asp:ListItem>
                                            <asp:ListItem Value="YER">Yemen Riyal</asp:ListItem>
                                            <asp:ListItem Value="ZMK">Zambian Kwacha</asp:ListItem>
                                            <asp:ListItem Value="ZWD">Zimbabwe Dollar</asp:ListItem>

                                        </asp:DropDownList>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False">
                                    <EditItemTemplate>
                                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update" Text="Update"></asp:LinkButton>
                                        &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel"></asp:LinkButton>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Edit" Text="Edit"></asp:LinkButton>
                                        &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete"></asp:LinkButton>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="myInsert" Text="Insert" ForeColor="white"></asp:LinkButton>

                                    </FooterTemplate>



                                </asp:TemplateField>

                            </Columns>
                            <EmptyDataTemplate>
                                <table>


                                    <tr>
                                        <td>
                                            <asp:TextBox ID="tbNameE" runat="server" Width="80px" MaxLength="50"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbValueE" runat="server" class="numeric" Width="60"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlCurrenciesE" runat="server" class="ddlCur">
                                                <asp:ListItem Value="ALL">Albanian Lek</asp:ListItem>
                                                <asp:ListItem Value="DZD">Algerian Dinar</asp:ListItem>
                                                <asp:ListItem Value="ARS">Argentine Peso</asp:ListItem>
                                                <asp:ListItem Value="AWG">Aruba Florin</asp:ListItem>
                                                <asp:ListItem Value="AUD">Australian Dollar</asp:ListItem>
                                                <asp:ListItem Value="BSD">Bahamian Dollar</asp:ListItem>
                                                <asp:ListItem Value="BHD">Bahraini Dinar</asp:ListItem>
                                                <asp:ListItem Value="BDT">Bangladesh Taka</asp:ListItem>
                                                <asp:ListItem Value="BBD">Barbados Dollar</asp:ListItem>
                                                <asp:ListItem Value="BYR">Belarus Ruble</asp:ListItem>
                                                <asp:ListItem Value="BZD">Belize Dollar</asp:ListItem>
                                                <asp:ListItem Value="BMD">Bermuda Dollar</asp:ListItem>
                                                <asp:ListItem Value="BTN">Bhutan Ngultrum</asp:ListItem>
                                                <asp:ListItem Value="BOB">Bolivian Boliviano</asp:ListItem>
                                                <asp:ListItem Value="BWP">Botswana Pula</asp:ListItem>
                                                <asp:ListItem Value="BRL">Brazilian Real</asp:ListItem>
                                                <asp:ListItem Value="GBP">British Pound</asp:ListItem>
                                                <asp:ListItem Value="BND">Brunei Dollar</asp:ListItem>
                                                <asp:ListItem Value="BGN">Bulgarian Lev</asp:ListItem>
                                                <asp:ListItem Value="BIF">Burundi Franc</asp:ListItem>
                                                <asp:ListItem Value="KHR">Cambodia Riel</asp:ListItem>
                                                <asp:ListItem Value="CAD">Canadian Dollar</asp:ListItem>
                                                <asp:ListItem Value="CVE">Cape Verde Escudo</asp:ListItem>
                                                <asp:ListItem Value="KYD">Cayman Islands Dollar</asp:ListItem>
                                                <asp:ListItem Value="XOF">CFA Franc (BCEAO)</asp:ListItem>
                                                <asp:ListItem Value="XAF">CFA Franc (BEAC)</asp:ListItem>
                                                <asp:ListItem Value="CLP">Chilean Peso</asp:ListItem>
                                                <asp:ListItem Value="CNY">Chinese Yuan</asp:ListItem>
                                                <asp:ListItem Value="COP">Colombian Peso</asp:ListItem>
                                                <asp:ListItem Value="KMF">Comoros Franc</asp:ListItem>
                                                <asp:ListItem Value="CRC">Costa Rica Colon</asp:ListItem>
                                                <asp:ListItem Value="HRK">Croatian Kuna</asp:ListItem>
                                                <asp:ListItem Value="CUP">Cuban Peso</asp:ListItem>
                                                <asp:ListItem Value="CZK">Czech Koruna</asp:ListItem>
                                                <asp:ListItem Value="DKK">Danish Krone</asp:ListItem>
                                                <asp:ListItem Value="DJF">Dijibouti Franc</asp:ListItem>
                                                <asp:ListItem Value="DOP">Dominican Peso</asp:ListItem>
                                                <asp:ListItem Value="XCD">East Caribbean Dollar</asp:ListItem>
                                                <asp:ListItem Value="ECS">Ecuador Sucre</asp:ListItem>
                                                <asp:ListItem Value="EGP">Egyptian Pound</asp:ListItem>
                                                <asp:ListItem Value="SVC">El Salvador Colon</asp:ListItem>
                                                <asp:ListItem Value="ERN">Eritrea Nakfa</asp:ListItem>
                                                <asp:ListItem Value="EEK">Estonian Kroon</asp:ListItem>
                                                <asp:ListItem Value="ETB">Ethiopian Birr</asp:ListItem>
                                                <asp:ListItem Value="EUR">Euro</asp:ListItem>
                                                <asp:ListItem Value="FKP">Falkland Islands Pound</asp:ListItem>
                                                <asp:ListItem Value="FJD">Fiji Dollar</asp:ListItem>
                                                <asp:ListItem Value="GMD">Gambian Dalasi</asp:ListItem>
                                                <asp:ListItem Value="GHC">Ghanian Cedi</asp:ListItem>
                                                <asp:ListItem Value="GIP">Gibraltar Pound</asp:ListItem>
                                                <asp:ListItem Value="GTQ">Guatemala Quetzal</asp:ListItem>
                                                <asp:ListItem Value="GNF">Guinea Franc</asp:ListItem>
                                                <asp:ListItem Value="GYD">Guyana Dollar</asp:ListItem>
                                                <asp:ListItem Value="HTG">Haiti Gourde</asp:ListItem>
                                                <asp:ListItem Value="HNL">Honduras Lempira</asp:ListItem>
                                                <asp:ListItem Value="HKD">Hong Kong Dollar</asp:ListItem>
                                                <asp:ListItem Value="HUF">Hungarian Forint</asp:ListItem>
                                                <asp:ListItem Value="ISK">Iceland Krona</asp:ListItem>
                                                <asp:ListItem Value="INR">Indian Rupee</asp:ListItem>
                                                <asp:ListItem Value="IDR">Indonesian Rupiah</asp:ListItem>
                                                <asp:ListItem Value="IRR">Iran Rial</asp:ListItem>
                                                <asp:ListItem Value="IQD">Iraqi Dinar</asp:ListItem>
                                                <asp:ListItem Value="ILS">Israeli Shekel</asp:ListItem>
                                                <asp:ListItem Value="JMD">Jamaican Dollar</asp:ListItem>
                                                <asp:ListItem Value="JPY">Japanese Yen</asp:ListItem>
                                                <asp:ListItem Value="JOD">Jordanian Dinar</asp:ListItem>
                                                <asp:ListItem Value="KZT">Kazakhstan Tenge</asp:ListItem>
                                                <asp:ListItem Value="KES">Kenyan Shilling</asp:ListItem>
                                                <asp:ListItem Value="KWD">Kuwaiti Dinar</asp:ListItem>
                                                <asp:ListItem Value="LAK">Lao Kip</asp:ListItem>
                                                <asp:ListItem Value="LVL">Latvian Lat</asp:ListItem>
                                                <asp:ListItem Value="LBP">Lebanese Pound</asp:ListItem>
                                                <asp:ListItem Value="LSL">Lesotho Loti</asp:ListItem>
                                                <asp:ListItem Value="LRD">Liberian Dollar</asp:ListItem>
                                                <asp:ListItem Value="LYD">Libyan Dinar</asp:ListItem>
                                                <asp:ListItem Value="LTL">Lithuanian Lita</asp:ListItem>
                                                <asp:ListItem Value="MOP">Macau Pataca</asp:ListItem>
                                                <asp:ListItem Value="MKD">Macedonian Denar</asp:ListItem>
                                                <asp:ListItem Value="MWK">Malawi Kwacha</asp:ListItem>
                                                <asp:ListItem Value="MYR">Malaysian Ringgit</asp:ListItem>
                                                <asp:ListItem Value="MVR">Maldives Rufiyaa</asp:ListItem>
                                                <asp:ListItem Value="MTL">Maltese Lira</asp:ListItem>
                                                <asp:ListItem Value="MRO">Mauritania Ougulya</asp:ListItem>
                                                <asp:ListItem Value="MUR">Mauritius Rupee</asp:ListItem>
                                                <asp:ListItem Value="MXN">Mexican Peso</asp:ListItem>
                                                <asp:ListItem Value="MDL">Moldovan Leu</asp:ListItem>
                                                <asp:ListItem Value="MNT">Mongolian Tugrik</asp:ListItem>
                                                <asp:ListItem Value="MAD">Moroccan Dirham</asp:ListItem>
                                                <asp:ListItem Value="MMK">Myanmar Kyat</asp:ListItem>
                                                <asp:ListItem Value="NAD">Namibian Dollar</asp:ListItem>
                                                <asp:ListItem Value="NPR">Nepalese Rupee</asp:ListItem>
                                                <asp:ListItem Value="ANG">Neth Antilles Guilder</asp:ListItem>
                                                <asp:ListItem Value="NZD">New Zealand Dollar</asp:ListItem>
                                                <asp:ListItem Value="NIO">Nicaragua Cordoba</asp:ListItem>
                                                <asp:ListItem Value="NGN">Nigerian Naira</asp:ListItem>
                                                <asp:ListItem Value="KPW">North Korean Won</asp:ListItem>
                                                <asp:ListItem Value="NOK">Norwegian Krone</asp:ListItem>
                                                <asp:ListItem Value="OMR">Omani Rial</asp:ListItem>
                                                <asp:ListItem Value="PKR">Pakistani Rupee</asp:ListItem>
                                                <asp:ListItem Value="PAB">Panama Balboa</asp:ListItem>
                                                <asp:ListItem Value="PGK">Papua New Guinea Kina</asp:ListItem>
                                                <asp:ListItem Value="PYG">Paraguayan Guarani</asp:ListItem>
                                                <asp:ListItem Value="PEN">Peruvian Nuevo Sol</asp:ListItem>
                                                <asp:ListItem Value="PHP">Philippine Peso</asp:ListItem>
                                                <asp:ListItem Value="PLN">Polish Zloty</asp:ListItem>
                                                <asp:ListItem Value="QAR">Qatar Rial</asp:ListItem>
                                                <asp:ListItem Value="RON">Romanian New Leu</asp:ListItem>
                                                <asp:ListItem Value="RUB">Russian Rouble</asp:ListItem>
                                                <asp:ListItem Value="RWF">Rwanda Franc</asp:ListItem>
                                                <asp:ListItem Value="WST">Samoa Tala</asp:ListItem>
                                                <asp:ListItem Value="STD">Sao Tome Dobra</asp:ListItem>
                                                <asp:ListItem Value="SAR">Saudi Arabian Riyal</asp:ListItem>
                                                <asp:ListItem Value="SCR">Seychelles Rupee</asp:ListItem>
                                                <asp:ListItem Value="SLL">Sierra Leone Leone</asp:ListItem>
                                                <asp:ListItem Value="SGD">Singapore Dollar</asp:ListItem>
                                                <asp:ListItem Value="SKK">Slovak Koruna</asp:ListItem>
                                                <asp:ListItem Value="SIT">Slovenian Tolar</asp:ListItem>
                                                <asp:ListItem Value="SBD">Solomon Islands Dollar</asp:ListItem>
                                                <asp:ListItem Value="SOS">Somali Shilling</asp:ListItem>
                                                <asp:ListItem Value="ZAR">South African Rand</asp:ListItem>
                                                <asp:ListItem Value="KRW">South Korean Won</asp:ListItem>
                                                <asp:ListItem Value="LKR">Sri Lanka Rupee</asp:ListItem>
                                                <asp:ListItem Value="SHP">St Helena Pound</asp:ListItem>
                                                <asp:ListItem Value="SDG">Sudanese Pound</asp:ListItem>
                                                <asp:ListItem Value="SZL">Swaziland Lilageni</asp:ListItem>
                                                <asp:ListItem Value="SEK">Swedish Krona</asp:ListItem>
                                                <asp:ListItem Value="CHF">Swiss Franc</asp:ListItem>
                                                <asp:ListItem Value="SYP">Syrian Pound</asp:ListItem>
                                                <asp:ListItem Value="TWD">Taiwan Dollar</asp:ListItem>
                                                <asp:ListItem Value="TZS">Tanzanian Shilling</asp:ListItem>
                                                <asp:ListItem Value="THB">Thai Baht</asp:ListItem>
                                                <asp:ListItem Value="TOP">Tonga Pa'ang</asp:ListItem>
                                                <asp:ListItem Value="TTD">Trinidad Tobago Dollar</asp:ListItem>
                                                <asp:ListItem Value="TND">Tunisian Dinar</asp:ListItem>
                                                <asp:ListItem Value="TRY">Turkish Lira</asp:ListItem>
                                                <asp:ListItem Value="AED">UAE Dirham</asp:ListItem>
                                                <asp:ListItem Value="UGX">Ugandan Shilling</asp:ListItem>
                                                <asp:ListItem Value="UAH">Ukraine Hryvnia</asp:ListItem>
                                                <asp:ListItem Value="USD">United States Dollar</asp:ListItem>
                                                <asp:ListItem Value="UYU">Uruguayan New Peso</asp:ListItem>
                                                <asp:ListItem Value="VUV">Vanuatu Vatu</asp:ListItem>
                                                <asp:ListItem Value="VEF">Venezuelan Bolivar Fuerte</asp:ListItem>
                                                <asp:ListItem Value="VND">Vietnam Dong</asp:ListItem>
                                                <asp:ListItem Value="YER">Yemen Riyal</asp:ListItem>
                                                <asp:ListItem Value="ZMK">Zambian Kwacha</asp:ListItem>
                                                <asp:ListItem Value="ZWD">Zimbabwe Dollar</asp:ListItem>

                                            </asp:DropDownList>

                                        </td>
                                        <td>
                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="myEInsert" Text="Insert" ForeColor="white"></asp:LinkButton>

                                        </td>
                                    </tr>
                                </table>

                            </EmptyDataTemplate>
                            <FooterStyle CssClass="ui-widget-header dnnGridFooter" />
                            <HeaderStyle CssClass="ui-widget-header dnnGridHeader" />

                            <EmptyDataRowStyle CssClass="ui-widget-header dnnGridHeader" />
                            <PagerStyle CssClass="dnnGridPager" />
                            <RowStyle CssClass="dnnGridItem" />
                            <SelectedRowStyle CssClass="dnnFormError" />
                            <SortedAscendingCellStyle BackColor="#FDF5AC" />
                            <SortedAscendingHeaderStyle BackColor="#4D0000" />
                            <SortedDescendingCellStyle BackColor="#FCF6C0" />
                            <SortedDescendingHeaderStyle BackColor="#820000" />
                        </asp:GridView>
                        <asp:LinqDataSource ID="dsPerdiemMulti" runat="server" ContextTypeName="StaffRmb.StaffRmbDataContext" EnableDelete="True" EnableInsert="True" EnableUpdate="True" EntityTypeName="" OrderBy="Name" TableName="AP_Staff_Rmb_PerDeimMuliTypes" Where="PortalId == @PortalId">
                            <WhereParameters>
                                <asp:ControlParameter ControlID="hfPortalId" DefaultValue="-1" Name="PortalId" PropertyName="Value" Type="Int32" />
                            </WhereParameters>
                        </asp:LinqDataSource>
                    </td>
                </tr>

                <tr valign="top">
                    <td>
                        <b>
                            <dnn:Label ID="Label2" runat="server" ControlName="tbEntBreakfast" ResourceKey="lblEntertaining" />
                        </b>
                    </td>
                    <td>
                        <table style="font-size: 9pt">
                            <tr>
                                <td>
                                    <asp:Label ID="Label16" runat="server" resourcekey="lblBreakfast"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbEntBreakfast" runat="server" Width="80px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label17" runat="server" resourcekey="lblLunch"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbEntLunch" runat="server" Width="80px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label18" runat="server" resourcekey="lblDinner"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbEntDinner" runat="server" Width="80px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label19" runat="server" resourcekey="lblOvernight"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbEntOvernight" runat="server" Width="80px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label20" runat="server" resourcekey="lblDay"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbEntDay" runat="server" Width="80px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div id='Tab3-tab'>
            <table style="font-size: 9pt;">
                <tr>
                    <td>
                        <b>
                            <dnn:Label ID="lblAccountsRole" runat="server" ControlName="rsgAccountsRoles" ResourceKey="lblAccountsRole" />
                        </b>
                    </td>
                    <td>
                        <cc1:RolesSelectionGrid ID="rsgAccountsRoles" runat="server">
                        </cc1:RolesSelectionGrid>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <dnn:Label ID="lblAccountsName" runat="server" ControlName="tbAccountsName" ResourceKey="lblAccountsName" />
                        </b>
                    </td>
                    <td>
                        <asp:TextBox ID="tbAccountsName" runat="server" Width="200px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <dnn:Label ID="lblAccountsEmail" runat="server" ControlName="tbAccountsEmail" ResourceKey="lblAccountsEmail" />
                        </b>
                    </td>
                    <td>
                        <asp:TextBox ID="tbAccountsEmail" runat="server" Width="200px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <dnn:Label ID="lblAuthUser" runat="server" ControlName="ddlAuthUser" ResourceKey="lblAuthUser" />
                        </b>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlAuthUser" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <dnn:Label ID="lblAuthAuthUser" runat="server" ControlName="ddlAuthAuthAuthUser"
                                ResourceKey="lblAuthAuthUser" />
                        </b>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlAuthAuthUser" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </div>
        <div id='Tab4-tab'>
            <table style="font-size: 9pt;">
                <%--            <tr>
                    <td>
                        <b>
                            <dnn:Label ID="lblUseDCode" runat="server" ControlName="cbUserDCode" ResourceKey="lblUseDCode" />
                        </b>
                    </td>
                    <td>
                        <asp:CheckBox ID="cbUseDCode" runat="server" AutoPostBack="true"  Enabled ="false" />
                    </td>
                </tr>--%>
                <tr valign="top">
                    <td>
                        <b>
                            <dnn:Label ID="Label3" runat="server" ControlName="cblExpenseTypes" ResourceKey="lblExpenseTypes" />
                        </b>
                    </td>
                    <td>
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="LineTypeId"
                            DataSourceID="dsLineTypes">
                            <Columns>
                                <asp:BoundField DataField="TypeName" HeaderText="TypeName" SortExpression="TypeName" />
                                <asp:TemplateField HeaderText="Enable">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hfLineTypeId" runat="server" Value='<%# Eval("LineTypeId") %>' />
                                        <asp:HiddenField ID="hfTypeName" runat="server" Value='<%# Eval("TypeName") %>' />
                                        <asp:CheckBox ID="cbEnable" runat="server" Checked='<%# IsEnabled(Eval("LineTypeId")) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Display Name">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="tbDisplayName" Text='<%# GetDisplayName(Eval("LineTypeId"), Eval("TypeName")) %>' MaxLength="50"/>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="PCode">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlPCode" runat="server" Width="60px" DataSourceID='<%# IIf(Eval("SpareField2") = "INCOME", "dsIncomeCodes", "dsAccountCodes")%>'
                                            DataTextField="DisplayName" SelectedValue='<%#  GetPCode(Eval("LineTypeId")) %>'
                                            DataValueField="AccountCode" AppendDataBoundItems="true" Visible='<%# StaffBrokerFunctions.GetSetting("NonDynamics", PortalId) <> "True" And Not Eval("SpareField2") = "INCOME"%>'>
                                            <asp:ListItem Text="" Value="" />
                                        </asp:DropDownList>
                                        <asp:TextBox runat="server" ID="tbPCode" Text='<%# GetPCodeText(Eval("LineTypeId"))%>' Visible='<%# StaffBrokerFunctions.GetSetting("NonDynamics", PortalId) = "True" And Not Eval("SpareField2") = "INCOME"%>' />

                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="DCode">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlDCode" runat="server" Width="60px"  DataSourceID='<%# IIf(Eval("SpareField2") = "INCOME", "dsIncomeCodes", "dsAccountCodes")%>'
                                            DataTextField="DisplayName" SelectedValue='<%# GetDCode(Eval("LineTypeId")) %>'
                                            DataValueField="AccountCode" AppendDataBoundItems="true" Visible='<%# StaffBrokerFunctions.GetSetting("NonDynamics", PortalId) <> "True" And Not CStr(Eval("TypeName")) = "Donation Income"%>'>
                                            <asp:ListItem Text="" Value="" />
                                        </asp:DropDownList>
                                        <asp:Label ID="Label38" runat="server" Text="Holding A/C" Visible='<%# CStr(Eval("TypeName")) = "Donation Income"%>'></asp:Label>
                                        <asp:TextBox runat="server" ID="tbDCode" Text='<%# GetDCodeText(Eval("LineTypeId"))%>' Visible='<%# StaffBrokerFunctions.GetSetting("NonDynamics", PortalId) = "True"%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <asp:Label ID="Label37" runat="server" forecolor="red" Font-Size="X-Small" Text='<%# IIf(Eval("SpareField2") = "INCOME", "*In some countries, staff can declare income received on their reimbursements (But in most cases this is not relevent or recommended).", "")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:LinqDataSource ID="dsLineTypes" runat="server" ContextTypeName="StaffRmb.StaffRmbDataContext"
                            EntityTypeName="" TableName="AP_Staff_RmbLineTypes" OrderBy="SpareField2">
                        </asp:LinqDataSource>
                        <asp:LinqDataSource ID="dsAccountCodes" runat="server" ContextTypeName="StaffRmb.StaffRmbDataContext"
                            EntityTypeName="" Select="new (AccountCode,  AccountCode + ' ' + '-' + ' ' + AccountCodeName  as DisplayName )"
                            TableName="AP_StaffBroker_AccountCodes" OrderBy="AccountCode" Where="PortalId == @PortalId &amp;&amp; AccountCodeType == @AccountCodeType">
                            <WhereParameters>
                                <asp:ControlParameter ControlID="hfPortalId" DefaultValue="-1" Name="PortalId" PropertyName="Value"
                                    Type="Int32" />
                                <asp:Parameter DefaultValue="4" Name="AccountCodeType" Type="Byte" />
                            </WhereParameters>
                        </asp:LinqDataSource>
                         <asp:LinqDataSource ID="dsIncomeCodes" runat="server" ContextTypeName="StaffRmb.StaffRmbDataContext"
                            EntityTypeName="" Select="new (AccountCode,  AccountCode + ' ' + '-' + ' ' + AccountCodeName  as DisplayName )"
                            TableName="AP_StaffBroker_AccountCodes" OrderBy="AccountCode" Where="PortalId == @PortalId &amp;&amp; AccountCodeType == @AccountCodeType">
                            <WhereParameters>
                                <asp:ControlParameter ControlID="hfPortalId" DefaultValue="-1" Name="PortalId" PropertyName="Value"
                                    Type="Int32" />
                                <asp:Parameter DefaultValue="3" Name="AccountCodeType" Type="Byte" />
                            </WhereParameters>
                        </asp:LinqDataSource>
                    </td>
                </tr>
            </table>
        </div>
        <div id='Tab5-tab'>
            <iframe width="853" height="480" src="https://www.youtube.com/embed/7h1HFWFuCLk?rel=0&wmode=transparent" frameborder="0" allowfullscreen></iframe>
        </div>
    </div>
</div>
<asp:HiddenField ID="hfPortalId" runat="server" Value="-1" />
<asp:HiddenField ID="theHiddenTabIndex" runat="server" Value="0" ViewStateMode="Enabled" />
<asp:LinkButton ID="SaveBtn" runat="server" ResourceKey="btnSave"></asp:LinkButton>
&nbsp;
<asp:LinkButton ID="CancelBtn" runat="server" ResourceKey="btnCancel"></asp:LinkButton>