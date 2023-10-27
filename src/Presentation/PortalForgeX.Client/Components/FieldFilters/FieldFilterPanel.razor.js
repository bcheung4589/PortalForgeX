export function init() {

    var fieldFilters = $(".field-filters");
    var dataPanelCssClass = fieldFilters.closest(".row").children(":first").next().attr('class');

    fieldFilters.find(".cmd-filters-collapse").click(function (e) {

        fieldFilters.find(".card-header").addClass("d-inline-block").removeClass("bg-dark").removeClass("text-white");
        fieldFilters.find(".card-body").addClass("d-inline-block").addClass("py-0").find("*").hide();
        fieldFilters.find(".field-filter").addClass("d-inline-block").show()
            .find("*").hide();
        fieldFilters.find(".field-filter").find(".cmd-filter-remove").show()
            .find("*").show();
        fieldFilters.find(".card-footer").hide();
        fieldFilters.find(".title").hide();
        fieldFilters.removeClass("card");

        $(this).hide()
            .closest(".row") // row
            .children(":first") // filter panel
            .removeClass("col-lg-3")
            .next() // data panel
            .removeClass(dataPanelCssClass).addClass("col");
    });

    fieldFilters.find(".cmd-filters-display").click(function (e) {

        fieldFilters.find(".card-header").removeClass("d-inline-block").addClass("bg-dark").addClass("text-white");
        fieldFilters.find(".card-body").removeClass("d-inline-block").removeClass("py-0").find("*").show();
        fieldFilters.find(".field-filter").removeClass("d-inline-block");
        fieldFilters.find(".card-footer").show();
        fieldFilters.find(".title").show();
        fieldFilters.addClass("card");
        fieldFilters.find(".cmd-filters-collapse").show();

        $(this)
            .closest(".row") // row
            .children(":first") // filter panel
            .addClass("col-lg-3")
            .next() // data panel
            .removeClass("col")
            .addClass(dataPanelCssClass);
    });
}