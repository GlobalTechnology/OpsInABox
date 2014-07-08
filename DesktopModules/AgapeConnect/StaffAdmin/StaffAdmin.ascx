<%@ Control Language="VB" AutoEventWireup="false" CodeFile="StaffAdmin.ascx.vb" Inherits="DotNetNuke.Modules.StaffAdmin.ViewStaffAdmin" %>
<%@ Register Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" TagPrefix="cc1" %>

<%@ Register Src="~/controls/labelcontrol.ascx" TagName="labelcontrol" TagPrefix="uc1" %>
<%@ Register Src="Controls/StaffChildren.ascx" TagName="StaffChildren" TagPrefix="uc2" %>
<%@ Register Src="Controls/Leaders.ascx" TagName="Leaders" TagPrefix="uc3" %>
<%@ Register Src="Controls/Plebs.ascx" TagName="Plebs" TagPrefix="uc4" %>
<%@ Register Src="Controls/Departments.ascx" TagName="Depts" TagPrefix="uc6" %>

<%@ Register src="Controls/AddStaff.ascx" tagname="AddStaff" tagprefix="uc5" %>
<script src="/js/jquery.watermarkinput.js" type="text/javascript"></script>
<script src="/js/jquery.numeric.js" type="text/javascript"></script>
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
            var pickerOpts = {
                dateFormat: '<%= GetDateFormat() %>'
            };

            $("#AddStaffPopup").dialog({
                autoOpen: false,
                height: 350,
                width: 550,
                modal: true,
                title: "Add Staff",
                close: function () {
                    allFields.val("").removeClass("ui-state-error");
                }
            });
            $("#AddStaffPopup").parent().appendTo($("form:first"));

         

            $("#SelectSpouse").dialog({
                autoOpen: false,
                height: 350,
                width: 550,
                modal: true,
                title: "Select Staff Spouse",
                close: function () {
                    allFields.val("").removeClass("ui-state-error");
                }
            });
            $("#SelectSpouse").parent().appendTo($("form:first"));


            $('.datepicker').datepicker(pickerOpts);
            $('.aButton').button();
            $('.numeric').numeric();
            $('#<%= GetAddStaffClientId("tbFirstName1") %>').Watermark('First name');
            $('#<%= GetAddStaffClientId("tbFirstName2") %>').Watermark('First name');
            $('#<%= GetAddStaffClientId("tbLastName1") %>').Watermark('Last name');
            $('#<%= GetAddStaffClientId("tbLastName2") %>').Watermark('Last name');
            <%= IIf(StaffBrokerFunctions.GetSetting("ZA-Mode", PortalId) = "True", "", "removeTab(3);  ")%>
          
        }

        $(document).ready(function () {
            setUpMyTabs();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                setUpMyTabs();
            });
        });
    } (jQuery, window.Sys));

    function showPopup() { $("#AddStaffPopup").dialog("open");MaritalChange();Create1();Create2();         return false; }
    function closePopup() { $("#AddStaffPopup").dialog("close"); }
  
    function showSpousePopup() { $("#SelectSpouse").dialog("open");  return false; }
    function closeSpousePopup() { $("#SelectSpouse").dialog("close"); }
    function ddlUpdate() {
        var e = document.getElementById('<%= GetClientIdForm("ddlMaritalStatus") %>'); // select element
        if (e == null)
            
            return;

        var cid = e.options[e.selectedIndex].value;

        if (cid == 0) {
            
            document.getElementById('<%=GetClientIdForm("lblName2")%>').style.display = "";
            document.getElementById('<%=GetClientIdForm("tbName2")%>').style.display = "none";
            document.getElementById('<%=GetClientIdForm("hlEditProfile2")%>').style.display = "";
            document.getElementById('<%=GetClientIdForm("btnImpersonateSpouse")%>').style.display = "";
            document.getElementById('<%=GetClientIdForm("hlNewSpouse")%>').style.display = "";

        }
        else if (cid == -1) {
           
            document.getElementById('<%=GetClientIdForm("lblName2")%>').style.display = "none";
            document.getElementById('<%=GetClientIdForm("tbName2")%>').style.display = "";
            document.getElementById('<%=GetClientIdForm("hlEditProfile2")%>').style.display = "none";
            document.getElementById('<%=GetClientIdForm("btnImpersonateSpouse")%>').style.display = "none";
            document.getElementById('<%=GetClientIdForm("hlNewSpouse")%>').style.display = "none";
        }
        else {
            
            document.getElementById('<%=GetClientIdForm("lblName2")%>').style.display = "none";
            document.getElementById('<%=GetClientIdForm("tbName2")%>').style.display = "none";
            document.getElementById('<%=GetClientIdForm("hlEditProfile2")%>').style.display = "none";
            document.getElementById('<%=GetClientIdForm("btnImpersonateSpouse")%>').style.display = "none";
            document.getElementById('<%=GetClientIdForm("hlNewSpouse")%>').style.display = "none";
        }


    }

    function removeTab(i) {
        var tab = $( "#tabs" ).find( ".ui-tabs-nav li:eq(" + i +")" ).remove();
        var panelId = tab.attr( "aria-controls" );
        // Remove the panel
        $( "#" + panelId ).remove();
        // Refresh the tabs widget
        $( "#tabs" ).tabs( "refresh" );
        // $('#Tab3-tab').remove  $('#tabs').tabs("disable", i);
    }

    function MaritalChange() {
     
        if ($('#<%= GetAddStaffClientId("ddlMaritalStatus")  %>').val() == "-1") {
           $('#<%= GetAddStaffClientId("pnlNonStaff")  %>').show();
            $('#<%= GetAddStaffClientId("pnlMarriedStaff")  %>').hide();
        } else if ($('#<%= GetAddStaffClientId("ddlMaritalStatus")  %>').val() == "0") {
        
            $('#<%= GetAddStaffClientId("pnlNonStaff")  %> ').hide();
            $('#<%= GetAddStaffClientId("pnlMarriedStaff")  %>').show();
           
        } else {
            $('#<%= GetAddStaffClientId("pnlNonStaff")  %>').hide();
            $('#<%= GetAddStaffClientId("pnlMarriedStaff") %>').hide();
        }
    }
    function Create1() {
        if ($('#<%= GetAddStaffClientId("cbCreate1")  %>').attr('checked') == 'checked') {
            
            $('#<%= GetAddStaffClientId("pnlCreate1")  %>').show();
        }
        else {
            $('#<%= GetAddStaffClientId("pnlCreate1")  %>').hide();
        }
    }
    function Create2() {
        if ($('#<%= GetAddStaffClientId("cbCreate2")  %>').attr('checked') == 'checked') {
            $('#<%= GetAddStaffClientId("pnlCreate2")  %>').show();
        }
        else {
            $('#<%= GetAddStaffClientId("pnlCreate2")  %>').hide();
        }
    }

