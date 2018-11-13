<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:param name="newValue" select="0" />
<xsl:param name="newValue1" select="0" />

  <!-- this template copies your input XML unchanged -->
  <xsl:template match="node()|@*">
    <xsl:copy>
      <xsl:apply-templates select="node()|@*" />
    </xsl:copy>
  </xsl:template>

<xsl:template match="endpoint[@bindingConfiguration='BasicHttpBinding_IDesktopService']/@address">
        <xsl:attribute name="address">
            <xsl:value-of select="$newValue"/>
        </xsl:attribute>
 </xsl:template>
     
 

<!-- or some other node selection -->
</xsl:stylesheet>