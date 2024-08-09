import { server } from '__tests__/_setup/mocks/msw-server';
import { fetchKeys } from 'app/queries/table-keys';
import * as queryUtil from 'app/util/queries';
import { HttpResponse, http } from 'msw';
import { describe, expect, it, vi } from 'vitest';

describe('queries: table-keys', () => {
  it('returns fetched data', async () => {
    server.use(http.get('*', () => HttpResponse.json(['value'])));

    const sut = await fetchKeys({ headers: new Headers() }, 'prefix');

    expect(sut).toStrictEqual(['value']);
  });

  it('throws if request failed', async () => {
    server.use(http.get('*', () => new HttpResponse('', { status: 404 })));

    const sut = () => fetchKeys({ headers: new Headers() }, 'prefix');

    await expect(sut).rejects.toThrowError('Failed to fetch');
  });

  it('throws if request failed and send unexpected notification', async () => {
    server.use(http.get('*', () => new HttpResponse('', { status: 404 })));
    const spy = vi.spyOn(queryUtil, 'sendUnexpectedNotification');

    const sut = () => fetchKeys({ headers: new Headers() }, 'prefix', true);

    await expect(sut).rejects.toThrowError('Failed to fetch');
    expect(spy).toHaveBeenCalledOnce();
  });

  it('throws if request failed and send 403 notification', async () => {
    server.use(http.get('*', () => new HttpResponse('', { status: 403 })));
    const spy = vi.spyOn(queryUtil, 'send403Notification');

    const sut = () => fetchKeys({ headers: new Headers() }, 'prefix', true);

    await expect(sut).rejects.toThrowError('Failed to fetch');
    expect(spy).toHaveBeenCalledOnce();
  });
});
