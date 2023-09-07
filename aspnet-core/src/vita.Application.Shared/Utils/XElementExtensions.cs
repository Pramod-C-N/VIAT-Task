using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.Xml;

namespace vita.Utils
{
    public static class XElementExtensions
    {
        public static XmlElement ToXmlElement(this string str)
        {
            var doc = new XmlDocument();
            doc.LoadXml(str);
            return doc.DocumentElement;
        }

        public static XmlElement ToXmlElement(this XElement el)
        {
            var doc = new XmlDocument();
            using (var rdr = el.CreateReader())
            {
                doc.Load(rdr);
            }
            return doc.DocumentElement;
        }
    }
}
