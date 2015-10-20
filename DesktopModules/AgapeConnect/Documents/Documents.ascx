<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Documents.ascx.vb" Inherits="DotNetNuke.Modules.AgapeConnect.Documents" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register Src="~/controls/urlcontrol.ascx" TagName="urlcontrol" TagPrefix="uc1" %>
<%@ Register Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" TagPrefix="cc1" %>
<link href="/js/jquery.coolfieldset.css" rel="stylesheet" type="text/css" />
<link href="/Portals/_default/Skins/AgapeBlue/flick/jquery-ui-1.8.18.custom.css"
    rel="stylesheet" type="text/css" />
<script src="/js/jquery.coolfieldset.js" type="text/javascript"></script>
<script src="/js/jquery.contextmenu.r2.js" type="text/javascript"></script>
<script type="text/javascript" src="/js/splitter-152.js"></script>
<script src="/js/jquery.cookie.js" type="text/javascript"></script>
<script src="/js/jquery.MultiFile.js" type="text/javascript"></script>
<script type="text/javascript">

    function setUpMyTabs() {

        $('.FolderButton').button({ icons: { primary: "ui-icon-plusthick"} });
        $('.LinkButton').button({ icons: { primary: "ui-icon-extlink"} });
        $('.Upload').button({ icons: { primary: "ui-icon-newwin"} });
        $('.aButton').button();

       $('.tblIconsDiv').css('width','<%= Settings("ColumnWidth") %>');   

        $("#divNewFolder").dialog({
            autoOpen: false,
            height: 300,
            width: 500,
            modal: true,
            title: 'New Folder'
        });
        $("#divNewFolder").parent().appendTo($("form:first"));

        $("#divNewVersion").dialog({
            autoOpen: false,
            height: 300,
            width: 500,
            modal: true,
            title: 'New Version'
        });
        $("#divNewVersion").parent().appendTo($("form:first"));

        $("#divNewIcon").dialog({
            autoOpen: false,
            height: 300,
            width: 500,
            modal: true,
            title: 'New Icon'
        });
        $("#divNewIcon").parent().appendTo($("form:first"));

        $("#divNewLink").dialog({
            autoOpen: false,
            height: 450,
            width: 500,
            modal: true,
            title: 'New Link'
        });
        $("#divNewLink").parent().appendTo($("form:first"));

        $("#divEditFolder").dialog({
            autoOpen: false,
            height: 500,
            width: 750,
            modal: true,
            title: 'Edit Folder'
        });
        $("#divEditFolder").parent().appendTo($("form:first"));

        $("#divEditFile").dialog({
            autoOpen: false,
            height: 600,
            width: 850,
            modal: true,
            title: 'Edit File'
        });
        $("#divEditFile").parent().appendTo($("form:first"));

        $("#divUpload").dialog({
            autoOpen: false,
            height: 320,
            width: 500,
            modal: true,
            title: 'Upload Files'
        });
        $("#divUpload").parent().appendTo($("form:first"));

        //  $(document)[0].oncontextmenu = function () { return false; } 
        $('.aFolder1').mousedown(function (e) {
            if (e.which === 3) {
                /* Right Mousebutton was clicked! */

                $('#myMenu1').css({ 'top': e.pageX, 'left': e.pageY })

            }
        });
        //MuliFile Uploader
        $('.multi').MultiFile({
            list: '#fileUploadList'
        });
       
         <% if Settings("DisplayStyle")="ExplorerNoTree" or Settings("DisplayStyle")="Table" or Request.QueryString("search")<>"" Then %>
            $("#LeftPane").hide();
            
        <% elseif (Settings("DisplayStyle")="Tree" or Settings("DisplayStyle")="GTree") and Request.QueryString("search")=""  %>
        $("#RightPane").hide();
            <% else %>
             $("#MySplitter").splitter({
            type: "v",
            outline: false,
            sizeLeft: 220,
            minLeft: 0,
            minRight: 400,
            resizeToWidth: false
        });
        <% End If %>

        // Here initialize the menou
        //Collapsable FieldSets
        $(".accordion")
			.accordion({
			    header: "> div > h3",
			    navigate: false
			});

			$('.SelectIcon').click(function (e) {
			   
			    if ($(e.target).attr('id') == "addIcon")
                {
                    $('#<%= hfNewIconMode.ClientId %>').val('File');
                    showNewIcon();
			        return;
                }
                else if($(e.target).attr('id') == "addIconF") {
                    $('#<%= hfNewIconMode.ClientId %>').val('Folder');
			        showNewIcon();
			        return;
			    }

			    // Unhighlight all the images
			    $('.SelectIcon').removeClass('iconHighlight');

			    // Highlight the newly selected image
			    $(this).addClass('iconHighlight');

			    $('#<%= hfSelectedIcon.ClientID %>').val(getImgFileId($(this).attr('src')));

			});

//        $('.Draggable').draggable({ start: handleDragStart });
//        $('.Droppable').droppable({
//            drop: handleDropEvent
//        });

        $('.aFile, .aLink, .TreeView a').click(function () {
           
            if ($(this).css("cursor") == "move") return false;
            
        });

        $('.aFolder').click(function (ev) {
            
            if ($(this).css("cursor") == "move") return false;
           
            if ($('#movable').val() == "true") {

                ev.preventDefault();
                $('#MySplitter, .aFile, .aLink, .aFolder').css('cursor', '');

                $('.aFile, .aLink, .aFile div, .aLink div').css('opacity', '');
                $('.aFile, .aLink, .aFile div, .aLink div').css('filter', '');

                $('#<%= hfMoveToId.ClientID %>').val(getFolderId($(ev.target).closest("a").attr("href")));

                $('#movable').val('false');

                __doPostBack();
                return false;
            }
        });

        $('#deselectIcon').click(function () { $('.SelectIcon').removeClass('iconHighlight'); $(this).addClass('iconHighlight'); $('#<%= hfSelectedIcon.ClientID %>').val(-1); });

        $("input:radio").click(function () {
            $(".linkLabels").hide();
            $('#<%= tbURL.ClientID %>').hide();
            $('#<%= ddlFiles.ClientID %>').hide();
            $('#<%= ddlPages.ClientID %>').hide();
            switch (this.value) {
                case "0": $("#divURL").show(); $('#<%= tbURL.ClientID %>').show();  break;
                case "1": $("#divYouTube").show(); $('#<%= tbURL.ClientID %>').show(); break;
                case "2": $("#divGoogle").show(); $('#<%= tbURL.ClientID %>').show(); break;
                case "3": $("#divPage").show(); $('#<%= ddlPages.ClientID %>').show(); break;
                case "4": $("#divFile").show(); $('#<%= ddlFiles.ClientID %>').show(); break;
            }

        });

    }

        $(document).ready(function () {
            setUpMyTabs();

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () { setUpMyTabs(); });
        });

     function getFolderId(c){
         var FLoc = c.indexOf("FolderId=");
         return c.substring(FLoc + 9);
     }
     function getFileId(c) {
         var FLoc = c.indexOf("DocId=");
         return c.substring(FLoc + 6);
     }
     function getImgFileId(c) {
         var FLoc = c.indexOf("FileId=");
         return c.substring(FLoc + 7);
     }

    function showUpload() { $("#divUpload").dialog("open"); return false; }
    function closeUpload() { $("#divUpload").dialog("close"); }
    function showNewFolder() { $("#divNewFolder").dialog("open"); return false; }
    function closeNewFolder() { $("#divNewFolder").dialog("close"); }
    function showEditFolder() { $("#divEditFolder").dialog("open"); return false; }
    function closeEditFolder() { $("#divEditFolder").dialog("close"); }
    function showEditFile() { $("#divEditFile").dialog("open"); return false; }
    function closeEditFile() { $("#divEditFile").dialog("close"); }
    function showNewVersion() { $("#divNewVersion").dialog("open"); return false; }
    function closeNewVersion() { $("#divNewVersion").dialog("close"); }
    function showNewIcon() { $("#divNewIcon").dialog("open"); return false; }
    function closeNewIcon() { $("#divNewIcon").dialog("close"); }
    function showNewLink() { $("#divNewLink").dialog("open"); return false; }
    function closeNewLink() { $("#divNewLink").dialog("close"); }
    function triggerFileUpload() {document.getElementById("File1").click(); }
    function saveVersion() {
        alert($('#<%= fuNewVersion.ClientID %>').val());
       // $('#<%= hfFileName.ClientID %>').val($('#<%= fuNewVersion.ClientID %>').value);
       // __doPostBack('<%= upEditFile.ClientID %>', '');
    }

    function moveMode(t) {
        $('#movable').val('true');
        $('#MySplitter, .aFile, .aLink').css('cursor', 'move');
        $('.aFile, .aLink, .aFile div, .aLink div').css('opacity', '0.4');
        $('.aFile, .aLink, .aFile div, .aLink div').css('filter', 'alpha(opacity=40)');
        $(t).css('cursor', 'move');
   }
    function editButtonClick(t) { $('#<%= hfEditFileId.ClientID %>').val(getFileId(t.href)); $('#<%= tbEditFileName.ClientID %>').val("Loading..."); $('#<%= tbEditFileDescription.ClientID %>').val(""); __doPostBack('<%= upEditFIle.ClientID %>', ''); showEditFile(); }
    function deleteButtonClick(t) { alert('Trigger was ' + t.href + '\nAction was Delete'); }
    //function editButtonClick(t) { alert("You clicked it!"); }

