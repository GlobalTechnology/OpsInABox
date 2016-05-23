<%@ Control Language="vb" AutoEventWireup="false" CodeFile="TagSettings.ascx.vb"
    Inherits="DotNetNuke.Modules.Stories.TagSettings" %>
<%@ Register Src="../StaffAdmin/Controls/acImage.ascx" TagName="acImage" TagPrefix="uc1" %>

<script>
    (function ($, Sys) {
    function setUpMyModule() {
        //Initialize confirmation popup for delete buttons
        $('.btnDelete').each(function(){
            $(this).dnnConfirm({
                text: '<%=LocalizeString("deleteTagConfirmationQuestion")%>',
                yesText: '<%=LocalizeString("deleteTagConfirmationYes")%>',
                noText: '<%=LocalizeString("deleteTagConfirmationNo")%>',
                title: '<%=LocalizeString("deleteTagConfirmationTitle")%>'
            });
        })
    }

    $(document).ready(function () {
        setUpMyModule();
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            setUpMyModule();
        });
    });
    }(jQuery, window.Sys));
</script>

<div id="TagSettings">

    <asp:GridView ID="gvTags"
        runat="server"
        BorderStyle="None"
        GridLines="Vertical"
        AutoGenerateColumns="False"
        DataKeyNames="StoryTagId"
        EmptyDataText="">
    <AlternatingRowStyle BackColor="#F7F7DE" />
    <HeaderStyle BackColor="#6B696B" ForeColor="White" CssClass="GridViewHeader" />
    <RowStyle CssClass="GridViewRows" />
    <Columns>
        <asp:TemplateField HeaderText="Image">
            <EditItemTemplate><uc1:acImage ID="ImagePicker" runat="server" SaveWidth="700" Updated="ImagePicker_ImageUpdated"/></EditItemTemplate>
            <ItemTemplate><asp:Image ID="TagThumbnail" runat="server" Width="50px"/></ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="TagName" HeaderText="TagName" SortExpression="TagName" />
        <asp:BoundField DataField="Keywords" HeaderText="Keywords" SortExpression="Keywords" />
        <asp:CheckBoxField DataField="Master" HeaderText="Master" SortExpression="Master"/>
        <asp:CommandField ShowEditButton="True" ControlStyle-CssClass="gvTemplateButtons" />
        <asp:TemplateField ShowHeader="False" ControlStyle-CssClass="btnDelete">
            <ItemTemplate>
                <asp:LinkButton ID="btnDelete"
                    runat="server"
                    CausesValidation="False"
                    CommandName="DeleteTag"
                    CommandArgument='<%# Eval("StoryTagId")%>'
                    ResourceKey="Delete">
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

    <div class="AddTagPanel">
        <asp:TextBox ID="tbAddTag" runat="server" MaxLength="50" Columns="30"></asp:TextBox>
        <asp:Button ID="btnAddTag" runat="server" ResourceKey="btnAddTag" CssClass="button" /> 
    </div>

<div class="submitPanel">
    <asp:LinkButton ID="CancelBtn" runat="server" class="Button" ResourceKey="Done"></asp:LinkButton>
</div>
</div>