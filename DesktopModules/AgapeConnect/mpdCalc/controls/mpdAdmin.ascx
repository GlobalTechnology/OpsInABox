<%@ Control Language="VB" AutoEventWireup="false" CodeFile="mpdAdmin.ascx.vb" Inherits="DesktopModules_AgapeConnect_mpdCalc_controls_mpdAdmin" %>

<style type="text/css">
    .form-horizontal .control-label {
        width: 200px;
    }

        .form-horizontal .control-label.conf {
            width: 180px;
            margin-left: 8px;
        }
        .form-horizontal .cell div {
        margin: 5px !important;
    }
    .cell .span8 label {
       display: inline;
position: relative;
margin-left: 5px;
top: 3px;
    }

    .cell .control-label {
        margin-right: 10px;
       
    }
    input, textarea, .uneditable-input{
        width: 70px ;
    }
</style>
<asp:HiddenField ID="hfMpdDefId" runat="server" />


<div class="alert alert-info" >
    <fieldset>
        <legend><h3><asp:Label ID="Label11" runat="server" ResourceKey="Title"/></h3></legend>

        <div class="form-horizontal"  >
            <div class="form-group cell"  >
               
                <asp:Label ID="Label1" runat="server"  for="rsgAccountsRoles"  ResourceKey="StaffWhoBudget" CssClass="span4 control-label"></asp:Label>
                <div class="span8" >
                     
                    <asp:CheckBoxList ID="cblStaffTypes" runat="server" RepeatLayout="Flow"></asp:CheckBoxList>
                </div>
            </div>
            <div class="form-group cell">
                <asp:Label ID="Label2" runat="server"  for="ddlAssessmentType"  ResourceKey="Assessment" CssClass="span4 control-label"></asp:Label>
                
                <div class="span8">
                    <asp:DropDownList ID="ddlAssessmentType" runat="server" class="form-control" Width="25%" >
                        <asp:ListItem Value="Percentage">Percentage</asp:ListItem>
                        <asp:ListItem Value="Formula">Formula</asp:ListItem>
                    </asp:DropDownList>
                    <asp:TextBox ID="tbAssessment" class="form-control"  runat="server" Width="70%"></asp:TextBox>
                </div>
                
            </div>
            <div class="form-group cell">
                <asp:Label ID="Label3" runat="server"  for="tbCompensation"  ResourceKey="Compensation" CssClass="span4 control-label"></asp:Label>
                <div class="span8">
                    <asp:DropDownList ID="ddlCompensationType" runat="server" class="form-control"  Width="25%">
                        <asp:ListItem Value="Percentage">Percentage</asp:ListItem>
                        <asp:ListItem Value="Formula">Formula</asp:ListItem>
                    </asp:DropDownList>
                    <asp:TextBox ID="tbCompensation" class="form-control"  runat="server" Width="70%"></asp:TextBox>
                </div>
                
            </div>
            <div class="form-group cell">
                <asp:Label ID="Label4" runat="server"  for="tbComplience"  ResourceKey="Complience" CssClass="span4 control-label"></asp:Label>
                <div class="span8">
                    
                    <asp:TextBox ID="tbComplience" class="form-control"  runat="server" Width="96%"></asp:TextBox>
                </div>
                
            </div>
            <div class="form-group cell">
                <asp:Label ID="Label5" runat="server"  for="tbDataserverURL"  ResourceKey="Dataserver" CssClass="span4 control-label"></asp:Label>
               
                <div class="span8">
                    
                    <asp:TextBox ID="tbDataserverURL" class="form-control"  runat="server" Width="80%" ></asp:TextBox>
                    <asp:LinkButton ID="btnTestDataserver" runat="server">Test</asp:LinkButton>
                    <asp:Image ID="imgOK" runat="server" ImageUrl="~/images/grant.gif"  Visible="false" />
                     <asp:Image ID="imgWarning" runat="server" ImageUrl="~/images/warning-icn.png"  Visible="false" />
                    <asp:Panel ID="pnlWarning" runat="server" Visible="false" CssClass="alert">
                    <asp:Label ID="lblWarning" runat="server"   ></asp:Label></asp:Panel>
                </div>
                
            </div>
             <div runat="server" id="pnlAccountCode" class="form-group cell" >
                <asp:Label ID="Label6" runat="server"  for="ddlAccount"  ResourceKey="AccountCode" CssClass="span4 control-label"></asp:Label>
               
                <div class="span8">
                    <asp:DropDownList ID="ddlAccount" runat="server">
                            </asp:DropDownList>
                </div>
                
            </div>
             <div class="form-group cell">
                <div class="span12">
                    
                    
                </div>
            </div>
             <div  class="form-group cell" >
                <asp:Label ID="Label7" runat="server"  for="ddlAuthUser"  ResourceKey="AuthUser" CssClass="span4 control-label"></asp:Label>
                
                <div class="span3">
                    <asp:DropDownList ID="ddlAuthUser" runat="server">
                    </asp:DropDownList>
                </div>
                <div class="help-inline mpd-help span5">
                   <asp:Label ID="Label9" runat="server"    ResourceKey="AuthUserHelp"></asp:Label>
                 
                </div>
            </div>
             <div class="form-group cell">
                <div class="span12">
                    
                    
                </div>
            </div>
            <div  class="form-group cell" >
                <asp:Label ID="Label8" runat="server"  for="ddlAuthAuthUser"  ResourceKey="AuthAuthUser" CssClass="span4 control-label"></asp:Label>
               
                <div class="span3">
                    <asp:DropDownList ID="ddlAuthAuthUser" runat="server">
                    </asp:DropDownList>
                </div>
                <div class="help-inline mpd-help span5">
                      <asp:Label ID="Label10" runat="server"    ResourceKey="AuthAuthUserHelp"></asp:Label>
               
                </div>
            </div>
            <div class="form-group cell">
                <div class="span12">
                    
                    
                </div>
            </div>
              <div class="form-group cell span12" style="text-align: center;">
              
                <asp:Button ID="btnUpdateConfig" runat="server" Text="Update" Font-Size="X-Large" CssClass="btn btn-primary" formnovalidate/>
            </div>
        </div>

          

    </fieldset>

</div>