<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ViewPDF.ascx.vb" Inherits="DotNetNuke.Modules.AgapeDocuments.ViewPDF" %>
<%@ Register Assembly="PdfViewer" Namespace="PdfViewer" TagPrefix="cc1" %>
<cc1:ShowPdf ID="ShowPdf1" runat="server" Width="100%" height="600px" />


<%--<iframe id="pdfFrame" runat="server" style="width:100%; height:600px;" frameborder="1"></iframe>--%>
<br /><br />
<fieldset>
<legend class="AgapeH3">Your Comments</legend>
<div style="width: 100%;">
<table>
    <tr valign="top">
        <td width="40%" class="Agape_Body_Text" >
        We are continually looking to improve our resources and would appreciate your comments/sugestions. Please 
feel free to suggest alterations or ideas that would make this resource better.<br /><br />
       
  <asp:Panel ID="pnlAddComment" runat="server">
        <div style="font-family: verdana; font-size: 10pt; color: #808080">
Enter your own comment:
</div>
<br />
    <asp:TextBox ID="CommentText" runat="server" Rows="8" TextMode="MultiLine" 
        Width="100%"></asp:TextBox>
        <br />
        <br />
         <asp:ImageButton ID="AddCommentButton" runat="server" ImageUrl="~/images/ButtonImages/AddComment1.gif"  
                                                onmouseover="this.src='/images/ButtonImages/AddComment2.gif';"  
                                                onmouseout="this.src='/images/ButtonImages/AddComment1.gif';" AlternateText="Add Comment" ToolTip="Add Comment"     />
       
    </asp:Panel>
            <asp:Panel ID="pnlNotLoggedIn" runat="server" CssClass="Agape_SubTitle" visibe="false">
                To leave a comment, please login.
            </asp:Panel>
    </td>
    <td width="60%">
    <asp:DataList ID="DataList1" runat="server" DataKeyField="CommentId" 
        DataSourceID="CommentsDS">
        <ItemTemplate>
        <table>
            <tr>
                <td><asp:Label ID="lblName" runat="server" class="AgapeH5" Text='<%# UserController.GetUser(PortalId, Eval("UserId"), false).Firstname & " " & UserController.GetUser(PortalId, Eval("UserId"), false).Lastname %>' />:</td>
            </tr>
            <tr>
                <td> <asp:Label ID="lblComment" runat="server" class="Agape_SubTitle" Text='<%# Eval("Comment") %>' />
                <br />
                    <asp:LinkButton ID="LinkButton1" runat="server" Font-Size="8pt" Visible='<%# CanDelete(Eval("UserId")) %>' CssClass="Agape_Hyperlink" CommandName="DeleteComment" CommandArgument='<%# Eval("CommentId") %>' >Delete Comment</asp:LinkButton>
                
                </td>
            </tr>
        </table>
         
        </ItemTemplate>
    </asp:DataList>
    </td>
    </tr>
   </table>



    <asp:LinqDataSource ID="CommentsDS" runat="server" 
        ContextTypeName="AgapeDocuments.AgapeDocumentsDataContext" EnableDelete="True" 
        EnableInsert="True" EnableUpdate="True" 
        TableName="Agape_Main_AgapeDocuments_Comments" 
        Where="AgapeDocumentId == @AgapeDocumentId">
        <WhereParameters>
            <asp:QueryStringParameter DefaultValue="0" Name="AgapeDocumentId" 
                QueryStringField="DocId" Type="Int64" />
        </WhereParameters>
    </asp:LinqDataSource>



</div>

</fieldset>