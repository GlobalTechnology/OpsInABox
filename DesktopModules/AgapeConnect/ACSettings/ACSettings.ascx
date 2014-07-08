<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ACSettings.ascx.vb" Inherits="DotNetNuke.Modules.AgapeConnect.ACSettings" %>

<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
 <%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>
 <%@ Register Src="~/controls/SectionHeadControl.ascx" TagName="SectionHead" TagPrefix="dnn2" %>

 <script type="text/javascript">
     (function ($, Sys) {
         function setUpMyTabs() {

             $("#divRUSure").dialog({
                 autoOpen: false,
                 height: 350,
                 width: 600,
                 modal: true,
                 title: "Create a New Reimbursement",
                 close: function () {
                     allFields.val("").removeClass("ui-state-error");
                 }
             });
             $("#divRUSure").parent().appendTo($("form:first"));
         }

         $(document).ready(function () {
             setUpMyTabs();
             Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                 setUpMyTabs();
             });
         });
     } (jQuery, window.Sys));
   
   
     function showPopup()  {$("#divRUSure").dialog("open");  return false;}
     function closePopup()  {$("#divRUSure").dialog("close");}


</script>
    
 <asp:HiddenField ID="hfPortalId" runat="server" />

 <div align="left">
 <table>
    <tr>
        <td width="200px"><b><dnn:label id="ttlCurrency" runat="server" controlname="lblCurrency" Text="Currency Symbol:" HelpText="All transaction on this site will be displayed using this currency symbol."   /></b></td>
        <td>
           <asp:Label ID="lblCurrency" runat="server"></asp:Label>
        </td>
    </tr>
     <tr>
        <td width="200px"><b><dnn:label id="ttlCompanyId" runat="server" controlname="lblCompanyId" Text="CompanyID:" HelpText="The CompanyID (as listed in your Accounts program) "   /></b></td>
        <td>
           <asp:Label ID="lblCompanyId" runat="server"></asp:Label>
        </td>
    </tr>
     <tr>
        <td width="200px"><b><dnn:label id="ttlPassword" runat="server" controlname="lblPassword" Text="Web Service Password:" HelpText="The datapump (installed on your Accounts server), uses a webservice to send and receive transactions. When you install the datapump, you will need to enter this password. This does not have to (and should not) be memorable."   /></b></td>
        <td>
           <asp:Label ID="lblPassword" runat="server"></asp:Label>
           &nbsp; &nbsp;
          
           <a href="#" onclick="showPopup();">change</a>
                
        </td>
         
    </tr>
    <tr>
        <td width="200px"><b><dnn:label id="ttlStatus" runat="server" controlname="lblStatus" Text="Datapump Status:" HelpText="Let's you know if the datapump has been setup correctly."   /></b></td>
        <td>
           <asp:Label ID="lblStatus" runat="server"></asp:Label>
        </td>
    </tr>
    
 </table>





<fieldset>
    <legend class="AgapeH3"> <dnn2:SectionHead CssClass="AgapeH4"
    runat="server"  ID="m_sectionHeadimageSection"
    IsExpanded="false" Section="pnlAdvanced"
    Text="Advanced Settings" /></legend>
    <div ID="pnlAdvanced" runat="server">
<asp:Label ID="lblHighlight" runat="server"  class="ui-state-highlight ui-corner-all" style="padding: 3px;" Text="Warning: Changing these values can break your site quickly! Do not change these values, unless you are confident you understand them."></asp:Label>
        
        <br /><br />
<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"  CssClass="dnnGrid" CellPadding="4"
    DataKeyNames="SettingId" DataSourceID="dsSettings">
    <Columns>
        <asp:BoundField DataField="SettingName" HeaderText="SettingName" 
            ReadOnly="True" SortExpression="SettingName" >
        <ItemStyle Font-Bold="True" />
        </asp:BoundField>
        <asp:BoundField DataField="SettingValue" HeaderText="SettingValue" 
            SortExpression="SettingValue" />
        <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
    </Columns>
         <FooterStyle CssClass="ui-widget-header dnnGridFooter" />
                   <HeaderStyle CssClass="ui-widget-header dnnGridHeader"  />
                   
                    <EmptyDataRowStyle CssClass="ui-widget-header dnnGridHeader" />
                    <PagerStyle CssClass="dnnGridPager" />
                    <RowStyle CssClass="dnnGridItem" />
                    <SelectedRowStyle CssClass="dnnFormError" />
                    <SortedAscendingCellStyle BackColor="#FDF5AC" />
                    <SortedAscendingHeaderStyle BackColor="#4D0000" />
                    <SortedDescendingCellStyle BackColor="#FCF6C0" />
                    <SortedDescendingHeaderStyle BackColor="#820000" />
</asp:GridView>
<asp:LinqDataSource ID="dsSettings" runat="server" 
    ContextTypeName="StaffBroker.StaffBrokerDataContext" EnableDelete="True" 
    EnableInsert="True" EnableUpdate="True" EntityTypeName="" 
    TableName="AP_StaffBroker_Settings" Where="PortalId == @PortalId">
    <WhereParameters>
        <asp:ControlParameter ControlID="hfPortalId" DefaultValue="-1" Name="PortalId" 
            PropertyName="Value" Type="Int32" />
    </WhereParameters>
</asp:LinqDataSource>
</div>
</fieldset>
</div>


<div id="divRUSure" class="ui-widget">
    <b>Are you sure that you want to generate a new password? </b> <br /><br />
    <i>This will break the automatic datalink with your accounts program (and TnT Dataserver), 
    until you enter the new password. Please only reset this password if you know what you are doing,
    or you believe that the webservice might have been compromised.
    
    </i><br /><br />
   <div width="100%" style="text-align: center">
    <asp:Button ID="btnOK" runat="server" Text="OK" class="aButton btn" OnClientClick="closePopup();"  />
    <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="aButton btn" OnClientClick="closePopup();"  />

    </div>
</div>