<%@ Control Language="VB" AutoEventWireup="false" CodeFile="RmbOther.ascx.vb" Inherits="controls_RmbOther" ClassName="controls_RmbOther"  %>
<%@ Register assembly="DotNetNuke" namespace="DotNetNuke.UI.WebControls" tagprefix="cc1" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register src="Currency.ascx" tagname="Currency" tagprefix="uc1" %>

<div class="Agape_SubTitle"> <asp:HiddenField ID="hfNoReceiptLimit" runat="server" Value="0" />
   <asp:Label ID="Label3" runat="server" Font-Italic="true" ForeColor="Gray" resourcekey="Explanation"></asp:Label> 
</div><br />

<table  style="font-size:9pt;">
<tr>
    <td><b><dnn:label id="Label4"  runat="server" controlname="tbDesc" ResourceKey="lblDesc"  /></b></td>
    <td><asp:TextBox ID="tbDesc" runat="server" Width="550px" Columns="27"></asp:TextBox></td>
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
        <asp:Panel ID="Panel1" runat="server" Visible="false" > <!-- Disabled taxible question for now -->
   

<tr>
    <td><b><dnn:label id="Label5"  runat="server" controlname="ddlType" ResourceKey="lblTaxable" /></b></td>
    <td>
        <asp:DropDownList ID="ddlType" runat="server">
        
            <asp:ListItem Value="True" ResourceKey="isTaxable"></asp:ListItem>
            <asp:ListItem Value="False" Selected="True" ResourceKey="notTaxable"></asp:ListItem>
        </asp:DropDownList><br />

    </td>
   
</tr>
 </asp:Panel>
</table>
 <asp:Label ID="ErrorLbl" runat="server" Font-Size="9pt" ForeColor="Red" />

