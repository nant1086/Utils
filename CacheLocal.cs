using Project.Properties;
using System;
using System.Diagnostics;
using System.Web.Caching;

namespace Project
{
    /// <summary>
    /// Provee funcionalidad para poner en cache información resultante de la ejecución de métodos con importante latencia
    /// </summary>
    public static class CacheLocal
    {
        /// <summary>
        /// Sincroniza el acceso de subprocesos al objeto cache
        /// </summary>
        private static readonly object locker = new object();

        private static readonly Stopwatch contador = new Stopwatch();
        
        /// <summary>
        /// Trata de obtener desde cache la información, de lo contrario ejecuta la función con los parámetros 
        /// y lo almacena en cache para posteriores llamadas con los mismos valores de parámetros
        /// </summary>
        public static TR Obtener<TR>(this Cache cache, Func<TR> funcion, Expiracion expiracion = null)
        {
            return ObtenerDeCache(cache, funcion.Method.Name, expiracion, () => funcion());
        }

        /// <summary>
        /// Trata de obtener desde cache la información, de lo contrario ejecuta la función con los parámetros 
        /// y lo almacena en cache para posteriores llamadas con los mismos valores de parámetros
        /// </summary>
        public static TR Obtener<TR, T1>(this Cache cache, Func<T1, TR> funcion, T1 d1, Expiracion expiracion = null)
        {
            return ObtenerDeCache(cache, string.Join("|", funcion.Method.Name, d1), expiracion, () => funcion(d1));
        }

        /// <summary>
        /// Trata de obtener desde cache la información, de lo contrario ejecuta la función con los parámetros 
        /// y lo almacena en cache para posteriores llamadas con los mismos valores de parámetros
        /// </summary>
        public static TR Obtener<TR, T1, T2>(this Cache cache, Func<T1, T2, TR> funcion, T1 d1, T2 d2, Expiracion expiracion = null)
        {
            var key = string.Join("|", funcion.Method.Name, d1, d2);
            return ObtenerDeCache(cache, key, expiracion, () => funcion(d1, d2));
        }
        /// <summary>
        /// Trata de obtener desde cache la información, de lo contrario ejecuta la función con los parámetros 
        /// y lo almacena en cache para posteriores llamadas con los mismos valores de parámetros
        /// </summary>
        public static TR Obtener<TR, T1, T2, T3>(this Cache cache, Func<T1, T2, T3, TR> funcion, T1 d1, T2 d2, T3 d3, Expiracion expiracion = null)
        {
            var key = string.Join("|", funcion.Method.Name, d1, d2, d3);
            return ObtenerDeCache(cache, key, expiracion, () => funcion(d1, d2, d3));
        }
        /// <summary>
        /// Trata de obtener desde cache la información, de lo contrario ejecuta la función con los parámetros 
        /// y lo almacena en cache para posteriores llamadas con los mismos valores de parámetros
        /// </summary>
        public static TR Obtener<TR, T1, T2, T3, T4>(this Cache cache, Func<T1, T2, T3, T4, TR> funcion, T1 d1, T2 d2, T3 d3, T4 d4, Expiracion expiracion = null)
        {
            var key = string.Join("|", funcion.Method.Name, d1, d2, d3, d4);
            return ObtenerDeCache(cache, key, expiracion, () => funcion(d1, d2, d3, d4));
        }
        /// <summary>
        /// Trata de obtener desde cache la información, de lo contrario ejecuta la función con los parámetros 
        /// y lo almacena en cache para posteriores llamadas con los mismos valores de parámetros
        /// </summary>
        public static TR Obtener<TR, T1, T2, T3, T4, T5>(this Cache cache, Func<T1, T2, T3, T4, T5, TR> funcion, T1 d1, T2 d2, T3 d3, T4 d4, T5 d5, Expiracion expiracion = null)
        {
            var key = string.Join("|", funcion.Method.Name, d1, d2, d3, d4, d5);
            return ObtenerDeCache(cache, key, expiracion, () => funcion(d1, d2, d3, d4, d5));
        }
        /// <summary>
        /// Trata de obtener desde cache la información, de lo contrario ejecuta la función con los parámetros 
        /// y lo almacena en cache para posteriores llamadas con los mismos valores de parámetros
        /// </summary>
        public static TR Obtener<TR, T1, T2, T3, T4, T5, T6>(this Cache cache, Func<T1, T2, T3, T4, T5, T6, TR> funcion, T1 d1, T2 d2, T3 d3, T4 d4, T5 d5, T6 d6, Expiracion expiracion = null)
        {
            var key = string.Join("|", funcion.Method.Name, d1, d2, d3, d4, d5, d6);
            return ObtenerDeCache(cache, key, expiracion, () => funcion(d1, d2, d3, d4, d5, d6));
        }
        /// <summary>
        /// Trata de obtener desde cache la información, de lo contrario ejecuta la función con los parámetros 
        /// y lo almacena en cache para posteriores llamadas con los mismos valores de parámetros
        /// </summary>
        public static TR Obtener<TR, T1, T2, T3, T4, T5, T6, T7>(this Cache cache, Func<T1, T2, T3, T4, T5, T6, T7, TR> funcion, T1 d1, T2 d2, T3 d3, T4 d4, T5 d5, T6 d6, T7 d7, Expiracion expiracion = null)
        {
            var key = string.Join("|", funcion.Method.Name, d1, d2, d3, d4, d5, d6, d7);
            return ObtenerDeCache(cache, key, expiracion, () => funcion(d1, d2, d3, d4, d5, d6, d7));
        }
        /// <summary>
        /// Trata de obtener desde cache la información, de lo contrario ejecuta la función con los parámetros 
        /// y lo almacena en cache para posteriores llamadas con los mismos valores de parámetros
        /// </summary>
        public static TR Obtener<TR, T1, T2, T3, T4, T5, T6, T7, T8>(this Cache cache, Func<T1, T2, T3, T4, T5, T6, T7, T8, TR> funcion, T1 d1, T2 d2, T3 d3, T4 d4, T5 d5, T6 d6, T7 d7, T8 d8, Expiracion expiracion = null)
        {
            var key = string.Join("|", funcion.Method.Name, d1, d2, d3, d4, d5, d6, d7, d8);
            return ObtenerDeCache(cache, key, expiracion, () => funcion(d1, d2, d3, d4, d5, d6, d7, d8));
        }
        
