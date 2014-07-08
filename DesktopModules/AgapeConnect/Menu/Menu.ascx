<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Menu.ascx.vb" Inherits="DotNetNuke.Modules.Menu.Menu"%>
<asp:HiddenField ID="MenuModuleHF" runat="server" />
<asp:HiddenField ID="PortalIdHF" runat="server" />

<style type="text/css">
.Agape_Menu_IntroText
{
    color: #AAA;
    font-style: italic;
    font-size: xx-small;
}

</style>


    <asp:Panel ID="mainPanel" runat="server">
    


 <asp:Label ID="PrefixLabel" runat="server" CSSClass="Agape_Menu_IntroText"></asp:Label>  
               
    <asp:Repeater ID="MenuItemGrid" runat="server"  >
    <ItemTemplate>
     <asp:HyperLink ID="HyperLink1" runat="server" 
                                        NavigateUrl='<%# GetDocumnetLinkURL(Eval("TagName")) %>' Font-Bold='<%# IIF(Settings("BoldTitles")="",True, Settings("BoldTitles"))  %>'  Font-Names="verdana"  width="100%"
                                         Target='_self' Text='<%# CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Eval("TagName").ToLower) %>'  ForeColor='<%# GetColor %>'  Font-Size="10pt" >
                                     
                                       </asp:HyperLink>
    </ItemTemplate>
    </asp:Repeater>


    <asp:Repeater ID="ManualMenu" runat="server"  >
    <ItemTemplate>
     <asp:HyperLink ID="HyperLink1" runat="server" 
                                        NavigateUrl='<%# GetLinkURL(Eval("LinkId")) %>' Font-Bold="true"  Font-Names="verdana"  width="100%"
                                         Target='<%# Eval("Target") %>'  Text='<%# Eval("DisplayName") %>'  ForeColor='<%# GetColor %>'  Font-Size="10pt" >
                                     
                                       </asp:HyperLink>

        <asp:Panel ID="Panel1" runat="server" style="height: 5px;"  Visible='<%# Eval("LinkType") = 20 %>'>
        </asp:Panel>
    </ItemTemplate>
    </asp:Repeater>




                   
               <asp:Label ID="SuffixLabel" runat="server"  CssClass="Agape_Menu_IntroText"></asp:Label>                
         

      

 
<br />
<asp:LinkButton ID="EditButton" runat="server" Visible="false">Edit</asp:LinkButton>
</asp:Panel>
