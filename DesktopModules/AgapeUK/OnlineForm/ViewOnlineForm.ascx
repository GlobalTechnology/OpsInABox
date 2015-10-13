<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ViewOnlineForm.ascx.vb" Inherits="DotNetNuke.Modules.OnlineForm.ViewOnlineForm"%>
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

<style type="text/css">
    .Menu_Prefix
{
	font-family: Verdana;
	font-size: 10pt;
	font-weight: normal;
	font-style: normal;
	color: black;
}
    .Menu_Suffix
{
    font-family:Verdana;
    font-size: 8pt;
    font-style:italic;
    color: gray;
}
    .WhiteLink
{
    font-family:Verdana;
    font-size: 12pt;
    font-weight:900;
}
               
</style>
<table>
<tr>
<td width="100%", class="Menu_Prefix">
   <asp:Label ID="PrefixLabel" runat="server" Text="Label"></asp:Label>
</td>
</tr>
<tr>
<td width="100%" >
    <table cellpadding="10">
        <asp:PlaceHolder ID="QuPlaceHolder" runat="server"></asp:PlaceHolder>
    
    </table>
</td>
</tr>
    <asp:Panel ID="EmailPanel" runat="server">

<tr>
<td>
    <table cellpadding="10">
    <tr>
    
    <td valign="top" style="font-weight: bolder; font-size: 10pt;" width="150px">Email Address:</td>
    <td>
        <asp:TextBox ID="Email" runat="server" width="300px"></asp:TextBox>
        <asp:RequiredFieldValidator ID="ReqEmail" runat="server" ErrorMessage="Please enter an email address." ControlToValidate="Email" Enabled="false"></asp:RequiredFieldValidator>
        
        </td>
        </tr></table></td>
</tr>
    </asp:Panel>
<tr>
    <td align="center" >
         
        <asp:Button ID="SubmitButton" runat="server" Text="Submit" CssClass="aButton" />
        
        <asp:Label ID="SentLabel" runat="server"  ForeColor="Red" Visible="false"></asp:Label>
        </td>
</tr>
<tr>
<td width="100%", class="Menu_Suffix">
    <asp:Label ID="SuffixLabel" runat="server" Text="Label"></asp:Label>
</td>
</tr>
</table>
<br />
<asp:LinkButton ID="EditButton" runat="server" Visible="false" CausesValidation="False">Edit</asp:LinkButton>&nbsp;
 <asp:LinkButton ID="ResultsButton" runat="server" Visible="false" CausesValidation="False">Results</asp:LinkButton>

 

