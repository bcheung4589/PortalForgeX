/*
* This will run after the page has been rendered.
*/
function pageLoad() {

}

/*
* This will run after a form-page has been rendered.
*/
function formPageLoad() {

    /**
     * CHARAKTER COUNTER
     */
    $(".chars-counter").parent().find(".form-control").keyup(function () {
        var inputField = $(this);
        inputField.parent().find(".chars-counter").text((inputField.attr("maxlength") - inputField.val().length) + " chars left");
    });

    var controlsWithCounter = $(".chars-counter").parent().find(".form-control");
    for (var i = 0; i < controlsWithCounter.length; i++) {
        var field = $(controlsWithCounter[i]);
        field.parent().find(".chars-counter").text((field.attr("maxlength") - field.val().length) + " chars left");
    }
}
