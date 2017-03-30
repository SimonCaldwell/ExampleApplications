<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>

  <xsl:template match="PurchaseOrder">
    <PurchaseOrder>
      <PurchaseOrderHeader>
        <Currency>
          <xsl:value-of select="line[@number='1']/@NEW_ITEM-CURRENCY"/>
        </Currency>
      </PurchaseOrderHeader>
      <PunchOutDetail>
        <xsl:for-each select="line">
          <Item>
            <Product>
              <xsl:value-of select="@NEW_ITEM-EXT_PRODUCT_ID"/>
            </Product>
            <Quantity>
              <xsl:value-of select="@NEW_ITEM-QUANTITY"/>
            </Quantity>
            <Description>
              <xsl:value-of select="@NEW_ITEM-DESCRIPTION"/>
            </Description>
            <UnitOfMeasure>EA</UnitOfMeasure>
            <UnitValue>
              <xsl:value-of select="@NEW_ITEM-PRICE"/>
            </UnitValue>
            <Currency>
              <xsl:value-of select="@NEW_ITEM-CURRENCY"/>
            </Currency>
          </Item>
        </xsl:for-each>
      </PunchOutDetail>
    </PurchaseOrder>
  </xsl:template>
</xsl:stylesheet>
