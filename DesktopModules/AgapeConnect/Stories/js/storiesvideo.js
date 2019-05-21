function popclosevideo() {
    $('.fr_video_popup').fadeOut();
    //player.pauseVideo();
    $('.ytopen iframe')[0].contentWindow.postMessage('{"event":"command","func":"' + 'pauseVideo' + '","args":""}', '*');
    $('.ytopen').removeClass('ytopen')
    if (document.getElementById('slider' + currentvidslider) === null) { }
    else {      //restart the slider after the video is closed
        setTimeout("$('#slider' + currentvidslider).data('nivoslider').start()", 1000);
    }
}

function popupvideo(videoid, sliderid) {
    currentvidslider = sliderid;

    currentvidid = videoid;
    $('#' + videoid).fadeIn();
    $('#' + videoid).css("display", "flex");
    $('#' + videoid).addClass("ytopen");

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