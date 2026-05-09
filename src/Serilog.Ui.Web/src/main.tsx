import { SerilogUiPropsProvider } from 'app/contexts/SerilogUiPropsProvider';
import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import App from './app/App';
import '@fontsource/mononoki';
import '@mantine/core/styles.css';
import '@mantine/dates/styles.css';
import '@mantine/notifications/styles.css';

const runMsw = async () => {
  const { worker } = await import('./__tests__/_setup/mocks/msw-worker');
  try {
    await worker.start({
      onUnhandledRequest: 'bypass',
    });
  } catch (err) {
    console.error(err);
    throw err;
  }
};

const main = async () => {
  const rootItem = document.getElementById('serilog-ui-app');
  if (rootItem == null) {
    throw new Error(
      'React app not found. Are you sure you loaded the HTML content correctly?',
    );
  }

  const root = createRoot(rootItem);

  // attach msw on development
  if (import.meta.env.MODE === 'development') {
    await runMsw();
  }

  root.render(
    <StrictMode>
      <SerilogUiPropsProvider>
        <App />
      </SerilogUiPropsProvider>
    </StrictMode>,
  );
};

void main();
