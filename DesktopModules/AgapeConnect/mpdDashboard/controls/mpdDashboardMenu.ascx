<%@ Control Language="VB" AutoEventWireup="false" CodeFile="mpdDashboardMenu.ascx.vb" Inherits="DesktopModules_AgapeConnect_mpdCalc_controls_mpdAdmin" %>
 <div class="navbar">
        <div class="navbar-inner">
            <asp:HyperLink ID="hlTitle" runat="server" class="brand">MPD Dashboard</asp:HyperLink>
           
            <ul class="nav">
                <li  id="menuCountries" runat="server"  ><a href="<%= NavigateURL()%>">Countries</a></li>
                <li  id="menuTeam" runat="server"  ><a href="<%= CountryURL & "?country=team"%>">Team</a></li>
               
                <li  id="menuStaff" runat="server"  class="dropdown">
                    <a  href="#" class="dropdown-toggle" data-toggle="dropdown">Your Staff Account <b class="caret"></b></a>
                    <ul class="dropdown-menu" role="menu">
                          <asp:Repeater ID="rpUserAccts" runat="server">
                            <ItemTemplate>
                                <li>
                                    <asp:HyperLink ID="hlUser" runat="server" NavigateUrl='<%# StaffUrl & "?mpd_user_id=" & Eval("AP_mpd_UserId")%>' Text='<%# "...in " & Eval("name")%>' />

                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                        
                    </ul>
                </li>
            </ul>
        </div>
    </div>



 
