<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:param name="newValue" select="0" />
<xsl:param name="newValue1" select="0" />
<xsl:param name="newValue2" select="Mitra_" />

  <!-- this template copies your input XML unchanged -->
  <xsl:template match="node()|@*">
    <xsl:copy>
      <xsl:apply-templates select="node()|@*" />
    </xsl:copy>
  </xsl:template>

 <xsl:template match="add[@key = 'CXEDataSource']/@value">
        <xsl:attribute name="value">
            <xsl:value-of select="$newValue"/>
        </xsl:attribute>
 </xsl:template>
<xsl:template match="add[@key = 'CXEPassword']/@value">
        <xsl:attribute name="value">
            <xsl:value-of select="$newValue1"/>
        </xsl:attribute>
 </xsl:template><xsl:template match="add[@key = 'CXEDatabase']/@value">
        <xsl:attribute name="value">
            <xsl:value-of select="concat($newValue2,'_CXE')"/>
        </xsl:attribute>
 </xsl:template>

<xsl:template match="add[@key = 'CXNDataSource']/@value">
        <xsl:attribute name="value">
            <xsl:value-of select="$newValue"/>
        </xsl:attribute>
 </xsl:template>
<xsl:template match="add[@key = 'CXNPassword']/@value">
        <xsl:attribute name="value">
            <xsl:value-of select="$newValue1"/>
        </xsl:attribute>
 </xsl:template><xsl:template match="add[@key = 'CXNDatabase']/@value">
        <xsl:attribute name="value">
            <xsl:value-of select="concat($newValue2,'_CXN')"/>
        </xsl:attribute>
 </xsl:template>
<xsl:template match="add[@key = 'PartnerDataSource']/@value">
        <xsl:attribute name="value">
            <xsl:value-of select="$newValue"/>
        </xsl:attribute>
 </xsl:template>
<xsl:template match="add[@key = 'PartnerDbPassword']/@value">
        <xsl:attribute name="value">
            <xsl:value-of select="$newValue1"/>
        </xsl:attribute>
 </xsl:template><xsl:template match="add[@key = 'PartnerDatabase']/@value">
        <xsl:attribute name="value">
            <xsl:value-of select="concat($newValue2,'_PTNR')"/>
        </xsl:attribute>
 </xsl:template>

<xsl:template match="add[@key = 'ComplianceDataSource']/@value">
        <xsl:attribute name="value">
            <xsl:value-of select="$newValue"/>
        </xsl:attribute>
 </xsl:template>
<xsl:template match="add[@key = 'ComplianceDbPassword']/@value">
        <xsl:attribute name="value">
            <xsl:value-of select="$newValue1"/>
        </xsl:attribute>
 </xsl:template><xsl:template match="add[@key = 'ComplianceDatabase']/@value">
        <xsl:attribute name="value">
            <xsl:value-of select="concat($newValue2,'_Compliance')"/>
        </xsl:attribute>
 </xsl:template>

<xsl:template match="add[@key = 'CatalogDataSource']/@value">
        <xsl:attribute name="value">
            <xsl:value-of select="$newValue"/>
        </xsl:attribute>
 </xsl:template>
<xsl:template match="add[@key = 'CatalogPassword']/@value">
        <xsl:attribute name="value">
            <xsl:value-of select="$newValue1"/>
        </xsl:attribute>
 </xsl:template><xsl:template match="add[@key = 'CatalogDatabase']/@value">
        <xsl:attribute name="value">
            <xsl:value-of select="concat($newValue2,'_PCAT')"/>
        </xsl:attribute>
 </xsl:template>
<!-- or some other node selection -->
</xsl:stylesheet>