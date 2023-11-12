import userEvent from '@testing-library/user-event';
import { JSDOM } from 'jsdom';

export const renderSerilogUiDom = async (config?: Window['config']) => {
  const user = userEvent.setup();
  const dom = await JSDOM.fromFile('src/index.html');

  document.body.innerHTML = dom.window.document.body.innerHTML;
  window.userEventLibApi = user;
  window.config = {
    routePrefix: config?.routePrefix ?? 'serilog-ui',
    authType: config?.authType ?? 'Jwt',
    homeUrl: config?.homeUrl,
  };
  await import('../../main');
};
