import * as $ from 'jquery';
window.$ = $;
window.jQuery = $;
import 'popper.js';
import 'bootstrap';
import netStack = require('netstack.js');
import { initTokenUi } from './authentication';
import { formatXml } from './util';
import { fetchLogs } from './fetch';
import { changePageByModalChoice } from './pagination';

const initEventListeners = () => {
    // print current year
    const date = new Date().getFullYear();
    document.getElementById('currentYearPrint').append(date.toString());
    // set full height
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

    // change page handler
    $("body").on("click", ".page-link-item", function (e) {
        e.preventDefault();
        $("#page").val($(this).attr("data-val"));
        fetchLogs();
    });

    // reset choosen page handlers
    $("#logCount").on("change", function () {
        $("#page").val("1");
        fetchLogs();
    });
    $("#logFilter").on("change", function () {
        $("#page").val("1");
        fetchLogs();
    });
    $("#submit").on("click", function () {
        $("#page").val("1");
        fetchLogs();
    });

    // open an objectDetails modal
    $("body").on("click", ".modal-trigger", function (e) {
        e.preventDefault();

        const modal = $("#messageModal");
        const modalBody = modal.find(".modal-body");
        const dataType = $(this).attr("data-type");
        let message = $(this).find("span").text();

        if (dataType === "xml") {
            message = $(this).find("span").html();
            message = formatXml(message, "  ");
            $(modalBody).removeClass("wrapped");
        } else if (dataType === "json") {
            const prop = JSON.parse(message);
            message = JSON.stringify(prop, null, 2);
            $(modalBody).removeClass("wrapped");
        } else {
            $(modalBody).addClass("wrapped");
        }

        modalBody.find("pre").text(message);
        modal.modal("show");
        $('.stacktrace').netStack({
            prettyprint: true
        });
    });

    const modalButton = document.querySelector('.custom-pagination-submit');
    modalButton.addEventListener('click', changePageByModalChoice);

    // on jwtSet
    $("#saveJwt").on("click", function () {
        const isJwtSaved = $(this).data("saved");
        if (isJwtSaved.toString() === "false") {
            const token = $<HTMLInputElement>("#jwtToken").val() as string;
            if (!token) return;

            sessionStorage.setItem("serilogui_token", token);
            $("#jwtToken").remove();
            $("#tokenContainer").text("*********");
            $(this).text("Clear");
            $(this).data("saved", "true");
            $("#jwtModalBtn").find("i").removeClass("fa-unlock").addClass("fa-lock");
            fetchLogs();
            return;
        }

        sessionStorage.removeItem("serilogui_token");
        $(this).text("Save");
        $(this).data("saved", "false");
        $("#tokenContainer").html('<input type="text" class="form-control" id="jwtToken" autocomplete="off" placeholder="Bearer eyJhbGciOiJSUz...">');
        $("#jwtModalBtn").find("i").removeClass("fa-lock").addClass("fa-unlock");
    });
}

const init = () => {
    initTokenUi();
    fetchLogs();
}

// mock fetch for development
if (process.env.NODE_ENV === 'development') {
    const setupWorker = async () => {
        const { worker } = await import('./mocks/browser');
        try {
            worker.start()
        } catch (err) {
            console.log(err)
        }
    }
    setupWorker().then(() => {
        initEventListeners();
        init();
    });
} else {
    initEventListeners();
    init();
}

