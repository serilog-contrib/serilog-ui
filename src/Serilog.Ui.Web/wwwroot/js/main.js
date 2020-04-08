(function ($) {
    "use strict";

    var fullHeight = function () {
        $(".js-fullheight").css("height", $(window).height());
        $(window).resize(function () {
            $(".js-fullheight").css("height", $(window).height());
        });
    };
    fullHeight();

    $("#sidebarCollapse").on("click", function () {
        $("#sidebar").toggleClass("active");
    });

    $(".page-link").on("click", function (e) {
        e.preventDefault();
        $("#page").val($(this).attr("data-val"));
        $("form").submit()
    });

    $("#logCount").on("change", function () {
        $("#page").val("1");
        $("form").submit()
    });

    $("#logFilter").on("change", function () {
        $("#page").val("1");
        $("form").submit()
    });

    $("#search").keypress(function (e) {
        $("#page").val("1");
        if (e.which === 13) {
            $("form").submit();
        }
    });
})(jQuery);