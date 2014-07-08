<%@ Control Language="VB" AutoEventWireup="False" CodeFile="GlobalProfile.ascx.vb" Inherits="DotNetNuke.Modules.AgapeConnect.GlobalProfile" %>
<link href="/Portals/_default/Skins/AgapeBlue/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
<style type="text/css">
    #gp_search .control-label {
        width: 100px;
    }
</style>
<asp:Label ID="lblTest" runat="server"></asp:Label><br />
<fieldset id="gp_search" class="span8">
    <legend><b>Search</b></legend>
    <div id="formRoot" class="form-horizontal">
        <div class="control-group ">
            <label class="control-label">First Name</label>
            <div class="controls">
                <asp:TextBox runat="server" ID="tbFirstNameSearch" />
            </div>
        </div>
        <div class="control-group ">
            <label class="control-label">Last Name</label>
            <div class="controls">
                <asp:TextBox runat="server" ID="tbLastNameSearch" />
            </div>
        </div>
       
        <div class="control-group ">
            <label class="control-label">Email</label>
            <div class="controls">
                <asp:TextBox runat="server" ID="tbEmailSearch" />
            </div>
        </div>

        <div class="control-group ">
            <label class="control-label">Assignment:</label>
            <div class="controls">
                <asp:DropDownList runat="server" ID="ddlRoleType" Width="150px" AppendDataBoundItems="true">
                </asp:DropDownList>

                <asp:DropDownList runat="server" ID="ddlMinistryLevel" AppendDataBoundItems="true">
                </asp:DropDownList>
            </div>
        </div>


        <div class="control-group ">
            <label class="control-label">Advanced</label>
            <div class="controls">
                <asp:TextBox runat="server" ID="tbAdvancedSearch" />
            </div>
        </div>
    </div>

    <asp:Button Text="Search" runat="server" ID="btnSearch" />

</fieldset>
<asp:Panel runat="server" Visible="false" ID="pnlResults">



    <fieldset id="gp_results" class="span4">
        <legend><b>Results</b></legend>
        <div class="bs-docs-example">
            <ul class="nav nav-pills nav-stacked">
                <asp:Repeater ID="rpResults" runat="server">
                    <ItemTemplate>
                        <li>
                            <asp:HiddenField ID="personId" runat="server" Value='<%# Eval("ID")%>' />
                            <asp:HyperLink ID="hlPersonName" runat="server" Text='<%# Eval("LastName") & ", " & Eval("FirstName") %>' NavigateUrl='<%# EditUrl("UserPage") & "?id=" & Eval("id") %>'></asp:HyperLink>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:Panel runat="server" ID="pnlPagination" Visible="false" class="pagination">
                    <ul>
                        <li id="prev" runat="server"><a href='<%= NavigateURL() & "?page=" & (pg - 1)%>'>Prev</a></li>
                        <asp:Literal ID="ltPages" runat="server"></asp:Literal>
                        <li id="next" runat="server"><a href='<%= NavigateURL() & "?page=" & (pg +1) %>'>Next</a></li>
                    </ul>
                   </asp:Panel>

        </div>
       
             






    </fieldset>

</asp:Panel>
