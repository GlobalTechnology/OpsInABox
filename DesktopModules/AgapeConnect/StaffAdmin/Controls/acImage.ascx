<%@ Control Language="VB" AutoEventWireup="false" CodeFile="acImage.ascx.vb" Inherits="DesktopModules_AgapePortal_StaffBroker_acImage" %>
<script src="/js/jquery.Jcrop.js" type="text/javascript"></script>
<link href="/js/jquery.Jcrop.css" rel="stylesheet" type="text/css" />
<script type="text/javascript">
    (function ($, Sys) {
        function setUpMyTabs<%= NewImage.ClientId() %>() {
            $('#<%= NewImage.ClientId() %>').dialog({
                autoOpen: false,
                height: 140,
                width: 300,
                modal: true,
                title: "Upload New Image",
                close: function () {
                    allFields.val("").removeClass("ui-state-error");
                }
            });

            $('#<%= NewImage.ClientId() %>').parent().appendTo($("form:first"));


            $('#<%= theImage.ClientId() %>').Jcrop({
            onChange:  updateHFs<%= theImage.ClientId() %>,
        onSelect:  updateHFs<%= theImage.ClientId() %>,
        aspectRatio: <%= Aspect %> 
        }, function(){
        // Use the API to get the real image size
        var bounds = this.getBounds();
        boundx = bounds[0];
        boundy = bounds[1];
        // Store the API in the jcrop_api variable
        jcrop_api = this;
      
      });

   
       $('.aButton').button();
         


         $('#<%= FileUpload1.ClientId() %>').change(function() {

    var val = $(this).val();

    switch(val.substring(val.lastIndexOf('.') + 1).toLowerCase()){
        case 'gif': case 'jpg': case 'png': case 'jpeg':
             $('#<%= btnUpload.ClientId() %>').button("enable"); 
            break;
        default:
            $(this).val('');
            $('#<%= btnUpload.ClientId() %>').button("disable"); 
            break;
    }
});
        }

        $(document).ready(function () {
            setUpMyTabs<%= NewImage.ClientId() %>();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                setUpMyTabs<%= NewImage.ClientId() %>();
               
            });
        });
    } (jQuery, window.Sys));

    function showPopup<%= NewImage.ClientId %>() { $(<%= NewImage.ClientId %>).dialog("open"); return false; }
    function closePopup<%= NewImage.ClientId %>() { $(<%= NewImage.ClientId %>).dialog("close"); }
     
   
     
     function updateHFs<%= theImage.ClientId() %>(c)
      {

        if (parseInt(c.w) > 0)
        {
          // var rx = 100 / c.w;
          //var ry = 100 / c.h;

          $('#<%= hfX.ClientID  %>').val(c.x);
          $('#<%= hfY.ClientID  %>').val(c.y);
          $('#<%= hfW.ClientID  %>').val(c.w);
          $('#<%= hfH.ClientID  %>').val(c.h);
        

        
        }
        return 
      };

  </script>
<asp:HiddenField ID="hfFileId" runat="server" />
<asp:HiddenField ID="hfX" runat="server" />
<asp:HiddenField ID="hfY" runat="server" />
<asp:HiddenField ID="hfW" runat="server" />
<asp:HiddenField ID="hfH" runat="server" />
<asp:HiddenField ID="hfLoaded" runat="server" Value="false" />
<asp:HiddenField ID="hfAspect" runat="server" />
<div style="width: 200px; text-align: center; position: relative">
<asp:Image ID="theImage" runat="server" />


<div id="helpText" title="<%=LocalizeString("lblCrop") %>" style="-moz-opacity:.50; filter:alpha(opacity=70); opacity:.50; width: 200px; background-color: Black; color:white; position: absolute; top: 0;"  >
<%=LocalizeString("lblCrop") %></div>
<div>
 <input type="button" id="btnNewImage" value="<%=LocalizeString("btnNewImage") %>"  onclick='showPopup<%= NewImage.ClientId %>();' class="aButton btn" style="font-size: 8pt" />
<asp:Button ID="btnUpdate" runat="server" resourcekey="btnUpdate" Text="Update" CssClass="aButton btn" style="font-size: 8pt" />
</div>
</div>


<div id="NewImage" runat="server" style="text-align: center; ">
 <asp:FileUpload ID="FileUpload1" runat="server" width="240px"/>
 <br /><br />
    <asp:Label ID="Label1" runat="server" ForeColor="Red" Font-Italic="true"></asp:Label>
 <asp:Button ID="btnUpload" runat="server" resourcekey="btnUpload" CssClass="aButton btn" Enabled="false" />
</div>
