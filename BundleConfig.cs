using System.IO;
using System.Web;
using System.Web.Hosting;
using System.Web.Optimization;
using WebApplication1.Properties;

namespace WebApplication1
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            string nombre, ruta, path = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "Scripts");
            foreach (var item in Directory.GetFiles(path, "*.js", SearchOption.AllDirectories))
            {
                nombre = item.Substring(path.Length).Replace(@"\", "/");
                ruta = HostingEnvironment.ApplicationPhysicalPath;
                
                if (Settings.Default.Minificar) // agrega bundles utilizando la estructura de carpetas y archivos como ruta virtual
                {
                    bundles.Add(new ScriptBundle("~/bundles" + nombre.Remove(nombre.Length - 3)).Include("~/" + item.Substring(ruta.Length).Replace(@"\", "/")));
                }
                else
                {
                    bundles.Add(new Bundle("~/bundles" + nombre.Remove(nombre.Length - 3)).Include("~/" + item.Substring(ruta.Length).Replace(@"\", "/")));
                }
            }

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include("~/Scripts/jquery.validate*"));
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/modernizr-*"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.js","~/Scripts/respond.js"));
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/bootstrap.css","~/Content/site.css"));
        }
    }
}
