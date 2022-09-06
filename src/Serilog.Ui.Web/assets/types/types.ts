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
            homeUrl?: string
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

export enum LogLevel {
    Verbose = "Verbose",
    Debug = "Debug",
    Information = "Information",
    Warning = "Warning",
    Error = "Error",
    Fatal = "Fatal"
}

export enum LogType {
    Json = "json",
    Xml = "xml",
}

export type SeriLogObject = {
    rowNo: number,
    level: LogLevel,
    message: string,
    timestamp: string,
    exception?: { [index: string]: string },
    properties?: { [index: string]: string },
    propertyType: LogType,
}

export type EncodedSeriLogObject = {
    rowNo: number,
    level: string,
    message: string,
    timestamp: string,
    exception?: string,
    properties?: string,
    propertyType: string,
}

export type SearchResult = {
    logs: EncodedSeriLogObject[],
    total: number,
    count: number,
    currentPage: number
}

export enum SearchSortParametersOptions {
    Timestamp = 'Timestamp',
    Level = 'Level',
    Message = 'Message'
}
export enum SearchSortDirectionOptions {
    Asc = 'Asc',
    Desc = 'Desc',
}

export enum SearchParameters {
    Count = 'count',
    Page = 'page',
    Level = 'level',
    Search = 'search',
    StartDate = 'startDate',
    EndDate = 'endDate',
    SortDirection = 'sortby',
    SortParameter = 'sorton'
}