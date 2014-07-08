<%@ Control language="vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Containers.Container" %>
<%@ Register TagPrefix="dnn" TagName="ACTIONS" Src="~/Admin/Containers/SolPartActions.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TITLE" Src="~/Admin/Containers/Title.ascx" %>
<%@ Register TagPrefix="dnn" TagName="ACTIONBUTTON" Src="~/Admin/Containers/ActionButton.ascx" %>

<div  style="width:100% ; ">
   <table cellpadding="0" cellspacing="0"   >
          <tr>
              <td class="plum_top_left"><img src="/portals/_default/containers/agapeblue/images/PlumTopLeft.gif" alt="" width="16" height="17"   /></td>
              <td class="plum_top_middle"/>
              <td class="plum_top_right"><img src="/portals/_default/containers/agapeblue/images/PlumTopRight.gif" alt="" width="19" height="17"     /></td>
          </tr>
          <tr>
              <td class="plum_center_left"></td>
              <td class="PlumMainContent">
              <table cellpadding="0" cellspacing="0" width="100%" >
                    <tr>
                        <td valign="middle" ><dnn:ACTIONS runat="server" id="dnnACTIONS" ProviderName="DNNMenuNavigationProvider" ExpandDepth="1" PopulateNodesFromClient="True" /></td>
                       
                        <td valign="middle" align="center"   nowrap="nowrap" width="100%" class="PlumTitle"><dnn:TITLE runat="server" id="dnnTITLE" /></td>
                       
                    </tr>
                </table>
              <table>
                <tr><td id="ContentPane" runat="server" valign="top" colspan="2" height="100%" width="100%"  ></td>
                
                </tr>
                <tr >
                    <td align="left" valign="middle" nowrap="nowrap" ><dnn:ACTIONBUTTON runat="server" id="dnnACTIONBUTTON1" CommandName="AddContent.Action" DisplayIcon="True" DisplayLink="True" /></td>
                    <td align="right" valign="middle" nowrap="nowrap"><dnn:ACTIONBUTTON runat="server" id="dnnACTIONBUTTON2" CommandName="SyndicateModule.Action" DisplayIcon="True" DisplayLink="True" />&nbsp;<dnn:ACTIONBUTTON runat="server" id="dnnACTIONBUTTON3" CommandName="PrintModule.Action" DisplayIcon="True" DisplayLink="False" />&nbsp;<dnn:ACTIONBUTTON runat="server" id="dnnACTIONBUTTON4" CommandName="ModuleSettings.Action" DisplayIcon="True" DisplayLink="False" /></td>
                
                </tr>
              </table>
           
              </td>
              
              
              <td class="plum_center_right"></td>
          </tr>
      
          
          <tr>
              <td class="plum_bottom_left"><img src="/portals/_default/containers/agapeblue/images/PlumBottomLeft.gif" alt="" width="16" height="20"     /></td>
              <td class="plum_bottom_middle" />
              <td class="plum_bottom_right"><img src="/portals/_default/containers/agapeblue/images/PlumBottomRight.gif" alt="" width="19" height="20"    /></td>
          </tr>
      </table>
</div>


