<%@ Control Language="VB" AutoEventWireup="false" CodeFile="EditOnlineForm.ascx.vb" Inherits="DotNetNuke.Modules.AgapeFR.OnlineForm.EditOnlineForm" %>


<asp:HiddenField ID="ModuleHF" runat="server" />
<asp:HiddenField ID="FormIdHF" runat="server" />
<table>
    
<tr>
<td width="100px">
    Introduction:
</td>
<td>
    <asp:TextBox ID="PrefixBox" runat="server" Rows="3" TextMode="MultiLine" Width="300px"></asp:TextBox>
</td>
</tr>
<tr>
<td width="100px">
    Footnote:
</td>
<td>
    <asp:TextBox ID="SuffixBox" runat="server" Rows="3" TextMode="MultiLine" Width="300px"></asp:TextBox>
</td>
</tr>
<tr>
<td> Email to</td>
<td><asp:TextBox ID="EmailTo" runat="server" Width="300px" /> </td>
</tr>
<tr>
<td> Ask for User's Email (and send acknowledgement)</td>
<td>
    <asp:CheckBox ID="Ack" runat="server" />
    </td>
</tr>
<tr>
<td> Acknowledgement Text</td>
<td><asp:TextBox ID="AckText" runat="server" Rows="3" TextMode="MultiLine" Width="300px" /> </td>
</tr>
<tr>
<td>Make Email Address a Required Field.</td>
<td>
    <asp:CheckBox ID="EmailReq" runat="server" /></td>
</tr>
<tr>
</table>

<br />
<asp:GridView ID="QuestionsGridView" runat="server" AutoGenerateColumns="False" 
    DataKeyNames="FormQuestionId" DataSourceID="QuestionsDS" ShowFooter="True">
    <Columns>
        <asp:TemplateField HeaderText="Question Text" SortExpression="QuestionText">
            <EditItemTemplate>
                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("QuestionText") %>'></asp:TextBox>
            </EditItemTemplate>
            <FooterTemplate>
                <asp:TextBox ID="FooterText" runat="server"></asp:TextBox>
            </FooterTemplate>
            <ItemTemplate>
                <asp:Label ID="Label1" runat="server" Text='<%# Bind("QuestionText") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Type" SortExpression="QuestionType">
            <EditItemTemplate>
                <asp:DropDownList ID="NewType" runat="server" SelectedValue='<%# Bind("QuestionType") %>'>
                    <asp:ListItem Value="0">Text Box</asp:ListItem>
                    <asp:ListItem Value="1">Multiline</asp:ListItem>
                    <asp:ListItem Value="2">Yes/No</asp:ListItem>
                    <asp:ListItem Value="3">DDL</asp:ListItem>
                    <asp:ListItem Value="4">CheckBox</asp:ListItem>
                    <asp:ListItem Value="5">Radio Button</asp:ListItem>
                    <asp:ListItem Value="6">Email Address</asp:ListItem>
                </asp:DropDownList>
            </EditItemTemplate>
            <FooterTemplate>
                <asp:DropDownList ID="FooterType" runat="server" >
                    <asp:ListItem Value="0">Text Box</asp:ListItem>
                    <asp:ListItem Value="1">Multiline</asp:ListItem>
                    <asp:ListItem Value="2">Yes/No</asp:ListItem>
                    <asp:ListItem Value="3">DDL</asp:ListItem>
                    <asp:ListItem Value="4">CheckBox</asp:ListItem>
                    <asp:ListItem Value="5">Radio Button</asp:ListItem>
                    <asp:ListItem Value="6">Email Address</asp:ListItem>
                </asp:DropDownList>
            </FooterTemplate>
            <ItemTemplate>
                <asp:DropDownList ID="NewType" runat="server" Enabled="False" 
                    SelectedValue='<%# Bind("QuestionType") %>'>
                    <asp:ListItem Value="0">Text Box</asp:ListItem>
                    <asp:ListItem Value="1">Multiline</asp:ListItem>
                    <asp:ListItem Value="2">Yes/No</asp:ListItem>
                    <asp:ListItem Value="3">DDL</asp:ListItem>
                    <asp:ListItem Value="4">CheckBox</asp:ListItem>
                    <asp:ListItem Value="5">Radio Button</asp:ListItem>
                    <asp:ListItem Value="6">Email Address</asp:ListItem>
                </asp:DropDownList>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Req" SortExpression="Required">
            <EditItemTemplate>
                <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("Required") %>' />
            </EditItemTemplate>
            <FooterTemplate>
                <asp:CheckBox ID="FooterReq" runat="server"  />
            </FooterTemplate>
            <ItemTemplate>
                <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("Required") %>' 
                    Enabled="false" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField ShowHeader="False">
            <EditItemTemplate>
                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" 
                    CommandName="Update" Text="Update"></asp:LinkButton>
                &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" 
                    CommandName="Cancel" Text="Cancel"></asp:LinkButton>
                &nbsp;<asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="False" 
                    CommandArgument='<%# Eval("FormQuestionId") %>' CommandName="DDL" Text="DDL"></asp:LinkButton>
            </EditItemTemplate>
            <FooterTemplate>
                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" 
                    CommandName="Insert" Text="Insert"></asp:LinkButton>
            </FooterTemplate>
            <ItemTemplate>
                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" 
                    CommandName="Edit" Text="Edit"></asp:LinkButton>
                &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" 
                    CommandName="Delete" Text="Delete"></asp:LinkButton>
                &nbsp;<asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="False" 
                    CommandArgument='<%# Eval("FormQuestionId") %>' CommandName="DDL" 
                    Text="DDL" Visible='<%# IIF(Eval("QuestionType")=3 or Eval("QuestionType")=5 , true, false) %>'></asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    <EmptyDataTemplate>
       <table>
       <tr>
            <td><asp:TextBox ID="NewText" runat="server"></asp:TextBox></td>
            <td><asp:DropDownList ID="NewType" runat="server">
                    <asp:ListItem Value="0">Text Box</asp:ListItem>
                    <asp:ListItem Value="1">Multiline</asp:ListItem>
                    <asp:ListItem Value="2">Yes/No</asp:ListItem>
                    <asp:ListItem Value="3">DDL</asp:ListItem>
                    <asp:ListItem Value="4">CheckBox</asp:ListItem>
                    <asp:ListItem Value="5">Radio Button</asp:ListItem>
                    <asp:ListItem Value="6">Email Address</asp:ListItem>
                </asp:DropDownList></td>
                <td><asp:CheckBox ID="NewReq" runat="server" /></td>
                <td><asp:LinkButton ID="AddNew" runat="server" CommandName="AddNew">Add</asp:LinkButton> </td>
       </tr>
       
       </table>
    </EmptyDataTemplate>
