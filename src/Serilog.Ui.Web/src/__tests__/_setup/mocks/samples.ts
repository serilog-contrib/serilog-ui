import { faker } from '@faker-js/faker';
import {
  AdditionalColumn,
  AdditionalColumnLogType,
  ColumnType,
  ColumnsInfo,
  LogLevel,
  RemovableColumns,
  type EncodedSeriLogObject,
  type SearchResult,
} from '../../../types/types';
import { dbKeysMock } from './fetchMock';

faker.seed(10);

//#region code samples
const createRandomJsonObject = (propLength: number = 13) => {
  const fakeObject = {};
  const nestedIndexes = [1, 4, 7];
  for (let index = 0; index < propLength; index++) {
    const fakeProperty = faker.lorem.slug(1);
    if (!nestedIndexes.includes(index)) {
      fakeObject[fakeProperty] = faker.lorem.text();
    } else {
      fakeObject[fakeProperty] = createRandomJsonObject(index - 1);
    }
  }
  return fakeObject;
};

const createRandomXmlObject = () => {
  const randomJson = createRandomJsonObject();
  const transformJsonStructureToXml = (object: string | Record<string, string>) => {
    if (typeof object === 'string') {
      const xmlTag = faker.lorem.slug(1);
      return `<${xmlTag}>${object}</${xmlTag}>`;
    }

    return Object.keys(object).reduce((prev, curr) => {
      const openXml = `<${curr}>`;
      const currentItem = object[curr];
      const xmlString =
        typeof currentItem === 'string'
          ? currentItem
          : transformJsonStructureToXml(currentItem);
      const closeXml = `</${curr}>`;
      return prev + openXml + xmlString + closeXml;
    }, '');
  };
  const randomXml = Object.keys(randomJson).reduce((prev, curr) => {
    const xmlString = transformJsonStructureToXml(randomJson[curr]);
    return prev + xmlString;
  }, '<root>');

  return `${randomXml}</root>`;
};

const jsonExample = JSON.stringify(createRandomJsonObject());
const xmlExample = createRandomXmlObject();
//#endregion

//#region additional columns info
const fakeAdditionalColumns: (AdditionalColumn & { value: () => unknown })[] = [
  {
    name: 'SampleText',
    typeName: ColumnType.string,
    codeType: null,
    value: faker.lorem.sentence,
  },
  {
    name: 'SampleDate',
    typeName: ColumnType.datetime,
    codeType: null,
    value: () => faker.date.recent({ days: 15 }).toISOString(),
  },
  {
    name: 'SampleBool',
    typeName: ColumnType.boolean,
    codeType: null,
    value: faker.datatype.boolean,
  },
  {
    name: 'SampleCode',
    typeName: ColumnType.code,
    codeType: AdditionalColumnLogType.Json,
    value: () => jsonExample,
  },
];

export const fakeColumnsInfo: ColumnsInfo = {
  [dbKeysMock[0]]: {
    additionalColumns: fakeAdditionalColumns,
    removedColumns: [],
  },
  [dbKeysMock[1]]: {
    additionalColumns: fakeAdditionalColumns,
    removedColumns: [RemovableColumns.properties],
  },
  [dbKeysMock[2]]: {
    additionalColumns: fakeAdditionalColumns,
    removedColumns: [RemovableColumns.properties, RemovableColumns.exception],
  },
};
//#endregion

//#region faker - logs
const createRandomLog = (): EncodedSeriLogObject => {
  const log: EncodedSeriLogObject = {
    rowNo: faker.number.int(),
    level: faker.helpers.enumValue(LogLevel),
    message: faker.lorem.lines(3),
    timestamp: faker.date.recent({ days: 30 }).toISOString(),
    propertyType: faker.helpers.arrayElement(['json', 'xml']),
  };

  const sampleData = log.propertyType === 'json' ? jsonExample : xmlExample;
  log.properties = sampleData;

  if ([LogLevel.Error, LogLevel.Fatal].includes(LogLevel[log.level])) {
    log.exception = log.properties;
  }

  return log;
};

const createRandomLogWithAdditionalColumns = (keyMockIndex = 1) => {
  const log = createRandomLog();

  const entry = fakeColumnsInfo[dbKeysMock[keyMockIndex]];

  entry.additionalColumns.forEach((element) => {
    const data = element as AdditionalColumn & { value: () => unknown };

    log[data.name] = data.value();
  });

  entry.removedColumns.forEach((element) => {
    delete log[element.toLowerCase()];
  });

  return log;
};

export const fakeLogs: SearchResult = {
  logs: faker.helpers.multiple(() => createRandomLogWithAdditionalColumns(0), {
    count: 137,
  }),
  total: 137,
  count: 10,
  currentPage: 1,
};

export const fakeLogs2ndTable: SearchResult = {
  ...fakeLogs,
  logs: faker.helpers.multiple(createRandomLogWithAdditionalColumns, { count: 95 }),
  total: 95,
};

export const fakeLogs3rdTable: SearchResult = {
  ...fakeLogs,
  logs: faker.helpers.multiple(() => createRandomLogWithAdditionalColumns(2), {
    count: 33,
  }),
  total: 33,
};
//#endregion
