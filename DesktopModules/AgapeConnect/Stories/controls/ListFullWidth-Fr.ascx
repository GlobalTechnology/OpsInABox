<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ListFullWidth-Fr.ascx.vb" Inherits="DotNetNuke.Modules.AgapeConnect.Stories.ListFullWidth_Fr" %>
<link href="/DesktopModules/AgapeConnect/Stories/themes/default/default.css" rel="stylesheet" type="text/css" media="screen" />

<script type="text/javascript">

   function registerClick(c)
   {
        $.ajax({ type: 'POST', url: "<%= NavigateURL() %>",
                        data: ({ StoryLink: c })
                    });
   }
</script>

<div id="ListFullWidth">
<asp:DataList runat="server" ID="dlStories" AllowPaging="true" Width="100%">
    <ItemTemplate>
        <asp:HyperLink ID="hlStory" runat="server"
            NavigateUrl='<%# Eval(ControlerConstants.LINK) %>'>
            <div class="items" runat="server">
                <div class="item">
                    <h4><asp:Label ID="lblStoryTitle" runat="server"  Text='<%# Eval(ControlerConstants.HEADLINE) %>' class="storyTitle" /></h4>
                    <p><asp:Label ID="lblStoryPreview" runat="server" Text='<%# Eval(ControlerConstants.DESCRIPTION) %>' class="storyPreview" /></p>
                </div>
                <div class="item">
                    <asp:Image ID="StoryThumbnail" runat="server" CssClass="thumbnail" ImageUrl='<%# Eval(ControlerConstants.LINKIMAGE) %>' />
                </div>
            </div>
        </asp:HyperLink>
    </ItemTemplate>
</asp:DataList>
</div>

<div style="width: 100%; text-align: center; margin-top: 20px;">
    <asp:Hyperlink ID="btnPrev" runat="server" Text="Previous" CssClass="button" resourceKey="btnPrevious" Visible="false" Width="80px"/>
    <asp:Hyperlink ID="btnNext" runat="server" Text="Next" CssClass="button" resourceKey="btnNext"  Visible="false" Width="80px"/>
</div>
<div style="clear: both;"></div>
<asp:Literal ID="ltPagination" runat="server"></asp:Literal>
