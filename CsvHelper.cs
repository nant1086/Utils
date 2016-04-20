using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Linq;
using Project.Properties;
using System.Collections;

namespace Project
{
    /// <summary>
    /// Provee funcionalidad para el manejo de información en formato csv
    /// </summary>
    public static class CsvHelper
    {
        private static Tuple<T, IEnumerable<T>> HeadAndTail<T>(this IEnumerable<T> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            var en = source.GetEnumerator();
            en.MoveNext();
            return Tuple.Create(en.Current, EnumerateTail(en));
        }

        private static IEnumerable<T> EnumerateTail<T>(IEnumerator<T> en)
        {
            while (en.MoveNext()) yield return en.Current;
        }
        /// <summary>
        /// Realiza el parseo del contenido csv
        /// </summary>
        /// <param name="content"></param>
        /// <param name="delimiter"></param>
        /// <param name="qualifier"></param>
        /// <returns></returns>
        public static IEnumerable<IList<string>> Parse(string content, char delimiter, char qualifier)
        {
            using (var reader = new StringReader(content))
                return Parse(reader, delimiter, qualifier);
        }

        public static Tuple<IList<string>, IEnumerable<IList<string>>> ParseHeadAndTail(TextReader reader, char delimiter, char qualifier)
        {
            return HeadAndTail(Parse(reader, delimiter, qualifier));
        }
        /// <summary>
        /// Carga un archivo csv
        /// </summary>
        /// <param name="path"></param>
        /// <param name="delimiter"></param>
        /// <param name="qualifier"></param>
        /// <returns></returns>
        public static IEnumerable<IList<string>> Load(string path, char delimiter = ',', char qualifier = '"')
        {
            using (var reader = new StreamReader(path, Encoding.GetEncoding(Settings.Default.DefaultEncoding)))
                return Parse(reader, delimiter, qualifier);
        }

        public static DataTable LoadTable(string path, char delimiter = ',', char qualifier = '"', bool headerIncluded = true)
        {
            var tabla = new DataTable();
            using (var reader = new StreamReader(path))
            {
                foreach (var row in Parse(reader, delimiter, qualifier))
                {
                    if (headerIncluded)
                    {
                        foreach (var item in row)
                        {
                            tabla.Columns.Add(headerIncluded ? item : "C" + tabla.Columns.Count + 1);
                        }
                        headerIncluded = false;
                        continue;
                    }
                    tabla.Rows.Add(row.ToArray());
                }
                
            }
            return tabla;
        }
        /// <summary>
        /// Realiza el parseo del contenido csv
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="delimiter"></param>
        /// <param name="qualifier"></param>
        /// <returns></returns>
        public static IEnumerable<IList<string>> Parse(TextReader reader, char delimiter, char qualifier)
        {
            var inQuote = false;
            var record = new List<string>();
            var buffer = new StringBuilder();

            while (reader.Peek() != -1)
            {
                var readChar = (char)reader.Read();

                if (readChar == '\n' || (readChar == '\r' && (char)reader.Peek() == '\n'))
                {
                    // If it's a \r\n combo consume the \n part and throw it away.
                    if (readChar == '\r')
                        reader.Read();

                    if (inQuote)
                    {
                        if (readChar == '\r')
                            buffer.Append('\r');
                        buffer.Append('\n');
                    }
                    else
                    {
                        if (record.Count > 0 || buffer.Length > 0)
                        {
                            record.Add(buffer.ToString());
                            buffer.Clear();
                        }

                        if (record.Count > 0)
                            yield return record;

                        record = new List<string>(record.Count);
                    }
                }
                else if (buffer.Length == 0 && !inQuote)
                {
                    if (readChar == qualifier)
                        inQuote = true;
                    else if (readChar == delimiter)
                    {
                        record.Add(buffer.ToString());
                        buffer.Clear();
                    }
                    else if (char.IsWhiteSpace(readChar))
                    {
                        // Ignore leading whitespace
                    }
                    else
                        buffer.Append(readChar);
                }
                else if (readChar == delimiter)
                {
                    if (inQuote)
                        buffer.Append(delimiter);
                    else
                    {
                        record.Add(buffer.ToString());
                        buffer.Clear();
                    }
                }
                else if (readChar == qualifier)
                {
                    if (inQuote)
                    {
                        if ((char)reader.Peek() == qualifier)
                        {
                            reader.Read();
                            buffer.Append(qualifier);
                        }
                        else
                            inQuote = false;
                    }
                    else
                        buffer.Append(readChar);
                }
                else
                    buffer.Append(readChar);
            }

            if (record.Count > 0 || buffer.Length > 0)
                record.Add(buffer.ToString());

            if (record.Count > 0)
                yield return record;
        }

        /// <summary>
        /// Crea un archivo separado por comas compatible con Excel con la información tabular especificada
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="tabla"></param>
        public static void CrearCsv(string ruta, object[,] tabla)
        {
            using (var stream = new StreamWriter(ruta, false, Encoding.UTF8))
            {
                var columnas = tabla.GetLength(1);
                var rows = tabla.GetLength(0);
                for (int r = 0; r < rows; r++)
                {
                    for (int c = 0; c < columnas; c++)
                    {
                        stream.Write('"');
                        stream.Write(tabla[r, c]);
                        stream.Write('"');
                        if (c < columnas - 1)
                        {
                            stream.Write(',');
                        }
                    }
                    stream.WriteLine();
                }
                stream.Flush();
            }
        }
        /// <summary>
        /// Crea un archivo separado por comas, donde los valores son encerrados entre comillas
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ruta">Ruta al archivo</param>
        /// <param name="coleccion">colección de objetos</param>
        /// <param name="columnas">Nombre de columnas, de ser nulo se agregan los nombres de las propiedades</param>
        /// <param name="enumerador">Sirve para obtener las propiedades en orden o formato específico, de ser nulo las propiedades se enumeran de forma predeterminada</param>
        public static void CrearCsv<T>(string ruta, IEnumerable<T> coleccion, IEnumerable<string> columnas = null, Func<T, IEnumerable<object>> enumerador = null)
        {
            using (var stream = new StreamWriter(ruta, false, Encoding.UTF8))
            {
                var props = columnas == null || enumerador == null ? typeof(T).GetProperties() : null;
                if (columnas == null)
                {
                    columnas = props.Select(p => p.Name);
                }
                if (enumerador == null)
                {
                    enumerador = o => props.Select(p => p.GetValue(o, null));
                }
                stream.WriteLine(string.Join(",", Enumerar(columnas)));

                foreach (var obj in coleccion)
                {
                    stream.WriteLine(string.Join(",", Enumerar(enumerador(obj))));
                }
                stream.Flush();
            }
        }
        /// <summary>
        /// Crea un archivo separado por comas compatible con Excel con la información tabular especificada
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="tabla">true para eliminar rows de la tabla y ejecutar el método dispose</param>
        public static void CrearCsv(string ruta, DataTable tabla, bool dispose = true)
        {
            if (File.Exists(ruta))
            {
                File.Delete(ruta);
            }
            using (var stream = new StreamWriter(ruta, false, Encoding.UTF8))
            {
                stream.WriteLine(string.Join(",", Enumerar(tabla.Columns)));
                foreach (DataRow row in tabla.Rows)
                {
                    stream.WriteLine(string.Join(",", Enumerar(row.ItemArray)));
                }
                stream.Flush();
            }
            tabla.Clear();
            tabla.Dispose();
        }
        
        private static IEnumerable<string> Enumerar(IEnumerable coleccion)
        {
            foreach (var item in coleccion)
            {
                yield return string.Concat('"', item, '"');
            }
        }
    }
}