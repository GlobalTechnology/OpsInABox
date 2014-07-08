<%@ Control Language="VB" AutoEventWireup="false" CodeFile="StaffAdvanceRmb.ascx.vb"
    Inherits="DotNetNuke.Modules.StaffAdvanceRmb.StaffAdvanceRmb" %>
<%@ Register src="Currency.ascx" tagname="Currency" tagprefix="uc1" %>
<script type="text/javascript">
   


     (function ($, Sys) {



         $(document).ready(function () {
             $(".heading").unbind('click');
             $(".content").hide();
             $(".heading").click(function () {
                 $(this).next(".content").slideToggle(500, function(){
				        if ($(this).is(':hidden')) {
				            $("#<%= lblChange.ClientId()  %>").text("(Click to show details...)");
				        } else {
				            $("#<%= lblChange.ClientId()  %>").text("(Click to hide details...)");
				        }
				
				        return false;
			       }  );

               
            });

         });
     } (jQuery, window.Sys));
   


</script>
<style type="text/css">
.heading {
margin: 1px;

padding: 3px 10px;
cursor: pointer;
position: relative;
background-color:#E2CB9A;
border-bottom-style: dashed;
border-width: 1px ;
}
.content {
padding: 5px 10px;
background-color:#fafafa;
}
</style>


<asp:Label ID="lblTitle" runat="server" Text="Advance Request Form" CssClass="AgapeH3" ResourceKey="lblAdvRequestForm"></asp:Label>
<asp:HiddenField ID="hfCurrentRequest" runat="server" />
<asp:HiddenField ID="hfOverAuth" runat="server" />

<table>
    <tr>
        <td>
            <asp:Label ID="lblMainText" runat="server" resurceKey="lblAdvanceDesc"> </asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <br />
            <asp:Label ID="lblIntro" runat="server" Font-Italic="True" ResourceKey="lblAdvIntro" ></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:TextBox ID="tbRequestText" runat="server" Rows="6" TextMode="MultiLine" resourceKey="tbAdvReason"
                Width="600px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblAmount" runat="server" ResourceKey="lblAmount" CssClass="AgapeH5"></asp:Label>&nbsp;
            
            <asp:Label ID="lblCur" runat="server"  Font-Bold="true" Font-Size="Large"></asp:Label> 
            <asp:TextBox ID="tbAmount" runat="server"  Font-Bold="true" Font-Size="Large" Width="100px" CssClass="numeric advAmount"></asp:TextBox>
           
            <uc1:Currency ID="Currency1" runat="server"  AdvMode="true"  />
           
        </td>
    </tr>
    
    <tr>
        <td>
            <div class="heading"  >
                <asp:Label ID="lblNotes" runat="server"  CssClass="Agape_Red_H5" ResourceKey="lblNotes"></asp:Label>
                <span style="width: 50px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                <asp:Label ID="lblChange" runat="server" CssClass="Agape_Body_Text" ResourceKey="lblShowDetails.Text"></asp:Label>
            </div>
            <div class="content">
                <ul>
                    <li>
                        <asp:Label ID="lblNotes1" runat="server" ResourceKey="lblNotes1"></asp:Label>
                     </li>
                    <li>
                    <asp:Label ID="lblNotes2" runat="server" ></asp:Label>
                    </li>
                    <li>
                    <asp:Label ID="lblNotes3" runat="server" ResourceKey="lblNotes3"></asp:Label>
                    
                        <asp:Image ID="imgExample" runat="server" Width="550px" Height="80px" ImageUrl="../images/AdvanceExample.jpg" />
                    </li>
                </ul>
             
            </div>
          
        </td>
    </tr>
   
</table>
