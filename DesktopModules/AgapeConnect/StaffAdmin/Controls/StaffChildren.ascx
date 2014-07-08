<%@ Control Language="VB" AutoEventWireup="false"   CodeFile="StaffChildren.ascx.vb" Inherits="DesktopModules_AgapePortal_StaffBroker_StaffChildren" %>
  
  <%@ Register src="~/controls/labelcontrol.ascx" tagname="labelcontrol" tagprefix="uc1" %>


<asp:HiddenField ID="hfChildStaffId" runat="server" />
   <asp:GridView ID="GridView1" ShowHeaderWhenEmpty="True"  runat="server"  CssClass="dnnGrid"
    AutoGenerateColumns="False" DataKeyNames="ChildId" DataSourceID="dsChildren"
                       CellPadding="4" ForeColor="#333333" ShowFooter="True" 
                    GridLines="None" >
                     <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:TemplateField HeaderText="First Name" SortExpression="FirstName">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox2" runat="server" Font-Size="12pt"  Width="120px" Text='<%# Bind("FirstName") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Font-Bold="true" Font-Size="12pt" Text='<%# Bind("FirstName") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                 <asp:TextBox ID="tbFirstName" runat="server" Font-Size="10pt"  Width="120px" ></asp:TextBox>
                            </FooterTemplate>
                            <HeaderStyle Wrap="False"  />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Birthday" SortExpression="Birthday">
                            <EditItemTemplate>
                                <asp:TextBox ID="tbBday" name="tbBday" runat="server" CssClass="datepicker"  Font-Size="10pt" Text='<%# Bind("Birthday", "{0:dd/MM/yyyy}") %>'></asp:TextBox>
                                
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="ftDOB" runat="server" CssClass="datepicker" Font-Size="10pt" Width="70px" ></asp:TextBox>
                              
                            </FooterTemplate>
                            <ItemTemplate>
                                <asp:Label  runat="server" ID="lbDOB"      Font-Size="10pt" Text='<%# Bind("Birthday", "{0:dd/MM/yyyy}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False">
            <EditItemTemplate>
                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" 
                    CommandName="Update" Text="Update"></asp:LinkButton>
                &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" 
                    CommandName="Cancel" Text="Cancel"></asp:LinkButton>
            </EditItemTemplate>
            <ItemTemplate>
                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" 
                    CommandName="Edit" Text="Edit"></asp:LinkButton>
                &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" 
                    CommandName="Delete" Text="Delete"  ></asp:LinkButton>
            </ItemTemplate>
            <FooterTemplate>
                  <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" 
                    CommandName="myInsert" Text="Insert" ForeColor="White"></asp:LinkButton>
            </FooterTemplate>
        </asp:TemplateField>
                       
                    </Columns>
                     <EmptyDataTemplate>
        <table>
        <tr>
            <td>
                <asp:TextBox ID="tbFirstNameE" runat="server"  Font-Size="10pt"  Width="120px" ></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="tbDOBE" runat="server" Font-Size="10pt"  Width="70px" class="datepicker" ></asp:TextBox>
                                
            </td>
            <td>
                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" 
                    CommandName="myInsertE" Text="Insert"  ForeColor="White" ></asp:LinkButton>
            </td>
        </tr>
       </table>
    </EmptyDataTemplate>
                    <FooterStyle CssClass="ui-widget-header " />
                   <HeaderStyle CssClass="ui-widget-header "    />
                   
                    <EmptyDataRowStyle CssClass="ui-widget-header" />
                    <PagerStyle CssClass="dnnGridPager" />
                    <RowStyle CssClass="dnnGridItem" />
                    <SelectedRowStyle CssClass="dnnFormError" />
                    <SortedAscendingCellStyle BackColor="#FDF5AC" />
                    <SortedAscendingHeaderStyle BackColor="#4D0000" />
                    <SortedDescendingCellStyle BackColor="#FCF6C0" />
                    <SortedDescendingHeaderStyle BackColor="#820000" />
                </asp:GridView>
         
                <asp:LinqDataSource ID="dsChildren" runat="server" ContextTypeName="StaffBroker.StaffBrokerDataContext"
                    EnableDelete="True" EnableInsert="True" EnableUpdate="True" EntityTypeName=""
                    OrderBy="Birthday" TableName="AP_StaffBroker_Childrens" Where="StaffId == @StaffId">
                    <WhereParameters>
                        <asp:ControlParameter ControlID="ddlStaff" Name="StaffId" PropertyName="SelectedValue"
                            Type="Int32" />
                    </WhereParameters>

                </asp:LinqDataSource>
              