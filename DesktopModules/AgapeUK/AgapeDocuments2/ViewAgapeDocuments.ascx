<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ViewAgapeDocuments.ascx.vb" Inherits="DotNetNuke.Modules.AgapeDocuments.ViewAgapeDocuments" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx"%>




<div style="background-color: White; padding: 25px; width: 450px; text-align: left">
<%--<asp:DataList
    ID="DataList1" runat="server" RepeatColumns="1"  Width="100%"
    RepeatDirection="Vertical">
    <ItemStyle VerticalAlign="Top"   />
    <ItemTemplate>
     
      <asp:HyperLink ID="HyperLink2" runat="server" Font-Names="Verdana" Font-Bold="true" Font-Size="14pt" CssClass="ToolboxLink" Text='<%# "<div style=""float: left; margin-right: 8px; margin-top: 8px;""><img src=""/Portals/_default/Skins/mToolbox/images/" & GetBulletName() & "Bullet.gif"" alt="">""  /></div><div style=""float: left; display: inline; width: 360px""><span class=""tbLinkText"" style=""color: " & GetForeColor() & ";"">" &  Eval("DocTitle") & "</span></div>" %>' Target="_blank" NavigateUrl='<%# GetDocument(Eval("FileId"), Eval("URL"), Eval("LinkType")) %>'></asp:HyperLink>
      
        
      
    
    </ItemTemplate>
</asp:DataList>--%>

<div style="color: #3f787d;">
<asp:TreeView ID="TreeView1" runat="server" NodeIndent="10"  
        CollapseImageUrl="ArrowWinD.gif" ExpandImageUrl="ArrowWinR.gif" 
         Font-Size="14pt"   ExpandDepth="0" RootNodeStyle-NodeSpacing="5px" NodeStyle-NodeSpacing="2px" NodeStyle-HorizontalPadding="5px">
           </asp:TreeView> 
</div>
<br /><br />


<asp:LinkButton ID="AddButton" runat="server" Visible="false" resourcekey="btnAdd" class="toolboxHyperlink" ></asp:LinkButton>&nbsp;&nbsp;<asp:LinkButton ID="EditBtn" runat="server" Visible="false" resourcekey="btnEdit"  class="toolboxHyperlink"></asp:LinkButton>
</div>