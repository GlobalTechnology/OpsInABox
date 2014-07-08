<%@ Control Language="VB" AutoEventWireup="false" CodeFile="BudgetManager.ascx.vb" Inherits="DotNetNuke.Modules.Budget.BudgetManager" %>
<script src="/js/jquery.numeric.js"></script>
<script src="/js/jquery.mousewheel.js" type="text/javascript"></script>
<script src="/js/mwheelIntent.js" type="text/javascript"></script>
<script src="/js/jquery.jscrollpane.min.js" type="text/javascript"></script>
<link href="/js/jquery.jscrollpane.css" rel="stylesheet" type="text/css" />
<script type="text/javascript">

    (function ($, Sys) {

        function setUpMyTabs() {
  $("#divImport").dialog({
                autoOpen: false,
                height: 300,
                width: 400,
                modal: true,
                title: "Import",
                close: function () {
                    //  allFields.val("").removeClass("ui-state-error");
                }
             });
            $("#divImport").parent().appendTo($("form:first"));
            $('.aButton').button();

            //$('.dropdown-toggle').dropdown();

            $('.numeric').numeric();
            $('.scroll-pane').jScrollPane();
            $('.insertPeriod').keyup(function () {

                var total = parseFloat($('#<%= tbP1new.ClientID%>').val())
                    + parseFloat($('#<%= tbP2new.ClientID%>').val())
                    + parseFloat($('#<%= tbP3new.ClientID%>').val())
                    + parseFloat($('#<%= tbP4new.ClientID%>').val())
                    + parseFloat($('#<%= tbP5new.ClientID%>').val())
                    + parseFloat($('#<%= tbP6new.ClientID%>').val())
                    + parseFloat($('#<%= tbP7new.ClientID%>').val())
                    + parseFloat($('#<%= tbP8new.ClientID%>').val())
                    + parseFloat($('#<%= tbP9new.ClientID%>').val())
                    + parseFloat($('#<%= tbP10new.ClientID%>').val())
                    + parseFloat($('#<%= tbP11new.ClientID%>').val())
                    + parseFloat($('#<%= tbP12new.ClientID%>').val());

                $('#<%= lblTotalNew.ClientID %>').text(total.toString());


            });
           
          

        }

        $(document).ready(function () {
            setUpMyTabs();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                setUpMyTabs();
            });
        });
    }(jQuery, window.Sys));



</script>
<style type="text/css">
    .scroll-pane {
        overflow: -moz-scrollbars-vertical;
        overflow-y: auto;
        overflow-x: hidden;
        max-height: 300px;
    }

    .insertPeriod {
        width:55px;
    }
    .headSmall {
        font-size: x-small;
        color:  lightgray;
    }
    .headBig {
        text-transform: capitalize;
        font-size: 9pt;
        color: white;  
        Width: 46px;
      
    }
    .income {
        color: blue;
    }

    .expense {
        color: red;
    }
</style>

<table width="100%">
    <tr>
        <td rowspan="2" style="font-size: large; width: 20%">
            <h3>Fiscal Year:</h3>

        </td>
        <td rowspan="2" style="width: 45%">
            <asp:DropDownList ID="ddlFiscalYear" runat="server" Font-Size="Large" AutoPostBack="true">
                
            </asp:DropDownList>
        </td>
        <td style="width: 15%; text-align: right;">
            <b>Filter by R/C:</b>

        </td>
        <td style="width: 20%">
            <asp:DropDownList ID="ddlRC" runat="server" DataTextField="Name" DataValueField="CostCentreCode" AutoPostBack="true" AppendDataBoundItems="true">
                <asp:ListItem Text="All R/C's" Value="All" />
                <asp:ListItem Text="All Staff" Value="AllStaff" />
                <asp:ListItem Text="All Non-Staff" Value="AllNonStaff" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td style="text-align: right;"><b>Filter by A/C:</b>
        </td>
        <td>
            <asp:DropDownList ID="ddlAC" runat="server" DataTextField="Name" DataValueField="AccountCode" AutoPostBack="true" AppendDataBoundItems="true">
                <asp:ListItem Text="All A/C's" Value="All" />
                <asp:ListItem Text="All Income & Expense Only" Value="IE" />
                <asp:ListItem Text="Income Only" Value="3" />
                <asp:ListItem Text="Expense Only" Value="4" />

            </asp:DropDownList>

        </td>
    </tr>
