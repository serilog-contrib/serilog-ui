// `shikiji/core` entry does not include any themes or languages or the wasm binary.
import { createOnigurumaEngine } from 'shiki';
import { createHighlighterCore } from 'shiki/core';

// directly import the theme and language modules, only the ones you imported will be bundled.
export const highlighter = await createHighlighterCore({
  engine: await createOnigurumaEngine(import('shiki/wasm')),
  themes: [
    // or a dynamic import if you want to do chunk splitting
    import('shiki/themes/tokyo-night.mjs'),
    import('shiki/themes/night-owl.mjs'),
  ],
  langs: [
    import('shiki/langs/json.mjs'),
    import('shiki/langs/json5.mjs'),
    import('shiki/langs/xml.mjs'),
    // shikiji will try to interop the module with the default export
    () => import('shiki/langs/css.mjs'),
  ],
});
