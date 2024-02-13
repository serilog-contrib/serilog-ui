import { createTheme } from '@mantine/core';
import { serilogUi } from './colors';

export const theme = createTheme({
  breakpoints: {
    xs: '28em', // 448px
    sm: '36em', // 576px
  },
  colors: serilogUi,
  fontFamily: 'Open Sans Variable, sans-serif',
  fontFamilyMonospace: 'Mononoki, sans-serif',
  autoContrast: true,
});
