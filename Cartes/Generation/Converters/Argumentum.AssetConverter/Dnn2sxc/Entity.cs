using System.Xml.Serialization;

namespace Argumentum.AssetConverter.Dnn2sxc
{
    [XmlRoot(ElementName = "Entity")]
    public class Entity
    {
        [XmlElement(ElementName = "Guid")]
        public string Guid { get; set; }
        [XmlElement(ElementName = "Language")]
        public string Language { get; set; }
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "Description")]
        public string Description { get; set; }
        [XmlElement(ElementName = "Example")]
        public string Example { get; set; }
        [XmlElement(ElementName = "Parent")]
        public string Parent { get; set; }
        [XmlElement(ElementName = "Family")]
        public string Family { get; set; }
        [XmlElement(ElementName = "Subfamily")]
        public string Subfamily { get; set; }
        [XmlElement(ElementName = "SubsubFamily")]
        public string SubsubFamily { get; set; }
        [XmlElement(ElementName = "Card")]
        public string Card { get; set; }
        [XmlElement(ElementName = "Author")]
        public string Author { get; set; }
        [XmlElement(ElementName = "Original")]
        public string Original { get; set; }
        [XmlElement(ElementName = "Difficulty")]
        public string Difficulty { get; set; }
        [XmlElement(ElementName = "Licence")]
        public string Licence { get; set; }
        [XmlElement(ElementName = "Date")]
        public string Date { get; set; }
        [XmlAttribute(AttributeName = "Type")]
        public string Type { get; set; }
    }
}