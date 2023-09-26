import { createRoot } from 'react-dom/client';
import App from './Components/App';

// Render your React component
const main = async () => {
  const rootItem = document.getElementById('serilog-ui-app');
  if (rootItem == null)
    throw new Error(
      'React app not found. Are you sure you loaded correctly the HTML content?',
    );

  const root = createRoot(rootItem);

  // attach msw on development
  if (process.env.NODE_ENV === 'development') {
    const { worker } = await import('../__tests__/util/mocks/msw-worker');
    try {
      await worker.start();
    } catch (err) {
      console.error(err);
    }
  }

  root.render(<App />);
};

void main();
