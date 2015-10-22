<%@ Control Language="VB" AutoEventWireup="false" CodeFile="DocumentViewer.ascx.vb" Inherits="DotNetNuke.Modules.AgapeConnect.Documents.DocumentViewer" %>
<%@ Register Assembly="PdfViewer" Namespace="PdfViewer" TagPrefix="cc1" %>
 <script src="/js/jquery.watermarkinput.js" type="text/javascript"></script>
 <script src="/js/jquery.contextmenu.r2.js" type="text/javascript"></script>
<script type="text/javascript">
 function setUpMyTabs() {


     $('.Download').button({ icons: { primary: "ui-icon-extlink"} });
        $('.aButton').button();
        $('#<%= tbMessage.ClientID %>').Watermark('Enter comment here...');


        $('.myBubble').contextMenu('CommentMenu', {
            bindings: {
                'DeleteComment': function (t) {
                   // alert('Trigger was ' + t.id + '\nAction was DeleteComment');
                    $('#<%= hfCommentId.ClientID %>').val(t.id);
                    __doPostBack('<%= commentUpdatePanel.ClientID %>', ''); 
                    
                
                }
                
            }
        });

        

    }

        $(document).ready(function () {
            setUpMyTabs();


            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () { setUpMyTabs(); });
        });
        </script>

        <style type="text/css">
        .Download, a.Download, a.Donwload:visited, a.Download.active
        {
            color: White;   
            font-weight: bold;
            width: 90%;
        }
        .bubble {
	position:relative;
	padding:7px;
	margin:6px 2em 0 0;
	color:#000;
	font-size: small;
	background:#DDD; /* default background for browsers without gradient support */
	/* css3 */
	background:-webkit-gradient(linear, 0 0, 0 100%, from(#EEE), to(#CCC));
	background:-moz-linear-gradient(#EEE, #CCC);
	background:-o-linear-gradient(#EEE, #CCC);
	background:linear-gradient(#EEE, #CCC);
	-webkit-border-radius:10px;
	-moz-border-radius:10px;
	border-radius:10px;
	text-align: left;
	float: left;
	min-width: 150px;
}
.bubble:after {
	content:"";
	position:absolute;
	bottom:-10px; /* value = - border-top-width - border-bottom-width */
	left:20px; /* controls horizontal position */
	border-width:10px 10px 0; /* vary these values to change the angle of the vertex */
	border-style:solid;
	border-color:#CCC transparent;
    /* reduce the damage in FF3.0 */
    display:block; 
    width:0;
}
.myBubble {
	position:relative;
	padding:7px;
	margin:8px 0 0 2em;
	color:#000;
	font-size: small;
	background:#DDF; /* default background for browsers without gradient support */
	/* css3 */
	background:-webkit-gradient(linear, 0 0, 0 100%, from(#EEF), to(#CCF));
	background:-moz-linear-gradient(#EEF, #CCF);
	background:-o-linear-gradient(#EEF, #CCF);
	background:linear-gradient(#EEF, #CCF);
	-webkit-border-radius:10px;
	-moz-border-radius:10px;
	border-radius:10px;
	text-align: left;
	float: right;
    min-width: 120px;
}
.myBubble:after {
	content:"";
	position:absolute;
	bottom:-10px; /* value = - border-top-width - border-bottom-width */
	left:20px; /* controls horizontal position */
	border-width:10px 10px 0; /* vary these values to change the angle of the vertex */
	border-style:solid;
	border-color:#CCF transparent;
    /* reduce the damage in FF3.0 */
    display:block; 
    width:0;
}
.subBubble
{
    color: #AAA;
    font-size: xx-small;
    font-style: italic ;
    text-align: left;
    margin-left: 5em;
    float: left;
}
.subMyBubble
{
    color: #AAA;
    font-size: xx-small;
    font-style: italic ;
    text-align: right;
    margin-right: 1em;
    float: right;
}
        
        </style>


<asp:HiddenField ID="hfCommentId" runat="server" />
<asp:HiddenField ID="hfFileId" runat="server" />
<div style="text-align: center; width: 100%;" >
 <asp:Label ID="lblError" runat="server" class="ui-state-error ui-corner-all" 
                Style="padding: 3px; margin-top: 3px; display: inline-block; width: 50%;" Visible ="false" ></asp:Label>
                </div>

<asp:Panel ID="theMainPanel" runat="server">


<table>
    <tr valign="top">
        <td width="1100px" align="center">
        <div style="text-align: left; margin-bottom: 5px;">
        <asp:Label ID="lblFileName" runat="server" CssClass="AgapeH2" style="font-size: 27pt"></asp:Label><br />
<asp:Label ID="lblFileUrl" runat="server" style="font-size: x-small; font-style: italic; color: #AAA;"></asp:Label>
</div>
            <iframe id="Viewer" runat="server" width="750px" height="1100px"></iframe>
        </td>
        <td width="250px" align="center">  
            <asp:LinkButton ID="btnDownload" runat="server" class="Download">Download File</asp:LinkButton>
          <br /><br />
            
             <asp:GridView ID="gvFileVersions" runat="server" AutoGenerateColumns="False" 
                                            CellPadding="4" ForeColor="#333333" GridLines="None">
                                            
                                            <AlternatingRowStyle BackColor="White" />
                                            
                                            <Columns>
                                                <asp:BoundField DataField="VersionNumber" HeaderText="Version" />
                                                <asp:TemplateField HeaderText="DateAdded">
                                                  
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label1" runat="server" Text='<%# GetFileDate(Eval("FileId")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# GetFileURL(Eval("DocId"), Eval("FileId")) %>' Text="View" Visible='<%# Eval("FileId") <> hfFileId.Value %>'></asp:HyperLink>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            
                                            <EditRowStyle BackColor="#2461BF" />
                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                            <RowStyle BackColor="#EFF3FB" />
                                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                            <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                            <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                            <SortedDescendingHeaderStyle BackColor="#4870BE" />
                                            
                                        </asp:GridView>
       <br /><br />

            <fieldset>
                <legend class="AgapeH3">Comments</legend>
               
               
                 <asp:UpdatePanel ID="commentUpdatePanel" runat="server">
                <ContentTemplate>


                <asp:PlaceHolder ID="phComments" runat="server"></asp:PlaceHolder>
                

                <div style="clear: both; " />
               
                <table>
                    <tr>
                        <td>
                          <asp:TextBox ID="tbMessage" runat="server" Width="100%" ></asp:TextBox>
                        </td>
                        <td style="padding-left: 5px" >
                         <asp:Button ID="btnAddMessage"
                    runat="server" Text="Send" Font-Size="XX-Small"   class="aButton btn" />
                        </td>
                    </tr>
                </table>

              </ContentTemplate>
              <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnAddMessage" EventName="Click" />
                    <asp:PostBackTrigger ControlID="btnDownload" />
              </Triggers>
               </asp:UpdatePanel>
            </fieldset>
        </td>
    </tr>
</table>

<div id="CommentMenu" class="contextMenu" style="z-index: 1000; " >
    <ul>
        <li id="DeleteComment" style="white-space: nowrap;"><img src="/icons/sigma/Delete_16x16_Standard.png" /> Remove Comment...</li>
        
    </ul>
</div>

</asp:Panel>