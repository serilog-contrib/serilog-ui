import '@fontsource/mononoki';
import '@mantine/core/styles.css';
import '@mantine/dates/styles.css';
import '@mantine/notifications/styles.css';
import { createRoot } from 'react-dom/client';
import App from './app/App';

const runMsw = async () => {
  const { worker } = await import('./__tests__/_setup/msw-worker');
  try {
    await worker.start({
      onUnhandledRequest: (req, print) => {
        const excludedPaths = ['/@fs/', '/app/', '/style/'];
        const url = new URL(req.url);
        if (excludedPaths.some((path) => url.pathname.startsWith(path))) {
          return;
        }

        print.warning();
      },
    });
  } catch (err) {
    console.error(err);
    throw err;
  }
};

const main = async () => {
  const rootItem = document.getElementById('serilog-ui-app');
  if (rootItem == null)
    throw new Error(
      'React app not found. Are you sure you loaded the HTML content correctly?',
    );

  const root = createRoot(rootItem);

  // attach msw on development
  if (import.meta.env.MODE === 'development') {
    await runMsw();
  }

  root.render(<App />);
};

void main();
