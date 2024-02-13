import FS from 'fs';
import { Window } from 'happy-dom';

/**
 * Load the DOM before each test and setup userEvent.
 * If the window.config should be overriden, either
 * - re-call the method in the test itself with params
 * - set the config manually in the test
 */
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
