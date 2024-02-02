import { printXmlCode } from 'app/util/prettyPrints';

export const renderContent = (contentType: string, modalContent: string) => {
  if (contentType === 'xml') return printXmlCode(contentType);
  if (contentType === 'json') {
    try {
      return JSON.stringify(JSON.parse(modalContent), null, 2) ?? '{}';
    } catch {
      console.warn(`${modalContent} is not a valid json!`);
    }
  }
  return '{}';
};
