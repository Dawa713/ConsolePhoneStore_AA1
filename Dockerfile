# ==================== ETAPA 1: Imagen base para ejecución ====================
FROM mcr.microsoft.com/dotnet/runtime:10.0 AS base
WORKDIR /app

# ==================== ETAPA 2: Imagen para compilación ====================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copiar archivo de proyecto y restaurar dependencias NuGet
COPY ["ConsolePhoneStore.csproj", "./"]
RUN dotnet restore "ConsolePhoneStore.csproj"

# Copiar todo el código fuente
COPY . .

# Compilar la aplicación en modo Release
RUN dotnet build "ConsolePhoneStore.csproj" -c Release -o /app/build

# ==================== ETAPA 3: Publicación ====================
FROM build AS publish
RUN dotnet publish "ConsolePhoneStore.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ==================== ETAPA 4: Imagen final ====================
FROM base AS final
WORKDIR /app

# Copiar archivos publicados desde la etapa anterior
COPY --from=publish /app/publish .

# Crear directorios necesarios para los datos
RUN mkdir -p Data

# Copiar archivos de datos iniciales (opcional, se pueden montar como volúmenes)
COPY Data/ ./Data/

# Punto de entrada de la aplicación
ENTRYPOINT ["dotnet", "ConsolePhoneStore.dll"]
