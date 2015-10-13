<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ViewBoxMenu.ascx.vb" Inherits="DotNetNuke.Modules.BoxMenu.ViewBoxMenu" %>
<script src="/js/jquery.jscrollpane.min.js" type="text/javascript"></script>
<link href="/js/jquery.jscrollpane.css" rel="stylesheet" type="text/css" />
<script src="/js/jquery.mousewheel.js" type="text/javascript"></script>
<script src="/js/mwheelIntent.js" type="text/javascript"></script>




<script type="text/javascript">



    (function ($, Sys) {
        function setUpMyTabs() {
            var stop = false;
            

            
            $("#accordion h3").click(function (event) {
                if (stop) {
                    event.stopImmediatePropagation();
                    event.preventDefault();
                    stop = false;
                }
            });
            $("#accordion")
			.accordion({
			    header: "> div > h3",
                navigate: false
              
			});




        }









        $(document).ready(function () {
            setUpMyTabs();
                  

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                setUpMyTabs();



            });
        });








    } (jQuery, window.Sys));

    

</script>





<style type="text/css">
    #demo
    {
        height:420px;
    }
    .scroll-pane
{

	height: 187px;
	width: 100%;
	overflow: auto;
	
}
.WhiteText, .WhiteText p
{
     color: White; 
     font-size: 8pt
}

    .ac_Header
    {
        color: White;
        font-size: 10pt;
        font-weight: bold ;
        width: 105px ;
        height: 24px;
        float: left;
        vertical-align: middle;
        padding-top: 9px;
        text-align:center ;
        
    }
    .ac_SubHeader
    {
        background-color: #F7F7F7;
        color: #CBCBCB ;
        font-size: 9pt;
        font-weight: bold ;
        float: left;
        padding-top: 9px;
        padding-left: 7px;
        height: 24px;
        width: 300px;  /*315*/
    }
    .aLink, .aLink:link, .aLink:visited, .aLink:active 
{
	color: #3E81B5 ;
	 text-decoration: none;
	 
}
    
    aLink:hover 
    {
        text-decoration: none ;
        
        
        border:0;
        border-color: transparent;
        
    }
    
    #aBlueBox
    {
        background: #7aa8e0; /* for non-css3 browsers */
        filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#5d8ac1', endColorstr='#96beee'); /* for IE */
        background: -webkit-gradient(linear, left top, left bottom, from(#5d8ac1), to(#96beee)); /* for webkit browsers */
        background: -moz-linear-gradient(top,  #5d8ac1,  #96beee); /* for firefox 3.6+ */    
    }
    #aRedBox
    {
        background: #9c0303; /* for non-css3 browsers */
        filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#730303', endColorstr='#cb0404'); /* for IE */
        background: -webkit-gradient(linear, left top, left bottom, from(#730303), to(#cb0404)); /* for webkit browsers */
        background: -moz-linear-gradient(top,  #730303,  #cb0404); /* for firefox 3.6+ */    
    }
    #aOrangeBox
    {
        background: #ff9900; /* for non-css3 browsers */
        filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#d78100', endColorstr='#ffc061'); /* for IE */
        background: -webkit-gradient(linear, left top, left bottom, from(#d78100), to(#ffc061)); /* for webkit browsers */
        background: -moz-linear-gradient(top,  #d78100,  #ffc061); /* for firefox 3.6+ */    
    }
    #aGreenBox
    {
        background: #99cc66; /* for non-css3 browsers */
        filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#74a542', endColorstr='#b6e488'); /* for IE */
        background: -webkit-gradient(linear, left top, left bottom, from(#74a542), to(#b6e488)); /* for webkit browsers */
        background: -moz-linear-gradient(top,  #74a542,  #b6e488); /* for firefox 3.6+ */    
    }
    #aGrayBox
    {
        background: #aeaeae; /* for non-css3 browsers */
        filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#919191', endColorstr='#c9c9c9'); /* for IE */
        background: -webkit-gradient(linear, left top, left bottom, from(#919191), to(#c9c9c9)); /* for webkit browsers */
        background: -moz-linear-gradient(top,  #919191,  #c9c9c9); /* for firefox 3.6+ */    
       
    }
    
    .ac_BlueShelf
    {
       background-image: url(/DesktopModules/AgapeEurope/BoxMenu/images/BlueFace.jpg);   
       background-repeat:no-repeat ;
       background-position: left top ;
       
       margin: 0;
       padding: 0 ;
    }
    
    .ac_RedShelf
    {
       background-image: url(/DesktopModules/AgapeEurope/BoxMenu/images/RedFace.jpg);   
       background-repeat:no-repeat ;
       background-position: left top ;
       
       margin: 0;
       padding: 0 ;
    }
    .ac_OrangeShelf
    {
       background-image: url(/DesktopModules/AgapeEurope/BoxMenu/images/OrangeFace.jpg);   
       background-repeat:no-repeat ;
       background-position: left top ;
       
       margin: 0;
       padding: 0 ;
    }
    .ac_GreenShelf
    {
       background-image: url(/DesktopModules/AgapeEurope/BoxMenu/images/GreenFace.jpg);   
       background-repeat:no-repeat ;
       background-position: left top ;
       
       margin: 0;
       padding: 0 ;
    }
    .ac_GreyShelf
    {
       background-image: url(/DesktopModules/AgapeEurope/BoxMenu/images/GreyFace.jpg);   
       background-repeat:no-repeat ;
       background-position: left top ;
       
       margin: 0;
       padding: 0 ;
    }
    
    .ac_ShelfText
    {
        padding: 26px 13px 26px 190px;
        
         height: 205px ;
     }
     .BlueTitle {font-size: 11pt; color: #5d8ac1;}
     .RedTitle {font-size: 11pt; color: #730303;}
     .OrangeTitle {font-size: 11pt; color: #d78100;}
     .GreenTitle {font-size: 11pt; color: #74a542;}
     .GreyTitle {font-size: 11pt; color: #919191;}
     
</style>
<div class="demo">
    <div id="accordion">
        <div>
            <h3 style="margin: 0;">
                <a href="#" id="Tab0" class="aLink">
                <div>
                    <div class="ac_Header" id="aBlueBox">
                        <asp:Label ID="lblTitle1" runat="server" resourcekey="Title1"></asp:Label>
                    </div>
                    <div class="ac_SubHeader">
                        <asp:Label ID="lblSubTitle1" runat="server" resourcekey="SubTitle1"></asp:Label>
                    </div>
                    <div style="clear: both"></div>
                    </div>
                </a>
            </h3>
            <div class="ac_BlueShelf">
                <div class="ac_ShelfText">
                    <div class="BlueTitle" >
                        <asp:Label ID="lblShelfTitle1" runat="server" Font-Bold="True" resourcekey="ShelfTitle1"></asp:Label>
                    </div>
                    <div class="scroll-pane" >
                        <asp:Label ID="lblShelf1" runat="server"  CssClass="WhiteText" resourcekey="Shelf1"></asp:Label>
                    </div>

                </div>
            </div>
        </div>
        <div>
            <h3 style="margin: 0;">
                <a href="#"  id="Tab1" class="aLink">
                <div>
                    <div class="ac_Header" id="aRedBox">
                        <asp:Label ID="lblTitle2" runat="server" resourcekey="Title2"></asp:Label>
                    </div>
                    <div class="ac_SubHeader">
                        <asp:Label ID="lblSubTitle2" runat="server" resourcekey="SubTitle2"></asp:Label>
                    </div>
                     <div style="clear: both"></div>
                   </div>
                </a>
            </h3>
            <div class="ac_RedShelf">
                <div class="ac_ShelfText">
                    <div  class="RedTitle">
                        <asp:Label ID="lblShelfTitle2" runat="server" Font-Bold="True" resourcekey="ShelfTitle2"></asp:Label>
                    </div>
                    <div class="scroll-pane">
                        <asp:Label ID="lblShelf2" runat="server"   CssClass="WhiteText"  resourcekey="Shelf2"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
        <div>
            <h3 style="margin: 0;">
                <a href="#"  id="Tab2" class="aLink">
                <div>
                    <div class="ac_Header" id="aOrangeBox">
                        <asp:Label ID="lblTitle3" runat="server" resourcekey="Title3"></asp:Label>
                    </div>
                    <div class="ac_SubHeader">
                        <asp:Label ID="lblSubTitle3" runat="server" resourcekey="SubTitle3"></asp:Label>
                    </div>
                     <div style="clear: both"></div>
                  </div>
                </a>
            </h3>
            <div class="ac_OrangeShelf">
                <div class="ac_ShelfText">
                    <div  class="OrangeTitle">
                        <asp:Label ID="lblShelfTitle3" runat="server" Font-Bold="True" resourcekey="ShelfTitle3"></asp:Label>
                    </div>
                    <div class="scroll-pane" >
                        <asp:Label ID="lblShelf3" runat="server"  CssClass="WhiteText"  resourcekey="Shelf3"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
        <div>
            <h3 style="margin: 0;">
                <a href="#"  id="Tab3" class="aLink">
                <div>
                    <div class="ac_Header" id="aGreenBox">
                        <asp:Label ID="lblTitle4" runat="server" resourcekey="Title4"></asp:Label>
                    </div>
                    <div class="ac_SubHeader">
                        <asp:Label ID="lblSubTitle4" runat="server" resourcekey="SubTitle4"></asp:Label>
                    </div>
                     <div style="clear: both"></div>
                    </div>
                </a>
            </h3>
            <div class="ac_GreenShelf">
                <div class="ac_ShelfText">
                    <div  class="GreenTitle">
                        <asp:Label ID="lblShelfTitle4" runat="server" Font-Bold="True" resourcekey="ShelfTitle4"></asp:Label>
                    </div>
                    <div class="scroll-pane" >
                        <asp:Label ID="lblShelf4" runat="server"  CssClass="WhiteText"  resourcekey="Shelf4"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
        <div>
            <h3 style="margin: 0;">
                <a href="#"  id="Tab4" class="aLink">
                <div>
                    <div class="ac_Header" id="aGrayBox">
                        <asp:Label ID="lblTitle5" runat="server" resourcekey="Title5"></asp:Label>
                    </div>
                    <div class="ac_SubHeader">
                        <asp:Label ID="lblSubTitle5" runat="server" resourcekey="SubTitle5"></asp:Label>
                    </div>
                     <div style="clear: both"></div>
                    </div>
                </a>
            </h3>
            <div class="ac_GreyShelf">
                <div class="ac_ShelfText">
                    <div  class="GreyTitle">
                        <asp:Label ID="lblShelfTitle5" runat="server" Font-Bold="True" resourcekey="ShelfTitle5"></asp:Label>
                    </div>
                    <div class="scroll-pane" >
                        <asp:Label ID="lblShelf5" runat="server"  CssClass="WhiteText" resourcekey="Shelf5"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<!-- End demo -->
