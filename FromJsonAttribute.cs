using Newtonsoft.Json;
using System;
using System.Globalization;
using System.IO;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Project
{
     //JSON nativo
    /// <summary>
    /// Deserializa desde json y mapea los valores al tipo del parámetro. 
    /// A diferencia del mapeo tradicional de parametros en una accion, este atributo 
    /// permite el mapeo de mas de 1 nivel de profundidad en la estructura del objeto
    /// </summary>
    public class FromJsonAttribute : CustomModelBinderAttribute
    {
        /// <summary>
        /// Deserializa desde json y mapea los valores al tipo del parámetro. 
        /// A diferencia del mapeo tradicional de parametros en una accion, este atributo 
        /// permite el mapeo de mas de 1 nivel de profundidad en la estructura del objeto
        /// </summary>
        public FromJsonAttribute()
        {

        }
        private readonly static JavaScriptSerializer serializer = new JavaScriptSerializer();

        private readonly static JsonModelBinder binder = new JsonModelBinder();

        public override IModelBinder GetBinder()
        {
            return binder;
        }

        private class JsonModelBinder : IModelBinder
        {
            public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
            {
                var stringified = controllerContext.HttpContext.Request[bindingContext.ModelName];
                if (string.IsNullOrWhiteSpace(stringified)) { return null; }
                return serializer.Deserialize(stringified, bindingContext.ModelType);
            }
        }
    }
    
    // JSON.net
    /// <summary>
    /// Deserializa desde json y mapea los valores al tipo del parámetro. Utiliza la libreria Json.net
    /// </summary>
    public class JsonParseAttribute : CustomModelBinderAttribute
    {
        /// <summary>
        /// Deserializa desde json y mapea los valores al tipo del parámetro. Utiliza la libreria Json.net
        /// </summary>
        public JsonParseAttribute()
        {

        }

        private static readonly CultureInfo mexico = new CultureInfo("es-MX");

        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Culture = mexico
        };

        private static readonly JsonSerializer serializer = JsonSerializer.Create(Settings);        

        private readonly static JsonModelBinder binder = new JsonModelBinder();

        public override IModelBinder GetBinder()
        {
            return binder;
        }

        private class JsonModelBinder : IModelBinder
        {
            public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
            {
                try
                {
                    var text = controllerContext.HttpContext.Request[bindingContext.ModelName];
                    if (string.IsNullOrWhiteSpace(text)) { return null; }
                    using (var stringReader = new StringReader(text))
                    {
                        using (var jsonReader = new JsonTextReader(stringReader))
                        {
                            return serializer.Deserialize(jsonReader, bindingContext.ModelType);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    throw;
                }            
            }
        }
    }
}