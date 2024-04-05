import { Box, Loader } from '@mantine/core';
import { renderCodeContent } from 'app/util/prettyPrints';
import { useEffect, useState } from 'react';
import classes from 'style/table.module.css';
import { LogType } from 'types/types';

const CodeContent = ({ prop, codeType }: { prop: string; codeType: LogType }) => {
  const [codeContent, setCodeContent] = useState<TrustedHTML>('');

  useEffect(() => {
    const fetchContent = async () => {
      if (!codeContent && prop) {
        const content = await renderCodeContent(prop as string, codeType);
        setCodeContent(content ?? '');
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
