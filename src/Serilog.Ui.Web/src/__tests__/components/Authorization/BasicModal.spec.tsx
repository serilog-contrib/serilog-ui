import { render, screen, userEvent } from '__tests__/_setup/testing-utils';
import BasicModal from 'app/components/Authorization/BasicModal';
import { IAuthPropertiesStorageKeys } from 'app/util/auth';
import { byLabelText, byRole } from 'testing-library-selector';
import { AuthType } from 'types/types';
import { afterEach, describe, expect, it, vi } from 'vitest';

const ui = {
  username: byRole('textbox', { name: 'Username' }),
  password: byLabelText(/password \*/i),
  save: byRole('button', { name: 'Save' }),
  change: byRole('button', { name: 'Change' }),
  close: byRole('button', { name: 'Close' }),
};

describe('Basic Modal', () => {
  const username = () => ui.username.get();
  const pwd = () => ui.password.get();

  afterEach(() => {
    sessionStorage.clear();
  });

  it('renders without saved info', async () => {
    const closeMock = vi.fn();
    render(<BasicModal onClose={closeMock} />, AuthType.Basic);

    expect(screen.getAllByRole('button')).toHaveLength(2);

    [username(), pwd()].forEach((val) => {
      expect(val).toBeInTheDocument();
      expect(val).not.toBeDisabled();
    });

    await userEvent.click(ui.close.get());
    expect(closeMock).toHaveBeenCalled();
  });

  it('renders with storage session info', () => {
    sessionStorage.setItem(IAuthPropertiesStorageKeys.basic_user, 'my test user');
    sessionStorage.setItem(IAuthPropertiesStorageKeys.basic_pwd, 'my test password');

    render(<BasicModal onClose={vi.fn()} />, AuthType.Basic);

    expect(username()).toHaveValue('my test user');
    expect(pwd()).toBeEmptyDOMElement();

    [username(), pwd()].forEach((val) => {
      expect(val).not.toBeDisabled();
    });
  });

  it('updates inputs, without saving them to the session storage', async () => {
    const {} = render(<BasicModal onClose={vi.fn()} />, AuthType.Basic);

    await userEvent.type(username(), 'my test user');
    await userEvent.type(pwd(), 'my test password');

    expect(username()).toHaveValue('my test user');
    expect(pwd()).toHaveValue('my test password');

    expect(sessionStorage.getItem(IAuthPropertiesStorageKeys.basic_user)).toBeNull();
    expect(sessionStorage.getItem(IAuthPropertiesStorageKeys.basic_pwd)).toBeNull();
  });

  it('saves inputs value to the session storage', async () => {
    render(<BasicModal onClose={vi.fn()} />, AuthType.Basic);

    await userEvent.type(username(), 'my test user');
    await userEvent.type(pwd(), 'my test password');

    await userEvent.click(ui.save.get());

    expect(sessionStorage.getItem(IAuthPropertiesStorageKeys.basic_user)).toBe(
      'my test user',
    );
    // basic pwd is never saved, as it's a sensitive data
    expect(sessionStorage.getItem(IAuthPropertiesStorageKeys.basic_pwd)).toBeNull();

    expect(ui.change.get()).toBeInTheDocument();
    expect(ui.save.query()).not.toBeInTheDocument();

    [username(), pwd()].forEach((val) => {
      expect(val).toBeDisabled();
    });
  });

  it('resets saved input and clears them from the session storage', async () => {
    render(<BasicModal onClose={vi.fn()} />, AuthType.Basic);

    await userEvent.type(username(), 'my test user');
    await userEvent.type(pwd(), 'my test password');

    await userEvent.click(ui.save.get());

    expect(sessionStorage.getItem(IAuthPropertiesStorageKeys.basic_user)).toBe(
      'my test user',
    );
    expect(sessionStorage.getItem(IAuthPropertiesStorageKeys.basic_pwd)).toBeNull();

    await userEvent.click(ui.change.get());

    expect(sessionStorage.getItem(IAuthPropertiesStorageKeys.basic_user)).toBeNull();
    expect(sessionStorage.getItem(IAuthPropertiesStorageKeys.basic_pwd)).toBeNull();

    expect(ui.save.query()).toBeInTheDocument();
    expect(ui.change.query()).not.toBeInTheDocument();

    [username(), pwd()].forEach((val) => {
      expect(val).toBeEmptyDOMElement();
      expect(val).not.toBeDisabled();
    });
  });
});
