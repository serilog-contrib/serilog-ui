import userEvent from '@testing-library/user-event';
import { JSDOM } from 'jsdom';

export const renderSerilogUiDom = async () => {
  const user = userEvent.setup();
  const dom = await JSDOM.fromFile('src/index.html');

  document.body.innerHTML = dom.window.document.body.innerHTML;
  window.userEventLibApi = user;

  await import('../../main');
};
