import { dbKeysMock } from '__tests__/_setup/mocks/samples';
import {
  getAllByRole,
  render,
  screen,
  userEvent,
  waitForElementToBeRemoved,
  within,
} from '__tests__/_setup/testing-utils';
import Search from 'app/components/Search/Search';
import * as logs from 'app/queries/logs';
import { IAuthPropertiesStorageKeys } from 'app/util/auth';
import dayjs from 'dayjs';
import objectSupport from 'dayjs/plugin/objectSupport';
import { byLabelText, byRole } from 'testing-library-selector';
import { AuthType } from 'types/types';
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
  }),
}));

const ui = {
  textbox: (name: string) => byRole('textbox', { name }),
  listbox: byRole('listbox'),
  options: (listbox: ReturnType<typeof byRole>) => getAllByRole(listbox.get(), 'option'),
  clear: byLabelText('reset filters'),
  submit: byRole('button', { name: 'Submit' }),
  end_date: byRole('button', { name: 'End date' }),
  day_btn: (name: string) => byRole('button', { name }),
  time_btn: (label: string) => byLabelText(label),
};

describe.only('Search', () => {
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

  it('fetch with selected end date', async () => {
    const spy = vi.spyOn(logs, 'fetchLogs');

    render(<Search onRefetch={vi.fn()} />, AuthType.Jwt);

    await selectTable();

    const date = dayjs({ day: 10, month: dayjs().month(), hour: 15 });
    const endDate = ui.end_date.get();
    // open end date modal
    await userEvent.click(endDate);
    // click sample day button
    await userEvent.click(ui.day_btn(date.format('DD MMMM YYYY')).get());
    await userEvent.type(ui.time_btn('end-time-input').get(), '15');
    screen.debug(ui.time_btn('end-time-input').get());

    // click submit date button
    const submitBtn = within(screen.getByRole('dialog')).getAllByRole('button').slice(-1);
    await userEvent.click(submitBtn[0]);
    await waitForElementToBeRemoved(screen.getByRole('dialog'));

    // submit request
    await userEvent.click(ui.submit.get());

    expect(spy).toHaveBeenLastCalledWith(
      expect.objectContaining({
        table: dbKeysMock[0],
        endDate: date.toISOString(),
      }),
      expect.any(Object),
      '',
    );
  });

  it('reset inputs', async () => {
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
        level: null,
      }),
      expect.any(Object),
      '',
    );

    await userEvent.click(ui.clear.get());

    expect(spy).toHaveBeenLastCalledWith(
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
