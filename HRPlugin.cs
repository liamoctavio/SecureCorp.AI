using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace SecureCorp.AI
{
    public class HRPlugin
    {
        // Simulamos una base de datos pequeña
        private readonly Dictionary<string, int> _salarios = new()
        {
            { "Juan Perez", 50000 },
            { "Ana Gomez", 120000 }, // CEO
            { "Carlos Ruiz", 45000 }
        };

        // [KernelFunction] le dice a Semantic Kernel: "Oye, la IA puede usar esto"
        // [Description] le explica a la IA PARA QUÉ sirve esta función.
        [KernelFunction]
        [Description("Obtiene el salario anual de un empleado dado su nombre.")]
        public string GetSalario([Description("Nombre del empleado")] string nombre)
        {
            // --- CAPA DE SEGURIDAD (GUARDRAILS) ---

            // 1. Normalización (Evitar trucos como "ana gomez " o "Ana GOMEZ")
            string usuarioNormalizado = nombre.Trim().ToLowerInvariant();

            // 2. Lista Negra (Hardcoded por ahora, idealmente vendría de una DB de permisos)
            if (usuarioNormalizado.Contains("ana gomez") || usuarioNormalizado.Contains("ceo"))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[ALERTA DE SEGURIDAD]: Intento de acceso no autorizado al registro de: {nombre}");
                Console.ResetColor();

                SecurityLogger.LogIncident(nombre, "Acceso a Salario Ejecutivo", "CRITICAL");

                // Devolvemos un error falso o genérico a la IA
                return "ERROR: Acceso restringido. Este incidente ha sido reportado a seguridad.";
            }
            // -------------------------------------

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[SISTEMA INTERNO]: Buscando salario de '{nombre}'...");
            Console.ResetColor();

            if (_salarios.TryGetValue(nombre, out int salario))
            {
                return $"El salario de {nombre} es ${salario} USD anuales.";
            }

            return $"No se encontró al empleado {nombre}.";
        }
    }
}