import { dbKeysMock, fakeColumnsInfo, fakeLogs } from '__tests__/_setup/mocks/samples';
import {
  render,
  screen,
  userEvent,
  waitFor,
  waitForElementToBeRemoved,
} from '__tests__/_setup/testing-utils';
import PropertiesModal from 'app/components/Table/PropertiesModal';
import { describe, expect, it, vi } from 'vitest';

const dbKey = dbKeysMock[0];
const logExample = fakeLogs.logs[0];
const mockForm = {
  getValues: vi.fn(),
};
vi.mock('../../../app/hooks/useSearchForm', () => ({
  useSearchForm: () => mockForm,
}));
vi.mock('../../../app/hooks/useSerilogUiProps', () => {
  return {
    useSerilogUiProps: () => ({
      columnsInfo: fakeColumnsInfo,
      isUtc: true,
    }),
  };
});

describe('PropertiesModal', () => {
  it('renders', () => {
    render(
      <PropertiesModal
        modalContent={{
          level: '',
          message: '',
          propertyType: '',
          rowNo: 1,
          timestamp: '',
        }}
        title="props"
      />,
    );

    const btn = screen.getByRole('button', { name: 'props' });
    expect(btn).toBeInTheDocument();
    expect(btn).toBeDisabled();
  });

  it('opens and close properties modal', async () => {
    render(<PropertiesModal modalContent={logExample} title="title" />);

    const btn = screen.getByRole('button', { name: 'title' });
    await userEvent.click(btn);

    expect(screen.getByRole('dialog')).toBeInTheDocument();

    const btnClose = screen.getByRole('button', { name: 'close-properties-modal' });
    await userEvent.click(btnClose);
    await waitForElementToBeRemoved(screen.queryByRole('dialog'));
  });

  it('show additional column: boolean', async () => {
    mockForm.getValues.mockReturnValue(dbKey);

    render(<PropertiesModal modalContent={logExample} title="title" />);

    const btn = screen.getByRole('button', { name: 'title' });
    await userEvent.click(btn);

    expect(screen.getByRole('switch', { name: 'SampleBool' })).toBeInTheDocument();
  });

  it('show additional column: dates', async () => {
    mockForm.getValues.mockReturnValue(dbKey);

    render(<PropertiesModal modalContent={logExample} title="title" />);

    const btn = screen.getByRole('button', { name: 'title' });
    await userEvent.click(btn);

    const date = screen.getByRole<HTMLInputElement>('textbox', {
      name: /sampledate/i,
    });
    expect(date).toBeInTheDocument();
    expect(date).toHaveValue((logExample['SampleDate'] as object)['value']);
  });

  it('show additional column: code', async () => {
    mockForm.getValues.mockReturnValue(dbKey);

    render(<PropertiesModal modalContent={logExample} title="title" />);

    const btn = screen.getByRole('button', { name: 'title' });
    await userEvent.click(btn);

    const codeBtn = screen.getByRole('button', { name: 'SampleCode' });
    expect(codeBtn).toBeInTheDocument();

    await userEvent.click(codeBtn);
    await waitFor(() => {
      expect(screen.getByRole('code')).toBeInTheDocument();
    });
  });

  it('show additional column: text (default)', async () => {
    mockForm.getValues.mockReturnValue(dbKey);

    render(<PropertiesModal modalContent={logExample} title="title" />);

    const btn = screen.getByRole('button', { name: 'title' });
    await userEvent.click(btn);

    const text = screen.getByRole('textbox', { name: 'SampleText' });
    expect(text).toBeInTheDocument();
  });

  it('not show properties column even if available in log, if column is removed', async () => {
    mockForm.getValues.mockReturnValue(dbKeysMock[1]);

    render(<PropertiesModal modalContent={logExample} title="title" />);

    const btn = screen.getByRole('button', { name: 'title' });
    await userEvent.click(btn);

    const properties = screen.queryByRole('textbox', { name: 'Properties' });
    expect(properties).not.toBeInTheDocument();
  });
});
