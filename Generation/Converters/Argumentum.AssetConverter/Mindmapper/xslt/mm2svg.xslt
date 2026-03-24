<?xml version="1.0" encoding="UTF-8"?>
<!--
    MindMapper - web based mind mapping tool.
    Copyright (C) 2018 Tim Stephenson

    This program is free software; you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation; either version 2 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along
    with this program; if not, write to the Free Software Foundation, Inc.,
    51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
-->
<xsl:stylesheet version="1.0"
  xmlns="http://www.w3.org/2000/svg"
  xmlns:dc="http://www.omg.org/spec/DD/20100524/DC"
  xmlns:svg="http://www.w3.org/2000/svg"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:param name="diagramId">
    <xsl:choose>
      <xsl:when test="1=2"><xsl:value-of select="/@ID"/></xsl:when>
      <xsl:otherwise><xsl:value-of select="generate-id()"/></xsl:otherwise>
    </xsl:choose>
  </xsl:param>
  <xsl:variable name="diagramName" select="//map/@NAME"/>

  <xsl:output method="xml" omit-xml-declaration="no" indent="yes" />

  <xsl:variable name="debug">false</xsl:variable>
  <xsl:variable name="fontSize" select="14"/>
  <xsl:variable name="margin">2</xsl:variable>

  <xsl:variable name="newTab">
    M192 896v-768h768v768h-768zM896 192h-640v640h640v-640zM128 64v672l-64 64v-800h800l-64 64h-672zM352 704l160-160-192-192 96-96 192 192 160-160v416z
  </xsl:variable>

  <xsl:variable name="minX">
    <xsl:call-template name="minimum">
      <xsl:with-param name="sequence" select="//dc:Bounds/@x"/>
      <xsl:with-param name="margin">50</xsl:with-param>
    </xsl:call-template>
  </xsl:variable>
  <xsl:variable name="minY">
    <xsl:call-template name="minimum">
      <xsl:with-param name="sequence" select="//dc:Bounds/@y"/>
      <xsl:with-param name="margin">50</xsl:with-param>
    </xsl:call-template>
  </xsl:variable>
  <xsl:variable name="maxX">
    <xsl:call-template name="maximum">
      <xsl:with-param name="sequence" select="//dc:Bounds/@x"/>
      <xsl:with-param name="margin">250</xsl:with-param>
    </xsl:call-template>
  </xsl:variable>
  <xsl:variable name="maxY">
    <xsl:call-template name="maximum">
      <xsl:with-param name="sequence" select="//dc:Bounds/@y"/>
      <xsl:with-param name="margin">100</xsl:with-param>
    </xsl:call-template>
  </xsl:variable>

  <xsl:template match="/">
    <xsl:element name="svg" namespace="http://www.w3.org/2000/svg">
      <xsl:attribute name="id"><xsl:value-of select="$diagramId"/></xsl:attribute>
      <xsl:attribute name="name"><xsl:value-of select="$diagramName"/></xsl:attribute>
      <xsl:attribute name="version">1.1</xsl:attribute>
      <xsl:attribute name="viewBox">
        <xsl:value-of select="$minX"/>
        <xsl:text> </xsl:text>
        <xsl:value-of select="$minY"/>
        <xsl:text> </xsl:text>
        <!-- The first expression is a trick to get absolute value -->
        <xsl:value-of select="($minX*($minX >=0) - $minX*($minX &lt; 0))+$maxX"/>
        <xsl:text> </xsl:text>
        <xsl:value-of select="($minY*($minY >=0) - $minY*($minY &lt; 0))+$maxY"/>
      </xsl:attribute>

      <defs>
        <linearGradient id="myGradient" x1="90%" y1="100%" x2="90%" y2="90%">
          <stop offset="0%"   stop-color="green" />
          <stop offset="100%" stop-color="white" />
        </linearGradient>
      </defs>

      <style type="text/css">
        <![CDATA[
          .link { cursor: pointer; }
          ellipse.level1 { stroke-width: 8px }
          ellipse.level2 { stroke-width: 4px }
          ellipse.level3 { stroke-width: 2px }
          ellipse.level4 { stroke-width: 1px }
          path.level1 { stroke-width: 8px }
          path.level2 { stroke-width: 4px }
          path.level3 { stroke-width: 2px }
          path.level4 { stroke-width: 1px }
          rect.level1 { stroke-width: 8px }
          rect.level2 { stroke-width: 4px }
          rect.level3 { stroke-width: 2px }
          rect.level4 { stroke-width: 1px }
          text { font-family: sans-serif }
          text.level1 { font-size: 20px }
          text.level2 { font-size: 16px }
          text.level3 { font-size: 12px }
          text.level4 { font-size: 10px }
        ]]>
      </style>

      <xsl:apply-templates select="/map/node/node/edge">
        <xsl:with-param name="level"><xsl:value-of select="number(1)"/></xsl:with-param>
      </xsl:apply-templates>
      <xsl:apply-templates select="/map/node">
        <xsl:with-param name="level"><xsl:value-of select="number(1)"/></xsl:with-param>
      </xsl:apply-templates>

    </xsl:element>
  </xsl:template>

  <xsl:template match="node">
    <xsl:param name="level"/>
    <xsl:comment>Processing node level <xsl:value-of select="$level"/></xsl:comment>

    <xsl:element name="g" namespace="http://www.w3.org/2000/svg">
      <xsl:attribute name="class">
        <xsl:if test="@LINK"> link</xsl:if>
      </xsl:attribute>
      <xsl:apply-templates select="@LINK"/>
      <xsl:choose>
        <xsl:when test="@SHAPE='ellipse'">
          <xsl:element name="ellipse" namespace="http://www.w3.org/2000/svg">
            <xsl:attribute name="id"><xsl:value-of select="@ID"/></xsl:attribute>
            <xsl:attribute name="class">
              <xsl:text>node level</xsl:text>
              <xsl:value-of select="$level"/>
              <xsl:if test="@LINK"> link</xsl:if>
            </xsl:attribute>
            <xsl:attribute name="cx"><xsl:value-of select="dc:Bounds/@x+dc:Bounds/@width div 2"/></xsl:attribute>
            <xsl:attribute name="cy"><xsl:value-of select="dc:Bounds/@y+$fontSize*1.5"/></xsl:attribute>
            <xsl:attribute name="rx"><xsl:value-of select="dc:Bounds/@width div 2"/></xsl:attribute>
            <xsl:attribute name="ry"><xsl:value-of select="dc:Bounds/@height div 2"/></xsl:attribute>
            <xsl:attribute name="stroke">
              <xsl:choose>
                <xsl:when test="edge/@COLOR">
                  <xsl:value-of select="edge/@COLOR"/>
                </xsl:when>
                <xsl:otherwise>#000</xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
            <xsl:attribute name="fill">#dcdcdc</xsl:attribute>
            <!--xsl:apply-templates select="@LINK"/-->
          </xsl:element>
        </xsl:when>
        <xsl:when test="@SHAPE='rect'">
          <xsl:element name="line" namespace="http://www.w3.org/2000/svg">
            <xsl:attribute name="id"><xsl:value-of select="@ID"/></xsl:attribute>
            <xsl:attribute name="class">
              <xsl:text>node level</xsl:text>
              <xsl:value-of select="$level"/>
              <xsl:if test="@LINK"> link</xsl:if>
            </xsl:attribute>
            <xsl:attribute name="x1"><xsl:value-of select="dc:Bounds/@x"/></xsl:attribute>
            <xsl:attribute name="y1"><xsl:value-of select="dc:Bounds/@y+dc:Bounds/@height"/></xsl:attribute>
            <xsl:attribute name="x2"><xsl:value-of select="dc:Bounds/@x+dc:Bounds/@width"/></xsl:attribute>
            <xsl:attribute name="y2"><xsl:value-of select="dc:Bounds/@y+dc:Bounds/@height"/></xsl:attribute>
            <xsl:attribute name="stroke">
              <xsl:choose>
                <xsl:when test="edge/@COLOR">
                  <xsl:value-of select="edge/@COLOR"/>
                </xsl:when>
                <xsl:otherwise>#000</xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
            <xsl:attribute name="fill">#dcdcdc</xsl:attribute>
            <!--xsl:apply-templates select="@LINK"/-->
          </xsl:element>
        </xsl:when>
        <xsl:otherwise>
          <xsl:element name="rect" namespace="http://www.w3.org/2000/svg">
            <xsl:attribute name="id"><xsl:value-of select="@ID"/></xsl:attribute>
            <xsl:attribute name="class">
              <xsl:text>node level</xsl:text>
              <xsl:value-of select="$level"/>
              <xsl:if test="@LINK"> link</xsl:if>
            </xsl:attribute>
            <xsl:attribute name="x"><xsl:value-of select="dc:Bounds/@x"/></xsl:attribute>
            <xsl:attribute name="y"><xsl:value-of select="dc:Bounds/@y"/></xsl:attribute>
            <xsl:attribute name="width"><xsl:value-of select="dc:Bounds/@width"/></xsl:attribute>
            <xsl:attribute name="height"><xsl:value-of select="dc:Bounds/@height"/></xsl:attribute>
            <xsl:attribute name="rx"><xsl:value-of select="5"/></xsl:attribute>
            <xsl:attribute name="ry"><xsl:value-of select="5"/></xsl:attribute>
            <xsl:attribute name="stroke">
              <xsl:choose>
                <xsl:when test="edge/@COLOR">
                  <!--xsl:text>url(#myGradient)</xsl:text-->
                  <xsl:value-of select="edge/@COLOR"/>
                </xsl:when>
                <xsl:otherwise>#000</xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
            <xsl:attribute name="fill">white</xsl:attribute>
            <!--xsl:apply-templates select="@LINK"/-->
          </xsl:element>
        </xsl:otherwise>
      </xsl:choose>
      <xsl:element name="text" namespace="http://www.w3.org/2000/svg">
        <xsl:attribute name="class">
          <xsl:text>node-label level</xsl:text>
          <xsl:value-of select="$level"/>
          <xsl:if test="@LINK"> link</xsl:if>
        </xsl:attribute>
        <xsl:attribute name="x"><xsl:value-of select="dc:Bounds/@x+$fontSize*2"/></xsl:attribute>
        <xsl:attribute name="y"><xsl:value-of select="dc:Bounds/@y+$fontSize*1.8"/></xsl:attribute>
        <xsl:attribute name="style">
          <xsl:text>font-size:</xsl:text><xsl:value-of select="$fontSize+((5-$level)*4)"/></xsl:attribute>
        <!--xsl:apply-templates select="@LINK"/-->
        <xsl:value-of select="@TEXT"/>
      </xsl:element>
      <xsl:if test="@LINK">
        <xsl:element name="path">
          <xsl:attribute name="class">icon link</xsl:attribute>
          <xsl:attribute name="d"><xsl:value-of select="$newTab"/></xsl:attribute>
          <xsl:attribute name="transform">translate(<xsl:value-of select="number(dc:Bounds/@x)+number(dc:Bounds/@width)-10"/>,<xsl:value-of select="number(dc:Bounds/@y)+10"/>) scale(0.02) rotate(-90)</xsl:attribute>
          <!--xsl:apply-templates select="@LINK"/-->
        </xsl:element>
      </xsl:if>
    </xsl:element>

    <xsl:apply-templates select="./node">
      <xsl:with-param name="level"><xsl:value-of select="$level+1"/></xsl:with-param>
    </xsl:apply-templates>
  </xsl:template>

  <xsl:template match="edge">
    <xsl:param name="level"/>
    <xsl:comment>Processing edge level <xsl:value-of select="$level"/></xsl:comment>

    <xsl:if test="../../dc:Bounds">
      <xsl:element name="path" namespace="http://www.w3.org/2000/svg">
        <xsl:attribute name="class">
          <xsl:text>path level</xsl:text>
          <xsl:value-of select="$level"/>
        </xsl:attribute>
        <xsl:attribute name="d">
          <xsl:text>M</xsl:text>
          <xsl:value-of select="../dc:Bounds/@x+50"/>
          <xsl:text> </xsl:text>
          <xsl:value-of select="../dc:Bounds/@y+$fontSize*1.5"/>
          <xsl:text> T</xsl:text>
          <xsl:value-of select="../../dc:Bounds/@x+50"/>
          <xsl:text> </xsl:text>
          <xsl:value-of select="../../dc:Bounds/@y+$fontSize*1.5"/>
        </xsl:attribute>
        <xsl:attribute name="stroke">
          <xsl:choose>
            <xsl:when test="@COLOR">
              <xsl:value-of select="@COLOR"/>
            </xsl:when>
            <xsl:otherwise>#000</xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
        <xsl:attribute name="fill">transparent</xsl:attribute>
        <xsl:attribute name="data-source">
          <xsl:value-of select="../../@ID"/>
        </xsl:attribute>
        <xsl:attribute name="data-target">
          <xsl:value-of select="../@ID"/>
        </xsl:attribute>
      </xsl:element>
    </xsl:if>

    <xsl:apply-templates select="../node/edge">
      <xsl:with-param name="level"><xsl:value-of select="$level+1"/></xsl:with-param>
    </xsl:apply-templates>
  </xsl:template>

  <xsl:template match="@LINK">
    <xsl:attribute name="onclick">
      <xsl:text>window.open('</xsl:text>
      <xsl:value-of select="."/>
      <xsl:text>', '_blank');</xsl:text>
    </xsl:attribute>
  </xsl:template>

  <!-- NAMED TEMAPLTES -->
  <xsl:template name="minimum">
    <xsl:param name="sequence"/>
    <xsl:param name="margin">0</xsl:param>

    <xsl:for-each select="$sequence">
      <xsl:sort select="." data-type="number" order="ascending"/>
      <xsl:if test="position()=1">
        <xsl:value-of select=".-$margin"/>
      </xsl:if>
    </xsl:for-each>
  </xsl:template>
  <xsl:template name="maximum">
    <xsl:param name="sequence"/>
    <xsl:param name="margin">0</xsl:param>

    <xsl:for-each select="$sequence">
      <xsl:sort select="." data-type="number" order="descending"/>
      <xsl:if test="position()=1">
        <xsl:value-of select=".+$margin"/>
      </xsl:if>
    </xsl:for-each>
  </xsl:template>

  <!-- standard copy template -->
  <xsl:template match="@*|node()">
    <xsl:copy>
      <xsl:apply-templates select="@*" />
      <xsl:apply-templates />
    </xsl:copy>
  </xsl:template>
</xsl:stylesheet>
