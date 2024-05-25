import { render, screen, userEvent } from '__tests__/_setup/testing-utils';
import JwtModal from 'app/components/Authorization/JwtModal';
import { IAuthPropertiesStorageKeys } from 'app/util/auth';
import { byLabelText, byRole } from 'testing-library-selector';
import { AuthType } from 'types/types';
import { afterEach, describe, expect, it, vi } from 'vitest';

const ui = {
  jwt: byLabelText(/JWT Token \*/i),
  save: byRole('button', { name: 'Save' }),
  change: byRole('button', { name: 'Change Token' }),
  close: byRole('button', { name: 'Close' }),
};

describe('Jwt Modal', () => {
  const jwt = () => ui.jwt.get();

  afterEach(() => {
    sessionStorage.clear();
  });

  it('renders without saved info', async () => {
    const closeMock = vi.fn();
    render(<JwtModal onClose={closeMock} />, AuthType.Jwt);

    expect(screen.getAllByRole('button')).toHaveLength(2);

    [jwt()].forEach((val) => {
      expect(val).toBeInTheDocument();
      expect(val).not.toBeDisabled();
    });

    await userEvent.click(ui.close.get());
    expect(closeMock).toHaveBeenCalled();
  });

  it('renders with storage session info', () => {
    sessionStorage.setItem(IAuthPropertiesStorageKeys.jwt_bearerToken, 'my jwt token');

    render(<JwtModal onClose={vi.fn()} />, AuthType.Jwt);

    expect(jwt()).toHaveValue('my jwt token');
    expect(jwt()).toBeEmptyDOMElement();

    [jwt()].forEach((val) => {
      expect(val).toBeDisabled();
    });
  });

  it('updates inputs, without saving them to the session storage', async () => {
    const {} = render(<JwtModal onClose={vi.fn()} />, AuthType.Jwt);

    await userEvent.type(jwt(), 'my test password');

    expect(jwt()).toHaveValue('my test password');

    expect(sessionStorage.getItem(IAuthPropertiesStorageKeys.jwt_bearerToken)).toBeNull();
  });

  it('saves inputs value to the session storage', async () => {
    render(<JwtModal onClose={vi.fn()} />, AuthType.Jwt);

    await userEvent.type(jwt(), 'my test jwt');

    await userEvent.click(ui.save.get());

    expect(sessionStorage.getItem(IAuthPropertiesStorageKeys.jwt_bearerToken)).toBe(
      'my test jwt',
    );

    expect(ui.change.get()).toBeInTheDocument();
    expect(ui.save.query()).not.toBeInTheDocument();

    [jwt()].forEach((val) => {
      expect(val).toBeDisabled();
    });
  });

  it('resets saved input and clears them from the session storage', async () => {
    render(<JwtModal onClose={vi.fn()} />, AuthType.Jwt);

    await userEvent.type(jwt(), 'my test jwt');

    await userEvent.click(ui.save.get());

    expect(sessionStorage.getItem(IAuthPropertiesStorageKeys.jwt_bearerToken)).toBe(
      'my test jwt',
    );

    await userEvent.click(ui.change.get());

    expect(sessionStorage.getItem(IAuthPropertiesStorageKeys.jwt_bearerToken)).toBeNull();

    expect(ui.save.query()).toBeInTheDocument();
    expect(ui.change.query()).not.toBeInTheDocument();

    [jwt()].forEach((val) => {
      expect(val).toBeEmptyDOMElement();
      expect(val).not.toBeDisabled();
    });
  });
});
