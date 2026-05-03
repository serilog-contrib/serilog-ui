import { AuthPropertiesProvider } from 'app/contexts/AuthPropertiesProvider';
import { FormProvider } from 'react-hook-form';
import { Outlet } from 'react-router';
import { useSearchForm } from './hooks/useSearchForm';

const Layout = () => {
  const { methods } = useSearchForm();

  return (
    <AuthPropertiesProvider>
      <FormProvider {...methods}>
        <Outlet />
      </FormProvider>
    </AuthPropertiesProvider>
  );
};

export default Layout;
