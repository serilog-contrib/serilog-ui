import { useState } from 'react';

enum options {
  'zero' = 0,
  'five' = 5,
  'fifteen' = 15,
  'thirty' = 30,
  'sixty' = 60,
  'onehundredtwenty' = 120,
  'threehundred' = 300,
  'ninehundred' = 900,
}

export const liveRefreshOptions = [
  { label: '5s', value: 'five' },
  { label: '15s', value: 'fifteen' },
  { label: '30s', value: 'thirty' },
  { label: '1m', value: 'sixty' },
  { label: '2m', value: 'onehundredtwenty' },
  { label: '5m', value: 'threehundred' },
  { label: '15m', value: 'ninehundred' },
];

export const useLiveRefresh = () => {
  const [refetchInterval, setRefetchInterval] = useState(0);

  const isLiveRefreshRunning = refetchInterval > 0;
  const liveRefreshLabel = !isLiveRefreshRunning
    ? ''
    : liveRefreshOptions.find((lr) => lr.value === options[refetchInterval / 1000])
        ?.label;

  const startLiveRefresh = (v: string | null) => {
    if (v === null) {
      return setRefetchInterval(0);
    }

    const eachSecond = options[v];
    const isNan = Number.isNaN(Number.parseInt(eachSecond, 10));
    setRefetchInterval(isNan ? 0 : eachSecond * 1000);
  };

  const stopLiveRefresh = () => {
    setRefetchInterval(0);
  };

  return {
    isLiveRefreshRunning,
    liveRefreshLabel,
    refetchInterval,
    startLiveRefresh,
    stopLiveRefresh,
  };
};
