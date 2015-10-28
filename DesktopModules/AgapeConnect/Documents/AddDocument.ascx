<%@ Control Language="VB" AutoEventWireup="false" CodeFile="AddDocument.ascx.vb" Inherits="DotNetNuke.Modules.AgapeConnect.Documents.AddDocument" %>
<script src="/js/jquery.MultiFile.js" type="text/javascript"></script>
<script type="text/javascript">
    $('.multi').MultiFile({
        list: '#fileUploadList'
    });


    function triggerFileUpload() {
        document.getElementById("File1").click();
    }
</script>

<div id="divUpload" style="text-align: center;">
    <input id="Button2" type="button" onclick="triggerFileUpload()" value="Add Files"
        class="aButton btn" style="float: right; margin: 7px 40px 0 0;" /><br />
    <div style="text-align: left;">
        <asp:Label ID="lblSelectFiles" runat="server" ResourceKey="lblSelectFiles"></asp:Label>
    </div>
    <input id="File1" type="file" style="visibility: hidden; height: 0;" class="multi" />
    <div style="text-align: left;">
        <asp:Label ID="Label1" runat="server" Font-Size="X-Small" ForeColor="Gray" Font-Italic="true"
            ResourceKey="lblSelectFilesHelp"></asp:Label>
    </div>
    <asp:Button ID="btnUpoadFiles" runat="server" Text="Upload Selected Files" class="aButton btn"
        Font-Size="X-Large" Style="margin-top: 30px;" />
</div>