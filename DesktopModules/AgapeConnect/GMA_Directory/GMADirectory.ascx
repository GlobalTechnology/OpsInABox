<%@ Control Language="VB" AutoEventWireup="False" CodeFile="GMADirectory.ascx.vb" Inherits="DotNetNuke.Modules.GMA.GMADirectory" %>
<%@ Register Src="~/controls/labelcontrol.ascx" TagName="labelcontrol" TagPrefix="uc1" %>
<link href="/Portals/_default/Skins/AgapeBlue/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
<script src="/Portals/_default/Skins/AgapeBlue/bootstrap/js/bootstrap.min.js"></script>






<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="gmaServerId" DataSourceID="dsGmaServers" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="10" ForeColor="Black" GridLines="Vertical">
    <AlternatingRowStyle BackColor="White" />
    <Columns>
        <asp:BoundField DataField="displayName" HeaderText="Display Name" SortExpression="displayName" />
        <asp:BoundField DataField="rootUrl" HeaderText="Root Url" SortExpression="rootUrl" />
        <asp:TemplateField ShowHeader="False">
            <EditItemTemplate>
                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update" Text="Update"  Visible='<%# Eval("addedByUser") = UserId or IsAdmin%>'></asp:LinkButton>
                &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel"  Visible='<%# Eval("addedByUser") = UserId or IsAdmin%>'></asp:LinkButton>
            </EditItemTemplate>
            <ItemTemplate>
                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Edit" Text="Edit"  Visible='<%# Eval("addedByUser") = UserId or IsAdmin%>'></asp:LinkButton>
                &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete"  Visible='<%# Eval("addedByUser") = UserId or IsAdmin%>'></asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>

    <FooterStyle BackColor="#CCCC99" />
    <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
    <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
    <RowStyle BackColor="#F7F7DE" />
    <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
    <SortedAscendingCellStyle BackColor="#FBFBF2" />
    <SortedAscendingHeaderStyle BackColor="#848384" />
    <SortedDescendingCellStyle BackColor="#EAEAD3" />
    <SortedDescendingHeaderStyle BackColor="#575357" />

</asp:GridView>

<asp:LinqDataSource ID="dsGmaServers" runat="server" ContextTypeName="GMA.gmaDataContext" EnableDelete="True" EnableInsert="True" EnableUpdate="True" EntityTypeName="" OrderBy="displayName" TableName="gma_Servers">
</asp:LinqDataSource>

<div class="form-horizontal">
    <fieldset>
    <legend>Add a GMA Server to the Global Directory</legend>
  <div class="control-group">
    <label class="control-label">Display Name:</label>
    <div class="controls">
     
           <asp:TextBox ID="tbDisplayName" runat="server" placeholder="Name"></asp:TextBox>
    </div>
  </div>
  <div class="control-group">
    <label class="control-label">GMA Server URL:</label>
    <div class="controls">
      <asp:TextBox ID="tbURL" runat="server" placeholder="URL" type="url" required></asp:TextBox>

        <asp:Label ID="lblURLHelp" runat="server" style="color: #c09853; font-size: x-small;" >(Don't forget the "https://" or "http://")</asp:Label>
    </div>
  </div>
  <div class="control-group">
    <div class="controls">
         <asp:Label ID="lblValidation" runat="server" style="color:red; font-size: x-small;" ></asp:Label><br />
        <asp:Button ID="btnSubmit" runat="server" CssClass="btn" Text="Submit" />
    
    </div>
  </div>
        </fieldset>
</div>