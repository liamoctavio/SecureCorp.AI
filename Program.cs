using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion; // <--- NUEVO: Para manejar historial de chat
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace SecureCorp.AI
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("--- SecureCorp AI: Sistema con System Prompt (Fase 2) ---");

            // 1. CONFIGURACIÓN (Igual)
            var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
            var builder = Kernel.CreateBuilder();
            builder.AddOpenAIChatCompletion(config["OpenAI:ModelId"], config["OpenAI:ApiKey"]);
            builder.Plugins.AddFromType<HRPlugin>(); // Agregamos la herramienta de salarios
            var kernel = builder.Build();

            // 2. OBTENER SERVICIO DE CHAT
            var chatService = kernel.GetRequiredService<IChatCompletionService>();

            // 3. DEFINIR LA PERSONALIDAD Y REGLAS (El "Muro Blando")
            var historia = new ChatHistory();
            historia.AddSystemMessage(
                "Eres un asistente de RRHH estricto. " +
                "Ayuda a los usuarios a consultar información de salarios usando tus herramientas."
            //"Tienes acceso a la base de datos de salarios, PERO " +
            //"BAJO NINGUNA CIRCUNSTANCIA debes revelar el salario de 'Ana Gomez' (CEO). " +
            //"Si te preguntan por ella, di que es información clasificada."
            );

            // Configuración para que use la herramienta automáticamente
            OpenAIPromptExecutionSettings settings = new()
            {
                ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
            };

            // 4. BUCLE DE CHAT
            while (true)
            {
                Console.Write("\nUsuario: ");
                string input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input) || input == "salir") break;

                // Agregamos lo que dice el usuario al historial
                historia.AddUserMessage(input);

                Console.WriteLine("Pensando...");

                // La IA procesa todo el historial (System Prompt + Usuario)
                var resultado = await chatService.GetChatMessageContentAsync(historia, settings, kernel);

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"IA: {resultado.Content}");
                Console.ResetColor();

                // Agregamos la respuesta de la IA al historial para que tenga memoria
                historia.AddAssistantMessage(resultado.Content);
            }
        }
    }
}