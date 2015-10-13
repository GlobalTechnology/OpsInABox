<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ViewAgapeDocuments.ascx.vb" Inherits="DotNetNuke.Modules.AgapeDocuments.ViewAgapeDocuments" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx"%>




<asp:DataList
    ID="DataList1" runat="server" RepeatColumns="1" 
    RepeatDirection="Horizontal">
    <ItemStyle VerticalAlign="Top"  />
    <ItemTemplate>
        <table >
            <tr valign="top" >
                <td>

               <asp:Image ID="Image1" runat="server" ImageUrl='<%# GetIcon( Eval("FileId"), Eval("URL"), Eval("LinkType")) %>' />
          
                 </td>
                 
                 <td>
                    <p>  </p>
                 </td>
                 
                 <td >
                        <p style="font-size: 2pt;">
                         <br />
                        </p>
                 <asp:HyperLink ID="HyperLink1" runat="server" Font-Names="Verdana" Font-Bold="true" Font-Size="14pt" ForeColor="#660000" Text='<%# Eval("DocTitle") %>' Target='<%# IsNewWindow(Eval("FileId"), Eval("LinkType")) %>' NavigateUrl='<%# GetDocument(Eval("AgapeDocumentId"),Eval("FileId"), Eval("URL"), Eval("LinkType")) %>'></asp:HyperLink>
               
                        <br />
                        
                        <asp:Label ID="Label2" runat="server"  CssClass="Agape_Subtitle" style="margin-bottom: 5px;"
                            Text='<%# Eval("SubTitle") %>'></asp:Label><br />
                        <asp:Label ID="Label1" runat="server" CssClass="Agape_Body_Text" Font-Size="8pt"
                            Text='<%# Server.HTMLDecode(Eval("DOcDescription")) %>'></asp:Label> 
                 </td>
            </tr>
        </table>
        
      
    
    </ItemTemplate>
</asp:DataList>

<br /><br />


<asp:LinkButton ID="AddButton" runat="server" Visible="false" resourcekey="btnAdd"></asp:LinkButton>&nbsp;&nbsp;<asp:LinkButton ID="EditBtn" runat="server" Visible="false" resourcekey="btnEdit"></asp:LinkButton>