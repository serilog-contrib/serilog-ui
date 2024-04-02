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

// column definitions
export enum ColumnType {
  code = 'code',
  datetime = 'date',
  shortstring = 'shortstring',
  string = 'string',
  text = 'text',
}
