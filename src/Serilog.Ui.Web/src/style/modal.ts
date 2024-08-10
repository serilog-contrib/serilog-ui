import {
  DefaultMantineColor,
  MantineColorScheme,
  MantineColorsTuple,
} from '@mantine/core';

export const boxButton = { justifyContent: 'center', alignContent: 'center' };

export const boxGridProperties = { gap: '1.2em', width: '100%' };

export const overlayProps = (
  colorScheme: MantineColorScheme,
  colors: Record<DefaultMantineColor, MantineColorsTuple>,
) => ({
  color: colorScheme === 'dark' ? colors.dark[9] : colors.gray[2],
  opacity: 0.55,
  blur: 3,
});
