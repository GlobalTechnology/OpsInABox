<%@ Control Language="VB" AutoEventWireup="false" CodeFile="RmbTravel.ascx.vb" Inherits="controls_RmbTravel" ClassName="controls_RmbTravel"  %>
<%@ Register assembly="DotNetNuke" namespace="DotNetNuke.UI.WebControls" tagprefix="cc1" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register src="Currency.ascx" tagname="Currency" tagprefix="uc1" %>

<div class="Agape_SubTitle"> <asp:HiddenField ID="hfNoReceiptLimit" runat="server" Value="0" />
    <asp:Label ID="Label6" runat="server" Font-Italic="true" ForeColor="Gray" resourcekey="Explanation"></asp:Label>
  </div><br />

<table   style="font-size:9pt; ">
<tr>
    <td><b><dnn:label id="Label4"  runat="server" controlname="tbDesc" ResourceKey="lblDesc"  /></b></td>
    <td><asp:TextBox ID="tbDesc" runat="server" Width="550px"></asp:TextBox></td>
</tr>
<tr>
     <td><b><dnn:label id="Label1"  runat="server" controlname="dtDate"  ResourceKey="lblDate"  /></b></td>
    <td  colspan="2">
       <asp:TextBox ID="dtDate" runat="server" Width="90px" class="datepicker"></asp:TextBox>
       <%-- <cc2:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="dtDate"  Format="dd/MM/yyyy" >
        </cc2:CalendarExtender>
        <cc2:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="dtDate" ValidChars="0123456789/">
        </cc2:FilteredTextBoxExtender>--%>
    </td>
</tr>

<tr>
    <td><b><dnn:label id="Label5"  runat="server" controlname="tbDesc" ResourceKey="lblType"  text="Type:" HelpText="Please enter the type of travel that best describes this expense" /></b></td>
  
    <td>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
        <ContentTemplate>
       <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="true">
         <asp:ListItem Value="0" ResourceKey="Parking">Parking</asp:ListItem>
         <asp:ListItem Value="1" ResourceKey="Train">Train or bus ticket</asp:ListItem>
         <asp:ListItem Value="2" ResourceKey="Flight">Flight</asp:ListItem>
         <asp:ListItem Value="3" ResourceKey="Taxi">Taxi</asp:ListItem>
         <asp:ListItem Value="4" ResourceKey="Travelcard">Travelcard/Season Ticket</asp:ListItem>
         <asp:ListItem Value="5" ResourceKey="HireCar">Hire Car</asp:ListItem>

         <asp:ListItem Value="6">Other</asp:ListItem>
           
   </asp:DropDownList>
        <asp:Panel ID="pnlTravelcard" runat="server">
     
            <asp:Label ID="Label7" runat="server" ResourceKey="lblTravelcard"></asp:Label>
     
         <asp:DropDownList ID="DropDownList3" runat="server">
         <asp:ListItem Value="Yes" ResourceKey="Yes">Yes</asp:ListItem>
         <asp:ListItem Value="No" ResourceKey="No">No</asp:ListItem>
     </asp:DropDownList>
     <br />
   <%--  <asp:Panel ID="pnlOyster" runat="server">
     
         <asp:CheckBox ID="cbOyster" runat="server" Text="I have only reimbursed itemized journeys. (Bus journeys must be itemized below)" /><br />
            <asp:TextBox ID="tbOyster" runat="server" TextMode="MultiLine" Width="400px" Height="50px"></asp:TextBox>
            </asp:Panel>--%>
     </asp:Panel>
     <br />
            <asp:Panel ID="pnlWorlPlace" runat="server" Visible="false">
            
            <asp:Label ID="Label8" runat="server" ResourceKey="lblNormal"></asp:Label>
     
         <asp:DropDownList ID="ddlWorkplace" runat="server">
         <asp:ListItem Value="Yes"  ResourceKey="Yes">Yes</asp:ListItem>
         <asp:ListItem Value="No"  ResourceKey="No" Selected="True">No</asp:ListItem>
     </asp:DropDownList></asp:Panel>
        </ContentTemplate>
        


        </asp:UpdatePanel>
        
   
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
</table>
 <asp:Label ID="ErrorLbl" runat="server" Font-Size="9pt" ForeColor="Red" />

