using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Optimization;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;
using System.Web.Hosting;

namespace Project
{
    public class BundleConfig
    {
        private static void AgregarBundles(BundleCollection bundles, XmlBundleConfig.XmlBundle item, bool sripts)
        {
            Bundle bundle = !item.Minificar
                ? new Bundle(item.virtualPath)
                : sripts
                     ? new ScriptBundle(item.virtualPath)
                     : new StyleBundle(item.virtualPath) as Bundle;

            foreach (var ruta in unir(item.Includes, item.Include))
            {
                if (!string.IsNullOrWhiteSpace(item.SearchPattern) && ruta.EndsWith("*"))
                {
                    bundle.IncludeDirectory(ruta.TrimEnd('*'), item.SearchPattern, item.SearchSubdirectories);
                }
                else
                {
                    bundle.Include(ruta);
                }
            }

            bundles.Add(bundle);
        }
        /// <summary>
        /// Une las colecciones quitando las cadenas vacias
        /// </summary>
        /// <param name="uno"></param>
        /// <param name="dos"></param>
        /// <returns></returns>
        private static IEnumerable<string> unir(IEnumerable<string> uno, params string[] dos)
        {
            return dos.Concat((uno ?? new string[0])).Where(p => !string.IsNullOrWhiteSpace(p));
        }

        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            string path = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, @"App_Data\Bundles.xml");

            if (File.Exists(path))
            {
                try
                {
                    using (var xml = XmlReader.Create(path))
                    {
                        var serial = new XmlSerializer(typeof(XmlBundleConfig));
                        var xmlbundles = (XmlBundleConfig)serial.Deserialize(xml);

                        foreach (var item in xmlbundles.Scripts)
                        {
                            AgregarBundles(bundles, item, true);
                        }

                        foreach (var item in xmlbundles.Styles)
                        {
                            AgregarBundles(bundles, item, false);
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    Log.Error("Hubo un error al cargar el archivo " + path, ex);
                }
            }
        }
    }
}