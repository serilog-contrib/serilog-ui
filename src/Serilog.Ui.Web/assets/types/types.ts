export { }
declare global {
    export interface JQuery {
        netStack: ({ prettyprint }: { prettyprint: boolean }) => void
    }
    export interface Window {
        $: JQueryStatic
        jQuery: JQueryStatic,
        config: {
            authType?: string,
            routePrefix?: string,
        }
    }
    export interface globalThis {
        $: JQueryStatic,
    }
}

export enum AuthType {
    Jwt = 'Jwt',
    Windows = 'Windows'
}

export type SeriLogObject = {

}