</script>
<style type="text/css">
    .cbList {
        white-space: nowrap;
        width: 100%;
        text-align: left;
    }

        .cbList input {
            margin-left: -20px;
        }

        .cbList td {
            padding-left: 20px;
        }

    .TreeView img {
        width: 16px;
    }

    .GTreeView a.aLink {
        color: #28686E;
    }

    .iconHighlight {
        border: 2px inset Blue !important;
    }

    .iconHighlightStart {
        border: 2px inset #CCC !important;
    }

    .SelectIcon {
        border: 2px insert transparent !important;
        cursor: pointer;
    }

    .ui-state-default {
        background-color: #aca;
    }

    .ui-state-hover {
        background-color: #bdb;
    }

    .ui-state-highlight {
        background-color: #add;
    }

    .ui-state-error {
        background-color: #eaa;
    }

    .splitter {
        height: 400px;
        margin: 1em 3em;
        border: 2px solid #79C9EC;
        background: #fff;
    }

    .splitter-pane {
        overflow: auto;
    }

    .splitter-bar-vertical {
        width: 6px;
        background-image: url(img/vgrabber.gif);
        background-repeat: no-repeat;
        background-position: center;
        opacity: 0.7;
    }

    .splitter-bar-vertical-docked {
        width: 10px;
        background-image: url(img/vdockbar-trans.gif);
        background-repeat: no-repeat;
        background-position: center;
    }

    .splitter-bar.ui-state-highlight {
        opacity: 0.7;
    }

    .splitter-iframe-hide {
        visibility: hidden;
    }

    .splitter-bar {
        width: 6px;
        background: #F6F6F6;
        opacity: 1.0;
    }

    .vmenu {
        border: 1px solid #aaa;
        position: absolute;
        background: #fff;
        display: none;
        font-size: 0.75em;
    }

        .vmenu .first_li span {
            width: 100px;
            display: block;
            padding: 5px 10px;
            cursor: pointer;
        }

        .vmenu .inner_li {
            display: none;
            margin-left: 120px;
            position: absolute;
            border: 1px solid #aaa;
            border-left: 1px solid #ccc;
            margin-top: -28px;
            background: #fff;
        }

        .vmenu .sep_li {
            border-top: 1px ridge #aaa;
            margin: 5px 0;
        }

        .vmenu .fill_title {
            font-size: 11px;
            font-weight: bold;
            height: 15px;
            overflow: hidden;
            word-wrap: break-word;
        }

    .MultiFile-list {
        border: 2pt solid gray;
        margin: 10px;
        padding: 3px 3px 0px 10px;
        height: 100px;
        text-align: left;
        overflow: auto;
    }

    .MultiFile-label {
        font-size: large;
    }

    .ui-widget-content .MultiFile-remove {
        color: Red;
        font-weight: bold;
    }

    .AcPane {
        height: 180px;
    }

    .tblImage {
        float: left;
        width: 80px;
    }

    .tblText {
        font-size: x-small;
    }

    a.aFile:hover, a.aFileRead:hover, a.aFolder:hover, a.aFolderRead:hover {
        text-decoration: none;
    }

    .Draggable {
        border: 1px inset transparent;
    }

        .Draggable:hover {
            border: 1px inset blue;
        }

    .tblIconsDiv {
        Display: inline-table;
        text-align: center;
        padding: 5px;
        -webkit-border-radius: 5px;
        -moz-border-radius: 5px;
        border-radius: 5px;
        margin: 0;
        border: 1pt outset #EEE;
    }

    .tblContainer {
        text-align: left;
        width: 100%;
        height: 90px;
        overflow: auto;
    }

    .IconsDiv {
        Display: inline-table;
        text-align: center;
        padding: 10px;
        width: 100px;
        max-height: 100px;
    }

    .normalImage {
        height: 70px;
    }

    .normalTitle {
        height: 30px;
        word-wrap: break-word;
        width: 100%;
        font-size: 8pt;
    }

    .normalContainer {
        text-align: center;
        width: 100%;
    }

    .hideRoot {
        background-color: Blue;
    }
