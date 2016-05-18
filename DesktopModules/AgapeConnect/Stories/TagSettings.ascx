<%@ Control Language="vb" AutoEventWireup="false" CodeFile="TagSettings.ascx.vb"
    Inherits="DotNetNuke.Modules.Stories.TagSettings" %>
<%@ Register Src="../StaffAdmin/Controls/acImage.ascx" TagName="acImage" TagPrefix="uc1" %>

<div id="TagSettings">

    <asp:GridView ID="gvTags"
        runat="server"
        BackColor="White"
        BorderColor="#DEDFDE"
        BorderStyle="None"
        ForeColor="Black"
        GridLines="Vertical"
        AutoGenerateColumns="False"
        DataKeyNames="StoryTagId"
        EmptyDataText="">
    <AlternatingRowStyle BackColor="#F7F7DE" />
    <HeaderStyle BackColor="#6B696B" ForeColor="White" CssClass="GridViewHeader" />
    <RowStyle CssClass="GridViewRows" />
    <Columns>
        <asp:TemplateField HeaderText="Image">
            <EditItemTemplate><uc1:acImage ID="ImagePicker" runat="server" Aspect="1.3" SaveWidth="700" Updated="ImagePicker_ImageUpdated"/></EditItemTemplate>
            <ItemTemplate><asp:Image ID="TagThumbnail" runat="server" Width="50px"/></ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="TagName" HeaderText="TagName" SortExpression="TagName" />
        <asp:BoundField DataField="Keywords" HeaderText="Keywords" SortExpression="Keywords" />
        <asp:CheckBoxField DataField="Master" HeaderText="Master" SortExpression="Master"/>
        <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
    </Columns>

    <EmptyDataTemplate>
        <div class=validationError>
            <asp:Label ID="lbEmpty" runat="server" ResourceKey="Empty"></asp:Label>
        </div>
    </EmptyDataTemplate>
    </asp:GridView>

    <div class="AddTagPanel">
        <asp:TextBox ID="tbAddTag" runat="server"></asp:TextBox>
        <asp:Button ID="btnAddTag" runat="server" ResourceKey="btnAddTag" CssClass="button" /> 
    </div>
    <div class="DeleteTagWarning">
        <asp:Label ID="lblTagsDelete" runat="server" ResourceKey="lblTagsDelete"></asp:Label>
    </div>

<div class="submitPanel">
    <asp:LinkButton ID="CancelBtn" runat="server" class="Button" ResourceKey="Done"></asp:LinkButton>
</div>
</div>