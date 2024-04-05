export enum AuthType {
  Basic = 'Basic',
  Jwt = 'Jwt',
  Custom = 'Custom',
}

export enum LogLevel {
  Verbose = 'Verbose',
  Debug = 'Debug',
  Information = 'Information',
  Warning = 'Warning',
  Error = 'Error',
  Fatal = 'Fatal',
}

export enum LogType {
  Json = 'json',
  Xml = 'xml',
  None = 'none',
}

export interface EncodedSeriLogObject {
  rowNo: number;
  level: string;
  message: string;
  timestamp: string;
  exception?: string;
  properties?: string;
  propertyType: string;
  [key: string]: unknown; // in case of additional columns
}

export interface SearchResult {
  logs: EncodedSeriLogObject[];
  total: number;
  count: number;
  currentPage: number;
}

export enum SearchParameters {
  Table = 'key',
  Count = 'count',
  Page = 'page',
  Level = 'level',
  Search = 'search',
  StartDate = 'startDate',
  EndDate = 'endDate',
  SortProperty = 'sortOn',
  SortDirection = 'sortBy',
}

export interface SearchForm {
  table: string;
  level: LogLevel | null;
  startDate: Date | null;
  endDate: Date | null;
  search: string;
  sortOn: SortPropertyOptions;
  sortBy: SortDirectionOptions;
  entriesPerPage: string;
  page: number;
}

export enum SortPropertyOptions {
  Timestamp = 'Timestamp',
  Level = 'Level',
  Message = 'Message',
}

export enum SortDirectionOptions {
  Asc = 'Asc',
  Desc = 'Desc',
}

// configuration definitions
export type AdditionalColumn = {
  name: string;
  typeName: ColumnType;
  codeType: LogType | null;
};

export enum RemovableColumns {
  exception = 'exception',
  properties = 'properties',
}

export type ColumnsInfo = {
  [dbKey: string]: {
    AdditionalColumns: AdditionalColumn[];
    RemovedColumns: RemovableColumns[];
  };
};

export interface SerilogUiConfig {
  authType?: AuthType;
  routePrefix?: string;
  homeUrl?: string;
  hideBrand?: boolean;
  disabledSortOnKeys?: string[];
  columnsInfo?: ColumnsInfo;
}

export enum ColumnType {
  boolean = 'boolean',
  code = 'code',
  datetime = 'date',
  shortstring = 'shortstring',
  string = 'string',
  text = 'text',
}
