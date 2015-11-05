function initPopupTitle() {

    /* Set the popup title with the page title */
    $("span.ui-dialog-title", parent.document).text(document.title);

}

$(document).ready(function () {
    initPopupTitle();

    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () { initOnClick(); });
});

