<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SuperPowers.ascx.vb" Inherits="DesktopModules_SuperPowers" %>

<div id="SuperPowers"> 
    <div class="thisStory"> 
        <asp:Button ID="btnEdit" runat="server" ResourceKey="Edit" class="Button" />
        <asp:Button ID="btnPublish" runat="server" ResourceKey="Publish" class="Button"  />
        <asp:Button ID="btnUnPublish" runat="server" ResourceKey="Unpublish" class="Button"  />
    </div>
    <div class="boostBlock"> 
        <asp:Panel ID="pnlBoostBlock" runat="server" Visible="true">
            <input type="checkbox" id="boost" class="boost" /><label for="boost">Boost</label>
            <input type="checkbox" id="block" class="block" /><label for="block">Block</label>
        </asp:Panel>
    </div>

    <div class="newStory">
        <asp:Button ID="btnNew" runat="server" ResourceKey="New" class="Button"  />
    </div>
    <div class="powerStatus">
        <asp:Label ID="lblPowerStatus" runat="server"></asp:Label>
    </div>
</div>    