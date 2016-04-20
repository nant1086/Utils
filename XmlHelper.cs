using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Project
{
    public static class XmlHelper
    {
        private static readonly XmlWriterSettings configIdentado = new XmlWriterSettings { Indent = true, NewLineOnAttributes = false, IndentChars = "\t", Encoding = Encoding.UTF8, OmitXmlDeclaration = true };
        private static readonly XmlParserContext ParserContext = new XmlParserContext(null, new XmlNamespaceManager(new NameTable()), null, XmlSpace.None);
        private static readonly XmlWriterSettings configCompacto = new XmlWriterSettings { Indent = false, NewLineOnAttributes = false, Encoding = Encoding.UTF8, OmitXmlDeclaration = true };
        /// <summary>
        /// Serializa el objeto a formato xml
        /// </summary>
        /// <param name="objeto"></param>
        /// <param name="identado">si es true formatea el xml para una fácil lectura, de lo contrario genera el xml en modo un poco mas compacto</param>
        /// <returns></returns>
        public static string Serializar(object objeto, bool identado = false)
        {
            var builderExclusion = new StringBuilder();
            using (var writerExclusion = XmlWriter.Create(builderExclusion, identado ? configIdentado : configCompacto))
            {
                var namespaceLess = new XmlSerializerNamespaces();
                namespaceLess.Add(string.Empty, string.Empty);

                var serialExclusion = new XmlSerializer(objeto.GetType());
                serialExclusion.Serialize(writerExclusion, objeto, namespaceLess);
                return builderExclusion.ToString(); 
            }
        }

        /// <summary>
        /// Deserializa el texto xml a objetos
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T Deserializar<T>(string xml)
        {
            using (var reader = new StringReader(xml))
            {
                var serial = new XmlSerializer(typeof(T));
                return (T)serial.Deserialize(reader);
            }
        }

    }
}