        private static TR ObtenerDeCache<TR>(Cache cache, string key, Expiracion expiracion, Func<TR> funcion)
        {
            if (Config.ExpiracionCache <= 0)
            {
                return funcion();
            }
            if (cache[key] != null)
            {
                return (TR)cache[key];
            }
            lock (locker)
            {
                if (cache[key] == null)
                {
                    contador.Restart();
                    AgregarCache(cache, funcion(), key, expiracion);
                    contador.Stop();
                    Log.Debug(string.Concat("CacheLocal ", key, " Tiempo=", contador.Elapsed));
                }
            }
            return (TR)cache[key];
        }

        private static void AgregarCache(Cache cache, object valor, string key, Expiracion expiracion)
        {
            DateTime absoluta = Cache.NoAbsoluteExpiration;
            TimeSpan ultmoAcceso = Cache.NoSlidingExpiration;
            if (expiracion != null)
            {
                ultmoAcceso = expiracion.UltimoAcceso;
                absoluta = expiracion.Absoluta;
            }
            else
            {
                absoluta = DateTime.Now.Add(TimeSpan.FromSeconds(Config.ExpiracionCache));
            }
            //Log.Debug(string.Format("inserción en cache. abs={0}|último acceso={1}", absoluta, ultmoAcceso));
            cache.Insert(key, valor, null, absoluta, ultmoAcceso);
        }

    }

    public class Expiracion
    {
        public Expiracion()
        {
            Absoluta = Cache.NoAbsoluteExpiration;
            UltimoAcceso = Cache.NoSlidingExpiration;
        }
        /// <summary>
        /// Indica si debe expirar en cierta periodo en el tiempo en concreto
        /// </summary>
        public DateTime Absoluta { get; set; }
        /// <summary>
        /// Indica si debe expirar a partir del último acceso
        /// </summary>
        public TimeSpan UltimoAcceso { get; set; }
    }
}