</style>
<div style="width: 100%">
    <div id="cover" style="position: absolute; width: 100%; height: 100%; z-index: 1;
        top: 0px; left: 0px; cursor: move; background: red; display: none;">
        &nbsp;
    </div>
    <div id="MySplitter">
        <div id="LeftPane" style="text-align: left;">
            <asp:TreeView ID="tvFolders" runat="server" ImageSet="XPFileExplorer" CssClass="TreeView"
                NodeIndent="15" ExpandDepth="1">
                <HoverNodeStyle Font-Underline="True" ForeColor="#6666AA" />
                <NodeStyle Font-Names="Tahoma" Font-Size="8pt" ForeColor="Black" HorizontalPadding="2px"
                    NodeSpacing="0px" VerticalPadding="2px" CssClass="myTreeViewNode" />
                <ParentNodeStyle Font-Bold="False" />
                <SelectedNodeStyle BackColor="#B5B5B5" Font-Underline="False" HorizontalPadding="0px"
                    VerticalPadding="0px" />
            </asp:TreeView>
            <asp:TreeView ID="graphicalTreeView" runat="server" NodeIndent="10" ExpandDepth="0" 
                Visible="False" CssClass="TreeView GTreeView" SelectAction="SelectExpand" CollapseImageUrl="/DesktopModules/AgapeConnect/Documents/images/ArrowWinD.gif"
                ExpandImageUrl="/DesktopModules/AgapeConnect/Documents/images/ArrowWinR.gif"
                Font-Size="14pt" RootNodeStyle-NodeSpacing="5px" NodeStyle-NodeSpacing="2px"
                NodeStyle-HorizontalPadding="5px">
            </asp:TreeView>
        </div>
        <div id="RightPane" class="aBlank">
            <%--<div style="white-space: normal; color: #CCC; font-size: small; font-style: italic;
                margin-left: 5px">
                <asp:Label ID="lblDisplaying" runat="server" resourceKey="lblDisplaying"></asp:Label>
                <asp:Label ID="lblDisplayingSearch" runat="server" resourceKey="lblDisplayingSearch"
                    Visible="false"></asp:Label>
                <asp:Label ID="lblFolder" runat="server"></asp:Label>
            </div>--%>
            <div style="clear: both;">
            </div>
            <br />
            <asp:ListView ID="dlFolderView" runat="server">
                <ItemTemplate>
                    <div id="Icons" runat="server" visible='<%# templateMode="Icons" %>' class='<%# "Draggable" & IIF(Settings("DisplayStyle")="Table", " tblIconsDiv", " IconsDiv")  %>'
                        width='<%# IIF(Settings("DisplayStyle")="Table", Settings("ColumnWidth") & "px", "") %>'>
                        <asp:HyperLink ID="HyperLink1" runat="server" ToolTip='<%# Eval("Description") & IIF(Eval("FileId") is nothing, "", vbnewline & "Author: " & Eval("Author"))  %>'
                            Target='<%# IIF((Eval("LinkType") =0 or Eval("LinkType")=2) and not Eval("FileId") is nothing, "_blank", "_self") %>'
                            CssClass='<%#   IIF(Eval("FileId") is nothing,IIF(  GetFilePermission(Eval("Permissions"))="Edit", "aFolder", "aFolderRead"),IIF(  GetFilePermission(Eval("Permissions"))="Edit", "aFile", "aFileRead")) %>'
                            NavigateUrl='<%#  IIF(Eval("FileId") is nothing, NavigateURL() & "?FolderId=" & Eval("FolderId"), GetFileUrl(Eval("DocId"), Eval("FileId")) ) %>'>
                            <div class='<%# IIF(Settings("DisplayStyle")="Table", "tblContainer", "normalContainer")  %>'
                                style="">
                                <asp:Image ID="icon" runat="server" CssClass='<%# IIF(Settings("DisplayStyle")="Table", "tblImage", "normalImage")  %>'
                                    ImageUrl='<%# GetFileIcon(Eval("FileId"), Eval("LinkType"), Eval("CustomIcon") ) %>'
                                    ToolTip='<%# Eval("Description") & IIF(Eval("FileId") is nothing, "", vbnewline & "Author: " & Eval("Author"))  %>' />
                                
                                <div style="padding: 0; margin: 0;">
                                    <asp:Label ID="lblItemName" runat="server" CssClass='<%# IIF(Settings("DisplayStyle")="Table", "tblTitle AgapeH4", "normalTitle")  %>'
                                        Text='<%# Eval("DisplayName") %>'></asp:Label>
                                </div>
                                <div id="theDesc" runat="server" visible='<%# Settings("DisplayStyle")="Table" %>'>
                                    <asp:Label ID="Label21" runat="server" CssClass="tblText" Text='<%# Eval("Description") %>'
                                        Style="word-wrap: break-word;"></asp:Label>
                                </div>
                            </div>
                        </asp:HyperLink>
                    </div>
                    <div id="docbuttons" style="float:right">
                        <asp:HyperLink ID="btnEditDoc" runat="server" class="aButton">Edit</asp:HyperLink>
                        <br />
                        <asp:HyperLink ID="btnDeleteDoc" runat="server" class="aButton">Delete</asp:HyperLink>
                    </div>
                    <div id="docbuttons" runat="server" style="float:right">
                        <asp:HyperLink ID="btnEditDoc" runat="server" CssClass="aButton">Edit</asp:HyperLink>
                        <%--<br />
                        <asp:HyperLink ID="btnDeleteDoc" runat="server" CssClass="aButton">Delete</asp:HyperLink>--%>
                    </div>
                </ItemTemplate>
            </asp:ListView>
        </div>
    </div>
