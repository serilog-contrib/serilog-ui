import { Button, Popover, Tooltip } from '@mantine/core';
import { IconRefresh } from '@tabler/icons-react';
import { liveRefreshOptions } from 'app/hooks/useLiveRefresh';
import useQueryLogs from 'app/hooks/useQueryLogs';
import classes from 'style/search.module.css';
import { theme } from 'style/theme';

export const RefreshButton = () => {
  const { isLiveRefreshRunning, liveRefreshLabel, startLiveRefresh, stopLiveRefresh } =
    useQueryLogs();

  if (isLiveRefreshRunning)
    return (
      <Tooltip label="Stop live refresh">
        <Button
          fz={9}
          size="compact-md"
          color={theme.colors?.green?.[7]}
          onClick={stopLiveRefresh}
          className={classes.refreshButton}
        >
          {liveRefreshLabel}
        </Button>
      </Tooltip>
    );

  return (
    <Popover width={105} trapFocus position="bottom" withArrow shadow="md">
      <Popover.Target>
        <Tooltip label="Start live refresh">
          <Button
            fz={9}
            size="compact-md"
            color={theme.colors?.gray?.[7]}
            className={classes.activateRefreshButton}
          >
            <IconRefresh />
          </Button>
        </Tooltip>
      </Popover.Target>
      <Popover.Dropdown>
        <Button.Group orientation="vertical">
          {liveRefreshOptions.map((p) => (
            <Button
              key={p.value}
              onClick={() => {
                startLiveRefresh(p.value);
              }}
              variant="default"
            >
              {p.label}
            </Button>
          ))}
        </Button.Group>
      </Popover.Dropdown>
    </Popover>
  );
};
