(function ($) {
    "use strict";

    let fullHeight = function () {
        $(".js-fullheight").css("height", $(window).height());
        $(window).resize(function () {
            $(".js-fullheight").css("height", $(window).height());
        });
    };
    fullHeight();

    $("#sidebarCollapse").on("click", function () {
        $("#sidebar").toggleClass("active");
    });

    $(".page-link").on("click", function (e) {
        e.preventDefault();
        $("#page").val($(this).attr("data-val"));
        $("form").submit()
    });

    $("#logCount").on("change", function () {
        $("#page").val("1");
        $("form").submit()
    });

    $("#logFilter").on("change", function () {
        $("#page").val("1");
        $("form").submit()
    });

    $("#search").on("keypress", function (e) {
        $("#page").val("1");
        if (e.which === 13) {
            $("form").submit();
        }
    });

    $(".modal-trigger").on("click", function (e) {
        e.preventDefault();
        let message = $(this).find("span").text();
        const isXml = $(this).attr("data-type") === "xml";
        if (isXml)
            message = formatXml(message);

        const modal = $("#messageModal");
        modal.find('.modal-body pre').text(message);
        modal.modal("show");
    });

    function formatXml(xml) {
        const xmlDoc = new DOMParser().parseFromString(xml, 'application/xml');
        const xsltDoc = new DOMParser().parseFromString([
            // describes how we want to modify the XML - indent everything
            '<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform">',
            '  <xsl:strip-space elements="*"/>',
            '  <xsl:template match="para[content-style][not(text())]">', // change to just text() to strip space in text nodes
            '    <xsl:value-of select="normalize-space(.)"/>',
            '  </xsl:template>',
            '  <xsl:template match="node()|@*">',
            '    <xsl:copy><xsl:apply-templates select="node()|@*"/></xsl:copy>',
            '  </xsl:template>',
            '  <xsl:output indent="yes"/>',
            '</xsl:stylesheet>',
        ].join('\n'), 'application/xml');

        const xsltProcessor = new XSLTProcessor();
        xsltProcessor.importStylesheet(xsltDoc);

        const resultDoc = xsltProcessor.transformToDocument(xmlDoc);
        const resultXml = new XMLSerializer().serializeToString(resultDoc);

        return resultXml;
    }
})(jQuery);