</div>
<div style="clear: both;" />
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
    <asp:Button ID="btnUpoadFiles" runat="server" Text="Upload Selected Files" CssClass="aButton btn"
        Font-Size="X-Large" Style="margin-top: 30px;" />
</div>
<div id="divNewFolder" style="text-align: center">
    <table width="100%">
        <tr>
            <td>
                <dnn:Label ID="lblFolderName" runat="server" ControlName="tbNewFolderName" ResourceKey="lblFolderName" />
            </td>
            <td>
                <asp:TextBox ID="tbNewFolderName" runat="server" Width="100%" Columns="100" Font-Size="X-Large"
                    CssClass="ui-state-default"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <dnn:Label ID="lblDescription" runat="server" ControlName="tbNewFolderDescription"
                    ResourceKey="lblDescription" />
            </td>
            <td>
                <asp:TextBox ID="tbNewFolderDescription" runat="server" Rows="5" Width="100%" ForeColor="Gray"
                    Font-Size="Large" TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>
    </table>
    <div style="margin-top: 12px">
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbNewFolderName"
            Display="static" Text="" ValidationGroup="NewFolder" ErrorMessage="* You must Enter a Folder Name<br />"></asp:RequiredFieldValidator>
        <asp:Button ID="btnAddFolder" runat="server" Text="Add Folder" CssClass="aButton btn" ValidationGroup="NewFolder" />
        <input id="btnCancel" type="button" value='<%= Translate("btnCancel") %>' onclick="closeNewFolder();"
            class="aButton btn" />
    </div>
