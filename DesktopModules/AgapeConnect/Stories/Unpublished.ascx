    <%@ Control Language="VB" AutoEventWireup="false" CodeFile="Unpublished.ascx.vb"
    Inherits="DotNetNuke.Modules.Stories.Unpublished" %>

<asp:HiddenField ID="hfTabModuleID" runat="server" Value="-1" />
<div id="Unpublished">
    <div class=validationError>
        <asp:Label ID="PublishValidator" runat="server" Visible="False"></asp:Label>
    </div>

    <asp:GridView ID="gvPublish"
        runat="server"
        BackColor="White"
        BorderColor="#DEDFDE"
        BorderStyle="None"
        ForeColor="Black"
        GridLines="Vertical"
        AutoGenerateColumns="False"
        DataKeyNames="StoryId"
        EmptyDataText="">
    <AlternatingRowStyle BackColor="#F7F7DE" />
    <HeaderStyle BackColor="#6B696B" ForeColor="White" CssClass="GridViewHeader" />
    <RowStyle CssClass="GridViewRows" />
    <Columns>
        <asp:BoundField DataField="StoryDate"
            DataFormatString="{0:dd MMM yyyy}"
            HeaderText="StoryDate" >
        </asp:BoundField>

        <asp:TemplateField HeaderText="Headline" >
            <EditItemTemplate>
                <asp:TextBox ID="tbHeadline" runat="server" Text='<%# Bind("Headline") %>'></asp:TextBox>
            </EditItemTemplate>
            <ItemTemplate>
                <asp:HyperLink ID="hlHeadline"
                    runat="server"
                    Text='<%# Eval("Headline")%>'
                    NavigateUrl='<%# NavigateURL & "?StoryId=" &  Eval("StoryId")%>'>Hyperlink
                </asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>

        <asp:BoundField DataField="Author"
            HeaderText="Author" >
        </asp:BoundField>

        <asp:TemplateField ShowHeader="False" ControlStyle-CssClass="gvTemplateButtons">
            <ItemTemplate>
                <asp:LinkButton ID="lbPublish"
                    runat="server"
                    CausesValidation="False"
                    CommandName="Publish"
                    CommandArgument='<%# Eval("StoryId") & "," & Eval("Headline")%>'
                    ResourceKey="Publish"
                    CssClass="Button">
                </asp:LinkButton>
                <asp:LinkButton ID="lbDelete" 
                    runat="server"
                    CausesValidation="False"
                    CommandName="Delete"
                    ResourceKey="Delete"
                    CssClass="Button">
                </asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>

    <EmptyDataTemplate>
        <div class=validationError>
            <asp:Label ID="lbEmpty" runat="server" ResourceKey="Empty"></asp:Label>
        </div>
    </EmptyDataTemplate>
    </asp:GridView>

<div class="submitPanel">
    <asp:LinkButton ID="CancelBtn" runat="server" class="Button" ResourceKey="Done"></asp:LinkButton>
</div>
</div>
