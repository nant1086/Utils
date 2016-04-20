using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;

namespace Project
{
    /// <summary>
    /// Implementa la serialización de Json para la libreria Json.net, la cual tiene las ventajas de ser mas rápida y configurable. Ver <see cref="http://www.newtonsoft.com/json/help"/>
    /// </summary>
    public class JsonNet : JsonResult
    {
        /// <summary>
        /// Implementa la serialización de Json para la libreria Json.net, la cual tiene las ventajas de ser mas rápida y configurable. Ver <see cref="http://www.newtonsoft.com/json/help"/>
        /// </summary>
        public JsonNet()
        {

        }

        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        private static readonly JsonSerializer serial = JsonSerializer.Create(Settings);

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null) { throw new ArgumentNullException("context"); }
            if (this.Data == null) { return; }

            if (this.JsonRequestBehavior == JsonRequestBehavior.DenyGet
                && string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("JSON GET is not allowed");
            }
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = string.IsNullOrWhiteSpace(this.ContentType) ? "application/json" : this.ContentType;

            if (this.ContentEncoding != null)
            {
                response.ContentEncoding = this.ContentEncoding;
            }

            response.Write(JsonConvert.SerializeObject(this.Data));
        }
        /// <summary>
        /// Serializa el objeto a Json
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public static string Serializar(object objeto)
        {
            return JsonConvert.SerializeObject(objeto, Aplicacion.Debug ? Formatting.Indented : Formatting.None, conversorFecha);
        }
        /// <summary>
        /// Deserializa un json a un objeto
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T Deserializar<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, Settings);
        }

        private static readonly JavaScriptDateTimeConverter conversorFecha = new JavaScriptDateTimeConverter();
    }

    public static class JsonHelper
    {

        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        private static readonly JavaScriptDateTimeConverter conversorFecha = new JavaScriptDateTimeConverter();

        /// <summary>
        /// Serializa el objeto a Json
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public static string Serializar(object objeto, Formatting formato = Formatting.None)
        {
            return JsonConvert.SerializeObject(objeto, formato, conversorFecha);
        }
        
        /// <summary>
        /// Deserializa un json a un objeto
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T Deserializar<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, Settings);
        }
    }
}