</div>
<div id="divNewVersion" style="text-align: center">
    <table width="100%">
        <tr>
            <td>
                <dnn:Label ID="Label15" runat="server" ControlName="tbEditFileName" ResourceKey="lblSelectFile" />
            </td>
            <td>
                <asp:FileUpload ID="fuNewVersion" runat="server" />
                <asp:HiddenField ID="hfFileName" runat="server" />
            </td>
        </tr>
    </table>
    <div style="margin-top: 12px">
        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="fuNewVersion"
            Display="static" Text="" ValidationGroup="NewVersion" ErrorMessage="* You must select a file<br />"></asp:RequiredFieldValidator>
        <asp:Button ID="btnSaveNewVersion" runat="server" Text="Add Version" CssClass="aButton btn"
            ValidationGroup="NewVersion" />
        <input id="Button5" type="button" value='<%= Translate("btnCancel") %>' onclick="closeNewVersion();"
            class="aButton btn" />
    </div>
</div>
<div id="divNewIcon" style="text-align: center">
    <table width="100%">
        <tr>
            <td>
                <dnn:Label ID="Label20" runat="server" ResourceKey="lblSelectIcon" />
            </td>
            <td>
                <asp:FileUpload ID="fuNewIcon" runat="server" />
                <asp:HiddenField ID="HiddenField1" runat="server" />
                <asp:HiddenField ID="hfNewIconMode" runat="server" />
            </td>
        </tr>
    </table>
    <div style="margin-top: 12px">
        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="fuNewIcon"
            Display="static" Text="" ValidationGroup="NewIcon" ErrorMessage="* You must select a file<br />"></asp:RequiredFieldValidator>
        <asp:Button ID="btnNewIcon" runat="server" Text="Add Icon" CssClass="aButton btn" ValidationGroup="NewIcon" />
        <input id="Button7" type="button" value='<%= Translate("btnCancel") %>' onclick="closeNewIcon();"
            class="aButton btn" />
    </div>
</div>
<div id="divNewLink" style="text-align: center">
    <table width="100%">
        <tr>
            <td>
                <dnn:Label ID="Label17" runat="server" ResourceKey="lblDisplayName" />
            </td>
            <td>
                <asp:TextBox ID="tbNewLinkName" runat="server" Width="100%" MaxLength="100" Font-Size="X-Large"
                    CssClass="ui-state-default"></asp:TextBox>
            </td>
        </tr>
        <tr valign="top">
            <td>
                <dnn:Label ID="Label18" runat="server" ResourceKey="lblDescription" />
            </td>
            <td>
                <asp:TextBox ID="tbNewLinkDescripiton" runat="server" Rows="4" Width="100%" ForeColor="Gray"
                    Font-Size="Large" TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <dnn:Label ID="Label19" runat="server" ResourceKey="lblFileAuthor" />
            </td>
            <td>
                <asp:TextBox ID="tbNewLinkAuthor" runat="server" MaxLength="100" Width="100%" ForeColor="Gray"
                    Font-Size="Large"></asp:TextBox>
            </td>
        </tr>
        <tr style="vertical-align: top;">
            <td style="width: 150px;">
                <dnn:Label ID="Label16" runat="server" ResourceKey="lblLinkType" />
            </td>
            <td style="text-align: Left">
                <asp:RadioButtonList ID="rbLinkType" runat="server">
                    <asp:ListItem Text="External URL" Value="0" Selected="True" />
                    <asp:ListItem Text="YouTube Video" Value="1" />
                    <asp:ListItem Text="Google Doc" Value="2" />
                    <asp:ListItem Text="A Page on this site" Value="3" />
                    <asp:ListItem Text="A File on this site" Value="4" />
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td>
                <div id="divURL" class="linkLabels">
                    <dnn:Label ID="lblURL" runat="server" ResourceKey="lblURL" />
                </div>
                <div id="divYouTube" class="linkLabels" style="display: none">
                    <dnn:Label ID="lblYouTube" runat="server" ResourceKey="lblYouTube" />
                </div>
                <div id="divGoogle" class="linkLabels" style="display: none">
                    <dnn:Label ID="lblGoogle" runat="server" ResourceKey="lblGoogle" />
                </div>
                <div id="divPage" class="linkLabels" style="display: none">
                    <dnn:Label ID="lblPage" runat="server" ResourceKey="lblPage" />
                </div>
                <div id="divFile" class="linkLabels" style="display: none">
                    <dnn:Label ID="lblFile" runat="server" ResourceKey="lblFile" />
                </div>
            </td>
            <td style="text-align: left;">
                <asp:DropDownList ID="ddlFiles" runat="server" Style="display: none;">
                </asp:DropDownList>
                <asp:DropDownList ID="ddlPages" runat="server" Style="display: none;">
                </asp:DropDownList>
                <asp:TextBox ID="tbURL" runat="server" MaxLength="100" Width="100%" ForeColor="Gray"
                    Font-Size="Large"></asp:TextBox>
            </td>
        </tr>
    </table>
    <div style="margin-top: 12px">
        <asp:Button ID="btnNewLink" runat="server" Text="Add Link" CssClass="aButton btn" ValidationGroup="NewLink" />
        <input id="Button6" type="button" value='<%= Translate("btnCancel") %>' onclick="closeNewLink();"
            class="aButton btn" />
    </div>
