(function ($) {
    "use strict";

    const fullHeight = function () {
        $(".js-fullheight").css("height", $(window).height());
        $(window).resize(function () {
            $(".js-fullheight").css("height", $(window).height());
        });
    };
    fullHeight();

    $(".sidebar-collapse").on("click", function () {
        $("#sidebar").toggleClass("active");
    });

    $(".page-link").on("click", function (e) {
        e.preventDefault();
        $("#page").val($(this).attr("data-val"));
        $("form").submit();
    });

    $("#logCount").on("change", function () {
        $("#page").val("1");
        $("form").submit();
    });

    $("#logFilter").on("change", function () {
        $("#page").val("1");
        $("form").submit();
    });

    $("#search").on("keypress", function (e) {
        $("#page").val("1");
        if (e.which === 13) {
            $("form").submit();
        }
    });

    $(".modal-trigger").on("click", function (e) {
        e.preventDefault();

        const modal = $("#messageModal");
        const modalBody = modal.find('.modal-body');
        const dataType = $(this).attr("data-type");
        let message = $(this).find("span").text();

        if (dataType === "xml") {
            message = formatXml(message, '  ');
            $(modalBody).removeClass('wrapped');
        } else if (dataType === "json") {
            const prop = JSON.parse(message);
            message = JSON.stringify(prop, null, 2);
            $(modalBody).removeClass('wrapped');
        } else {
            $(modalBody).addClass('wrapped');
        }

        modalBody.find('pre').text(message);
        modal.modal("show");
    });

    $("#loginClose").on('click', function () {
        localStorage.setItem("serilogui_token", $("#jwtToken").val());
    });
})(jQuery);

function formatXml(xml, tab) { // tab = optional indent value, default is tab (\t)
    var formatted = '', indent = '';
    tab = tab || '\t';
    xml.split(/>\s*</).forEach(function (node) {
        if (node.match(/^\/\w/)) indent = indent.substring(tab.length); // decrease indent by one 'tab'
        formatted += indent + '<' + node + '>\r\n';
        if (node.match(/^<?\w[^>]*[^\/]$/)) indent += tab;              // increase indent
    });
    return formatted.substring(1, formatted.length - 3);
}