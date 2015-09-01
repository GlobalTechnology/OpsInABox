<%@ Control Language="VB" AutoEventWireup="false" CodeFile="EditStaffProfile.ascx.vb" Inherits="DotNetNuke.Modules.AgapeConnect.EditStaffProfile" %>
<%@ Register Src="../StaffAdmin/Controls/acImage.ascx" TagName="acImage" TagPrefix="uc1" %>
<%@ Register Src="~/controls/labelcontrol.ascx" TagName="labelcontrol" TagPrefix="uc1" %>
<%@ Register Src="../StaffAdmin/Controls/Leaders.ascx" TagName="Leaders" TagPrefix="uc3" %>
<%@ Register Src="../StaffAdmin/Controls/Plebs.ascx" TagName="Plebs" TagPrefix="uc4" %>
<%@ Register Src="../StaffAdmin/Controls/Departments.ascx" TagName="Depts" TagPrefix="uc6" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>


<%@ Register src="../../../controls/user.ascx" tagname="user" tagprefix="uc2" %>


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
            var pickerOpts = {
                dateFormat: '<%= GetDateFormat() %>'
            };
            $('.datepicker').datepicker(pickerOpts);
            <%= IIF(Settings("DisplayGivingTab") = "True","","removeTab(3);  ")  %>
        }
        $(document).ready(function () {
            setUpMyTabs();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                setUpMyTabs();
            });
        });
    } (jQuery, window.Sys));

    function removeTab(i) { $('#tabs').tabs("remove", i); }


</script>
<asp:Panel ID="pnlStaffProfile" runat="server">

