import {
  ActionIcon,
  Badge,
  Group,
  em,
  useMantineColorScheme,
  useMantineTheme,
} from '@mantine/core';
import { useMediaQuery } from '@mantine/hooks';
import { IconHomeDot, IconMoonStars, IconSun } from '@tabler/icons-react';
import { useSerilogUiProps } from 'app/hooks/useSerilogUiProps';
import { currentYear } from 'app/util/dates';
import { isStringGuard } from 'app/util/guards';
import { serilogUiUrl } from 'app/util/prettyPrints';
import classes from 'style/header.module.css';
import { HeaderActivity } from './HeaderActivity';

type IDispatch = {
  toggleMobile: () => void;
};
interface IProps extends IDispatch {
  isMobileOpen: boolean;
}

const Head = ({ isMobileOpen, toggleMobile }: IProps) => {
  const theme = useMantineTheme();
  const { colorScheme, toggleColorScheme } = useMantineColorScheme();
  const isSmallishSize = useMediaQuery(`(max-width: ${em(360)})`);
  const isMobileSize = useMediaQuery(`(max-width: ${em(768)})`);

  const { homeUrl } = useSerilogUiProps();

  return (
    <Group
      display="grid"
      bg={colorScheme === 'dark' ? theme.colors.gray[8] : theme.colors.gray[3]}
      className={isMobileSize ? classes.mobileGrid : classes.desktopGrid}
      w="100%"
      h="100%"
      p={isSmallishSize ? 2 : isMobileSize ? 'xs' : 'lg'}
    >
      <Group className={classes.activityGroup}>
        <HeaderActivity isMobileOpen={isMobileOpen} toggleMobile={toggleMobile} />
      </Group>

      <Group className={classes.bannerGroup}>
        <ActionIcon
          variant="default"
          component={isStringGuard(homeUrl) ? 'a' : 'button'}
          href={isStringGuard(homeUrl) ? homeUrl : ''}
          target="_blank"
          size={24}
        >
          <IconHomeDot size="1rem" stroke={3} />
        </ActionIcon>
        <ActionIcon
          variant="default"
          onClick={() => {
            toggleColorScheme();
          }}
          size={24}
        >
          {colorScheme === 'dark' ? (
            <IconSun size="1rem" stroke="3" />
          ) : (
            <IconMoonStars size="1rem" stroke="3" />
          )}
        </ActionIcon>
        <Badge
          component="a"
          href={serilogUiUrl}
          target="_blank"
          size={isMobileSize ? 'sm' : 'lg'}
          rightSection={currentYear}
        >
          Serilog UI
        </Badge>
      </Group>
    </Group>
  );
};

export default Head;
