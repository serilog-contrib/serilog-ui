import { ActionIcon, CopyButton, Tooltip } from '@mantine/core';
import { IconCheck, IconCopy } from '@tabler/icons-react';
import { memo } from 'react';

export const CopySection = memo(({ value }: { value: string }) => (
  <CopyButton value={value}>
    {({ copied, copy }) => (
      <Tooltip label={copied ? 'Copied' : 'Copy'} withArrow position="right">
        <IconCopySection copied={copied} copy={copy} />
      </Tooltip>
    )}
  </CopyButton>
));

const IconCopySection = memo(
  ({ copied, copy }: { copied: boolean; copy: () => void }) => (
    <ActionIcon color={copied ? 'teal' : 'gray'} variant="subtle" onClick={copy}>
      {copied ? <IconCheck /> : <IconCopy />}
    </ActionIcon>
  ),
);