<div style="width: 100%; text-align: Center;" >
<asp:HiddenField ID="hfPortalId" runat="server" />
<asp:HiddenField ID="theHiddenTabIndex" runat="server" Value="0" ViewStateMode="Enabled" />
<asp:HiddenField ID="hfUserId1" runat="server" />
<asp:HiddenField ID="hfUserId2" runat="server" />
<asp:Label ID="lblTest" runat="server" visible="False"></asp:Label>
<asp:Label ID="lblWarning" runat="server" Font-Italic="true" ForeColor="Gray">Don't forget to click update (bottom of the page) to save your changes.</asp:Label>
<div id="tabs" style="width: 100%; text-align: Left;">
    <ul>
        <li><a href='#Tab0-tab'><asp:Label ID="lblProfile" runat="server" ></asp:Label></a></li>
        <li><a href='#Tab1-tab'><asp:Label ID="lblEmployment" runat="server" ></asp:Label></a></li>
        <li><a href='#Tab2-tab'><asp:Label ID="lblLeadership" runat="server" ></asp:Label></a></li>
        <li><a href='#Tab3-tab'><asp:Label ID="lblFinance" runat="server" ></asp:Label></a></li>
        
    </ul>
    <div style="width: 100%; min-height: 350px; background-color: #FFFFFF;">
        <div id='Tab0-tab' style="text-align: center;">
        <table width="100%" style="text-align: center;">
            <tr >
                <td width="100px"></td>
                <td width="50%">
                    <asp:Label ID="lblName1" class="AgapeH3" runat="server" >Name1</asp:Label>
                </td>
                <td width="50%">
                    <asp:Label ID="lblName2"  class="AgapeH3" runat="server" >Name2</asp:Label>
                </td>
            </tr>
            <tr >
                <td width="100px">
                <uc1:labelcontrol ID="lblPhoto" runat="server" Text="Photo:"  Width="100px" HelpText="Please upload a photo of yourself. Please crop it to the right shape (square), by draging a box over the image and clicking update." />
                </td>
                <td align="center">
                    <uc1:acImage ID="profileImage1" runat="server"  Aspect="1.0"  Width="200"  />
                </td>
                <td align="center">
                   <uc1:acImage ID="profileImage2" runat="server"  Aspect="1.0"  Width="200" />
                </td>
            </tr>
            <tr >
                <td width="100px">
                    <uc1:labelcontrol ID="lblEmail" runat="server" Width="100px" Text="Email:" HelpText="Please keep your email address up-to-date, as this is the email we will use to contact you." />
                </td>
                <td width="50%" align="left">
                    <asp:TextBox ID="tbEmail1" runat="server" Width="90%"></asp:TextBox>
                </td>
                <td width="50%" align="left">
                  <asp:TextBox ID="tbEmail2" runat="server" Width="90%"></asp:TextBox>
                </td>
            </tr>
            

       </table>
        <asp:DataList ID="dlProfileProps" runat="server" Width="100%" >
                            <ItemTemplate>
                            <table width="100%" align="left">
                                <tr>
                                    <td width="100px">
                                      
                                        <uc1:labelcontrol ID="lbcPropName" runat="server" Text='<%# GetLocalStaffProfileName(Eval("PropertyName"))%>'
                                            HelpText='<%# GetLocalStaffProfileHelp(Eval("PropertyName")) %>'  Width="100px" />
                                        
                                        <asp:HiddenField ID="hfPropName" runat="server" Value='<%# Eval("PropertyName") %>' />
                                    <asp:HiddenField ID="hfPropType" runat="server" Value='<%# Eval("DataType") %>' />
                                    
                                    </td>
                                    <td width="50%" align="left">
                                        <asp:TextBox ID="tbPropValue1" runat="server" Visible='<%# Eval("DataType")<>359  %>' Text='<%# GetProfileValue(Eval("PropertyName"), False,  Eval("DataType") ) %>' Width="90%" ></asp:TextBox>
                                        <asp:TextBox ID="tbDateValue1" runat="server" Visible='<%# Eval("DataType")=359  %>' Text='<%# GetProfileValue(Eval("PropertyName"), False,  Eval("DataType") ) %>' Width="100px" CssClass="datepicker" ></asp:TextBox>
                                     </td>
                                     <td width="50%" align="left">
                                        <asp:TextBox ID="tbPropValue2" runat="server" Visible='<%# Eval("DataType")<>359 and  hfUserId2.Value >0   %>' Text='<%# GetProfileValue(Eval("PropertyName"), True,  Eval("DataType") ) %>' Width="90%" ></asp:TextBox>
                                        <asp:TextBox ID="tbDateValue2" runat="server" Visible='<%# Eval("DataType")=359 and  hfUserId2.Value >0  %>' Text='<%# GetProfileValue(Eval("PropertyName"), True,  Eval("DataType") ) %>' Width="100px" CssClass="datepicker" ></asp:TextBox>
                                    </td>
                                </tr>
                                </table>
                            </ItemTemplate>
                        </asp:DataList>
     

    </div> 
        <div id='Tab1-tab' style="text-align: center;">
            <table>
                <tr>
                    <td>
                        <uc1:labelcontrol ID="lblResponsibility" runat="server" Width="200px" Text="Responsibility Center" HelpText="Your Financial Responsibility Center (a.k.a Cost Center) - this is the Financial Account that will be used as your personal Cost Center" />
                    </td>
                    <td>
                        <asp:TextBox ID="tbCostCenter" runat="server" Enabled="false" ></asp:TextBox>
                    </td>
                </tr>
            </table>

           <asp:DataList ID="dlStaffProfile" runat="server" DataSourceID="dsStaffProfile" DataKeyField="StaffPropertyDefinitionId" >
                            <ItemTemplate>
                            <table id="tblStaffProfile" runat="server" visible='<%# AmIVisible(  Eval("StaffPropertyDefinitionId"))  %>'>
                                <tr>
                                    <td align="left">
                                        
                                        
                                        <uc1:labelcontrol ID="lbcPropName" runat="server" Text='<%#  Eval("PropertyName") %>'
                                            HelpText='<%# GetTextFromNullable( Eval("PropertyHelp") ,Eval("PropertyName")) %>'  Width="200px" />
                                        <asp:HiddenField ID="hfPropName" runat="server" Value='<%# Eval("PropertyName") %>' />
                                    <asp:HiddenField ID="hfPropType" runat="server" Value='<%# Eval("Type") %>' />
                                    
                                    </td>
                                    <td  align="left">


                                        <asp:TextBox ID="tbPropValue" runat="server" width="250px" Enabled="false" Visible='<%# Eval("Type") =0 %>' Text='<%# GetStaffProfileValue(Eval("AP_StaffBroker_StaffProfiles") ) %>'></asp:TextBox>
                                    <asp:TextBox ID="tbPropValueNumber" runat="server" Width="250px" Enabled="false" Visible='<%# Eval("Type") =1 %>' CssClass="numeric" Text='<%# GetStaffProfileValue(Eval("AP_StaffBroker_StaffProfiles")) %>'></asp:TextBox>
                                        <asp:CheckBox ID="cbProbValue" runat="server" Width="250px"  Enabled="false" Visible='<%# Eval("Type") =2 %>' Checked='<%# GetStaffProfileValueChecked(Eval("AP_StaffBroker_StaffProfiles")) %>' />
                                    
                                    
                                    </td>
                                </tr>
                                </table>
                            </ItemTemplate>
                        </asp:DataList>

                       



                        <asp:LinqDataSource ID="dsStaffProfile" runat="server" EntityTypeName="" ContextTypeName="StaffBroker.StaffBrokerDataContext"
    OrderBy="ViewOrder" TableName="AP_StaffBroker_StaffPropertyDefinitions" Where="PortalId == @PortalId">
    <WhereParameters>
        <asp:ControlParameter ControlID="hfPortalId" Name="PortalId" PropertyName="Value"
            Type="Int32" />
       
    </WhereParameters>
