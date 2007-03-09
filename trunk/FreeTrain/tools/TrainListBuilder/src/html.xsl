<?xml version="1.0" encoding="Shift_JIS" ?>
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:saxon="http://icl.com/saxon"
	extension-element-prefixes="saxon">
	
	<xsl:output method="html" encoding="Shift_JIS" />
	
	<xsl:template match="/">
		<saxon:output href="toc.html" method="html">
			<html>
				<head>
					<xsl:call-template name="css"/>
				</head>
				<body>
					<ul>
						<xsl:for-each select="//companies/company">
							<xsl:sort select="."/>
							
							<xsl:variable name="trains" select="//train[company=current()/text()]"/>
							<li>
								<a href="{generate-id(.)}.html" target="body">
									<xsl:value-of select="."/>
								</a>
								<small>
									<xsl:text> (</xsl:text>
									<xsl:value-of select="count($trains)"/>
									<xsl:text>�R)</xsl:text>
								</small>
							</li>
							<saxon:output href="{generate-id(.)}.html" method="html">
								<html>
									<head>
										<title><xsl:value-of select="." />�ԗ����X�g</title>
										<xsl:call-template name="css"/>
									</head>
									<body>
										<h1><xsl:value-of select="." />�ԗ����X�g</h1>
										<xsl:apply-templates select="$trains">
											<xsl:sort select="name"/>
										</xsl:apply-templates>
									</body>
								</html>
							</saxon:output>
						</xsl:for-each>
					</ul>
					<p>�S<xsl:value-of select="count(//train)"/>��</p>
				</body>
			</html>
		</saxon:output>
		<saxon:output href="index.html" method="html">
			<html>
				<head>
					<title>FreeTrain�ԗ����X�g</title>
				</head>
				<frameset cols="250,*">
					<frame src="toc.html" />
					<frame src="first.html" name="body" />
				</frameset>
			</html>
		</saxon:output>
		<saxon:output href="first.html" method="html">
			<html>
				<head>
					<xsl:call-template name="css"/>
				</head>
				<body>
					<h1>FreeTrain�ԗ����X�g</h1>
					<p>
						���̒m��͈͂Ō��J����Ă���FreeTrain�p�̎ԗ����W�߂Ă݂܂����B����HTML�����v���O�������̂ɋ����̂���l�͎��܂łǂ����B
					</p>
				</body>
			</html>
		</saxon:output>
	</xsl:template>
	
	<xsl:template match="train">
		<h2><xsl:value-of select="name"/></h2>
		<table border="0">
			<tr>
				<td rowspan="3">
					<img src="{@id}.png"/>
				</td>
				<td><nobr>��ҁF</nobr></td>
				<td>
					<xsl:value-of select="author"/>
				</td>
			</tr>
			<tr>
				<td><nobr>���x�F</nobr></td>
				<td>
					<xsl:value-of select="speed"/>
				</td>
			</tr>
			<tr>
				<td><nobr>�����F</nobr></td>
				<td>
					<xsl:value-of select="description"/>
				</td>
			</tr>
		</table>
	</xsl:template>
	
	
	<xsl:template name="css">
		<style>
			h2 {
				background-color: lightblue;
			}
		</style>
	</xsl:template>
</xsl:stylesheet>
