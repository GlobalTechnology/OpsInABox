<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ViewRmbTemplates.ascx.vb" Inherits="DotNetNuke.Modules.StaffRmb.viewRmbTemplates" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx"%>
<fieldset>
<legend><span class="Agape_Red_H3">  Rmb Splash Screen</span></legend>
<asp:FormView ID="FormView1" runat="server" DataKeyNames="TemplateId" 
    DataSourceID="TemplateDS" DefaultMode="Edit">
    <EditItemTemplate>
       
       
        <dnn:TextEditor ID="StoryText" runat="server" Height="600" Width="800" Text='<%# Bind("Template") %>' />
       
        <asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True" 
            CommandName="Update" Text="Update" />
    </EditItemTemplate>
   
</asp:FormView>
</fieldset>

<asp:LinqDataSource ID="TemplateDS" runat="server" 
    ContextTypeName="Resources.ResourcesDataContext" EnableUpdate="True" 
    TableName="Agape_Main_EmailTemplates" 
    Where="PortalId == @PortalId &amp;&amp; TemplateName == @TemplateName">
    <WhereParameters>
        <asp:Parameter DefaultValue="0" Name="PortalId" Type="Int32" />
        <asp:Parameter DefaultValue="RmbSplash" Name="TemplateName" 
            Type="String" />
    </WhereParameters>
</asp:LinqDataSource>


<fieldset>
<legend><span class="Agape_Red_H3">Rmb Confirmation</span></legend>
Tags:<br />
[RMBNO] -  The Reimbursement Number<br />
[USERREF] - Their (optional) User Reference<br />
[STAFFNAME] - The the name of the staff member<br />
[STAFFACTION] - instructions on what they must do<br />
[EXTRA] - Added information for if they have an unusual reimbursement<br />
[APPROVERS] - The list of peole who can approve this reimbursement<br />

<asp:FormView ID="FormView2" runat="server" DataKeyNames="TemplateId" 
    DataSourceID="ConfDS" DefaultMode="Edit">
    <EditItemTemplate>
       
       
        <dnn:TextEditor ID="StoryText" runat="server" Height="600" Width="800" Text='<%# Bind("Template") %>' />
       
        <asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True" 
            CommandName="Update" Text="Update" />
    </EditItemTemplate>
   
</asp:FormView>
</fieldset>

<asp:LinqDataSource ID="ConfDS" runat="server" 
    ContextTypeName="Resources.ResourcesDataContext" EnableUpdate="True" 
    TableName="Agape_Main_EmailTemplates" 
    Where="PortalId == @PortalId &amp;&amp; TemplateName == @TemplateName">
    <WhereParameters>
        <asp:Parameter DefaultValue="0" Name="PortalId" Type="Int32" />
        <asp:Parameter DefaultValue="RmbConf" Name="TemplateName" 
            Type="String" />
    </WhereParameters>
</asp:LinqDataSource>

<fieldset>
<legend><span class="Agape_Red_H3">Approver Email</span></legend>
Tags:<br />
[RMBNO] -  The Reimbursement Number<br />
[USERREF] - Their (optional) User Reference<br />
[STAFFNAME] - The the name of the staff member<br />
[STAFFACTION] - instructions on what they must do<br />
[APPROVERS] - The list of peole who can approve this reimbursement<br />

<asp:FormView ID="FormView3" runat="server" DataKeyNames="TemplateId" 
    DataSourceID="ApprDS" DefaultMode="Edit">
    <EditItemTemplate>
       
       
        <dnn:TextEditor ID="StoryText" runat="server" Height="600" Width="800" Text='<%# Bind("Template") %>' />
       
        <asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True" 
            CommandName="Update" Text="Update" />
    </EditItemTemplate>
   
</asp:FormView>
</fieldset>

<asp:LinqDataSource ID="ApprDS" runat="server" 
    ContextTypeName="Resources.ResourcesDataContext" EnableUpdate="True" 
    TableName="Agape_Main_EmailTemplates" 
    Where="PortalId == @PortalId &amp;&amp; TemplateName == @TemplateName">
    <WhereParameters>
        <asp:Parameter DefaultValue="0" Name="PortalId" Type="Int32" />
        <asp:Parameter DefaultValue="ApproverEmail" Name="TemplateName" 
            Type="String" />
    </WhereParameters>
</asp:LinqDataSource>

<fieldset>
<legend><span class="Agape_Red_H3">Approved Email</span></legend>
Tags:<br />
[RMBNO] -  The Reimbursement Number<br />
[USERREF] - Their (optional) User Reference<br />
[STAFFNAME] - The the name of the staff member<br />
[APPROVER] - The name of the person who approved this reimbursement<br />
[CHANGES] - the place where it records if the approver made any changes<br />

<asp:FormView ID="FormView4" runat="server" DataKeyNames="TemplateId" 
    DataSourceID="ApprovedDS" DefaultMode="Edit">
    <EditItemTemplate>
       
       
        <dnn:TextEditor ID="StoryText" runat="server" Height="600" Width="800" Text='<%# Bind("Template") %>' />
       
        <asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True" 
            CommandName="Update" Text="Update" />
    </EditItemTemplate>
   
</asp:FormView>
</fieldset>

<asp:LinqDataSource ID="ApprovedDS" runat="server" 
    ContextTypeName="Resources.ResourcesDataContext" EnableUpdate="True" 
    TableName="Agape_Main_EmailTemplates" 
    Where="PortalId == @PortalId &amp;&amp; TemplateName == @TemplateName">
    <WhereParameters>
        <asp:Parameter DefaultValue="0" Name="PortalId" Type="Int32" />
        <asp:Parameter DefaultValue="ApprovedEmail" Name="TemplateName" 
            Type="String" />
    </WhereParameters>
</asp:LinqDataSource>
<fieldset>
<legend><span class="Agape_Red_H3">Approved Email - Approvers Version</span></legend>
Tags:<br />
[RMBNO] -  The Reimbursement Number<br />
[STAFFNAME] - The the name of the staff member<br />
[APPRNAME] - The approver who approved this reimbursement<br />
[THISAPPRNAME] - The person this email is sent to<br />

<asp:FormView ID="FormView5" runat="server" DataKeyNames="TemplateId" 
    DataSourceID="ApprovedApprDS" DefaultMode="Edit">
    <EditItemTemplate>
       
       
        <dnn:TextEditor ID="StoryText" runat="server" Height="600" Width="800" Text='<%# Bind("Template") %>' />
       
        <asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True" 
            CommandName="Update" Text="Update" />
    </EditItemTemplate>
   
</asp:FormView>
</fieldset>

<asp:LinqDataSource ID="ApprovedApprDS" runat="server" 
    ContextTypeName="Resources.ResourcesDataContext" EnableUpdate="True" 
    TableName="Agape_Main_EmailTemplates" 
    Where="PortalId == @PortalId &amp;&amp; TemplateName == @TemplateName">
    <WhereParameters>
        <asp:Parameter DefaultValue="0" Name="PortalId" Type="Int32" />
        <asp:Parameter DefaultValue="ApprovedApprEmail" Name="TemplateName" 
            Type="String" />
    </WhereParameters>
</asp:LinqDataSource>
<br /><br />
<asp:LinkButton ID="btnReutrn" runat="server">Return</asp:LinkButton>