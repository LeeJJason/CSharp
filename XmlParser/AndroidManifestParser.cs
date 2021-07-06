using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace XmlParser
{
    class AndroidManifestParser
    {
        public static void Run()
        {
            Console.WriteLine(XNamespace.Xml);
            Console.WriteLine(XNamespace.Xmlns);
            XName xName = XName.Get("name", XNamespace.Xmlns.NamespaceName);
            Console.WriteLine(xName);

            XDocument document = XDocument.Load("../../AndroidManifest.xml");
            XElement rootElement = document.Root;

            XName name = XName.Get("android", XNamespace.Xmlns.NamespaceName);
            XAttribute attribute = rootElement.Attribute(name);
            name = XName.Get("name", attribute.Value);

            var permissions = rootElement.Elements("uses-permission");
            foreach (var permission in permissions)
            {
                var att = permission.Attribute(name);

                Console.WriteLine(att.Name.LocalName);
                Console.WriteLine(att.Name.NamespaceName);
                Console.WriteLine(att.Name.Namespace);
                Console.WriteLine(att.Value);
                Console.WriteLine(att);
                Console.WriteLine("----------------------");
            }
        }
    }
}