</script>
<div style="width: 100%;">

<asp:HiddenField ID="hfPortalId" runat="server" />
<asp:HiddenField ID="theHiddenTabIndex" runat="server" value="0" ViewStateMode="Enabled" />

<asp:Label runat="server" Text="Staff Administration" CssClass="AgapeH2"></asp:Label>
<table>
    
    <tr>
        
        <td>
            <uc1:labelcontrol ID="lblSelectStaffMember" runat="server" Text="Select a Staff Member:"
                HelpText="Select a Staff Member/Couple from the list" />
        </td>
        <td>
            <asp:DropDownList ID="ddlStaff" runat="server" AutoPostBack="true"> </asp:DropDownList>
        </td>
        <td>
                           
        </td>
    </tr>
    
</table>


<asp:Label ID="lblWarning" runat="server" Font-Italic="true" ForeColor="Gray">Don't forget to click update (bottom right) to save your changes.</asp:Label>

<asp:FormView ID="FormView1" runat="server" DataKeyNames="StaffId" DataSourceID="dsStaffDetail"
     DefaultMode="Edit"   RowStyle-HorizontalAlign ="Center" Width="100%"  >
    <EditItemTemplate>
        <div id="tabs" style="width: 90%; text-align: Left;" >
            <ul >
                <li><a href='#Tab1-tab'>Personal Details</a></li>
                <li><a href='#Tab2-tab'>Staff Details</a></li>
              
                <li><a href='#Tab3-tab'>Leadership Relationships</a></li>
                  <li><a href='#Tab2a-tab'>Payroll</a></li>
            </ul>
            <div style="width: 100%;  min-height: 350px; background-color: #FFFFFF;">
                <div id='Tab1-tab'   >
                    <table>
                        <tr style="height: 60px;">
                            <td width="100px" >
                                <uc1:labelcontrol ID="labelcontrol4" runat="server"  Text="Display Name:" HelpText="Enter a display name for this staff member/couple. This is the name by which they will be referred in this portal"  />
                            </td>
                            
                            <td colspan="3">
                                <asp:TextBox ID="DisplayNameTextBox" runat="server" Font-Bold="true" Width="400px"
                                    Font-Size="16pt" Text='<%# Bind("DisplayName") %>' />
                            </td>
                            
                        </tr>
                        <tr>
                            <!-- NAMES -->
                            <td>
                            </td>
                            <td style="white-space: nowrap; width: 50%;">
                                <div style="float: left ; margin-right: 10px;">
                                    <asp:Image ID="ProfileImage" runat="server" width="100px" ImageUrl= '<%# GetPhoto(Eval("UserId1")) %>' />
                                 </div>
                                <asp:Label ID="lblName1"  CssClass="AgapeH3" runat="server" Font-Size="20pt" Font-Bold="true" Text='<%# Eval("User.FirstName") %>' />
                               <br />
                                <asp:HyperLink ID="hlEditProfile1" runat="server"  NavigateUrl='<%# getEditProfileUrl(Eval("UserId1")) %>'>Edit Profile</asp:HyperLink>
                                 &nbsp; &nbsp;
                                <asp:LinkButton ID="btnImpersonateUser" runat="server" CommandName="Impersonate" CommandArgument='<%# Eval("UserId1") %>'>Impersonate</asp:LinkButton>
                                
                                
                                <div style="clear: both ;"></div>
                            </td>
                            <td width="50px" style="min-width: 50px !important;" >
                            &nbsp;  
                            </td>
                            <td style="white-space: nowrap; width: 50%;">
                                <div style="float: left ; margin-right: 10px;">
                                    <asp:Image ID="Image1" runat="server" width="100px" Visible='<%# Eval("UserId2")>0 %>' ImageUrl='<%# GetPhoto(Eval("UserId2")) %>' />
                                 </div>
                                <asp:Label ID="lblName2"  CssClass="AgapeH3" runat="server" Font-Size="20pt" Font-Bold="true" Text='<%# Eval("User2.FirstName") %>' />
                                
                                <asp:HyperLink ID="hlEditProfile2" runat="server"  NavigateUrl='<%# getEditProfileUrl(Eval("UserId2")) %>'>Edit Profile</asp:HyperLink>
                               <asp:LinkButton ID="btnImpersonateSpouse" runat="server" CommandName="Impersonate" CommandArgument='<%# Eval("UserId2") %>'>Impersonate</asp:LinkButton>
                                
                                <asp:TextBox ID="tbName2"  CssClass="AgapeH3" runat="server"  Font-Size="20pt" Font-Bold="true" Text='<%# StaffBrokerFunctions.GetStaffProfileProperty(Eval("StaffId"), "SpouseName") %>' />
                            </td>
                        </tr>
                        <tr>
                            <!-- Marital Status -->
                            <td >
                                <uc1:labelcontrol ID="labelcontrol2" runat="server" Text="Marital Status:" HelpText="Please select this staff members Marital Status" />
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlMaritalStatus" runat="server" Width="200px" AutoPostBack="false"
                                    SelectedValue='<%# ConvertMaritalStatus(Eval("UserId2")) %>' onchange="ddlUpdate();">
                                    <asp:ListItem Value="-2">Single</asp:ListItem>
                                    <asp:ListItem Value="0">Married - Spouse on staff.</asp:ListItem>
                                    <asp:ListItem Value="-1">Married - Spouse not on staff.</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td >
                            </td>
                            <td>
                                <asp:HyperLink ID="hlNewSpouse" runat="server" onclick="showSpousePopup();">Select New Spouse   (?!)</asp:HyperLink>
                              
                            </td>
                        </tr>
                        <tr>
                            <!-- DOB -->
                            <td >
                                <uc1:labelcontrol ID="labelcontrol1" runat="server" Text="Birthday:" HelpText="The Staff Member's Date of Birth" />
                            </td>
                            <td>
                                <asp:TextBox ID="tbUserDate" CssClass="datepicker" runat="server" width="100px" Text='<%# GetBirthday(Eval("UserId1"))  %>' />
                                
                                
                               
                            </td>
                            <td >
                            </td>
                            <td>
                                <asp:TextBox ID="tbSpouseDateStaff" runat="server"  CssClass="datepicker"  width="100px"  Text='<%# GetBirthday(Eval("UserId2"))  %>'
                                    Visible='<%# Eval("UserId2")>0 %>' />
                              
                                <asp:TextBox ID="tbSpouseDateNotStaff" runat="server"  CssClass="datepicker"  width="100px"  Text='<%# StaffBrokerFunctions.GetStaffProfileProperty(Eval("StaffId"), "SpouseDOB") %>'
                                    Visible='<%# Eval("UserId2")=-1 %>' />
                               
                            </td>
                        </tr>
                        <tr>
                            <!-- NAMES -->
                            <td >
                                <uc1:labelcontrol ID="labelcontrol3" runat="server" Text="Email:" HelpText="The Staff Member's Email" />
                            </td>
                            <td     >
                                <asp:TextBox ID="tbEmail" runat="server" Width="100%" Text='<%# Eval("User.Email") %>' />
                            </td>
                            <td >
                            </td>
                            <td>
                                <asp:TextBox ID="tbEmailSpouse" runat="server" Width="100%" Text='<%# Eval("User2.Email") %>'
                                    Visible='<%# Eval("UserId2")>0 %>' />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <br />
                            </td>
                        </tr>
                        <tr valign="top">
                            <td>
                                <uc1:labelcontrol ID="labelcontrol5" runat="server" Text="Children:" HelpText="Enter child names and ages here" />
                            </td>
                            <td colspan="3">
                                <uc2:StaffChildren ID="StaffChildren1" runat="server" StaffId='<%# Eval("StaffId") %>' />
                            </td>
                        </tr>
                    </table>
                    <br />
                </div>
               
                <div id='Tab3-tab' >
                    Use this panel to define leadership relationships. This will affect who can approve
                    person reimbursements etc...<br />
                    <table>
                        <tr>
                            <td>
                            </td>
                            <td  style="white-space: nowrap; width: 100%;">
                                <asp:Label ID="Label2" runat="server"  CssClass="AgapeH3"  Text='<%# Eval("User.FirstName") %>' />
                            </td>
                            <td style="min-width: 50px !important;">
                            </td>
                            <td style="white-space: nowrap; width: 100%; min-width: 350px!important;" >
                                <asp:Label ID="lblSpouseName1" runat="server" CssClass="AgapeH3" Text='<%# Eval("User2.FirstName") %>' />
                                <asp:Label ID="lblSoiuseName2" runat="server"  CssClass="AgapeH3"  Text='<%# StaffBrokerFunctions.GetStaffProfileProperty(Eval("StaffId"), "SpouseName")   %>' />
                            </td>
                        </tr>
                        <tr valign="top">
                            <td>
                                <uc1:labelcontrol ID="labelcontrol7" runat="server" Width="120px"  Text="This staff member reports to:"
                                    HelpText="Enter the person/people that his staff member reports to. They should be setup as a staff member in this portal." />
                            </td>
                            <td>
                                <uc3:Leaders ID="Leaders1" runat="server" UID='<%# Eval("UserId1") %>' />
                            </td>
                            <td>
                            </td>
                            <td>
                                <uc3:Leaders ID="Leaders2" runat="server" UID='<%# Eval("UserId2") %>' Visible='<%# Eval("UserId2")>0 %>' />
                            </td>
                        </tr>
                        <tr style="height: 15px;">
                            <td colspan="4">
                                &nbsp;
                            </td>
                        </tr>
                        

                         <tr valign="top">
                            <td>
                                <uc1:labelcontrol ID="labelcontrol8" runat="server"  Width="120px" Text="The following report to this staff member:"
                                    HelpText="These staff report to this staff member. You can remove reporting relationships here, but you must add the reporting relationship on the staff members profile." />
                            </td>
                            <td>
                                <uc4:Plebs ID="Leaders3" runat="server" UID='<%# Eval("UserId1") %>' />
                            </td>
                            <td>
                            </td>
                            <td>
                                <uc4:Plebs ID="Leaders4" runat="server" UID='<%# Eval("UserId2") %>' Visible='<%# Eval("UserId2")>0 %>' />
                            </td>
                        </tr>
                        <tr style="height: 15px;">
                            <td colspan="4">
                                &nbsp;
                            </td>
                        </tr>
                        <tr valign="top">
                            <td>
                                <uc1:labelcontrol ID="labelcontrol9" runat="server"  Width="120px" Text="Manager of these Departments:"
                                    HelpText="This user is setup as the Manager, or Delegate Manager for the following Responsibility Centers. A department must always have a manager, so please click on the department to change the manager on the Departments page. " />
                            </td>
                            <td>
                                
                                <uc6:Depts ID="Depts1" runat="server" UID='<%# Eval("UserId1") %>' />
                            </td>
                            <td>
                            </td>
                            <td>
                                <uc6:Depts ID="Depts2" runat="server" UID='<%# Eval("UserId2") %>' Visible='<%# Eval("UserId2")>0 %>' />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id='Tab2-tab' >
                    <table  >
                        <tr>
                            <td>
                                <uc1:labelcontrol ID="labelcontrol6" runat="server" Text="Responsibility Center"
                                    HelpText="Enter their chargable Responsibility Center (cost centre) here" Width="200px" />
                            </td>
                            <td colspan="1">
                                
                                <asp:DropDownList ID="DropDownList1" runat="server" Visible='<%# StaffBrokerFunctions.GetSetting("NonDynamics", PortalId)<> "True"%>' SelectedValue='<%# StaffBrokerFunctions.ValidateCostCenter(Eval("CostCenter"), PortalId) %>'
                                    DataSourceID="dsCostCenters" DataTextField="DisplayName" DataValueField="CostCentreCode"
                                    AppendDataBoundItems="true">
                                    <asp:ListItem Text="" Value="" />
                                </asp:DropDownList>
                                 
                                <asp:TextBox ID="tbCostCentreCode" runat="server" Visible='<%# StaffBrokerFunctions.GetSetting("NonDynamics", PortalId) = "True"%>' Text='<%# Eval("CostCenter")%>'></asp:TextBox>

                                </td><td align="left">
                               
                                <table>
                                <tr>
                                <td><asp:CheckBox ID="cbPayOnly" runat="server" Checked= '<%#  IIF(StaffBrokerFunctions.GetStaffProfileProperty(Eval("StaffId"), "PayOnly")="", false, StaffBrokerFunctions.GetStaffProfileProperty(Eval("StaffId"), "PayOnly"))  %>' /></td>
                                <td width="100px">
                                <uc1:labelcontrol ID="labelcontrol11" runat="server" Text="Payment Only" Width="100px" style="text-align: left; max-width: 100px !important;"
                                    HelpText="Centrally Funded staff are sometimes paid through a Pay Only Responsibility Center. You cannat reimburse from or give to a PayOnly Responsiblity Center." />
                           <td width="100%"></td>   
                           </td>
                           </tr>
                            </table>
            


                                <asp:LinqDataSource ID="dsCostCenters" runat="server" ContextTypeName="StaffBroker.StaffBrokerDataContext"
                                    EntityTypeName="" OrderBy="@Type Desc,CostCentreCode" Select="new (CostCentreCode,CostCentreCode + ' ' + '-' + ' ' + CostCentreName as DisplayName)"
                                    TableName="AP_StaffBroker_CostCenters" Where="PortalId == @PortalId">
                                    <WhereParameters>
                                        <asp:ControlParameter ControlID="hfPortalId" DefaultValue="-1" Name="PortalId" PropertyName="Value"
                                            Type="Int32" />
                                        <asp:Parameter DefaultValue="1" Name="Type" Type="Byte" />
                                    </WhereParameters>
                                </asp:LinqDataSource>
                            </td>
                            
                        </tr>
                        <tr>
                            <td>
                            <uc1:labelcontrol ID="labelcontrol10" runat="server" Text="...or Personal Account Code:"
                                    HelpText="Centrally funded staff use are paid reimbursements using a personal account code"
                                    Width="200px" />
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlPAC" runat="server" DataSourceID="dsAssets" DataTextField="DisplayName"
                                    DataValueField="AccountCode" AppendDataBoundItems="true" SelectedValue='<%# StaffBrokerFunctions.ValidateAccountCode(StaffBrokerFunctions.GetStaffProfileProperty(Eval("StaffId"), "PersonalAccountCode"), PortalId) %>'>
                                    <asp:ListItem Text="" Value="" />
                                </asp:DropDownList>
                                <asp:LinqDataSource ID="dsAssets" runat="server" ContextTypeName="StaffBroker.StaffBrokerDataContext"
                                    EntityTypeName="" OrderBy="AccountCode" TableName="AP_StaffBroker_AccountCodes"
                                    Select="new (AccountCode,AccountCode + ' ' + '-' + ' ' + AccountCodeName as DisplayName)"
                                    Where="AccountCodeType == @AccountCodeType &amp;&amp; PortalId == @PortalId">
                                    <WhereParameters>
                                        <asp:Parameter DefaultValue="1" Name="AccountCodeType" Type="Byte" />
                                        <asp:ControlParameter ControlID="hfPortalId" DefaultValue="-1" Name="PortalId" PropertyName="Value"
                                            Type="Int32" />
                                    </WhereParameters>
                                </asp:LinqDataSource>
                            </td>
                            <td width="100%">
                            <asp:Label ID="lblCentralOnly" runat="server" Font-Italic="true" ForeColor="Gray">* Only applies to Centrally Funded Staff</asp:Label>

                                
                            </td>
                        </tr>
                         <tr>
                            <td>
                                <uc1:labelcontrol ID="labelcontrol12" runat="server" Text="Staff Type" HelpText="Enter their Staff Type here. You can edit the list of Staff Types, using the link at the bottom of this page."  Width="200px"/>
                            </td>
                            <td>
                                <asp:DropDownList ID="DropDownList2" runat="server" DataSourceID="dsStaffTypes" 
                                    DataTextField="Name" DataValueField="StaffTypeId" SelectedValue='<%# Bind("StaffTypeId") %>'>
                                </asp:DropDownList>
                                


                                
                                <asp:LinqDataSource ID="dsStaffTypes" runat="server" 
                                    ContextTypeName="StaffBroker.StaffBrokerDataContext" EntityTypeName="" 
                                    TableName="AP_StaffBroker_StaffTypes" Where="PortalId == @PortalId">
                                    <WhereParameters>
                                        <asp:ControlParameter ControlID="hfPortalId" DefaultValue="-1" Name="PortalId" 
                                            PropertyName="Value" Type="Int32" />
                                    </WhereParameters>
                                </asp:LinqDataSource>
                                


                                
                            </td>
                        </tr>
                    </table>
                        
                        <asp:DataList ID="DataList1" runat="server" DataSourceID="dsStaffProfile" Width="100%" DataKeyField="StaffPropertyDefinitionId">
                            <ItemTemplate>
                            <table>
                                <tr>
                                    <td>
                                        
                                        
                                        <uc1:labelcontrol ID="lbcPropName" runat="server" Text='<%# Eval("PropertyName") %>'
                                            HelpText='<%# Eval("PropertyHelp") %>'  Width="200px" />
                                        <asp:HiddenField ID="hfPropName" runat="server" Value='<%# Eval("PropertyName") %>' />
                                    <asp:HiddenField ID="hfPropType" runat="server" Value='<%# Eval("Type") %>' />
                                    
                                    </td>
                                    <td>


                                        <asp:TextBox ID="tbPropValue" runat="server"  Width="250px"  Visible='<%# Eval("Type") =0 %>' Text='<%# GetProfileValue(Eval("AP_StaffBroker_StaffProfiles") ) %>'></asp:TextBox>
                                    <asp:TextBox ID="tbPropValueNumber" runat="server"  Width="250px"  Visible='<%# Eval("Type") =1 %>' CssClass="numeric" Text='<%# GetProfileValue(Eval("AP_StaffBroker_StaffProfiles")) %>'></asp:TextBox>
                                        <asp:CheckBox ID="cbProbValue" runat="server" Visible='<%# Eval("Type") =2 %>' Checked='<%# GetProfileValueChecked(Eval("AP_StaffBroker_StaffProfiles")) %>' />
                                    
                                    
                                    </td>
                                </tr>
                                </table>
                            </ItemTemplate>
                        </asp:DataList>
                 
                    <br />
                </div>
                 <div id='Tab2a-tab' >
                    <table>
                       <tr>
                            <!-- NAMES -->
                            <td style="width:100px"> &nbsp;
                            </td>
                            <td style="white-space: nowrap; width: 50%;">
                                
                                <asp:Label ID="Label1"  CssClass="AgapeH3" runat="server" Font-Size="20pt" Font-Bold="true" Text='<%# Eval("User.FirstName") %>' />
                                
                                
                                <div style="clear: both ;"></div>
                            </td>
                            <td width="50px" style="min-width: 50px !important;" >
                            &nbsp;  
                            </td>
                            <td style="white-space: nowrap; width: 50%;">
                                
                                <asp:Label ID="Label3"  CssClass="AgapeH3" runat="server" Font-Size="20pt" Font-Bold="true" Text='<%# Eval("User2.FirstName") %>' />
                                
                               
                                
                            </td>
                        </tr>
                        
                        
                    </table>

                    <h4>Payroll</h4>
                            <asp:DataList ID="dlPayroll" runat="server" Width="100%" DataSource='<%# GetPayroll("Payroll")%>'>
                                <ItemTemplate>
                                    <table width="100%" align="left">
                                        <tr>
                                            <td width="100px">

                                                <uc1:labelcontrol ID="lbcPropName" runat="server" Text='<%# GetLocalStaffProfileName(Eval("PropertyName")) %>'
                                                    HelpText='<%# GetLocalStaffProfileHelp(Eval("PropertyName"))  %>' Width="100px" />

                                                <asp:HiddenField ID="hfPropName" runat="server" Value='<%# Eval("PropertyName") %>' />
                                                <asp:HiddenField ID="hfPropType" runat="server" Value='<%# Eval("DataType") %>' />
                                            </td>
                                            <td width="50%" align="left">
                                                <asp:TextBox ID="tbPropValue1" runat="server"  Text='<%# GetProfileValue(Eval("PropertyName"), False,  Eval("DataType") ) %>' Width="90%"></asp:TextBox>
                                            </td>
                                            <td width="50%" align="left">
                                                <asp:TextBox ID="tbPropValue2" runat="server" Visible='<%# IsMarriedStaff() %>'  Text='<%# GetProfileValue(Eval("PropertyName"), True,  Eval("DataType") ) %>' Width="90%"></asp:TextBox>
                                              
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:DataList>
                    <h4>Payroll-Deductions</h4>
                            <asp:DataList ID="dlPayrollDeductions" runat="server" Width="100%" DataSource='<%# GetPayroll("Payroll-Deductions") %>'>
                                <ItemTemplate>
                                    <table width="100%" align="left">
                                        <tr>
                                            <td width="100px">

                                                <uc1:labelcontrol ID="lbcPropName" runat="server" Text='<%# GetLocalStaffProfileName(Eval("PropertyName")) %>'
                                                    HelpText='<%# GetLocalStaffProfileHelp(Eval("PropertyName"))  %>' Width="100px" />

                                                <asp:HiddenField ID="hfPropName" runat="server" Value='<%# Eval("PropertyName") %>' />
                                                <asp:HiddenField ID="hfPropType" runat="server" Value='<%# Eval("DataType") %>' />
                                            </td>
                                            <td width="50%" align="left">
                                                <asp:TextBox ID="tbPropValue1" runat="server"  Text='<%# GetProfileValue(Eval("PropertyName"), False,  Eval("DataType") ) %>' Width="90%"></asp:TextBox>
                                            </td>
                                            <td width="50%" align="left">
                                                <asp:TextBox ID="tbPropValue2" runat="server" Visible='<%# IsMarriedStaff() %>'  Text='<%# GetProfileValue(Eval("PropertyName"), True,  Eval("DataType") ) %>' Width="90%"></asp:TextBox>
                                              
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:DataList>
                    <h4>Payroll-Earnings</h4>
                            <asp:DataList ID="dlPayrollEarnings" runat="server" Width="100%" DataSource='<%# GetPayroll("Payroll-Earnings") %>'>
                                <ItemTemplate>
                                    <table width="100%" align="left">
                                        <tr>
                                            <td width="100px">

                                                <uc1:labelcontrol ID="lbcPropName" runat="server" Text='<%# GetLocalStaffProfileName(Eval("PropertyName")) %>'
                                                    HelpText='<%# GetLocalStaffProfileHelp(Eval("PropertyName"))  %>' Width="100px" />

                                                <asp:HiddenField ID="hfPropName" runat="server" Value='<%# Eval("PropertyName") %>' />
                                                <asp:HiddenField ID="hfPropType" runat="server" Value='<%# Eval("DataType") %>' />
                                            </td>
                                            <td width="50%" align="left">
                                                <asp:TextBox ID="tbPropValue1" runat="server"  Text='<%# GetProfileValue(Eval("PropertyName"), False,  Eval("DataType") ) %>' Width="90%"></asp:TextBox>
                                            </td>
                                            <td width="50%" align="left">
                                                <asp:TextBox ID="tbPropValue2" runat="server" Visible='<%# IsMarriedStaff() %>'  Text='<%# GetProfileValue(Eval("PropertyName"), True,  Eval("DataType") ) %>' Width="90%"></asp:TextBox>
                                              
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:DataList>


                        
                </div>
            </div>
           
        </div>
         <div style="bottom: 70px; text-align: right; width: 90%; background-color: #FFFFFF; ">
                <asp:Button ID="UpdateButton" runat="server"  CausesValidation="false"
                    CommandName="Update" Text="Update" style="margin: 5px;"  Font-Bold="true"
                    Font-Size="15pt" class="aButton btn btn-success" />
            </div>
    </EditItemTemplate>
    <ItemTemplate>
        &nbsp;
    </ItemTemplate>
