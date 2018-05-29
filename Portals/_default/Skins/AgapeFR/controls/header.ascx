<%@ Control Language="vb" AutoEventWireup="false" Explicit="True" %>
<%@ Register TagPrefix="dnn" TagName="USER" Src="~/Admin/Skins/User.ascx" %>
<%@ Register TagPrefix="dnn" TagName="LOGIN" Src="~/Admin/Skins/Login.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SEARCH" Src="~/DesktopModules/AgapeFR/Search/Search.ascx" %>
<%@ Register TagPrefix="ddr" Namespace="DotNetNuke.Web.DDRMenu.TemplateEngine" Assembly="DotNetNuke.Web.DDRMenu" %>
<%@ Register TagPrefix="ddr" TagName="MENU" src="~/DesktopModules/DDRMenu/Menu.ascx" %>
<meta name="theme-color" content="#0e71b4">
<meta name="viewport" content="width=device-width, initial-scale=1.0">
<script runat="server">
    Protected Function Translate(ResourceKey As String) As String
        Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
        Dim strFile As String = System.IO.Path.GetFileName(Server.MapPath(PS.ActiveTab.SkinSrc))
        strFile = PS.ActiveTab.SkinPath + Localization.LocalResourceDirectory + "/" + strFile
        Return Localization.GetString(ResourceKey, strFile)
    End Function

</script>
<script src="/js/jquery.watermarkinput.js" type="text/javascript"></script>
<script type="text/javascript">
    (function ($, Sys) {
        function initSearchText() {
            $('#SearchContainer span input[type=search]').Watermark('<%=Translate("SearchWatermark.Text")%>');
        }
        $(document).ready(function () {
            initSearchText();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                initSearchText();
            });
        });
    }(jQuery, window.Sys));
</script>
<div id="mySidenav" class="sidenav">
    <ddr:MENU ID="MENU1" MenuStyle="/templates/AgapeFRMenu/" NodeSelector="*,0,+1" runat="server" includehidden="false" />
    <% If UserController.Instance.GetCurrentUserInfo().UserID > 0  %>
        <ul id="usercontainer" runat="server">
            <li class="parent">
                <a href="#" id="UserContainer" class="parent"><%=UserController.Instance.GetCurrentUserInfo().DisplayName%></a>
                <span class="menudrop"></span>
                <ul>
                    <li><dnn:LOGIN runat="server" ID="dnnLOGIN" CssClass="user" /></li>
                </ul>
            </li>
        </ul>
    <% End If %>
</div>

<div id="controlPanelContainer">
    <div id="ControlPanel" runat="server" />
