    <%@ Control Language="VB" AutoEventWireup="false" CodeFile="Unpublished.ascx.vb"
    Inherits="DotNetNuke.Modules.Stories.Unpublished" %>

<asp:HiddenField ID="hfTabModuleID" runat="server" Value="-1" />
<div id="Unpublished">
    <div class=validationError>
        <asp:Label ID="PublishValidator" runat="server" Visible="False"></asp:Label>
    </div>
<asp:GridView ID="gvPublish" runat="server" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px"
     CellPadding="4" ForeColor="Black" GridLines="Vertical" AutoGenerateColumns="False" DataKeyNames="StoryId" DataSourceID="dsStagedStories">
    <AlternatingRowStyle BackColor="White" />
    <Columns>
        <asp:BoundField DataField="StoryDate" DataFormatString="{0:dd MMM yyyy}" HeaderText="StoryDate" SortExpression="StoryDate" />
        <asp:TemplateField HeaderText="Headline" SortExpression="Headline">

            <EditItemTemplate>
                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Headline") %>'></asp:TextBox>
            </EditItemTemplate>
            <ItemTemplate>
                <asp:HyperLink ID="HyperLink1" runat="server" Text='<%# Eval("Headline")%>' NavigateUrl='<%# NavigateURL & "?StoryId=" &  Eval("StoryId")%>'>HyperLink</asp:HyperLink>
    
            </ItemTemplate>
            <ItemStyle Font-Bold="True" />
        </asp:TemplateField>
        <asp:BoundField DataField="Author" HeaderText="Author" SortExpression="Author" />
        <asp:TemplateField ShowHeader="False">
            <ItemTemplate>
                <asp:LinkButton ID="lbPublish" runat="server" CausesValidation="False" CommandName="Publish"
                    CommandArgument='<%# Eval("StoryId") & "," & Eval("Headline")%>' Text="Publish"></asp:LinkButton>
                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete"></asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    <EmptyDataTemplate>
        <i>There are no unpublished stories at this time.</i>
    </EmptyDataTemplate>
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

    <asp:LinqDataSource ID="dsStagedStories" runat="server" ContextTypeName="Stories.StoriesDataContext" EnableDelete="True" EntityTypeName="" OrderBy="StoryDate desc" TableName="AP_Stories" Where="TabModuleID == @TabModuleID &amp;&amp; IsVisible == @IsVisible">
        <WhereParameters>
            <asp:ControlParameter ControlID="hfTabModuleID" Name="TabModuleID" PropertyName="Value" Type="Int32" />
            <asp:Parameter DefaultValue="False" Name="IsVisible" Type="Boolean" />
        </WhereParameters>
    </asp:LinqDataSource>

<div class="submitPanel">
    <asp:LinkButton ID="CancelBtn" runat="server" class="Button" ResourceKey="Done"></asp:LinkButton>
</div>
</div>