</asp:FormView>
<asp:LinqDataSource ID="dsStaffDetail" runat="server" ContextTypeName="StaffBroker.StaffBrokerDataContext"
    EnableUpdate="True" EntityTypeName="" TableName="AP_StaffBroker_Staffs" Where="StaffId == @StaffId">
    <WhereParameters>
        <asp:ControlParameter ControlID="ddlStaff" Name="StaffId" PropertyName="SelectedValue"
            Type="Int32" DefaultValue="-1" />
    </WhereParameters>
</asp:LinqDataSource>
<asp:LinqDataSource ID="dsStaffProfile" runat="server" EntityTypeName="" ContextTypeName="StaffBroker.StaffBrokerDataContext"
    OrderBy="ViewOrder" TableName="AP_StaffBroker_StaffPropertyDefinitions" Where="PortalId == @PortalId &amp;&amp; Display == @Display">
    <WhereParameters>
        <asp:ControlParameter ControlID="hfPortalId" Name="PortalId" PropertyName="Value"
            Type="Int32" />
        <asp:Parameter DefaultValue="true" Name="Display" Type="Boolean" />
    </WhereParameters>
</asp:LinqDataSource>



 <input type="button" value="Add Staff"  onclick="showPopup();" class="aButton btn btn-primary" style="font-size: 8pt" />
           

