<%@ Control Language="VB" AutoEventWireup="false" CodeFile="AddStaff.ascx.vb" Inherits="DesktopModules_AgapePortal_StaffBroker_AddStaff" %>

 <%@ Register src="~/controls/labelcontrol.ascx" tagname="labelcontrol" tagprefix="uc1" %>




    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>

<asp:HiddenField ID="hfPortalId" runat="server" />
<br />

        <table align="center">        
            <tr valign="top">
                <td>
                    <uc1:labelcontrol ID="labelcontrol1" runat="server" Text="TheKey Username" HelpText="Enter the staff members TheKey Username. If the user does not have an account on this site, click 'Create' and enter their name." />
                </td>
                <td>
                    <asp:TextBox ID="tbGCXUserName" runat="server" Width="220px"></asp:TextBox>
                    

                    <asp:CheckBox ID="cbCreate1" runat="server" Text="Create?"  onClick="Create1();" />
                    <div ID="pnlCreate1" runat="server">
            
                        <asp:TextBox ID="tbFirstName1" runat="server"></asp:TextBox>
                        
                       <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="tbFirstName1" ErrorMessage="Please enter the user's first name." Display="Dynamic" Text="*"></asp:RequiredFieldValidator>
                      --%>  <asp:TextBox ID="tbLastName1" runat="server"></asp:TextBox>
                        
                       <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="tbLastName1" ErrorMessage="Please enter the user's last name." Display="Dynamic" Text="*"></asp:RequiredFieldValidator>
                       --%> 
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <uc1:labelcontrol ID="labelcontrol8" runat="server" Text="Marital Status" HelpText="Select the Staff Member's marital status." />
                </td>
                <td>
                <asp:DropDownList ID="ddlMaritalStatus" runat="server" Width="220px" onChange="MaritalChange();" >
                    <asp:ListItem Value="-2">Single</asp:ListItem>
                    <asp:ListItem Value="0">Married - Spouse on staff.</asp:ListItem>
                    <asp:ListItem Value="-1">Married - Spouse not on staff.</asp:ListItem>
                </asp:DropDownList>
                </td>
            </tr>
           <tbody  ID="pnlMarriedStaff" runat="server">
            <tr  valign="top" >
                <td>
                    <uc1:labelcontrol ID="labelcontrol4" runat="server" Text="Spouse's GCX Username" HelpText="Enter the wife/husband's GCX Username. If the spouse does not have an account on this site, click 'Create' and enter their name." />
                </td>
                <td>
                    <asp:TextBox ID="tbSpouseGCX" runat="server" Width="220px"></asp:TextBox>
                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbSpouseGCX" ErrorMessage="Please enter a GCX Username for the spouse." Display="Dynamic" Text="*">
                    </asp:RequiredFieldValidator>--%>

                    <asp:CheckBox ID="cbCreate2" runat="server" Text="Create?" onClick="Create2();" />
                    <div ID="pnlCreate2" runat="server">
                        <asp:TextBox ID="tbFirstName2" runat="server"></asp:TextBox>
                        
                     <%--   <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="tbFirstName2" ErrorMessage="Please enter a first name for spouse." Display="Dynamic" Text="*"></asp:RequiredFieldValidator>
                     --%>   
                        <asp:TextBox ID="tbLastName2" runat="server"></asp:TextBox>
                        
                       <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="tbLastName2" ErrorMessage="Please enter a last name for spouse." Display="Dynamic" Text="*"></asp:RequiredFieldValidator>
                       --%> 
                    </div>
                </td>
            </tr>
            </tbody>
    <tbody ID="pnlNonStaff" runat="server">
   
    <tr>
        <td>
            <uc1:labelcontrol ID="labelcontrol5" runat="server" Text="Spouse's First Name" HelpText="Enter the wife/husband's First Name" />
        </td>
        <td>
            <asp:TextBox ID="tbSpouseName" runat="server" Width="220px"></asp:TextBox>
            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbSpouseName" ErrorMessage="Please enter a name for spouse." Display="Dynamic" Text="*">
    </asp:RequiredFieldValidator>--%>
        </td>
    </tr>
    


    <tr>
        <td>
            <uc1:labelcontrol ID="labelcontrol6" runat="server" Text="Spouse's Birthday" HelpText="Select the wife/husband's date of birth." />
        </td>
        <td>
           
             <asp:TextBox ID="dtSpouseDOB" runat="server" Width="90px"  CssClass="datepicker"></asp:TextBox>
      
        </td>
    </tr>
     </tbody> 
    
    <tr>
        <td><uc1:labelcontrol ID="labelcontrol2" runat="server" Text="Staff Type" HelpText="Select the staff type from the list." /></td>
        <td>
            <asp:DropDownList ID="ddlStaffType" runat="server" DataSourceID="dsStaffTypes" 
                DataTextField="Name" DataValueField="StaffTypeId" >
            </asp:DropDownList>
            <asp:LinqDataSource ID="dsStaffTypes" runat="server" 
                ContextTypeName="StaffBroker.StaffBrokerDataContext" EntityTypeName="" 
                OrderBy="StaffTypeId" Select="new (StaffTypeId, Name)" 
                TableName="AP_StaffBroker_StaffTypes" Where="PortalId == @PortalId">
                <WhereParameters>
                    <asp:ControlParameter ControlID="hfPortalId" Name="PortalId" 
                        PropertyName="Value" Type="Int32" />
                </WhereParameters>
            </asp:LinqDataSource>
        </td>
    </tr>
    </table>
   


    <table>
    
<tr>
    <td colspan="2" align="center">
   
        <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" ></asp:Label> <br />
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
        <asp:Button ID="Button1" runat="server" Text="Add Staff Member" class="aButton btn" />
    </td>
</tr>
</table>
   </ContentTemplate>

   <Triggers>
        <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
    </Triggers>
    </asp:UpdatePanel>

