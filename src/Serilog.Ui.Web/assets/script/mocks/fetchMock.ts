import { rest } from 'msw'

export const handlers = [
    // Handles a GET /user request
    rest.get('https://127.0.0.1:1234/api/logs', (req, res, ctx) => {
        console.log('here')
        return res(ctx.status(200), ctx.json([]))
    }),
];

