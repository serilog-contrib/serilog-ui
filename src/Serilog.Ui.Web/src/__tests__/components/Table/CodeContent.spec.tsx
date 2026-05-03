import {
  renderSerilogUiTestWrapper,
  screen,
  waitFor,
} from '__tests__/_setup/testing-utils';
import CodeContent from 'app/components/Table/CodeContent';
import { LogType } from 'types/types';
import { describe, expect, it } from 'vitest';

describe('codeContent', () => {
  it('renders', async () => {
    renderSerilogUiTestWrapper(
      <CodeContent
        codeType={LogType.Json}
        content={JSON.stringify({ myProp: 'value' })}
      />,
    );

    await waitFor(() => {
      expect(screen.getByRole('code')).toBeInTheDocument();
    });
    expect(screen.getByText('myProp')).toBeInTheDocument();
  });
});