</table>




<asp:HiddenField ID="hfPortalId" runat="server" Value="0" />

<table class="budGrid" cellpadding="3" cellspacing="0" style="width: 100%; background-color: #FAFAF0;">
    <tr style="background-color: #6B696B; font-weight: bold; font-size: small; color: white; white-space: nowrap;">
        <td style="width: 47px">A/C</td>
        <td style="width: 47px">R/C</td>
        <td class="headSmall"><asp:Label ID="lblP1" runat="server" CssClass="headBig" Text="P1"></asp:Label></td>
        <td class="headSmall"><asp:Label ID="lblP2" runat="server" CssClass="headBig" Text="P2"></asp:Label></td>
        <td class="headSmall"><asp:Label ID="lblP3" runat="server" CssClass="headBig" Text="P3"></asp:Label></td>
        <td class="headSmall"><asp:Label ID="lblP4" runat="server" CssClass="headBig" Text="P4"></asp:Label></td>
        <td class="headSmall"><asp:Label ID="lblP5" runat="server"  CssClass="headBig" Text="P5"></asp:Label></td>
        <td class="headSmall"><asp:Label ID="lblP6" runat="server" CssClass="headBig" Text="P6"></asp:Label></td>
        <td class="headSmall"><asp:Label ID="lblP7" runat="server" CssClass="headBig" Text="P7"></asp:Label></td>
        <td class="headSmall"><asp:Label ID="lblP8" runat="server"  CssClass="headBig" Text="P8"></asp:Label></td>
        <td class="headSmall"><asp:Label ID="lblP9" runat="server" CssClass="headBig" Text="P9"></asp:Label></td>
        <td class="headSmall"><asp:Label ID="lblP10" runat="server" CssClass="headBig" Text="P10"></asp:Label></td>
        <td class="headSmall"><asp:Label ID="lblP11" runat="server" CssClass="headBig" Text="P11"></asp:Label></td>
        <td class="headSmall"><asp:Label ID="lblP12" runat="server" CssClass="headBig" Text="P12"></asp:Label></td>
        
        <td style="width: 55px">Total</td>
        <td></td>

    </tr>

    <tr> 
        <td colspan="16">
            <div class="scroll-pane" style="margin: 0 -2px;">
                <asp:GridView ID="GridView1" runat="server" BackColor="White" ShowHeader="false" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="3" ForeColor="Black" GridLines="Vertical" AutoGenerateColumns="False" DataKeyNames="BudgetSummaryId" DataSourceID="dsBudgetSummaries" ShowFooter="False" Width="100%">
                    <AlternatingRowStyle BackColor="White" Font-Size="X-Small" />
                    <Columns>
                        <asp:TemplateField HeaderText="Account" SortExpression="Account" ItemStyle-Width="50" ItemStyle-Font-Size="9pt" ItemStyle-Font-Bold="true">
                            <EditItemTemplate>
                                <asp:Label ID="Label2" runat="server" Width="48px" Text='<%# Eval("Account") %>' ></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label14" runat="server" ToolTip='<%# Eval("AP_StaffBroker_AccountCode.AccountCodeName") %>' Text='<%# Bind("Account")  %>'  CssClass='<%# IIf({AccountType.Income, AccountType.AccountsPayable}.Contains(Eval("AP_StaffBroker_AccountCode.AccountCodeType")), "income", "expense")%>'></asp:Label>
                            </ItemTemplate>

                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="RC" SortExpression="RC" ItemStyle-Width="50" ItemStyle-Font-Size="9pt" ItemStyle-Font-Bold="true">
                            <EditItemTemplate>
                                <asp:Label ID="Label1" runat="server" Width="48px" Text='<%# Eval("RC")%>'></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" ToolTip='<%# Eval("AP_StaffBroker_CostCenter.CostCentreName") %>' Text='<%# Eval("RC")%>'></asp:Label>
                            </ItemTemplate>


                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="P1" SortExpression="P1" HeaderStyle-Width="60">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("P1", "{0:0.00}")%>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# FormatCurrency(Eval("P1"))%>'  CssClass='<%# IIf({AccountType.Income, AccountType.AccountsPayable}.Contains(Eval("AP_StaffBroker_AccountCode.AccountCodeType")), "income", "expense")%>'></asp:Label>
                            </ItemTemplate>

                            <ControlStyle Width="60px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="P2" SortExpression="P2">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("P2", "{0:0.00}") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# FormatCurrency(Eval("P2"))%>'  CssClass='<%# IIf({AccountType.Income, AccountType.AccountsPayable}.Contains(Eval("AP_StaffBroker_AccountCode.AccountCodeType")), "income", "expense")%>'></asp:Label>
                            </ItemTemplate>
                           
                            <ControlStyle Width="60px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="P3" SortExpression="P3">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("P3", "{0:0.00}") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label4" runat="server" Text='<%# FormatCurrency(Eval("P3"))%>'  CssClass='<%# IIf({AccountType.Income, AccountType.AccountsPayable}.Contains(Eval("AP_StaffBroker_AccountCode.AccountCodeType")), "income", "expense")%>'></asp:Label>
                            </ItemTemplate>
                            
                            <ControlStyle Width="60px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="P4" SortExpression="P4">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("P4", "{0:0.00}") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label5" runat="server" Text='<%# FormatCurrency(Eval("P4"))%>'  CssClass='<%# IIf({AccountType.Income, AccountType.AccountsPayable}.Contains(Eval("AP_StaffBroker_AccountCode.AccountCodeType")), "income", "expense")%>'></asp:Label>
                            </ItemTemplate>
                           
                            <ControlStyle Width="60px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="P5" SortExpression="P5">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("P5", "{0:0.00}") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label6" runat="server" Text='<%# FormatCurrency(Eval("P5"))%>'  CssClass='<%# IIf({AccountType.Income, AccountType.AccountsPayable}.Contains(Eval("AP_StaffBroker_AccountCode.AccountCodeType")), "income", "expense")%>'></asp:Label>
                            </ItemTemplate>
                          
                            <ControlStyle Width="60px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="P6" SortExpression="P6">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox6" runat="server" Text='<%# Bind("P6", "{0:0.00}") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label7" runat="server" Text='<%# FormatCurrency(Eval("P6"))%>'  CssClass='<%# IIf({AccountType.Income, AccountType.AccountsPayable}.Contains(Eval("AP_StaffBroker_AccountCode.AccountCodeType")), "income", "expense")%>'></asp:Label>
                            </ItemTemplate>
                           
                            <ControlStyle Width="60px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="P7" SortExpression="P7">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox7" runat="server" Text='<%# Bind("P7", "{0:0.00}") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label8" runat="server" Text='<%# FormatCurrency(Eval("P7"))%>'  CssClass='<%# IIf({AccountType.Income, AccountType.AccountsPayable}.Contains(Eval("AP_StaffBroker_AccountCode.AccountCodeType")), "income", "expense")%>'></asp:Label>
                            </ItemTemplate>
                          
                            <ControlStyle Width="60px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="P8" SortExpression="P8">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox8" runat="server" Text='<%# Bind("P8", "{0:0.00}") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label9" runat="server" Text='<%# FormatCurrency(Eval("P8"))%>'  CssClass='<%# IIf({AccountType.Income, AccountType.AccountsPayable}.Contains(Eval("AP_StaffBroker_AccountCode.AccountCodeType")), "income", "expense")%>'></asp:Label>
                            </ItemTemplate>
                       
                            <ControlStyle Width="60px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="P9" SortExpression="P9">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox9" runat="server" Text='<%# Bind("P9", "{0:0.00}") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label10" runat="server" Text='<%# FormatCurrency(Eval("P9"))%>'  CssClass='<%# IIf({AccountType.Income, AccountType.AccountsPayable}.Contains(Eval("AP_StaffBroker_AccountCode.AccountCodeType")), "income", "expense")%>'></asp:Label>
                            </ItemTemplate>
                         
                            <ControlStyle Width="60px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="P10" SortExpression="P10">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox10" runat="server" Text='<%# Bind("P10", "{0:0.00}") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label11" runat="server" Text='<%# FormatCurrency(Eval("P10"))%>'  CssClass='<%# IIf({AccountType.Income, AccountType.AccountsPayable}.Contains(Eval("AP_StaffBroker_AccountCode.AccountCodeType")), "income", "expense")%>'></asp:Label>
                            </ItemTemplate>
                           
                            <ControlStyle Width="60px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="P11" SortExpression="P11">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox11" runat="server" Text='<%# Bind("P11", "{0:0.00}") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label12" runat="server" Text='<%# FormatCurrency(Eval("P11"))%>'  CssClass='<%# IIf({AccountType.Income, AccountType.AccountsPayable}.Contains(Eval("AP_StaffBroker_AccountCode.AccountCodeType")), "income", "expense")%>'></asp:Label>
                            </ItemTemplate>
                            
                            <ControlStyle Width="60px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="P12" SortExpression="P12">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox12" runat="server" Text='<%# Bind("P12", "{0:0.00}") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label13" runat="server" Text='<%# FormatCurrency(Eval("P12"))%>'  CssClass='<%# IIf({AccountType.Income, AccountType.AccountsPayable}.Contains(Eval("AP_StaffBroker_AccountCode.AccountCodeType")), "income", "expense")%>'></asp:Label>
                            </ItemTemplate>
                          
                            <ControlStyle Width="60px" />
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Total">
                            <ItemTemplate>

                                <asp:Label runat="server" Font-Bold="true" Width="60px" Text='<%# FormatCurrency(CDbl(Eval("P1") + Eval("P2") +Eval("P3") +Eval("P4") +Eval("P5") +Eval("P6") +Eval("P7") +Eval("P8") +Eval("P9") +Eval("P10") +Eval("P11") +Eval("P12"))) %>'></asp:Label>
                            </ItemTemplate>
                         <ControlStyle Width="60px" />
                        </asp:TemplateField>


                        <asp:TemplateField ShowHeader="False" ItemStyle-Width="54px">
                            <EditItemTemplate>
                               <div style="white-space:nowrap;">
                                <asp:ImageButton ID="ImageButton2" runat="server"  ImageUrl ="~/icons/sigma/Save_16X16_Standard.png" CausesValidation="True"  CommandName="Update" Height="16px" ToolTip="Update" />
                                <asp:ImageButton ID="ImageButton1" runat="server"  ImageUrl ="~/images/cancel.gif"  CausesValidation="False"  CommandName="Cancel" Height="16px" ToolTip="Cancel" />
                            </div>
                                   </EditItemTemplate>
                            <ItemTemplate >
                              
                                <asp:ImageButton ID="ImageButton2" runat="server"  ImageUrl ="~/images/edit.gif" CausesValidation="False"  CommandName="Edit" Height="12px" />
                                <asp:Image ID="Image1" runat="server" ImageUrl ="~/images/error-icn.png" Visible='<%# Eval("Error") = True%>' ToolTip='<%# Eval("ErrorMessage")  %>' Height="12px" />
                                <asp:Image ID="Image2" runat="server" ImageUrl ="~/images/icon_scheduler_16px.gif" Visible='<%# Eval("Changed") = True and Not Eval("Error") = True %>' ToolTip='This budget entry will be downloaded into dynamics in the next 10 minutes.' Height="12px" />
                                <asp:Image ID="Image3" runat="server" ImageUrl ="~/images/help-icn.png" Visible='<%# (Not String.IsNullOrEmpty(Eval("ErrorMessage")) and Eval("Error") = False ) %>' ToolTip='<%# Eval("ErrorMessage")  %>' Height="12px" />
                               <asp:ImageButton ID="ImageButton3" runat="server"  ImageUrl ="~/images/delete.gif" CausesValidation="False"  CommandName="myDelete" CommandArgument='<%# Eval("BudgetSummaryId")%>'  ToolTip="Clear/Remove the current row" Height="12px" />
                                
                            </ItemTemplate>
                            <ItemStyle Wrap="false" />
                        </asp:TemplateField>



                    </Columns>
                    <FooterStyle BackColor="#CCCC99" Font-Bold="True" />
                    <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" Height="20px" Wrap="false" Font-Size="9pt" />
                    <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                    <RowStyle BackColor="#F7F7DE" Font-Size="xSmall" />
                    <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                    <SortedAscendingCellStyle BackColor="#FBFBF2" />
                    <SortedAscendingHeaderStyle BackColor="#848384" />
                    <SortedDescendingCellStyle BackColor="#EAEAD3" />
                    <SortedDescendingHeaderStyle BackColor="#575357" />

                </asp:GridView>

                <asp:LinqDataSource ID="dsBudgetSummaries" runat="server" EntityTypeName="" ContextTypeName="Budget.BudgetDataContext" TableName="AP_Budget_Summaries" 
                    Where="Portalid == @Portalid &amp;&amp; FiscalYear == @FiscalYear &amp;&amp; AP_StaffBroker_AccountCode.AccountCodeType>2 &amp;&amp; ( (RC == @RC) || (@RC==&quot;All&quot;) || (@RC==&quot;AllStaff&quot; &amp;&amp; AP_StaffBroker_CostCenter.Type==1) || (@RC==&quot;AllNonStaff&quot; &amp;&amp; AP_StaffBroker_CostCenter.Type!=1)) &amp;&amp; (@AC==&quot;All&quot; || @AC == Account || (@AC==&quot;3&quot; &amp;&amp; AP_StaffBroker_AccountCode.AccountCodeType==3) || (@AC==&quot;4&quot; &amp;&amp; AP_StaffBroker_AccountCode.AccountCodeType==4) || (@AC==&quot;IE&quot; &amp;&amp; (AP_StaffBroker_AccountCode.AccountCodeType==3 || AP_StaffBroker_AccountCode.AccountCodeType==4)))  &amp;&amp; (P1!=0 || P2!=0 || P3!=0 || P4!=0 || P5!=0 || P6!=0 || P7!=0 || P8!=0 || P9!=0 || P10!=0 || P11!=0 || P12!=0) " EnableInsert="True" EnableUpdate="True" OrderBy="Account">
                    <WhereParameters>
                        <asp:ControlParameter ControlID="hfPortalId" Name="Portalid" PropertyName="Value" Type="Int32" />
                        <asp:ControlParameter ControlID="ddlRC" Name="RC" PropertyName="SelectedValue" Type="String" />
                        <asp:ControlParameter ControlID="ddlAC" Name="AC" PropertyName="SelectedValue" Type="String" />
                        <asp:ControlParameter ControlID="ddlFiscalYear" Name="FiscalYear" PropertyName="SelectedValue" Type="Int32" />
                    </WhereParameters>
                </asp:LinqDataSource>
            </div>
        </td>
    </tr>
    

    <tr style="background-color: #CCCC99; font-weight: bold; font-size: 8pt;">
        <td>Total</td>
        <td>PTD:<br />
            <asp:Label ID="Label15" runat="server" Font-Italic="true"  Font-Bold="false" Text="YTD:"></asp:Label></td>
        <td>
            <asp:Label ID="lblPTD1" runat="server"  ></asp:Label><br />
            <asp:Label ID="lblYTD1" runat="server" Font-Italic="true"  Font-Bold="false" ></asp:Label>
        </td>
        <td>
            <asp:Label ID="lblPTD2" runat="server" ></asp:Label><br />
            <asp:Label ID="lblYTD2" runat="server" Font-Italic="true"  Font-Bold="false" ></asp:Label>
        </td>
        <td>
            <asp:Label ID="lblPTD3" runat="server" ></asp:Label><br />
            <asp:Label ID="lblYTD3" runat="server" Font-Italic="true"  Font-Bold="false"></asp:Label>
        </td>
        <td>
            <asp:Label ID="lblPTD4" runat="server" ></asp:Label><br />
            <asp:Label ID="lblYTD4" runat="server" Font-Italic="true"  Font-Bold="false" ></asp:Label>
        </td>
        <td>
            <asp:Label ID="lblPTD5" runat="server"  ></asp:Label><br />
            <asp:Label ID="lblYTD5" runat="server" Font-Italic="true"  Font-Bold="false" ></asp:Label>
        </td>
        <td>
            <asp:Label ID="lblPTD6" runat="server" ></asp:Label><br />
            <asp:Label ID="lblYTD6" runat="server" Font-Italic="true"  Font-Bold="false"></asp:Label>
        </td>
        <td>
            <asp:Label ID="lblPTD7" runat="server"  ></asp:Label><br />
            <asp:Label ID="lblYTD7" runat="server" Font-Italic="true"  Font-Bold="false" ></asp:Label>
        </td>
        <td>
            <asp:Label ID="lblPTD8" runat="server"  ></asp:Label><br />
            <asp:Label ID="lblYTD8" runat="server" Font-Italic="true"  Font-Bold="false" ></asp:Label>
        </td>
        <td>
            <asp:Label ID="lblPTD9" runat="server" ></asp:Label><br />
            <asp:Label ID="lblYTD9" runat="server" Font-Italic="true"  Font-Bold="false" ></asp:Label>
        </td>
        <td>
            <asp:Label ID="lblPTD10" runat="server"  ></asp:Label><br />
            <asp:Label ID="lblYTD10" runat="server" Font-Italic="true"  Font-Bold="false" ></asp:Label>
        </td>
        <td>
            <asp:Label ID="lblPTD11" runat="server"  ></asp:Label><br />
            <asp:Label ID="lblYTD11" runat="server" Font-Italic="true"  Font-Bold="false" ></asp:Label>
        </td>
        <td>
            <asp:Label ID="lblPTD12" runat="server"  ></asp:Label><br />
            <asp:Label ID="lblYTD12" runat="server" Font-Italic="true"  Font-Bold="false"></asp:Label>
        </td>

     
        <td colspan="2">  <asp:Label ID="lblTotal" runat="server" ></asp:Label></td>
        

    </tr>

    <tr>
        <td colspan="16" style="text-align: center; font-style: italic; font-size: x-small; padding-top: 5px; border-bottom: dashed 1px #808080">--Insert New Row--
        </td>
    </tr>


    <tr>

        <td rowspan="2" style="padding: 0; margin:0;">
            <asp:DropDownList ID="ddlAccountNew" runat="server" DataTextField="Name" DataValueField="AccountCode" Font-Size="7pt" Width="50px"></asp:DropDownList>

        </td>
        <td rowspan="2" style="padding: 0; margin:0;">
            <asp:DropDownList ID="ddlRCNew" runat="server" DataTextField="Name" Width="50px" DataValueField="CostCentreCode" Font-Size="7pt"></asp:DropDownList>
        </td>
        <td>
            <asp:TextBox ID="tbP1new" runat="server" CssClass="insertPeriod numeric">0</asp:TextBox></td>
        <td>
            <asp:TextBox ID="tbP2new" runat="server" CssClass="insertPeriod numeric">0</asp:TextBox></td>
        <td>
            <asp:TextBox ID="tbP3new" runat="server" CssClass="insertPeriod numeric">0</asp:TextBox></td>
        <td>
            <asp:TextBox ID="tbP4new" runat="server" CssClass="insertPeriod numeric">0</asp:TextBox></td>
        <td>
            <asp:TextBox ID="tbP5new" runat="server" CssClass="insertPeriod numeric">0</asp:TextBox></td>
        <td>
            <asp:TextBox ID="tbP6new" runat="server" CssClass="insertPeriod numeric">0</asp:TextBox></td>
        <td>
            <asp:TextBox ID="tbP7new" runat="server" CssClass="insertPeriod numeric">0</asp:TextBox></td>
        <td>
            <asp:TextBox ID="tbP8new" runat="server" CssClass="insertPeriod numeric">0</asp:TextBox></td>
        <td>
            <asp:TextBox ID="tbP9new" runat="server" CssClass="insertPeriod numeric">0</asp:TextBox></td>
        <td>
            <asp:TextBox ID="tbP10new" runat="server" CssClass="insertPeriod numeric">0</asp:TextBox></td>
        <td>
            <asp:TextBox ID="tbP11new" runat="server" CssClass="insertPeriod numeric">0</asp:TextBox></td>
        <td>
            <asp:TextBox ID="tbP12new" runat="server" CssClass="insertPeriod numeric">0</asp:TextBox></td>
        <td>
            <asp:Label ID="lblTotalNew" runat="server" Width="60px" Text="0"></asp:Label></td>
        <td>
            <asp:LinkButton ID="btnInsertRow" runat="server" Width="58px" Font-Size="X-Small">Insert</asp:LinkButton>
        </td>
    </tr>
    <tr>
        <td colspan="12" align="right" style="font-size: x-small; color: grey;">
           <i>...or split this year total evenly accross 12 months:</i> 
        </td>
        <td>
         <asp:TextBox ID="tbTotalNew" runat="server" CssClass="insertPeriod numeric">0</asp:TextBox></td>
        <td>
            <asp:LinkButton ID="btnInsertAutoSplit" runat="server" Width="58px" Font-Size="X-Small">Insert</asp:LinkButton>
        </td>
    </tr>


      <tr>
        <td id="WarningRow" runat="server" colspan="16" style="background-color: #F9DB4F; text-align: center; font-weight: bold; font-size: small; padding-top: 5px; " visible="false">
            <p>There is already a budget entry for the row you are trying to insert. Would you like to <b>Replace</b> or <b>Add To</b> the current values?</p>
          
            <p>  
           
            <asp:Button ID="btnReplace" runat="server" Text="Replace" CssClass="aButton btn"  Font-Size="x-small"/> &nbsp;
            <asp:Button ID="btnAddTo" runat="server" Text="Add To" CssClass="aButton btn"  Font-Size="x-small"/> &nbsp;
            <asp:Button ID="btnCancelInsert" runat="server" Text="Cancel" CssClass="aButton btn"  Font-Size="x-small"/> </p>
        </td>
    </tr>






