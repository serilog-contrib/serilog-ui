import {
  ActionIcon,
  Badge,
  Group,
  MantineStyleProp,
  em,
  useMantineColorScheme,
  useMantineTheme,
} from '@mantine/core';
import { useMediaQuery } from '@mantine/hooks';
import { IconMoonStars, IconSun } from '@tabler/icons-react';
import { useMemo } from 'react';
import { HeaderActivity } from './HeaderActivity';

const styles: { [key: string]: MantineStyleProp } = {
  mobileGrid: {
    gridTemplateColumns: '2fr 2fr',
    gap: '0.5em',
  },
  desktopGrid: {
    gridTemplateColumns: 'repeat(2, 1fr)',
    gap: '1em',
  },
  activityGroup: {
    gridColumn: '1 / 2',
    height: '100%',
    justifyContent: 'start',
    alignContent: 'center',
  },
  bannerGroup: {
    gridColumn: '2 / 3',
    height: '100%',
    justifyContent: 'end',
    alignContent: 'center',
  },
};

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

  const currentYear = useMemo(() => new Date().getFullYear(), []);

  return (
    <Group
      display="grid"
      bg={colorScheme === 'dark' ? theme.colors.gray[8] : theme.colors.gray[3]}
      style={isMobileSize ? styles.mobileGrid : styles.desktopGrid}
      w="100%"
      h="100%"
      p={isSmallishSize ? 2 : isMobileSize ? 'xs' : 'lg'}
    >
      <Group style={styles.activityGroup}>
        <HeaderActivity isMobileOpen={isMobileOpen} toggleMobile={toggleMobile} />
      </Group>

      <Group style={styles.bannerGroup}>
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
          href="https://github.com/serilog-contrib/serilog-ui"
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
