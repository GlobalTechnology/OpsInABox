<%@ Control Language="VB" AutoEventWireup="false"   CodeFile="LanguageEditor.ascx.vb" Inherits="DesktopModules_AgapeConnect_Translate_LanuguageEditor" %>


<asp:HiddenField ID="hfTranslateResx" runat="server" />
  <asp:DataList ID="dlEditor" runat="server"  width="100%">
            <ItemTemplate>
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td> 
                            <asp:Label ID="lblName" runat="server" font-size="XX-Small" ForeColor="Gray" Text='<%# Eval("Name") %>'></asp:Label>
                        </td>
                        <td width="5px">
                         &nbsp;
                        </td>
                        <td>
                         
                        </td>
                        <td></td>
                    </tr>
                    <tr valign="middle" >
                        <td width="48%">
                            <asp:TextBox ID="tbEnglish" runat="server" Text='<%# Eval("English") %>' TextMode="MultiLine" Rows="3" Width="100%" Enabled="False"  ToolTip='<%# Eval("Comment") %>'></asp:TextBox>
                        </td>
                        <td width="4%"> &nbsp;</td>
                        <td width="48%">
                            <asp:TextBox ID="tbTranslation" runat="server" Text='<%# Eval("Foreign") %>' TagKey='<%#  Eval("ForeignName") %>' onkeyup="$(this).addClass('Translation'); pendingChanges();"  CssClass="foreign" TextMode="MultiLine" Rows="3" Width="100%"  ></asp:TextBox>
                        </td>
                        <td style="padding-left: 3px;">
                            <div> <asp:CheckBox ID="cbPortalVarient" runat="server" Text="Portal Varient" Checked='<%# Eval("PortalVarient") or True %>' Visible="false" /></div>
                            <div style="margin: 5px ;" title="GoogleTranslate">
                                <Button id="Button1" type="button" value="Translate"  style="height: 20px;" onclick="bingTranslate('<%# Eval("English") %>', '<%#  Eval("ForeignName").Replace("\", "\\") %>' );" class="aGoogleButton" />
                           
                               
                            <asp:Button ID="btnGoogleTranslate" runat="server"  Font-Size="X-Small" Text="Google Translate"  Visible="false"  /></div>
                        </td>
                    </tr>
                </table>
                
                
                
                
            </ItemTemplate>
 </asp:DataList>