</table>

<asp:Label ID="lblError" runat="server" ForeColor="Red" Text=""></asp:Label>
<p>
<asp:Button ID="btnExport" runat="server" Text="Export"  CssClass="aButton btn" Font-Size="X-Small"  />&nbsp;&nbsp;
<input type="button" class="aButton btn" onclick="$('#divImport').dialog('open'); " style="font-size: x-small"  value="Import"/>&nbsp;&nbsp;

<asp:HyperLink ID="hlImportTemplate" runat="server" NavigateUrl="~/DesktopModules/AgapeConnect/BudgetManager/Budget-Import.xls">  


    <img style="margin-bottom: -3px;" src="/Icons/Sigma/ExtXls_16X16_Standard.png" />
    Import Template</asp:HyperLink>
</p>
<div id="divImport" class="ui-widget">
  <p> <strong>Select File to Import:</strong> </p>
    <p><asp:FileUpload ID="fuImport" runat="server" /></p>

    <p>
        Would you like to <b><i>overwrite</i></b> existing values, or <b><i>add</i></b> the imported values to the existing entries?
    </p>
     <p><span style="font-style: italic; color: grey;">
               * Please ensure you have selected the correct year. The budget entries you are importing will be added to the <strong> <%= ddlFiscalYear.SelectedItem.Text%> Fiscal Year</strong>. 
               To select a different year, click Cancel and select a different year (top left).
           </span></p>
    <div style="width: 100%; text-align: center;">
        <asp:Button ID="btnImpOverwrite" runat="server" Text="Overwrite"  CssClass="aButton btn"  />&nbsp; &nbsp;
        <asp:Button ID="btnImpAddTo" runat="server" Text="Add To"  CssClass="aButton btn"  />&nbsp; &nbsp;
        <input type="button" class="aButton btn" onclick="$('#divImport').dialog('close'); "  value="Cancel"/>

    </div>
</div>


