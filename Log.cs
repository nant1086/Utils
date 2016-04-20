using log4net;
using System;
using System.IO;
using System.Web;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace Project
{
    public static class Log
    {
        private static readonly string Carpetalogs = HttpContext.Current.Server.MapPath("~/App_Data/logs");
        private static readonly string ArchivoExcepcionNoControlada = Path.Combine(Carpetalogs, "ExcepcionNoControlada.log");

        static Log()
        {
            log4net.Config.XmlConfigurator.Configure();
            GlobalContext.Properties["usuario"] = new Custom();
        }
        internal class Custom
        {
            public override string ToString()
            {
                return Convert.ToString(LogicalThreadContext.Properties[MagicStr.NombreUsuario] ?? "(vacío)");
            }
        }
        /// <summary>
        /// Establece el nombre del usuario que se pretende loguear en la llamada actual
        /// </summary>
        /// <param name="usuario"></param>
        public static void EstablecerUsuario(string usuario)
        {
            LogicalThreadContext.Properties[MagicStr.NombreUsuario] = usuario;
        }
        /// <summary>
        /// Obtiene o establece si la solicitud provino del mismo equipo
        /// </summary>
        public static bool Local { get; set; }

        private static readonly ILog logger = LogManager.GetLogger(typeof(Log));

        /// <summary>
        /// Loguea solamente cuando la solicitud provino del mismo equipo
        /// </summary>
        /// <param name="objeto"></param>
        public static void Debug(object objeto)
        {
            if (Local)
            {
                Task.Factory.StartNew(() => logger.Debug(objeto), objeto);
            }
        }
        
        /// <summary>
        /// Indica mensajes informativos. 
        /// </summary>
        /// <param name="objeto"></param>
        public static void Info(object objeto)
        {
            Task.Factory.StartNew(() => logger.Info(objeto), objeto);
        }

        /// <summary>
        /// Loguea con nivel de gravedad medio
        /// </summary>
        /// <param name="objeto"></param>
        public static void Warn(object objeto)
        {
            Task.Factory.StartNew(() => logger.Warn(objeto), objeto);
        }

        /// <summary>
        /// Loguea con nivel de gravedad alto
        /// </summary>
        /// <param name="objeto"></param>
        public static void Error(object objeto)
        {
            try
            {
                Task.Factory.StartNew(() => logger.Error(objeto), objeto);
            }
            catch (Exception ex)
            {
                File.AppendAllText(ArchivoExcepcionNoControlada, string.Concat("Ocurrió una excepción no controlada", Environment.NewLine, ex));
            }
        }
        /// <summary>
        /// Loguea con nivel de gravedad alto
        /// </summary>
        /// <param name="objeto"></param>
        /// <param name="exception"></param>
        public static void Error(object objeto, Exception exception)
        {
            try
            {
                Task.Factory.StartNew(() => logger.Error(dato, exception), objeto);
            }
            catch (Exception ex)
            {
                File.AppendAllText(ArchivoExcepcionNoControlada, string.Concat("Ocurrió una excepción no controlada", Environment.NewLine, ex));
            }
        }
        /// <summary>
        /// Loguea cuando la aplicación tiene una excepción no controlada
        /// </summary>
        /// <param name="objeto"></param>
        /// <param name="ex"></param>
        public static void Fatal(object objeto, Exception exception)
        {
            try
            {
                Task.Factory.StartNew(() => logger.Fatal(dato, exception), objeto);
            }
            catch (Exception ex)
            {
                File.AppendAllText(ArchivoExcepcionNoControlada, string.Concat("Ocurrió una excepción no controlada", Environment.NewLine, ex));
            }
        }

        /// <summary>
        /// Loguea solamente cuando la solicitud provino del mismo equipo
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void DebugFormat(string format, params object[] args)
        {
            if (Local)
            {
                Task.Factory.StartNew(() => logger.DebugFormat(format, args));
            }
        }
        /// <summary>
        /// Indica mensajes informativos. 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void InfoFormat(string format, params object[] args)
        {
            Task.Factory.StartNew(() => logger.InfoFormat(format, args));
        }
        /// <summary>
        /// Loguea con nivel de gravedad medio
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void WarnFormat(string format, params object[] args)
        {
            Task.Factory.StartNew(() => logger.WarnFormat(format, args));
        }
        /// <summary>
        /// Loguea con nivel de gravedad alto
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void ErrorFormat(string format, params object[] args)
        {
            try
            {
                Task.Factory.StartNew(() => logger.ErrorFormat(format, args));
            }
            catch (Exception ex)
            {
                File.AppendAllText(ArchivoExcepcionNoControlada, string.Concat("Ocurrió una excepción no controlada", Environment.NewLine, ex));
            }
        }
        /// <summary>
        /// Loguea cuando la aplicación tiene una excepción no controlada
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void FatalFormat(string format, params object[] args)
        {
            try
            {
                Task.Factory.StartNew(() => logger.FatalFormat(format, args));
            }
            catch (Exception ex)
            {
                File.AppendAllText(ArchivoExcepcionNoControlada, string.Concat("Ocurrió una excepción no controlada", Environment.NewLine, ex));
            }
        }


    }
}