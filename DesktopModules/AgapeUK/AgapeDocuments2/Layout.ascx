<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Layout.ascx.vb" Inherits="DotNetNuke.Modules.AgapeDocuments.Layout" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx"%>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>

<asp:HiddenField ID="ModuleIdHF" runat="server" />
<asp:HiddenField ID="PortalIdHF" runat="server" />

<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
    DataKeyNames="AgapeDocumentId" DataSourceID="LinksDS" Font-Size="9pt"  >
    <Columns>
       <asp:TemplateField ShowHeader="False">
            <ItemTemplate>
               <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" 
                    CommandName="Promote"  Text="Up" 
                    CommandArgument='<%# CInt(Eval("AgapeDocumentId")) %>' ImageUrl="~/images/up.gif"/>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField ShowHeader="False">
            <ItemTemplate>
                <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" 
                    CommandName="Demote"  Text="Down" 
                    CommandArgument='<%# CInt(Eval("AgapeDocumentId")) %>' ImageUrl="~/images/dn.gif"/>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="DocTitle" HeaderText="DocTitle" 
            SortExpression="DocTitle" />
        <asp:BoundField DataField="Subtitle" HeaderText="Group Title" 
            SortExpression="Subtitle" />
       
        <asp:TemplateField ShowHeader="False">
            <ItemTemplate>
            <div style="white-space: nowrap;">
                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" 
                    CommandName="Edit"  resourcekey="Edit"></asp:LinkButton>
                &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" 
                    CommandName="Delete" resourcekey="Delete"></asp:LinkButton>
                    </div>
            </ItemTemplate>
            <EditItemTemplate>
             <div style="white-space: nowrap;">
                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" 
                    CommandName="Update" resourcekey="Update"></asp:LinkButton>
                &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" 
                    CommandName="Cancel" resourcekey="Cancel"></asp:LinkButton>
                    </div>
            </EditItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<asp:LinqDataSource ID="LinksDS" runat="server" 
    ContextTypeName="AgapeDocuments.AgapeDocumentsDataContext" EnableDelete="True" 
    EnableUpdate="True" OrderBy="SortOrder" TableName="Agape_Main_AgapeDocuments" 
    Where="ModuleId == @ModuleId &amp;&amp; PortalId == @PortalId">
    <WhereParameters>
        <asp:ControlParameter ControlID="ModuleIdHF" Name="ModuleId" 
            PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="PortalIdHF" Name="PortalId" 
            PropertyName="Value" Type="Int32" />
    </WhereParameters>
</asp:LinqDataSource>
<br />

<table>
    <tr>
        <td><b><dnn:label id="lblColumns" runat="server" controlname="Columns"  resourcekey="lblColumns"  /></b></td>
        <td> 
            <asp:DropDownList ID="Columns" runat="server">
                <asp:ListItem>1</asp:ListItem>
                <asp:ListItem>2</asp:ListItem>
                <asp:ListItem>3</asp:ListItem>
                <asp:ListItem>4</asp:ListItem>
            </asp:DropDownList>  </td>
    </tr>

</table>

<asp:LinkButton ID="ReturnBtn" runat="server" resourcekey="btnUpdate"></asp:LinkButton> &nbsp; &nbsp;
<asp:LinkButton ID="CancelBtn" runat="server" resourcekey="Cancel"></asp:LinkButton>

