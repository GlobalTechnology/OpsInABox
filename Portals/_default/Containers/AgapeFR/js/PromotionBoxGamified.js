function initOnClick() {
    $('.PromotionBoxGamified').click(function () {
        document.location = $(this).find('.button').attr('href');
        return false;
    });
}

$(document).ready(function () {
    initOnClick();

    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () { initOnClick(); });
});