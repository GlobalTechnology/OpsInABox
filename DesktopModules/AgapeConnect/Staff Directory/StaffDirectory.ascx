<%@ Control Language="VB" AutoEventWireup="False" CodeFile="StaffDirectory.ascx.vb" Inherits="DotNetNuke.Modules.StaffDirectory.StaffDirectory" %>
<%@ Register Src="~/controls/labelcontrol.ascx" TagName="labelcontrol" TagPrefix="uc1" %>
<script src="/js/jquery.watermarkinput.js" type="text/javascript"></script>

<script type="text/javascript">

    (function ($, Sys) {
        function setUpMyTabs() {
            
     
            $('.Watermark').Watermark('<%= Translate("Search") %>');

        }

        $(document).ready(function () {
            setUpMyTabs();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                setUpMyTabs();
            });
        });
    } (jQuery, window.Sys));

   
</script>

<table width="100%">
<tr valign="top">
    <td style="padding-right:10px"> 
    <asp:Panel ID="Panel1" runat="server" DefaultButton="SearchBtn">

      <table width="100%">
    <tr valign="middle">
        <td width="100%"><asp:TextBox ID="SearchBox" CssClass="Watermark" runat="server" Width ="140px" Font-Size="8pt" ></asp:TextBox></td>
        <td><asp:ImageButton ID="SearchBtn" runat="server" ImageUrl="~/Portals/_default/Skins/AgapeBlue/images/search.gif" ToolTip="Search"  /></td>
    </tr>
</table>
     </asp:Panel>



        <asp:ListBox ID="ListBox1" runat="server"  
    DataTextField="DisplayName" DataValueField="UserId" AutoPostBack="true" Width="164px" height="450px"></asp:ListBox>
       
  

    
    </td>
    <td width="100%">
        <asp:Panel ID="ContactPanel" runat="server">
            <fieldset>
                <legend class="AgapeH3">
                    <asp:Label ID="FirstName" runat="server"></asp:Label>
                    <asp:Label ID="LastName" runat="server"></asp:Label></legend>
                <table cellspacing="10px" width="100%">
                    <tr valign="top">
                        <td style="white-space: nowrap;" width="50%">
                            <asp:DataList ID="dlProfileProps" runat="server" Width="100%" CellSpacing="15">
                                <ItemTemplate>
                                    <asp:Label ID="lblCatHeader" runat="server" class="AgapeH4" Text='<%# Eval("PropertyCategory") & ":" %>'></asp:Label>
                                    <asp:DataList ID="dlProfilePropsInner" runat="server" Width="100%" DataSource='<%# Eval("Props")%>'>
                                        <ItemTemplate>
                                            <table width="100%" align="left">
                                                <tr>
                                                    <td width="100px">
                                                        <asp:Label ID="lbcPropName" runat="server" Font-Bold="true" Text='<%# Localization.GetString("ProfileProperties_" & Eval("PropertyName") & ".Text", "/DesktopModules/Admin/Security/App_LocalResources/Profile.ascx.resx", System.Threading.Thread.CurrentThread.CurrentCulture.Name)  %>'
                                                            Width="100px" />
                                                    </td>
                                                    <td  align="left">
                                                        <asp:Label ID="lblPropValue" runat="server" Text='<%# GetProfileValue(Eval("PropertyName"), False,  Eval("DataType") ) %>'></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                    </asp:DataList>
                                </ItemTemplate>
                            </asp:DataList>
                        </td>
                        <td width="50%">
                            <div style="padding: 17px">
                                <asp:Panel ID="FamilyPanel" runat="server">
                                     <asp:Label ID="Label8" runat="server" CssClass="AgapeH4"  ResourceKey="lblFamily" Text="Family"></asp:Label>
                                    <table>
                                        <tr>
                                            <td width="100px">
                                                <asp:Label ID="Label7" runat="server" Font-Bold="true" ResourceKey="lblMarriedTo" Text="Married To"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Spouse" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:Panel ID="ChildrenPanel" runat="server" style="padding-left: 3px;" >
                                        <asp:Label ID="Label3" runat="server" Font-Bold="true" ResourceKey="lblChildren" Text="Children"></asp:Label>
                                        <table style="margin-left: 30px;" >
                                            <tr style="color: #999;">
                                                <td width="70px">
                                                    <asp:Label ID="Label4" runat="server" Font-Bold="true" ResourceKey="lblName" Text="Name"></asp:Label>
                                                </td>
                                                <td width="70px">
                                                   <asp:Label ID="Label5" runat="server" Font-Bold="true" ResourceKey="lblBirthday" Text="Birthday"></asp:Label>
                                                </td>
                                                <td width="70px">
                                                    <asp:Label ID="Label6" runat="server" Font-Bold="true" ResourceKey="lblAge" Text="Age"></asp:Label>
                                                </td>
                                            </tr>
                                            <asp:PlaceHolder ID="ChildrenPH" runat="server"></asp:PlaceHolder>
                                        </table>
                                    </asp:Panel>
                                </asp:Panel>
                                <div style="margin-top: 15px;" >
                                 <asp:Label ID="Label2" runat="server" CssClass="AgapeH4"  ResourceKey="lblEmployment" Text="Employment"></asp:Label>
                                </div>

                                    <table>
                                        <tr valign="top">
                                            <td width="100px" >
                                                <asp:Label ID="Label1" runat="server" Font-Bold="true" ResourceKey="lblReportsTo" Text="Reports to"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblReportsTo" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>

                                <asp:DataList ID="dlStaffProps" runat="server" Width="100%">
                                    <ItemTemplate>
                                        <table width="100%" align="left">
                                            <tr>
                                                <td width="100px">
                                                    <asp:Label ID="lbcPropName" runat="server" Font-Bold="true" Text='<%# Eval("Key") & ":" %>'
                                                        Width="100px" />
                                                </td>
                                                <td width="100%" align="left">
                                                    <asp:Label ID="lblPropValue" runat="server" Text='<%# Eval("Value") %>'></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:DataList>
                            </div>
                        </td>
                        <td width="200px;">
                            <asp:Image ID="imgProfileImage" runat="server" Width="200px" BorderColor="Black" BorderWidth="2pt" BorderStyle="Solid" />
                        </td>
                    </tr>
                </table>
            </fieldset>
        </asp:Panel>
    </td>
</tr>
</table>

<asp:LinkButton ID="btnSettings" runat="server" ResourceKey="Settings">Settings</asp:LinkButton>