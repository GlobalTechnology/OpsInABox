//create player div on page
$('body').append("<div id='popdiv'></div>");
$('#popdiv').load("/DesktopModules/AgapeConnect/Stories/controls/popup.html");

//set up video
var tag = document.createElement('script');

tag.src = "https://www.youtube.com/iframe_api";
var firstScriptTag = document.getElementsByTagName('script')[0];
firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);

// creates an <iframe> (and YouTube player) after the API code downloads.
var player;
var currentvidslider;
var currentvidid;
function onYouTubeIframeAPIReady() {
    player = new YT.Player('popplayer', {
        origin: 'https://www.agapefrance.org',
       // height: '432',
        //width: '768',
        playerVars: { 'rel': 0, 'color': 'white' }
    });
}

function popclosevideo() {
    $('#fr_video_popup').fadeOut();
    player.pauseVideo();

    if (document.getElementById('slider' + currentvidslider) === null) { }
    else {      //restart the slider after the video is closed
        setTimeout("$('#slider' + currentvidslider).data('nivoslider').start()", 1000); 
    }
}

function popupvideo(videoid, sliderid) {
    currentvidslider = sliderid;

    if (currentvidid == videoid) { } else { player.cueVideoById(videoid); }
    currentvidid = videoid;
    $('#fr_video_popup').fadeIn();
    $('#fr_video_popup').css("display", "flex");
    
    if (document.getElementById('slider' + sliderid) == null) { }
    else {   //stop the slider while the video is open
        $('#slider' + sliderid).data('nivoslider').stop();
    }
}

$(document).keyup(function (e) {
    if (e.which == 27) {
        popclosevideo();
    }
});