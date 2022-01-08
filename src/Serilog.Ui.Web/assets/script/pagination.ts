import * as $ from 'jquery'
import { fetchLogs } from './fetch';

export const printPagination = (totalItems, itemPerPage, currentPage) => {
    const defaultPageLength = 5;
    const totalPages = Math.ceil(totalItems / itemPerPage);
    //let totalPages = parseInt(totalItems / itemPerPage);
    //totalPages += totalItems % itemPerPage !== 0 ? 1 : 0;
    let startIndex, endIndex;

    if (totalPages <= defaultPageLength) {
        startIndex = 1;
        endIndex = totalPages;
    } else if (totalPages < currentPage + 5) {
        startIndex = totalPages - defaultPageLength;
        endIndex = totalPages;
    } else {
        startIndex = currentPage;
        endIndex = (currentPage - 1) + defaultPageLength;
    }

    const hasPrev = totalPages > 1 && startIndex > 1;
    const hasNext = totalPages > defaultPageLength && endIndex !== totalPages;
    const pagination = $("#pagination");
    $(pagination).empty();

    if (hasPrev) {
        const prevVal = currentPage - 1;
        $(pagination).append(`<li class="page-item first"><a href="#" data-val="1" tabindex="${prevVal}" class="page-link page-link-item">First</a></li>`);
        $(pagination).append(`<li class="page-item previous"><a href="#" data-val="${prevVal}" tabindex="${prevVal}" class="page-link page-link-item">Previous</a></li>`);
    }

    for (let i = startIndex; i <= endIndex; i++) {
        if (currentPage === i) {
            $(pagination).append(`<li class="page-item active"><span data-val="${i}" class="page-link page-link-item disabled">${i} <span class="sr-only">(current)</span></span></li>`);
        }
        else {
            $(pagination).append(`<li><a href="#" data-val="${i}" tabindex="${i}" class="page-link page-link-item">${i}</a></li>`);
        }
    }

    if (hasNext) {
        const nextVal = currentPage + 1;
        if (endIndex < totalPages + 5) {
            $(pagination).append(`<li class="page-item page-link" data-toggle="modal" data-target="#changePageModal">&nbsp;...&nbsp;</li>`);
            $(pagination).append(`<li class="page-item next"><a href="#" data-val="${nextVal}" tabindex="${nextVal}" class="page-link page-link-item">Next</a></li>`);
            $(pagination).append(`<li class="page-item previous"><a href="#" data-val="${totalPages}" tabindex="${nextVal}" class="page-link page-link-item">Last</a></li>`);
        }
    }

    updatePagingModal(currentPage, totalPages);
}

export const changePageByModalChoice = () => {
    const customPage = document.querySelector<HTMLInputElement>('#custom-pagination-choice');
    const [value, currMax] = [Number.parseInt(customPage.value), Number.parseInt(customPage.getAttribute('max'))];
    if (value > currMax) return;
    document.querySelector("#page").setAttribute("value", value.toString());
    fetchLogs(value);
    $('#changePageModal').modal('hide');
}

export const updatePagingModal = (current, totalPages) => {
    document.querySelector('.custom-pagination-choice-totals').textContent = totalPages;
    const customPage = document.querySelector<HTMLInputElement>('#custom-pagination-choice');
    customPage.setAttribute('max', totalPages);
    customPage.value = current;
}