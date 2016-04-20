using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Optimization;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;
using System.Web.Hosting;

[XmlType("BundleConfig")]
public class XmlBundleConfig
{
    [XmlElement("ScriptBundle")]
    public XmlBundle[] Scripts = new XmlBundle[0];

    [XmlElement("StyleBundle")]
    public XmlBundle[] Styles = new XmlBundle[0];

    public class XmlBundle
    {
        [XmlAttribute("VirtualPath")]
        public string virtualPath { get; set; }

        [XmlAttribute("Include")]
        public string Include { get; set; }

        [XmlAttribute("SearchPattern")]
        public string SearchPattern { get; set; }

        [XmlAttribute("SearchSubdirectories")]
        public bool SearchSubdirectories { get; set; }

        [XmlAttribute("Minificar")]
        public bool Minificar { get; set; }

        [XmlElement("Include")]
        public string[] Includes = new string[0];

    }
}