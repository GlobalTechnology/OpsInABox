<%@ Control Language="VB" AutoEventWireup="False" CodeFile="mpdCalc.ascx.vb" Inherits="DotNetNuke.Modules.AgapeConnect.mpdCalc" %>
<%@ Register Src="~/controls/labelcontrol.ascx" TagName="labelcontrol" TagPrefix="uc1" %>
<%@ Register Src="~/DesktopModules/AgapeConnect/mpdCalc/controls/mpdItem.ascx" TagPrefix="uc1" TagName="mpdItem" %>
<%@ Register Src="~/DesktopModules/AgapeConnect/mpdCalc/controls/mpdTotal.ascx" TagPrefix="uc1" TagName="mpdTotal" %>
<%@ Register Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" TagPrefix="cc1" %>
<%@ Register Src="~/DesktopModules/AgapeConnect/mpdCalc/controls/mpdAdmin.ascx" TagPrefix="uc1" TagName="mpdAdmin" %>

<script src="/js/jquery.numeric.js" type="text/javascript"></script>
<link href="/Portals/_default/Skins/AgapeBlue/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
<script src="/Portals/_default/Skins/AgapeBlue/bootstrap/js/bootstrap.min.js"></script>
<script src="/DesktopModules/AgapeConnect/mpdCalc/js/mpd.js"></script>
<link href="/DesktopModules/AgapeConnect/mpdCalc/css/mpd.css" rel="stylesheet" />
<script type="text/javascript">
    var startPeriodId = '<%= ddlStartPeriod.ClientID%>';
    var customDateId = '<%= customDate.ClientID%>'
    var complienceId  = '<%= cbCompliance.ClientID%>';
    var btnSubmitId = '<%= btnSubmit.ClientID%>';
    var assessmentId = '<%= hfAssessment.ClientId %>';    
    var mpdGoalId = '<%= hfMpdGoal.ClientId %>';
    var compensationId = '<%= hfCompentation.ClientID%>';
    var compensationValueId = '<%= hfCompensationValue.ClientID%>';
    var age = <%= Age1%>;
    var isCouple = '<%= IsCouple() %>';
    var age2 = <%= Age2%> ;
    var staffType=  '<%=StaffType %>';
    
    
    function replaceStaffProfileTags(f){
        <%= TagReplacementScript %>
        return f;
    }



    


</script>


<asp:HiddenField ID="hfAssessment" runat="server" Value="" />
<asp:HiddenField ID="hfCompentation" runat="server" Value="" />
<asp:HiddenField ID="hfCompensationValue" runat="server" Value="0.0" />
<asp:HiddenField ID="hfMpdGoal" runat="server" Value="0.0" />



<asp:HiddenField ID="hfTagReplacementScript" runat="server" Value="" />












