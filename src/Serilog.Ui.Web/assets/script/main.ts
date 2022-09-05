import * as $ from 'jquery';
window.jQuery = $; // global jquery for netstack
import 'popper.js';
import 'bootstrap';
import netStack = require('netstack.js'); // global require for netstack
import { initTokenUi, updateJwtToken } from './authentication';
import { fetchLogs } from './fetch';
import { changePageByModalChoice } from './pagination';

const initListenersAndDynamicInfo = () => {
    // print current year
    const date = new Date().getFullYear();
    document.getElementById('currentYearPrint').append(date.toString());

    // enable sidebar collapse
    document.querySelectorAll(".sidebar-collapse").forEach(sl => {
        sl.addEventListener("click", () => {
            document.querySelector("#sidebar").classList.toggle("active");
        })
    });

    // on jwt save/clear
    document.querySelector("#saveJwt").addEventListener("click", updateJwtToken);

    // reset choosen page handlers
    const resetLogPage = () => {
        document.querySelector<HTMLInputElement>("#page").value = "1";
        fetchLogs();
    }
    document.querySelector("#logCount").addEventListener("change", resetLogPage);
    document.querySelector("#logFilter").addEventListener("change", resetLogPage);
    document.querySelector("#submit").addEventListener("click", resetLogPage);

    // enable changePage button
    document.querySelector('.custom-pagination-submit').addEventListener('click', changePageByModalChoice);
}

const initHomeButton = () => {
    var homeButton = document.querySelector<HTMLAnchorElement>("#homeAnchor");

    if (window?.config?.homeUrl && window.config.homeUrl != homeButton.href) {
        homeButton.href = window?.config?.homeUrl;
    }
}

const init = () => {
    initListenersAndDynamicInfo();
    initTokenUi();
    fetchLogs();

    initHomeButton();
}

if (process.env.NODE_ENV === 'development') {
    // mock fetch for development
    const setupWorker = async () => {
        const { worker } = await import('../__tests__/util/mocks/msw-worker');
        try {
            await worker.start()
        } catch (err) {
            console.log(err)
        }
    }
    setupWorker().then(() => {
        init();
    });
} else {
    init();
}

