export function getOptionValue(datalistId, key) {

    var datalistOption = $("#" + datalistId + " [value='" + key + "']");

    if (datalistOption.length < 1) {
        return;
    }

    var labelAttr = datalistOption.attr("label");
    if (typeof labelAttr !== "undefined") {
        return datalistOption.attr("label");
    }

    return datalistOption.text();

}