using System;
using System.IO;

namespace Onlyou.Server.Helpers
{
    public static class Logger
    {
        private static readonly string _logPath = Path.Combine(AppContext.BaseDirectory, "Logs");


        public static void LogError(Exception ex)
        {
            try
            {
                if (!Directory.Exists(_logPath))
                { Directory.CreateDirectory(_logPath); }

                string fileName = $"Log_{DateTime.Now:yyyyMMdd}.txt";
                string fullPath = Path.Combine(_logPath, fileName);

                using (StreamWriter writer = new StreamWriter(fullPath, true)) 
                {
                    writer.WriteLine($"[{DateTime.Now}] ERROR: {ex.Message}");
                    writer.WriteLine(ex.StackTrace);
                    writer.WriteLine("--------------------------------------------------");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


    }
}
