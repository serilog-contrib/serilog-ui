import { faker } from '@faker-js/faker';
import {
  LogLevel,
  type EncodedSeriLogObject,
  type SearchResult,
} from '../../../types/types';

faker.seed(10);

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

export const fakeLogs: SearchResult = {
  logs: faker.helpers.multiple(createRandomLog, { count: 137 }),
  total: 139,
  count: 10,
  currentPage: 1,
};
