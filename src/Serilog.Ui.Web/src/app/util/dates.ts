import dayjs from 'dayjs';
import localizedFormat from 'dayjs/plugin/localizedFormat';
import utc from 'dayjs/plugin/utc';

dayjs.extend(localizedFormat);
dayjs.extend(utc);

export const formatLocalDate = (date: string) => dayjs(date).format('ll HH:mm:ss');

export const formatUtcDate = (date: string) => dayjs.utc(date).format('ll HH:mm:ss');
