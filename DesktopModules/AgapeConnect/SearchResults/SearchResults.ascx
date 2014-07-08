<%@ Control Language="C#" AutoEventWireup="false" Inherits="DotNetNuke.Modules.SearchResults.MySearchResults" CodeFile="SearchResults.ascx.cs" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>

<style type="text/css">
    .seachImage {
        height: 80px;
        border: 1pt solid black;
        margin-right: 12px;
    }
    .dnnGridItem:hover, .dnnGridAltItem:hover  {
        border: 2px solid Blue;
        background-color: #BAD1ED;
    }
    .searchTitle h4 {
        margin-bottom: 2px;

    }
</style>

<div class="dnnForm dnnSearchResults dnnClear">
    <asp:Label ID="lblMessage" runat="server" />
    <asp:DataGrid ID="dgResults" runat="server" AutoGenerateColumns="False" AllowPaging="true" BorderStyle="None" ShowHeader="False" GridLines="None" PagerStyle-Visible="false">
        <ItemStyle CssClass="dnnGridItem" HorizontalAlign="Left" VerticalAlign="Top" />
        <AlternatingItemStyle CssClass="dnnGridAltItem" />
        <FooterStyle CssClass="dnnGridFooter" />
        <Columns>

            <asp:TemplateColumn>
                <ItemTemplate>

                     <asp:HyperLink ID="lnkLink" runat="server" CssClass="CommandButton" NavigateUrl='<%# FormatURL((int)DataBinder.Eval(Container.DataItem,"TabId"),(string)DataBinder.Eval(Container.DataItem,"Guid")) %>'  >
				   
                    <table>
                        <tr valign="top">
                            <td>
                                <asp:Image ID="imgImage" runat="server" ImageUrl='<%# (string)DataBinder.Eval(Container.DataItem, "Image")  %>' CssClass="seachImage"  />


                            </td>

                            <td width="100%">
                                <asp:Label ID="HyperLink1" runat="server" CssClass="searchTitle"  Text='<%# "<h4>" + DataBinder.Eval(Container.DataItem, "Title") + "</h4>" %>' />
                                
                                <asp:Panel ID="Panel1" runat="server" CssClass="Agape_Story_subtitle">
                                    <asp:Label ID="Label1" runat="server" CssClass="Normal" Text='<%# DataBinder.Eval(Container.DataItem, "AuthorName")  %>' Visible='<%#  ((string)DataBinder.Eval(Container.DataItem, "SearchKey")).StartsWith("S")  %>' />

                                    <asp:Label ID="Label2" runat="server" CssClass="Normal" Text='<%# FormatDate((DateTime)DataBinder.Eval(Container.DataItem, "PubDate")) %>' />


                                    <br />
                                </asp:Panel>

                                <asp:Label ID="Label3" runat="server" CssClass="Normal" Text='<%# DataBinder.Eval(Container.DataItem, "Description") + "<br>" %>' Visible="<%# !String.IsNullOrEmpty(ShowDescription()) %>" />

                            </td>
                        </tr>
                    </table>



                         </asp:HyperLink>




                    
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>
    <dnn:PagingControl ID="ctlPagingControl" runat="server" Mode="PostBack" />
</div>
