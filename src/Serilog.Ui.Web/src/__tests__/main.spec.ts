/* eslint-disable testing-library/no-node-access */
import { act } from '@testing-library/react';
import { Window } from 'happy-dom';
import { afterEach, beforeEach, describe, expect, it } from 'vitest';
import { loadSerilogUiDom } from './_setup/dom-renderer';

describe('index', () => {
  let fakeHappyDomWindow: Window;
  beforeEach(async () => {
    await act(async () => {
      fakeHappyDomWindow = await loadSerilogUiDom();
    });
  });

  afterEach(async () => {
    fakeHappyDomWindow.happyDOM.abort();
    fakeHappyDomWindow.close();
  });

  it('loads the index with the expected react app id', async () => {
    expect(
      fakeHappyDomWindow.document.getElementById('serilog-ui-app'),
    ).toBeInTheDocument();
  });
});
