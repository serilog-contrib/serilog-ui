import { screen } from '@testing-library/react';
import { UserEvent } from '@testing-library/user-event';
import { beforeEach, describe, expect, it } from 'vitest';

describe('interactions with the Auth JWT Modal', () => {
  let user: UserEvent;
  const getAuthDialog = () =>
    screen.queryByRole('dialog', {
      name: 'Login Modal',
    });

  beforeEach(() => {
    // TODO
    user = window.userEventLibApi as unknown as UserEvent;
  });

  it('opens the modal', async () => {
    expect(getAuthDialog()).toBeNull();

    await user.click(await screen.findByText(/Authorize/i));

    const openDialog = getAuthDialog();
    expect(openDialog).toBeVisible();
    expect(openDialog).not.toBeNull();
  });

  it('closes the modal clicking the X', async () => {
    await user.click(await screen.findByText(/Authorize/i));
    const authDialog = getAuthDialog();

    expect(authDialog).toBeVisible();

    await user.click(screen.getByRole('button', { name: 'Close Auth Modal' }));

    expect(authDialog).not.toBeVisible();
  });
});
