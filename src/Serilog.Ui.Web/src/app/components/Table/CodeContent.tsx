import { Box, Loader } from '@mantine/core';
import { renderCodeContent } from 'app/util/prettyPrints';
import { useEffect, useState } from 'react';
import classes from 'style/table.module.css';
import { LogType } from 'types/types';

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

  return (
    <div
      dangerouslySetInnerHTML={{ __html: codeContent }}
      className={classes.detailModalCode}
    ></div>
  );
};

export default CodeContent;
