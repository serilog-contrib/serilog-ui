import * as $ from 'jquery';
import { paging } from './pagination';
import { cleanHtmlTags, fixedLengthMessageWithModal, formatDate, formatXml } from './util';
import { AuthPropertiesSingleton } from './authentication';
import { AuthType } from '../types/types';

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

// asc or desc
// readme instr for build and dev

export const fetchLogs = (identifiedPage?: number) => {
    const tbody = $("#logTable tbody");
    const page = identifiedPage ?? ($("#page").val() || "1");
    const count = $("#count").children("option:selected").val();
    const level = $("#level").children("option:selected").val();
    const startDate = $("#startDate").val();
    const endDate = $("#endDate").val();

    if (startDate && endDate !== null) {
        const start = Date.parse(startDate);
        const end = Date.parse(endDate);
        if (start > end) {
            alert("Start date cannot be greater than end date");
            return;
        }
    }

    const searchTerm = escape($("#search").val());
    const host = process.env.NODE_ENV === "development" ? "" : location.pathname.replace("/index.html", "");
    const url = `${host}/api/logs?page=${page}&count=${count}&level=${level}&search=${searchTerm}&startDate=${startDate}&endDate=${endDate}`;

    const token = sessionStorage.getItem("serilogui_token");
    let xf = null;
    if (AuthPropertiesSingleton.authType !== AuthType.Windows)
        $.ajaxSetup({ headers: { 'Authorization': token } });
    else {
        xf = {
            withCredentials: true
        };
    }
    $.get({
        url: url,
        xhrFields: xf,
        success: function (data) {
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
                    <span class="overflow-auto"><truncate length="100">${fixedLengthMessageWithModal(cleanHtmlTags(log.message), 100)}</truncate></span>
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
            paging(data.total, data.count, data.currentPage);
        }
    }).fail(function (error) {
        if (error.status === 403) {
            console.log(error);
            alert("You are not authorized you to access logs.\r\nYou are not logged in or you don't have enough permissions to perform the requested operation.");
        } else if (error.status === 500) {
            const x = JSON.parse(error.responseJSON.errorMessage);
            alert(x.errorMessage);
        } else {
            alert(error.responseText);
        }
    });
}
