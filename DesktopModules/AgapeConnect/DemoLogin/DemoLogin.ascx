<%@ Control Language="VB" AutoEventWireup="false" CodeFile="DemoLogin.ascx.vb" Inherits="DotNetNuke.Modules.AgapePortal.DemoLogin" %>
<script type="text/javascript">

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




<div align="left">
<div class="AgapeH3">
    
Welcome to AgapeConnect (<%= PortalSettings.PortalName%>)!
</div>


<table cellspacing="10px">
    <tr valign="top">
        <td>
        <asp:Panel ID="pnlNotKnown" runat="server">
<p>
Every Agap&eacute;Connect site is linked to the accounts system of a local country.
This website is tied to the Accounts system for a demo location we call "<%= PortalSettings.PortalName%>". <span class="AgapeH5"> Welcome to <%= PortalSettings.PortalName%>!</span>
</p>
<p>
Before a staff member can use the features on this site, they have to be setup. We need to know  a few details like which responsibility centers are yours, and who approves your reimbursments.
Since you are not a member of <%= PortalSettings.PortalName%>, you cannot access these features as yourself.
So we have setup a couple of demonstration accouts, so that you can explore the site. 
</p>
<p class="ui-state-highlight ui-corner-all" style="padding: 3px; margin: 0 30px 0 30px;">
    Tip: 
    I recommend that you start with <b><i>Staff Member</i></b> - to submit a reimbursement.
    Then login as <b><i>Team Leader</i></b> approve the reimbursement and
    if you are interested in the Accounts Functions log in as <b><i>Accounts Team</i></b> to process/download the reimbursement.


</p>

<p> <i>
    * Please note that this site is still in development - and that we have not ironed out all the bugs yet.
    We will also be implementing translation, but this is not yet in place.
    So feel free to have a play - you won't do any damage, but expect to experience a few glitches until we finish development. 
</i></p>

</asp:Panel>
<asp:Panel ID="pnlDemo" runat="server" Visible="false">
<p  class="ui-state-highlight ui-corner-all" style=" font-size: 11pt; padding: 3px; margin: 0 30px 0 30px;">You are logged in as     <asp:Label ID="lblUserName" runat="server" Font-Bold="true" />.
Click on the Expenses Icon (or select Expenses from the menu) to 
    <asp:Label ID="lblTask" runat="server" /> a reimbursement.
<span style="color: #777;"><i>(All data on this website is assumed to be fictitious and will be ignored!)</i></span> 
</p>




<p> <i>
    * Please note that this site is still in development - and that we have not ironed out all the bugs yet.
    We will also be implementing translation, but this is not yet in place.
    So feel free to have a play - you won't do any damage, but expect to experience a few glitches until we finish development. 
</i></p>

</asp:Panel>
        </td>
        <td width="600px" >
        <p>
            <div class="AgapeH4">Which user would you like to impersonate?</div> 
            
            </p>
            <div align="center" style="color: #777; font-style:italic ;">

                <table cellspacing="15px">
                    <tr valign="top">
                        <td>
                            <asp:Button ID="btnStaff" runat="server" Text="Staff Member" width="120px" CssClass="aButton btn" />
                        </td>
                        <td>
                            A typical staff member. Use this account to submit reimbursments and view your Account Reports.
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnLeader" runat="server" Text="Team Leader" width="120px" CssClass="aButton btn"  />
                        </td>
                        <td>
                            The Team Leader (for the above staff member). Use this account to approve personal reimbursements (from the above user), or Department Reimbursments.
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnAccounts" runat="server" Text="Accounts Team" width="120px" CssClass="aButton btn"  />
                        </td>
                        <td>
                            A member of the Accounts Team. Use this account to process/download approved reimbursements.
                            To access the accounts functions for Expenses, go to the Expenses page and select Edit Mode (Top Right).
                            An account team member can access the site as a typical staff member when in view mode and as an Accounts User
                            when in Edit mode.
                        </td>
                    </tr>
                     <tr>
                        <td>
                            <asp:Button ID="btnAdministrator" runat="server" Text="Administrator" width="120px" CssClass="aButton btn"  Enabled="false"/>
                        </td>
                        <td>
                            In order to setup new staff member, or edit their account details, you must
                            be logged in as an administrator. For security reasons, we cannot give you access
                            to this user. (sorry!)
                        </td>
                    </tr>
                </table>
             
            
            </div>


        </td>
    </tr>

</table>

</div>