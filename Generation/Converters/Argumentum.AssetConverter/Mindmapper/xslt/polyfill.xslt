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
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:dc="http://www.omg.org/spec/DD/20100524/DC"
  xmlns:svg="http://www.w3.org/2000/svg">

  <xsl:output method="xml" omit-xml-declaration="no" indent="yes" />

  <xsl:variable name="debug">true</xsl:variable>
  <xsl:variable name="fontSize" select="10"/>
  <xsl:variable name="marginX">10</xsl:variable>
  <xsl:variable name="marginY">10</xsl:variable>
  <xsl:variable name="shapeWidth" select="200"/>
  <xsl:variable name="horizInterval"><xsl:value-of select="$shapeWidth * 1.25"/></xsl:variable>
  <xsl:variable name="vertInterval"><xsl:value-of select="$fontSize * 8"/></xsl:variable>

  <xsl:variable name="diagramId">
    <xsl:choose>
      <xsl:when test="1=2"><xsl:value-of select="/@ID"/></xsl:when>
      <xsl:otherwise><xsl:value-of select="generate-id()"/></xsl:otherwise>
    </xsl:choose>
  </xsl:variable>
  <xsl:variable name="diagramName" select="//map/@NAME"/>

  <xsl:template match="/">
    <xsl:element name="map">
      <xsl:attribute name="id"><xsl:value-of select="$diagramId"/></xsl:attribute>
      <xsl:attribute name="name"><xsl:value-of select="$diagramName"/></xsl:attribute>
      <xsl:attribute name="version">1.0.1</xsl:attribute>

      <xsl:apply-templates select="/map/node"/>
    </xsl:element>
  </xsl:template>

  <xsl:template match="node">
    <xsl:param name="parentPosition"></xsl:param>
    <xsl:param name="parentY"><xsl:value-of select="$marginY"/></xsl:param>
    <xsl:param name="level">0</xsl:param>
    <xsl:variable name="semId" select="./@ID"/>
    <xsl:variable name="semElement" select="//*[@id=$semId]"/>
    <xsl:variable name="diElement" select="$semElement"/>
    <xsl:variable name="position">
      <xsl:choose>
        <xsl:when test="@POSITION='left'">left</xsl:when>
        <xsl:when test="@POSITION='right'">right</xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$parentPosition"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:if test="$debug='true'">
      <xsl:comment>
        <xsl:text>semId = </xsl:text><xsl:value-of select="$semId"/> is at level <xsl:value-of select="$level"/>
      </xsl:comment>
    </xsl:if>

    <xsl:choose>
      <xsl:when test="dc:Bounds">
        <xsl:element name="node">
          <xsl:apply-templates select="@*"/>
          <xsl:apply-templates/>
        </xsl:element>
      </xsl:when>
      <xsl:otherwise>
        <!-- Make an educated guess at bounds -->
        <xsl:variable name="pos" select="position()"></xsl:variable>
        <xsl:variable name="count" select="count(../node)"></xsl:variable>

        <xsl:variable name="width">
          <xsl:choose>
            <xsl:when test="$diElement/dc:Bounds">
              <xsl:comment>Taking width from existing bounds</xsl:comment>
              <xsl:value-of select="$diElement/dc:Bounds/@width"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:comment>Synthesizing width</xsl:comment>
              <xsl:value-of select="$shapeWidth"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>
        <xsl:variable name="height">
          <!-- Assume one line enough -->
          <xsl:value-of select="$fontSize*4"/>
        </xsl:variable>
        <xsl:variable name="x">
          <xsl:choose>
            <xsl:when test="$position='left'"><xsl:value-of select="-($level * $horizInterval)"/></xsl:when>
            <xsl:when test="$position='right'"><xsl:value-of select="$level * $horizInterval"/></xsl:when>
            <xsl:otherwise>0</xsl:otherwise>
          </xsl:choose>
        </xsl:variable>
        <xsl:variable name="y">
          <xsl:choose>
            <xsl:when test="$pos &gt; $count div 2 and $pos = round($count div 2)">
              <xsl:value-of select="$parentY"/>
            </xsl:when>
            <xsl:when test="$pos &lt; $count div 2">
              <xsl:value-of select="$parentY - ($vertInterval * $pos)"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="$parentY + ($vertInterval * $pos div 2)"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>

        <xsl:if test="$debug='true'">
          <xsl:comment>
            <xsl:text>Calc x,y as </xsl:text><xsl:value-of select="$x"/>, <xsl:value-of select="$y"/>
            count: <xsl:value-of select="$count"/>, count/2: <xsl:value-of select="$count div 2"/>,
            pos: <xsl:value-of select="$pos"/>,
            pos &gt; count/2: <xsl:value-of select="$pos &lt; $count div 2"/>,
            round count/2: <xsl:value-of select="round($count div 2)"/>,
            pos = round: <xsl:value-of select="$pos = round($count div 2)"/>,
          </xsl:comment>
        </xsl:if>

        <xsl:element name="node">
          <xsl:apply-templates select="@*"/>
          <xsl:apply-templates select="./edge"/>
          <xsl:element name="Bounds" namespace="http://www.omg.org/spec/DD/20100524/DC">
            <xsl:attribute name="height">
              <xsl:value-of select="$height"/>
            </xsl:attribute>
            <xsl:attribute name="width">
              <xsl:value-of select="$width"/>
            </xsl:attribute>
            <xsl:attribute name="x">
              <xsl:value-of select="$x"/>
            </xsl:attribute>
            <xsl:attribute name="y">
              <xsl:value-of select="$y"/>
            </xsl:attribute>
          </xsl:element>
          <xsl:apply-templates select="./node">
            <xsl:with-param name="parentPosition" select="$position"/>
            <xsl:with-param name="parentY" select="$y"/>
            <xsl:with-param name="level" select="$level+1"/>
          </xsl:apply-templates>
        </xsl:element>

      </xsl:otherwise>
    </xsl:choose>

  </xsl:template>

  <xsl:template match="edge">
    <xsl:copy>
      <xsl:apply-templates select="@*"/>
    </xsl:copy>
  </xsl:template>

  <!-- standard copy template -->
  <xsl:template match="@*">
    <xsl:copy>
      <xsl:apply-templates select="@*"/>
    </xsl:copy>
  </xsl:template>
</xsl:stylesheet>
