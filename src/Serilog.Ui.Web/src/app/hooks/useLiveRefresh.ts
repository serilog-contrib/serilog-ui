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
  { label: '5s', liveLabel: '5s', value: 'five' },
  { label: '15s', liveLabel: '15s', value: 'fifteen' },
  { label: '30s', liveLabel: '30s', value: 'thirty' },
  { label: '1m', liveLabel: '1m', value: 'sixty' },
  { label: '2m', liveLabel: '2m', value: 'onehundredtwenty' },
  { label: '5m', liveLabel: '5m', value: 'threehundred' },
  { label: '15m', liveLabel: '15m', value: 'ninehundred' },
];

export const useLiveRefresh = () => {
  const [refetchInterval, setRefetchInterval] = useState(0);

  const isLiveRefreshRunning = refetchInterval > 0;
  const liveRefreshLabel = !isLiveRefreshRunning
    ? ''
    : liveRefreshOptions.find((lr) => lr.value === options[refetchInterval / 1000])
        ?.liveLabel;

  const startLiveRefresh = (v: string | null) => {
    if (v === null) {
      return setRefetchInterval(0);
    }

    const eachSecond = options[v];
    setRefetchInterval(eachSecond * 1000);
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
