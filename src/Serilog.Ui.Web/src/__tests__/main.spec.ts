/* eslint-disable testing-library/no-node-access */
import { act } from '@testing-library/react';
import FS from 'fs';
import { Window } from 'happy-dom';
import { afterEach, beforeEach, describe, expect, it } from 'vitest';

export const loadSerilogUiDom = async () => {
  const fakeWindow = new Window({
    settings: { disableJavaScriptFileLoading: true, disableCSSFileLoading: true },
  });

  const doc = fakeWindow.document;

  const html = await FS.promises.readFile('src/index.html');

  doc.write(html.toString());

  // Wait for async tasks such as scripts, styles, fetches and timers to complete
  await fakeWindow.happyDOM.waitUntilComplete();

  return fakeWindow;
};

describe('index', () => {
  let fakeHappyDomWindow: Window;
  beforeEach(async () => {
    await act(async () => {
      fakeHappyDomWindow = await loadSerilogUiDom();
    });
  });

  afterEach(async () => {
    await fakeHappyDomWindow.happyDOM.abort();
    fakeHappyDomWindow.close();
  });

  it('loads the index page, with the expected react-app id inside', async () => {
    expect(
      fakeHappyDomWindow.document.getElementById('serilog-ui-app'),
    ).toBeInTheDocument();
  });
});
