<%@ Control Language="VB" AutoEventWireup="false" CodeFile="RmbStaffMtg.ascx.vb" Inherits="controls_RmbStaffMtg" ClassName="controls_RmbStaffMtg"  %>

<%@ Register assembly="DotNetNuke" namespace="DotNetNuke.UI.WebControls" tagprefix="cc1" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register src="Currency.ascx" tagname="Currency" tagprefix="uc1" %>

<div class="Agape_SubTitle"> <asp:HiddenField ID="hfNoReceiptLimit" runat="server" Value="0" />
    <asp:Label ID="Label6" runat="server" Font-Italic="true" ForeColor="Gray" resourcekey="Explanation"></asp:Label>
</div><br />
<table  style="font-size:9pt; ">
<tr>
    <td width="150px;"><b><dnn:label id="Label4"  runat="server" controlname="tbConfName" ResourceKey="lblDesc"  /></b></td>
    <td><asp:TextBox ID="tbConfName" runat="server" Width="550px"></asp:TextBox></td>
</tr>
<tr>
    <td><b><dnn:label id="Label1"  runat="server" controlname="dtConfDate"  ResourceKey="lblDate"  /></b></td>
    <td colspan="2">
         <asp:TextBox ID="dtConfDate" runat="server" Width="90px" class="datepicker"></asp:TextBox>
       
    </td>
</tr>
<tr>
    <td><b><dnn:label id="Label2"  runat="server" controlname="tbAmount"  ResourceKey="lblAmount"  /></b></td>
    <td><table>
                <tr>
                    <td>
                        <asp:TextBox ID="tbAmount" runat="server" Width="90px" class="numeric rmbAmount"></asp:TextBox>
                    </td>
                    <td>
                        <uc1:Currency ID="Currency1" runat="server" />
                    </td>
                </tr>
            </table></td>
</tr>
    <tr id="ReceiptLine" runat="server">
        <td>
            <b>
                <dnn:Label ID="ttlReceipt" runat="server" ControlName="ddlVATReceipt" />
            </b>
        </td>
        <td>
            <asp:DropDownList ID="ddlVATReceipt" runat="server" CssClass="ddlReceipt">

                <asp:ListItem ResourceKey="Standard" Value="1">Paper Receipt</asp:ListItem>
                <asp:ListItem ResourceKey="Electronic" Value="2">Electronic Receipt</asp:ListItem>
                <asp:ListItem Value="-1">No Receipt (under [LIMIT])</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr id="VATLine" runat="server" class="VATLine" visible="false">
        <td>
            <b>
                <dnn:Label ID="lblVAT" runat="server" ControlName="cbVAT" ResourceKey="lblVAT" />
            </b>
        </td>
        <td>
            <asp:CheckBox ID="cbVAT" runat="server" />
        </td>
    </tr>
    <tr valign="top" >
    <td><b><dnn:label id="Label5"  runat="server" controlname="ddlVATReceipt" ResourceKey="lblSplit"  text="Split cost:" HelpText="If the cost of this meeting is to be shared between several accounts (i.e. a team retreat), please specify how this should be charged. List each ministry/staff account and how much should be charged to each cost centre. " /></b></td>
    <td>
        <asp:CheckBox ID="CheckBox1" runat="server" ResourceKey="Split" Text="Split this cost accross the following Staff/Ministry Accounts*:" /> <br />

        <asp:TextBox ID="TextBox1" runat="server" TextMode="MultiLine" Width="400px" Height="120px"></asp:TextBox><br />
        <asp:Label ID="Label7" runat="server" ResourceKey="SplitInfo"></asp:Label>
      
    </td>
   
</tr>
</table>
 <asp:Label ID="ErrorLbl" runat="server" Font-Size="9pt" ForeColor="Red" />

