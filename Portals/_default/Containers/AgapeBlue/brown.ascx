<%@ Control language="vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Containers.Container" %>

<%@ Register TagPrefix="dnn" TagName="TITLE" Src="~/Admin/Containers/Title.ascx" %>
<%@ Register TagPrefix="dnn" TagName="ACTIONBUTTON" Src="~/Admin/Containers/ActionButton.ascx" %>

<div  style="width:100% ; margin-left: 0; margin-right: 0 ;">
   <table cellpadding="0" cellspacing="0" style="width: 100%;  -moz-border-radius-bottomleft: 15px; -moz-border-radius-bottomright: 15px; -moz-border-radius-topleft: 3px; -moz-border-radius-topright: 3px;"  >
          <tr>
              <td class="brown_top_left"><img src="/portals/_default/containers/agapeblue/images/BrownTopLeft.gif" alt="" width="16" height="17"  /></td>
              <td class="brown_top_middle"></td>
              <td class="brown_top_right"><img src="/portals/_default/containers/agapeblue/images/BrownTopRight.gif" alt="" width="19" height="17"  /></td>
          </tr>
          <tr>
              <td class="brown_center_left"></td>
              <td class="BrownMainContent">
              <table cellpadding="-1" cellspacing="-1" width="100%">
                    <tr>
                        <td valign="middle" ></td>
                       
                        <td valign="middle" align="center"  width="100%" nowrap="nowrap" class="BrownTitle"><dnn:TITLE runat="server" id="dnnTITLE" /></td>
                       
                    </tr>
                </table>
              <table width="100%">
                <tr><td id="ContentPane" runat="server" valign="top" colspan="2" height="100%" width="100%" ></td>
                
                </tr>
                <tr >
                    <td align="left" valign="middle" nowrap="nowrap" ><dnn:ACTIONBUTTON runat="server" id="dnnACTIONBUTTON1" CommandName="AddContent.Action" DisplayIcon="True" DisplayLink="True" /></td>
                    <td align="right" valign="middle" nowrap="nowrap"><dnn:ACTIONBUTTON runat="server" id="dnnACTIONBUTTON2" CommandName="SyndicateModule.Action" DisplayIcon="True" DisplayLink="True" />&nbsp;<dnn:ACTIONBUTTON runat="server" id="dnnACTIONBUTTON3" CommandName="PrintModule.Action" DisplayIcon="True" DisplayLink="False" />&nbsp;<dnn:ACTIONBUTTON runat="server" id="dnnACTIONBUTTON4" CommandName="ModuleSettings.Action" DisplayIcon="True" DisplayLink="False" /></td>
                
                </tr>
              </table>
           
              </td>
              
              
              <td class="brown_center_right"></td>
          </tr>
      
          
          <tr>
              <td class="brown_bottom_left"><img src="/portals/_default/containers/agapeblue/images/BrownBottomLeft.gif" alt="" / width="16" height="20" /></td>
              <td class="brown_bottom_middle"></td>
              <td class="brown_bottom_right"><img src="/portals/_default/containers/agapeblue/images/BrownBottomRight.gif" alt="" width="19" height="20" /></td>
          </tr>
      </table>
</div>


