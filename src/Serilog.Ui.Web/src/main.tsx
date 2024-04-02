import '@fontsource/mononoki';
import '@mantine/core/styles.css';
import '@mantine/dates/styles.css';
import '@mantine/notifications/styles.css';
import { createRoot } from 'react-dom/client';
import App from './app/App';

/**
 * TODO:
(branch/routing-by-id)
 * >> if auth on index is active, redirect to unauth route instead of empty html page (react router)
 * >> react router setup to catch id routing
 * >> endpoint and additional model property
 * >> filter by _id (react router and opening log by itself)

(branch/custom-columns)
 * >> custom columns

(extras)
 * >>> let mongo registers {n} logs pages
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
