<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Plebs.ascx.vb" Inherits="DesktopModules_AgapePortal_StaffBroker_Plebs" %>
<%@ Register src="~/controls/labelcontrol.ascx" TagName="labelcontrol" TagPrefix="uc1" %>
<script type="text/javascript">
    (function ($, Sys) {
        function setUpMyTabs<%= pnlDelegate.ClientId() %>() {
           
            $('#<%= pnlDelegate.ClientId() %>').dialog({
                autoOpen: false,
                height: 155,
                width: 500,
                modal: true,
                title: "Delegate Leadership Relationship",
                close: function () {
                    allFields.val("").removeClass("ui-state-error");
                }
            });

            $('#<%= pnlDelegate.ClientId() %>').parent().appendTo($("form:first"));

            
      // $('.aButton').button();
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
    $(<%= hfPlebId.ClientId %>).val(x);
        $(<%= pnlDelegate.ClientId %>).dialog("open"); 
        return false; 
    }
    function closePopup<%= pnlDelegate.ClientId %>() { $(<%= pnlDelegate.ClientId %>).dialog("close"); }
     
   

  </script>

<asp:HiddenField ID="hfUserId" runat="server" />
<asp:DataList ID="DataList2" runat="server" DataSource='<%# StaffBrokerFunctions.GetTeam(hfUserID.value) %>' Width="300px">
    <ItemTemplate>
    <div style="margin-bottom: 3px;">
    <asp:Label ID="lblLeaderName" runat="server" Font-Bold="true"  Text='<%# Eval("FirstName") & " " & Eval("LastName") %>'  ></asp:Label>
   <asp:Label ID="Label3" runat="server" Font-Size="X-Small" ForeColor="Gray" Visible='<%# AmIDelegate(Eval("UserId")) %>' Text='<%# "(delegated from " &  GetLeaderName( Eval("UserId")) & ")" %>' ></asp:Label>

   &nbsp; &nbsp;   <asp:LinkButton ID="btnRemovePleb" runat="server" ForeColor="Gray" Font-Size="X-Small" Visible='<%# CanRemove() And Not IsDelegated(Eval("UserId")) %>'   CommandName="RemovePleb" CommandArgument='<%# Eval("UserId") %>'>Remove</asp:LinkButton>
        <asp:Label ID="Panel2" runat="server" Font-Size="X-Small" ResourceKey="btnDelegate" Visible='<%# Not IsDelegated(Eval("UserId")) and Not AmIDelegate(Eval("UserId")) %>'   >
      &nbsp;
            <a href="#"  onclick="showPopup<%# pnlDelegate.ClientId & "(" & Eval("UserId") & ")" %>" style="color: Gray">Delegate</a>
            </asp:Label> 
       

        <asp:Panel ID="Panel1" runat="server" Font-Size="X-Small" Visible='<%# IsDelegated(Eval("UserId")) %>' style="padding-left: 10px;">
        
      Delegated to -->  <asp:Label ID="lblDelegate" runat="server"  Text='<%# GetDelegateName(Eval("UserId")) %>'  ></asp:Label>
      <asp:LinkButton ID="LinkButton1" runat="server" ForeColor="Gray"  CommandName="RemoveDelegate" CommandArgument='<%# Eval("UserId") %>'>Remove</asp:LinkButton>
      </asp:Panel>
      </div>
    </ItemTemplate>
    
</asp:DataList>


<div id="pnlDelegate" runat="server" style="text-align: center; ">
 <asp:DropDownList ID="ddlDelegate" runat="server" Font-Size="XX-Small"  DataSource='<%#  StaffBrokerFunctions.GetStaff(hfUserId.value ) %>'
                 DataTextField="DisplayName" DataValueField="UserID" AppendDataBoundItems="true">
                 <asp:ListItem Value="0"></asp:ListItem>
            </asp:DropDownList>
    <asp:HiddenField ID="hfPlebId" runat="server" />

            <br />
    <asp:CheckBox ID="CheckBox1" runat="server" Checked="false" ResourceKey="lblTesting" Text="Delegate everyone that reports to me to this staff member" />


 <br /><br />
    <asp:Label ID="Label1" runat="server" ForeColor="Red" Font-Italic="true"></asp:Label>
 <asp:Button ID="btnDelegate" runat="server" Text="Delegate" ResourceKey="btnDelegate" Width="100px" CssClass="aButton btn"   />
</div>
