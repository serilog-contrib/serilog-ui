import {
  type EncodedSeriLogObject,
  LogLevel,
  type SearchResult,
} from '../../../types/types';
import { faker } from '@faker-js/faker';

faker.seed(10);

const createRandomLog = (): EncodedSeriLogObject => {
  const log: EncodedSeriLogObject = {
    rowNo: faker.number.int(),
    level: faker.helpers.enumValue(LogLevel),
    message: faker.lorem.lines(3),
    timestamp: faker.date.recent({ days: 30 }).toISOString(),
    propertyType: faker.helpers.arrayElement(['json', 'xml']),
  };

  
  log.properties = JSON.stringify({'prop': faker.helpers.objectEntry({ ...log })});

  if ([LogLevel.Error, LogLevel.Fatal].includes(log.level as LogLevel))
    log.exception = JSON.stringify(faker.helpers.objectEntry({ ...log }));

  return log;
};

export const fakeLogs: SearchResult = {
  logs: faker.helpers.multiple(createRandomLog, { count: 139 }),
  total: 139,
  count: 10,
  currentPage: 1,
};
