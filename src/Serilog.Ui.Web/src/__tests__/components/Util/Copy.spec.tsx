import {
  renderSerilogUiTestWrapper,
  screen,
  userEvent,
} from '__tests__/_setup/testing-utils';
import { CopySection } from 'app/components/Util/Copy';
import { describe, expect, it, vi } from 'vitest';

describe('copy', () => {
  it('renders', async () => {
    const spy = vi.spyOn(navigator.clipboard, 'writeText');
    renderSerilogUiTestWrapper(
      <div>
        <CopySection value="my-value" />
      </div>,
    );

    await userEvent.click(screen.getByRole('button'));

    expect(spy).toHaveBeenCalledOnce();
  });
});
