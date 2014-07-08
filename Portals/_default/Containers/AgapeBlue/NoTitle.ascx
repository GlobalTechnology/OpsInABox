<%@ Control language="vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Containers.Container" %>
<%@ Register TagPrefix="dnn" TagName="ACTIONS" Src="~/Admin/Containers/SolPartActions.ascx" %>
<%@ Register TagPrefix="dnn" TagName="ACTIONBUTTON" Src="~/Admin/Containers/ActionButton.ascx" %>



<div  style="width:100% ; margin-left: 0; margin-right: 0 ; ">
   <table cellpadding="0" cellspacing="0" width="100%" style="width: 100%; margin-left: 0; margin-right: 0 ; -moz-border-radius-bottomleft: 15px; -moz-border-radius-bottomright: 15px; -moz-border-radius-topleft: 3px; -moz-border-radius-topright: 3px;"  >
          <tr>
              <td class="green_top_left"><img src="/portals/_default/containers/agapeblue/images/GreenTopLeft.gif" alt="" width="16" height="17"   /></td>
              <td class="green_top_middle"></td>
              <td class="green_top_right"><img src="/portals/_default/containers/agapeblue/images/GreenTopRight.gif" alt="" width="19" height="17"   /></td>
          </tr>
          <tr>
              <td class="green_center_left"></td>
              <td class="GreenMainContent">
              <table cellpadding="-1" cellspacing="-1" width="100%">
                    <tr>
                        <td valign="middle" ><dnn:ACTIONS runat="server" id="dnnACTIONS" ProviderName="DNNMenuNavigationProvider" ExpandDepth="1" PopulateNodesFromClient="True" /></td>
                      
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
              
              
              <td class="green_center_right"></td>
          </tr>
      
          
          <tr>
              <td class="green_bottom_left"><img src="/portals/_default/containers/agapeblue/images/GreenBottomLeft.gif" alt="" / width="16" height="20"   /></td>
              <td class="green_bottom_middle"></td>
              <td class="green_bottom_right"><img src="/portals/_default/containers/agapeblue/images/GreenBottomRight.gif" alt="" width="19" height="20"   /></td>
          </tr>
      </table>
</div>


