using Project.Properties;
using ClosedXML.Excel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;

namespace Project
{
    public static class ExcelHelper
    {
        /*************************************************************************************************
        
            ConectionStrings:
                Provider=Microsoft.ACE.OLEDB.12.0;Extended Properties=Excel 12.0 XML;Data Source={0};
                Provider=Microsoft.Jet.OLEDB.4.0;Extended Properties=Excel 8.0;Data Source={0};
            
        *************************************************************************************************/
        
        /// <summary>
        /// Carga en un conjunto de datos las hojas y filas de un archivo excel
        /// </summary>
        /// <param name="ruta"></param>
        /// <returns></returns>
        public static DataSet Cargar(string ruta)
        {
            var ds = new DataSet();

            using (var conn = new OleDbConnection(string.Format(Settings.Default.MicrosoftACE, ruta)))
            {
                conn.Open();
                using (var cmd = new OleDbCommand())
                {
                    cmd.Connection = conn;
                    DataTable dtSheet = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                    using (var da = new OleDbDataAdapter(cmd))
                    {
                        foreach (DataRow dr in dtSheet.Rows)
                        {
                            string sheetName = dr["TABLE_NAME"].ToString();

                            if (sheetName.EndsWith("$"))
                            {
                                cmd.CommandText = string.Concat("SELECT * FROM [", sheetName, "]");

                                var dt = new DataTable(sheetName);
                                da.Fill(dt);
                                ds.Tables.Add(dt);
                            }
                        }
                    }

                }
                conn.Close();
            }
            return ds;
        }

        private static IEnumerable<string> Enumerar(IEnumerable coleccion)
        {
            foreach (var item in coleccion)
            {
                yield return string.Concat('"', item, '"');
            }
        }
        /// <summary>
        /// Crea un archivo excel con una pestaña por cada tabla del conjunto de datos
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="dataSet"></param>
        /// <param name="dispose">true para eliminar rows del dataSet y ejecutar el método dispose</param>
        public static void CrearExcel(string ruta, DataSet dataSet, bool dispose = true)
        {
            if (File.Exists(ruta))
            {
                File.Delete(ruta);
            }
            using (var libro = new XLWorkbook())
            {
                libro.Worksheets.Add(dataSet);
                libro.SaveAs(ruta);
            }
            if (dispose)
            {
                dataSet.Clear();
                dataSet.Dispose();
            }
        }
        /// <summary>
        /// Crea un archivo excel con una pestaña por cada tabla del conjunto de datos
        /// </summary>
        /// <param name="flujo"></param>
        /// <param name="dataSet"></param>
        /// <param name="dispose">true para eliminar rows del dataSet y ejecutar el método dispose</param>
        public static void CrearExcel(Stream flujo, DataSet dataSet, bool dispose = true)
        {
            using (var libro = new XLWorkbook())
            {
                libro.Worksheets.Add(dataSet);
                libro.SaveAs(flujo);
            }
            if (dispose)
            {
                dataSet.Clear();
                dataSet.Dispose();
            }
        }
        /// <summary>
        /// Crea un archivo excel con una pestaña por cada tabla del conjunto de datos
        /// </summary>
        /// <param name="flujo"></param>
        /// <param name="tabla"></param>
        /// <param name="dispose">true para eliminar rows de la tabla y ejecutar el método dispose</param>
        public static void CrearExcel(Stream flujo, DataTable tabla, bool dispose = true)
        {
            using (var libro = new XLWorkbook())
            {
                libro.Worksheets.Add(tabla);
                libro.SaveAs(flujo);
            }
            if (dispose)
            {
                tabla.Clear();
                tabla.Dispose();
            }
        }
        /// <summary>
        /// Crea un archivo excel con la información tabular especificada
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="table"></param>
        /// <param name="dispose">true para eliminar rows del table y ejecutar el método dispose</param>
        public static void CrearExcel(string ruta, DataTable table, bool dispose = true)
        {
            if (File.Exists(ruta))
            {
                File.Delete(ruta);
            }
            using (var libro = new XLWorkbook())
            {
                libro.Worksheets.Add(table);
                libro.SaveAs(ruta);
            }
            if (dispose)
            {
                table.Clear();
                table.Dispose();
            }
        }        
    }
}