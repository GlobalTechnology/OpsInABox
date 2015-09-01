<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Departments.ascx.vb" Inherits="DesktopModules_AgapePortal_StaffBroker_Depts" %>
<%@ Register src="~/controls/labelcontrol.ascx" TagName="labelcontrol" TagPrefix="uc1" %>

<script type="text/javascript">
    (function ($, Sys) {
        function setUpMyTabs<%= pnlDelegate.ClientId() %>() {
            $('#<%= pnlDelegate.ClientId() %>').dialog({
                autoOpen: false,
                height: 155,
                width: 330,
                modal: true,
                title: "Delegate Department Leadership",
                close: function () {
                    allFields.val("").removeClass("ui-state-error");
                }
            });

            $('#<%= pnlDelegate.ClientId() %>').parent().appendTo($("form:first"));

            
       $('.aButton').button();
    }

        $(document).ready(function () {
            setUpMyTabs<%= pnlDelegate.ClientId() %>();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                setUpMyTabs<%= pnlDelegate.ClientId() %>();
               
            });
        });
    } (jQuery, window.Sys));

    function showPopup<%= pnlDelegate.ClientId %>(x) 
    { 
    $(<%= hfDeptId.ClientId %>).val(x);
        $(<%= pnlDelegate.ClientId %>).dialog("open"); 
        return false; 
    }
    function closePopup<%= pnlDelegate.ClientId %>() { $(<%= pnlDelegate.ClientId %>).dialog("close"); }
     
   

  </script>

<asp:HiddenField ID="hfUserId" runat="server" />
<asp:DataList ID="DataList2" runat="server" DataSource='<%# GetDepartments(hfUserID.value) %>' Width="300px">
    <ItemTemplate>
    <div style="margin-bottom: 3px;">
      
        <asp:LinkButton ID="btnRemoveLeader" runat="server" CommandName="GotoDept" CommandArgument='<%# Eval("CostCenterId")  %>' Text='<%# Eval("Name") %>'></asp:LinkButton>
        
         <asp:Label ID="Panel2" runat="server" Font-Size="X-Small" Visible='<%# not Eval("CostCentreDelegate") is nothing and  Eval("CostCentreDelegate")<>CInt(hfUserID.value)   %>'   >
    
        <a href="#"  onclick="showPopup<%# pnlDelegate.ClientId & "(" & Eval("CostCenterId") & ")" %>" style="color: Gray"   >Delegate</a>
        </asp:Label>
        <asp:Label ID="Label3" runat="server" Font-Size="X-Small" ForeColor="Gray" Visible='<%# Eval("CostCentreDelegate") =  CInt(hfUserID.value) %>' Text='<%# "(delegated from " &  GetLeaderName( Eval("CostCenterId")) & ")" %>' ></asp:Label>

         <asp:Panel ID="Panel1" runat="server" Font-Size="X-Small" Visible='<%# Not( Eval("CostCentreDelegate") is nothing) and  Eval("CostCentreDelegate")<>CInt(hfUserID.value) %>' style="padding-left: 10px;">
         Delegated to -->  <asp:Label ID="lblDelegate" runat="server"  Text='<%# GetDelegateName(Eval("CostCenterId")) %>'  ></asp:Label>
      <asp:LinkButton ID="LinkButton1" runat="server" ForeColor="Gray"  CommandName="RemoveDelegate" CommandArgument='<%# Eval("CostCenterId") %>'>Remove</asp:LinkButton>
      </asp:Panel>
        </div>

    </ItemTemplate>
    
</asp:DataList>


<div id="pnlDelegate" runat="server" style="text-align: center; ">
 <asp:DropDownList ID="ddlDelegate" runat="server" Font-Size="XX-Small"  DataSource='<%#  StaffBrokerFunctions.GetStaff(hfUserId.value ) %>'
                 DataTextField="DisplayName" DataValueField="UserID" AppendDataBoundItems="true">
                 <asp:ListItem Value="0">Not Set</asp:ListItem>
            </asp:DropDownList>
    <asp:HiddenField ID="hfDeptId" runat="server" />

            <br />
    <asp:CheckBox ID="CheckBox1" runat="server" Checked="false" resourcekey="cbDelegate" Text="Delegate all my undelegated Departments to this staff member" />



 <br /><br />
    <asp:Label ID="Label1" runat="server" ForeColor="Red" Font-Italic="true"></asp:Label>
 <asp:Button ID="btnDelegate" resourcekey="btnDelegate" runat="server" Text="Delegate" Width="100px" CssClass="aButton btn"   />
</div>