﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Argumentum.AssetConverter.Mindmapper
{
	[XmlRoot(ElementName = "font")]
	public class Font
	{
		[XmlAttribute(AttributeName = "size")]
		public string Size { get; set; }
		[XmlText]
		public string Text { get; set; }
		[XmlAttribute(AttributeName = "BOLD")]
		public string BOLD { get; set; }
		[XmlAttribute(AttributeName = "NAME")]
		public string NAME { get; set; }
		[XmlAttribute(AttributeName = "SIZE")]
		public string SIZE { get; set; }
		[XmlAttribute(AttributeName = "color")]
		public string Color { get; set; }
	}

    //[XmlRoot(ElementName = "p")]
    //public class P
    //{
    //	[XmlElement(ElementName = "font")]
    //	public Font Font { get; set; }
    //}

    [XmlRoot(ElementName = "body")]
    public class Body
    {
		//[XmlElement(ElementName = "p")]
		//public P P { get; set; }

		[XmlAnyAttribute]
        public List<XmlAttribute> Attributes = new List<XmlAttribute>();
        [XmlAnyElement]
        public List<XmlElement> Elements = new List<XmlElement>();

	}

    [XmlRoot(ElementName = "html")]
	public class Html
    {
        [XmlElement(ElementName = "head")] 
        public string Head { get; set; } = "\n";
        
        [XmlElement(ElementName = "body")] 
        public Body Body { get; set; } = new Body(); //= "<p>\r\n</p>";
    }

	[XmlRoot(ElementName = "richcontent")]
	public class Richcontent
	{
		[XmlElement(ElementName = "html")]
		public Html Html { get; set; } = new Html();

        [XmlAttribute(AttributeName = "TYPE")] public string TYPE { get; set; } = "NOTE";
    }

	[XmlRoot(ElementName = "edge")]
	public class Edge
	{
		[XmlAttribute(AttributeName = "WIDTH")]
		public string WIDTH { get; set; }
		[XmlAttribute(AttributeName = "STYLE")]
		public string STYLE { get; set; }
		[XmlAttribute(AttributeName = "COLOR")]
		public string COLOR { get; set; }
	}

	[XmlRoot(ElementName = "icon")]
	public class Icon
	{
		[XmlAttribute(AttributeName = "BUILTIN")]
		public string BUILTIN { get; set; }
	}


	[XmlRoot(ElementName = "arrowlink")]
	public class Arrowlink
	{
		[XmlAttribute(AttributeName = "COLOR")]
		public string Color { get; set; }

		[XmlAttribute(AttributeName = "DESTINATION")]
		public string Destination { get; set; }

		[XmlAttribute(AttributeName = "ENDARROW")]
		public string EndArrow { get; set; }

		[XmlAttribute(AttributeName = "ENDINCLINATION")]
		public string EndInclination { get; set; }

		[XmlAttribute(AttributeName = "ID")]
		public string ID { get; set; }

		[XmlAttribute(AttributeName = "STARTARROW")]
		public string StartArrow { get; set; }

		[XmlAttribute(AttributeName = "STARTINCLINATION")]
		public string StartInclination { get; set; }

		// Add other attributes as needed
	}


	[XmlRoot(ElementName = "node")]
	public class Node
	{

        static int GetUnixTime(DateTime utcTime)
        {
            return (int) (utcTime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        [XmlElement(ElementName = "edge")]
		public Edge Edge { get; set; }

        [XmlAttribute(AttributeName = "CREATED")]
        public string CREATED { get; set; } = GetUnixTime(DateTime.UtcNow).ToString(CultureInfo.InvariantCulture);
		[XmlAttribute(AttributeName = "ID")]
		public string ID { get; set; }
		[XmlAttribute(AttributeName = "MODIFIED")]
		public string MODIFIED { get; set; } = GetUnixTime(DateTime.UtcNow).ToString(CultureInfo.InvariantCulture);

        [XmlAttribute(AttributeName = "TEXT")] public string TEXT { get; set; } = "";
		
        
        [XmlElement(ElementName = "richcontent")]
		public List<Richcontent> Richcontents { get; set; } = new List<Richcontent>();


		[XmlElement(ElementName = "font")]
		public Font Font { get; set; }

		[XmlElement(ElementName = "arrowlink")]
		public List<Arrowlink> Arrowlinks { get; set; } = new List<Arrowlink>();


		[XmlElement(ElementName = "node")]
		public List<Node> Nodes { get; set; } = new List<Node>();


		[XmlAttribute(AttributeName = "POSITION")]
		public string POSITION { get; set; }
		[XmlAttribute(AttributeName = "STYLE")]
		public string STYLE { get; set; }

        [XmlElement(ElementName = "icon")] public List<Icon> Icons { get; set; } = new List<Icon>();

		[XmlAttribute(AttributeName = "LINK")]
		public string LINK { get; set; }
		[XmlElement(ElementName = "cloud")]
		public string Cloud { get; set; }
		[XmlAttribute(AttributeName = "BACKGROUND_COLOR")]
		public string BACKGROUND_COLOR { get; set; }
		[XmlAttribute(AttributeName = "COLOR")]
		public string COLOR { get; set; }
	}

	[XmlRoot(ElementName = "map")]
	[XmlInclude(typeof(FreeplaneMap))]
	public class FreemindMap
	{
		[XmlElement(ElementName = "node")]
		public Node Node { get; set; }

        [XmlAttribute(AttributeName = "version")]
        public virtual string Version { get; set; } = "1.0.1";
    }

	public class FreeplaneMap : FreemindMap
	{
		[XmlAttribute(AttributeName = "version")]
		public override string Version { get; set; } = "freeplane 1.11.5";
	}
}
