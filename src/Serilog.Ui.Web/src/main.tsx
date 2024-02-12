import '@fontsource-variable/open-sans';
import '@fontsource/mononoki';
import '@mantine/code-highlight/styles.css';
import '@mantine/core/styles.css';
import '@mantine/dates/styles.css';
import '@mantine/notifications/styles.css';
import { createRoot } from 'react-dom/client';
import App from './app/App';

/**
 * TODO:
 * - change basic to receive data from ui...
 * - filter by _id
 * - sort by option
 * - better colors?
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
    const { worker } = await import('./__tests__/__setup__/msw-worker');
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
