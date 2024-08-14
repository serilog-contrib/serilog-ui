import { Box, Loader, Textarea } from '@mantine/core';
import { renderCodeContent } from 'app/util/prettyPrints';
import { useEffect, useState } from 'react';
import classes from 'style/table.module.css';
import { LogType } from 'types/types';
import { CopySection } from '../Util/Copy';

const CodeContent = ({ content, codeType }: { content: string; codeType: LogType }) => {
  const [codeContent, setCodeContent] = useState<TrustedHTML>('');

  useEffect(() => {
    const fetchContent = async () => {
      if (!codeContent && content) {
        const transformation = await renderCodeContent(content, codeType);
        setCodeContent(transformation ?? '');
      }
    };

    void fetchContent();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  if (!codeContent)
    return (
      <Box w="100%" display="flex" style={{ justifyContent: 'center' }}>
        <Loader color="grape" />
      </Box>
    );

  if ((codeType as unknown as string) === 'string')
    return (
      <Textarea
        autosize
        value={codeContent as string}
        disabled
        rightSectionPointerEvents="all"
        rightSection={<CopySection value={codeContent as string} />}
        minRows={1}
        maxRows={25}
      />
    );

  return (
    <div
      dangerouslySetInnerHTML={{ __html: codeContent }}
      className={classes.detailModalCode}
    ></div>
  );
};

export default CodeContent;
