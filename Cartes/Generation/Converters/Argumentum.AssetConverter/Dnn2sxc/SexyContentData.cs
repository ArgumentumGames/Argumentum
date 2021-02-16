using System.Collections.Generic;
using System.Xml.Serialization;

namespace Argumentum.AssetConverter.Dnn2sxc
{
    [XmlRoot(ElementName = "SexyContentData")]
    public class SexyContentData
    {
        [XmlElement(ElementName = "Entity")]
        public List<Entity> Entity { get; set; }
    }
}