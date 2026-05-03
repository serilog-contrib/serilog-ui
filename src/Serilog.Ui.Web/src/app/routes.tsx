import type { RouteObject } from 'react-router';
import { ErrorBoundaryPage } from './components/ErrorPage';
import { HomePageNotAuthorized } from './components/HomePageNotAuthorized';
import { Index } from './components/Index';
import Layout from './Layout';

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
