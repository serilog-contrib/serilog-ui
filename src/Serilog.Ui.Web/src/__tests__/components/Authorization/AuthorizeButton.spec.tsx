import {
  render,
  screen,
  userEvent,
  waitFor,
  within,
} from '__tests__/_setup/testing-utils';
import AuthorizeButton from 'app/components/Authorization/AuthorizeButton';
import { AuthType } from 'types/types';
import { describe, expect, it } from 'vitest';

describe('AuthorizeButton', () => {
  it('renders null on custom authtype', () => {
    render(<AuthorizeButton />, AuthType.Custom);

    expect(screen.queryByRole('button')).not.toBeInTheDocument();
  });

  it('renders button with Basic modal', async () => {
    render(<AuthorizeButton />, AuthType.Basic);

    const btn = screen.getByRole('button');
    expect(btn).toBeInTheDocument();

    await userEvent.click(btn);

    const dialog = screen.getByRole('dialog');
    expect(dialog).toBeInTheDocument();

    // waiting for suspense
    await waitFor(() => {
      const basicUserTextBox = within(dialog).queryByRole('textbox', {
        name: 'Username',
      });
      expect(basicUserTextBox).toBeInTheDocument();
    });
  });

  it('renders button with JWT modal', async () => {
    render(<AuthorizeButton />, AuthType.Jwt);

    const btn = screen.getByRole('button');
    expect(btn).toBeInTheDocument();

    await userEvent.click(btn);

    const dialog = screen.getByRole('dialog');
    expect(dialog).toBeInTheDocument();

    // waiting for suspense
    await waitFor(() => {
      const jwtTextBox = within(dialog).queryByLabelText(/JWT Token \*/i);
      expect(jwtTextBox).toBeInTheDocument();
    });
  });
});
