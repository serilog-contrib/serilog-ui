import dayjs from 'dayjs';
import localizedFormat from 'dayjs/plugin/localizedFormat';
import utc from 'dayjs/plugin/utc';

dayjs.extend(localizedFormat);
dayjs.extend(utc);

export const currentYear = dayjs().year();

export const formatLocalDate = (date: string) => dayjs(date).format('ll HH:mm:ss');

export const formatLocalSplitDate = (date: string) => {
  const dayjsDate = dayjs(date);

  return [dayjsDate.format('ll'), dayjsDate.format('HH:mm:ss')];
};

export const formatUtcDate = (date: string) =>
  dayjs.utc(date).format('ll HH:mm:ss [[UTC]]');

export const formatUtcSplitDate = (date: string) => {
  const dayjsDate = dayjs.utc(date);

  return [dayjsDate.format('ll'), dayjsDate.format('HH:mm:ss [[UTC]]')];
};

export const isValidJwtUnixDate = (unixTime: number) => {
  const parse = dayjs.unix(unixTime).utc(true);

  return parse.isAfter(dayjs());
};
