import * as $ from 'jquery'
import { AuthType } from '../types/types';

class AuthProperties {
    authType?: AuthType;
    routePrefix?: string;
    constructor() { }
    async init() {
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

        $("#jwtToken").remove();
        document.querySelector("#tokenContainer").textContent = "*********";
        const saveJwt = document.querySelector<HTMLButtonElement>("#saveJwt");
        saveJwt.textContent = "Clear";
        saveJwt.dataset.saved = "true";
        $("#jwtModalBtn").find("i").removeClass("fa-unlock").addClass("fa-lock");
    } else {
        $("#jwtModalBtn").remove();
        sessionStorage.removeItem("serilogui_token");
    }
}