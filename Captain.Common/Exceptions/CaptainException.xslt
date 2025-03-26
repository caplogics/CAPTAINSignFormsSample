<?xml version="1.0" encoding="utf-8"?>

  <xsl:stylesheet version="1.1" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
   xmlns="http://www.w3.org/1999/xhtml"
   xmlns:fo="http://www.w3.org/1999/XSL/Format"
   xmlns:xlink="http://www.w3c.org/1999/xlink"
   xmlns:internal="http://www.ich.org"
   xmlns:fda-regional="http://www.ich.org/fda"
   xmlns:msxsl="urn:schemas-microsoft-com:xslt"
   xmlns:js="javascript:code">

    <xsl:output method="html" version="1.0" encoding="UTF-8" indent="yes" />
    <xsl:template match="/">
      <html>
        <body>
          <h2>Catain Exception</h2>
          <table border="1">
            <tr bgcolor="#9acd32">
              <th align="left">DateTime</th>
              <th align="left">Class</th>
              <th align="left">Method</th>
              <th align="left">LineNumber</th>
              <th align="left">ColumnNumber</th>
              <th align="left">Error</th>
              <th align="left">SeverityLevel</th>
            </tr>
            <xsl:for-each select="Exception">
              <tr>
                <td><xsl:value-of select="DateTime" /></td>
                <td><xsl:value-of select="Class" /></td>
                <td><xsl:value-of select="Method" /></td>
                <td><xsl:value-of select="LineNumber" /></td>
                <td><xsl:value-of select="ColumnNumber" /></td>
                <td><xsl:value-of select="Error" /></td>
                <td><xsl:value-of select="SeverityLevel" /></td>
              </tr>
            </xsl:for-each>
          </table>
        </body>
      </html>
    </xsl:template>
</xsl:stylesheet>