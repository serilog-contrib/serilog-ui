export enum AuthType {
  Basic = 'Basic',
  Jwt = 'Jwt',
  Windows = 'Windows',
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
}

export interface SeriLogObject {
  rowNo: number;
  level: LogLevel;
  message: string;
  timestamp: string;
  exception?: Record<string, string>;
  properties?: Record<string, string>;
  propertyType: LogType;
}

export interface EncodedSeriLogObject {
  rowNo: number;
  level: string;
  message: string;
  timestamp: string;
  exception?: string;
  properties?: string;
  propertyType: string;
}

export interface SearchResult {
  logs: EncodedSeriLogObject[];
  total: number;
  count: number;
  currentPage: number;
}

export enum SearchParameters {
  Count = 'count',
  Page = 'page',
  Level = 'level',
  Search = 'search',
  StartDate = 'startDate', // wip
  EndDate = 'endDate', // wip
  SortDirection = 'sort', // wip
}

export interface SearchForm {
  table: string;
  level: LogLevel | null;
  startDate: Date | null;
  endDate: Date | null;
  search: string;
  entriesPerPage: string;
  page: number;
}

// column definitions
export enum ColumnType {
  code = 'code',
  datetime = 'date',
  shortstring = 'shortstring',
  string = 'string',
  text = 'text',
}
