import { AuthType } from '../types/types';
import { fetchLogs } from './fetch';

class AuthProperties {
    authType?: AuthType;
    routePrefix?: string;
    constructor() { }
    init() {
        let auth = "";
        ({ authType: auth, routePrefix: this.routePrefix } = window.config);
        this.authType = AuthType[auth];
    }

    private static instance: AuthProperties;

    static getInstance() {
        if (!this.instance) {
            this.instance = new AuthProperties();
            this.instance.init();
        }
        return this.instance;
    }
}

export const AuthPropertiesSingleton = AuthProperties.getInstance();

export const initTokenUi = () => {
    if (AuthPropertiesSingleton.authType === AuthType.Jwt) {
        const token = sessionStorage.getItem("serilogui_token");
        if (!token) return;
        saveTokenInfo();
    } else {
        const jwtBtn = document.querySelector("#jwtModalBtn");
        jwtBtn.parentNode.removeChild(jwtBtn);
        sessionStorage.removeItem("serilogui_token");
    }
}

export const updateJwtToken = () => {
    const isJwtSaved = document.querySelector<HTMLButtonElement>("#saveJwt").dataset.saved;
    console.log(isJwtSaved)
    if (isJwtSaved === "false") {
        const token = document.querySelector<HTMLInputElement>("#jwtToken").value;
        if (!token) return;

        sessionStorage.setItem("serilogui_token", token);
        saveTokenInfo();
        fetchLogs();
        return;
    }
    removeTokenInfo();
}

const saveTokenInfo = () => {
    removeTokenInput();
    document.querySelector("#tokenContainer").textContent = "*********";
    const saveJwt = document.querySelector<HTMLButtonElement>("#saveJwt");
    saveJwt.textContent = "Clear";
    saveJwt.dataset.saved = "true";
    const jwtBtnClasses = document.querySelector("#jwtModalBtn i").classList;
    jwtBtnClasses.remove('fa-unlock');
    jwtBtnClasses.add('fa-lock');
}

const removeTokenInput = () => {
    const jwtToken = document.querySelector("#jwtToken");
    jwtToken.innerHTML = "";
}

const removeTokenInfo = () => {
    sessionStorage.removeItem("serilogui_token");
    const saveJwt = document.querySelector<HTMLButtonElement>("#saveJwt");
    saveJwt.textContent = "Save";
    saveJwt.dataset.saved = "false";
    const jwtBtnClasses = document.querySelector("#jwtModalBtn i").classList;
    jwtBtnClasses.remove('fa-lock');
    jwtBtnClasses.add('fa-unlock');
    document.querySelector('#tokenContainer').innerHTML =
        '<input type="text" class="form-control" id="jwtToken" autocomplete="off" placeholder="Bearer eyJhbGciOiJSUz...">';
}