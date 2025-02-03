using UnityEditor;
using UnityEngine;
using System.IO;
using UnityEditor.Build.Reporting;

public class BuildScript
{
    public static void BuildGame()
    {
        string buildPath = "release/";

        // Crear archivo de log
        string logFile = "build_log.txt";
        File.WriteAllText(logFile, "🛠 Iniciando Build de Unity...\n");

        // Asegurar que el directorio exista
        if (!Directory.Exists(buildPath))
        {
            File.AppendAllText(logFile, "📁 Creando directorio: " + buildPath + "\n");
            Directory.CreateDirectory(buildPath);
        }

        // Configurar las opciones del build
        BuildPlayerOptions buildOptions = new BuildPlayerOptions
        {
            scenes = new[] { "Assets/Scenes/hola.unity" },  // Escenas a incluir
            locationPathName = buildPath,  // Ruta de salida
            target = BuildTarget.WebGL,  // Plataforma
            options = BuildOptions.None
        };

        File.AppendAllText(logFile, "🚀 Iniciando compilación para WebGL...\n");

        // Ejecutar el build
        BuildReport report = BuildPipeline.BuildPlayer(buildOptions);
        BuildSummary summary = report.summary;

        // Verificar el resultado del build
        if (summary.result == BuildResult.Succeeded)
        {
            File.AppendAllText(logFile, "✅ Build exitoso!\n");
            File.AppendAllText(logFile, "📁 Creado en el  directorio: " + buildPath + "\n");
            Debug.Log("✅ Build exitoso!");
        }
        else if (summary.result == BuildResult.Failed)
        {
            File.AppendAllText(logFile, "❌ Error en el build! Revise la consola.\n");
            Debug.LogError("❌ Error en el build!");
        }
        else if (summary.result == BuildResult.Cancelled)
        {
            File.AppendAllText(logFile, "⚠️ Build cancelado!\n");
            Debug.LogWarning("⚠️ Build cancelado!");
        }
        else if (summary.result == BuildResult.Unknown)
        {
            File.AppendAllText(logFile, "❓ Resultado desconocido del build.\n");
            Debug.LogWarning("❓ Resultado desconocido del build.");
        }
    }
}
