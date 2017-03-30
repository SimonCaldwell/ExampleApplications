<!-- This is the standard transformation used to create the xml needed to connect to a suppliers cxml punchout (or marketplace) site.  -->
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:output method="xml" version="1.0" encoding="utf-8" indent="yes" omit-xml-declaration="yes" />

  <xsl:template match="/">

    <cXML>
      <xsl:attribute name="payloadID">
        <xsl:value-of select="Details/@PayloadID" />
      </xsl:attribute>
      <xsl:attribute name="timestamp">
        <xsl:value-of select="Details/@Timestamp" />
      </xsl:attribute>
      <Header>
        <From>
          <Credential domain="NetworkId">
            <Identity>
              <xsl:value-of select="Details/@PunchoutFromIdentity" />
            </Identity>
          </Credential>
        </From>
        <To>
          <Credential domain="DUNS">
            <Identity>
              <xsl:value-of select="Details/@PunchoutToIdentity" />
            </Identity>
          </Credential>
        </To>
        <Sender>
          <Credential domain="NetworkUserId">
            <Identity>
              <xsl:value-of select="Details/@PunchoutSenderIdentity" />
            </Identity>
            <SharedSecret>
              <xsl:value-of select="Details/@PunchoutSenderSharedSecret" />
            </SharedSecret>
          </Credential>

          <UserAgent>PROACTIS</UserAgent>
        </Sender>


      </Header>

      <Request deploymentMode="test">
        <PunchOutSetupRequest operation="create">
          <BuyerCookie>
            <xsl:value-of select="Details/@BuyerCookie" />
          </BuyerCookie>
          <Extrinsic name="User">
            <xsl:value-of select="Details/@LogonName" />
          </Extrinsic>
          <Extrinsic name="Email">
            <xsl:value-of select="Details/@Email" />
          </Extrinsic>

          <BrowserFormPost>
            <URL>
              <xsl:value-of select="Details/@ReturnURL" />
            </URL>
          </BrowserFormPost>
          <SupplierSetup>
            <URL>
              <xsl:value-of select="Details/@URL" />
            </URL>
          </SupplierSetup>
        </PunchOutSetupRequest>
      </Request>
    </cXML>


  </xsl:template>

</xsl:stylesheet>
