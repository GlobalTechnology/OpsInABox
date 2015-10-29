<%@ Control Language="VB" AutoEventWireup="false" CodeFile="AddDocument.ascx.vb" Inherits="DotNetNuke.Modules.AgapeConnect.Documents.AddDocument" %>

<div id="divUpload" style="text-align: center;">
    <asp:FileUpload id="FileUpload1" runat="server" />
    <br />
    <asp:Label ID="lblName" runat="server" />
    <asp:TextBox ID="tbName" runat="server"></asp:TextBox>
    <br />
    <asp:Label ID="lblDescription" runat="server" />
    <asp:TextBox ID="tbDescription" runat="server"></asp:TextBox>
    <br />
    <asp:Button ID="btnUploadFiles" runat="server" Text="upload" />
</div>