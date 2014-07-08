<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Departments.ascx.vb" Inherits="DotNetNuke.Modules.StaffAdmin.Departments" %>

 <%@ Register src="../../../controls/labelcontrol.ascx" tagname="labelcontrol" tagprefix="uc1" %>
 <%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>

 <%@ Register src="~/DesktopModules/AgapeConnect/StaffAdmin/Controls/acImage.ascx" tagname="acImage" tagprefix="uc2" %>

 <script type="text/javascript">
     /*globals jQuery, window, Sys */



     (function ($, Sys) {
         function setUpMyTabs() {

             $('.aButton').button();

         }

         $(document).ready(function () {
             setUpMyTabs();
             Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                 setUpMyTabs();
             });
         });
     } (jQuery, window.Sys));



</script>

 <asp:HiddenField ID="hfPortalId" runat="server" />

<table width="100%">
    <tr valign="top" >
        <td width="200px">
            <asp:ListBox ID="lbDepartments" runat="server" DataSourceID="dsDepartments" 
                DataTextField="Name" DataValueField="CostCenterId" AutoPostBack="True" width="100%" Height="400px"></asp:ListBox>
            
            <asp:LinqDataSource ID="dsDepartments" runat="server" 
                ContextTypeName="StaffBroker.StaffBrokerDataContext" EntityTypeName="" 
                OrderBy="Name" TableName="AP_StaffBroker_Departments" 
                Where="PortalId == @PortalId">
                <WhereParameters>
                    <asp:ControlParameter ControlID="hfPortalId" Name="PortalId" 
                        PropertyName="Value" Type="Int32" />
                </WhereParameters>
            </asp:LinqDataSource>
            
        </td>

        <td style="margin-left: 40px">
            <asp:FormView ID="FormView1" runat="server" DataKeyNames="CostCenterId" 
                DataSourceID="dsTheDept" Width="100%" DefaultMode="Edit" >
                <EditItemTemplate>
                    <fieldset>
                        <legend> <asp:Label ID="Label1" runat="server" Font-Bold="true" Font-Size="16pt" Text='<%# Eval("Name") %>' /></legend>
                 
                    <table>
                        <tr>
                            <td>
                                <uc1:labelcontrol ID="labelcontrol3" runat="server" Text="Name" HelpText="Enter the Department name" />
                            </td>
                            <td colspan="3"> 
                                <asp:TextBox ID="TextBox1" runat="server" Font-Size="14pt" Font-Bold="true" Text='<%# Bind("Name") %>' />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <uc1:labelcontrol ID="labelcontrol1" runat="server" Text="Responsibility Centre" HelpText="Enter the Responsibility Cetner (cost centre) code. This is the code that your accounts program uses to identify this Responsibility Centre." />
                            </td>
                            <td> 
                               <asp:DropDownList ID="ddlCostCentre" runat="server" SelectedValue='<%# StaffBrokerFunctions.ValidateCostCenter( Eval("CostCentre"), PortalId) %>'  
                                    DataSourceID="dsCostCenters" DataTextField="DisplayName" 
                                    DataValueField="CostCentreCode" AppendDataBoundItems="true"  Visible='<%# StaffBrokerFunctions.GetSetting("NonDynamics", PortalId)<> "True"%>'>
                                    <asp:ListItem Text="" Value=""  />
                                </asp:DropDownList>
                               
                                 <asp:TextBox ID="tbCostCentreCode" runat="server" Visible='<%# StaffBrokerFunctions.GetSetting("NonDynamics", PortalId) = "True"%>' Text='<%# Eval("CostCentre")%>'></asp:TextBox>
                            </td>
                            <td style="width: 250px;">
                                <uc1:labelcontrol ID="labelcontrol5" runat="server" Text="Can Reimburse" HelpText="Check this option if you want staff to be able to reimburse from this cost centre" /><br />
                                  <uc1:labelcontrol ID="labelcontrol12" runat="server" Text="Can Recieve Non-Donation Income" HelpText="In some countries, non-donation income (like conference income) can declared on a reimbursement (using the Non-Donation Income expense type) - and the staff member must specify what the income is for (i.e. where the money should be booked). If this box is checked it will appear in the list of available options. Note that is not applicable in most countries." />
                            </td>
                            <td>
                                <asp:CheckBox ID="CanRmbCheckBox" runat="server" Checked='<%# Bind("CanRmb") %>' /><br />
                                 <asp:CheckBox ID="CanRecieveNonDonaitonIncome" runat="server" Checked='<%# Bind("Spare1")%>' />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <uc1:labelcontrol ID="labelcontrol2" runat="server" Text="Manager" HelpText="Please select a manager for this cost centre. He will approve reimbursements and be notified of financial transactions." />
                            </td>
                            <td> 
                                <asp:DropDownList ID="ddlDelegate" runat="server" Font-Size="X-Small"  DataSource='<%#  getStaffAdd(Eval("CostCentreManager")) %>'
                 DataTextField="DisplayName" DataValueField="UserID" AppendDataBoundItems="true" SelectedValue='<%# Bind("CostCentreManager") %>' >
                 <asp:ListItem Value="" Text="Not Set"></asp:ListItem>
            </asp:DropDownList>
                            </td>
                            <td style="width: 250px;">
                                <uc1:labelcontrol ID="labelcontrol6" runat="server" Text="Can Charge" HelpText="Check this option if you want to allow staff to charge purchases to this cost centre. (Note: You may not be using any modules that require user payments)" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("CanCharge") %>' />
                            </td>
                         
                        </tr>
                        <tr>
                            <td>
                                <uc1:labelcontrol ID="labelcontrol4" runat="server" Text="Delegate" HelpText="If you are away temporarily, the Department Manager can delegate his responsibilites. Both the manager and the delegate will be notified of financial transactions and either can approve reimbursements" />
                            </td>
                            <td> 
                                <asp:DropDownList ID="DropDownList2" runat="server" Font-Size="X-Small"  DataSource='<%#  getStaffAdd(Eval("CostCentreDelegate")) %>'
                 DataTextField="DisplayName" DataValueField="UserID" AppendDataBoundItems="true" SelectedValue='<%# Bind("CostCentreDelegate") %>' >
                 <asp:ListItem Value="" Text="Not Set"></asp:ListItem>
            </asp:DropDownList>
                            </td>
                             <td style="width: 250px;">
                                <uc1:labelcontrol ID="labelcontrol7" runat="server" Text="Can Receive Donations" HelpText="Check this option if you want to allow the public to give online to this Department. (This requires that you setup and use the Online Gifts Module) " />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox2" runat="server" Checked='<%# Bind("CanGiveTo") %>' />
                            </td>
                        </tr>
                         <tr>
                            <td>
                                <uc1:labelcontrol ID="labelcontrol10" runat="server" Text="PayType" HelpText="This field allows you to enter information required by your Donation system." />
                            </td>
                            <td> 
                                <asp:TextBox ID="TextBox2" runat="server" Text = '<%# Bind("PayType") %>'></asp:TextBox>
                            </td>
                             <td style="width: 250px;">
                               <uc1:labelcontrol ID="labelcontrol11" runat="server" Text="Is Project:" HelpText="This Department is a Project." />
                        

                            </td>
                            <td>
                                  <asp:CheckBox ID="CheckBox3" runat="server" Checked='<%# Bind("IsProject") %>' />
                            </td>
                        </tr>
                    </table>

                    <fieldset class="dept-giving">
                        <legend>
                           <asp:Label ID="Label2" runat="server" Text="Giving Options"></asp:Label>
                        </legend>
                        <table align="left">
                            <tr>
                                <td>
                                    <uc1:labelcontrol ID="labelcontrol8" runat="server" Text="Giving Shortcut" HelpText="Enter a shortname for this Department. If you have configures the Online Gifts module - donors can give to this department at https://give.agape.org.uk/ <i>givingshortcut</i>" />
                                </td>
                                <td>
                                    <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("GivingShortcut") %>' />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <uc1:labelcontrol ID="labelcontrol13" runat="server" Text="Photo" HelpText="Enter a shortname for this Department. If you have configures the Online Gifts module - donors can give to this department at https://give.agape.org.uk/ <i>givingshortcut</i>" />
                                </td>
                                <td>
                                     <uc2:acImage ID="acImage1" runat="server"  Width="200"  Apsect="1" FileId='<%# Bind("PhotoId") %>' OnPreRender ="LoadImage" />
                                      <asp:Label ID="lblError" runat="server" ForeColor="Red" Text="Please set the photo aspect before updating" Visible="False"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <uc1:labelcontrol ID="labelcontrol9" runat="server" Text="Giving Text:" HelpText="You can customise this departments giving page by inserting content here. This will be displayed at the top of your giving page." />
                                <td>
                                   
                                    </td>
                         
                            </tr>
                        </table>
                                <br />
                                   <dnn:TextEditor ID="teMessage" runat="server" Width="650px" TextRenderMode="Raw"  Text='<%# Bind("GivingText") %>' HtmlEncode="False" defaultmode="Rich" height="240" choosemode="False" chooserender="False"  />
         
                           
                    </fieldset>


                    <br />
                    <asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True" 
                        CommandName="Update" Text="Update" CssClass="aButton btn"  />
                <%--  &nbsp;<asp:LinkButton ID="NewButton" runat="server" CausesValidation="False" 
                        CommandName="New" Text="Add New Department" />--%>
                        </fieldset>&nbsp;
                </EditItemTemplate>
                <InsertItemTemplate>
                   <fieldset>
                        <legend> <asp:Label ID="Label1" runat="server" Font-Bold="true" Font-Size="16pt" Text="Add New Department" /></legend>
                 
                    <table>
                        <tr>
                            <td>
                                <uc1:labelcontrol ID="labelcontrol3" runat="server" Text="Name" HelpText="Enter the Department name" />
                            </td>
                            <td colspan="3"> 
                                <asp:TextBox ID="TextBox1" runat="server" Font-Size="14pt" Font-Bold="true" Text='<%# Bind("Name") %>' />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <uc1:labelcontrol ID="labelcontrol1" runat="server" Text="Responsibility Center" HelpText="Enter the Responsibility Center (cost centre) code. This is the code that your accounts program uses to identify this Responsibility Center." />
                            </td>
                            <td> 
                              
                                   <asp:DropDownList ID="DropDownList1" runat="server" SelectedValue='<%# Bind("CostCentre") %>'
                                    DataSourceID="dsCostCenters" DataTextField="DisplayName" 
                                    DataValueField="CostCentreCode" AppendDataBoundItems="true">
                                    <asp:ListItem Text="" Value="" />
                                </asp:DropDownList>
                                
                            </td>
                            <td style="width: 250px;">
                                <uc1:labelcontrol ID="labelcontrol5" runat="server" Text="Can Reimburse" HelpText="Check this option if you want staff to be able to reimburse from this cost centre" /><br />
                                  <uc1:labelcontrol ID="labelcontrol12" runat="server" Text="Can Recieve Non-Donation Income" HelpText="In some countries, non-donation income (like conference income) can declared on a reimbursement (using the Non-Donation Income expense type) - and the staff member must specify what the income is for (i.e. where the money should be booked). If this box is checked it will appear in the list of available options. Note that is not applicable in most countries." />
                            </td>
                            <td>
                                <asp:CheckBox ID="CanRmbCheckBox" runat="server" Checked='<%# Bind("CanRmb") %>' /><br />
                                 <asp:CheckBox ID="CanRecieveNonDonaitonIncome" runat="server" Checked='<%# Eval("Spare1")="True"%>' />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <uc1:labelcontrol ID="labelcontrol2" runat="server" Text="Manger" HelpText="Please select a manager for this cost centre. He will approve reimbursements and be notified of financial transactions." />
                            </td>
                            <td> 
                             <asp:DropDownList ID="ddlDelegate" runat="server" Font-Size="X-Small"  DataSource='<%#  getStaffAdd(Eval("CostCentreManager")) %>'
                 DataTextField="DisplayName" DataValueField="UserID" AppendDataBoundItems="true" SelectedValue='<%# Bind("CostCentreManager") %>' >
                 <asp:ListItem Value="" Text="Not Set"></asp:ListItem>
            </asp:DropDownList>
                            </td>
                            <td style="width: 250px;">
                                <uc1:labelcontrol ID="labelcontrol6" runat="server" Text="Can Charge" HelpText="Check this option if you want to allow staff to charge purchases to this cost centre. (Note: You may not be using any modules that require user payments)" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("CanCharge") %>' />
                            </td>
                         
                        </tr>
                        <tr>
                            <td>
                                <uc1:labelcontrol ID="labelcontrol4" runat="server" Text="Delegate" HelpText="If you are away temporarily, the Department Manager can delegate his responsibilites. Both the manager and the delegate will be notified of financial transactions and either can approve reimbursements" />
                            </td>
                            <td> 
                                 <asp:DropDownList ID="DropDownList2" runat="server" Font-Size="X-Small"  DataSource='<%#  getStaffAdd(Eval("CostCentreDelegate")) %>'
                 DataTextField="DisplayName" DataValueField="UserID" AppendDataBoundItems="true" SelectedValue='<%# Bind("CostCentreDelegate") %>' >
                 <asp:ListItem Value="" Text="Not Set"></asp:ListItem>
            </asp:DropDownList>
                            </td>
                             <td style="width: 250px;">
                                <uc1:labelcontrol ID="labelcontrol7" runat="server" Text="Can Receive Donations" HelpText="Check this option if you want to allow the public to give online to this Department. (This requires that you setup and use the Online Gifts Module) " />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox2" runat="server" Checked='<%# Bind("CanGiveTo") %>' />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <uc1:labelcontrol ID="labelcontrol10" runat="server" Text="PayType" HelpText="This field allows you to enter information required by your Donation system." />
                            </td>
                            <td> 
                                <asp:TextBox ID="TextBox2" runat="server" Text = '<%# Bind("PayType") %>'></asp:TextBox>
                            </td>
                              <td style="width: 250px;">
                               <uc1:labelcontrol ID="labelcontrol11" runat="server" Text="Is Project:" HelpText="This Department is a Project." />
                        

                            </td>
                            <td>
                                  <asp:CheckBox ID="CheckBox3" runat="server" Checked='<%# Bind("IsProject") %>' />
                            </td>
                        </tr>
                    </table>

                    <fieldset  class="dept-giving">
                        <legend>
                           <asp:Label ID="Label2" runat="server" Text="Giving Options" cssclass="AgapeH4"></asp:Label>
                        </legend>
                        <table align="left">


                            <tr>
                                <td>
                                    <uc1:labelcontrol ID="labelcontrol8" runat="server" Text="Giving Shortcut" HelpText="Enter a shortname for this Department. If you have configures the Online Gifts module - donors can give to this department at https://give.agape.org.uk/ <i>givingshortcut</i>" />
                                </td>
                                <td>
                                    <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("GivingShortcut") %>' />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <uc1:labelcontrol ID="labelcontrol13" runat="server" Text="Photo" HelpText="Enter a shortname for this Department. If you have configures the Online Gifts module - donors can give to this department at https://give.agape.org.uk/ <i>givingshortcut</i>" />
                                </td>
                                <td>
                                     <uc2:acImage ID="acImage1" runat="server"  Width="200"  Apsect="1" FileId='<%# Bind("PhotoId") %>' OnPreRender ="LoadImage" />
                                
                                <asp:Label ID="lblError" runat="server" ForeColor="Red" Text="Please set the photo aspect before updating" Visible="False"></asp:Label>

                                </td>
                            </tr>>


                            <tr>
                                <td>
                                    <uc1:labelcontrol ID="labelcontrol9" runat="server" Text="Giving Text:" HelpText="You can customise this departments giving page by inserting content here. This will be displayed at the top of your giving page." />
                                <td></td>
                         
                            </tr>
                        </table>
                                <br />
                                   <dnn:TextEditor ID="teMessage" runat="server" Width="650px" TextRenderMode="Raw"  Text='<%# Bind("GivingText") %>' HtmlEncode="False" defaultmode="Rich" height="240" choosemode="False" chooserender="False"  />
         
                           
                    </fieldset>
                    <asp:LinkButton ID="InsertButton" runat="server" CausesValidation="True" 
                        CommandName="Insert" Text="Insert" CssClass="aButton btn" />
                   
                </InsertItemTemplate>
                <ItemTemplate>
                    
                    <asp:LinkButton ID="EditButton" runat="server" CausesValidation="False" 
                        CommandName="Edit" Text="Edit"  />
                    &nbsp;<asp:LinkButton ID="DeleteButton" runat="server" CausesValidation="False" 
                        CommandName="Delete" Text="Delete" />
                    &nbsp;<asp:LinkButton ID="NewButton" runat="server" CausesValidation="False" 
                        CommandName="New" Text="New" />
                </ItemTemplate>
            </asp:FormView>
            



            <asp:LinqDataSource ID="dsTheDept" runat="server" 
                ContextTypeName="StaffBroker.StaffBrokerDataContext" EnableDelete="True" 
                EnableInsert="True" EnableUpdate="True" EntityTypeName="" 
                TableName="AP_StaffBroker_Departments" Where="CostCenterId == @CostCenterId">
                <WhereParameters>
                    <asp:ControlParameter ControlID="lbDepartments" Name="CostCenterId" 
                        PropertyName="SelectedValue" Type="Int32" DefaultValue="0" />
                </WhereParameters>
            </asp:LinqDataSource>
           
       <asp:LinqDataSource ID="dsCostCenters" runat="server" 
                                    ContextTypeName="StaffBroker.StaffBrokerDataContext" EntityTypeName="" 
                                    OrderBy="CostCentreCode" Select="new (CostCentreCode,CostCentreCode + ' ' + '-' + ' ' + CostCentreName as DisplayName)" 
                                    TableName="AP_StaffBroker_CostCenters" 
                Where="PortalId == @PortalId ">
                                    <WhereParameters>
                                        <asp:ControlParameter ControlID="hfPortalId" DefaultValue="-1" Name="PortalId" 
                                            PropertyName="Value" Type="Int32" />
                                        <asp:Parameter DefaultValue="0" Name="Type" Type="Byte" />
                                    </WhereParameters>
                                </asp:LinqDataSource>

        </td>
    </tr>
</table>

<br />

<asp:Button ID="btnAddNew" runat="server" Text="Add New Department" CssClass="aButton btn" />
<asp:LinkButton ID="btnBulkUpload" runat="server" Text="Bulk Upload"  />