$(document).ready(function () {
    checkCookie();
    $("#popupclose").click(function () {
        $('#my_popup').fadeOut();
        setCookie('sawpopup', true, 1);
    });
    $("#frsignup").click(function () {
        setCookie('sawpopup', true, 1);
    });
});

function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}
function getCookie(cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}
function checkCookie() {
    var sawpopup = getCookie("sawpopup");
    if (sawpopup != 'true') {
        poppit();
    }
}
function poppit() {
    $('#my_popup').delay(5000).fadeIn();
}