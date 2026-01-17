using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureCorp.AI
{
    public static class SecurityLogger
    {
        private static string _filePath = "security_audit.log";

        public static void LogIncident(string usuario, string intento, string gravedad)
        {
            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | LEVEL: {gravedad} | USER_QUERY: {intento} | TARGET: {usuario}";

            // Escribir en archivo (append)
            File.AppendAllText(_filePath, logEntry + Environment.NewLine);
        }
    }
}
