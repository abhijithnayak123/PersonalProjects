<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:param name="MoneyGramBillerFolderPath" select="0" />
<xsl:param name="MGBillerArchiveFolderPath" select="0" />

  <!-- this template copies your input XML unchanged -->
  <xsl:template match="node()|@*">
    <xsl:copy>
      <xsl:apply-templates select="node()|@*" />
    </xsl:copy>
  </xsl:template>

 <xsl:template match="add[@key = 'MoneyGramBillerFolderPath']/@value">
        <xsl:attribute name="value">
            <xsl:value-of select="$MoneyGramBillerFolderPath"/>
        </xsl:attribute>
 </xsl:template>

 <xsl:template match="add[@key = 'MGBillerArchiveFolderPath']/@value">
        <xsl:attribute name="value">
            <xsl:value-of select="$MGBillerArchiveFolderPath"/>
        </xsl:attribute>
 </xsl:template>
 

<!-- or some other node selection -->
</xsl:stylesheet>