export function init() {

    /**
     * Wire up the bootstrap popovers, powered by popper.js
     */
    [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]')).map(function (popoverTriggerEl) {

        var parentDialog = $(popoverTriggerEl).closest(".modal-body");

        return new bootstrap.Popover(popoverTriggerEl, {
            container: parentDialog.length > 0 ? parentDialog[0] : 'body'
        });
    });
}