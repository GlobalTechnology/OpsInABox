<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Templates.ascx.vb" Inherits="DotNetNuke.Modules.StaffAdmin.TemplateManager" %>


 <%@ Register src="../../../controls/labelcontrol.ascx" tagname="labelcontrol" tagprefix="uc1" %>
 <%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>

<asp:HiddenField ID="hfPortalID" runat="server" />
 <table>
    <tr>
        <td>
            <uc1:labelcontrol ID="labelcontrol1" runat="server" Text="Template Name" HelpText="Select a template to edit" />
        </td>
        <td>
        
     <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="dsTemplates" AutoPostBack="true"
                DataTextField="TemplateName" DataValueField="TemplateId">
     </asp:DropDownList>
            <asp:LinqDataSource ID="dsTemplates" runat="server" 
                ContextTypeName="StaffBroker.TemplatesDataContext" EntityTypeName="" 
                TableName="AP_StaffBroker_Templates" Where="PortalId == @PortalId">
                <WhereParameters>
                    <asp:ControlParameter ControlID="hfPortalID" Name="PortalId" 
                        PropertyName="Value" Type="Int32" />
                </WhereParameters>
            </asp:LinqDataSource>
        </td>
    </tr>
 </table>
<asp:FormView ID="FormView1" runat="server" DataKeyNames="TemplateId" 
    DataSourceID="dsTheTemplate" DefaultMode="Edit">
    <EditItemTemplate>
    <fieldset>
        <legend><asp:Label ID="lblTemplateName" runat="server" CssClass="AgapeH3" Text='<%# Bind("TemplateName") %>'></asp:Label></legend>
        <table>
            <tr>
                <td> <uc1:labelcontrol ID="labelcontrol1" runat="server" Text="Description:" HelpText="Enter a brief description of the temple, along with instructions regarding replacement tags" />
                </td>
                <td>

                    <asp:TextBox ID="TextBox2" runat="server" Width="90%" height="140px" TextMode="MultiLine" Text='<%# Bind("TemplateDescription") %>' style="margin-left: 24px;  background-color: #fff9c8;   " ></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td> <uc1:labelcontrol ID="labelcontrol2" runat="server" Text="Template:" HelpText="Enter the template content here." />
                </td>
                <td>
                    <dnn:TextEditor ID="TextEditor1" runat="server" Width="100%" TextRenderMode="Raw"  Text='<%# Bind("TemplateHTML") %>' HtmlEncode="False" defaultmode="Rich" height="440" choosemode="True" chooserender="False"   />
                </td>
            </tr>
        </table>

 
    </fieldset>
        
        <br />
        <asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True" 
            CommandName="Update" Text="Update" />
        &nbsp;<asp:LinkButton ID="UpdateAddButton" runat="server" 
            CausesValidation="False" CommandName="New" Text="New" />
            &nbsp;<asp:LinkButton ID="UpdateDeleteButton" runat="server" 
            CausesValidation="False" CommandName="Delete" Text="Delete" />
    </EditItemTemplate>
    <InsertItemTemplate>
        <fieldset>
        <legend><asp:Label ID="lblTemplateName" runat="server" CssClass="AgapeH3" Text="New Template"></asp:Label></legend>
        <table>
            <tr>
                <td> <uc1:labelcontrol ID="labelcontrol3" runat="server" Text="Name:" HelpText="Enter a unique name for this template." />
                </td>
                <td>
                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("TemplateName") %>' style="margin-left: 23px;" width="300px" Font-Size="12pt" Font-Bold="true"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td> <uc1:labelcontrol ID="labelcontrol1" runat="server" Text="Desciption:" HelpText="Enter a brief description of the temple, along with instructions regarding replacement tags" />
                </td>
                <td>
                     <asp:TextBox ID="TextBox2" runat="server" Width="90%" height="140px" TextMode="MultiLine" Text='<%# Bind("TemplateDescription") %>' style="margin-left: 24px;  background-color: #fff9c8;   " ></asp:TextBox>
                      </td>
            </tr>
            <tr>
                <td> <uc1:labelcontrol ID="labelcontrol2" runat="server" Text="Template:" HelpText="Enter the template content here." />
                </td>
                <td>
                    <dnn:TextEditor ID="tbInsertTemplate" runat="server" Width="100%" TextRenderMode="Raw"  Text='<%# Bind("TemplateHTML") %>' HtmlEncode="False" defaultmode="Rich" height="440" choosemode="True" chooserender="False"   />
                </td>
            </tr>
        </table>

    </fieldset>


    <br />
        <asp:LinkButton ID="InsertButton" runat="server" CausesValidation="True" 
            CommandName="Insert" Text="Insert" />
        &nbsp;<asp:LinkButton ID="InsertCancelButton" runat="server" 
            CausesValidation="False" CommandName="Cancel" Text="Cancel" />
    </InsertItemTemplate>
    <ItemTemplate>
        
    </ItemTemplate>
    
 </asp:FormView>
 
 
 <asp:LinqDataSource ID="dsTheTemplate" runat="server" 
    ContextTypeName="StaffBroker.TemplatesDataContext" EnableDelete="True" 
    EnableInsert="True" EnableUpdate="True" EntityTypeName="" 
    TableName="AP_StaffBroker_Templates" Where="TemplateId == @TemplateId">
     <WhereParameters>
         <asp:ControlParameter ControlID="DropDownList1" Name="TemplateId" 
             PropertyName="SelectedValue" Type="Int64" />
     </WhereParameters>
</asp:LinqDataSource>
 
 <br /><br />
 <fieldset>
    <legend class="AgapeH3">Preview</legend>
     <asp:Literal ID="litPreview" runat="server"></asp:Literal>




 </fieldset>
 
 