<fieldset>
    <legend>
        <asp:Label ID="lblStaffName" runat="server" ></asp:Label></legend>
    <asp:Label ID="lblStatus" runat="server" class="label label-info mpd-status">Draft</asp:Label>
    <div class="subTitle">
         <asp:Label ID="Label1" runat="server" ResourceKey="StartDate" ></asp:Label>
        <asp:DropDownList ID="ddlStartPeriod" runat="server"></asp:DropDownList>
        <span id="customDate" runat="server" style="display: none;">
            <asp:DropDownList ID="ddlPeriod" runat="server" Width="120px" Font-Bold="true">
                <asp:ListItem Text="January" Value="1"></asp:ListItem>
                <asp:ListItem Text="February" Value="2"></asp:ListItem>
                <asp:ListItem Text="March" Value="3"></asp:ListItem>
                <asp:ListItem Text="April" Value="4"></asp:ListItem>
                <asp:ListItem Text="May" Value="5"></asp:ListItem>
                <asp:ListItem Text="June" Value="6"></asp:ListItem>
                <asp:ListItem Text="July" Value="7"></asp:ListItem>
                <asp:ListItem Text="August" Value="8"></asp:ListItem>
                <asp:ListItem Text="September" Value="9"></asp:ListItem>
                <asp:ListItem Text="October" Value="10"></asp:ListItem>
                <asp:ListItem Text="November" Value="11"></asp:ListItem>
                <asp:ListItem Text="December" Value="12"></asp:ListItem>

            </asp:DropDownList>
            <asp:DropDownList ID="ddlYear" runat="server" Width="100px" Font-Bold="true">
            </asp:DropDownList>
        </span>

    </div>
    <div class="clearfix" />

    <div id="formRoot" class="form-horizontal">

        <asp:Repeater ID="rpSections" runat="server">

            <ItemTemplate>

                <asp:ImageButton ID="ImageButton1" runat="server" class="mpd-btn-order" ImageUrl="~/images/action_up.gif" CommandName="UP" CommandArgument='<%# Eval("SectionId")%>' Visible='<%# Eval("Number") > 1 And IsEditMode()%>' formnovalidate />
                <asp:ImageButton ID="ImageButton2" runat="server" class="mpd-btn-order" ImageUrl="~/images/action_down.gif" CommandName="DOWN" CommandArgument='<%# Eval("SectionId")%>' Visible='<%# Eval("Number")< LastSection  And IsEditMode()%>' formnovalidate />
                <div class="mpd-section-title" style="float: left;">
                    <h3>
                        <asp:Label ID="lblSectionName" runat="server" Text='<%# Eval("Name")%>'></asp:Label></h3>
                </div>

                <asp:HyperLink ID="btnEditSectionName" runat="server" CssClass="btn-edit-section" ResourceKey="Edit" Visible='<%# IsEditMode%>'></asp:HyperLink>
                <div class="mpd-edit-section" style="display: none">
                    <asp:TextBox ID="tbSectionName" runat="server" Font-Size="X-Large" Font-Bold="true" Width="300px" Text='<%# Eval("Name")%>'></asp:TextBox>
                    <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument='<%# Eval("SectionId")%>' CommandName="EditSectionTitle" formnovalidate>Save</asp:LinkButton>
                    &nbsp; &nbsp;
                    <a href="#" class="btn-edit-section-cancel"> <asp:Label ID="lblStaffName" runat="server" ResourceKey="Cancel" ></asp:Label></a>
                </div>


                <div style="clear: both;" />
                </h3>
             
            <div class="well">

                <asp:Repeater ID="rpItems" runat="server" DataSource='<%# CType(Eval("AP_mpdCalc_Questions"), System.Data.Linq.EntitySet(Of MPD.AP_mpdCalc_Question)).OrderBy(Function (c) c.QuestionNumber)%>'>

                    <ItemTemplate>
                        <uc1:mpdItem runat="server" ID="theMpdItem" SectionId='<%# Eval("SectionId")%>' QuestionId='<%# Eval("QuestionId")%>' Mode='<%# Eval("Type")%>'
                            Formula='<%# Eval("Formula")%>' ItemName='<%# GetName(Eval("QuestionId"),Eval("Name"))%>' Help='<%# Eval("Help")%>'
                            Monthly='<%# GetAnswer(Eval("QuestionId"))%>'
                            ItemId='<%# Eval("AP_mpdCalc_Section.Number") & "." & Eval("QuestionNumber")%>' TaxSystem='<%# Eval("TaxSystem")%>'
                            Threshold1='<%# CType(Eval("Threshold1"), Nullable(Of Decimal))%>' Threshold2='<%# CType(Eval("Threshold2"), Nullable(Of Decimal))%>' Threshold3='<%# CType(Eval("Threshold3"), Nullable(Of Decimal))%>'
                            Rate1='<%# CType(Eval("Rate1"), Nullable(Of Double))%>' Rate2='<%#  CType(Eval("Rate2"), Nullable(Of Double))%>' Rate3='<%# CType(Eval("Rate3"), Nullable(Of Double))%>' Rate4='<%#  CType(Eval("Rate4"), Nullable(Of Double))%>'
                            Fixed='<%# Eval("Fixed")%>' Min='<%# Eval("Min")%>' Max='<%# Eval("Max")%>' 
                             AccountCode='<%# IIf(String.IsNullOrEmpty(Eval("AccountCode")), DefaultAccount, Eval("AccountCode"))%>'/>
                    </ItemTemplate>

                </asp:Repeater>

                <uc1:mpdItem runat="server" ID="mpdItem14" SectionId='<%# Eval("SectionId")%>' QuestionId='-1' Mode='INSERT' Formula='' ItemName='' Help='' ItemId='<%# Eval("Number") & "." & GetMaxQuestionNumber(Eval("AP_mpdCalc_Questions"))%>' TaxSystem='FIXED_RATE' Min='0' />
                <asp:Panel ID="ptnDeleteSection" runat="server" class="mpd-insert" Visible='<%# CType(Eval("AP_mpdCalc_Questions"), System.Data.Linq.EntitySet(Of MPD.AP_mpdCalc_Question)).Count =0%>'>
                    <asp:LinkButton ID="btnDeleteSection" runat="server" CssClass="btn-delete-section" CommandArgument='<%# Eval("SectionId")%>' CommandName="DeleteSection">Delete Section</asp:LinkButton>
                </asp:Panel>
                <uc1:mpdTotal runat="server" ID="totSection1" ItemName='<%# Eval("Name")%>' Bold="True" IsSectionTotal="True" />
            </div>
            </ItemTemplate>

        </asp:Repeater>

        <asp:Panel ID="pnlInsert" runat="server" Visible="false">
            <div class="mpd-insert-section">
                <asp:HyperLink ID="hlInsert" runat="server" CssClass="btn-section-insert">Insert New Section</asp:HyperLink>
            </div>
            <asp:Panel ID="Panel1" runat="server" class="mpd-section-insert well" Style="display: none;">
                <div class="control-group span5">
                    <label class="control-label" style="width: 160px">Section Title</label>
                    <div class="controls">
                        <asp:TextBox ID="tbInsertSectionName" runat="server" placeholder="Section Title" Width="300px" ValidationGroup="insertSection"></asp:TextBox>

                    </div>
                </div>
                <div class="control-group span4">
                    <label class="control-label" style="width: 160px">Display at index:</label>
                    <div class="controls">
                        <asp:DropDownList ID="ddlInsertOrder" runat="server" Width="110px" AppendDataBoundItems="true">
                            <asp:ListItem Text="1 - Top" Value="1"></asp:ListItem>

                        </asp:DropDownList>
                    </div>
                </div>
                <div class="control-group span3">
                    <asp:Button ID="btnInsertSection" runat="server" ResourceKey="Insert" CssClass="btn btn-primary" formnovalidate />
                    &nbsp;&nbsp;
            <input type="button" id="insert-cancel" class="btn insert-cancel" value="Cancel" />
                </div>
            </asp:Panel>


        </asp:Panel>

        <div class="well">
            <asp:Label ID="lblPercentage" runat="server" class="percentage" Text=""></asp:Label>
            <uc1:mpdTotal runat="server" ID="totSubTotal" ItemName="SubTotal" Bold="false" Mode="monthly" IsSubtotal="True" />
            <uc1:mpdTotal runat="server" ID="totAssessment" ItemName="Assessment" Bold="false" Mode="monthly" IsAssessment="True" />
            <uc1:mpdTotal runat="server" ID="totGoal" ItemName="MPD Goal" Bold="True" Mode="monthly" IsMPDGoal="True" />
             <uc1:mpdTotal runat="server" ID="totMyPortion" ItemName="I am responsible to raise" Bold="True" Mode="monthly"  IsMyPortion="True" />
            <uc1:mpdItem runat="server" ID="itemCurrent" ItemName="Current Support Level" ItemId="" Help="" Mode="BASIC_MONTH" IsCurrentSupport="True" />

            <uc1:mpdTotal runat="server" ID="totRemaining" ItemName="Amount to discover" Bold="false" Mode="monthly" IsRemaining="True" />
            <div style="clear: both" />
            <div class="checkboxOuter">

                <asp:CheckBox ID="cbCompliance" runat="server" CssClass="checkbox" />
            </div>


            <div style="width: 100%; text-align: center;">
                <asp:Button ID="btnSave" runat="server" ResourceKey="Save" Font-Size="X-Large" CssClass="btn" formnovalidate />
                &nbsp;&nbsp;
                <asp:Button ID="btnSubmit" runat="server" ResourceKey="Submit" Font-Size="X-Large" CssClass="btn btn-primary" Enabled="false" Visible="false" />
                <asp:Button ID="btnApprove" runat="server" ResourceKey="Approve" Font-Size="X-Large" CssClass="btn btn-primary" Visible="false" />
                <asp:Button ID="btnProcess" runat="server" ResourceKey="Process" Font-Size="X-Large" CssClass="btn btn-primary" Visible="false" />
                &nbsp;&nbsp;
                 <asp:Button ID="btnCancel" runat="server" ResourceKey="Reject" Font-Size="X-Large" CssClass="btn" />
                &nbsp;&nbsp;
                <asp:Button ID="btnBack" runat="server" ResourceKey="Back" Font-Size="X-Large" CssClass="btn" />
            </div>
        </div>

    </div>
</fieldset>

<uc1:mpdAdmin runat="server" id="mpdAdminPanel" Visible="False" />

