import { FormProvider } from 'react-hook-form';
import { Outlet, RouteObject } from 'react-router';
import { ErrorBoundaryPage } from './components/ErrorPage';
import { HomePageNotAuthorized } from './components/HomePageNotAuthorized';
import { Index } from './components/Index';
import { useSearchForm } from './hooks/useSearchForm';

const Layout = () => {
  const { methods } = useSearchForm();

  return (
    <FormProvider
      // eslint-disable-next-line react/jsx-props-no-spreading
      {...methods}
    >
      <Outlet />
    </FormProvider>
  );
};

export const routes: RouteObject[] = [
  {
    ErrorBoundary: ErrorBoundaryPage,
    element: <Layout />,
    children: [
      {
        element: <Index />,
        index: true,
      },
      {
        element: <HomePageNotAuthorized />,
        path: 'access-denied/',
      },
    ],
  },
];
