<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:param name="DataSource" select="0" />
<xsl:param name="DBOwnerPassword" select="0" />
<xsl:param name="DBBackupPath" select="0" />
<xsl:param name="DBPrefix" select="Alloy" />
<xsl:param name="DeploymentServer" select="localhost" />
<xsl:param name="ServiceBaseUrl"/>



  <!-- this template copies your input XML unchanged -->
  <xsl:template match="node()|@*">
    <xsl:copy>
      <xsl:apply-templates select="node()|@*" />
    </xsl:copy>
  </xsl:template>

 <xsl:template match="add[@key = 'DataSource']/@value">
        <xsl:attribute name="value">
            <xsl:value-of select="$DataSource"/>
        </xsl:attribute>
 </xsl:template>

 <xsl:template match="add[@key = 'DBOwnerPassword']/@value">
        <xsl:attribute name="value">
            <xsl:value-of select="$DBOwnerPassword"/>
        </xsl:attribute>
 </xsl:template>

 <xsl:template match="add[@key = 'DBBackupPath']/@value">
        <xsl:attribute name="value">
            <xsl:value-of select="$DBBackupPath"/>
        </xsl:attribute>
 </xsl:template>

  <xsl:template match="add[@key = 'DBPrifix']/@value">
        <xsl:attribute name="value">
            <xsl:value-of select="$DBPrefix"/>
        </xsl:attribute>
 </xsl:template>

 <xsl:template match="add[@key = 'DeploymentServer']/@value">
        <xsl:attribute name="value">
            <xsl:value-of select="$DeploymentServer"/>
        </xsl:attribute>
 </xsl:template>

 <xsl:template match="add[@key = 'ServiceBaseUrl']/@value">
        <xsl:attribute name="value">
            <xsl:value-of select="$ServiceBaseUrl"/>
        </xsl:attribute>
 </xsl:template> 
 
 <!-- or some other node selection -->
</xsl:stylesheet>