</div>
<div id="divEditFolder" style="text-align: center">
    <asp:UpdatePanel ID="upEditFolder" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlEditFolder" runat="server" Enabled="false">
                <table cellpadding="4px;">
                    <tr valign="top">
                        <td width="50%">
                            <table width="100%">
                                <tr>
                                    <td>
                                        <dnn:Label ID="aLabel" runat="server" ControlName="tbNewFolderName" ResourceKey="lblFolderName" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbEditFolderName" runat="server" Width="100%" MaxLength="100" Font-Size="X-Large"
                                            CssClass="ui-state-default"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr valign="top">
                                    <td>
                                        <dnn:Label ID="Label3" runat="server" ControlName="tbNewFolderDescription" ResourceKey="lblDescription" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbEditFolderDescription" runat="server" Rows="5" Width="100%" ForeColor="Gray"
                                            Font-Size="Large" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <div style="text-align: left;">
                                <div id="Div1" class="accordion">
                                    <div>
                                        <h3>
                                            <a href="#" id="A1" class="AcHdr">
                                                <asp:Label ID="Label10" runat="server" Font-Bold="true" ResourceKey="lblVersions"></asp:Label>
                                            </a>
                                        </h3>
                                        <div id="Div2" class="AcPane">
                                            <asp:GridView ID="GridView1" runat="server">
                                            </asp:GridView>
                                        </div>
                                    </div>
                                    <div>
                                        <h3>
                                            <a href="#" id="A2" class="AcHdr">
                                                <asp:Label ID="Label11" runat="server" Font-Bold="true" ResourceKey="lblIcon"></asp:Label>
                                            </a>
                                        </h3>
                                        <div id="divIcons" class="AcPane">
                                           
                                            <asp:DataList ID="ddlIconsF" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                <ItemTemplate>
                                                    <asp:Image ID="imgIcon" CssClass='<%# IIF( Eval("Selected"), "iconHighlight SelectIcon" , "SelectIcon") %>'
                                                        runat="server" Height="40px" ImageUrl='<%# Eval("Path") & "?FileId=" & Eval("FileId") %>' />
                                                </ItemTemplate>
                                            </asp:DataList>
                                            <div class="SelectIcon" style="display: inline-block; text-align: center; vertical-align: middle;
                                                width: 44px; background-color: #EEE; padding: 10px 0 10px 0; font-size: xx-small">
                                                Default Icon</div>
                                            <div id="addIconF" class="SelectIcon" style="display: inline-block; text-align: center;
                                                vertical-align: middle; width: 44px; background-color: #EEE; padding: 10px 0 10px 0;
                                                font-size: xx-small">
                                                Add Icon</div>


                                        </div>
                                    </div>
                                    <div>
                                        <h3>
                                            <a href="#" id="A3" class="AcHdr">
                                                <asp:Label ID="Label13" runat="server" Font-Bold="true" ResourceKey="lblPermissions"></asp:Label>
                                            </a>
                                        </h3>
                                        <div id="Div4" class="AcPane">
                                            <div style="max-height: 200px">
                                                <asp:GridView ID="gvFolderPermissions" runat="server" AutoGenerateColumns="False"
                                                    CellPadding="4" ForeColor="#333333" GridLines="None">
                                                    <AlternatingRowStyle BackColor="White" />
                                                    <Columns>
                                                        <asp:BoundField DataField="RoleName" HeaderText="Role" />
                                                        <asp:TemplateField HeaderText="View">
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="hfRoleId" runat="server" Value='<%# Eval("RoleId") %>' />
                                                                <asp:CheckBox ID="cbRead" runat="server" Checked='<%# Eval("Read") or Eval("Edit") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Edit">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="cbEdit" runat="server" Enabled='<%# Eval("RoleName")<>"Unauthenticated Users" %>'
                                                                    Checked='<%# Eval("Edit") %>' />
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
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnEditFolder" />
        </Triggers>
    </asp:UpdatePanel>
    <div style="margin-top: 12px">
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbEditFolderName"
            Display="static" Text="" ValidationGroup="NewFolder" ErrorMessage="* You must Enter a Folder Name<br />"></asp:RequiredFieldValidator>
        <asp:Button ID="btnEditFolder" runat="server" ResourceKey="Save" CssClass="aButton btn"
            ValidationGroup="EditFolder" />
        <input id="Button3" type="button" value='<%= Translate("btnCancel") %>' onclick="closeEditFolder();"
            class="aButton btn" />
    </div>
