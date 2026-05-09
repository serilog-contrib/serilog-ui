import {
  renderSerilogUiTestWrapper,
  screen,
  userEvent,
  waitFor,
  waitForElementToBeRemoved,
} from '__tests__/_setup/testing-utils';
import DetailsModal from 'app/components/Table/DetailsModal';
import { describe, expect, it } from 'vitest';

describe('detailsModal', () => {
  it('renders', () => {
    renderSerilogUiTestWrapper(
      <DetailsModal
        buttonTitle='title'
        modalContent='my-content'
        modalTitle='modal'
        disabled
      />,
    );

    const btn = screen.getByRole('button', { name: 'Title' });
    expect(btn).toBeInTheDocument();
    expect(btn).toBeDisabled();
  });

  it('opens and close details modal with content', async () => {
    renderSerilogUiTestWrapper(
      <DetailsModal
        buttonTitle='title'
        modalContent='my-content'
        modalTitle='modal'
      />,
    );

    const btn = screen.getByRole('button', { name: 'Title' });
    await userEvent.click(btn);

    await waitFor(() => {
      expect(screen.getByText('my-content')).toBeInTheDocument();
    });

    const btnClose = screen.getByRole('button', {
      name: 'close-details-modal',
    });
    expect(screen.getByRole('dialog')).toBeInTheDocument();

    await userEvent.click(btnClose);
    expect(screen.queryByRole('dialog')).not.toBeInTheDocument();
  });
});
