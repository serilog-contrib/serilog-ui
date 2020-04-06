(function ($) {
    "use strict";

    var fullHeight = function () {
        $('.js-fullheight').css('height', $(window).height());
        $(window).resize(function () {
            $('.js-fullheight').css('height', $(window).height());
        });
    };
    fullHeight();

    $('#sidebarCollapse').on('click', function () {
        $('#sidebar').toggleClass('active');
    });

    $('.page-link').on('click', function (e) {
        e.preventDefault();
        $('#page').val($(this).attr("data-val"));
        $('form').submit()
    });
})(jQuery);