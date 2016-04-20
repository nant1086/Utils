using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace Project
{
    public static class GeneralHelper
    {
        /// <summary>
        /// Obtiene el valor de la llave especificada en el web.config
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string ObtieneConfiguracion(string key)
        {
            string config = "";
            try
            {
                config = ConfigurationManager.AppSettings[key];
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                config = null;
            }
            return (config);
        }
        /// <summary>
        /// Compara el texto descartando caracteres no alfanuméricos, opcionalmente puede ignorar si son mayúsculas o minúsculas
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="textoComparar"></param>
        /// <param name="ignorarCase">True para ignorar si son mayúsculas o minúsculas</param>
        /// <returns></returns>
        public static bool Igual(this string actual, string textoComparar, bool ignorarCase = true)
        {
            return string.Compare(actual, textoComparar, CultureInfo.InvariantCulture, ignorarCase ? CompareOptions.IgnoreSymbols | CompareOptions.IgnoreCase : CompareOptions.IgnoreSymbols) == 0;
        }
        /// <summary>
        /// Si el texto es nulo devuelve string.Empty, sino ejecuta el metodo string.Trim()
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        public static string TrimSafe(this string texto)
        {
            return texto == null ? string.Empty : texto.Trim();
        }
        /// <summary>
        /// Si el texto es nulo devuelve string.Empty, sino transforma el texto con la primer letra de cada palabra en mayúscula
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        public static string Capitalize(this string texto)
        {
            return texto == null ? string.Empty : CultureInfo.CurrentCulture.TextInfo.ToTitleCase(texto);
        }
        /// <summary>
        /// Si el texto es nulo devuelve string.Empty, sino lo devuelve en minúsculas
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        public static string ToLowerSafe(this string texto)
        {
            return texto == null ? string.Empty : texto.ToLower();
        }
        /// <summary>
        /// Si el texto es nulo devuelve string.Empty, sino lo devuelve en mayúsculas
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        public static string ToUpperSafe(this string texto)
        {
            return texto == null ? string.Empty : texto.ToUpper();
        }
       
        /// <summary>
        /// Obtiene el valor char del enum en string
        /// </summary>
        /// <param name="enumeracion"></param>
        /// <returns></returns>
        public static string Char(this Enum enumeracion)
        {
            return Convert.ToChar(enumeracion).ToString();
        }
        /// <summary>
        /// Si ya existe la llave, actualiza el valor, si no agrega el par llave/valor
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="diccionario"></param>
        /// <param name="llave"></param>
        /// <param name="valor"></param>
        public static void Upsert<TKey, TValue>(this Dictionary<TKey, TValue> diccionario, TKey llave, TValue valor)
        {
            if (diccionario != null)
            {
                if (diccionario.ContainsKey(llave))
                {
                    diccionario[llave] = valor;
                }
                else
                {
                    diccionario.Add(llave, valor);
                }
            }
        }
        /// <summary>
        /// Trata de obtener el valor de la llave especificada, si la llave no se encuentra devuelve el valor por default
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="diccionario"></param>
        /// <param name="llave"></param>
        /// <returns></returns>
        public static TValue TryGet<TKey, TValue>(this Dictionary<TKey, TValue> diccionario, TKey llave)
        {
            if (diccionario != null && diccionario.ContainsKey(llave))
            {
                return diccionario[llave];
            }
            return default(TValue);
        }
        /// <summary>
        /// Obtiene el nombre del mes para la cultura actual
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToMonthNameCurrentCulture(this DateTime dateTime)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dateTime.Month);
        }
        /// <summary>
        /// Obtiene el nombre del mes en formato corto para la cultura actual
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToShortMonthNameCurrentCulture(this DateTime dateTime)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(dateTime.Month);
        }
        /// <summary>
        /// Devuelve true si el valor es igual a alguna de las opciones
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="valor"></param>
        /// <param name="opciones"></param>
        /// <returns></returns>
        public static bool In<T>(this T valor, params T[] opciones)
        {
            return Array.IndexOf(opciones, valor) >= 0;
        }
        /// <summary>
        /// Crea un diccionario javascript a partir de la coleccion, donde el Value es la llave y el Text es el valor
        /// </summary>
        /// <param name="coleccion"></param>
        /// <returns></returns>
        public static HtmlString CrearDiccionarioJs(IEnumerable<System.Web.Mvc.SelectListItem> coleccion)
        {
            var diccionario = new System.Collections.Specialized.ListDictionary();
            foreach (var item in coleccion)
            {
                diccionario.Add(item.Value, item.Text);
            }
            return new HtmlString(JsonNet.Serializar(diccionario));
        }
       
        /// <summary>
        /// Indica si dos colecciones tienen objetos diferentes sin importar el orden 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="comparer">Si es nulo, se utiliza el comparador por defecto</param>
        /// <returns></returns>
        public static bool Differs<T>(this IEnumerable<T> first, IEnumerable<T> second, IEqualityComparer<T> comparer = null)
        {
            return comparer != null
                ? first.Except(second, comparer).Any() || second.Except(first, comparer).Any()
                : first.Except(second).Any() || second.Except(first).Any();
        }
        /// <summary>
        /// Converte un valor string a entero de 32 bits sin lanzar excepciones. Si la conversión falla devuelve cero
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        public static int ToIntSafe(string texto)
        {
            int numero;
            int.TryParse(texto, out numero);
            return numero;
        }
        /// <summary>
        /// Valida si el objeto es nulo para devolver el valor de la función, de lo contrario devuelve el valor por defecto
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TR"></typeparam>
        /// <param name="obj"></param>
        /// <param name="getter"></param>
        /// <param name="porDefecto"></param>
        /// <returns></returns>
        public static TR IsNull<T,TR>(this T obj, Func<T,TR> getter, TR porDefecto)
        {
            return obj != null ? getter(obj) : porDefecto;
        }
        /// <summary>
        /// Valida si el objeto es nulo para devolver el valor de la función, de lo contrario devuelve el valor por defecto
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TR"></typeparam>
        /// <param name="obj"></param>
        /// <param name="getter"></param>
        /// <returns></returns>
        public static TR IsNull<T, TR>(this T obj, Func<T, TR> getter)
        {
            return obj != null ? getter(obj) : default(TR);
        }
        /// <summary>
        /// Compara dos cadenas de texto sin importar si son mayúsculas o minúsculas y descartando símbolos. Si son iguales devuelve
        /// cero, si es mayor devuelve positivo, de lo contrario devuelve negativo
        /// </summary>
        /// <param name="uno"></param>
        /// <param name="dos"></param>
        /// <returns></returns>
        public static int Comparar(this string uno, string dos)
        {
            return string.Compare(uno, dos, CultureInfo.InvariantCulture, CompareOptions.IgnoreCase | CompareOptions.IgnoreSymbols);
        }
        /// <summary>
        /// Valida si los valores no son nulos, de lo contrario asigna un valor predeterminado
        /// </summary>
        /// <param name="table"></param>
        /// <param name="valores"></param>
        public static void AddRow(this DataTable table, params object[] valores)
        {
            for (int i = 0; i < valores.Length; i++)
            {
                if (valores[i] == null)
                {
                    valores[i] = table.Columns[i].DataType == typeof(string)
                                ? string.Empty
                                : table.Columns[i].DataType == typeof(DateTime)
                                ? DateTime.MinValue
                                : (object)0;
                }
            }
            table.Rows.Add(valores);
        }
        /// <summary>
        /// Inicia un subproceso. Útil para evitar incongruencia de valores de parámetros cuando se utilizan expresiones lambda
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TR"></typeparam>
        /// <param name="factory"></param>
        /// <param name="funcion"></param>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static void StartNew<T, TR>(this TaskFactory factory, Func<T, TR> funcion, T valor)
        {
            factory.StartNew(() => funcion(valor));
        }
        /// <summary>
        /// Inicia un subproceso. Útil para evitar incongruencia de valores de parámetros cuando se utilizan expresiones lambda
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="TR"></typeparam>
        /// <param name="factory"></param>
        /// <param name="funcion"></param>
        /// <param name="valor1"></param>
        /// <param name="valor2"></param>
        public static void StartNew<T1, T2, TR>(this TaskFactory factory, Func<T1, T2, TR> funcion, T1 valor1, T2 valor2)
        {
            factory.StartNew(() => funcion(valor1, valor2));
        }
        /// <summary>
        /// Inicia un subproceso. Útil para evitar incongruencia de valores de parámetros cuando se utilizan expresiones lambda
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="TR"></typeparam>
        /// <param name="factory"></param>
        /// <param name="funcion"></param>
        /// <param name="valor1"></param>
        /// <param name="valor2"></param>
        /// <param name="valor3"></param>
        public static void StartNew<T1, T2, T3, TR>(this TaskFactory factory, Func<T1, T2, T3, TR> funcion, T1 valor1, T2 valor2, T3 valor3)
        {
            factory.StartNew(() => funcion(valor1, valor2, valor3));
        }
        /// <summary>
        /// Inicia un subproceso. Útil para evitar incongruencia de valores de parámetros cuando se utilizan expresiones lambda
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="TR"></typeparam>
        /// <param name="factory"></param>
        /// <param name="funcion"></param>
        /// <param name="valor1"></param>
        /// <param name="valor2"></param>
        /// <param name="valor3"></param>
        /// <param name="valor4"></param>
        public static void StartNew<T1, T2, T3, T4, TR>(this TaskFactory factory, Func<T1, T2, T3, T4, TR> funcion, T1 valor1, T2 valor2, T3 valor3, T4 valor4)
        {
            factory.StartNew(() => funcion(valor1, valor2, valor3, valor4));
        }
        /// <summary>
        /// Inicia un subproceso. Útil para evitar incongruencia de valores de parámetros cuando se utilizan expresiones lambda
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="TR"></typeparam>
        /// <param name="factory"></param>
        /// <param name="funcion"></param>
        /// <param name="valor1"></param>
        /// <param name="valor2"></param>
        /// <param name="valor3"></param>
        /// <param name="valor4"></param>
        /// <param name="valor5"></param>
        public static void StartNew<T1, T2, T3, T4, T5, TR>(this TaskFactory factory, Func<T1, T2, T3, T4, T5, TR> funcion, T1 valor1, T2 valor2, T3 valor3, T4 valor4, T5 valor5)
        {
            factory.StartNew(() => funcion(valor1, valor2, valor3, valor4, valor5));
        }
    }
}