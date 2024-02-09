import wdyr from '@welldone-software/why-did-you-render';
import * as React from 'react';

wdyr(React, {
  collapseGroups: true,
  hotReloadBufferMs: 1000,
  include: [/app/],
  trackHooks: true,
});
