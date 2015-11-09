<%@ Control Language="vb" AutoEventWireup="false" CodeFile="DocumentSettings.ascx.vb" Inherits="DotNetNuke.Modules.AgapeConnect.Documents.DocumentSettings" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<div id="DocumentSettings" class="Documents">

    <div  class="FieldRow">
        <asp:Label ID="lblFolder" runat="server" ResourceKey="lblRoot" CssClass="FieldLabel"></asp:Label>
        <asp:DropDownList ID="ddlRoot" runat="server"></asp:DropDownList>
        <asp:HyperLink ID="btnEdit" CssClass="btnEdit" runat="server"></asp:HyperLink>
        <asp:HyperLink ID="btnDelete" CssClass="btnDelete" runat="server"></asp:HyperLink>
        <asp:HyperLink ID="btnAdd" CssClass="btnAdd" runat="server"></asp:HyperLink>
        <hr />
    </div>
     
    <div class="SubmitPanel"> 
        <asp:Button ID="SaveBtn" runat="server" ResourceKey="btnSave" CssClass="button"></asp:Button>
        <asp:Button ID="CancelBtn" runat="server" ResourceKey="btnCancel" CssClass="button"></asp:Button>
    </div>

</div>
