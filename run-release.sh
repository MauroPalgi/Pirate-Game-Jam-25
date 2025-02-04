#!/bin/bash

echo "🔄 Iniciando build de Unity antes del commit..."

# Ruta donde está instalada Unity en tu sistema (Usar "/" en rutas de Windows para Bash)
UNITY_PATH="C:/Program Files/Unity/Hub/Editor/2022.3.39f1/Editor/Unity.exe"

# Ruta del proyecto de Unity
PROJECT_PATH="C:/mauro/repos/Pirate-Game-Jam-25"

# Carpeta donde se generará el build
BUILD_PATH="$PROJECT_PATH/release"

echo "📂 Proyecto: $PROJECT_PATH"
echo "📦 Carpeta de build: $BUILD_PATH"
echo "🗑️ Eliminando archivos en $BUILD_PATH..."
rm -rf "$BUILD_PATH"/*

echo "✅ Carpeta limpia."


BUILD_SIZE=$(du -sh "$BUILD_PATH" | cut -f1)

echo "📏 Tamaño del build: $BUILD_SIZE"


# echo "$UNITY_PATH" -quit -batchmode -logFile "$PROJECT_PATH/build.log" -projectPath "$PROJECT_PATH" -executeMethod BuildScript.BuildGame --verbose

echo "🚪 Cerrando Unity si está abierto..."
taskkill //IM Unity.exe //F 2>null

echo "⏳ Esperando 5 segundos..."
sleep 5  # Espera a que Unity cierre completamente

echo "🚀 Iniciando build..."
"$UNITY_PATH" -quit -batchmode -logFile "$PROJECT_PATH/build.log" -projectPath "$PROJECT_PATH" -executeMethod BuildScript.BuildGame --verbose

# Verificar si el build se generó correctamente
if [ $? -ne 0 ]; then
    echo "❌ Error: El build falló. Revisa el log en build.log."
    exit 1  # Detiene el commit si el build falla
fi

echo "✅ Build exitoso. Puedes hacer commit."

BUILD_SIZE=$(du -sh "$BUILD_PATH" | cut -f1)

echo "📏 Tamaño del build: $BUILD_SIZE"
exit 0  # Permite el commit si el build fue exitoso