</div>
<div id="divEditFile" style="text-align: center">
    <asp:UpdatePanel ID="upEditFile" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlEditFile" runat="server">
                <table cellpadding="4px;">
                    <tr valign="top">
                        <td width="65%">
                            <table width="100%">
                                <tr>
                                    <td>
                                        <dnn:Label ID="Label2" runat="server" ResourceKey="lblDisplayName" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbEditFileName" runat="server" Width="100%" MaxLength="100" Font-Size="X-Large"
                                            CssClass="ui-state-default"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dnn:Label ID="Label4" runat="server" ResourceKey="lblFileDescription" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbEditFileDescription" runat="server" Rows="5" Width="100%" ForeColor="Gray"
                                            Font-Size="Large" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dnn:Label ID="Label5" runat="server" ResourceKey="lblFileAuthor" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbEditFileAuthor" runat="server" MaxLength="100" Width="100%" ForeColor="Gray"
                                            Font-Size="Large"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dnn:Label ID="Label14" runat="server" ResourceKey="lblVersion" />
                                    </td>
                                    <td style="white-space: nowrap;">
                                        <asp:Label ID="tbVersion" runat="server" Width="90px" ForeColor="Gray" Font-Size="Large"></asp:Label>
                                        <asp:HyperLink ID="btnNewversion" onclick="showNewVersion();" runat="server">Upload New Version</asp:HyperLink>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dnn:Label ID="Label8" runat="server" ResourceKey="lblTags" />
                                    </td>
                                    <td>
                                        <asp:CheckBoxList ID="cbTags" runat="server" RepeatColumns="2" Font-Size="XX-Small"
                                            CssClass="cbList">
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dnn:Label ID="Label9" runat="server" ResourceKey="lblKeywords" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbKeywords" runat="server" MaxLength="250" Width="100%" ForeColor="Gray"
                                            Font-Size="Large"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dnn:Label ID="lblTrashed" runat="server" ResourceKey="lblTrashed" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cbTrashed" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <div style="text-align: left;">
                                <div id="accordion" class="accordion">
                                    <div>
                                        <h3>
                                            <a href="#" id="Tab0" class="AcHdr">
                                                <asp:Label ID="Label12" runat="server" Font-Bold="true" ResourceKey="lblVersions"></asp:Label>
                                            </a>
                                        </h3>
                                        <div id="paneVersions" class="AcPane">
                                            <asp:GridView ID="gvFileVersions" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                ForeColor="#333333" GridLines="None">
                                                <AlternatingRowStyle BackColor="White" />
                                                <Columns>
                                                    <asp:BoundField DataField="VersionNumber" HeaderText="Version" />
                                                    <asp:TemplateField HeaderText="DateAdded">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label1" runat="server" Text='<%# GetFileDate(Eval("FileId")) %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# GetFileURL(Eval("DocId"), Eval("FileId")) %>'
                                                                Text="View"></asp:HyperLink>
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
                                        </div>
                                    </div>
                                    <div>
                                        <h3>
                                            <a href="#" id="Tab1" class="AcHdr">
                                                <asp:Label ID="Label6" runat="server" Font-Bold="true" ResourceKey="lblIcon"></asp:Label>
                                            </a>
                                        </h3>
                                        <div id="paneIcon" class="AcPane">
                                            <asp:DataList ID="ddlIcons" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                <ItemTemplate>
                                                    <asp:Image ID="imgIcon" CssClass='<%# IIF( Eval("Selected"), "iconHighlight SelectIcon" , "SelectIcon") %>'
                                                        runat="server" Height="40px" ImageUrl='<%# Eval("Path") & "?FileId=" & Eval("FileId") %>' />
                                                </ItemTemplate>
                                            </asp:DataList>
                                            <div class="SelectIcon" style="display: inline-block; text-align: center; vertical-align: middle;
                                                width: 44px; background-color: #EEE; padding: 10px 0 10px 0; font-size: xx-small">
                                                Default Icon</div>
                                            <div id="addIcon" class="SelectIcon" style="display: inline-block; text-align: center;
                                                vertical-align: middle; width: 44px; background-color: #EEE; padding: 10px 0 10px 0;
                                                font-size: xx-small">
                                                Add Icon</div>
                                        </div>
                                    </div>
                                    <div>
                                        <h3>
                                            <a href="#" id="Tab3" class="AcHdr">
                                                <asp:Label ID="Label7" runat="server" Font-Bold="true" ResourceKey="lblPermissions"></asp:Label>
                                            </a>
                                        </h3>
                                        <div id="panePermissions" class="AcPane">
                                            <div>
                                                <asp:GridView ID="gvPermissions" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                    ForeColor="#333333" GridLines="None">
                                                    <AlternatingRowStyle BackColor="White" />
                                                    <Columns>
                                                        <asp:BoundField DataField="RoleName" HeaderText="Role" />
                                                        <asp:TemplateField HeaderText="View">
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="hfRoleId" runat="server" Value='<%# Eval("RoleId") %>' />
                                                                <asp:CheckBox ID="cbRead" runat="server" Checked='<%# Eval("Read") or Eval("Edit") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Edit">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="cbEdit" runat="server" Enabled='<%# Eval("RoleName")<>"Unauthenticated Users" %>'
                                                                    Checked='<%# Eval("Edit") %>' />
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
                                            </div>
                                        </div>
                                    </div>
                                </div>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:HiddenField ID="hfVersionDocId" runat="server" />
            <asp:HiddenField ID="hfEditFileId" runat="server" />
            
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnEditFile" />
            <asp:PostBackTrigger ControlID="btnSaveNewVersion" />
            <asp:PostBackTrigger ControlID="btnNewIcon" />
        </Triggers>
    </asp:UpdatePanel>
    <div style="margin-top: 12px">
        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="tbEditFileName"
            Display="static" Text="" ValidationGroup="EditFile" ErrorMessage="* You must Enter a Display Name<br />"></asp:RequiredFieldValidator>
        <asp:Button ID="btnEditFile" runat="server" ResourceKey="Save" CssClass="aButton btn" UseSubmitBehavior="true"
            ValidationGroup="EditFile" />
        <input id="Button4" type="button" value='<%= Translate("btnCancel") %>' onclick="closeEditFile();"
            class="aButton btn" />
    </div>
