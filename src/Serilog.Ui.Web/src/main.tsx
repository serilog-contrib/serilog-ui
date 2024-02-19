import '@fontsource/mononoki';
import '@mantine/core/styles.css';
import '@mantine/dates/styles.css';
import '@mantine/notifications/styles.css';
import { createRoot } from 'react-dom/client';
import App from './app/App';

/**
 * TODO:
 * - change basic to receive data from ui...
 * - filter by _id (react router and opening log by itself)
 * - remove index.html from end of url and intercept all towards...
 * - sort by option
 * - update basic auth to receive from header and check by method
 * - sample with bassic and jwt auth that passes when receivign from modal auth heade
 * - if auth on index is active, redirect to unauth view instead of html page
 */

const main = async () => {
  const rootItem = document.getElementById('serilog-ui-app');
  if (rootItem == null)
    throw new Error(
      'React app not found. Are you sure you loaded the HTML content correctly?',
    );

  const root = createRoot(rootItem);

  // attach msw on development
  if (import.meta.env.MODE === 'development') {
    const { worker } = await import('./__tests__/_setup/msw-worker');
    try {
      await worker.start();
    } catch (err) {
      console.error(err);
      throw err;
    }
  }

  root.render(<App />);
};

void main();
