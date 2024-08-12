import { render, screen, userEvent } from '__tests__/_setup/testing-utils';
import { CopySection } from 'app/components/Util/Copy';
import { describe, expect, it, vi } from 'vitest';

describe('Copy', () => {
  it('renders', async () => {
    const spy = vi.spyOn(navigator.clipboard, 'writeText');
    render(
      <div>
        <CopySection value="my-value" />
      </div>,
    );

    await userEvent.click(screen.getByRole('button'));

    expect(spy).toHaveBeenCalledOnce();
  });
});
