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
  table: string | null;
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
export enum AdditionalColumnLogType {
  None,
  Xml,
  Json,
}

export type AdditionalColumn = {
  name: string;
  typeName: ColumnType;
  codeType: AdditionalColumnLogType | null;
};

export enum RemovableColumns {
  exception = 'Exception',
  properties = 'Properties',
}

export type ColumnsInfo = {
  [dbKey: string]: {
    additionalColumns: AdditionalColumn[];
    removedColumns: RemovableColumns[];
  };
};

export enum DispatchedCustomEvents { RemoveTableKey = 'remove-table-key' }

export interface SerilogUiConfig {
  authType?: AuthType;
  routePrefix?: string;
  homeUrl?: string;
  showBrand?: boolean;
  expandDropdownsByDefault?: boolean;
  blockHomeAccess?: boolean;
  disabledSortOnKeys?: string[];
  renderExceptionAsStringKeys?: string[];
  columnsInfo?: ColumnsInfo;
}

export enum ColumnType {
  boolean = 'boolean',
  code = 'code',
  datetime = 'datetime',
  datetimeoffset = 'datetimeoffset',
  shortstring = 'shortstring',
  string = 'string',
  text = 'text',
}
