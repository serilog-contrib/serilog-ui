import * as $ from 'jquery';
import { printPagination } from './pagination';
import { cleanHtmlTags, fixedLengthMessageWithModal, formatDate, formatXml, getBgLogLevel } from './util';
import { AuthPropertiesSingleton } from './authentication';
import { AuthType, LogLevel, SearchResult } from '../types/types';
import parseISO from 'date-fns/esm/parseISO';
import isAfter from 'date-fns/esm/isAfter';

export const fetchLogs = (identifiedPage?: number) => {
    const prepareUrl = prepareSearchUrl(identifiedPage);
    if (!prepareUrl.areDatesAdmitted) return;

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
        url: prepareUrl.url,
        xhrFields: xf,
        success: onFetchLogs,
    }).fail((error) => {
        console.warn(error);
        if (error.status === 403) {
            alert("You are not authorized you to access logs.\r\nYou are not logged in or you don't have enough permissions to perform the requested operation.");
        } else if (error.status === 500) {
            const fatalServerError = JSON.parse(error.responseJSON.errorMessage);
            alert(fatalServerError.errorMessage);
        } else {
            alert(error.responseText);
        }
    });
}

const prepareSearchUrl = (identifiedPage?: number) => {
    const startDate = document.querySelector<HTMLInputElement>("#startDate").value;
    const endDate = document.querySelector<HTMLInputElement>("#endDate").value;
    if (startDate && endDate) {
        const start = parseISO(startDate);
        const end = parseISO(endDate);
        if (isAfter(start, end)) {
            alert("Start date cannot be greater than end date");
            return { areDatesAdmitted: false, url: '' };
        }
    }

    const page = identifiedPage ?? (document.querySelector<HTMLInputElement>("#page").value || "1");
    const count = $("#count").children("option:selected").val();
    const level = $("#level").children("option:selected").val();
    const searchTerm = escape(document.querySelector<HTMLInputElement>("#search").value);
    const host = process.env.NODE_ENV === "development" ? "" : location.pathname.replace("/index.html", "");

    const url = `${host}/api/logs?page=${page}&count=${count}&level=${level}&search=${searchTerm}&startDate=${startDate}&endDate=${endDate}`;
    return { areDatesAdmitted: true, url };
}

const onFetchLogs = (data: SearchResult) => {
    const tbody = $("#logTable tbody");
    $(tbody).empty();
    data.logs.forEach((log) => {
        let exception = "";
        if (!!log.exception) {
            exception = `<a href="#" title="Click to view" class="modal-trigger" data-type="text">
                View <span style="display: none">${log.exception}</span></a>`;
        }
        const row = `<tr class="${log.level}">
            <td class="text-center">${log.rowNo}</td>
            <td class="text-center"><span class="log-level text-white ${getBgLogLevel(LogLevel[log.level])}">${log.level}</span></td>
            <td class="text-center">${formatDate(log.timestamp)}</td>
            <td class="log-message">
                <span class="overflow-auto"><truncate length="100">${fixedLengthMessageWithModal(cleanHtmlTags(log.message), 100)}</truncate></span>
            </td>
            <td class="text-center">
                ${exception}
            </td>
            <td class="text-center">
                <a href="#" class="modal-trigger" title="Click to view" data-type="${log.propertyType}">
                    View <span style="display: none">${log.properties}</span>
                </a>
            </td>
        </tr>`;
        $(tbody).append(row);
    });
    updateSearchResultInfo(data);
    printPagination(data.total, data.count, data.currentPage);
}

export const updateSearchResultInfo = (data: SearchResult) => {
    document.querySelector("#totalLogs").textContent = data.total.toString();
    document.querySelector("#showingItemsStart").textContent = data.currentPage.toString();
    document.querySelector("#showingItemsEnd").textContent = data.count.toString();
}