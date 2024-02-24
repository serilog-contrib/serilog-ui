import '@fontsource/mononoki';
import '@mantine/core/styles.css';
import '@mantine/dates/styles.css';
import '@mantine/notifications/styles.css';
import { createRoot } from 'react-dom/client';
import App from './app/App';

/**
 * TODO:
 * - test sort by option to validate development
 * - remove index.html from end of url and intercept all calls towards
 * > change basic auth to receive data from ui (header) and check it with custom impl
 * > sample with basic and jwt auth, that passes when receivin' from the modal auth header
 * >> change options registration to fluent interface on all DI registration
 * >> let mongo registers {n} logs pages
 * >> if auth on index is active, redirect to unauth view instead of html page
 * >> filter by _id (react router and opening log by itself)
 * >> custom columns
 * >> clean sample and install samples with each provider
 * >> remove net 5
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
