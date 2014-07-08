<%@ Control Language="VB" AutoEventWireup="False" CodeFile="UserPage.ascx.vb" Inherits="DotNetNuke.Modules.AgapeConnect.gr_mapping_mod" %>
<link href="/Portals/_default/Skins/AgapeBlue/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
<script type="text/javascript" src="https://www.google.com/jsapi"></script>
<script type="text/javascript">
   
    google.load("visualization", "1", { packages: ["corechart"] });
    google.setOnLoadCallback(drawChart);

   

    function drawChart() {
         <%= graphScript %>
     
        
    }

    $(function () {

        $(".datepicker").datepicker({ dateFormat: "yy-mm-dd" });




    });

</script>
<style type="text/css">
    #gp_search .control-label {
        width: 100px;
    }
</style>


<asp:Label ID="lblText" runat="server" Text=""></asp:Label>
<%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>--%>
<fieldset id="gp_search" class="span6">
    <legend><b>
        <asp:Label ID="lblTitle" runat="server" Font-Size="XX-Large"></asp:Label></b></legend>
    <div id="formRoot" class="form-horizontal">
        <div class="control-group ">
            <label class="control-label">First Name</label>
            <div class="controls">
                <asp:TextBox runat="server" ID="tbFirstName" />
            </div>
        </div>
        <div class="control-group ">
            <label class="control-label">Last Name</label>
            <div class="controls">
                <asp:TextBox runat="server" ID="tbLastName" />
            </div>
        </div>
        <div class="control-group ">
            <label class="control-label">Preferred Name</label>
            <div class="controls">
                <asp:TextBox runat="server" ID="tbPreferredName" />
            </div>
        </div>
        <div class="control-group ">
            <label class="control-label">Gender</label>
            <div class="controls">
                <asp:DropDownList ID="ddlGender" runat="server">
                    <asp:ListItem Text="Unknown" Value="" />
                    <asp:ListItem Text="Male" />
                    <asp:ListItem Text="Female" />
                </asp:DropDownList>
            </div>
        </div>
        <div class="control-group ">
            <label class="control-label">Marital Status</label>
            <div class="controls">
                <asp:DropDownList ID="ddlMaritalStatus" runat="server" Width="100px">
                    <asp:ListItem Text="Unknown" Value="" />
                    <asp:ListItem Text="Single" />
                    <asp:ListItem Text="Married" />
                </asp:DropDownList>

                <asp:HyperLink ID="hlSpouse" runat="server"></asp:HyperLink>

            </div>
        </div>
        <div class="control-group ">
            <label class="control-label">Email</label>
            <div class="controls">
                <asp:TextBox runat="server" ID="tbEmail" />
            </div>
        </div>
        <div class="control-group ">
            <label class="control-label">Birthday</label>
            <div class="controls">
                <asp:TextBox runat="server" ID="tbBirthday" CssClass="datepicker" />
            </div>
        </div>

        <fieldset>
            <legend><b>Address</b></legend>
            <div class="control-group ">
                <label class="control-label">Address1</label>
                <div class="controls">
                    <asp:TextBox runat="server" ID="tbAddress1" />
                </div>
            </div>
            <div class="control-group ">
                <label class="control-label">Address2</label>
                <div class="controls">
                    <asp:TextBox runat="server" ID="tbAddress2" />
                </div>
            </div>
            <div class="control-group ">
                <label class="control-label">City</label>
                <div class="controls">
                    <asp:TextBox runat="server" ID="tbCity" />
                </div>
            </div>
            <div class="control-group ">
                <label class="control-label">State</label>
                <div class="controls">
                    <asp:TextBox runat="server" ID="tbState" />
                </div>
            </div>
            <div class="control-group ">
                <label class="control-label">PostalCode</label>
                <div class="controls">
                    <asp:TextBox runat="server" ID="tbPostalCode" />
                </div>
            </div>
            <div class="control-group ">
                <label class="control-label">Country</label>
                <div class="controls">
                    <asp:TextBox runat="server" ID="tbCountry" />
                </div>
            </div>
        </fieldset>
        <fieldset>
            <legend><b>Assignments</b></legend>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="control-group ">
                        <label class="control-label">Ministry Scope:</label>
                        <div class="controls">
                            <asp:DropDownList runat="server"  ID="ddlMinistryLevel" AppendDataBoundItems="true" AutoPostBack="true">
                                <asp:ListItem Text="" />
                            </asp:DropDownList>

                        </div>
                    </div>
                    <div class="control-group ">
                        <label class="control-label">Ministry</label>
                        <div class="controls">
                            <asp:DropDownList runat="server" ID="ddlMinistry" AppendDataBoundItems="true">
                                <asp:ListItem Text="" />
                            </asp:DropDownList>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddlMinistryLevel" EventName="SelectedIndexChanged" />
                    <asp:PostBackTrigger ControlID="btnSave" />
                </Triggers>
            </asp:UpdatePanel>
            <div class="control-group ">
                <label class="control-label">Role</label>
                <div class="controls">
                    <asp:DropDownList runat="server" ID="ddlRoleType" AppendDataBoundItems="true">
                        <asp:ListItem Text="" />
                    </asp:DropDownList>
                </div>
            </div>

        </fieldset>

    </div>
    <asp:Button ID="btnSave" runat="server" Text="Update" CssClass="btn btn-primary" />


</fieldset>
<fieldset class="span6">
    <legend><b>Related Graphs</b></legend>


    
    <asp:PlaceHolder ID="phGraphs" runat="server"></asp:PlaceHolder>

</fieldset>
