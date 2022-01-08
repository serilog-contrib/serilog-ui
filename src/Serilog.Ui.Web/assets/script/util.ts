// wip - datejs
export const formatDate = (date) => {
    const dt = new Date(date);
    return `${(dt.getMonth() + 1).toString().padStart(2, "0")}/${dt.getDate().toString().padStart(2, "0")}/${dt.getFullYear().toString().padStart(4, "0")}
            ${dt.getHours().toString().padStart(2, "0")}:${dt.getMinutes().toString().padStart(2, "0")}:${dt.getSeconds().toString().padStart(2, "0")}.${dt.getMilliseconds().toString()}`;
}

export const formatXml = (xml: string, tab = "\t") => {
    let formatted = "", indent = "";
    xml.split(/>\s*</).forEach(function (node) {
        // decrease indent by one "tab"
        if (node.match(/^\/\w/)) indent = indent.substring(tab.length);
        formatted += indent + "<" + node + ">\r\n";
        // increase indent
        if (node.match(/^<?\w[^>]*[^\/]$/)) indent += tab;
    });
    return formatted.substring(1, formatted.length - 3);
}

export const fixedLengthMessageWithModal = (str: string, sliceEnd: number) => {
    if (str.length <= sliceEnd) {
        return str;
    }

    const truncated = str.slice(0, sliceEnd) + "...";
    const html = `<a href="#" title="Click to view" class="modal-trigger" data-type="text">
                    ${truncated}
                    <span style=\"display: none\">${str}</span>
                  </a>`;
    return html;
}

export const cleanHtmlTags = (str: string) => {
    return String(str).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;');
}