<div id="AddStaffPopup">
 
<uc5:AddStaff ID="AddStaff1" runat="server" />

</div>



<div id="SelectSpouse">
 
    <table border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                 <uc1:labelcontrol ID="labelcontrol1" runat="server" Text="GCX Username" HelpText="Enter the spouse's GCX Username. " />
               
             
            </td>
            <td>
             <asp:TextBox ID="tbGCXUserName" runat="server" Width="220px"></asp:TextBox>
                    
            </td>
        </tr>
        <tr>
            <td>
             <uc1:labelcontrol ID="labelcontrol13" runat="server" Text="First Name" HelpText="Enter the Spouse's first name. " />
               
            </td>
            <td>
                  <asp:TextBox ID="tbFirstName" runat="server"></asp:TextBox>
            </td>
        </tr>
         <tr>
    <td colspan="2" align="center">
   
        <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" ></asp:Label> <br />
      
        <asp:Button ID="btnAddSpouse" runat="server" Text="Add Spouse" class="aButton btn" />
    </td>
</tr>
    </table>
   
</div>

&nbsp; &nbsp;
    <asp:LinkButton ID="btnBulkAdd" runat="server">Upload Staff from File</asp:LinkButton>  &nbsp; &nbsp;

<asp:HyperLink ID="hlStaffProfileProps" runat="server" >Edit Staff Profile Properties</asp:HyperLink> &nbsp; &nbsp;
<asp:LinkButton ID="btnRepRel" runat="server">Staff Reporting Relationships</asp:LinkButton>  &nbsp; &nbsp;
<asp:LinkButton ID="btnTnT" runat="server">Refresh All TnT WebUsers</asp:LinkButton>  &nbsp; &nbsp;
<asp:LinkButton ID="btnChangeUsername" runat="server">Change Username</asp:LinkButton> &nbsp; &nbsp;
<asp:LinkButton ID="btnRCReport" runat="server">RC-Report</asp:LinkButton>
</div>