</div>
<div id="FolderMenu" class="contextMenu" style="z-index: 1000">
    <ul>
        <li id="Edit">
            <img src="/icons/sigma/Edit_16X16_Standard.png" />
            Edit</li>
        <li id="Move">
            <img src="/icons/sigma/DragDrop_15x15_Standard.png" />
            Move</li>
        <li id="Delete">
            <img src="/icons/sigma/Delete_16X16_Standard.png" />
            Delete</li>
    </ul>
</div>
<div id="FileReadMenu" class="contextMenu" style="z-index: 1000">
    <ul>
    </ul>
</div>
<div id="FolderRead" class="contextMenu" style="z-index: 1000">
    <ul>
    </ul>
</div>
<div id="BlankMenu" class="contextMenu" style="z-index: 1000;">
    <ul>
        <li id="NewFolder" style="white-space: nowrap;">
            <img src="/icons/sigma/AddFolder_16x16_Standard.png" />
            New Folder...</li>
        <li id="AddFiles" style="white-space: nowrap;">
            <img src="/icons/sigma/AddTab_16x16_Standard.png" />
            Add Files...</li>
    </ul>
</div>
<div id="editbuttons" runat="server" style="height: 20px; float: right;">
    <%--<asp:HyperLink ID="hlFolderButton" runat="server" onclick="showNewFolder();" CssClass="aButton">New Folder</asp:HyperLink>--%>
    <asp:HyperLink ID="hlNewLink" runat="server" onclick="showNewLink();" CssClass="aButton">New Link</asp:HyperLink>
    <asp:HyperLink ID="hlUpload" runat="server" onclick="showUpload();" CssClass="aButton">Upload Files</asp:HyperLink>
    <asp:LinkButton ID="btnSettings" runat="server" CssClass="aButton">Settings</asp:LinkButton>
</div>
<asp:HiddenField ID="hfEditFolderId" runat="server" Value="-1" />
<input type="hidden" value="false" id="movable" />
<asp:HiddenField ID="hfMoveId" runat="server" Value="" />
<asp:HiddenField ID="hfMoveToId" runat="server" Value="" />
<asp:HiddenField ID="hfFileMoveId" runat="server" Value="" />
<asp:HiddenField ID="hfSelectedIcon" runat="server" />
<%--<asp:Label ID="lblDebug" runat="server"></asp:Label>--%>