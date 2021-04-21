using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace XmlParser
{
    class Namespace
    {
        public static void Run()
        {
            RunNamespaceName();
            RunNone();
            RunXml();
            RunXmlns();
        }

        public static void RunNamespaceName()
        {
            Console.WriteLine("\n----------------RunNamespaceName");
            string markup =@"<aw:Root xmlns:aw='http://www.adventure-works.com'/>";
            XElement root = XElement.Parse(markup);
            Console.WriteLine("Content : " + markup);
            Console.WriteLine("\tNamespaceName : " + root.Name.Namespace.NamespaceName);

            markup = @"<Root xmlns:aw='http://www.adventure-works.com'/>";
            root = XElement.Parse(markup);
            Console.WriteLine("Content : " + markup);
            Console.WriteLine("\tNamespaceName : " + root.Name.Namespace.NamespaceName);

            markup = @"<Root xmlns='http://www.adventure-works.com'/>";
            root = XElement.Parse(markup);
            Console.WriteLine("Content : " + markup);
            Console.WriteLine("\tNamespaceName : " + root.Name.Namespace.NamespaceName);
            /*
             http://www.adventure-works.com  
             */
        }

        public static void RunNone()
        {
            Console.WriteLine("\n----------------RunNone");
            XNamespace aw = "http://www.adventure-works.com";

            XElement root = new XElement("Root",
                new XElement(aw + "ChildInNamespace", "content"),
                new XElement("ChildAttrNamespace", new XAttribute(XName.Get("aw", XNamespace.Xmlns.NamespaceName), aw), "content"),
                new XElement("ChildInNoNamespace", "content")
            );

            if (root.Name.Namespace == XNamespace.None)
                Console.WriteLine("Root element is in no namespace");
            else
                Console.WriteLine("Root element is in a namespace");

            XElement element = root.Element(aw + "ChildInNamespace");
            if (element.Name.Namespace == XNamespace.None)
                Console.WriteLine("ChildInNamespace element is in no namespace");
            else
                Console.WriteLine("ChildInNamespace element is in a namespace");

            element = root.Element("ChildAttrNamespace");
            if (element.Name.Namespace == XNamespace.None)
                Console.WriteLine("ChildAttrNamespace element is in no namespace");
            else
                Console.WriteLine("ChildAttrNamespace element is in a namespace");

            element = root.Element("ChildInNoNamespace");
            if (element.Name.Namespace == XNamespace.None)
                Console.WriteLine("ChildInNoNamespace element is in no namespace");
            else
                Console.WriteLine("ChildInNoNamespace element is in a namespace");

            Console.WriteLine(root);
            /*
                Root element is in no namespace  
                ChildInNamespace element is in a namespace  
                ChildInNoNamespace element is in no namespace  
             */
        }

        public static void RunXml()
        {
            Console.WriteLine("\n----------------RunXml");
            XElement root = new XElement("Root",
    new XAttribute(XNamespace.Xml + "space", "preserve"),
    new XElement("Child", "content")
);
            Console.WriteLine(root);
            /*
                <Root xml:space="preserve">  
                  <Child>content</Child>  
                </Root>   
             */
        }

        public static void RunXmlns()
        {
            Console.WriteLine("\n----------------RunXmlns");
            XNamespace aw = "http://www.adventure-works.com";
            XElement root = new XElement(aw + "Root",
                new XAttribute(XNamespace.Xmlns + "aw", "http://www.adventure-works.com"),
                new XElement(aw + "Child", "content")
            );
            Console.WriteLine(root);
        }
    }
}