</asp:GridView>





<asp:LinqDataSource ID="QuestionsDS" runat="server" 
    ContextTypeName="AgapeFR.OnlineForm.OnlineFormDataContext" EnableDelete="True" 
    EnableInsert="True" EnableUpdate="True" 
    TableName="Agape_Public_OnlineForm_Questions" Where="FormId == @FormId">
    <WhereParameters>
        <asp:ControlParameter ControlID="FormIdHF" Name="FormId" PropertyName="Value" 
            Type="Int32" />
    </WhereParameters>
</asp:LinqDataSource>

<asp:Panel ID="DDLPanel" runat="server" Visible="false">
<br />
<fieldset>
<legend>DDL Values</legend>
    <asp:HiddenField ID="QuestionIdHF" runat="server"/>
    <table>
        <tr>
            <td>
                <asp:ListBox ID="DDLListBox" runat="server" DataSourceID="DDLDataSource" 
                    DataTextField="RowText" DataValueField="DDLRowId"></asp:ListBox>
                <asp:LinqDataSource ID="DDLDataSource" runat="server" 
                    ContextTypeName="AgapeFR.OnlineForm.OnlineFormDataContext" 
                    Select="new (DDLRowId, RowText)" TableName="Agape_Public_OnlineForm_DDLs" 
                    Where="QuestionId == @QuestionId">
                    <WhereParameters>
                        <asp:ControlParameter ControlID="QuestionIdHF" Name="QuestionId" 
                            PropertyName="Value" Type="Int32" />
                    </WhereParameters>
                </asp:LinqDataSource>
            
            
            </td>
            <td>
                <asp:TextBox ID="DDLText" runat="server"></asp:TextBox><asp:LinkButton ID="AddNewDDL" runat="server">Add</asp:LinkButton> <br />
                <asp:LinkButton ID="RemoveDDL" runat="server">Remove</asp:LinkButton>
            
            
            </td>
        </tr>
    </table>
    
    
    
</fieldset>
</asp:Panel>

<table style="font-family: verdana">
<tr>
<td>
    <asp:LinkButton ID="SaveMenuButton" runat="server">Update</asp:LinkButton>
</td>
<td>
    <asp:LinkButton ID="CancelButton" runat="server">Cancel</asp:LinkButton>
</td>
</tr>
</table>