import { dbKeysMock } from '__tests__/_setup/mocks/samples';
import {
  act,
  fireEvent,
  render,
  screen,
  userEvent,
  waitForElementToBeRemoved,
  within,
} from '__tests__/_setup/testing-utils';
import Search from 'app/components/Search/Search';
import { searchFormInitialValues } from 'app/hooks/useSearchForm';
import * as logs from 'app/queries/logs';
import { IAuthPropertiesStorageKeys } from 'app/util/auth';
import dayjs from 'dayjs';
import objectSupport from 'dayjs/plugin/objectSupport';
import { byLabelText, byRole } from 'testing-library-selector';
import { AuthType, DispatchedCustomEvents } from 'types/types';
import { beforeAll, describe, expect, it, vi } from 'vitest';

dayjs.extend(objectSupport);

const headers = () => {
  const head = new Headers();
  head.append('authorization', 'test');
  return head;
};
vi.mock('../../../app/hooks/useAuthProperties', () => ({
  useAuthProperties: () => ({
    fetchInfo: {
      headers: { headers: headers() },
      routePrefix: '',
    },
    isHeaderReady: true,
  }),
}));

const ui = {
  textbox: (name: string) => byRole('textbox', { name }),
  listbox: byRole('listbox'),
  options: (listbox: ReturnType<typeof byRole>) => byRole('option').getAll(listbox.get()),
  start_date: byRole('button', { name: 'Start date' }),
  end_date: byRole('button', { name: 'End date' }),
  day_btn: (name: string) => byRole('button', { name }),
  time_btn: (label: string) => byLabelText<HTMLInputElement>(label),
  clear: byLabelText('reset filters'),
  submit: byRole('button', { name: 'Submit' }),
};
const sampleDate = dayjs({
  day: 10,
  month: dayjs().month(),
  hour: 15,
  minutes: 15,
  seconds: 30,
});

