<%@ Control Language="VB" AutoEventWireup="false" CodeFile="RmbPerDiemMulti.ascx.vb" Inherits="controls_RmbPerDiemMulti" ClassName="controls_RmbPerDiemMulti"  %>

<%@ Register assembly="DotNetNuke" namespace="DotNetNuke.UI.WebControls" tagprefix="cc1" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>

<div class="Agape_SubTitle"> <asp:HiddenField ID="hfNoReceiptLimit" runat="server" Value="0" />
    <asp:Label ID="Label3" runat="server" Font-Italic="true" ForeColor="Gray" resourcekey="Explanation"></asp:Label>
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
    <td>
        <b><dnn:label id="Label5"  runat="server" controlname="tbAmount" ResourceKey="lblAmount" /></b>
    
    </td>
    <td>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Label ID="Label2" runat="server" ResourceKey="NoOfPeople"></asp:Label>
        <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True">
            <asp:ListItem>1</asp:ListItem>
            <asp:ListItem>2</asp:ListItem>
            
        </asp:DropDownList>
        <asp:Label ID="Label6" runat="server" ResourceKey="NoOfDays"></asp:Label>
         <asp:DropDownList ID="DropDownList3" runat="server" AutoPostBack="True">
            <asp:ListItem>2</asp:ListItem>
            <asp:ListItem>3</asp:ListItem>
            <asp:ListItem>4</asp:ListItem>
            <asp:ListItem>5</asp:ListItem>
            <asp:ListItem>6</asp:ListItem>
            <asp:ListItem>7</asp:ListItem>
            <asp:ListItem>8</asp:ListItem>
            <asp:ListItem>9</asp:ListItem>
            <asp:ListItem>10</asp:ListItem>
            <asp:ListItem>11</asp:ListItem>
            <asp:ListItem>12</asp:ListItem>
            <asp:ListItem>12</asp:ListItem>
            <asp:ListItem>13</asp:ListItem>
            <asp:ListItem>14</asp:ListItem>
        
        </asp:DropDownList>
        <asp:Label ID="Label7" runat="server" ResourceKey="Type"></asp:Label>
        <asp:DropDownList ID="DropDownList2" runat="server" AutoPostBack="True">
          <%--  <asp:ListItem Value="26.50">Whole Day</asp:ListItem>
            <asp:ListItem Value="5.5">Breakfast</asp:ListItem>
            <asp:ListItem Value="8">Lunch</asp:ListItem>
            <asp:ListItem Value="13">Dinner</asp:ListItem>--%>
        </asp:DropDownList><br />
        <asp:Label ID="Label8" runat="server" ResourceKey="AmountClaimed"></asp:Label>
         <asp:TextBox ID="tbAmount" runat="server" Width="90px" class="numeric" Visible="False"></asp:TextBox>
    <%--<cc2:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="tbAmount"    FilterType="Custom" ValidChars=".0123456789">
        </cc2:FilteredTextBoxExtender>--%>
     <asp:Label ID="lblMaxAmt" runat="server" Text="Label" ></asp:Label>
     </ContentTemplate>
     <Triggers>
        <asp:AsyncPostBackTrigger ControlID="DropDownList1" EventName="SelectedIndexChanged" />
         <asp:AsyncPostBackTrigger ControlID="DropDownList2" EventName="SelectedIndexChanged" />
         <asp:AsyncPostBackTrigger ControlID="DropDownList3" EventName="SelectedIndexChanged" />
        
     </Triggers>
             </asp:UpdatePanel>
    </td>
</tr>


</table>
 <asp:Label ID="ErrorLbl" runat="server" Font-Size="9pt" ForeColor="Red" />