</asp:LinqDataSource>
</div>
        <div id='Tab2-tab'>
       <table align="left">
                        <tr>
                            <td>
                            </td>
                            <td style="white-space: nowrap; width: 350px;">
                                <asp:Label ID="lbl2Name1" runat="server"  CssClass="AgapeH3"  />
                            </td>
                            <td width="30px">
                            </td>
                            <td style="white-space: nowrap; width: 350px;">
                                <asp:Label ID="lbl2Name2" runat="server" CssClass="AgapeH3"  />
                            </td>
                        </tr>
                        <tr valign="top">
                            <td >
                                <uc1:labelcontrol ID="lblReport" runat="server" Width="120px" Text="This staff member reports to:"
                                    HelpText="Enter the person/people that his staff member reports to. They should be setup as a staff member in this portal." />
                            </td>
                            <td>
                                <uc3:leaders ID="Leaders1" runat="server" isReadOnly="True" />
                            </td>
                            <td>
                            </td>
                            <td>
                                <uc3:leaders ID="Leaders2" runat="server"  isReadOnly="True" />
                            </td>
                        </tr>
                        <tr style="height: 15px;">
                            <td colspan="4">
                                &nbsp;
                            </td>
                        </tr>
                        

                         <tr valign="top">
                            <td>
                                <uc1:labelcontrol ID="lblLeading" runat="server"  Width="120px" Text="The following report to this staff member:"
                                    HelpText="These staff report to this staff member. You can remove reporting relationships here, but you must add the reporting relationship on the staff members profile." />
                            </td>
                            <td>
                                <uc4:plebs ID="Plebs1" runat="server" CanRemove="false" />
                            </td>
                            <td>
                            </td>
                            <td>
                                <uc4:plebs ID="Plebs2" runat="server" CanRemove="false"  />
                            </td>
                        </tr>
                        <tr style="height: 15px;">
                            <td colspan="4">
                                &nbsp;
                            </td>
                        </tr>
                        <tr valign="top">
                            <td>
                                <uc1:labelcontrol ID="lblManager" runat="server"  Width="120px" Text="Manager of these Departments:"
                                    HelpText="This user is setup as the Manager, or Delegate Manager for the following Responsibility Centers. A department must always have a manager, so please click on the department to change the manager on the Departments page. " />
                            </td>
                            <td>
                                
                                <uc6:depts ID="Depts1" runat="server" />
                            </td>
                            <td>
                            </td>
                            <td>
                                <uc6:depts ID="Depts2" runat="server" />
                            </td>
                        </tr>
                    </table>






           
        </div>
        <div id='Tab3-tab' style="text-align:center;">
            <div>
             <asp:DataList ID="dlGivingQuestions" runat="server" DataSourceID="dsStaffProfile" DataKeyField="StaffPropertyDefinitionId" Width="100%" >
                            <ItemTemplate>
                            <table id="tblStaffProfile" runat="server" visible='<%# AmIVisibleGiving(  Eval("StaffPropertyDefinitionId"))  %>'>
                                <tr>
                                    <td align="left">
                                        
                                        
                                        <uc1:labelcontrol ID="lbcPropName" runat="server" Text='<%#  Eval("PropertyName") %>'
                                            HelpText='<%# GetTextFromNullable( Eval("PropertyHelp") ,Eval("PropertyName")) %>'  Width="200px" />
                                        <asp:HiddenField ID="hfPropName" runat="server" Value='<%# Eval("PropertyName") %>' />
                                    <asp:HiddenField ID="hfPropType" runat="server" Value='<%# Eval("Type") %>' />
                                    
                                    </td>
                                    <td align="left">


                                        <asp:TextBox ID="tbPropValue" runat="server" Width="250px"  Visible='<%# Eval("Type") =0 %>' Text='<%# GetStaffProfileValue(Eval("AP_StaffBroker_StaffProfiles") ) %>'></asp:TextBox>
                                    <asp:TextBox ID="tbPropValueNumber" runat="server" Width="250px"  Visible='<%# Eval("Type") =1 %>' CssClass="numeric" Text='<%# GetStaffProfileValue(Eval("AP_StaffBroker_StaffProfiles")) %>'></asp:TextBox>
                                        <asp:CheckBox ID="cbProbValue" runat="server"  Visible='<%# Eval("Type") =2 %>' Checked='<%# GetStaffProfileValueChecked(Eval("AP_StaffBroker_StaffProfiles")) %>' />
                                    
                                    
                                    </td>
                                </tr>
                                </table>
                            </ItemTemplate>
                        </asp:DataList>
                 </div>
                <div style="width: 680px; float: left;">
                   <fieldset style="width: 100%; text-align: left">
                <legend class="AgapeH3"><asp:Label ID="lblCustomize" runat="server"></asp:Label></legend>
                <b><asp:Label ID="lblGivePage" runat="server"></asp:Label></b> <span style="font-weight: bold; color: Gray; font-size: 11pt">http://Don'tknowyet.com</span><br />

               
         <p><asp:Label ID="lblGiveInstructions" runat="server"></asp:Label></p>

            <br />
            
            <dnn:TextEditor ID="tbGivingText" runat="server" Height="500" Width="670px"  /><br />
            </fieldset>
                </div>
                <div style="width: 300px; float: right;">
                    <asp:Label ID="lblJointPhoto" runat="server" CssClass="AgapeH5" />                    
                    <uc1:acImage ID="JointPhoto" runat="server" Aspect="1" Width="300" />
                    <br />
                    <asp:Button ID="ProfileButton" runat="server" Text="See Public Page" CssClass="aButton btn" />
                </div>
                <div style="clear: both;">
                </div>
                        <br /><br />

           
        </div>
    </div>
</div>




    <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="aButton btn" Font-Size="Large" />
    <div align="left">
<asp:LinkButton ID="btnSettings" runat="server">Settings</asp:LinkButton>
</div>


</div>

    </asp:Panel>