describe('Search', () => {
  const selectTable = async () => {
    const tableInput = ui.textbox('Table').get();

    await userEvent.click(tableInput);

    const selectOption = ui.options(ui.listbox)[0];

    await userEvent.selectOptions(ui.listbox.get(), selectOption);
  };

  beforeAll(() => {
    sessionStorage.setItem(IAuthPropertiesStorageKeys.jwt_bearerToken, 'token');
  });

  it('renders correctly', () => {
    render(<Search onRefetch={vi.fn()} />);

    expect(screen.getByRole('form', { name: 'search-logs-form' })).toBeInTheDocument();
  });

  describe('fields', () => {
    it('fetch with selected table', async () => {
      const spy = vi.spyOn(logs, 'fetchLogs');

      render(<Search onRefetch={vi.fn()} />, AuthType.Jwt);

      await selectTable();

      expect(spy).toHaveBeenLastCalledWith(
        expect.objectContaining({
          table: dbKeysMock[0],
        }),
        expect.any(Object),
        '',
      );
    });

    it('fetch with selected level', async () => {
      const spy = vi.spyOn(logs, 'fetchLogs');

      render(<Search onRefetch={vi.fn()} />, AuthType.Jwt);

      await selectTable();

      const levelInput = ui.textbox('Level').get();

      await userEvent.click(levelInput);

      const selectOption = ui.options(ui.listbox)[2];

      await userEvent.selectOptions(ui.listbox.get(), selectOption);

      await userEvent.click(ui.submit.get());

      expect(spy).toHaveBeenLastCalledWith(
        expect.objectContaining({
          table: dbKeysMock[0],
          level: selectOption.getAttribute('value'),
        }),
        expect.any(Object),
        '',
      );
    });

    it('fetch with selected text search', async () => {
      const spy = vi.spyOn(logs, 'fetchLogs');

      render(<Search onRefetch={vi.fn()} />, AuthType.Jwt);

      await selectTable();

      const levelInput = ui.textbox('Search').get();

      await userEvent.type(levelInput, 'my search');

      await userEvent.click(ui.submit.get());

      expect(spy).toHaveBeenLastCalledWith(
        expect.objectContaining({
          table: dbKeysMock[0],
          search: 'my search',
        }),
        expect.any(Object),
        '',
      );
    });

    it('fetch with selected start date', async () => {
      const spy = vi.spyOn(logs, 'fetchLogs');

      render(<Search onRefetch={vi.fn()} />, AuthType.Jwt);

      await selectTable();

      // open end date modal
      await userEvent.click(ui.start_date.get());

      // click sample day button
      await userEvent.click(ui.day_btn(sampleDate.format('DD MMMM YYYY')).get());
      // click sample time button
      // using fireEvent due to userEvent.type not supporting seconds
      // ref https://github.com/testing-library/user-event/blob/d0362796a33c2d39713998f82ae309020c37b385/tests/event/input.ts#L298
      fireEvent.change(ui.time_btn('start-time-input').get(), {
        target: { value: '15:15:30' },
      });

      // click submit date button
      const submitBtn = within(screen.getByRole('dialog'))
        .getAllByRole('button')
        .slice(-1);
      await userEvent.click(submitBtn[0]);
      await waitForElementToBeRemoved(screen.queryByRole('dialog'));

      // submit request
      await userEvent.click(ui.submit.get());

      expect(spy).toHaveBeenLastCalledWith(
        expect.objectContaining({
          table: dbKeysMock[0],
          startDate: expect.toBeSameDate(sampleDate, { unit: 'seconds' }),
        }),
        expect.any(Object),
        '',
      );
    });

    it('fetch with selected end date', async () => {
      const spy = vi.spyOn(logs, 'fetchLogs');

      render(<Search onRefetch={vi.fn()} />, AuthType.Jwt);

      await selectTable();

      // open end date modal
      await userEvent.click(ui.end_date.get());

      // click sample day button
      await userEvent.click(ui.day_btn(sampleDate.format('DD MMMM YYYY')).get());
      // click sample time button
      // using fireEvent due to userEvent.type not supporting seconds
      // ref https://github.com/testing-library/user-event/blob/d0362796a33c2d39713998f82ae309020c37b385/tests/event/input.ts#L298
      fireEvent.change(ui.time_btn('end-time-input').get(), {
        target: { value: '15:15:30' },
      });

      // click submit date button
      const submitBtn = within(screen.getByRole('dialog'))
        .getAllByRole('button')
        .slice(-1);
      await userEvent.click(submitBtn[0]);
      await waitForElementToBeRemoved(screen.queryByRole('dialog'));

      // submit request
      await userEvent.click(ui.submit.get());

      expect(spy).toHaveBeenCalledWith(
        expect.objectContaining({
          table: dbKeysMock[0],
          endDate: expect.toBeSameDate(sampleDate, { unit: 'seconds' }),
        }),
        expect.any(Object),
        '',
      );
    });
  });

  it('refetches automatically when page is more than one', async () => {
    const spy = vi.spyOn(logs, 'fetchLogs');
    searchFormInitialValues.page = 2;

    render(<Search onRefetch={vi.fn()} />, AuthType.Jwt);

    await selectTable();
    const levelInput = ui.textbox('Search').get();
    await userEvent.type(levelInput, 'my search');
    await userEvent.click(ui.submit.get());

    expect(spy).toHaveBeenNthCalledWith(
      1,
      expect.objectContaining({
        table: dbKeysMock[0],
        search: '',
        page: 2,
      }),
      expect.any(Object),
      '',
    );
    expect(spy).toHaveBeenNthCalledWith(
      2,
      expect.objectContaining({
        table: dbKeysMock[0],
        search: 'my search',
        level: null,
        page: 1,
      }),
      expect.any(Object),
      '',
    );
    searchFormInitialValues.page = 1;
  });

  it('invokes onRefetch on submit', async () => {
    const spy = vi.spyOn(logs, 'fetchLogs');

    const onRefetchMock = vi.fn();
    render(<Search onRefetch={onRefetchMock} />, AuthType.Jwt);

    await selectTable();

    const levelInput = ui.textbox('Search').get();

    await userEvent.type(levelInput, 'my search');

    await userEvent.click(ui.submit.get());

    expect(spy).toHaveBeenNthCalledWith(
      2,
      expect.objectContaining({
        table: dbKeysMock[0],
        search: 'my search',
        level: null,
      }),
      expect.any(Object),
      '',
    );
    expect(onRefetchMock).toHaveBeenCalledOnce();
  });

  it('invokes reset on RemoveTableKey event, removing the table value', async () => {
    const tableInput = ui.textbox('Table').get;
    render(<Search onRefetch={vi.fn()} />, AuthType.Jwt);

    await selectTable();
    expect(tableInput()).toHaveValue(dbKeysMock[0]);

    act(() => {
      document.dispatchEvent(new CustomEvent(DispatchedCustomEvents.RemoveTableKey));
    });

    expect(tableInput()).toHaveValue('');
  });

  it('clean inputs calling refetch', async () => {
    const spy = vi.spyOn(logs, 'fetchLogs');

    render(<Search onRefetch={vi.fn()} />, AuthType.Jwt);

    await selectTable();

    const levelInput = ui.textbox('Search').get();

    await userEvent.type(levelInput, 'my search');

    await userEvent.click(ui.submit.get());

    expect(spy).toHaveBeenNthCalledWith(
      2,
      expect.objectContaining({
        table: dbKeysMock[0],
        search: 'my search',
        level: null,
      }),
      expect.any(Object),
      '',
    );

    await userEvent.click(ui.clear.get());

    expect(spy).toHaveBeenNthCalledWith(
      3,
      expect.objectContaining({
        table: dbKeysMock[0],
        search: '',
        level: null,
      }),
      expect.any(Object),
      '',
    );
  });
});
