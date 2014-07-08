<%@ Control Language="VB" AutoEventWireup="false" CodeFile="FCX_API_Console.ascx.vb" Inherits="DotNetNuke.Modules.AgapeConnect.FCX_API_Console" %>

<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>

<script src="/js/jquery.watermarkinput.js" type="text/javascript"></script>

<script type="text/javascript">
    $(document).ready(function () {


        $(".cb-enable").click(function () {
            var parent = $(this).parents('.switch');
            $('.cb-disable', parent).removeClass('selected');
            $(this).addClass('selected');

        });
        $(".cb-disable").click(function () {
            var parent = $(this).parents('.switch');
            $('.cb-enable', parent).removeClass('selected');
            $(this).addClass('selected');

        });
        $('.aButton').button();
        $('.ProductName').Watermark('Product Name');
    });

</script>
<style type="text/css">
   .cb-enable, .cb-disable, .cb-enable span, .cb-disable span { background: url(/DesktopModules/AgapeConnect/FCX_API_Console/switch.gif) repeat-x; display: block; float: left; }
    .cb-enable span, .cb-disable span { line-height: 30px; display: block; background-repeat: no-repeat; font-weight: bold; }
    .cb-enable span { background-position: left -90px; padding: 0 10px; }
    .cb-disable span { background-position: right -180px;padding: 0 10px; }
    .cb-disable.selected { background-position: 0 -30px; }
    .cb-disable.selected span { background-position: right -210px; color: #fff; }
    .cb-enable.selected { background-position: 0 -60px; }
    .cb-enable.selected span { background-position: left -150px; color: #fff; }
    .switch label { cursor: pointer; }
    .switch input { display: none; }

</style>

<asp:HiddenField ID="hfPortalId" runat="server" />
<fieldset>
    
    <legend><span class="AgapeH3">Your API-Keys</span></legend>
   <p>
        The FCX-API is a system that allows remote applications to generate transactions in Dynamics/Donorwise. 
        The webservices are located at <a href="https://<%= PortalSettings.DefaultPortalAlias%>/FCX/FCX-API.asmx">https://<%= PortalSettings.DefaultPortalAlias%>/FCX/FCX-API.asmx</a>. 
        In order to use the webservice, the remote application must have its own API-KEY. 
        When accessing the API, the call must come from a URL on the whitelist. 
        Once the transaction has been processed, the remote application will be notified at the URL you specify for ITN (Instant Transaction Notification).</p>
        <p>
        <i>(*Note: If you reissue an API key, your existing application will not be able to access the API until you configure it to use the new key.)</i>
   </p>
     <table border="0" cellpadding="5" cellspacing="0" width="100%">
                <tr style="background-color: #507CD1; color: White; font-size: large;">
                    <td  width="205px">
                       <b>Name</b>
                    </td>
                    <td width="430px">
                      <b>API-KEY / ITN</b>
                        
                    </td>
                    <td width="273px">
                      <b>Whitelist</b>
                    </td>
                    <td></td>
                </tr>
            </table>
    <asp:DataList ID="DataList1" runat="server" DataSourceID="dsAPIKeys" 
        CellPadding="4" ForeColor="#333333" >
        <AlternatingItemStyle BackColor="White" />
        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <ItemStyle BackColor="#EFF3FB" />
        <ItemTemplate>
         

            <table border="0" cellpadding="0" cellspacing="5">
                <tr>
                    <td  width="200px">
                        <asp:Label ID="lblName" runat="server" Font-Bold="true" Font-Size="Large" Text='<%# Eval("ProductName") %>'></asp:Label>
                 
                    </td>
                    <td width="440px">
                        <table border="0" cellpadding="0" cellspacing="4">
                            <tr>
                                <td>
                                <b><dnn:label id="ttlAPIKey" runat="server"  Text="API-KEY:" HelpText="The API-KEY gives remote systems access to your accounts via the FCX-API."   /></b>
                                </td>
                                <td>
                                    <asp:Label ID="lblAPIKEY" runat="server" Text='<%# Eval("API_KEY") %>'></asp:Label> <br />
                                </td>
                                <td>
                                    <asp:LinkButton ID="lbReIssue" runat="server"  Font-Size="Smaller" CommandName="ReKey" CommandArgument='<%# Eval("DeveloperId") %>'>Reissue</asp:LinkButton>
                                
                                </td>
                            </tr>
                            
                             <tr>
                                <td>
                                 <b><dnn:label id="Label3" runat="server"  Text="ITN:" HelpText="Instant Transaction Notification is the system by which the FCX-API notifies you when a batch has been processed (and any changes to its status). A list of batches (in JSON format) will be posted to the URL you enter here."   /></b>
                                </td>
                                <td >
                                <asp:TextBox ID="tbITN" runat="server" Text='<%# Bind("ITN") %>' width="100%"></asp:TextBox>
                                </td>
                                <td>
                                <asp:LinkButton ID="LinkButton1" runat="server" Font-Size="Smaller" CommandName="SaveITN" CommandArgument='<%# Eval("DeveloperId") %>' >Change</asp:LinkButton>
                                </td>

                            </tr>
                        </table>
                        
                    </td>
                    <td width="270px">
                        <asp:GridView ID="gvWhiteList" runat="server" GridLines="None" OnRowCommand="WhilstlistCommand"   AutoGenerateColumns="false" ShowHeader="false"
                            DataSource='<%# (From c as string in CStr(Eval("WhiteList")).Split(",") select Value = c.Trim, DeveloperId=Eval("DeveloperId")) %>' >
                            <Columns>
                                <asp:BoundField DataField="Value" ItemStyle-Font-Bold="true" ItemStyle-Width="200px" ItemStyle-Font-Size="Smaller"   />
                       
                                <asp:TemplateField>
                                    <ItemTemplate>
                                         <asp:LinkButton ID="LinkButton1" runat="server" Font-Size="XX-Small" CommandName="Remove" CommandArgument='<%# Eval("DeveloperId") & ";" &  Eval("Value")  %>' >Remove</asp:LinkButton>
                               
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:TextBox ID="tbAddWhiteList" runat="server" Font-Size="Smaller" Width="200px" ></asp:TextBox>
                        <asp:LinkButton ID="lbAddWhiteList" runat="server" CommandName="AddWhitelist" CommandArgument='<%# Eval("DeveloperId") %>'>Add</asp:LinkButton>
                    </td>
                   <td>
                       

                       <asp:Panel ID="Panel1" runat="server"  Visible='<%#  Eval("Active") %>' class="field switch"   style="white-space: nowrap ;"  >
                            <asp:LinkButton ID="LinkButton4" runat="server" CssClass="cb-enable selected" CommandName="Enable" Enabled="false"  CommandArgument='<%# Eval("DeveloperId") %>'  ><span>On</span></asp:LinkButton>
                            <asp:LinkButton ID="LinkButton5" runat="server"  CssClass="cb-disable"  CommandName="Disable"   CommandArgument='<%# Eval("DeveloperId") %>' ><span>Off</span></asp:LinkButton>
                        </asp:Panel>
                        <asp:Panel ID="Panel2" runat="server"  Visible='<%# Not Eval("Active") %>' class="field switch"  style="white-space: nowrap ;" >
                            <asp:LinkButton ID="LinkButton2" runat="server" CssClass="cb-enable"  CommandName="Enable"   CommandArgument='<%# Eval("DeveloperId") %>'  ><span>On</span></asp:LinkButton>
                            <asp:LinkButton ID="LinkButton3" runat="server"  CssClass="cb-disable selected"  CommandName="Disable" Enabled="false"  CommandArgument='<%# Eval("DeveloperId") %>' ><span>Off</span></asp:LinkButton>
                        </asp:Panel>
                   </td>
                </tr>
            </table>
        </ItemTemplate>
        <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
    </asp:DataList>

    <table border="0" cellpadding="0" cellspacing="4">
        <tr>
            <td>
                <b>Create a new key:</b>
            </td>
            <td>
                <asp:TextBox ID="tbNewName" runat="server" CssClass="ProductName"></asp:TextBox>
            </td>
            <td>
                <asp:Button ID="btnAddNewApiKey" runat="server" Text="Create" class="aButton btn" />
            </td>
        </tr>
    </table>


    <asp:LinqDataSource ID="dsAPIKeys" runat="server" EntityTypeName="" 
        ContextTypeName="FCX.FCXDataContext" EnableDelete="True" EnableInsert="True" 
        EnableUpdate="True" OrderBy="ProductName" TableName="FCX_API_Keys" 
        Where="PortalId == @PortalId">
        <WhereParameters>
            <asp:ControlParameter ControlID="hfPortalId" DefaultValue="-1" Name="PortalId" 
                PropertyName="Value" Type="Int32" />
        </WhereParameters>
    </asp:LinqDataSource>

</fieldset>

<br />
<fieldset>
    <legend><span class="AgapeH3">Transaction Broker</span></legend>
   <p>
        Transaction Broker is a program that runs within FCX and pulls down transactions (which have been submitted to this API)
        and processes them into Dynamics/Donorwise. This program must use the following API-KEY.
        You can also (optionally) restrict access to a specficed ip-address. 
</p>
   <p>
        <i>(*Note: If you reissue an API key, Transaction Broker will not be able to access the API until you configure it to use the new key.)</i>
   </p>
    <table border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <b><dnn:label id="ttlAPIKey" runat="server"  Text="API-KEY:" HelpText="The API-KEY gives Transaction Broker access to your accounts via the FCX-API."   /></b>
            </td>
            <td>
                <asp:Label ID="lblTBKEY" runat="server" ></asp:Label> <br />
            </td>
            <td>
                <asp:LinkButton ID="btnTBRekey" runat="server">Reissue</asp:LinkButton>
            </td>
        </tr>
         <tr>
            <td>
                <b><dnn:label id="Label1" runat="server"  Text="IP-Address(Optional):" HelpText="You can restrict access to the transaction broker to a single ip-address. (Enter the IP-Address of the server where Transaction Broker is installed, and ensure that it has a static ip-address."   /></b>
            </td>
            <td>
                <asp:TextBox ID="tbIP" runat="server" ></asp:TextBox> <br />
            </td>
            <td>
                <asp:LinkButton ID="btnSaveIp" runat="server">Save</asp:LinkButton>
            </td>
        </tr>
    </table>
</fieldset>