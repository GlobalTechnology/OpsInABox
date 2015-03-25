<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ViewOnlineForm.ascx.vb" Inherits="DotNetNuke.Modules.AgapeFR.OnlineForm.ViewOnlineForm"%>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<dnn:DnnCssInclude ID="OnlineFormCSS" runat="server" FilePath="css/OnlineForm.css" PathNameAlias="SkinPath" />


<div class="OnlineForm">
    <div class="Menu_Prefix">
       <asp:Label ID="PrefixLabel" runat="server" Text=""></asp:Label>
    </div>
    <div>
       <asp:PlaceHolder ID="QuPlaceHolder" runat="server"></asp:PlaceHolder>
    </div>
    <asp:Panel ID="EmailPanel" runat="server">
        <div class="FieldRow">
            <asp:Label ID="LblEmailAddress" runat="server" ResourceKey="LblEmailAddress" CssClass="FieldLabel"></asp:Label>
            <asp:Label ID="EmailAddressStar" runat="server" ResourceKey="MandatoryField Star" CssClass="Star"></asp:Label><asp:TextBox ID="Email" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="ReqEmail" runat="server" resourcekey="ReqEmail.ErrorMessage" ControlToValidate="Email" Enabled="false"></asp:RequiredFieldValidator>
        </div>
    </asp:Panel>
    <div class="SubmitPanel">      
        <asp:Label ID="Star" runat="server" ResourceKey="Star" CssClass="Star"></asp:Label>&nbsp;<asp:Label ID="LblMandatoryFields" runat="server" resourcekey="LblMandatoryFields" />
        <br />
        <asp:Button ID="SubmitButton" runat="server" resourcekey="SubmitButton" CssClass="button" />
    </div>
    <div class="Menu_Suffix">
        <asp:Label ID="SuffixLabel" runat="server" Text=""></asp:Label>
    </div>
</div>

