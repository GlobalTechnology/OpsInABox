<%@ Control Language="vb" AutoEventWireup="false" CodeFile="DocumentSettings.ascx.vb" Inherits="DotNetNuke.Modules.AgapeConnect.Documents.DocumentSettings" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<div id="DocumentSettings" class="Documents">

    <div id="divFolder" class="FieldRow">
        <asp:Label ID="lblFolder" runat="server" ResourceKey="lblRoot" CssClass="FieldLabel"></asp:Label>
        <asp:DropDownList ID="ddlRoot" runat="server"></asp:DropDownList>
        <asp:HyperLink ID="btnEdit" CssClass="btnEdit" runat="server"></asp:HyperLink>
        <asp:LinkButton ID="btnDelete" CssClass="btnDelete" runat="server"></asp:LinkButton>
        <asp:LinkButton ID="btnAdd" CssClass="btnAdd" runat="server"></asp:LinkButton>
        
    </div>

    <div id="divButtonAdd">
        <asp:UpdatePanel ID="upButtonAdd" runat="server">
            <ContentTemplate>
                <div class="FieldSubRow">
                    <asp:Label ID="lblAddSubFolder" runat="server" ResourceKey="lblAddSubFolder" CssClass="FieldSubLabel" visible="false"></asp:Label>
                    <asp:textbox ID="tbAddSubFolder" runat="server" visible="false"></asp:textbox>
                    <asp:Button ID="btnAddSubFolder" runat="server" ResourceKey="btnAddSubFolder" visible="false" CssClass="button"></asp:Button>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName = "Click"/>
            </Triggers>
        </asp:UpdatePanel>
        <hr />   
    </div>
     
    <div class="SubmitPanel">
        
        <asp:Button ID="btnSave" runat="server" ResourceKey="btnSave" CssClass="button"></asp:Button>
        <asp:Button ID="btnCancel" runat="server" ResourceKey="btnCancel" CssClass="button"></asp:Button>
    </div>

</div>
