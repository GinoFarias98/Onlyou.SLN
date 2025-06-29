using System;
using System.IO;

namespace Onlyou.Server.Utils
{
    public static class Logger
    {
        private static readonly string _logPath = Path.Combine(AppContext.BaseDirectory, "Logs");

        public static void LogError(Exception ex)
        {
            try
            {
                string fullPath = ObtenerRutaArchivo();

                using (StreamWriter writer = new StreamWriter(fullPath, true))
                {
                    writer.WriteLine("========== ERROR ==========");
                    writer.WriteLine($"Fecha: {DateTime.Now}");
                    writer.WriteLine($"Mensaje: {ex.Message}");
                    writer.WriteLine($"Tipo de excepción: {ex.GetType().FullName}");
                    writer.WriteLine("StackTrace:");
                    writer.WriteLine(ex.StackTrace);
                    writer.WriteLine("===========================");
                    writer.WriteLine(Environment.NewLine);
                }
            }
            catch
            {
                Console.WriteLine($"⚠️ No se pudo escribir el archivo de log.");
            }
        }

        public static void LogInfo(string message)
        {
            try
            {
                string fullPath = ObtenerRutaArchivo();

                using (StreamWriter writer = new StreamWriter(fullPath, true))
                {
                    writer.WriteLine("========== INFO ==========");
                    writer.WriteLine($"Fecha: {DateTime.Now}");
                    writer.WriteLine($"Mensaje: {message}");
                    writer.WriteLine("===========================");
                    writer.WriteLine(Environment.NewLine);
                }
            }
            catch
            {
                Console.WriteLine($"⚠️ No se pudo escribir el archivo de log.");
            }
        }

        private static string ObtenerRutaArchivo()
        {
            if (!Directory.Exists(_logPath))
                Directory.CreateDirectory(_logPath);

            string fileName = $"Log_{DateTime.Now:yyyyMMdd}.txt";
            return Path.Combine(_logPath, fileName);
        }
    }
}
