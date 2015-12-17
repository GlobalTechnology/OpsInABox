<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ViewOnlineForm.ascx.vb" Inherits="DotNetNuke.Modules.AgapeFR.OnlineForm.ViewOnlineForm"%>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<dnn:DnnCssInclude ID="OnlineFormCSS" runat="server" FilePath="css/OnlineForm.css" PathNameAlias="SkinPath" />

<script type="text/javascript">

    function FreezeButton() {
        window.setTimeout("disableButton('" + window.event.srcElement.id + "')", 0);
        window.setTimeout("enableButton('" + window.event.srcElement.id + "')", 1000);
    }

    function disableButton(buttonID) {
        document.getElementById(buttonID).disabled = true;
    }
    function enableButton(buttonID) {
        document.getElementById(buttonID).disabled = false;
    }
</script>


<div class="OnlineForm">
    <div class="Menu_Prefix">
       <asp:Label ID="PrefixLabel" runat="server" Text=""></asp:Label>
    </div>
    <div class="FieldRow">
    <asp:Panel ID="EmailPanel" runat="server">
            <asp:Label ID="LblEmailAddress" runat="server" ResourceKey="LblEmailAddress" CssClass="FieldLabel"></asp:Label>
            <asp:Label ID="EmailAddressStar" runat="server" CssClass="MandatoryFieldEmailAck Star"></asp:Label><asp:TextBox ID="EmailWithAck" runat="server"></asp:TextBox>    
    </asp:Panel>
    </div>
     <div>
       <asp:PlaceHolder ID="QuPlaceHolder" runat="server"></asp:PlaceHolder>
    </div>
    <div class="SubmitPanel">      
        <asp:Label ID="Star" runat="server" ResourceKey="Star" CssClass="Star"></asp:Label>&nbsp;<asp:Label ID="LblMandatoryFields" runat="server" resourcekey="LblMandatoryFields" />
        <br />
        <asp:Button ID="SubmitButton" runat="server" resourcekey="SubmitButton" CssClass="button" OnClientClick="FreezeButton()"/>
    </div>
    <div class="Menu_Suffix">
        <asp:Label ID="SuffixLabel" runat="server" Text=""></asp:Label>
    </div>
</div>

