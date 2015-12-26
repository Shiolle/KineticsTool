using Kinetics.Storage.Situation;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Kinetics.Storage.HitLocationTable;

namespace Kinetics.Storage
{
    public class StorageController : IStorageController
    {
        public void SaveUnits(SitRepSto sitRep, string path)
        {
            var xmlSr = new XmlSerializer(typeof(SitRepSto));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            var writerSettings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = true,
                IndentChars = ("\t")
            };

            using (var writer = XmlWriter.Create(path, writerSettings))
            {
                xmlSr.Serialize(writer, sitRep, namespaces);
            }
        }

        public SitRepSto ReadSitRep(string path)
        {
            SitRepSto sitRep;

            var xmlSr = new XmlSerializer(typeof(SitRepSto));
            using (var reader = new StreamReader(path))
            {
                sitRep = (SitRepSto)xmlSr.Deserialize(reader);
            }
            return sitRep;
        }

        public HitTablesCatalogSto LoadHitTableCatalog(string path)
        {
            HitTablesCatalogSto result;

            var xmlSr = new XmlSerializer(typeof(HitTablesCatalogSto));
            using (var reader = new StreamReader(path))
            {
                result = (HitTablesCatalogSto)xmlSr.Deserialize(reader);
            }
            return result;
        }

        public void SaveHitTableCatalog(HitTablesCatalogSto catalog, string path)
        {
            var xmlSr = new XmlSerializer(typeof(HitTablesCatalogSto));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            var writerSettings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = true,
                IndentChars = ("\t")
            };

            using (var writer = XmlWriter.Create(path, writerSettings))
            {
                xmlSr.Serialize(writer, catalog, namespaces);
            }
        }
    }
}
