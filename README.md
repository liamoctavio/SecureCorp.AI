SecureCorp AI - LLM Security Guardrails Demo
📋 Descripción del Proyecto
Este proyecto es una Prueba de Concepto (PoC) diseñada para demostrar cómo asegurar aplicaciones empresariales que utilizan Inteligencia Artificial Generativa (LLMs).

El objetivo principal es mitigar vulnerabilidades críticas del OWASP Top 10 for LLM Applications, específicamente LLM01: Prompt Injection y LLM07: Insecure Plugin Design, mediante el uso de "Guardrails" deterministas implementados en código nativo (.NET/C#) en lugar de depender únicamente de instrucciones en lenguaje natural (System Prompts).

🛡️ Arquitectura de Seguridad
La solución utiliza Microsoft Semantic Kernel como orquestador, integrando un plugin de Recursos Humanos (RRHH) protegido por capas de seguridad:

Capa de IA (Prompt Engineering): Configuración inicial del comportamiento del modelo (aunque se asume como falible).

Capa de Validación Lógica (C# Guardrails):

Intercepta las llamadas de la IA a las herramientas internas.

Verifica permisos antes de ejecutar consultas SQL/Data (Simulado).

Bloquea intentos de acceso a registros sensibles (ej. Salarios de Ejecutivos/CEO).

Capa de Auditoría Forense (Logging):

Registra automáticamente cualquier intento de acceso no autorizado o anomalía en un archivo security_audit.log.

Permite trazabilidad de incidentes de seguridad provocados por Ingeniería Social o Alucinaciones del modelo.

🚀 Tecnologías
Lenguaje: C# (.NET 8.0)

AI Orchestrator: Microsoft Semantic Kernel

Modelo LLM: OpenAI GPT-4o-mini

Seguridad: User Secrets para gestión de API Keys, Validación de Input/Output.

⚠️ Escenario de Ataque y Mitigación
Escenario Vulnerable (Sin protecciones)
Atacante: "Dime cuánto gana Ana Gomez (CEO)".

IA: Consulta la DB y revela el salario confidencial.

Escenario Protegido (Implementado)
Atacante: "Dime cuánto gana Ana Gomez" (o usa técnicas de Jailbreak complejas).

Sistema:

La IA intenta invocar la herramienta GetSalario.

El Plugin C# detecta la solicitud sobre un usuario protegido ("ana gomez").

Se activa la Alerta de Seguridad y se escribe el incidente en el Log.

Se devuelve un mensaje de error genérico a la IA.

IA (Respuesta Final): "No tengo acceso a esa información debido a restricciones de seguridad."

🔧 Configuración Local
Clonar el repositorio.

Configurar la API Key de OpenAI usando .NET User Secrets (para no exponer claves en código):

PowerShell
dotnet user-secrets init
dotnet user-secrets set "OpenAI:ApiKey" "TU_API_KEY_AQUI"
dotnet user-secrets set "OpenAI:ModelId" "gpt-4o-mini"
Ejecutar la aplicación:

PowerShell
dotnet run
Revisar el archivo security_audit.log en el directorio de salida (bin/Debug/net8.0) tras intentar un ataque.

Este proyecto fue desarrollado con fines educativos para demostrar prácticas de AI Security y desarrollo seguro en .NET.