</div>
<div id="bar1" class="centeredbox bar menuclosed">
    <div class="menuoverlay"></div>
    <div id="logoHeader">
        <a href="/">
            <svg id="aflogo" xmlns:rdf="http://www.w3.org/1999/02/22-rdf-syntax-ns#" xmlns="http://www.w3.org/2000/svg" y="0px" xml:space="preserve" height="112" viewBox="0 0 567.544 266.545" version="1.1" enable-background="new 0 0 841.89 595.275" x="0px" xmlns:cc="http://creativecommons.org/ns#" xmlns:dc="http://purl.org/dc/elements/1.1/"><metadata id="metadata290"><rdf:RDF><cc:Work rdf:about=""><dc:format>image/svg+xml</dc:format><dc:type rdf:resource="http://purl.org/dc/dcmitype/StillImage"/><dc:title/></cc:Work></rdf:RDF></metadata>
                <path id="path236" class="aflightblue" fill="#b2dbed" d="m142.91 222.63c-42.82-20.81-112.69 2.9-142.62-67.46-3.8339-27.14 31.475-63.672 64.86-56.3-5.967 0.566-12.109 1.679-19.037 5.008 6.208 3.538 13.444-2.743 22.149-2.517 10.74 0.28 29.042 8.459 24.314 10.331-20.351-10.273-76.754 9.757-70.905 39.343 2.808 14.204 21.333 31.101 28.634 36.011 26.458 17.814 73.727 11.666 92.601 35.584z" fill-rule="evenodd" clip-rule="evenodd"/>
                <g id="g238" class="afdarkblue" fill="#0e71b4" transform="translate(-123.45 -135.81)">
                <path id="path240" d="m293.71 368.2c-2.902 0.583-3.952-2.963-0.988-3.498 10.437-1.895 21.243-2.138 25.751-2.138 1.914 0 1.914 2.235 0 2.332-8.275 0.389-16.118 1.507-24.763 3.304zm4.693 14.624c-1.235 6.947-2.532 14.089-1.111 15.596 1.791 1.991-2.655 3.595-4.384 1.749-2.285-2.43-0.494-9.911 1.235-17.005-1.358-0.632-1.482-2.526 0.556-2.915l0.124-0.049c1.112-4.469 1.915-8.453 1.359-10.348-0.556-1.798 2.779-2.43 3.334-0.681 0.618 1.943 0.124 5.976-0.618 10.446 5.125-0.584 12.166-1.118 15.068-1.118 2.038 0 2.038 2.43 0 2.43-3.027 0.001-10.746 1.069-15.563 1.895z"/>
                <path id="path242" d="m341.13 380.98c-2.038 0.098-4.755 0.438-7.225 0.875-8.275 1.457-8.769 4.615-10.127 10.349 0.185 3.644 0.124 7.044-0.494 8.405-0.494 1.068-1.42 1.359-2.162 1.359-3.396 0-1.111-7.919-0.926-8.987l0.309-1.604c-0.186-4.227-0.679-8.502-0.926-9.717-0.494-1.895 3.211-2.332 3.396-0.34l0.185 2.089c1.667-2.138 4.57-3.547 10.065-4.178 2.902-0.389 6.175-0.195 7.966 0 2.348 0.243 2.286 1.555-0.061 1.749z"/>
                <path id="path244" d="m351.81 400.7c-5.805 0-10.127-3.401-10.127-7.773 0-7.336 11.548-14.77 17.291-14.77 5.311 0 7.163 2.964 7.163 5.49 0 1.943-3.829 1.991-3.829-0.049 0-1.458-0.371-2.721-3.334-2.721-3.52 0-12.968 6.268-12.968 12.049 0 3.498 2.594 5.053 5.805 5.053 4.323 0 11.671-2.964 14.635-16.228 0.433-1.846 3.582-1.555 3.396 0.341-0.185 2.138-0.556 13.021 0.741 14.526 1.853 1.749-2.594 4.373-3.891 0.583-0.556-1.651-0.494-3.595-0.556-5.296-3.889 6.414-9.448 8.795-14.326 8.795z"/>
                <path id="path246" d="m419.56 402.35c-5.619 0-5.928-3.255-6.792-11.806-0.494-4.712-0.741-8.453-5.434-8.453-4.755 0-9.263 3.838-13.277 8.453-4.261 5.005-7.904 10.154-10.93 10.154-3.952 0-4.693-2.77-5.064-5.247-0.309-2.672-1.173-6.705-1.482-12-0.124-2.381 3.767-2.479 4.076-0.146 0.124 0.973 0.185 7.58 0.865 11.563 0.37 2.478 0.556 3.158 1.42 3.158 1.297 0 7.657-8.065 8.522-9.037 4.076-4.615 9.572-10.3 15.871-10.3 7.348 0 9.139 4.13 9.942 11.515 0.556 5.149 0.247 9.183 2.717 9.183 1.42 0 2.902-0.292 4.508-1.069 1.976-1.312 3.767-0.437 2.778 0.826-1.236 1.505-3.891 3.206-7.72 3.206z"/>
                <path id="path248" d="m444.32 400.7c-7.534 0-14.944-1.992-14.944-6.753 0-5.296 12.845-14.381 20.688-14.381 3.211 0 6.052 1.117 6.545 3.352 0.433 1.75-2.902 2.332-3.396 0.535-0.246-0.535-2.531-0.583-3.148-0.583-6.114 0-16.488 7.87-16.488 11.077 0 1.604 2.717 3.983 10.745 3.983 2.841 0 9.016-0.583 13.523-1.555 2.224-0.485 3.026 1.458 0.864 2.089-4.632 1.362-11.116 2.236-14.389 2.236z"/>
                <path id="path250" d="m476.43 400.75c-5.742 0-12.104-1.507-12.104-7.53 0-1.652 0.494-3.45 1.421-5.15-1.482-0.972-0.433-2.381 1.297-2.041 3.211-4.421 8.892-8.064 14.696-8.064 3.521 0 8.77 1.166 8.77 4.762 0 5.053-8.831 7.87-14.203 7.87-2.1 0-4.508-0.34-6.669-0.972-0.68 1.264-0.988 2.478-0.988 3.596 0 3.498 3.396 4.81 7.78 4.81 5.126 0 10.251-1.7 13.895-3.79 1.914-1.068 3.458 0.194 1.667 1.313-3.397 2.183-8.337 5.196-15.562 5.196zm5.31-19.677c-4.014 0-8.15 2.915-10.683 6.268 1.729 0.34 3.582 0.583 5.249 0.583 3.335 0 10.807-1.943 10.807-5.198 0-0.827-1.482-1.653-5.373-1.653z"/>
                </g>
                <path id="path252" class="afdarkblue" fill="#0e71b4" d="m156.55 228.49c-33.724-25.69-99.691-17.632-112.71-83.957 1.772-24.207 39.752-49.561 67.721-37.198-5.348-0.581-10.971-0.713-17.689 0.932 4.788 4.177 12.344 0.026 19.962 1.781 9.391 2.162 23.942 12.524 19.432 13.298-15.965-12.54-69.376-5.262-69.826 21.426-0.22 12.812 12.881 30.77 18.38 36.326 19.901 20.167 62.653 23.292 74.733 47.392z" fill-rule="evenodd" clip-rule="evenodd"/>
                <g id="g254" transform="translate(-123.45 -135.81)">
                <path id="path256" class="afgray" fill="#868889" d="m313.36 155.67c13.211 32.98 20.419 56.565 21.626 70.754 5.396 33.903 3.056 62.673-7.02 86.312-9.445 22.159-23.765 38.442-42.962 48.855l-0.74-0.315 0.36-0.847 1.057-0.426-5.609-2.391 1.037-2.435 0.74 0.315c2.835-3.129 5.249-4.977 7.242-5.544l1.101-0.531-2.454 3.707-0.361 0.847c0.847 0.361 2.717-1.093 5.611-4.362l0.316-0.741-0.741-0.316-1.162 0.381c8.653-9.738 14.587-18.379 17.806-25.931l2.988-9.358 2.245-2.92c2.95-10.832 4.037-16.416 3.261-16.746 3.628-19.467 4.955-30.115 3.984-31.947-0.297-33.146-1.882-54.96-4.753-65.439-4.489-19.006-7.305-28.836-8.444-29.489-6.25-18.006-10.82-27.626-13.715-28.86l1.037-2.435 5.23 1.229 0.847 0.361 0.485 1.207 2.796 0.191 8.192 16.874zm5.59 151.22-0.361 0.848-1.589 6.076 0.741 0.316 1.399-3.281 0.657-3.598-0.847-0.361z"/>
                </g>
                <g id="g258" transform="translate(-123.45 -135.81)">
                <path id="path260" class="afdarkblue" fill="#0e71b4" d="m392.79 231.91 0.816 2.347 0.915 0.42 0.447 0.715 2.101 4.399-1.346 0.842c-1.526-2.44-8.99-6.383-22.389-11.828-0.712-0.962-7.879-3.435-21.5-7.414-7.609-2.526-22.361-4.102-44.256-4.727-1.446-0.833-8.127 0.159-20.046 2.98-0.41-0.655-3.817 0.193-10.219 2.543l-1.368 1.851-5.422 2.398c-4.176 2.613-8.398 7.492-12.67 14.637l-0.037-0.97-0.392-0.625-0.409 0.256c-1.437 2.39-1.933 3.942-1.485 4.656l0.468-0.293 1.833-2.017-0.077 0.918c0.119 1.665-0.502 3.667-1.861 6.009l0.391 0.625-1.346 0.842-2.96-4.731-0.019 0.881-0.468 0.293-0.391-0.625c2.111-15.978 9.292-27.8 21.545-35.468 13.072-8.18 31.433-9.818 55.086-4.909 9.639 1.173 26.947 7.484 51.923 18.934l13.136 7.061zm-114.52-14.174-2.206 0.51-1.814 1.136 0.391 0.625 3.608-1.263 0.468-0.293-0.447-0.715z"/>
                </g>
                <g id="g262" transform="translate(-123.45 -135.81)">
                <path id="path264" class="afmedblue" fill="#66b8dc" d="m644.47 267.32-0.881-0.566-0.413 0.099-0.334-0.121-1.842-0.913 0.228-0.63c1.142 0.412 4.644-0.221 10.507-1.898 0.485 0.134 3.508-0.91 9.063-3.132 3.183-1.141 8.488-4.456 15.915-9.943 0.696-0.1 2.656-2.167 5.877-6.203 0.307 0.11 1.217-1.057 2.729-3.502l-0.027-0.969 1.176-2.207c0.706-1.954 0.839-4.67 0.397-8.153l0.265 0.313 0.292 0.105 0.069-0.191c-0.145-1.167-0.384-1.811-0.719-1.932l-0.079 0.219-0.084 1.146-0.213-0.324c-0.473-0.522-0.788-1.348-0.945-2.478l-0.291-0.106 0.228-0.63 2.212 0.8-0.223-0.297 0.078-0.219 0.293 0.105c3.456 5.851 4.148 11.642 2.076 17.374-2.209 6.114-7.876 11.434-17.002 15.958-3.504 2.118-10.889 4.525-22.154 7.223l-6.198 1.072zm41.687-25.083 0.6-0.743 0.308-0.849-0.293-0.106-0.869 1.358-0.079 0.219 0.333 0.121z"/>
                </g>
                <path id="path270" class="afblack" d="m223.31 144.29 1.2843-3.9336 3.6858-0.004 40.002 87.272-6.8942-0.14844-13.63-30.197-42.371 0.21485 1.935-5.7294 37.859-0.001"/>
                <path id="path272" class="afblack" d="m345.11 154.69c-10.54-5.973-20.319-8.769-30.729-8.769-21.972 0-38.229 16.262-38.229 38.237 0 19.44 12.827 38.24 40.129 38.24 10.036 0 16.893-1.901 23.751-5.338v-23.37h-18.288v-5.593h25.015v33.157c-10.282 4.954-20.696 7.365-30.732 7.365-26.92 0-47.114-19.056-47.114-44.713 0-25.406 19.686-44.209 46.481-44.209 9.782 0 17.529 1.907 29.717 7.368v7.625z"/>
                <path id="path274" class="afblack" d="m438.09 227.47h-6.983l-34.858-77.047 0.004-0.006-34.87 77.052h-6.986l40.002-87.272h3.689z"/>
                <path id="path276" class="afblack" d="m472.39 140.84c20.318 0 28.319 10.417 28.319 22.867 0 11.177-6.476 23.244-28.952 23.244h-16.513v40.525h-6.73v-86.636h23.876zm-17.146 40.52h16.129c16.13 0 22.095-7.745 22.095-17.527 0-9.401-6.09-17.403-21.332-17.403h-16.892v34.93z"/>
                <path id="path278" class="afblack" d="m560.78 146.43h-40.511v34.676h39.497v5.59h-39.497v35.188h41.529v5.592h-48.263v-86.636h47.244v5.59z"/>
                <path id="path284" class="afmedblue" fill="#66b8dc" d="m404.66 190.82c0 4.22-3.414 7.631-7.627 7.631-4.211 0-7.624-3.411-7.624-7.631 0-4.204 3.413-7.624 7.624-7.624 4.213 0 7.627 3.42 7.627 7.624z"/>
            </svg>
        </a>
    </div>
    <div id="menu">
        <div id="featuredlinks">
            <span><a href="http://dons.agapefrance.org">Faire un Don</a> | <a href="https://agapemedia.fr" target="_blank">Boutique</a></span>
        </div>
        <div id="menubuttons">
            <svg class="searchicon" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="50px" version="1.1" height="50px" viewBox="0 0 64 64" enable-background="new 0 0 64 64">
                <g>
                <path class="stripe" d="M25.915,52.205c6.377,0,12.206-2.344,16.708-6.196L59.98,63.365c0.392,0.391,0.904,0.587,1.418,0.587   c0.513,0,1.025-0.196,1.418-0.587c0.392-0.393,0.588-0.904,0.588-1.418s-0.196-1.027-0.588-1.419L45.459,43.172   c3.853-4.5,6.197-10.331,6.197-16.707c0-14.194-11.549-25.741-25.741-25.741c-14.194,0-25.742,11.547-25.742,25.741   C0.173,40.658,11.721,52.205,25.915,52.205z M25.915,4.735c11.98,0,21.729,9.747,21.729,21.729c0,11.98-9.749,21.729-21.729,21.729   c-11.981,0-21.73-9.748-21.73-21.729C4.185,14.482,13.934,4.735,25.915,4.735z"/>
                </g>
            </svg>
            <svg class="menubtn" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="50px" version="1.1" height="50px" viewBox="0 0 64 64" enable-background="new 0 0 64 64">
                <g>
                    <path class="stripe" d="M2.252,10.271h58.871c1.124,0,2.034-0.91,2.034-2.034c0-1.123-0.91-2.034-2.034-2.034H2.252    c-1.124,0-2.034,0.911-2.034,2.034C0.218,9.36,1.128,10.271,2.252,10.271z"/>
                    <path class="stripe" d="m61.123,30.015h-58.871c-1.124,0-2.034,0.912-2.034,2.035 0,1.122 0.91,2.034 2.034,2.034h58.871c1.124,0 2.034-0.912 2.034-2.034-7.10543e-15-1.123-0.91-2.035-2.034-2.035z"/>
                    <path class="stripe" d="m61.123,53.876h-58.871c-1.124,0-2.034,0.91-2.034,2.034 0,1.123 0.91,2.034 2.034,2.034h58.871c1.124,0 2.034-0.911 2.034-2.034-7.10543e-15-1.124-0.91-2.034-2.034-2.034z"/>
                </g>
            </svg>
        </div>
    </div>
    <div id="SearchContainer">
        <dnn:SEARCH runat="server" ID="dnnSEARCH" UseDropDownList="False" ShowWeb="False" ShowSite="False" Submit="<div id=&quot;SearchSubmit&quot;></div>" />
    </div>
</div>
<script>
    $(document).ready(function () {
        $("li.menuopen ul").slideDown();
    });
    function menutoggle() { //hide and show nav menu
        $("#mySidenav").toggleClass("opensidenav");
        $("#bar1").toggleClass("menuopen");
        $("#bar1").toggleClass("menuclosed");
        $("#logoHeader").toggleClass("white");
    }
    
    $(".menubtn").click(function() { 
        menutoggle();
    });

    $(".menuoverlay").click(function () {
        menutoggle();
    });

    $(".parent .offlink, a#UserContainer").click(function () { //hide and show second level menu for parent link click
        $(this).nextAll("ul").eq(0).slideToggle();
        $(".parent ul").not($(this).nextAll('ul').eq(0)).slideUp();
        $(this).parent().toggleClass("menuopen");
        $("li.parent").not($(this).parent()).removeClass("menuopen");
    });

    $(".searchicon").click(function () { //hide and show search field
        $("#SearchContainer, #mySidenav").toggleClass("opensearch");
        $(".opensearch input").focus();
    });
    
</script>