<%@ Control Language="VB" AutoEventWireup="false" CodeFile="RmbEntAct.ascx.vb" Inherits="controls_RmbEntAct" ClassName="controls_RmbEntAct"  %>

<%@ Register assembly="DotNetNuke" namespace="DotNetNuke.UI.WebControls" tagprefix="cc1" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register src="Currency.ascx" tagname="Currency" tagprefix="uc1" %>

<div class="Agape_SubTitle"> <asp:HiddenField ID="hfNoReceiptLimit" runat="server" Value="0" />
    <asp:Label ID="Label6" runat="server" Font-Italic="true" ForeColor="Gray" resourcekey="Explanation"></asp:Label>
</div><br />

<table   style="font-size:9pt; ">
<tr>
    <td><b><dnn:label id="Label4"  runat="server" controlname="tbDesc" ResourceKey="lblDesc" /></b></td>
    <td><asp:TextBox ID="tbDesc" runat="server" Width="550px"></asp:TextBox></td>
</tr>
<tr>
     <td><b><dnn:label id="Label1"  runat="server" controlname="dtDate" ResourceKey="lblDate"  /></b></td>
    <td  colspan="2">
       <asp:TextBox ID="dtDate" runat="server" Width="90px" class="datepicker"></asp:TextBox>
        
    </td>
</tr>
<tr>
   <td><b><dnn:label id="Label2"  runat="server" controlname="tbAmount" ResourceKey="lblAmount"  /></b></td>
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
<tr>
   <td><b><dnn:label id="Label5"  runat="server" controlname="tbAmount"  ResourceKey="lblTaxable" text="More Info:" HelpText="This is just to demonstrate this type of control, that might be helpful in the taxible/nontaxible questions." /></b></td>
    <td>  <asp:RadioButtonList ID="RadioButtonList1" runat="server" Font-Size="9pt">
        <asp:ListItem Selected="True" ResourceKey="notTaxable">Entertaining Supporters or Ministry Contacts</asp:ListItem>
        <asp:ListItem  ResourceKey="isTaxable">Other</asp:ListItem>
        
    </asp:RadioButtonList>
        <asp:CheckBox ID="CheckBox1" runat="server" ResourceKey="notPersonal" Text="None of the items being reimbursed were used for personal use!!" />
    </td>
</tr>



</table>
 <asp:Label ID="ErrorLbl" runat="server" Font-Size="9pt" ForeColor="Red" />

