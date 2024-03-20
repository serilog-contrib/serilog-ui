import '@fontsource/mononoki';
import '@mantine/core/styles.css';
import '@mantine/dates/styles.css';
import '@mantine/notifications/styles.css';
import { createRoot } from 'react-dom/client';
import App from './app/App';

/**
 * TODO:
(branch/v3) 
 * > change basic auth to receive data from ui (header) and check it with custom impl
 * [testing phase] sample with basic and jwt auth, that passes when receivin' from the modal auth header
 * >> if auth on index is active, redirect to unauth view instead of html page (react router and unhaut page...)

 * > table props not setting default when fetching data (block log if no table key is set)
 * > block api call if keys empty, block api call if ui path undefined -> send noitification if keys missg
 * > try running token check each {x} time and send notification if expired

(branch/routing-by-id)
 * >> react router setup to catch id routing
 * >> endpoint and additional model property
 * >> filter by _id (react router and opening log by itself)

(branch/custom-columns)
 * >> custom columns

(branch/clean-sample)
 * >> clean sample and install samples with each provider

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
