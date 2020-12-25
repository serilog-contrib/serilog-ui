"use strict";

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
        fetchData();
    });

    $("#logFilter").on("change", function () {
        $("#page").val("1");
        fetchData();
    });

    $("#search").on("keypress", function (e) {
        if (e.which === 13) {
            $("#page").val("1");
            fetchData();
        }
    });

    $("#search").on('input', function () {
        if ($(this).val() !== "") {
            return;
        }

        $("#page").val("1");
        fetchData();
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

const routePrefix = {
    url: '',
    set setUrl(route) {
        this.url = route;
    }
}

const init = (route) => {
    routePrefix.setUrl = route;
    fetchData();
}

const fetchData = () => {
    const tbody = $("#logTable tbody");
    const count = $("#count").children("option:selected").val();
    const level = $("#level").children("option:selected").val();
    const searchTerm = escape($("#search").val());
    const url = `/${routePrefix.url}/api/logs?count=${count}&level=${level}&search=${searchTerm}`;
    //const url = `/${routePrefix.url}/api/logs?count=${count}&level=${level}`;
    $.get(url, function (data) {
        $("#totalLogs").html(data.total);
        $("#showingItemsStart").html(data.page);
        $("#showingItemsEnd").html(data.count);
        $(tbody).empty();
        data.logs.forEach(function (log) {
            let exception = "";
            if (log.exception != undefined) {
                exception =
                    `<a href="#" title="Click to view" class="modal-trigger" data-type="text">
                        View <span style="display: none">${log.exception}</span>
                    </a>`;
            }
            const row = `<tr class="${log.level}">
                <td class="text-center">${log.rowNo}</td>
                <td class="text-center"><span class="log-level text-white ${levelClass(log.level)}">${log.level}</span></td>
                <td class="text-center">${formatDate(log.timestamp)}</td>
                <td class="log-message">
                    <span class="overflow-auto"><truncate length="100">${truncateString(log.message, 100)}</truncate></span>
                </td>
                <td class="text-center">
                    ${exception}
                 </td>
                <td class="text-center">
                    <a href="#" class="modal-trigger" title="Click to view" data-type="${log.propertyType}">
                    View
                        <span style="display: none">${log.properties}</span>
                    </a>
                </td>
            </tr>`;
            $(tbody).append(row);
        });
    }).fail(function (error) {
        console.log(error);
        alert("error");
    });
}

const levelClass = (logLevel) => {
    switch (logLevel) {
        case "Verbose":
        case "Debug":
            return "bg-success";
        case "Information":
            return "bg-primary";
        case "Warning":
            return "bg-warning";
        case "Error":
            return "bg-danger";
        default:
            return "";
    }
}

const formatDate = (date) => {
    var dt = new Date(date);
    return `${(dt.getMonth() + 1).toString().padStart(2, '0')}/${dt.getDate().toString().padStart(2, '0')}/${dt.getFullYear().toString().padStart(4, '0')}
            ${dt.getHours().toString().padStart(2, '0')}:${dt.getMinutes().toString().padStart(2, '0')}:${dt.getSeconds().toString().padStart(2, '0')}`;
}

const truncateString = (str, num) => {
    if (str.length <= num) {
        return str;
    }

    const truncated = str.slice(0, num) + '...';
    const html = `<a href="#" title="Click to view" class="modal-trigger" data-type="text">
                    ${truncated}
                    <span style=\"display: none\">${str}</span>
                  </a>`;

    return html;
}

let formatXml = (xml, tab) => { // tab = optional indent value, default is tab (\t)
    var formatted = '', indent = '';
    tab = tab || '\t';
    xml.split(/>\s*</).forEach(function (node) {
        if (node.match(/^\/\w/)) indent = indent.substring(tab.length); // decrease indent by one 'tab'
        formatted += indent + '<' + node + '>\r\n';
        if (node.match(/^<?\w[^>]*[^\/]$/)) indent += tab;              // increase indent
    });
    return formatted.substring(1, formatted.length - 3);
}