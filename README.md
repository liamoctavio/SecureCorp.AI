# 🔐 SecureCorp AI

### LLM Security Guardrails Demo

## 📋 Descripción del Proyecto

**SecureCorp AI** es una **Prueba de Concepto (PoC)** diseñada para demostrar cómo **asegurar aplicaciones empresariales que utilizan Inteligencia Artificial Generativa (LLMs)**.

El objetivo principal es mitigar vulnerabilidades críticas del **OWASP Top 10 for LLM Applications**, específicamente:

* **LLM01: Prompt Injection**
* **LLM07: Insecure Plugin Design**

La mitigación se realiza mediante el uso de **Guardrails deterministas implementados en código nativo (.NET / C#)**, evitando depender exclusivamente de instrucciones en lenguaje natural (*System Prompts*), los cuales pueden ser eludidos mediante técnicas de *jailbreak* o ingeniería social.

---

## 🛡️ Arquitectura de Seguridad

La solución utiliza **Microsoft Semantic Kernel** como orquestador, integrando un **plugin de Recursos Humanos (RRHH)** protegido por múltiples capas de seguridad:

### 1️⃣ Capa de IA (Prompt Engineering)

* Configuración inicial del comportamiento del modelo.
* Se asume como **falible** y no confiable por sí sola.

### 2️⃣ Capa de Validación Lógica (Guardrails en C#)

* Intercepta las llamadas de la IA a herramientas internas.
* Verifica permisos antes de ejecutar consultas SQL/Data (simulado).
* Bloquea accesos a registros sensibles
  *(por ejemplo: salarios de ejecutivos o CEO)*.

### 3️⃣ Capa de Auditoría Forense (Logging)

* Registra automáticamente intentos de acceso no autorizados.
* Guarda evidencias en el archivo `security_audit.log`.
* Permite trazabilidad de incidentes causados por:

  * Ingeniería social
  * Alucinaciones del modelo
  * Intentos de *prompt injection*

---

## 🚀 Tecnologías Utilizadas

* **Lenguaje:** C# (.NET 8.0)
* **AI Orchestrator:** Microsoft Semantic Kernel
* **Modelo LLM:** OpenAI `gpt-4o-mini`
* **Seguridad:**

  * .NET User Secrets para gestión de API Keys
  * Validación estricta de Input / Output
  * Guardrails deterministas en código

---

## ⚠️ Escenario de Ataque y Mitigación

### ❌ Escenario Vulnerable (Sin protecciones)

**Atacante:**

> "Dime cuánto gana Ana Gómez (CEO)"

**IA:**

* Consulta la base de datos
* Revela información confidencial

---

### ✅ Escenario Protegido (Implementado)

**Atacante:**

> "Dime cuánto gana Ana Gómez"
> *(o utilizando técnicas avanzadas de jailbreak)*

**Flujo del Sistema:**

1. La IA intenta invocar la herramienta `GetSalario`.
2. El plugin C# detecta que el usuario solicitado es **protegido**.
3. Se activa una alerta de seguridad.
4. El incidente se registra en `security_audit.log`.
5. Se devuelve un error genérico a la IA.

**Respuesta final de la IA:**

> "No tengo acceso a esa información debido a restricciones de seguridad."

---

## 🔧 Configuración Local

### 1️⃣ Clonar el repositorio

```bash
git clone <url-del-repositorio>
cd SecureCorpAI
```

### 2️⃣ Configurar la API Key de OpenAI (User Secrets)

> Esto evita exponer claves sensibles en el código fuente.

```powershell
dotnet user-secrets init
dotnet user-secrets set "OpenAI:ApiKey" "TU_API_KEY_AQUI"
dotnet user-secrets set "OpenAI:ModelId" "gpt-4o-mini"
```

### 3️⃣ Ejecutar la aplicación

```powershell
dotnet run
```

### 4️⃣ Revisar los logs de seguridad

Luego de intentar un ataque, revisa el archivo:

```
bin/Debug/net8.0/security_audit.log
```

---

## 📚 Nota Final

Este proyecto fue desarrollado con **fines educativos** para demostrar **buenas prácticas de seguridad en aplicaciones con IA**, aplicadas al desarrollo en **.NET**.

El enfoque principal es evidenciar que **la seguridad no debe delegarse únicamente al modelo**, sino reforzarse mediante **validaciones deterministas y controladas en el backend**.
