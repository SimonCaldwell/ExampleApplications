<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>

  <xsl:template match="cXML/Header"/>

  <xsl:template match="cXML/Message/PunchOutOrderMessage">
    <PurchaseOrder>
      <PurchaseOrderHeader>
        <SessionID>
          <xsl:value-of select="BuyerCookie"/>
        </SessionID>
        <Currency>
          <xsl:value-of select="PunchOutOrderMessageHeader/Total/Money/@currency"/>
        </Currency>
        <TotalValue>
          <xsl:value-of select="PunchOutOrderMessageHeader/Total/Money"/>
        </TotalValue>
      </PurchaseOrderHeader>
      <PunchOutDetail>
        <xsl:for-each select="ItemIn">
          <Item>
            <Product>
              <xsl:value-of select="ItemID/SupplierPartID"/>
            </Product>
            <Quantity>
              <xsl:value-of select="@quantity"/>
            </Quantity>
            <Description>
              <xsl:value-of select="ItemDetail/Description"/>
            </Description>
            <UnitOfMeasure>
              <xsl:value-of select="ItemDetail/UnitOfMeasure"/>
            </UnitOfMeasure>
            <UnitValue>
              <xsl:value-of select="ItemDetail/UnitPrice/Money"/>
            </UnitValue>
            <Currency>
              <xsl:value-of select="ItemDetail/UnitPrice/Money/@currency"/>
            </Currency>
            <UNSPSCCode>
              <xsl:value-of select="ItemDetail/Classification[@domain='UNSPSC']"/>
            </UNSPSCCode>
            <Supplier>
              <xsl:value-of select="SupplierID"/>
            </Supplier>


            <References>
              <xsl:if test="ItemID/SupplierPartAuxiliaryID">
                <Reference name='PartID'>
                  <xsl:value-of select="ItemID/SupplierPartAuxiliaryID"/>
                </Reference>
              </xsl:if>

              <xsl:if test="ItemDetail/Extrinsic[@name='Contract Reference']">
                <Reference name='ContractReference'>
                  <xsl:value-of select="ItemDetail/Extrinsic[@name='Contract Reference']"/>
                </Reference>
              </xsl:if>

            </References>

          </Item>
        </xsl:for-each>
      </PunchOutDetail>
    </PurchaseOrder>
  </xsl:template>
</xsl:stylesheet>
