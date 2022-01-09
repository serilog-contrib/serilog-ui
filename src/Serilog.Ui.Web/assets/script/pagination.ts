import * as $ from 'jquery'
import { fetchLogs } from './fetch';

type PagingInfo = {
    currentPage: number,
    totalPages: number,
    boundaries: { startIndex: number, endIndex: number },
    hasPrev: boolean,
    hasNext: boolean
}
export const printPagination = (totalItems: number, itemPerPage: number, currentPage: number) => {
    const defaultPageLength = 5;
    const totalPages = Math.ceil(totalItems / itemPerPage);
    const boundaries = { ...getPagingBoundaries(currentPage, totalPages, defaultPageLength) };

    const pagingInfo: PagingInfo = {
        currentPage,
        totalPages,
        boundaries,
        hasPrev: totalPages > 1 && boundaries.startIndex > 1,
        hasNext: totalPages > defaultPageLength && boundaries.endIndex !== totalPages
    }

    createPaginationNodes(pagingInfo);
    updatePaginationModal(currentPage, totalPages);
    attachPaginationListeners();
}

const getPagingBoundaries = (currentPage: number, totalPages: number, defaultPageLength: number) => {
    if (totalPages <= defaultPageLength)
        return { startIndex: 1, endIndex: totalPages };

    if (totalPages < currentPage + 5)
        return { startIndex: totalPages - defaultPageLength, endIndex: totalPages };

    return { startIndex: currentPage, endIndex: (currentPage - 1) + defaultPageLength }
}

const createPaginationNodes = (cycle: PagingInfo) => {
    const paginationWrapper = document.querySelector("#pagination");
    paginationWrapper.innerHTML = "";

    const paginationItems: string[] = [];
    if (cycle.hasPrev) {
        const prevVal = cycle.currentPage - 1;
        paginationItems.push(`<li class="page-item first"><a href="#" data-val="1" tabindex="${prevVal}" class="page-link page-link-item">First</a></li>`);
        paginationItems.push(`<li class="page-item previous"><a href="#" data-val="${prevVal}" tabindex="${prevVal}" class="page-link page-link-item">Previous</a></li>`);
    }

    for (let i = cycle.boundaries.startIndex; i <= cycle.boundaries.endIndex; i++) {
        if (cycle.currentPage === i) {
            paginationItems.push(`<li class="page-item active"><span data-val="${i}" class="page-link page-link-item disabled">${i} <span class="sr-only">(current)</span></span></li>`);
        }
        else {
            paginationItems.push(`<li><a href="#" data-val="${i}" tabindex="${i}" class="page-link page-link-item">${i}</a></li>`);
        }
    }

    if (cycle.hasNext) {
        const nextVal = cycle.currentPage + 1;
        if (cycle.boundaries.endIndex < cycle.totalPages + 5) {
            paginationItems.push(`<li class="page-item page-link" data-toggle="modal" data-target="#changePageModal">&nbsp;...&nbsp;</li>`);
            paginationItems.push(`<li class="page-item next"><a href="#" data-val="${nextVal}" tabindex="${nextVal}" class="page-link page-link-item">Next</a></li>`);
            paginationItems.push(`<li class="page-item previous"><a href="#" data-val="${cycle.totalPages}" tabindex="${nextVal}" class="page-link page-link-item">Last</a></li>`);
        }
    }
    paginationWrapper.innerHTML = paginationItems.join('');
}

export const updatePaginationModal = (current, totalPages) => {
    document.querySelector('.custom-pagination-choice-totals').textContent = totalPages;
    const customPage = document.querySelector<HTMLInputElement>('#custom-pagination-choice');
    customPage.setAttribute('max', totalPages);
    customPage.value = current;
}

const attachPaginationListeners = () => {
    document.querySelectorAll('.page-link-item').forEach(i => i.addEventListener("click", (e) => {
        e.preventDefault();
        document.querySelector<HTMLInputElement>('#page').value = i.getAttribute('data-val');
        fetchLogs();
    }));
}

export const changePageByModalChoice = () => {
    const customPage = document.querySelector<HTMLInputElement>('#custom-pagination-choice');
    const [value, currMax] = [Number.parseInt(customPage.value), Number.parseInt(customPage.getAttribute('max'))];
    if (value > currMax) return;
    document.querySelector("#page").setAttribute("value", value.toString());
    fetchLogs(value);
    $('#changePageModal').modal('hide');
}