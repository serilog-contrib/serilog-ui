import { render, screen } from '__tests__/_setup/testing-utils';
import BasicModal from 'app/components/Authorization/BasicModal';
import { describe, expect, it, vi } from 'vitest';

describe('Basic Modal', () => {
  it('renders', () => {
    render(<BasicModal onClose={vi.fn()} />);

    expect(screen.getAllByRole('button')).toHaveLength(2);
  });
});
