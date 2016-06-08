<%@ Control language="vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Containers.Container" %>

<%@ Register TagPrefix="dnn" TagName="TITLE" Src="~/Admin/Containers/Title.ascx" %>
<%@ Register TagPrefix="dnn" TagName="ACTIONBUTTON" Src="~/Admin/Containers/ActionButton.ascx" %>

<div  style="width:192px ; max-width:192px; margin-left: 0; margin-right: 0 ; margin-bottom: 10px ;">
   <table cellpadding="0" cellspacing="0" width="100%" style="width: 100%; margin-left: 0; margin-right: 0 ; -moz-border-radius-bottomleft: 15px; -moz-border-radius-bottomright: 15px; -moz-border-radius-topleft: 3px; -moz-border-radius-topright: 3px;"  >
          <tr>
               <td class="blue_stripe_top">
                 <table cellpadding="-1" cellspacing="-1" width="100%">
                    <tr>
                        <td valign="middle" ></td>
                       
                        <td valign="middle" align="center" width="100%" nowrap="nowrap" class="BlueTitle"><dnn:TITLE runat="server" id="dnnTITLE" /></td>
                       
                    </tr>
                </table>
               </td>
              
          </tr>
          <tr>
             
              <td class="BlueStripeMainContent" >
            
              <table width="100%">
                <tr><td  id="ContentPane" runat="server" valign="top" colspan="2" height="100%" width="100%" ></td>
                
                </tr>
                <tr >
                    <td align="left" valign="middle" nowrap="nowrap" ><dnn:ACTIONBUTTON runat="server" id="dnnACTIONBUTTON1" CommandName="AddContent.Action" DisplayIcon="True" DisplayLink="True" /></td>
                    <td align="right" valign="middle" nowrap="nowrap"><dnn:ACTIONBUTTON runat="server" id="dnnACTIONBUTTON2" CommandName="SyndicateModule.Action" DisplayIcon="True" DisplayLink="True" />&nbsp;<dnn:ACTIONBUTTON runat="server" id="dnnACTIONBUTTON3" CommandName="PrintModule.Action" DisplayIcon="True" DisplayLink="False" />&nbsp;<dnn:ACTIONBUTTON runat="server" id="dnnACTIONBUTTON4" CommandName="ModuleSettings.Action" DisplayIcon="True" DisplayLink="False" /></td>
                
                </tr>
              </table>
           
              </td>
              
              
              
          </tr>
      
          
          <tr>
              
              <td class="blue_stripe_bottom">
              
              </td>
             
          </tr>
      </table>
</div>


