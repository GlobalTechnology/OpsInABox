function initOnClick() {

    /* This enables the click on the whole box to follow the link on the <a> with "button" class. */
    $('.PromotionBoxGamified').click(function () {
        var link = $(this).find('.button');
        window.open(link.attr('href'), link.attr('target') != null ? link.attr('target') : '_self');
        return false;
    });
}

$(document).ready(function () {
    initOnClick();

    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () { initOnClick(); });
});