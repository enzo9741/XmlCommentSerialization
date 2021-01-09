using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XmlSrc
{
    class Serializateur
    {
        public static void Serializer(object demo)
        {
            if (!Directory.Exists(@"C:\XmlComment"))
            {
                Console.WriteLine("Creation of folder");
                Directory.CreateDirectory(@"C:\XmlComment");
            }
            XmlSerializer serializer =
            new XmlSerializer(typeof(DemoClass.Demo));
            TextWriter writer = new StreamWriter(@"C:\XmlComment\file.xml");
            serializer.Serialize(writer, demo);
            writer.Close();
            Console.WriteLine("File Write");
        }
    }
}
