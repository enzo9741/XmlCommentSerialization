using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XmlSrc
{
    class Deserializateur
    {
        public static DemoClass demo;
        public static void Deserialize()
        {
            demo = new DemoClass();
            XmlSerializer serializer = new XmlSerializer(typeof(DemoClass));
            serializer.UnknownNode += new
            XmlNodeEventHandler(serializer_UnknownNode);
            serializer.UnknownAttribute += new
            XmlAttributeEventHandler(serializer_UnknownAttribute);
            FileStream fs = new FileStream(@"C:\XmlComment", FileMode.Open);
            demo = (DemoClass)serializer.Deserialize(fs);
            fs.Close();
        }
        protected static void serializer_UnknownNode(object sender, XmlNodeEventArgs e)
        {
            Console.WriteLine("Unknown Node:" + e.Name + "\t" + e.Text);
        }

        protected static void serializer_UnknownAttribute(object sender, XmlAttributeEventArgs e)
        {
            System.Xml.XmlAttribute attr = e.Attr;
            Console.WriteLine("Unknown attribute " + attr.Name + "='" + attr.Value + "'");
        }
    }
}
