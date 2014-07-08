<%@ Control Language="VB" AutoEventWireup="false" CodeFile="RmbEquipment.ascx.vb"
    Inherits="controls_RmbEquipment" ClassName="controls_RmbEquipment" %>
<%@ Register Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" TagPrefix="cc1" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>


<%@ Register src="Currency.ascx" tagname="Currency" tagprefix="uc1" %>


<div class="Agape_SubTitle">
    <asp:HiddenField ID="hfNoReceiptLimit" runat="server" Value="0" />
    <asp:Label ID="Label6" runat="server" Font-Italic="true" ForeColor="Gray" resourcekey="Explanation"></asp:Label>
</div>
<br />
<table style="font-size: 9pt;">
    <tr>
        <td>
            <b>
                <dnn:Label ID="Label4" runat="server" ControlName="tbDesc" ResourceKey="lblDesc" />
            </b>
        </td>
        <td>
            <asp:TextBox ID="tbDesc" runat="server" Width="550px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <b>
                <dnn:Label ID="Label1" runat="server" ControlName="dtDate" ResourceKey="lblDate" />
            </b>
        </td>
        <td colspan="2">
            <asp:TextBox ID="dtDate" runat="server" Width="90px" class="datepicker"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <b>
                <dnn:Label ID="Label2" runat="server" ControlName="tbAmount" ResourceKey="lblAmount" />
            </b>
        </td>
        <td>
            <table>
                <tr>
                    <td>
                        <asp:TextBox ID="tbAmount" runat="server" Width="90px" class="numeric rmbAmount"></asp:TextBox>
                    </td>
                    <td>
                        <uc1:Currency ID="Currency1" runat="server" />
                    </td>
                </tr>
            </table>
             
            
            <%--  <cc2:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="tbAmount"    FilterType="Custom" ValidChars=".0123456789">
        </cc2:FilteredTextBoxExtender>--%>
        
           
           
        </td>
    </tr>
    <tr id="ReceiptLine" runat="server">
        <td>
            <b>
                <dnn:Label ID="ttlReceipt" runat="server" ControlName="ddlVATReceipt" />
            </b>
        </td>
        <td>
            <asp:DropDownList ID="ddlVATReceipt" runat="server"  CssClass="ddlReceipt">
               
                <asp:ListItem ResourceKey="Standard" Value="1">Paper Receipt</asp:ListItem>
                <asp:ListItem  Value="2" ResourceKey="Electronic">Electronic Receipt</asp:ListItem>
            <asp:ListItem Value="-1">No Receipt (under [LIMIT])</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr id="VATLine" runat="server"  class="VATLine" visible="false">
        <td>
            <b>
                <dnn:Label ID="lblVAT" runat="server" ControlName="cbVAT"  ResourceKey="lblVAT" />
            </b>
        </td>
        <td>
            <asp:CheckBox ID="cbVAT" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <b>
                <dnn:Label ID="Label5" runat="server" ControlName="ddlVATReceipt" ResourceKey="lblMoreInfo" />
            </b>
        </td>
        <td>
            <asp:DropDownList ID="ddlType" runat="server">
                <asp:ListItem Value="0" ResourceKey="Computer">Computer</asp:ListItem>
                <asp:ListItem Value="1" ResourceKey="Software">Software</asp:ListItem>
                <asp:ListItem Value="2" ResourceKey="Peripheral">Computer Peripheral</asp:ListItem>
                <asp:ListItem Value="3" Selected="True" ResourceKey="Other">Other</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
</table>
<asp:Label ID="ErrorLbl" runat="server" Font-Size="9pt" ForeColor="Red" />
