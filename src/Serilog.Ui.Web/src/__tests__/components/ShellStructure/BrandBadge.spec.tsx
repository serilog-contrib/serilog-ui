import { render, screen } from '__tests__/_setup/testing-utils';
import BrandBadge from 'app/components/ShellStructure/BrandBadge';
import dayjs from 'dayjs';
import { describe, expect, it, vi } from 'vitest';

const propsMock = {
  showBrand: true,
};
vi.mock('../../../app/hooks/useSerilogUiProps', () => {
  return {
    useSerilogUiProps: () => propsMock,
  };
});

describe('BrandBadge', () => {
  it('renders', () => {
    render(<BrandBadge size="sm" />);

    expect(screen.getByText('Serilog UI')).toBeInTheDocument();
    expect(screen.getByText(dayjs().year())).toBeInTheDocument();
  });

  it('not render if flag is false', () => {
    propsMock.showBrand = false;

    render(<BrandBadge size="sm" />);

    expect(screen.queryByText('Serilog UI')).not.toBeInTheDocument();
    expect(screen.queryByText(dayjs().year())).not.toBeInTheDocument();

    propsMock.showBrand = true;
  });
});
