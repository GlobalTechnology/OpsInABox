<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ListFullWidth-Fr.ascx.vb" Inherits="DotNetNuke.Modules.AgapeConnect.Stories.ListFullWidth_Fr" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<dnn:DnnJsInclude runat="server" FilePath="/DesktopModules/AgapeConnect/Stories/js/videopopup.js" />
<dnn:DnnCssInclude runat="server" FilePath="/DesktopModules/AgapeConnect/Stories/themes/default/france.css" />

<script type="text/javascript">

   function registerClick(c)
   {
        $.ajax({ type: 'POST', url: "<%= NavigateURL() %>",
                        data: ({ StoryLink: c })
                    });
   }
</script>

<div id="ListFullWidth">
<asp:Repeater runat="server" ID="dlStories">
    <ItemTemplate>
        <asp:HyperLink ID="hlStory" runat="server" href='<%# Eval(ControlerConstants.URL) %>' OnClick='<%# Eval(ControlerConstants.OPENLINK) %>'>
            <h4><asp:Label ID="lblStoryTitle" runat="server"  Text='<%# Eval(ControlerConstants.HEADLINE) %>' class="storyTitle" /></h4>
            <div class="frresourceimg">
                <asp:Image ID="StoryThumbnail" runat="server" CssClass="thumbnail" ImageUrl='<%# Eval(ControlerConstants.LINKIMAGE) %>' />
                <asp:Image ID="playbutton" visible='<%# Eval(ControlerConstants.LINKIMAGECSS) %>' runat="server" CssClass="playButton" ImageUrl='/DesktopModules/AgapeConnect/Stories/themes/default/playAspect2.png' />
            </div>
            <p><asp:Label ID="lblStoryPreview" runat="server" Text='<%# Eval(ControlerConstants.DESCRIPTION) %>' class="storyPreview" /></p>
        </asp:HyperLink>
    </ItemTemplate>
</asp:Repeater>
</div>

<div class="afpagebuttons">
    <asp:Hyperlink ID="btnPrev" runat="server" Text="Previous" CssClass="button" resourceKey="btnPrevious" Visible="false" Width="80px"/>
    <asp:Hyperlink ID="btnNext" runat="server" Text="Next" CssClass="button" resourceKey="btnNext"  Visible="false" Width="80px"/>
</div>
