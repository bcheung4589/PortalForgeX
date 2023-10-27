export function init() {
    $(document).ready(function () {

        $(document).click(function (e) {

            if (!$(e.target).is('.context-menu')) {
                $('.collapse').collapse('hide');
            }
        });
    });
}